using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu()]
public class DosageFormConfig : SerializedScriptableObject
{
    public string dosageFormName;
    public BodySystem bodySystem;
    public BodyPartConfig[] bodyParts;
}
