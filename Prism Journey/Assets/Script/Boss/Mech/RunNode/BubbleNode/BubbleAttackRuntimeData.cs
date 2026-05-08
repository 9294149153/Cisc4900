using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAttackRuntimeData : BossAttackRuntimeBase
{
    public List<GameObject> TelegraphObject;
    public List<GameObject>AttackObject;

    public List<ITelegraph>  Telegraph;
    public List<IAttackActor> AttackActor;

    public List<Vector3> targetPosition;

    public override void Reset()
    {
        base.Reset();

        TelegraphObject.Clear();
        AttackObject.Clear();
        Telegraph .Clear();
        AttackActor.Clear();
        targetPosition.Clear();

    Debug.Log("Bubble Runtime Data  Rest");

    }

    public void InitData()
    {
        Telegraph = new List<ITelegraph>();
        AttackActor = new List<IAttackActor>();
        targetPosition= new List<Vector3>();
        TelegraphObject = new List<GameObject>();
        AttackObject = new List<GameObject>();
    }
}
