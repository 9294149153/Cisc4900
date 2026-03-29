using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBossHPRange : Node
{
  
    private BossContext context;
    private float maxHP;
    private float minHP;

    public CheckBossHPRange(BossContext context,float maxHP, float minHP)
    {
        this.context = context;
        this. maxHP = maxHP;// upper bound
        this. minHP = minHP;// lower bound

    }

    public override NodeState Evaluate()
    {
        float hp = context.bossHealth.CurrentHP; // read current hp
        

        if (hp <= maxHP && hp > minHP)
        { 
            return NodeState.Success; // hp is inside this phase range
        }

        return NodeState.Failure; // hp not in this phase range
    }
}
