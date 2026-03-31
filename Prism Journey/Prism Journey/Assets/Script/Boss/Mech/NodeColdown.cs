using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class NodeColdown :Node
{
    private float durationTimeMax;
    private float timer;
    private bool isStart;

    public static bool isColdown;
    public NodeColdown(float DuraionTimeMax)
    {
        durationTimeMax=DuraionTimeMax;
        
    }

    public override NodeState Evaluate()
    {

        if (isColdown==true)
        {
            return NodeState.Success;
        }
        // first time entering this node
        if (!isStart)
        {
            isStart = true;
            timer = 0f;
            isColdown= false;
            return NodeState.Running;
        }

               timer += Time.deltaTime;

        //free time have not reach the duration value 
        // node still running
        if (timer < durationTimeMax)
        {
            return NodeState.Running;
        }
       
        isStart = false;
        SetColdown(true);
        timer = 0f;

        return NodeState.Success;
    }

   public static void SetColdown(bool value)
    {
        isColdown = value;
    }
}
