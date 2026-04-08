using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackPool 
{

    private BossContext context;

    public List<BossAttackType> pool = new List<BossAttackType>();
    public BossAttackPool (BossContext context)
    {
        this.context = context;
    }

    public List<BossAttackOption> GetBossAttackTypesWithPhrase(BossPhrase phrase)
    {
        List<BossAttackOption> attackOption= new List<BossAttackOption>();

        switch (phrase)
        {
            case BossPhrase.Phase1:
                AddSphereIfValid(attackOption);
                break;
            case BossPhrase.Phase2:
                AddSphereIfValid(attackOption);
                AddBubbleIfValid(attackOption);
                break;

            case BossPhrase.Phase3:
                AddSphereIfValid(attackOption);
                AddBubbleIfValid(attackOption);
                AddLaserIfValid(attackOption);
                break;

            case BossPhrase.Phase4:

                break;
        }   


        return attackOption;
    }

    private void AddSphereIfValid(List<BossAttackOption> attacks)
    {
        // check are the config enable this attack , if yes add to the pool if no then ignore 
        if (context.remoteConfig.sphereSweepConfig.enabled)
        {
            attacks.Add(new BossAttackOption(
                BossAttackType.SphereSweepAttack
            ));
        }
    }

    private void AddBubbleIfValid(List<BossAttackOption> attacks)
    {
        // check are the config enable this attack , if yes add to the pool if no then ignore 
        if (context.remoteConfig.bubleAttackConfig.enabled)
        {
            attacks.Add(new BossAttackOption(
                BossAttackType.BubbleAttack
            ));
        }
    }

    private void AddLaserIfValid(List<BossAttackOption>attacks)
    {
        // check are the config enable this attack , if yes add to the pool if no then ignore 
        if (context.remoteConfig.laserAttackConfig.enabled)
        {
            attacks.Add(new BossAttackOption(
                BossAttackType.LaserAttack
            ));
        }
    }

}


