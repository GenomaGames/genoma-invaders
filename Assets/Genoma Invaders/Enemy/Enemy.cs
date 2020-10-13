using UnityEngine;

// https://docs.unity3d.com/2020.1/Documentation/ScriptReference/RequireComponent.html
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Rigidbody2D.html
    private Rigidbody2D rb2D;
    private Score score;

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/MonoBehaviour.Awake.html
    private void Awake()
    {
        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Component.GetComponent.html
        rb2D = GetComponent<Rigidbody2D>();

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Object.FindObjectOfType.html
        score = FindObjectOfType<Score>();
    }

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/MonoBehaviour.OnDestroy.html
    private void OnDestroy()
    {
        Debug.Log("OnDestroy1");
        score.AddScore(100); // Increment game score by 100
    }
}
