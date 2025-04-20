using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static event Action<Boss> OnStart;

    public event Action<Boss> OnDie;

    public float DiseaseLevel
    {
        get => diseaseLevel;
    }

    [SerializeField]
    private float diseaseLevel = 10;
    [Required]
    [SerializeField]
    private AnimationClip dieAnimation;
    [SerializeField]
    private AudioClip hurtSound;
    [SerializeField]
    private AudioClip explosionSound;

    private readonly int animatorDieParam = Animator.StringToHash("Die");
    private readonly int animatorHurtParam = Animator.StringToHash("Hurt");

    private new Collider2D collider2D;
    private Animator animator;
    private float dieAnimationLength;
    private Damageable damageable;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        damageable = GetComponent<Damageable>();

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

    private void Start()
    {
        OnStart?.Invoke(this);
    }

    [ContextMenu("Kill")]
    private void Die()
    {
        collider2D.enabled = false;

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
