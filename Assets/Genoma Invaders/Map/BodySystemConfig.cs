using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class BodySystemConfig : SerializedScriptableObject
{
    public string systemName;
    public Sprite sprite;
    public Color color = Color.white;
    public BodyPartConfig[] parts;
    [Required]
    [AssetSelector(Filter = "t:scene")]
    [DisplayAsString]
    public Object scene;
}
