using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleRuntime : BossAttackRuntimeBase
{

    public GameObject telegraphObject;
    public ITelegraph telegraph;

    public GameObject attackObject;
    public IAttackActor attackActor;


  
    public override void Reset()
    {
        base.Reset();

        telegraphObject = null;
        telegraph = null;
        attackObject = null;
        attackActor = null;
    }
}
