using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public event Action Died;

    private int currentHealth = 100;
    private bool isDead;

    public bool IsDead => isDead;

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            isDead = true;
            Died?.Invoke();
        }
    }
}
