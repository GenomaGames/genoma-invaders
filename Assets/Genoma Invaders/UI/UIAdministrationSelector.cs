using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIAdministrationSelector : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dosageFormNameText;

    private DosageForm[] dosageForms;
    private int currentDosageFormIndex = 0;

    public void NextDosageForm()
    {
        currentDosageFormIndex++;

        if (currentDosageFormIndex >= dosageForms.Length)
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
            currentDosageFormIndex = dosageForms.Length - 1;
        }

        UpdateInfo();
    }

    public void SelectDosageForm()
    {
        GameManager.Instance.SelectDosageForm(dosageForms[currentDosageFormIndex]);
    }

    private void Awake()
    {
        dosageForms = Enum.GetValues(typeof(DosageForm))
            .Cast<DosageForm>()
            .Where(dosageForm => dosageForm != DosageForm.None)
            .ToArray();
    }

    private void Start()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        dosageFormNameText.text = dosageForms[currentDosageFormIndex].ToString();
    }
}
