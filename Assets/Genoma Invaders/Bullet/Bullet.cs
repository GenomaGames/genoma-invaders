using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    private new Rigidbody2D rigidbody2D;
    private Vector3 move = Vector3.up;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (move != Vector3.zero)
        {
            Vector3 translation = move * speed * Time.fixedDeltaTime;
            Vector3 newPosition = transform.position + translation;

            rigidbody2D.MovePosition(newPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Enemy"))
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();

            enemy.Damage();

            Destroy(gameObject);
        }
        else if (collider2D.CompareTag("Bullet Bounds"))
        {
            Destroy(gameObject);
        }
    }
}
