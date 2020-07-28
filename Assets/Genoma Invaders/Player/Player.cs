using UnityEngine;

public class Player : MonoBehaviour
{
    void Update()
    {
        // https://docs.unity3d.com/ScriptReference/Input.GetAxisRaw.html
        float rawHorizontalAxis = Input.GetAxisRaw("Horizontal");

        Vector3 direction = Vector3.zero;
        direction.x = rawHorizontalAxis;

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Time-deltaTime.html
        float timeSinceLastFrame = Time.deltaTime;

        Vector3 translation = direction * timeSinceLastFrame;

        // https://docs.unity3d.com/2020.1/Documentation/ScriptReference/Transform.Translate.html
        transform.Translate(
          translation
        );
    }
}
