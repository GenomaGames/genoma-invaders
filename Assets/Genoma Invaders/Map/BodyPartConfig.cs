using Sirenix.OdinInspector;
using UnityEngine;

public enum ConnectionDirection
{
    Up,
    Right,
    Down,
    Left,
}

[CreateAssetMenu()]
public class BodyPartConfig : SerializedScriptableObject
{
    public string partName;
    public BodySystemConfig bodySystem;
    public Vector2 position;
    public BodyPartConfig[] connectedParts;
}
