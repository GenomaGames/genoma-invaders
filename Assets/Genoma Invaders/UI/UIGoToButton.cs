using UnityEngine;

public class UIGoToButton : MonoBehaviour
{
    public void GoToDesktop()
    {
        GameManager.Instance.Quit();
    }

    public void GoToMainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }

    public void GoToNewGame()
    {
        GameManager.Instance.NewGame();
    }
}
