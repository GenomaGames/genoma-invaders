using UnityEngine;

// https://docs.unity3d.com/2020.1/Documentation/ScriptReference/RequireComponent.html
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Rigidbody2D.html
    private Rigidbody2D rb2D;
    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Vector3-up.html
    private Vector3 move = Vector3.up;

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/MonoBehaviour.Awake.html
    private void Awake()
    {
        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Component.GetComponent.html
        rb2D = GetComponent<Rigidbody2D>();
    }

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/MonoBehaviour.FixedUpdate.html
    private void FixedUpdate()
    {
        if (move != Vector3.zero)
        {
            // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Time-fixedDeltaTime.html
            Vector3 translation = move * speed * Time.fixedDeltaTime;
            Vector3 newPosition = transform.position + translation;

            // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Rigidbody2D.MovePosition.html
            rb2D.MovePosition(newPosition);
        }
    }
}
