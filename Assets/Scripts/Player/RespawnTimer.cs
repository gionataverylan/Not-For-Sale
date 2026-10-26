using UnityEngine;
using System.Collections;
using System;

public class RespawnTimer : MonoBehaviour
{
    [SerializeField] private int spawnTimer = 3;
    private PlayerHealth playerHealth;
    public event Action<int> onCountChanged;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        playerHealth.onDeath += StartCounter;    
    }

    private void OnDisable()
    {
        playerHealth.onDeath -= StartCounter;
    }

    private void StartCounter()
    {
        StartCoroutine(CounterRestart());
    }

    private IEnumerator CounterRestart()
    {
        for (int i = spawnTimer; i > 0; i--)
        {
            onCountChanged?.Invoke(i);
            yield return new WaitForSeconds(1f);
        }
        
        playerHealth.Respawn();
    }
}
