using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : MonoBehaviour, IEnemyStates
{
    public EnemyState StateType => EnemyState.Chase;
    private EnemyState exit = EnemyState.Idle;
    private EnemyState next = EnemyState.ChoseAttack;
    private EnemyContext enemyContext;

    private float toChoseAttackTimer;
   
    private void Awake()
    {
        if (enemyContext == null) enemyContext = GetComponent<EnemyContext>();
    }
    public void Enter()
    {
      
      //  enemyContext.anim.GetStateForAnimation(StateType);
        enemyContext.enemyMotor.SetChaseSpeed(enemyContext.enemyConfig.chaseSpeed);
        enemyContext.enemyMotor.ResumeAgent(enemyContext.player.position);
        toChoseAttackTimer = 0;
    }

    public void Exit()
    {
       // enemyContext.anim.animator.SetBool("Chase", false);
    }

    public void Tick()
    {
        //In the chase range
        if (enemyContext.enemyConfig.outChaseRange > enemyContext.enemySensor.DisToPlayer)
        {
          enemyContext.enemyMotor.FaceTarget(enemyContext.player.position);
          enemyContext.agent.SetDestination(enemyContext.player.position);

            //EnterAttack When time reach
            toChoseAttackTimer+= Time.deltaTime;

            if (toChoseAttackTimer >= enemyContext.enemyConfig.ToChoseAtattackColdownMax)
            {
                enemyContext.enemyBrain.ChangeState(next);
            }
            

        }
        else
        {
            //Out Chase Range
            enemyContext.enemyMotor.StopAgent();
            enemyContext.enemyBrain.ChangeState(exit);

        }
    }
}
