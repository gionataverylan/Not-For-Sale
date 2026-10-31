using System;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int amount);
    bool IsDead {  get; }
}

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public event Action OnDeath;
    private bool isDead;
    private int currentHealth = 100;
    public bool IsDead => isDead;

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log(currentHealth);

        if(currentHealth <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }
}
