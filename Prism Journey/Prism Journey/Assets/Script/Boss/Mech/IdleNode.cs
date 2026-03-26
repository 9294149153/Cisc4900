using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleNode : Node
{
    public override NodeState Evaluate()
    {
        // Boss does nothing for now
        Debug.Log("Boss is Idle");

        // Return running because idle usually continues existing
        return NodeState.Running;
    }
}
