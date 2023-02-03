using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static Action<int> OnPlayerLivesChanged;
    public static Action OnTouchUIEnabled;
    public static Action OnTouchUIDisabled;

    public static GameManager Instance
    {
        get;
        private set;
    }

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
    [Range(0f, 100f)]
    private float initialDiseaseLevel = 50;
    [SerializeField]
    private float diseaseLevelRiseSpeed = 1;
    [SerializeField]
    private string seed = "Genoma Games";

    private Transform playerSpawn;
    private int playerLives;
    private bool isInGameplayScene = false;
    private float timeUntilPlayerSpawns;
    private bool isPlayerSpawning = false;

    public void EnableTouchUI()
    {
        IsTouchUIEnabled = true;

        OnTouchUIEnabled?.Invoke();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Level_001");
    }

    public void OnTotalEnemiesChanged(int totalEnemies)
    {
        if (totalEnemies <= 0)
        {
            if (DiseaseManager.Instance.DiseaseLevel < 50)
            {
                Win();
            }
            else
            {
                Lose();
            }
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;

            Random.InitState(SeedHash);
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

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;

        if (DiseaseManager.Instance != null)
        {
            DiseaseManager.Instance.OnLevelEmptied -= Win;
            DiseaseManager.Instance.OnLevelFilled -= Lose;
        }

        EnhancedTouchSupport.Disable();
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
        SceneManager.LoadScene("Game Over");
    }

    private void Win()
    {
        SceneManager.LoadScene("You Win");
    }

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        isInGameplayScene = newScene.name.StartsWith("Level");

        if (isInGameplayScene)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        if (DiseaseManager.Instance != null)
        {
            DiseaseManager.Instance.OnLevelEmptied += Win;
            DiseaseManager.Instance.OnLevelFilled += Lose;
        }

        playerLives = initialPlayerLives;

        GameObject playerSpawnGO = GameObject.FindGameObjectWithTag("Player Spawn");

        if (playerSpawnGO == null)
        {
            throw new UnityException("No player spawn found in the scene");
        }

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

    private void OnPlayerDie(Player player)
    {
        player.OnDie -= OnPlayerDie;

        if (playerLives > 0)
        {
            playerLives--;

            OnPlayerLivesChanged?.Invoke(playerLives);

            timeUntilPlayerSpawns = playerSpawnCooldown;
            isPlayerSpawning = true;
        }
        else
        {
            Lose();
        }
    }
}
