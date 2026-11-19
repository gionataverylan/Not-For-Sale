using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Unity.Netcode;

public class EnemySpawner : NetworkBehaviour
{
    [SerializeField, FormerlySerializedAs("enemy")] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField, FormerlySerializedAs("enemyCount")] private int baseEnemyCount;
    [SerializeField] private float pauseBetweenWaves;
    [SerializeField] private float spawnInterval;

    private readonly NetworkVariable<int> currentWave = new NetworkVariable<int>();
    private readonly NetworkVariable<int> aliveEnemies = new NetworkVariable<int>();

    public event Action<int> WaveStarted;
    public event Action<int> AliveEnemiesChanged;

    public override void OnNetworkSpawn()
    {
        currentWave.OnValueChanged += HandleWaveChanged;
        aliveEnemies.OnValueChanged += HandleAliveEnemiesChanged;

        WaveStarted?.Invoke(currentWave.Value);
        AliveEnemiesChanged?.Invoke(aliveEnemies.Value);

        if(IsServer && IsSetupValid())
        {
            StartCoroutine(WaveLoop());
        }
    }

    public override void OnNetworkDespawn()
    {
        currentWave.OnValueChanged -= HandleWaveChanged;
        aliveEnemies.OnValueChanged -= HandleAliveEnemiesChanged;

        StopAllCoroutines();
    }

    private bool IsSetupValid()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("EnemySpawner: не заданы префаб врага, игрок или точки спавна. Волны не запущены.", this);
            return false;
        }

        return true;
    }

    private Transform ChoosePosition()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    private void CreateEnemy()
    {
        Transform point = ChoosePosition();
        GameObject enemyItem = Instantiate(enemyPrefab, point.position, point.rotation);

        enemyItem.GetComponent<NetworkObject>().Spawn();
        enemyItem.GetComponent<EnemyHealth>().Died += HandleEnemyDied;
    }

    private IEnumerator Spawn(int count)
    {
        for(int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(spawnInterval);
            CreateEnemy();
        }
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            currentWave.Value += 1;

            int count = Mathf.Max(1, (int)((baseEnemyCount * currentWave.Value) / 1.5f));
            aliveEnemies.Value = count;

            StartCoroutine(Spawn(count));

            yield return new WaitUntil(() => aliveEnemies.Value <= 0);
            yield return new WaitForSeconds(pauseBetweenWaves);
        }
    }

    private void HandleEnemyDied()
    {
        aliveEnemies.Value -= 1;
    }

    private void HandleWaveChanged(int previousValue, int newValue)
    {
        WaveStarted?.Invoke(newValue);
    }

    private void HandleAliveEnemiesChanged(int previousValue, int newValue)
    {
        AliveEnemiesChanged?.Invoke(newValue);
    }
}
