using Sirenix.OdinInspector;
using System.Collections.Generic;
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
    [Required]
    public Dictionary<ConnectionDirection, BodyPartConfig> connectedParts = new();
    [Required]
    public Dictionary<BodySystem, BodyPartConfig> connectedSystems = new();
}
