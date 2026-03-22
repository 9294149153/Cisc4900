using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAttackNode : Node
{
    private Transform boss;
    private Transform player;

    public ZoneAttackNode(Transform boss, Transform player)
    {
        this.boss = boss;     // store boss position
        this.player = player; // store player reference
    }

    public override NodeState Evaluate()
    {
        Debug.Log("Boss uses Zone Attack"); // print attack type for testing

        Debug.Log("Instantiate zone near boss"); // later you can spawn the zone prefab here

        Debug.Log("Zone slowly tracks player position"); // later you can move zone slowly toward player

        Debug.Log("Player takes damage if inside zone"); // later you can add real damage check

        return NodeState.Success; // attack finished for this test example
    }
}
