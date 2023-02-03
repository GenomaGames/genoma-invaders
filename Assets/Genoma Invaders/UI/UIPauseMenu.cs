using UnityEngine;

public class UIPauseMenu : MonoBehaviour
{
    private CanvasGroup canvasGroud;

    public void Hide()
    {
        canvasGroud.blocksRaycasts = false;
        canvasGroud.interactable = false;
        canvasGroud.alpha = 0;
    }

    public void Quit()
    {
        GameManager.Instance.GoToMainMenu();
    }

    public void Show()
    {
        canvasGroud.blocksRaycasts = true;
        canvasGroud.interactable = true;
        canvasGroud.alpha = 1;
    }

    private void Awake()
    {
        canvasGroud = GetComponent<CanvasGroup>();
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
