using UnityEngine;

public class NavigationSystem : SingletonMonoBehaviour<NavigationSystem>
{
    public BodyPartConfig CurrentBodyPart
    {
        get => currentBodyPart;
        private set => currentBodyPart = value;
    }

    [SerializeField]
    private BodyPartConfig currentBodyPart;

    public BodyPartConfig[] GetConnectedBodyParts()
    {
        BodyPartConfig[] connectedBodyParts = currentBodyPart.connectedParts;

        return connectedBodyParts;
    }

    public void GoToBodyPart(BodyPartConfig bodyPart)
    {
        Debug.Log($"Body part {bodyPart.partName} selected");
        currentBodyPart = bodyPart;

        SceneLoader.LoadScene(bodyPart.bodySystem.sceneName);
    }
}
