using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBodySystem : MonoBehaviour
{
    [SerializeField]
    private BodySystemConfig bodySystem;
    [SerializeField]
    [AssetsOnly]
    private GameObject bodyPartPrefab;

    private Image image;
    private RectTransform bodyPartsParent;
    private RectTransform rectTransform;

    [ContextMenu("Setup")]
    public void Setup()
    {
        UpdateImage();
        SetupBodyParts();
    }

    public void Setup(BodySystemConfig bodySystemConfig)
    {
        bodySystem = bodySystemConfig;

        UpdateImage();
        SetupBodyParts();
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    private GameObject InstantiateBodyPart(BodyPartConfig config)
    {
        GameObject partGo = Instantiate(bodyPartPrefab, bodyPartsParent);

        partGo.name = config.partName;
        partGo.transform.localPosition = config.position;

        return partGo;
    }

    private RectTransform InstantiateBodyPartsParent()
    {
        GameObject bodyPartsParentGO = new("Body Parts", typeof(RectTransform));

        RectTransform bodyPartsParent = bodyPartsParentGO.GetComponent<RectTransform>();

        bodyPartsParent.SetParent(rectTransform, false);
        bodyPartsParent.sizeDelta = rectTransform.sizeDelta;

        return bodyPartsParent;
    }

    //private void OnClickBodyPart(BodyPartConfig bodyPart)
    //{
    //    Debug.Log($"Body Part {bodyPart.partName} Clicked!");

    //    GameManager.Instance.SelectBodyPart(bodyPart);
    //}

    //private void SetupBodyPartsNavigation(Dictionary<ConnectionDirection, Button> buttonsByDirection)
    //{
    //    foreach (ConnectionDirection direction in buttonsByDirection.Keys)
    //    {
    //        Button bodyPartButton = buttonsByDirection[direction];

    //        IEnumerable<ConnectionDirection> otherDirections = buttonsByDirection.Keys
    //            .Where(key => key != direction);

    //        Navigation navigation = new()
    //        {
    //            mode = Navigation.Mode.Automatic,
    //        };

    //        foreach (ConnectionDirection otherDirection in otherDirections)
    //        {
    //            Button otherPartButton = buttonsByDirection[otherDirection];

    //            switch (otherDirection)
    //            {
    //                case ConnectionDirection.Up:
    //                    navigation.selectOnUp = otherPartButton;
    //                    break;
    //                case ConnectionDirection.Right:
    //                    navigation.selectOnRight = otherPartButton;
    //                    break;
    //                case ConnectionDirection.Down:
    //                    navigation.selectOnDown = otherPartButton;
    //                    break;
    //                case ConnectionDirection.Left:
    //                    navigation.selectOnLeft = otherPartButton;
    //                    break;
    //            }
    //        }

    //        bodyPartButton.navigation = navigation;
    //    }

    //    if (buttonsByDirection.Count > 0)
    //    {
    //        Button randomButton = buttonsByDirection.Values.ElementAt(Random.Range(0, buttonsByDirection.Values.Count - 1));
            
    //        EventSystem.current.SetSelectedGameObject(randomButton.gameObject);
    //    }

    //}

    private void SetupBodyParts()
    {
        bodyPartsParent = InstantiateBodyPartsParent();

        BodyPartConfig currentBodyPart = NavigationSystem.Instance.CurrentBodyPart;

        if (currentBodyPart.bodySystem == bodySystem)
        {
            Debug.Log($"Current part {currentBodyPart.partName}");

            GameObject currentBodyPartGO = InstantiateBodyPart(currentBodyPart);

            Button currentBodyPartButton = currentBodyPartGO.GetComponent<Button>();

            currentBodyPartButton.interactable = false;

            //Dictionary<ConnectionDirection, BodyPartConfig> connectedBodyParts = currentBodyPart.connectedParts;
            //Dictionary<ConnectionDirection, Button> buttonsByDirection = new();

            //foreach (ConnectionDirection direction in connectedBodyParts.Keys)
            //{
            //    BodyPartConfig bodyPart = currentBodyPart.connectedParts[direction];

            //    Debug.Log($"{currentBodyPart.partName} is connected to the {direction} with the {bodyPart.partName}");

            //    GameObject bodyPartGO = InstantiateBodyPart(bodyPart);

            //    Button bodyPartButton = bodyPartGO.GetComponent<Button>();

            //    bodyPartButton.onClick.AddListener(() =>
            //    {
            //        OnClickBodyPart(bodyPart);
            //    });

            //    buttonsByDirection.Add(direction, bodyPartButton);
            //}

            //SetupBodyPartsNavigation(buttonsByDirection);
        }

        //if (currentBodyPart.connectedSystems.TryGetValue(bodySystem, out BodyPartConfig systemBodyPart))
        //{
        //    Debug.Log($"{currentBodyPart.partName} is connected to by the {bodySystem.systemName} system with the {systemBodyPart.partName}");

        //    GameObject bodyPartGO = InstantiateBodyPart(systemBodyPart);

        //    Button bodyPartButton = bodyPartGO.GetComponent<Button>();

        //    bodyPartButton.onClick.AddListener(() =>
        //    {
        //        OnClickBodyPart(systemBodyPart);
        //    });
        //}
    }

    private void UpdateImage()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        
        image.sprite = bodySystem.sprite;
        image.color = bodySystem.color;
    }
}
