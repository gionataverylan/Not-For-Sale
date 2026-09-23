using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float attackRange = 2f;

    private enum EnemyState { Chase, Attack, Death };
    private EnemyState currentState = EnemyState.Chase;
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
        switch (currentState)
        {
            case EnemyState.Chase: ChaseState(); break;
            case EnemyState.Attack: AttackState(); break;
            case EnemyState.Death: return;
        }
    }

    private void ChaseState()
    {
        agent.SetDestination(target.position);

        if(Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
    }

    private void AttackState()
    {
        if (Vector3.Distance(transform.position, transform.position) > attackRange)
        {
            currentState |= EnemyState.Attack;
            return;
        }
    }

    private void HandleDeath()
    {
        currentState = EnemyState.Death;

        agent.enabled = false;
        Destroy(gameObject);
        Debug.Log("Death");
    }
}
