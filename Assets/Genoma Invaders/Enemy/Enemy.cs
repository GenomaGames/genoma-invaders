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

    private bool isDead = false;
    private PowerUpDropper powerUpDropper;
    private new Rigidbody2D rigidbody2D;
    private new Collider2D collider2D;
    private Animator animator;
    private int animatorDieParam = Animator.StringToHash("Die");

    public void Damage()
    {
        Die();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        powerUpDropper = GetComponent<PowerUpDropper>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Move(Vector2.down);
        }
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
        powerUpDropper.TryDrop();
        collider2D.enabled = false;

        isDead = true;

        if (OnDie != null)
        {
            OnDie(this);
        }

        animator.SetTrigger(animatorDieParam);

        Destroy(gameObject, 1);
    }
}
