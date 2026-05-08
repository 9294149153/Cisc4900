using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAttackRuntimeData :BossAttackRuntimeBase
{
    public GameObject TelegraphObject;
    public GameObject AttackObject;

    public ITelegraph Telegraph;
    public IAttackActor AttackActor;

    public Vector3 leftEdge;
    public Vector3 rightEdge;

   

    public override void Reset()
    {
        base.Reset();

        TelegraphObject = null;
        AttackObject = null;
        Telegraph = null;
        AttackActor = null;

        leftEdge= Vector3.zero;
        rightEdge= Vector3.zero;
        Debug.Log("Sphere Rest");

    }
}
