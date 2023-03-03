#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

public struct Patient
{
    public string name;
    public DiseaseConfig disease;
}

public class GameManager : MonoBehaviour
{
    public static Action<int> OnPlayerLivesChanged;
    public static Action OnTouchUIEnabled;
    public static Action OnTouchUIDisabled;
    public static Action OnGamePaused;
    public static Action OnGameUnpaused;

    public BodyPartConfig CurrentBodyPart
    {
        get => currentBodyPart;
        private set => currentBodyPart = value;
    }

    public DosageFormConfig[] DosageForms
    {
        get => dosageForms;
    }

    public static GameManager Instance
    {
        get;
        private set;
    }

    public bool IsGamePaused
    {
        get;
        private set;
    } = false;

    public bool IsTouchUIEnabled
    {
        get;
        private set;
    } = false;

    public Patient[] Patients
    {
        get => patients;
    }

    public Patient CurrentPatient
    {
        get;
        private set;
    }

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
    private BodyPartConfig currentBodyPart;
    [SerializeField]
    private DosageFormConfig[] dosageForms;
    [SerializeField]
    private DiseaseConfig[] diseases;

    private Transform playerSpawn;
    private int playerLives;
    private bool isInGameplayScene = false;
    private float timeUntilPlayerSpawns;
    private bool isPlayerSpawning = false;
    private Patient[] patients;
    private System.Random random;

    private readonly string[] patientNames =
    {
        "John Doe",
        "Joe Bloggs",
        "Mister X",
        "Joe Schmoe",
        "Jane Smith",
        "Hans Meier",
    };

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
        SceneManager.LoadScene("Patient Selection");
    }

    public void OnTotalEnemiesChanged(int totalEnemies)
    {
        if (totalEnemies <= 0)
        {
            FinishLevel();
        }
    }

    public void Pause()
    {
        if (isInGameplayScene)
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

    public void SelectBodyPart(BodyPartConfig bodyPart)
    {
        Debug.Log($"Dosage form {bodyPart.partName} selected");
        currentBodyPart = bodyPart;

        SceneLoader.LoadScene(bodyPart.bodySystem.sceneName);
    }

    public void SelectDosageForm(DosageFormConfig dosageForm)
    {
        Debug.Log($"Dosage form {dosageForm.dosageFormName} selected");

        BodyPartConfig bodyPart = dosageForm.bodyParts[random.Next(0, dosageForm.bodyParts.Length)];

        SelectBodyPart(bodyPart);
    }

    public void SelectPatient(Patient patient)
    {
        CurrentPatient = patient;

        Debug.Log($"Patient {patient.name} selected");
        SceneLoader.LoadScene("Administration Selection");
    }

    public void Unpause()
    {
        if (isInGameplayScene)
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

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;

            Random.InitState(SeedHash);
            random = new System.Random(SeedHash);

            patients = new Patient[3];

            if (Touchscreen.current != null && Gamepad.current == null)
            {
                EnableTouchUI();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isInGameplayScene)
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

        if (SceneManager.GetActiveScene().name == "Patient Selection")
        {
            if (GUILayout.Button("Regenerate Patients"))
            {
                GeneratePatients();
            }
        }
    }

    private void EndGame()
    {
        if (DiseaseManager.Instance != null)
        {
            DiseaseManager.Instance.OnLevelEmptied -= Win;
            DiseaseManager.Instance.OnLevelFilled -= Lose;
        }
    }

    private void FinishLevel()
    {
        Debug.Log($"Level {SceneManager.GetActiveScene().name} Finished");

        SceneManager.LoadScene("Body Part Selection");
    }

    private void GeneratePatients()
    {
        for (int i = 0; i < patients.Length; i++)
        {
            Patient patient = GeneratePatient();

            patients[i] = patient;
        }
    }

    private Patient GeneratePatient()
    {
        Patient patient = new()
        {
            name = patientNames[random.Next(patientNames.Length)],
            disease = diseases[random.Next(diseases.Length)],
        };

        return patient;
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

        isInGameplayScene = newScene.name.StartsWith("Level");

        if (isInGameplayScene)
        {
            StartGame();
        }
        else if (newScene.name == "Patient Selection")
        {
            GeneratePatients();
        }
    }

    private void StartGame()
    {
        DiseaseManager diseaseManager = DiseaseManager.Instance;

        if (diseaseManager != null)
        {
            diseaseManager.ResetLevel();
            diseaseManager.SetDisease(CurrentPatient.disease);

            diseaseManager.OnLevelEmptied += Win;
            diseaseManager.OnLevelFilled += Lose;
        }

        playerLives = initialPlayerLives;

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
