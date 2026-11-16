using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;

    private readonly NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    private readonly NetworkVariable<bool> isDead = new NetworkVariable<bool>();

    public event Action Died;
    public event Action Respawned;
    public event Action<int> HealthChanged;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth.Value;
    public bool IsDead => isDead.Value;

    public override void OnNetworkSpawn()
    {
        currentHealth.OnValueChanged += HandleHealthChanged;
        isDead.OnValueChanged += HandleDeadChanged;

        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }
    }

    public override void OnNetworkDespawn()
    {
        currentHealth.OnValueChanged -= HandleHealthChanged;
        isDead.OnValueChanged -= HandleDeadChanged;
    }

    public void TakeDamage(int amount)
    {
        if (!IsServer || isDead.Value) return;

        currentHealth.Value = Mathf.Max(0, currentHealth.Value - amount);

        if (currentHealth.Value <= 0)
        {
            isDead.Value = true;
        }
    }

    [ContextMenu("Respawn")]
    public void Respawn()
    {
        if (!IsServer || !isDead.Value) return;

        currentHealth.Value = maxHealth;
        isDead.Value = false;
    }

    private void HandleHealthChanged(int previousValue, int newValue)
    {
        HealthChanged?.Invoke(newValue);
    }

    private void HandleDeadChanged(bool previousValue, bool newValue)
    {
        if (newValue)
        {
            Died?.Invoke();
        }
        else
        {
            Respawned?.Invoke();
        }
    }
}