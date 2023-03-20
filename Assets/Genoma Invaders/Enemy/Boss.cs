using System;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public event Action<Boss> OnDie;

    public float DiseaseLevel
    {
        get => diseaseLevel;
    }

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
    private readonly int animatorHurtParam = Animator.StringToHash("Hurt");

    private bool isDead = false;
    private PowerUpDropper powerUpDropper;
    private new Collider2D collider2D;
    private Animator animator;
    private float dieAnimationLength;
    private Damageable damageable;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        damageable = GetComponent<Damageable>();
        powerUpDropper = GetComponent<PowerUpDropper>();

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

    private void OnEnable()
    {
        damageable.OnDamaged += OnDamaged;
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
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
            diseaseLevel *= 2;
        }
    }

    [ContextMenu("Kill")]
    private void Die()
    {
        isDead = true;
        collider2D.enabled = false;

        //powerUpDropper.TryDrop();

        animator.SetTrigger(animatorDieParam);

        AudioManager.Instance.Play(explosionSound);

        OnDie?.Invoke(this);

        Destroy(gameObject, dieAnimationLength);
    }

    private void OnDamaged(Damageable damageable, int damage)
    {
        if (damageable.CurrentHealth > 0)
        {
            animator.SetTrigger(animatorHurtParam);
            AudioManager.Instance.Play(hurtSound);
        }
        else
        {
            Die();
        }
    }
}
