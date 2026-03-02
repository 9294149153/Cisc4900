using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

/*
public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] private EnemysConfig enemyConfig;

    private IEnemyState currentState;

    public IEnemyState idleState { get; private set; } = new EnemyIdle();
    public IEnemyState patrolState { get; private set; } = new EnemyPatrol();
    public EnemyChase chaseState { get; private set; } = new EnemyChase();
     public EnemyAttack attackState { get; private set; } = new EnemyAttack();

    
   

    //Patrol
    public float patrolSpeed => enemyConfig != null ? enemyConfig.patrolSpeed :4f;
    public float patrolPointTolerance=>enemyConfig!=null ? enemyConfig.patrolPointTolerance : 1f;
    public float patrolRadius => enemyConfig != null ? enemyConfig.patrolRadius : 9f;

    public float patrolWaitTime => enemyConfig != null ? enemyConfig.patrolWaitTime : 0.3f;

    [HideInInspector]  public Vector3 patrolCenter;



    //Chase 
    public float ChaseSpeed => enemyConfig != null ? enemyConfig.chaseSpeed : 5f;


    //Attack
    public float AttackRange => enemyConfig != null ? enemyConfig.attackRange :4f;
    public float attackColdownMax => enemyConfig != null ? enemyConfig.attackCooldownMax : 1.2f;
    public AttackDefinition attack => enemyConfig != null ? enemyConfig.attack : null;

    [Header("Attack")]
    public bool playerInAttackZone;
    public Transform attackSpawnPoint;



    //AlramDetection Condition
    public bool playerInAlarmZone { get; private set; }
    public Transform playerTransform{ get; private set; }
    



    //Visual
    public VisualManager enemyVisual { get; private set; }




    



    public NavMeshAgent agent;

    private void Awake()
    {
        if(!agent) agent=GetComponent<NavMeshAgent>();
        enemyVisual = GetComponentInChildren<VisualManager>();

        agent.updateRotation = true;
        agent.angularSpeed = 360f;
    }
    private void Start()
    {


        //First time state bind 
        SwitchState(idleState);


        //PatrolState
        patrolCenter =transform.position;

    }

    private void Update()
    {
        currentState.Tick(this);


    }
    public void SwitchState(IEnemyState state)
    {
        if(state == null)return;
        currentState = state;
        currentState.Enter(this);
    }





    public void PlayerInAlramZone(int isTrigger)
    {
        if(isTrigger == 1)
        {
            playerInAlarmZone = true;

        }
        else
        {
            playerInAlarmZone = false;
        }

      
    }
    public void AssignPlayerTransform(Transform player)
    {
        playerTransform = player;
    }


    //Visual the patrol zone range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = Application.isPlaying ? patrolCenter : transform.position;
        Gizmos.DrawWireSphere(center, patrolRadius);
    }


}
*/