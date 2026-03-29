using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MechBossLocalConfig", menuName = "Config/MechBoss Local Config")]
public class MechBossLocalConfig : ScriptableObject
{
    [Header("Meta")]
    public string configVersion = "1.0";
    public string bossId = "mech_boss";

    [Header("Sphere Sweep Telegraph")]
    public float telegraphTrackingDuration = 4f;
    public float fillDuration = 1.5f;
    public float telegraphWidth = 4f;
    public float telegraphLength = 10f;
    public float telegraphTrackingSpeed = 8f;

    [Header("Sphere Sweep Attack")]
    public float sizeTimerMax = 5f;
    public float sphereScaleSpeed = 0.8f;
    public float sphereAttackMoveSpeed = 4f;
    public float sphereSweepAttackAnimationDurationTime = 3f;

}
