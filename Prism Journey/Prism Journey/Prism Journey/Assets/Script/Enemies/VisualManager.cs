using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{

   

    [SerializeField]private Animator animator;

    

    private void Start()
    {
        
    }
    public void PlayIdleAnimation()
    {
        animator.SetTrigger("Idle");
    }


    public void PlayIdle() => animator.SetTrigger("Idle");
    public void PlayPatrol(bool inPatrol)
    {
        animator.SetBool("Patrol", inPatrol);
    }
}
