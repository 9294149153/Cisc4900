using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttackRuntimeBase
{

    public AttackPhrase currentPhrase = AttackPhrase.None;
    public float PhraseTimer ;

    public virtual void Reset()
    {
        currentPhrase = AttackPhrase.None;
        PhraseTimer = 0f;
      
    }
}



