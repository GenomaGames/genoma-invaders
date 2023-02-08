using UnityEngine;
using UnityEngine.EventSystems;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject firstSelected;

    private CanvasGroup canvasGroup;

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0;
    }

    public void Quit()
    {
        GameManager.Instance.GoToMainMenu();
    }

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        canvasGroup.alpha = 1;

        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        GameManager.OnGamePaused += Show;
        GameManager.OnGameUnpaused += Hide;

        if (GameManager.Instance.IsGamePaused)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void OnDisable()
    {
        GameManager.OnGamePaused -= Show;
        GameManager.OnGameUnpaused -= Hide;
    }
}
