using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossContext :MonoBehaviour
{

    // ===== References =====
    public Transform bossTransform;
    public Transform player;
    public BossConfig bossConfig;
    public BossHealth bossHealth;
    public MechBossAnimationBrain mechAnimation;
    public Transform TelegraphSpawnPosition;
    public GameObject rectangleTelegrapgPrefab;
    public GameObject[] sphereAttackPrefab;


    [Header("Sphere Sweep Attack Runtime Values")]
    public bool sphereSweepEnabled = true;

    public float telegraphTrackingDuration;
    public float fillDuration;
    public float telegraphWidth;
    public float telegraphLength;
    public float telegraphTrackingSpeed;

    public float sizeTimerMax;
    public float sphereScaleSpeed;
    public float sphereAttackMoveSpeed;
    public float sphereAnimationDuration;


   /* [Header("SphereSweepAttackRefference")]
    public GameObject rectangleTelegrapgPrefab;
    public GameObject[] sphereAttackPrefab;
   
    public float telegraphTrackingDuration = 4f;
    public float fillDuration = 1.5f;
    public float telegraphWidth = 4f;
    public float telegraphLength = 10f;
    public float telegraphTrackingSpeed;

     [Header("SphereAttackData")]
   
    public float sizeTimerMax = 5f;
    public  float sphereScaleSpeed = 0.8f;
    public float sphereAttackMoveSpeed = 4;
    public float sphereSweepAttackAnimationDurationTime = 3f;*/


    [Header("MechBossAnimationRefference")]
    public Transform mechBossVisual;


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

    public void ApplyLocalConfig(MechBossLocalConfig config)
    {
        if (config == null)
        {
            Debug.LogError("[BossContext] ApplyMechBossLocalConfig failed: config is null.");
            return;
        }

        telegraphTrackingDuration = config.telegraphTrackingDuration;
        fillDuration = config.fillDuration;
        telegraphWidth = config.telegraphWidth;
        telegraphLength = config.telegraphLength;
        telegraphTrackingSpeed = config.telegraphTrackingSpeed;

        sizeTimerMax = config.sizeTimerMax;
        sphereScaleSpeed = config.sphereScaleSpeed;
        sphereAttackMoveSpeed = config.sphereAttackMoveSpeed;
        sphereAnimationDuration = config.sphereSweepAttackAnimationDurationTime;

        Debug.Log($"[BossContext] Local config applied. Version: {config.configVersion}");
    }

   

}
