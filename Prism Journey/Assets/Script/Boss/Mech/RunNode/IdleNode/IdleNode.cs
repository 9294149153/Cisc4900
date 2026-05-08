using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class IdleNode : Node
{
    [Header("Boss Context Reference")]
    private readonly BossContext context;
    private float timer;
    private bool started;


    public IdleNode(BossContext context)
    {
        this.context=context;
    }
    public override NodeState Evaluate()
    {
      
        //First Time enter Idle State
        if (!started)
        {
            started=true;
            timer = 0f;
           return NodeState.Running;

        }

        //start to count the time before to next  node
        timer += Time.deltaTime;
        if (timer < context.remoteConfig.IdleConfig.idleDuration)
        {
            return NodeState.Running;
        }
           
        
        started= false;
        timer= 0;
        Debug.Log("IdleNode work Perfectlt to the end");
        return NodeState.Success;
    }
}
