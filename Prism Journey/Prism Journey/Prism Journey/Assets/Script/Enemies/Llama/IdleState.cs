using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IEnemyStates
{
    public EnemyState StateType => EnemyState.Idle;

    public void Enter()
    {
        Debug.Log("Enter Idle");
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        
    }
}
