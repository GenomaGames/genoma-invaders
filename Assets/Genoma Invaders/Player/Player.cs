using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class Player : MonoBehaviour
{
    public event Action<Player> OnDie;
    public event Action OnShot;

    public Transform BulletsParent
    {
        get => bulletsParent;
    }

    public GameObject BulletPrefab
    {
        get => bulletPrefab;
    }

    public float FireRate
    {
        get => fireRate;
        set => fireRate = value;
    }

    private float ShotCooldown
    {
        get
        {
            return Mathf.Round((1 / fireRate) * 100) / 100;
        }
    }

    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private LayerMask boundsLayer;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float fireRate = 2f;
    [SerializeField]
    [Tooltip("Required to destroy de GO when it ends")]
    private AnimationClip dieAnimation;

    private Transform bulletsParent;
    private float initalFireRate;
    private Vector2 moveInput;
    private new Rigidbody2D rigidbody2D;
    private new Collider2D collider2D;
    private Animator animator;
    private ContactFilter2D boundsContactFilter2D;
    private PlayerInput playerInput;
    private float currentShotCooldown = 0;
    private bool isHoldingShootInput = false;
    private bool isDead = false;
    private float dieAnimationLength;
    private readonly int animatorDieParam = Animator.StringToHash("Die");
    private readonly int animatorMoveXParam = Animator.StringToHash("Move X");

    public void Damage()
    {
        Die();
    }

    public void ResetFireRate()
    {
        fireRate = initalFireRate;
    }

    public void OnDieAnimationEnded()
    {
    
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        //Debug.Log($"Move: {context.phase} {context.ReadValue<Vector2>()}");

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnShootInput(InputAction.CallbackContext context)
    {
        //Debug.Log($"Shoot: {context.phase} {context.ReadValue<float>()}");

        if (context.started)
        {
            isHoldingShootInput = true;
        }

        if (context.performed)
        {
            Shoot();
        }

        if (context.canceled)
        {
            isHoldingShootInput = false;
        }
    }

    public void OnMenuInput(InputAction.CallbackContext context)
    {
        //Debug.Log($"Menu: {context.phase} {context.ReadValue<float>()}");

        if (context.performed)
        {
            GameManager.Instance.GoToMainMenu();
        }
    }

    public void OnTapInput(InputAction.CallbackContext context)
    {
        Debug.Log($"Tap!");

        if (!GameManager.Instance.IsTouchUIEnabled && (context.started || context.performed))
        {
            GameManager.Instance.EnableTouchUI();
        }
    }

    private void Awake()
    {
        initalFireRate = fireRate;

        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        playerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        boundsContactFilter2D = new ContactFilter2D();
        boundsContactFilter2D.SetLayerMask(boundsLayer);

        if (dieAnimation == null)
        {
            throw new UnityException("Die Animation needs to be set.");
        }

        AnimationClip dieAnimationClipFromAnimator = animator.runtimeAnimatorController.animationClips.Where(clip => clip.name == dieAnimation.name).First();

        if (dieAnimationClipFromAnimator == null)
        {
            throw new UnityException("Provided Die Animation not found in the attached animator controller.");
        }

        dieAnimationLength = dieAnimationClipFromAnimator.length;
    }

    private void Start()
    {
        if (EventSystem.current != null)
        {
            InputSystemUIInputModule inputSystemUIInputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
            playerInput.uiInputModule = inputSystemUIInputModule;
        }

        playerInput.camera = Camera.main;

        GameObject bulletsParentGO = GameObject.FindGameObjectWithTag("Player Bullets Parent");

        if (bulletsParentGO == null)
        {
            bulletsParentGO = new GameObject("Player Bullets");
            bulletsParentGO.tag = "Player Bullets Parent";

            bulletsParent = bulletsParentGO.transform;
        }
    }

    private void Update()
    {
        if (currentShotCooldown > 0)
        {
            currentShotCooldown = Mathf.Clamp(currentShotCooldown - Time.deltaTime, 0, ShotCooldown);
        }

        if (!isDead)
        {
            if (isHoldingShootInput)
            {
                Shoot();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (moveInput != Vector2.zero)
            {
                Move(moveInput);
            }

            animator.SetFloat(animatorMoveXParam, moveInput.x);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider2D)
    {
        if (otherCollider2D.CompareTag("Power Up"))
        {
            PowerUp powerUp = otherCollider2D.GetComponent<PowerUp>();

            powerUp.Apply(this);
        }
    }

    private void Move(Vector2 direction)
    {
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + direction * Time.fixedDeltaTime * speed;

        if (!CanMoveTo(newPosition))
        {
            Vector2 horizontalMoveDirection = new Vector2(direction.x, 0);
            newPosition = currentPosition + horizontalMoveDirection * Time.fixedDeltaTime * speed;
        }

        if (!CanMoveTo(newPosition))
        {
            Vector2 verticalMoveDirection = new Vector2(0, direction.y);
            newPosition = currentPosition + verticalMoveDirection * Time.fixedDeltaTime * speed;
        }

        if (CanMoveTo(newPosition))
        {
            rigidbody2D.MovePosition(newPosition);
        }
    }

    private bool CanMoveTo(Vector2 targetPosition)
    {
        Vector2 currentPosition = transform.position;
        Vector2 direction = targetPosition - currentPosition;
        float distance = Vector2.Distance(currentPosition, targetPosition);

        RaycastHit2D[] hits = new RaycastHit2D[1];

        int hitCount = rigidbody2D.Cast(direction, boundsContactFilter2D, hits, distance);

        bool canMove = hitCount == 0;

        return canMove;
    }

    private void Shoot()
    {
        //Debug.Log("Shoot");

        if (currentShotCooldown <= 0)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletsParent);

            OnShot?.Invoke();

            currentShotCooldown = ShotCooldown;
        }
    }

    [ContextMenu("Kill")]
    private void Die()
    {
        isDead = true;
        collider2D.enabled = false;

        animator.SetTrigger(animatorDieParam);

        OnDie?.Invoke(this);

        Destroy(gameObject, dieAnimationLength);
    }
}
