using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int initialHealth { get; } = 100;
    private int currentHealth;
    private bool isDead;
    public event Action onDeath;
    public event Action<int> onHealthChanged;

    private void Awake()
    {
        currentHealth = initialHealth;
    }

    public void TakeDamage(int amount)
    {
        if(isDead) return;

        currentHealth -= amount;
        onHealthChanged?.Invoke(currentHealth);

        Debug.Log(currentHealth);

        if(currentHealth <= 0)
        {
            isDead = true;
            onDeath?.Invoke();
        }
    }
}
