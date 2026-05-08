using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSelector
{
    
    private BossContext context;

    public AttackSelector(BossContext context)
    {
        this.context = context;
    }


    public BossAttackType GetAttackFromPoolWithRandom(List<BossAttackOption> attackOption)
    {
        int index = 0;
        if (attackOption != null || attackOption.Count>0)
        {
          index=UnityEngine.Random.Range(0, attackOption.Count);
            return attackOption[index].AttackType; 
        }

        return BossAttackType.None;
    }
}
