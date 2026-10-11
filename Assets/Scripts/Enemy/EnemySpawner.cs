using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private Transform target;
    [SerializeField] private int enemyCount;
    [SerializeField] private float spawnInterval;

    private void Start()
    {
        StartCoroutine(Spawn());
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
        enemyAI.SetTarget(target);
    }

    private IEnumerator Spawn()
    {
        for(int i = 0; i < enemyCount; i++)
        {
            yield return new WaitForSeconds(spawnInterval);
            CreateEnemy();
        }
    }
}
