using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnDie;

    public float DiseaseLevel
    {
        get => diseaseLevel;
    }

    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float diseaseLevel = 10;

    private new Rigidbody2D rigidbody2D;

    public void Damage()
    {
        Die();
    }

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move(Vector2.down);
    }

    private void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        if (otherCollider2D.CompareTag("Player"))
        {
            Player player = otherCollider2D.GetComponent<Player>();

            player.Damage();
        }
        else if (otherCollider2D.CompareTag("Enemy Finish Line"))
        {
            GameManager.Instance.UpdateDiseaseLevel(diseaseLevel);

            transform.position = new Vector3(transform.position.x, 40, transform.position.z);
            speed += 1;
            diseaseLevel *= 2;
        }
    }

    private void Move(Vector2 direction)
    {
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + direction * Time.fixedDeltaTime * speed;

        rigidbody2D.MovePosition(newPosition);
    }

    private void Die()
    {
        if (OnDie != null)
        {
            OnDie(this);
        }

        Destroy(gameObject);
    }
}
