using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MechBossRemoteConfig 
{
    public string version = "1.1";


    public IdleConfig IdleConfig = new IdleConfig();
    public SphereSweepConfig sphereSweepConfig= new SphereSweepConfig();
    public BubbleAttackConfig bubleAttackConfig= new BubbleAttackConfig();
    public LaserAttackConfig laserAttackConfig= new LaserAttackConfig();
    public PhraseHolderConfig phraseHolderConfig= new PhraseHolderConfig();
    public DifficultyConfig difficultyConfig = new DifficultyConfig();
   
}
[Serializable]

public  class PhraseHolderConfig
{
    public float phase2HpPercent = 0.75f;
    public float phase3HpPercent = 0.50f;
    public float phase4HpPercent = 0.25f;
    public float hiddenPhaseHpPercent = 0.10f;
}

[Serializable]
public class DifficultyConfig
{
    public bool easyEnableHiddenPhase = false;
    public bool normalEnableHiddenPhase = false;
    public bool hardEnableHiddenPhase = true;
}
[Serializable]
public class IdleConfig
{
    
    public float idleDuration = 5f;
}
[Serializable]
public class SphereSweepConfig
{
    public bool enabled = true;
    public float animationDuration = 4f;
    public float trackingDuration = 3.5f;
    public float trackingSpeed = 1.5f;
    public float fillDuration = 2f;
    public int   spawnAmount = 1;
    public float telegraphWidth = 2f;
    public float telegraphLength = 10f;
    public float sphereScaleDuration = 1;
    public float sphereScaleSpeed = 8f;
    public float moveSpeed = 30f;
    public float actorHeightOffset = 1.5f;
}

[Serializable]
public class BubbleAttackConfig
{
    public bool enabled = true;
    public float animationDuration = 0.8f;
    public float trackingDuration = 5f;
    public float trackingSpeed = 15f;
    public float fillDuration = 1.0f;
    public float telegraphWidth = 5f;
    public float telegraphLength = 5f;
    public float bubbleSpawnHeight = 8f;
    public float bubbleRadius = 2f;
    public float bubbleAttackDuration = 0.5f;
    public float fallSpeed = 9f;
    public float damageDeal = 1f;
    public int spwanAmount = 5;
}

public class LaserAttackConfig
{

    public bool enabled = true;
    public float animationDuration = 1f;
    public float trackingDuration = 15f;
    public float trackingSpeed = 15f;
    public float fillDuration = 1.0f;
    public float telegraphWidth = 8f;
    public float telegraphLength = 8f;
    public float attackDuration =0.5f;
    public float fallSpeed = 9f;
    public float damageDeal = 2f;
    public int spwanAmount = 5;

}