using TMPro;
using UnityEngine;

public class UIAdministrationSelector : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dosageFormNameText;

    private int currentDosageFormIndex = 0;

    public void NextDosageForm()
    {
        currentDosageFormIndex++;

        if (currentDosageFormIndex >= GameManager.Instance.DosageForms.Length)
        {
            currentDosageFormIndex = 0;
        }

        UpdateInfo();
    }

    public void PreviousDosageForm()
    {
        currentDosageFormIndex--;

        if (currentDosageFormIndex < 0)
        {
            currentDosageFormIndex = GameManager.Instance.DosageForms.Length - 1;
        }

        UpdateInfo();
    }

    public void SelectDosageForm()
    {
        GameManager.Instance.SelectDosageForm(GameManager.Instance.DosageForms[currentDosageFormIndex]);
    }

    private void Start()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        dosageFormNameText.text = GameManager.Instance.DosageForms[currentDosageFormIndex].dosageFormName;
    }
}
