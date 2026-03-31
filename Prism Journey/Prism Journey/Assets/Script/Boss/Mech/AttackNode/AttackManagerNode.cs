using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManagerNode : Node
{
    private BossContext context;

    private SphereSweepAttackNode sphereSweepNode;
    private BubbleAttackNode bubbleAttackNode;

    private Node currentAttackNode;
    public AttackManagerNode(BossContext context)
    {
        this.context = context;

        sphereSweepNode = new SphereSweepAttackNode(context);
        bubbleAttackNode = new BubbleAttackNode(context);
    }

    public override NodeState Evaluate()
    {
        // if one attack is already chosen and running, continue it
        if (currentAttackNode != null)
        {
            NodeState currentResult = currentAttackNode.Evaluate();

            if (currentResult == NodeState.Running)
                return NodeState.Running;

            if (currentResult == NodeState.Success)
            {
                currentAttackNode = null;
                return NodeState.Success;
            }

            if (currentResult == NodeState.Failure)
            {
                currentAttackNode = null;
                return NodeState.Failure;
            }
        }

        // no current attack → choose next valid one
        Node nextAttack = ChooseNextAttack();

        if (nextAttack == null)
            return NodeState.Failure;

        currentAttackNode = nextAttack;

        return currentAttackNode.Evaluate();
    }
    private Node ChooseNextAttack()
    {
        // Example priority:
        // Sphere first, then Bubble

        if (context.sphereSweepEnabled)
            return sphereSweepNode;

        if (context.bubbleAttackEnable)
            return bubbleAttackNode;

        return null;
    }

}
