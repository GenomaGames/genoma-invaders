using System;
using UnityEngine;

public class NavigationSystem : SingletonMonoBehaviour<NavigationSystem>
{
    public event Action<BodyPartConfig, BodyPartConfig> OnBodyPartChanged;

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

        BodyPartConfig oldBodyPart = currentBodyPart;

        currentBodyPart = bodyPart;

        OnBodyPartChanged.Invoke(oldBodyPart, currentBodyPart);
    }
}
