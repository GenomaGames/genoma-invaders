using System;
using System.Linq;
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
    [SerializeField]
    [Tooltip("Required to destroy de GO when it ends")]
    private AnimationClip dieAnimation;
    [SerializeField]
    private AudioClip hurtSound;
    [SerializeField]
    private AudioClip explosionSound;

    private readonly int animatorDieParam = Animator.StringToHash("Die");

    private bool isDead = false;
    private PowerUpDropper powerUpDropper;
    private new Rigidbody2D rigidbody2D;
    private new Collider2D collider2D;
    private Animator animator;
    private float dieAnimationLength;

    public void Damage()
    {
        AudioManager.Instance.Play(hurtSound);
        Die();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        powerUpDropper = GetComponent<PowerUpDropper>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        if (dieAnimation == null)
        {
            throw new UnityException("Die Animation needs to be set.");
        }

        AnimationClip dieAnimationClipFromAnimator = animator.runtimeAnimatorController.animationClips.Where(clip => clip.name == dieAnimation.name).First();

        if (dieAnimationClipFromAnimator != null)
        {
            dieAnimationLength = dieAnimationClipFromAnimator.length;
        }
        else
        {
            throw new UnityException("Provided Die Animation not found in the attached animator controller.");
        }
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
            DiseaseManager.Instance.UpdateDiseaseLevel(diseaseLevel);

            transform.position = new Vector3(transform.position.x, 40, transform.position.z);
            speed += 1;
            diseaseLevel *= 2;
        }
    }

    private void Move(Vector2 direction)
    {
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + speed * Time.fixedDeltaTime * direction;

        rigidbody2D.MovePosition(newPosition);
    }

    [ContextMenu("Kill")]
    private void Die()
    {
        isDead = true;
        collider2D.enabled = false;

        powerUpDropper.TryDrop();

        animator.SetTrigger(animatorDieParam);

        AudioManager.Instance.Play(explosionSound);

        OnDie?.Invoke(this);

        Destroy(gameObject, dieAnimationLength);
    }
}
