using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int damage = 25;

    private enum EnemyState { Chase, Attack, Death, Idle }

    private EnemyState currentState = EnemyState.Chase;
    private Transform target;
    private IDamageable targetDamageable;
    private NavMeshAgent agent;
    private EnemyHealth enemyHealth;
    private float attackCooldownCounter;

    private bool IsTargetDead => targetDamageable != null && targetDamageable.IsDead;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        enemyHealth.Died += HandleDeath;
    }

    private void OnDisable()
    {
        enemyHealth.Died -= HandleDeath;
    }

    private void Update()
    {
        if (currentState == EnemyState.Death) return;
        if (target == null) return;

        if (IsTargetDead && currentState != EnemyState.Idle)
        {
            currentState = EnemyState.Idle;
            agent.isStopped = true;
            attackCooldownCounter = attackCooldown;
        }

        switch (currentState)
        {
            case EnemyState.Chase: ChaseState(); break;
            case EnemyState.Attack: AttackState(); break;
            case EnemyState.Idle: IdleState(); break;
        }
    }

    private void ChaseState()
    {
        agent.SetDestination(target.position);

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= attackRange)
        {
            currentState = EnemyState.Attack;
            agent.isStopped = true;
        }
    }

    private void AttackState()
    {
        float dist = Vector3.Distance(transform.position, target.position);

        if (dist > attackRange)
        {
            currentState = EnemyState.Chase;
            agent.isStopped = false;
            return;
        }

        attackCooldownCounter -= Time.deltaTime;

        if (attackCooldownCounter <= 0)
        {
            targetDamageable?.TakeDamage(damage);
            attackCooldownCounter = attackCooldown; 
        }
    }

    private void IdleState()
    {
        if (IsTargetDead) return;

        currentState = EnemyState.Chase;
        agent.isStopped = false;
    }

    private void HandleDeath()
    {
        currentState = EnemyState.Death;
        agent.enabled = false;
        Destroy(gameObject);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        target.TryGetComponent(out targetDamageable);

        if (target != null)
        {
            target.TryGetComponent(out targetDamageable);
        }
    }
}
