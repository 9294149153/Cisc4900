using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MechBossLocalConfig", menuName = "Boss/Mech Boss Local Config")]
public class MechBossLocalConfigSO : ScriptableObject
{
    [Header("General")]
    public string configVersion = "1.0";
    public string bossId = "mech_boss";
    public bool useRemoteConfig = true;
    public string difficulty = "normal";

    [Header("Sphere Sweep Attack")]
    public bool sphereSweepEnabled = true;
    public float telegraphTrackingDuration = 3.5f;
    public float fillDuration = 1.2f;
    public float telegraphWidth = 5.0f;
    public float telegraphLength = 12.0f;
    public float telegraphTrackingSpeed = 10.0f;
    public float sizeTimerMax = 4.5f;
    public float sphereScaleSpeed = 1.1f;
    public float sphereAttackMoveSpeed = 6.0f;
    public float sphereAnimationDuration = 2.5f;

    public MechBossRemoteConfigData ToRuntimeData()
    {
        return new MechBossRemoteConfigData
        {
            configVersion = configVersion,
            bossId = bossId,
            general = new GeneralConfig
            {
                useRemoteConfig = useRemoteConfig,
                difficulty = difficulty
            },
            attacks = new AttackGroupConfig
            {
                sphereSweepAttack = new SphereSweepAttackConfig
                {
                    enabled = sphereSweepEnabled,
                    telegraphTrackingDuration = telegraphTrackingDuration,
                    fillDuration = fillDuration,
                    telegraphWidth = telegraphWidth,
                    telegraphLength = telegraphLength,
                    telegraphTrackingSpeed = telegraphTrackingSpeed,
                    sizeTimerMax = sizeTimerMax,
                    sphereScaleSpeed = sphereScaleSpeed,
                    sphereAttackMoveSpeed = sphereAttackMoveSpeed,
                    sphereAnimationDuration = sphereAnimationDuration
                }
            }
        };
    }
}