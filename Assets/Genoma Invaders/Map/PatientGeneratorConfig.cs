using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu()]
public class PatientGeneratorConfig : SerializedScriptableObject
{
    public string[] maleNames = new string[0];
    public Sprite[] malePictures = new Sprite[0];
    public string[] femaleNames = new string[0];
    public Sprite[] femalePictures = new Sprite[0];
    public string[] surnames = new string[0];
}
