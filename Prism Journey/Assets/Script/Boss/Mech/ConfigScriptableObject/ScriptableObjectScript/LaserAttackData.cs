using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss Attacks/Laser Attack Data")]
public class LaserAttackData : ScriptableObject
{

    public bool enabled = true;

    [Header("Animation")]

    public float animationDuration = 0.8f;

    [Header("Telegraph")]

    public GameObject telegraphPrefab;

    // Telegraph width.
    public float telegraphWidth = 1f;
    public float telegraphLength = 10f;
    public float trackingDuration = 5f;
    public float trackingSpeed = 15f;
    public float fillDuration = 1.0f;

    [Header("Projectile")]
    public GameObject[] laserActorPrefab;
    public float attackSize = 0.5f;
    public float damageDeal = 5f;
    public int spwanAmount = 5;

}
  