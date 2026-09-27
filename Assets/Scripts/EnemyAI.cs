using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 2f;

    [SerializeField] private int damage = 25;

    private enum EnemyState { Chase, Attack, Death };
    private EnemyState currentState = EnemyState.Chase;
    private NavMeshAgent agent;
    private EnemyHealth enemyHealth;
    private float attackCooldownCounter;

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

        float dist = Vector3.Distance(transform.position, target.position);
        Debug.Log("Дистанция: " + dist);

        if (Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            currentState = EnemyState.Attack;
            agent.isStopped = true;
        }
    }

    private void AttackState()
    {
        if (Vector3.Distance(transform.position, target.position) > attackRange)
        {
            currentState = EnemyState.Chase;
            agent.isStopped = false;
            return;
        }

        attackCooldownCounter -= Time.deltaTime;

        if (attackCooldownCounter <= 0)
        {
            if (target.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
            }

            attackCooldownCounter = attackCooldown; 
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
