using UnityEngine;

public class FireRatePowerUpEffect : PowerUpEffect
{
    [SerializeField]
    private float fireRate = 4;
    [SerializeField]
    private int ammo = 50;
    private int remainningAmmo;
    private Player player;

    public override void Apply(Player player)
    {
        player.FireRate = fireRate;

        remainningAmmo = ammo;

        this.player = player;
        player.OnShot += OnPlayerShot;
    }

    private void OnPlayerShot()
    {
        if (remainningAmmo > 0)
        {
            remainningAmmo--;
        }
        else
        {
            player.ResetFireRate();
            player.OnShot -= OnPlayerShot;
            Destroy(gameObject);
        }
    }
}
