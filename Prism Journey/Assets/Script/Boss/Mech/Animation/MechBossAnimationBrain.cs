using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public enum BossAnimationStage
{
    None,
    Idle,
    Attack
}
public class MechBossAnimationBrain : MonoBehaviour
{


    [SerializeField] private Animator animator;
    private BossAnimationStage currentState;

    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogError($"[MechBossAnimationBrain] Animator is NULL on {gameObject.name}", this);

        }
        currentState = BossAnimationStage.None;
    }

    private void Update()
    {
        switch (currentState)
        {
            case BossAnimationStage.None:
                // Not Play Animation on this stage
                break;

        case BossAnimationStage.Idle:
                // PLayer After the Attack Aniamtion of the Boss
                animator.SetBool("Idle", true);
                break;
        case BossAnimationStage.Attack:
                //PLay while  enter the attack Node
                animator.SetBool("Idle", false);
                animator.SetTrigger("Attack");
               
                break;
        }
    }

    public void SetCurrentAniamitonStage(BossAnimationStage aniamtionStage)
    {
        if (animator == null)
        {
            Debug.LogError($"[MechBossAnimationBrain]+ Animator Component are missing | animator={animator}",this);
        }

        if (currentState == aniamtionStage) return;
        
        currentState = aniamtionStage;
    }
}
