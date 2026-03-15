using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualManager : MonoBehaviour
{
   [SerializeField] private EnemyAi_Blade bladeEnemy;
    private Animator animator;

    private StateVisual currentStateVisual;


    private void BladeEnemy_OnEnemyVisualChange(object sender, EnemyAi_Blade.OnEnemyVisualChangeArgs e)
    {
        currentStateVisual = e.stateVisual;

        switch (currentStateVisual)
        {
            case StateVisual.Idle:

                break;
            case StateVisual.Patrol:
                animator.SetBool("Patrol", true);
                animator.SetBool("Chase", false);
                animator.SetBool("Attack", false);

                break;
            case StateVisual.Chase:
                animator.SetBool("Patrol", false);
                animator.SetBool("Chase", true);
                animator.SetBool("Attack", false);
                break;
            case StateVisual.Attack:
                animator.SetBool("Patrol", false);
                animator.SetBool("Chase", false);
                animator.SetBool("Attack", true);
                break;
        }   
    }
    private void Awake()
    {
        animator= GetComponent<Animator>();
        bladeEnemy = GetComponentInParent<EnemyAi_Blade>();
        bladeEnemy.OnEnemyVisualChange += BladeEnemy_OnEnemyVisualChange;
    }

  

    private void Update()
    {
       
    }

}
