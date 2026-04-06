using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossPhrase
{
    Phase1,
    Phase2,
    Phase3,
    Phase4,
}

public enum BossAttackType
{
    None,
    SphereSweepAttack,
    BubbleAttack,
    Attack3,
    Attack4
}

public enum BossDifficulty
{
    Easy,
    Normal,
    Hard
}

public enum AttackPhrase
{
    None,

    Enter,

    StartAnimation,

    WaitAnimation,

    SpawnTelegraph,

    TrackTarget,

    // Fill telegraph / charge warning.
    Fill,

    SpawnAttack,

    // Attack active phase.
    AttackActive,

    // Cleanup temporary objects.
    Cleanup,

    // Finished phase.
    Finished
}