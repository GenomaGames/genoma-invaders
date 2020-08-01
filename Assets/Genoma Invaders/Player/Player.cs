using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 2;

    [SerializeField]
    private GameObject bullet;

    private void Update()
    {
        // https://docs.unity3d.com/ScriptReference/Input.GetAxisRaw.html
        float rawHorizontalAxis = Input.GetAxisRaw("Horizontal");

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Vector3-zero.html
        Vector3 direction = Vector3.zero;
        direction.x = rawHorizontalAxis;

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Time-deltaTime.html
        float timeSinceLastFrame = Time.deltaTime;

        Vector3 translation = direction * speed * timeSinceLastFrame;

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Transform.Translate.html
        transform.Translate(
          translation
        );

        // https://docs.unity3d.com/ScriptReference/Input.GetButtonDown.html
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 playerPosition = transform.position;

            // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Quaternion-identity.html
            Instantiate(bullet, playerPosition, Quaternion.identity);
        }
    }
}
