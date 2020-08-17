using UnityEngine;

// https://docs.unity3d.com/2020.1/Documentation/ScriptReference/RequireComponent.html
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 2;

    [SerializeField]
    private GameObject bullet;

    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Rigidbody2D.html
    private Rigidbody2D rb2D;
    // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Vector3-zero.html
    private Vector3 move = Vector3.zero;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Input.GetAxisRaw.html
        float rawHorizontalAxis = Input.GetAxisRaw("Horizontal");

        move.x = rawHorizontalAxis;

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Input.GetButtonDown.html
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 playerPosition = transform.position;

            // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Quaternion-identity.html
            Instantiate(bullet, playerPosition, Quaternion.identity);
        }
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
