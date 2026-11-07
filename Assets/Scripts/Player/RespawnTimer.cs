using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class RespawnTimer : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("spawnTimer")] private int respawnDelay = 3;

    private PlayerHealth playerHealth;

    public event Action<int> CountChanged;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        playerHealth.Died += StartCounter;    
    }

    private void OnDisable()
    {
        playerHealth.Died -= StartCounter;
    }

    private void StartCounter()
    {
        StartCoroutine(CounterRestart());
    }

    private IEnumerator CounterRestart()
    {
        for (int i = respawnDelay; i > 0; i--)
        {
            CountChanged?.Invoke(i);
            yield return new WaitForSeconds(1f);
        }
        
        playerHealth.Respawn();
    }
}
