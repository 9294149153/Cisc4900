using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss Attacks/Bubble Attack Data")]
public class BubbleAttackData : ScriptableObject
{
    [Header("Animation")]

    public float animationDuration = 2.0f;

    [Header("Telegraph")]

    public GameObject telegraphPrefab;

 

    // Telegraph width.
    public float telegraphWidth = 5f;

    public float telegraphLength = 5f;

    public float trackingDuration = 1.0f;

    public float trackingSpeed = 8f;

    public float chargeDuration = 1.0f;


    [Header("Projectile")]

    public GameObject[] actorPrefab;

    public float damageDeal = 5f;
    public float moveSpeed = 25f;

    public float reachThreshold = 0.05f;

    [Header("TeleGraph  && Projectile Share")]
    public int spawnAmount = 5;

    [Header("Offsets")]

    // Vertical spawn offset.
    public float actorHeightOffset = 1.5f;
}
