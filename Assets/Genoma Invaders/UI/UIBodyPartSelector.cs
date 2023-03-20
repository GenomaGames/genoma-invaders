using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBodyPartSelector : MonoBehaviour
{
    [Required]
    [SerializeField]
    private TMP_Text currentLocationName;
    [SerializeField]
    private Button[] buttons;

    private void Awake()
    {
        if (buttons != null && buttons.Length > 0)
        {
            UpdateCurrentLocation();
            SetupDestinations();
        }
        else
        {
            throw new UnityException("No buttons were referenced");
        }
    }

    private void SetupDestinations()
    {
        BodyPartConfig[] connectedParts = NavigationSystem.Instance.CurrentBodyPart.connectedParts;

        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            var connectedPart = i < connectedParts.Length ? connectedParts[i] : null;

            if (connectedPart != null)
            {
                button.gameObject.SetActive(true);

                TMP_Text text = button.GetComponentInChildren<TMP_Text>();

                text.text = connectedPart.partName;

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => NavigationSystem.Instance.GoToBodyPart(connectedPart));

                if (i == 0)
                {
                    EventSystem.current.SetSelectedGameObject(button.gameObject);
                }
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateCurrentLocation()
    {
        currentLocationName.text = NavigationSystem.Instance.CurrentBodyPart.partName;
    }
}
