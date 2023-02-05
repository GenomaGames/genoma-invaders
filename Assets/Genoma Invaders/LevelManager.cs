using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public BodyPart CurrentBodyPart
    {
        get => currentBodyPart;
    }

    public BodySystem CurrentBodySystem
    {
        get => currentBodySystem;
    }

    [SerializeField]
    private BodyPart currentBodyPart;
    [SerializeField]
    private BodySystem currentBodySystem;

    public void LoadLevel()
    {
        
    }
}
