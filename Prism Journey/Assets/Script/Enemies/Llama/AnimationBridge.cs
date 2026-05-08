using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBridge : MonoBehaviour
{
   public Animator animator;
   private EnemyState enemyState;

    private readonly static int[] animations =
    {
        Animator.StringToHash("Empty"),
        Animator.StringToHash("Idle"),
        Animator.StringToHash("Chase"),
        Animator.StringToHash("Melee Attack"),
        Animator.StringToHash("Spit Attack"),
        Animator.StringToHash("Bounce Attack"),
        Animator.StringToHash("Roll Attack"),


    };

    [SerializeField] private float fadeDuration = 0.15f;
    private int currentState;

    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogError($"[AnimationBride] Animator is NULL on {gameObject.name}", this);

        }

    }
    private void Start()
    {
        PlayIdle();
    }


    public void PlayIdle()
    {
        PlayAnimation(animations[1]);
    }

    public void PlayChase()
    {
        PlayAnimation(animations[2]);
    }
    public void PlayMelee()
    {
        PlayAnimation(animations[3]);
    }

    public void PlaySpit()
    {
        PlayAnimation(animations[4]);
    }
    public void PlayBounce()
    {
        PlayAnimation(animations[5]);
    }

    public void PlayRoll()
    {
        PlayAnimation(animations[6]);
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
