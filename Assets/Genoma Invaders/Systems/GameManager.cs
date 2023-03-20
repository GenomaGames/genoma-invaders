#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public static Action<int> OnPlayerLivesChanged;
    public static Action OnTouchUIEnabled;
    public static Action OnTouchUIDisabled;
    public static Action OnGamePaused;
    public static Action OnGameUnpaused;
    public static Action OnGameplayStarted;

    public DosageFormConfig[] DosageForms
    {
        get => dosageForms;
    }

    public bool IsGamePaused
    {
        get;
        private set;
    } = false;
    public bool IsInGameplayScene
    {
        get;
        private set;
    } = false;

    public bool IsTouchUIEnabled
    {
        get;
        private set;
    } = false;

    public string Seed
    {
        get => seed;
    }

    public int SeedHash
    {
        get => seed.GetHashCode();
    }

    [SerializeField]
    private int initialPlayerLives = 3;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private float playerSpawnCooldown = 2;
    [SerializeField]
    private string seed = "Genoma Games";
    [SerializeField]
    private DosageFormConfig[] dosageForms;

    private Transform playerSpawn;
    private int playerLives;
    private float timeUntilPlayerSpawns;
    private bool isPlayerSpawning = false;
    private System.Random random;

    public void EnableTouchUI()
    {
        IsTouchUIEnabled = true;

        OnTouchUIEnabled?.Invoke();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }


    public void GoToMap()
    {
        SceneManager.LoadScene("Map");
    }

    public void NewGame()
    {
        DiseaseManager.Instance.GeneratePatients();
        SceneManager.LoadScene("Patient Selection");
    }

    public void OnTotalEnemiesChanged(int totalEnemies)
    {
        if (totalEnemies <= 0)
        {
            FinishLevel();
        }
    }

    public void PauseGame()
    {
        if (IsInGameplayScene)
        {
            if (!IsGamePaused)
            {
                IsGamePaused = true;
                OnGamePaused?.Invoke();
            }
            else
            {
                throw new UnityException("Game is already paused");
            }
        }
        else
        {
            throw new UnityException("Can not pause in non gameplay scenes");
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level.Stomach");
    }

    public void SelectDosageForm(DosageFormConfig dosageForm)
    {
        Debug.Log($"Dosage form {dosageForm.dosageFormName} selected");

        BodyPartConfig bodyPart = dosageForm.bodyParts[random.Next(0, dosageForm.bodyParts.Length)];

        StartGame();

        NavigationSystem.Instance.GoToBodyPart(bodyPart);
    }

    public void Unpause()
    {
        if (IsInGameplayScene)
        {
            if (IsGamePaused)
            {
                IsGamePaused = false;
                OnGameUnpaused?.Invoke();
            }
            else
            {
                throw new UnityException("Game is already playing");
            }
        }
        else
        {
            throw new UnityException("Can not unpause in non gameplay scenes");
        }
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        if (DiseaseManager.Instance != null)
        {
            DiseaseManager.Instance.OnLevelEmptied += Win;
            DiseaseManager.Instance.OnLevelFilled += Lose;
        }
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;

        if (DiseaseManager.Instance != null)
        {
            DiseaseManager.Instance.OnLevelEmptied -= Win;
            DiseaseManager.Instance.OnLevelFilled -= Lose;
        }
    }

    private new void Awake()
    {
        base.Awake();

        if (Instance != this)
        {
            return;
        }

        Random.InitState(SeedHash);
        random = new System.Random(SeedHash);

        if (Touchscreen.current != null && Gamepad.current == null)
        {
            EnableTouchUI();
        }
    }

    private void Update()
    {
        if (IsInGameplayScene)
        {
            if (isPlayerSpawning)
            {
                timeUntilPlayerSpawns = Mathf.Clamp(timeUntilPlayerSpawns - Time.deltaTime, 0, playerSpawnCooldown);

                if (timeUntilPlayerSpawns <= 0)
                {
                    SpawnPlayer();
                }
            }
        }
    }

    private void OnGUI()
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Level"))
        {
            if (GUILayout.Button("Finish Level"))
            {
                FinishLevel();
            }
        }
    }

    private void EndGame()
    {
    }

    private void FinishLevel()
    {
        Debug.Log($"Level {SceneManager.GetActiveScene().name} Finished");

        SceneManager.LoadScene("Body Part Selection");
    }

    private void SpawnPlayer()
    {
        GameObject playerGameObject = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);

        Player player = playerGameObject.GetComponent<Player>();

        player.OnDie += OnPlayerDie;

        isPlayerSpawning = false;
    }

    private void Lose()
    {
        Debug.Log("LOSE");
        EndGame();
        SceneManager.LoadScene("Game Over");
    }

    private void Win()
    {
        Debug.Log("WIN");
        EndGame();
        SceneManager.LoadScene("You Win");
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        IsGamePaused = false;

        IsInGameplayScene = newScene.name.StartsWith("Level");

        if (IsInGameplayScene)
        {
            GameObject playerSpawnGO = GameObject.FindGameObjectWithTag("Player Spawn");

            if (playerSpawnGO != null)
            {
                playerSpawn = playerSpawnGO.transform;

                GameObject playerGO = GameObject.FindGameObjectWithTag("Player");

                if (playerGO == null)
                {
                    SpawnPlayer();
                }
                else
                {
                    Player player = playerGO.GetComponent<Player>();

                    player.OnDie += OnPlayerDie;
                }
            }
            else
            {
                throw new UnityException("No player spawn found in the scene");
            }
        }
    }

    private void StartGame()
    {
        if (DiseaseManager.Instance != null)
        {
            DiseaseManager.Instance.ResetLevel();
            DiseaseManager.Instance.SetDisease(DiseaseManager.Instance.CurrentPatient.disease);
        }

        playerLives = initialPlayerLives;
    }

    private void OnPlayerDie(Player player)
    {
        player.OnDie -= OnPlayerDie;

        if (playerLives > 0)
        {
            playerLives--;

            OnPlayerLivesChanged?.Invoke(playerLives);

            timeUntilPlayerSpawns = playerSpawnCooldown;
            isPlayerSpawning = true;

            AnalyticsManager.SendPlayerKilled();
        }
        else
        {
            Lose();
        }
    }
}
