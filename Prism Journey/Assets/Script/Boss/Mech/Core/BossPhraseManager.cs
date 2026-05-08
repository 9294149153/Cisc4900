using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class BossPhraseManager 
{
    private readonly BossContext context;

    public BossPhraseManager (BossContext context)
    {
        this.context = context;
    }

   //return BossPhrase(Enum) according to the percentage of current Hp and Max Hp
    public BossPhrase EvaluatePhrase()
    {
        if(context== null )
        {
            return BossPhrase.Phase1;
        }


        float hpPercent = 0f;


        if (context.maxHp > 0f)  hpPercent= context.currentHp/ context.maxHp;


        if (hpPercent <= context.remoteConfig.phraseHolderConfig.phase4HpPercent)
            return BossPhrase.Phase4;

        if (hpPercent <= context.remoteConfig.phraseHolderConfig.phase3HpPercent)
            return BossPhrase.Phase3;

        if (hpPercent <= context.remoteConfig.phraseHolderConfig.phase2HpPercent)
            return BossPhrase.Phase2;

        return BossPhrase.Phase1;
    }

  

}
