using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : IEnemyState
{
    public void EixstState(EnemyStateManager enemy)
    {
        
    }

    public void EnterState(EnemyStateManager enemy)
    {
        enemy.agent.speed = 4f;
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        //Player  in  the alram range
        if(enemy.playerInAlarmZone == true)
        {
            enemy.agent.SetDestination(enemy.playerTransform.position);

            //if Player in Attack Range
            if (enemy.playerInAttackZone == true)
            {
                enemy.SwitchState(enemy.attackState);
            }
            else
            {
                //not in attack range
                
            }
        }
        else
        {
            enemy.SwitchState(enemy.patrolState);
            return;
        }
             


        


    }
}
