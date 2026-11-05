using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private Transform target;
    [SerializeField] private int enemyCount;
    [SerializeField] private float pauseBetweenWaves;
    [SerializeField] private float spawnInterval;

    private int currentWave;
    private int aliveEnemies;

    public event Action<int> onWaveStarted;
    public event Action<int> onEnemyStarted;

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private Transform ChoosePosition()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    private void CreateEnemy()
    {
        Transform point = ChoosePosition();
        GameObject enemyItem = Instantiate(enemy, point.position, point.rotation);

        EnemyAI enemyAI = enemyItem.GetComponent<EnemyAI>();

        EnemyHealth enemyHealth = enemyItem.GetComponent<EnemyHealth>();
        enemyHealth.OnDeath += HandleDeathEnemy;

        enemyAI.SetTarget(target);
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
            onWaveStarted?.Invoke(currentWave);

            int count = Mathf.Max(1, (int)((enemyCount * currentWave) / 1.5f));
            aliveEnemies = count;
            onEnemyStarted?.Invoke(aliveEnemies);

            StartCoroutine(Spawn(count));

            yield return new WaitUntil(() => aliveEnemies <= 0);
            yield return new WaitForSeconds(pauseBetweenWaves);
        }
    }

    private void HandleDeathEnemy()
    {
        aliveEnemies -= 1;
        onEnemyStarted?.Invoke(aliveEnemies);

        Debug.Log(aliveEnemies);
    }
}
