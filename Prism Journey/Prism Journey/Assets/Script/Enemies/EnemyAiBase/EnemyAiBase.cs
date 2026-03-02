

using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAIBase : MonoBehaviour
{

    
    [Header("Refs")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Transform player;

    [Header("Common")]
    [SerializeField] protected float patrolSpeed = 2f;
    [SerializeField] protected float chaseSpeed = 5f;
    [SerializeField] protected float attackRange = 2f;

    [Header("Patrol Zone")]
    [SerializeField] protected float patrolRadius = 6f;
    [SerializeField] protected float patrolPointTolerance = 0.5f;
    [SerializeField] protected float patrolWaitTime = 1f;
    protected Vector3 patrolCenter;

    [Header("ChaseDetection zone")]
    [SerializeField] protected float chaseZoneRadius = 10f;
    [SerializeField] protected bool inChaseRange;

    [Header("AttackDetection zone")]
    [SerializeField] protected float attackZoneRadius = 5f;
    [SerializeField] protected bool inAttackRange;

    protected IEnemyState currentState;



    protected virtual void Update()
    {
        Debug.Log("not calling state");
    }

    protected abstract void InitStates();
    protected abstract void SetInitialState();

    public virtual void SwitchState(IEnemyState next)
    {
        Debug.Log("not have own SwitchState logic");
    }

    // ---- helpers for all enemies ----
    public float DistanceToPlayer()
    {
        if (!player) return Mathf.Infinity;
        return Vector3.Distance(transform.position, player.position);
    }

    public void StopAgent()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void ResumeAgent()
    {
        agent.isStopped = false;
    }

    public virtual void SetChaseRange(bool inRange)
    {

        inChaseRange= inRange;
    }

    public virtual void SetAttackRange(bool inRange)
    {

        inAttackRange = inRange;
    }
    public void SetSpeed(float s) => agent.speed = s;

    // Expose to states safely
    public NavMeshAgent Agent => agent;
    public Transform Player => player;
    public Vector3 PatrolCenter => patrolCenter;
    public float PatrolRadius => patrolRadius;
    public float PatrolPointTolerance => patrolPointTolerance;
    public float PatrolWaitTime => patrolWaitTime;
    public float PatrolSpeed => patrolSpeed;
    public float ChaseSpeed => chaseSpeed;

    public float chaseZoneRaidus => chaseZoneRadius;
    public float AttackZoneRadius=> attackZoneRadius;
    public bool InChaseRange => inChaseRange;

    public bool InAttackRange => inAttackRange;
   
}
