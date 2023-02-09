using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class BodySystemConfig : ScriptableObject
{
    public string systemName;
    public Sprite sprite;
    public Color color = Color.white;
    public BodyPartConfig[] parts;
    [Required]
    public SceneAsset scene;
}
