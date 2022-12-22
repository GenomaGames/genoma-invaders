using UnityEngine;

public class UIRestartButton : MonoBehaviour
{
    public void RestartGame()
    {
        GameManager.Instance.Restart();
    }
}
