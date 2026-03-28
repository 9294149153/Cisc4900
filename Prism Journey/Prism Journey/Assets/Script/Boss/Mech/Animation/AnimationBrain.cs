using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationBrain : MonoBehaviour
{

    private readonly static int[] animations =
    {
        Animator.StringToHash("Idle"),
        Animator.StringToHash("SphereSweepAttack")
    };  
   
    private Animator animator;
    private Animation[] currentAnimation;
    private bool[] layerLock;
    private Action<int> DefaultAnimation;
}

public enum MechBossAnimation
{

    Idle,
    SphereSweepAttack
}
