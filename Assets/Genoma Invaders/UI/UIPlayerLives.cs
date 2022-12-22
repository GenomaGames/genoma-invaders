using UnityEngine;
using UnityEngine.UI;

public class UIPlayerLives : MonoBehaviour
{
    private Image[] lives;

    private void OnEnable()
    {
        GameManager.OnPlayerLivesChanged += OnPlayerLivesChanged;
    }

    private void Start()
    {
        lives = GetComponentsInChildren<Image>();
    }

    private void OnDisable()
    {
        GameManager.OnPlayerLivesChanged -= OnPlayerLivesChanged;
    }

    private void OnPlayerLivesChanged(int remainingLives)
    {
        for (int i = 0; i < 3; i++)
        {
            if (lives[i] != null)
            {
                lives[i].color = remainingLives > i ? Color.white : Color.clear;
            }
        }
    }
}
