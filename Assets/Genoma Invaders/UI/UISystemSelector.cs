using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystemSelector : SerializedMonoBehaviour
{
    [SerializeField]
    private bool isAutoSetupEnabled = true;
    [SerializeField]
    private BodySystemConfig[] bodySystems;
    [SerializeField]
    [AssetsOnly]
    private GameObject bodySystemPrefab;
    [SerializeField]
    private RectTransform bodySystemsParent;
    [SerializeField]
    [AssetsOnly]
    private GameObject bodySystemButtonPrefab;
    [SerializeField]
    private Dictionary<Button, CanvasGroup> tabs;

    private RectTransform rectTransform;
    private CanvasGroup selectedCanvasGroup;

    public void SelectTab(CanvasGroup newCanvasGroup)
    {
        if (selectedCanvasGroup != null)
        {
            selectedCanvasGroup.alpha = 0;
            selectedCanvasGroup.interactable = false;
            selectedCanvasGroup.blocksRaycasts = false;
        }

        selectedCanvasGroup = newCanvasGroup;

        selectedCanvasGroup.alpha = 1;
        selectedCanvasGroup.interactable = true;
        selectedCanvasGroup.blocksRaycasts = true;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        //foreach (CanvasGroup canvasGroup in tabs.Values)
        //{
        //    canvasGroup.alpha = 0;
        //    canvasGroup.interactable = false;
        //    canvasGroup.blocksRaycasts = false;
        //}

        //foreach (Button button in tabs.Keys)
        //{
        //    button.onClick.AddListener(() =>
        //    {
        //        SelectTab(tabs[button]);
        //    });
        //}

        //SelectTab(tabs.Values.First());
    }

    private void Start()
    {
        if (isAutoSetupEnabled)
        {
            SetupBodySystems();
        }
    }

    private void SetupBodySystems()
    {
        tabs.Clear();

        for (int i = bodySystemsParent.childCount; i > 0; i = bodySystemsParent.childCount)
        {
            DestroyImmediate(bodySystemsParent.GetChild(0).gameObject);
        }

        for (int i = rectTransform.childCount; i > 0; i = rectTransform.childCount)
        {
            DestroyImmediate(rectTransform.GetChild(0).gameObject);
        }

        foreach (BodySystemConfig bodySystem in bodySystems)
        {
            SetupBodySystem(bodySystem);
        }
    }

    private void SetupBodySystem(BodySystemConfig bodySystem)
    {
        GameObject bodySystemGO = Instantiate(bodySystemPrefab, bodySystemsParent);

        bodySystemGO.name = bodySystem.systemName;

        UIBodySystem uIBodySystem = bodySystemGO.GetComponent<UIBodySystem>();

        uIBodySystem.BodySystem = bodySystem;

        uIBodySystem.Setup();

        GameObject buttonGO = Instantiate(bodySystemButtonPrefab, rectTransform);

        buttonGO.name = $"{bodySystem.systemName} Button";

        TMP_Text buttonText = buttonGO.GetComponentInChildren<TMP_Text>();
        buttonText.text = bodySystem.systemName;

        Button button = buttonGO.GetComponent<Button>();
        CanvasGroup canvasGroup = bodySystemGO.GetComponent<CanvasGroup>();

        tabs.Add(button, canvasGroup);

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        
        if (GameManager.Instance.CurrentBodyPart.bodySystem == bodySystem)
        {
            SelectTab(canvasGroup);
        }

        button.onClick.AddListener(() => SelectTab(canvasGroup));
    }
}
