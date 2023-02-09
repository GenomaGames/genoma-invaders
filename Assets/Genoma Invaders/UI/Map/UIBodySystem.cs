using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBodySystem : MonoBehaviour
{
    [SerializeField]
    [AssetsOnly]
    private GameObject bodyPartPrefab;

    private RectTransform rectTransform;
    private RectTransform bodyPartsParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetupBodyParts();
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

        bodyPartsParent.SetParent(rectTransform.parent, false);
        bodyPartsParent.sizeDelta = rectTransform.sizeDelta;

        return bodyPartsParent;
    }

    private void OnClickBodyPart(BodyPartConfig bodyPart)
    {
        Debug.Log($"Body Part {bodyPart.partName} Clicked!");

        GameManager.Instance.SelectBodyPart(bodyPart);
    }

    private void SetupBodyPartsNavigation(Dictionary<ConnectionDirection, Button> buttonsByDirection)
    {
        foreach (ConnectionDirection direction in buttonsByDirection.Keys)
        {
            Button bodyPartButton = buttonsByDirection[direction];

            IEnumerable<ConnectionDirection> otherDirections = buttonsByDirection.Keys
                .Where(key => key != direction);

            Navigation navigation = new()
            {
                mode = Navigation.Mode.Explicit,
            };

            foreach (ConnectionDirection otherDirection in otherDirections)
            {
                Button otherPartButton = buttonsByDirection[otherDirection];

                switch (otherDirection)
                {
                    case ConnectionDirection.Up:
                        navigation.selectOnUp = otherPartButton;
                        break;
                    case ConnectionDirection.Right:
                        navigation.selectOnRight = otherPartButton;
                        break;
                    case ConnectionDirection.Down:
                        navigation.selectOnDown = otherPartButton;
                        break;
                    case ConnectionDirection.Left:
                        navigation.selectOnLeft = otherPartButton;
                        break;
                }
            }

            bodyPartButton.navigation = navigation;
        }

        Button randomButton = buttonsByDirection.Values.ElementAt(Random.Range(0, buttonsByDirection.Values.Count - 1));

        EventSystem.current.SetSelectedGameObject(randomButton.gameObject);
    }

    private void SetupBodyParts()
    {
        bodyPartsParent = InstantiateBodyPartsParent();

        BodyPartConfig currentPartConfig = GameManager.Instance.CurrentBodyPart;

        Debug.Log($"Current part {currentPartConfig.partName}");

        GameObject currentBodyPartGO = InstantiateBodyPart(currentPartConfig);

        Button currentBodyPartButton = currentBodyPartGO.GetComponent<Button>();

        currentBodyPartButton.interactable = false;

        Dictionary<ConnectionDirection, BodyPartConfig> connectedBodyParts = currentPartConfig.connectedParts;
        Dictionary<ConnectionDirection, Button> buttonsByDirection = new();

        foreach (ConnectionDirection direction in connectedBodyParts.Keys)
        {
            BodyPartConfig bodyPart = currentPartConfig.connectedParts[direction];

            Debug.Log($"{currentPartConfig.partName} is connected to the {direction} with the {bodyPart.partName}");

            GameObject bodyPartGO = InstantiateBodyPart(bodyPart);

            Button bodyPartButton = bodyPartGO.GetComponent<Button>();

            bodyPartButton.onClick.AddListener(() =>
            {
                OnClickBodyPart(bodyPart);
            });

            buttonsByDirection.Add(direction, bodyPartButton);
        }

        SetupBodyPartsNavigation(buttonsByDirection);
    }
}
