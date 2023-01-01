using System;
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

    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private LayerMask boundsLayer;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletsParent;
    [SerializeField]
    private float fireRate = 2f;
    private float initalFireRate;

    private float ShotCooldown
    {
        get
        {
            return Mathf.Round((1 / fireRate) * 100) / 100;
        }
    }

    private Vector2 moveInput;
    private new Rigidbody2D rigidbody2D;
    private ContactFilter2D boundsContactFilter2D;
    private PlayerInput playerInput;
    private float currentShotCooldown = 0;
    private bool isHoldingFireInput = false;

    public void Damage()
    {
        Die();
    }

    public void ResetFireRate()
    {
        fireRate = initalFireRate;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        //Debug.Log($"Move: {context.phase} {context.ReadValue<Vector2>()}");

        moveInput = context.ReadValue<Vector2>();
    }

    public void OnFireInput(InputAction.CallbackContext context)
    {
        //Debug.Log($"Fire: {context.phase} {context.ReadValue<float>()}");

        if (context.started)
        {
            isHoldingFireInput = true;
        }

        if (context.performed)
        {
            Shoot();
        }

        if (context.canceled)
        {
            isHoldingFireInput = false;
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

    private void Awake()
    {
        initalFireRate = fireRate;
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        boundsContactFilter2D = new ContactFilter2D();
        boundsContactFilter2D.SetLayerMask(boundsLayer);
    }

    private void Start()
    {
        if (EventSystem.current != null)
        {
            InputSystemUIInputModule inputSystemUIInputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
            playerInput.uiInputModule = inputSystemUIInputModule;
        }

        playerInput.camera = Camera.main;
    }

    private void Update()
    {
        if (currentShotCooldown > 0)
        {
            currentShotCooldown = Mathf.Clamp(currentShotCooldown - Time.deltaTime, 0, ShotCooldown);
        }

        if (isHoldingFireInput)
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            Move(moveInput);
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

            if (OnShot != null)
            {
                OnShot();
            }

            currentShotCooldown = ShotCooldown;
        }
    }

    private void Die()
    {
        OnDie.Invoke(this);

        Destroy(gameObject);
    }
}
