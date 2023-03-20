using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public event Action<Damageable, int> OnDamaged;

    public int CurrentHealth
    {
        get => currentHealth;
        private set
        {
            currentHealth = value >= 0 ? value : 0;
        }
    }

    [SerializeField]
    private int maxHealth = 1;
    [SerializeField]
    private int currentHealth = 1;

    public void Damage(int damage)
    {
        CurrentHealth -= damage;

        OnDamaged?.Invoke(this, damage);
    }
}
