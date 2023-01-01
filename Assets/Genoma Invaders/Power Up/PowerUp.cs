using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private GameObject powerUpEffectPrefab;

    private new Rigidbody2D rigidbody2D;

    public void Apply(Player player)
    {
        GameObject powerUpEffectGameObject = Instantiate(powerUpEffectPrefab, Vector2.zero, Quaternion.identity);
        PowerUpEffect powerUpEffect = powerUpEffectGameObject.GetComponent<PowerUpEffect>();
        powerUpEffect.Apply(player);

        Destroy(gameObject);
    }

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move(Vector2.down);
    }

    private void Move(Vector2 direction)
    {
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + direction * Time.fixedDeltaTime * speed;

        rigidbody2D.MovePosition(newPosition);
    }
}
