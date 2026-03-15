using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class IdleState : MonoBehaviour, IEnemyStates
{
    public EnemyState StateType => EnemyState.Idle;
    private EnemyState nextState = EnemyState.Chase ;

    private EnemyContext context;

    private void Awake()
    {
        if(context == null) context = GetComponent<EnemyContext>();
       


    }
    

    public void Enter()
    {
        Debug.Log("Enter Idle");
        context.anim.GetStateForAnimation(StateType);
    }

    public void Exit()
    {
        context .anim.animator.SetBool("Idle",false);
    }

    public void Tick()
    {
       

        //When player inside the range , go to chase state
        if (context.enemyConfig.inChaseRange > context.enemySensor.DisToPlayer)
        {
            context.enemyBrain.ChangeState(nextState);
        }
    }
}
