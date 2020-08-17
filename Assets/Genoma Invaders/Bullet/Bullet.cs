using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Rigidbody2D.html
    private Rigidbody2D rb2D;

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/MonoBehaviour.Awake.html
    private void Awake () {
        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Component.GetComponent.html
        rb2D = GetComponent<Rigidbody2D>();
    }

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Vector3-up.html
        Vector3 direction = Vector3.up;

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Time-deltaTime.html
        float timeSinceLastFrame = Time.deltaTime;

        Vector3 translation = direction * speed * timeSinceLastFrame;

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Transform.Translate.html
        transform.Translate(
          translation
        );
    }
}
