using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{

    private IEnemyState currentState;

    public EnemyIdle idleState = new EnemyIdle();
    public EnemyPatrol patrolState = new EnemyPatrol();
    public EnemyChase chaseState = new EnemyChase();
    public EnemyAttack attackState = new EnemyAttack(); 

    //public Transform player;

    [Header("Patrol Zone")]
    public float patrolRadius = 6f ;
    public float patrolPointTolerance = 0.5f; // how close counts as "arrived"
    public Vector3 startPosition;
    public float patrolWaitTime = 0.5f;
    [HideInInspector] public Vector3 patrolCenter;


    [Header("Alarm Zone")]
    public bool playerInAlarmZone;
    public Transform playerTransform;


    [Header("Attack")]
    public bool playerInAttackZone;
    public GameObject attackPrefabs;
    public Transform attackSpawnPoint;



    public NavMeshAgent agent;

    private void Awake()
    {
        if(!agent) agent=GetComponent<NavMeshAgent>();

        agent.updateRotation = true;
       agent.angularSpeed = 360f;
    }
    private void Start()
    {


        //IdleState
        currentState = idleState;
        currentState.EnterState(this);


        //PatrolState
        patrolCenter=transform.position;
        startPosition=transform.position;





        
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
