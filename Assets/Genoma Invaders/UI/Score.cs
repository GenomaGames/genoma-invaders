using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private int score = 0;
    // https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/script-Text.html
    private Text text;

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/MonoBehaviour.Awake.html
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        text.text = score.ToString();
    }

    public void AddScore (int points)
    {
        score += points;

        text.text = score.ToString();
    }
}
