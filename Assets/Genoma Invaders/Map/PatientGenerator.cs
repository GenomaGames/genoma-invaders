using Sirenix.OdinInspector;
using UnityEngine;

public class PatientGenerator : MonoBehaviour
{
    [SerializeField]
    [Required]
    private PatientGeneratorConfig config;

    private System.Random random;

    private void Awake()
    {
        random = new System.Random(GameManager.Instance.SeedHash);
    }

    //public Patient Generate()
    //{
    //    bool isMale = random.Next(2) == 0;

    //    Patient patient = new Patient
    //    {
    //        name = isMale ? config.maleNames[random.Next(config.maleNames.Length)] : config.femaleNames[random.Next(config.femaleNames.Length)],
    //        disease = 
    //    }
    //}
}
