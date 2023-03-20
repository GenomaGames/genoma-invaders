using UnityEngine;
using TMPro;

public class UIPatientSelector : MonoBehaviour
{
    [SerializeField]
    private TMP_Text patientNameText;

    private int currentPatientIndex = 0;

    public void NextPatient()
    {
        currentPatientIndex++;

        if (currentPatientIndex >= DiseaseManager.Instance.Patients.Length)
        {
            currentPatientIndex = 0;
        }

        UpdateInfo();
    }

    public void PreviousPatient()
    {
        currentPatientIndex--;

        if (currentPatientIndex < 0)
        {
            currentPatientIndex = DiseaseManager.Instance.Patients.Length - 1;
        }

        UpdateInfo();
    }

    public void SelectPatient()
    {
        DiseaseManager.Instance.SelectPatient(currentPatientIndex);
    }

    private void Start()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        patientNameText.text = DiseaseManager.Instance.Patients[currentPatientIndex].name;
    }
}
