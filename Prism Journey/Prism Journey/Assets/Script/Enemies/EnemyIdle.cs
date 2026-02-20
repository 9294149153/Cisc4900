using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyIdle : IEnemyState
{

    private float idleTimer;
    private float idleTimeMax = 5f;
    public void EixstState(EnemyStateManager enemy)
    {
      
    }

    public void EnterState(EnemyStateManager enemy)
    {
        idleTimer = 0f;
        Debug.Log(idleTimer);
    }

    public void UpdateState(EnemyStateManager enemy)
    {
       

       
        if(idleTimer >= idleTimeMax)
        {
            enemy.SwitchState(enemy.patrolState);
        }
        else
        {
            idleTimer += 1f * Time.deltaTime;
            Debug.Log(idleTimer);
        }

        
    }
}
