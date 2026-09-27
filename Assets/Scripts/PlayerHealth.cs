using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private int currentHealth = 100;
    private bool isDead;
    public event Action onDeath;

    public void TakeDamage(int amount)
    {
        if(isDead) return;

        currentHealth -= amount;
        Debug.Log(currentHealth);

        if(currentHealth <= 0)
        {
            isDead = true;
            onDeath?.Invoke();
        }
    }
}
