using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Patient
{
    public string name;
    public DiseaseConfig disease;
}

public class DiseaseManager : SingletonMonoBehaviour<DiseaseManager>
{
    public static event Action OnLevelFilled;
    public static event Action OnLevelEmptied;
    public static event Action<float> OnLevelUpdated;

    public BodyPartConfig BossLocation
    {
        get;
        private set;
    }

    public DiseaseConfig CurrentDisease
    {
        get;
        private set;
    }

    public Patient CurrentPatient
    {
        get;
        private set;
    }

    public float DiseaseLevel
    {
        get;
        private set;
    } = 0;

    public Patient[] Patients
    {
        get => patients;
    }
    private Patient[] patients;

    [SerializeField]
    private DiseaseConfig[] diseases;
    [SerializeField]
    [Range(0f, 100f)]
    private float initialDiseaseLevel = 50;

    private readonly string[] patientNames =
    {
        "John Doe",
        "Joe Bloggs",
        "Mister X",
        "Joe Schmoe",
        "Jane Smith",
        "Hans Meier",
    };

    private System.Random random;

    public Patient[] GeneratePatients()
    {
        for (int i = 0; i < patients.Length; i++)
        {
            Patient patient = GeneratePatient();

            patients[i] = patient;
        }

        return patients;
    }

    public void SelectPatient(Patient patient)
    {
        CurrentPatient = patient;

        CurrentDisease = CurrentPatient.disease;

        Debug.Log($"Patient {patient.name} selected, with the disease {CurrentDisease.diseaseName}");

        BossLocation = ChooseBossLocation(CurrentDisease);

        Debug.Log($"Boss located at {BossLocation.partName}");

        SceneLoader.LoadScene("Administration Selection");
    }

    public void SelectPatient(int patientIndex)
    {
        Patient patient = patients[patientIndex];

        SelectPatient(patient);
    }

    public void ResetLevel()
    {
        SetDiseaseLevel(initialDiseaseLevel);
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

    private new void Awake()
    {
        base.Awake();

        if (Instance != this)
        {
            return;
        }

        patients = new Patient[3];
    }

    private void Start()
    {
        random = new System.Random(GameManager.Instance.SeedHash);
        ResetLevel();
    }

    private void Update()
    {
        if (GameManager.Instance.IsInGameplayScene && CurrentDisease != null)
        {
            float diseaseLevelChange = Time.deltaTime * CurrentDisease.levelRiseSpeed;

            UpdateDiseaseLevel(diseaseLevelChange);
        }
    }

    private void OnGUI()
    {
        if (SceneManager.GetActiveScene().name == "Patient Selection")
        {
            if (GUILayout.Button("Regenerate Patients"))
            {
                GeneratePatients();
            }
        }
    }

    private BodyPartConfig ChooseBossLocation(DiseaseConfig disease)
    {
        BossLocation = disease.bossBodyParts[random.Next(disease.bossBodyParts.Length)];

        return BossLocation;
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
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
