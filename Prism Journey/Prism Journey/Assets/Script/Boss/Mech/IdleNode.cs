using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class IdleNode : Node
{

    private bool isIdle = true;
    private float timer;
    private float idleDurationTimerMax=3f;

    //Context Reffernce
    private BossContext context;

    public IdleNode(BossContext context)
    {
        this.context=context;
    }
    public override NodeState Evaluate()
    {
        //First Time enter Idle
        if (isIdle==true)
        {
            isIdle= false;
            timer = 0;
           return NodeState.Running;

        }
       
        //Ensure visual move to the correct position
        if(Vector3.Distance(context.mechBossVisual.position, context.bossTransform.position) > 0.04f)
        {
            context.mechBossVisual.position = Vector3.MoveTowards(context.mechBossVisual.position, context.bossTransform.position, 10f * Time.deltaTime);

            return NodeState.Running;
        }

        //

        //start to count the time before to next  node
        //and set the boss into correct posistion 
        if (timer < idleDurationTimerMax)
        {
            timer += Time.deltaTime;

            return NodeState.Running;
        }



        return NodeState.Failure;
    }
}
