using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiseaseManager : MonoBehaviour
{
    public event Action OnLevelFilled;
    public event Action OnLevelEmptied;
    public event Action<float> OnLevelUpdated;

    public static DiseaseManager Instance
    {
        get;
        private set;
    }

    public float DiseaseLevel
    {
        get;
        private set;
    } = 0;

    [SerializeField]
    [Range(0f, 100f)]
    private float initialDiseaseLevel = 50;

    private DiseaseConfig disease;

    public void ResetLevel()
    {
        SetDiseaseLevel(initialDiseaseLevel);
    }

    public void SetDisease(DiseaseConfig diseaseConfig)
    {
        disease = diseaseConfig;
    }

    public void UpdateDiseaseLevel(float diseaseLevelChange)
    {
        DiseaseLevel = Mathf.Clamp(DiseaseLevel + diseaseLevelChange, 0, 100);

        //Debug.Log($"Disease Level Updated to {DiseaseLevel}");
        OnLevelUpdated?.Invoke(DiseaseLevel);

        if (DiseaseLevel == 100)
        {
            OnLevelFilled?.Invoke();
        }
        else if (DiseaseLevel == 0)
        {
            OnLevelEmptied?.Invoke();
        }
    }
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;

            ResetLevel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float diseaseLevelChange = Time.deltaTime * disease.levelRiseSpeed;

        UpdateDiseaseLevel(diseaseLevelChange);
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
    }

    private void SetDiseaseLevel(float newLevel)
    {
        DiseaseLevel = Mathf.Clamp(newLevel, 0, 100);

        //Debug.Log($"Disease Level Updated to {DiseaseLevel}");
        OnLevelUpdated?.Invoke(DiseaseLevel);

        if (DiseaseLevel == 100)
        {
            OnLevelFilled?.Invoke();
        }
        else if (DiseaseLevel == 0)
        {
            OnLevelEmptied?.Invoke();
        }
    }
}
