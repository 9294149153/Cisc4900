using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*
public class EnemyAttack : IEnemyState
{

    private float nextAttackTimer;

    public void Eixst(EnemyStateManager enemy)
    {
       
    }

    public void Enter(EnemyStateManager enemy)
    {
        nextAttackTimer = 0;
        enemy.attackSpawnPoint = enemy.playerTransform;
        enemy.agent.isStopped = true;   // pauses movement
        enemy.agent.ResetPath(); // clears the current path/destination
        enemy.attack.Perform(enemy);
    }

    public void Tick(EnemyStateManager enemy)
    {
        //Player not in the attack zone 
        if (enemy.playerInAttackZone == false)
        {
            enemy.SwitchState(enemy.chaseState);

        }
        else
        {
            //player  in attack zone 
            //and the attackColdtime reach the max
            if (nextAttackTimer >= enemy.attackColdownMax)
            {
                enemy.attackSpawnPoint = enemy.playerTransform ;
                enemy.agent.isStopped = true;   
                enemy.agent.ResetPath();
                enemy.attack.Perform(enemy);
                nextAttackTimer = 0;
            }
            else
            {
                nextAttackTimer += Time.deltaTime;
               // enemy.agent.SetDestination(enemy.playerTransform.position);
            }
        }
    }
}
*/