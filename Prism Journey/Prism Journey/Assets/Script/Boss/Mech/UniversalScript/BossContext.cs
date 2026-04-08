using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class BossContext :MonoBehaviour
{

    // ===== References =====
    public Transform bossTransform;
    public Transform player;
    public BossConfig bossConfig;
    public BossHealth bossHealth;
    public MechBossAnimationBrain mechAnimation;
    public Transform TelegraphSpawnPosition;
    public Transform plane;


    [Header("Boss HP  Stats")]
    public float maxHp = 100f;
    public float currentHp = 50f;


    [Header("Difficulty")]
    public BossDifficulty currentDifficulty = BossDifficulty.Easy;



    [Header("Runtime State")]
    public  BossPhrase currentPhrase = BossPhrase.Phase1;
    public BossAttackType currentAttackType = BossAttackType.None;
    public bool isAttackRunning = false;
    public bool hiddenPhaseEntered = false;


    [Header("Remote Config Runtime")]
    public MechBossRemoteConfig remoteConfig;


    [Header("Attack Data Assets")]
    public SphereAttackData sphereSweepData;
   public BubbleAttackData bubbleAttackData;
   public LaserAttackData laserAttackData;











    [Header("IdleNode")]
    public bool idleEnable = true;

    [Header("Sphere Sweep Attack Runtime Values")]
   

    public float telegraphTrackingDuration;
    public float fillDuration;
    public float telegraphWidth;
    public float telegraphLength;
    public float telegraphTrackingSpeed;

    public float sizeTimerMax;
    public float sphereScaleSpeed;
    public float sphereAttackMoveSpeed;
    public float sphereAnimationDuration;

    //Sphere Attack Prefab 
    public GameObject rectangleTelegrapgPrefab;
    public GameObject[] sphereAttackPrefab;
   
   

    [Header("Bubble Attack Runtime Values")]
   

    //telegraph Data
    public float bubleTelegraphTrackingDuration;
    public float bubleTelegraphfillDuration;
    public float bubleTelegraphtelegraphWidth;
    public float bubleTelegraphtelegraphLength;
    public float bubleTelegraphtelegraphTrackingSpeed;
    public int   bubleSpwanTelegraphAmount=4;

    //BubblePrefab data
    public GameObject[] bubblePrefab;
    public float bubleRadius = 2.6f;
    public float bubbleDamageToBoss = 5f;
    public float bubbleDamageToPlayer = 5f;

    //Animation Data
    public float bubbleAnimationDurationTime=2.6f;

    //Bubble Attack Prefab
    public GameObject sphereTelegraphPrefab;




    [Header("MechBossAnimationRefference")]
    public Transform mechBossVisual;




    [Header("AttackStatsManager")]
    public bool sphereSweepEnabled = true;
    public bool bubbleAttackEnable = true;


    public void ApplyRemoteConfig(MechBossRemoteConfigData data)
    {
        if (data == null)
        {
            Debug.LogError("[MechBossContext] Config data is null.");
            return;
        }

        if (data.attacks == null)
        {
            Debug.LogWarning("[MechBossContext] attacks section is null.");
            return;
        }

        if (data.attacks.sphereSweepAttack == null)
        {
            Debug.LogWarning("[MechBossContext] sphereSweepAttack section is null.");
            return;
        }

        SphereSweepAttackConfig config = data.attacks.sphereSweepAttack;

        sphereSweepEnabled = config.enabled;
        telegraphTrackingDuration = config.telegraphTrackingDuration;
        fillDuration = config.fillDuration;
        telegraphWidth = config.telegraphWidth;
        telegraphLength = config.telegraphLength;
        telegraphTrackingSpeed = config.telegraphTrackingSpeed;
        sizeTimerMax = config.sizeTimerMax;
        sphereScaleSpeed = config.sphereScaleSpeed;
        sphereAttackMoveSpeed = config.sphereAttackMoveSpeed;
        sphereAnimationDuration = config.sphereAnimationDuration;

        Debug.Log("[MechBossContext] Config applied.");
    }

  
   

}
