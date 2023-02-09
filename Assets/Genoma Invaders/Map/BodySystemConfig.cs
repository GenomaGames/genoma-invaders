using UnityEngine;

[CreateAssetMenu()]
public class BodySystemConfig : ScriptableObject
{
    public string systemName;
    public BodyPartConfig[] parts;
}
