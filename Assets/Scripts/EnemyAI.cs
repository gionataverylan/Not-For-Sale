using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;

    private NavMeshAgent agent;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        enemyHealth.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        enemyHealth.OnDeath -= HandleDeath;
    }

    private void Update()
    {
        ChaseState();
    }

    private void ChaseState()
    {
        agent.SetDestination(target.position);
    }

    private void HandleDeath()
    {
        Debug.Log("Death");
        //currentState = EnemyState.Death;
    }
}
