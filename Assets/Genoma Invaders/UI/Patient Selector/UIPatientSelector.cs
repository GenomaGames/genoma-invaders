using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UIPatientSelector : MonoBehaviour
{
    [SerializeField]
    private TMP_Text patientNameText;

    private int currentPatientIndex = 0;

    public void NextPatient()
    {
        currentPatientIndex++;

        if (currentPatientIndex >= GameManager.Instance.Patients.Length)
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
            currentPatientIndex = GameManager.Instance.Patients.Length - 1;
        }

        UpdateInfo();
    }

    public void SelectPatient()
    {
        GameManager.Instance.SelectPatient(GameManager.Instance.Patients[currentPatientIndex]);
    }

    private void Start()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        patientNameText.text = GameManager.Instance.Patients[currentPatientIndex].name;
    }
}
