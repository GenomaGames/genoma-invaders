using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 Direction
    {
        get => direction;
        set => direction = value;
    }

    [SerializeField]
    private float speed = 5;

    private new Rigidbody2D rigidbody2D;
    private Vector2 direction = Vector2.up;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
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

    private void Move()
    {
        if (direction != Vector2.zero)
        {
            Vector3 rotatedDirection = transform.rotation * direction;
            Vector3 translation = rotatedDirection * speed * Time.fixedDeltaTime;
            Vector3 newPosition = transform.position + translation;

            rigidbody2D.MovePosition(newPosition);
        }
    }
}
