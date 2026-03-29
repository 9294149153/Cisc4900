using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MechBossAnimationBrain : MonoBehaviour
{

    private readonly static int[] animations =
    {
        Animator.StringToHash("Empty"),
        Animator.StringToHash("MechIdle"),
        Animator.StringToHash("SphereSweepAttack")
    };

    [Header("Crossfade Settings")]
    [SerializeField] private float fadeDuration = 0.15f;

    [SerializeField] private Animator animator;
    private int currentState;

    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogError( $"[MechBossAnimationBrain] Animator is NULL on {gameObject.name}",this);

        }

    }

    public void PlayEmpty()
    {
        PlayAnimation(animations[0]);
    }
    public void PlayIdle()
    {
        PlayAnimation(animations[1]);
        
    }

    public void PlayerSphereSweepAttack()
    {
        PlayAnimation(animations[2]);
    }

    private void PlayAnimation(int newState)
    {

        // Stop if Animator is missing
        if (animator == null)
        {
            Debug.LogWarning("Animator is missing.");
            return;
        }

        // Do not replay the same animation again and again
        if (currentState == newState)
            return;

        currentState = newState;

        // Smoothly change animation
        animator.CrossFade(newState, fadeDuration);
    }


   
}





public enum MechBossAnimation
{

    Idle,
    SphereSweepAttack
}
