using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    private int currentHealth;
    private bool isDead;
    public event Action onDeath;
    public event Action onRespawn;
    public event Action<int> onHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if(isDead) return;

        currentHealth -= amount;
        onHealthChanged?.Invoke(currentHealth);

        if(currentHealth <= 0)
        {
            isDead = true;
            onDeath?.Invoke();
        }
    }

    [ContextMenu("Respawn")]
    public void Respawn()
    {
        if (!isDead) return;

        isDead = false;
        currentHealth = maxHealth;

        onHealthChanged?.Invoke(currentHealth);
        onRespawn?.Invoke();
    }
}
