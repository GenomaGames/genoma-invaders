using UnityEngine;

public class MultishotPowerUpEffect : PowerUpEffect
{
    [SerializeField]
    private int ammo = 25;
    private int remainningAmmo;
    private Player player;

    public override void Apply(Player player)
    {
        remainningAmmo = ammo;

        this.player = player;

        player.OnShot += OnPlayerShot;
    }

    private void OnPlayerShot()
    {
        if (remainningAmmo > 0)
        {
            GameObject leftBulletGameObject = Instantiate(player.BulletPrefab, player.transform.position, Quaternion.identity, player.BulletsParent);

            Bullet leftBullet = leftBulletGameObject.GetComponent<Bullet>();
            leftBullet.Direction = Quaternion.Euler(0, 0, 20f) * leftBullet.Direction;

            GameObject rightBulletGameObject = Instantiate(player.BulletPrefab, player.transform.position, Quaternion.identity, player.BulletsParent);

            Bullet rightBullet = rightBulletGameObject.GetComponent<Bullet>();
            rightBullet.Direction = Quaternion.Euler(0, 0, -20f) * rightBullet.Direction;

            remainningAmmo--;
        }
        else
        {
            player.OnShot -= OnPlayerShot;
            Destroy(gameObject);
        }
    }
}
