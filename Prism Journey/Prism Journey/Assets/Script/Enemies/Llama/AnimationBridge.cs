using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBridge : MonoBehaviour
{
   public Animator animator;
   private EnemyState enemyState;
  
   


    private void Awake()
    {
        if(animator == null) animator = GetComponent<Animator>();


    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                animator.SetBool("Idle", true);
             


                break;
            case EnemyState.Chase:
                
                animator.SetBool("Chase", true);
               
                break;
            case EnemyState.ChoseAttack:
                animator.SetBool("ChoseAttack", true);
                
                break;

            case EnemyState.MeleeAttack:
                animator.SetTrigger("Melee");
               
                break;

            case EnemyState.RollAttack:
                animator.SetBool("Rolling",true);

                break;

            case EnemyState.SpitAttack:
                animator.SetBool("Spit", true);
                break;


        }



    }

    public void GetStateForAnimation(EnemyState state)
    {
        enemyState = state;
    }


}
