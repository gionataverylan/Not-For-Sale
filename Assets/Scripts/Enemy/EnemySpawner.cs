using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, FormerlySerializedAs("enemy")] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField, FormerlySerializedAs("target")] private Transform player;
    [SerializeField, FormerlySerializedAs("enemyCount")] private int baseEnemyCount;
    [SerializeField] private float pauseBetweenWaves;
    [SerializeField] private float spawnInterval;

    private int currentWave;
    private int aliveEnemies;

    public event Action<int> WaveStarted;
    public event Action<int> AliveEnemiesChanged;

    private void Start()
    {
        if (!IsSetupValid()) return;

        StartCoroutine(WaveLoop());
    }
    private bool IsSetupValid()
    {
        if (enemyPrefab == null || player == null || spawnPoints == null || spawnPoints.Length == 0)
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

        EnemyAI enemyAI = enemyItem.GetComponent<EnemyAI>();
        EnemyHealth enemyHealth = enemyItem.GetComponent<EnemyHealth>();

        enemyHealth.Died += HandleDeathEnemy;
        enemyAI.SetTarget(player);
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
            currentWave += 1;
            WaveStarted?.Invoke(currentWave);

            int count = Mathf.Max(1, (int)((baseEnemyCount * currentWave) / 1.5f));
            aliveEnemies = count;
            AliveEnemiesChanged?.Invoke(aliveEnemies);

            StartCoroutine(Spawn(count));

            yield return new WaitUntil(() => aliveEnemies <= 0);
            yield return new WaitForSeconds(pauseBetweenWaves);
        }
    }

    private void HandleDeathEnemy()
    {
        aliveEnemies -= 1;
        AliveEnemiesChanged?.Invoke(aliveEnemies);
    }
}
