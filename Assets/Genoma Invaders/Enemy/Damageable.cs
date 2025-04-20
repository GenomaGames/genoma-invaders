using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public event Action<Damageable, int> OnDamaged;


    [SerializeField]
    private int maxHealth = 1;
    [SerializeField]
    private int currentHealth = 1;


    public int CurrentHealth
    {
        get => currentHealth;
        private set
        {
            currentHealth = value >= 0 ? value : 0;
        }
    }


    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    public void Damage(int damage)
    {
        CurrentHealth -= damage;

        OnDamaged?.Invoke(this, damage);
    }
}
