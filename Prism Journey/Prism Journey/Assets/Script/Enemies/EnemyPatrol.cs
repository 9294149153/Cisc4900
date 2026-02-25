using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : IEnemyState
{

    private float waitTimer;
    public void EixstState(EnemyStateManager enemy)
    {
        
    }

    public void EnterState(EnemyStateManager enemy)
    {
       waitTimer = 0;
        if (enemy.transform.position != enemy.startPosition)
        {
            enemy.agent.SetDestination(enemy.startPosition);
        }
        SetRandomDestination(enemy);
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.agent == null) return;

        // still walking
        if (enemy.agent.pathPending) return;

        // arrived at destination
        if (enemy.agent.remainingDistance <= enemy.patrolPointTolerance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= enemy.patrolWaitTime)
            {
                waitTimer = 0f;
                SetRandomDestination(enemy);
            }
        }

        //Player touch The alram Zone 
        //Enter Chase State
        if (enemy.playerInAlarmZone == true)
        {
            enemy.SwitchState(enemy.chaseState);
        }
        else
        {

        }
    }


    public void SetRandomDestination(EnemyStateManager enemy)
    {
        Vector3 randomPoint = GetRandomPointInZone(enemy.patrolCenter, enemy.patrolRadius);

        // Snap to NavMesh so agent can walk there
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, enemy.patrolRadius, NavMesh.AllAreas))
        {
            enemy.agent.SetDestination(hit.position);
        }
    }

    private Vector3 GetRandomPointInZone(Vector3 center, float radius)
    {
        // Random point inside a circle (x,z)
        Vector2 r = Random.insideUnitCircle * radius;
        return new Vector3(center.x + r.x, center.y, center.z + r.y);
    }


}
