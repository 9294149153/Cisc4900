using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss Attacks/Sphere Sweep Attack Data")]
public class SphereAttackData : ScriptableObject
{
    [Header("Animation")]

    public float animationDuration = 1.0f;

    [Header("Telegraph")]

    public GameObject telegraphPrefab;

    // Telegraph width.
    public float telegraphWidth =1f;

    public float telegraphLength = 10f;

    public float trackingDuration = 1.0f;

    public float trackingSpeed = 8f;

    public float chargeDuration = 1.0f;

    [Header("Projectile")]

    public GameObject[] sphereActorPrefab;

    public float actorScaleDuration = 0.5f;

    public float actorScaleSpeed = 8f;

    public float moveSpeed = 25f;

    public float reachThreshold = 0.05f;


    [Header("TeleGraph  && Projectile Share")]
    public int spawnAmount = 1;

    [Header("Offsets")]

    // Vertical spawn offset.
    public float actorHeightOffset = 1.5f;
}
