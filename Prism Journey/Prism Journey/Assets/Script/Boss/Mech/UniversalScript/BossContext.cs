using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossContext :MonoBehaviour
{

    // ===== References =====
    public Transform bossTransform;
    public Transform player;
    //public Animator Animator { get; private set; }
    public BossConfig bossConfig;
    public BossHealth bossHealth;
    

    [Header("SphereSweepAttackRefference")]
    public GameObject rectangleTelegrapgPrefab;
    public GameObject sphereAttackPrefab;
   
    public float telegraphTrackingDuration = 4f;
    public float fillDuration = 1.5f;
    public float telegraphWidth = 4f;
    public float telegraphLength = 10f;
    public float sphereSweepAttacktrackingSpeed;

    [Header("SphereAttackData")]
   
    public float sizeTimerMax = 5f;
    public  float sphereScalespeed = 0.8f;
    public float sphereAttackMoveSpeed = 4;



}
