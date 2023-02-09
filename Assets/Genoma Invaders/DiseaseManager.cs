using System;
using UnityEngine;

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
    [SerializeField]
    [Range(0f, 20f)]
    private float diseaseLevelRiseSpeed = 1;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new UnityException("There is more than one Disease Manager");
        }

        UpdateDiseaseLevel(initialDiseaseLevel);
    }

    private void Update()
    {
        float diseaseLevelChange = Time.deltaTime * diseaseLevelRiseSpeed;

        UpdateDiseaseLevel(diseaseLevelChange);
    }
}
