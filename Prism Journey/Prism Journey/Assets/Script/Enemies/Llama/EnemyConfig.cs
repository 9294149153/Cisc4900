using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyConfig 
{
    [Header("Chase Detection Stats")]
    public float inChaseRange =8f;
    public float outChaseRange=12f;
    public float chaseSpeed = 4.5f;
    public float ToChoseAtattackColdownMax = 0.5f;



    [Header("MeleeConfigStats")]
    public float meleeRangeRadius = 3f;
    public float meleeExitTimerMax = 0.5f;

    [Header("Melee Hitbox")]
    public Vector3 boxHalfExtents = new Vector3(1f, 1f, 1f);
    public Vector3 boxOffset = new Vector3(0f, 1f, 1.5f);
    public LayerMask targetLayer;
    
}
