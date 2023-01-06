using UnityEngine;

public class LevelScroller : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    private void Update()
    {
        Vector2 translation = Vector2.down * Time.deltaTime * speed;

        transform.Translate(translation);
    }
}
