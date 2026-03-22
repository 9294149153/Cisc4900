using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    private List<Node> children;

    public Selector(List<Node> children)
    {
        this.children = children; // store all child nodes
    }

    public override NodeState Evaluate()
    {
        foreach (Node node in children) // check each child one by one
        {
            NodeState result = node.Evaluate(); // run child

            if (result == NodeState.Success)
            {
                return NodeState.Success; // stop early if success
            }
            else if (result == NodeState.Running)
            {
                return NodeState.Running; // still working
            }
        }

        return NodeState.Failure; // none succeeded
    }
}
