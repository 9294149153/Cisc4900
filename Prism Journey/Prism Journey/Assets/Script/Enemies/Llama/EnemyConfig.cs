using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[System.Serializable]
public class EnemyConfig 
{
    [Header("Chase Detection Stats")]
    public float inChaseRange =10f;
    public float outChaseRange=12f;
    public float chaseSpeed = 5f;
    public float ToChoseAtattackColdownMax = 0.5f;


    [Header("AttackConfigStats")]
    public float meleeRangeRadius = 3f;
    public float RrangeRaidus = 6f;


    [Header("MeleeConfigStats")]
   
    public float meleeExitTimerMax = 0.5f;
    public float meleeDamage=10f;

    [Header("Melee Hitbox")]
    public Vector3 boxHalfExtents = new Vector3(1f, 1f, 1f);
    public Vector3 boxOffset = new Vector3(0f, 1f, 1.5f);
    public LayerMask targetLayer;

    [Header("RollConfigStats")]
    public float rollingRadius = 1.3f;
    public float rollingDamage = 15f;
    public float rollingDuration = 6f;
    public float rollingSpeed = 6f;

    [Header("SpitConfigStats")]
    public float spitDamage = 20f;
    public float spitDuration = 0.5f;
    public GameObject projectilePrefab;
    public Vector3 attackScale =  new Vector3(1, 1, 1);


}
