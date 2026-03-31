using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyContext : MonoBehaviour
{
    public Transform player;
    public Transform projectileSpwanPosition;
    public NavMeshAgent agent;
    public AnimationBridge anim;
    public EnemyBrain enemyBrain;
    public EnemySensor enemySensor;
    public EnemyMotor enemyMotor;
    public EnemyConfig enemyConfig;//Not monoBehavior
    public EnemyAttackConfig enemyAttackConfig;//not monoBehavior
  

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyMotor = GetComponent<EnemyMotor>();
        enemyBrain = GetComponent<EnemyBrain>();
        enemySensor = GetComponent<EnemySensor>();

    }
}
