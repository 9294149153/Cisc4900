using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAttack : IEnemyState
{

    private float returnChaseStateColdown = 3f;
    private float returnChaseStateTimeCount;
    public void EixstState(EnemyStateManager enemy)
    {
      
    }

    public void EnterState(EnemyStateManager enemy)
    {
       
         Debug.Log("on attack state");
        returnChaseStateTimeCount = 0f;

        if (enemy.agent != null)
        {
            enemy.agent.isStopped = true;
            enemy.agent.ResetPath();
        }

        if (enemy.attackPrefabs != null && enemy.attackSpawnPoint != null)
        {
            GameObject obj = GameObject.Instantiate(
                enemy.attackPrefabs,
                enemy.attackSpawnPoint.position,
                Quaternion.Euler(90f, enemy.transform.eulerAngles.y, 0f)
            );

            GameObject.Destroy(obj, 2f);
        }
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if( returnChaseStateTimeCount>returnChaseStateColdown)
        {
            enemy.SwitchState(enemy.chaseState);
        }
        else
        {
            returnChaseStateTimeCount += Time.deltaTime;
        }
    }
}
