using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{

    private IEnemyState currentState;

    public EnemyIdle idleState = new EnemyIdle();
    public EnemyPatrol patrolState = new EnemyPatrol();

    //public Transform player;

    [Header("Patrol Zone")]
    public float patrolRadius = 6f ;
    public float patrolPointTolerance = 0.5f; // how close counts as "arrived"
    public float patrolWaitTime = 1f;
    [HideInInspector] public Vector3 patrolCenter;

    public NavMeshAgent agent;

    private void Awake()
    {
        if(!agent) agent=GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        //IdleState
        currentState = idleState;
        currentState.EnterState(this);


        //PatrolState
        patrolCenter=transform.position;



        
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }


    public void SwitchState(IEnemyState state)
    {
        if(state == null)return;
        currentState = state;
        currentState.EnterState(this);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = Application.isPlaying ? patrolCenter : transform.position;
        Gizmos.DrawWireSphere(center, patrolRadius);
    }


}
