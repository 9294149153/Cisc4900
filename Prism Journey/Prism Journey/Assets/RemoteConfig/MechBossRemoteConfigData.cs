using System;
using UnityEngine;

[Serializable]
public class MechBossRemoteConfigData
{
    public string configVersion;
    public string bossId;

    public GeneralConfig general;
    public AttackGroupConfig attacks;
}

[Serializable]
public class GeneralConfig
{
    public bool useRemoteConfig;
    public string difficulty;
}

[Serializable]
public class AttackGroupConfig
{
    public SphereSweepAttackConfig sphereSweepAttack;
}

[Serializable]
public class SphereSweepAttackConfig
{
    public bool enabled;

    public float telegraphTrackingDuration;
    public float fillDuration;
    public float telegraphWidth;
    public float telegraphLength;
    public float telegraphTrackingSpeed;

    public float sizeTimerMax;
    public float sphereScaleSpeed;
    public float sphereAttackMoveSpeed;
    public float sphereAnimationDuration;
}
