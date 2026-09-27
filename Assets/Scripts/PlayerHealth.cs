using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private int currentHealth = 100;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(currentHealth);
    }
}
