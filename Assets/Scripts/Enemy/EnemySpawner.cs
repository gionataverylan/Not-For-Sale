using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private Transform target;

    private void Start()
    {
        CreateEnemy();
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
}
