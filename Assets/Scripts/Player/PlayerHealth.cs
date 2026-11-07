using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private bool isDead;

    public event Action Died;
    public event Action Respawned;
    public event Action<int> HealthChanged;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if(isDead) return;

        currentHealth -= amount;
        HealthChanged?.Invoke(currentHealth);

        if(currentHealth <= 0)
        {
            isDead = true;
            Died?.Invoke();
        }
    }

    [ContextMenu("Respawn")]
    public void Respawn()
    {
        if (!isDead) return;

        isDead = false;
        currentHealth = maxHealth;

        HealthChanged?.Invoke(currentHealth);
        Respawned?.Invoke();
    }
}
