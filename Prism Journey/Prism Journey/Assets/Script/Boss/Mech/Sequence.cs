using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    private List<Node> children;

    public Sequence(List<Node> children)
    {
        this.children = children; // store child nodes
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in children)
        {
            NodeState result = node.Evaluate(); // run child

            if (result == NodeState.Failure)
            {
                return NodeState.Failure; // stop if one fails
            }
            else if (result == NodeState.Running)
            {
                return NodeState.Running; // still executing
            }
        }

        return NodeState.Success; // all succeeded
    }
}
