using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class RespawnTimer : NetworkBehaviour
{
    [SerializeField] private int respawnDelay = 3;

    private readonly NetworkVariable<int> secondsLeft = new NetworkVariable<int>();

    private PlayerHealth playerHealth;

    public event Action<int> CountChanged;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public override void OnNetworkSpawn()
    {
        secondsLeft.OnValueChanged += HandleSecondsChanged;

        if (IsServer)
        {
            playerHealth.Died += StartCountdown;
        }
    }

    public override void OnNetworkDespawn()
    {
        secondsLeft.OnValueChanged -= HandleSecondsChanged;

        if (IsServer)
        {
            playerHealth.Died -= StartCountdown;
        }
    }

    private void StartCountdown()
    {
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        for (int i = respawnDelay; i > 0; i--)
        {
            secondsLeft.Value = i;
            yield return new WaitForSeconds(1f);
        }

        playerHealth.Respawn();
        secondsLeft.Value = 0;
    }

    private void HandleSecondsChanged(int previousValue, int newValue)
    {
        CountChanged?.Invoke(newValue);
    }
}