using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Disease", menuName = "Disease")]
public class DiseaseConfig : SerializedScriptableObject
{
    public string diseaseName;
    [Range(0f, 20f)]
    public float levelRiseSpeed = 1;
    public BodyPartConfig[] bossBodyParts;
}
