using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerAnimationBrain : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private Animator playerAnim;

    [Header("Crossfade Settings")]
    [SerializeField] private float fadeDuration = 0.15f;

    private int currentState;

    private readonly static int[] animations = {

        Animator.StringToHash("Idle"),
        Animator.StringToHash("Run"),
        Animator.StringToHash("Dash"),
        Animator.StringToHash("Base Layer.TakeAttack"),
        Animator.StringToHash("Base Layer.Die")
    };

    

    private void Awake()
    {
        if (playerAnim == null) playerAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayAnimation(animations[0]);// play Idle
    }

    public void PlayIdle()
    {
        PlayAnimation(animations[0]);
    }
    public void PlayRun()
    {
        PlayAnimation(animations[1]);
    }

    public void PlayDash()
    {
        PlayAnimation(animations[2]);
    }
    public void PlayTakeDamage()
    {
        PlayAnimation(animations[3]);
    }

    public void PlayDie()
    {
        PlayAnimation(animations[4]);
    }

    private void PlayAnimation(int newState )
    {
      
        // Stop if Animator is missing
        if (playerAnim == null)
        {
            Debug.LogWarning("Animator is missing.");
            return;
        }

        // Do not replay the same animation again and again
        if (currentState == newState)
            return;

        currentState = newState;

        // Smoothly change animation
        playerAnim.CrossFade(newState, fadeDuration);
    }

}

  
        


public enum PlayerAnimationState
{
    None,
    Idle,
    Run,
    TakeAttack,
    Die
}