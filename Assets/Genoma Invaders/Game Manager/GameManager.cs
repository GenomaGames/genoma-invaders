using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

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

    public float DiseaseLevel
    {
        get;
        private set;
    } = 0;

    public bool IsTouchUIEnabled
    {
        get;
        private set;
    } = false;

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

    private Transform playerSpawn;
    private int playerLives;
    private bool isInGameScene = false;
    private float timeUntilPlayerSpawns;
    private bool isPlayerSpawning = false;

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void UpdateDiseaseLevel(float diseaseLevelChange)
    {
        DiseaseLevel = Mathf.Clamp(DiseaseLevel + diseaseLevelChange, 0, 100);

        //Debug.Log($"Disease Level Updated to {DiseaseLevel}");

        if (DiseaseLevel == 100)
        {
            Lose();
        }
        else if (DiseaseLevel == 0)
        {
            Win();
        }
    }

    public void OnTotalEnemiesChanged(int totalEnemies)
    {
        if (totalEnemies <= 0)
        {
            if (DiseaseLevel < 50)
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
        EnhancedTouchSupport.Enable();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Touch.activeTouches.Count > 0 && !IsTouchUIEnabled)
        {
            EnableTouchUI();
        }

        if (isInGameScene)
        {
            float diseaseLevelChange = Time.deltaTime * diseaseLevelRiseSpeed;

            UpdateDiseaseLevel(diseaseLevelChange);

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
        EnhancedTouchSupport.Disable();
    }

    private void EnableTouchUI()
    {
        IsTouchUIEnabled = true;

        if (OnTouchUIEnabled != null)
        {
            OnTouchUIEnabled();
        }
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
        isInGameScene = newScene.name == "Game";

        if (isInGameScene)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        playerLives = initialPlayerLives;
        DiseaseLevel = initialDiseaseLevel;
        playerSpawn = GameObject.FindGameObjectWithTag("Player Spawn").transform;

        SpawnPlayer();
    }

    private void OnPlayerDie(Player player)
    {
        player.OnDie -= OnPlayerDie;

        if (playerLives > 0)
        {
            playerLives--;

            if (OnPlayerLivesChanged != null)
            {
                OnPlayerLivesChanged(playerLives);
            }

            timeUntilPlayerSpawns = playerSpawnCooldown;
            isPlayerSpawning = true;
        }
        else
        {
            Lose();
        }
    }
}
