using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class EnemyChase : IEnemyState
{
    public void Eixst(EnemyStateManager enemy)
    {
       
    }

    public void Enter(EnemyStateManager enemy)
    {
        Debug.Log("Weclome to chase state");
        enemy.enemyVisual.PlayPatrol(false);
    }

    public void Tick(EnemyStateManager enemy)
    {
        if (enemy.playerInAlarmZone)
        {
            enemy.agent.SetDestination(enemy.playerTransform.position);


            //also in attack zone
            if (enemy.playerInAttackZone)
            {
                enemy.SwitchState(enemy.attackState);
            }
        }
        else
        {
            enemy.SwitchState(enemy.patrolState);
        }
       
    }
}
*/