using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Disease", menuName = "Disease")]
public class DiseaseConfig : SerializedScriptableObject
{
    public string diseaseName;
    public float diseaseLevelRiseSpeed = 1;
}
