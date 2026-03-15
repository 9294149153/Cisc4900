using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoseAttackState : MonoBehaviour , IEnemyStates
{
    private EnemyContext enemyContext;

    public EnemyState StateType => EnemyState.ChoseAttack;

    [SerializeField]private List<EnemyState> attackStates;
    private void Awake()
    {
        if (enemyContext == null) enemyContext = GetComponent<EnemyContext>();
        SetupAttacks();
    }

    public void Enter()
    {
        Debug.Log("ChoseAttackState");
        enemyContext.anim.GetStateForAnimation(StateType);
        enemyContext.enemyMotor.StopAgent();

        // attackStates List empty
        if (attackStates.Count == 0)
        {
            enemyContext.enemyBrain.ChangeState(EnemyState.Chase);
            
        }

       

    }

    public void Exit()
    {
        enemyContext.anim.animator.SetBool("ChoseAttack", false);
    }

    public void Tick()
    {
        //EnemyState next = attackStates[Random.Range(0, attackStates.Count)];
        //enemyContext.enemyBrain.ChangeState(next);

        if (attackStates.Count > 0)
        {
            if (enemyContext.enemyAttackConfig.hasMelee && enemyContext.enemySensor.DisToPlayer <= enemyContext.enemyConfig.meleeRangeRadius)
            {
                enemyContext.enemyBrain.ChangeState(EnemyState.MeleeAttack);
            }
            else
            {
                enemyContext.enemyBrain.ChangeState(EnemyState.Chase);
            }
            

            

           
        }
        
    }

    void SetupAttacks()
    {
        attackStates = new List<EnemyState>();

        if (enemyContext.enemyAttackConfig.hasMelee)
            attackStates.Add(EnemyState.MeleeAttack);

        if (enemyContext.enemyAttackConfig.hasSpit)
            attackStates.Add(EnemyState.SpitAttack);

        if (enemyContext.enemyAttackConfig.hasRoll)
            attackStates.Add(EnemyState.RollAttack);

        if (enemyContext.enemyAttackConfig.hasBounce)
            attackStates.Add(EnemyState.BounceAttack);
    }

}
