using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
/*
public class EnemyIdle : IEnemyState
{

    private float idleTimer;
    private float idleTimeMax = 2.5f;

    public void Eixst(EnemyStateManager enemy)
    {
        
    }

   

    public void Enter(EnemyStateManager enemy)
    {
        idleTimer = 0f;
        Debug.Log(idleTimer);
        
    }

   


    public void Tick(EnemyStateManager enemy)
    {

        if (idleTimer >= idleTimeMax)
        {
            enemy.SwitchState(enemy.patrolState);
        }
        else
        {
            idleTimer += 1f * Time.deltaTime;
            enemy.enemyVisual.PlayIdle();
            Debug.Log(idleTimer);
        }

    }
        
    
}
 */