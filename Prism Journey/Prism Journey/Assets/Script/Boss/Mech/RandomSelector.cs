using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : Node
{
    private List<Node> children;

    public RandomSelector(List<Node> children)
    {
        this.children = children; // store all possible random child nodes
    }

    public override NodeState Evaluate()
    {
        if (children == null || children.Count == 0)
            return NodeState.Failure; // fail if no child exists

        int randomIndex = Random.Range(0, children.Count); // pick one random child
        return children[randomIndex].Evaluate(); // run the random child
    }
}
