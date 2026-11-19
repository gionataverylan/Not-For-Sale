using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : NetworkBehaviour
{
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float targetRefreshInterval = 0.5f;

    private enum EnemyState { Chase, Attack, Death, Idle }

    private EnemyState currentState = EnemyState.Chase;
    private Transform target;
    private IDamageable targetDamageable;
    private NavMeshAgent agent;
    private EnemyHealth enemyHealth;
    private float attackCooldownCounter;
    private float targetRefreshCounter;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            agent.enabled = false;
            enabled = false;
            return;
        }

        enemyHealth.Died += HandleDeath;
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            enemyHealth.Died -= HandleDeath;
        }
    }

    private void OnDisable()
    {
        enemyHealth.Died -= HandleDeath;
    }

    private void Update()
    {
        if (currentState == EnemyState.Death) return;

        RefreshTarget();

        if (target == null)
        {
            EnterIdle();
            return;
        }

        if (currentState == EnemyState.Idle)
        {
            currentState = EnemyState.Idle;
            agent.isStopped = false;
        }

        switch (currentState)
        {
            case EnemyState.Chase: ChaseState(); break;
            case EnemyState.Attack: AttackState(); break;
        }
    }

    private void RefreshTarget()
    {
        targetRefreshCounter -= Time.deltaTime;

        bool targetLost = target == null || (targetDamageable != null && targetDamageable.IsDead);

        if (targetRefreshCounter > 0f && !targetLost) return;

        targetRefreshCounter = targetRefreshInterval;
        FindNearestPlayer();
    }

    private void FindNearestPlayer()
    {
        target = null;
        targetDamageable = null;
        
        float bestDistance = float.MaxValue;

        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            NetworkObject playerObject = client.PlayerObject;

            if (playerObject == null) continue;
            if (!playerObject.TryGetComponent(out IDamageable damageable)) continue;
            if (damageable.IsDead) continue;

            float distance = Vector3.Distance(transform.position, playerObject.transform.position);

            if(distance < bestDistance)
            {
                bestDistance = distance;
                target = playerObject.transform;
                targetDamageable = damageable;
            }
        }
    }

    private void EnterIdle()
    {
        if(currentState == EnemyState.Idle) return;

        currentState = EnemyState.Idle;
        agent.isStopped = true;
        attackCooldownCounter = attackCooldown;
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

    private void HandleDeath()
    {
        currentState = EnemyState.Death;
        agent.enabled = false;
        NetworkObject.Despawn();
    }
}
