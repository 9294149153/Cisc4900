using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAttackState : MonoBehaviour, IEnemyStates
{
    public EnemyState StateType => EnemyState.RollAttack;
    private EnemyContext enemyContext;
    private float timer;
    private bool hitPlayer;
    private void Awake()
    {
        if (enemyContext == null) enemyContext = GetComponent<EnemyContext>();

    }

    public void Enter()
    {
        hitPlayer = false;
        timer = 0;
        enemyContext.enemyMotor.StopAgent();
      //  enemyContext.anim.GetStateForAnimation(StateType);
        enemyContext.enemyMotor.FaceTarget(enemyContext.player.position);
        enemyContext.enemyMotor.SetChaseSpeed(enemyContext.enemyConfig.rollingSpeed);
    }

    public void Exit()
    {
       // enemyContext.anim.animator.SetBool("Rolling", false);
        hitPlayer = false;
    }

    public void Tick()
    {
        if (hitPlayer == false && timer<=enemyContext.enemyConfig.rollingDuration)
        {
            
                enemyContext.enemyMotor.ResumeAgent(enemyContext.player.position);
                enemyContext.enemyMotor.FaceTarget(enemyContext.player.position);
                timer += Time.deltaTime;
                DoRollingHit();
            

        }
        else
        {
            enemyContext.enemyBrain.ChangeState(EnemyState.Chase);
        }
    }


    private void DoRollingHit()
    {
        Vector3 center = transform.position;

        Collider[] hits = Physics.OverlapSphere(
       center,
       enemyContext.enemyConfig.rollingRadius,
       enemyContext.enemyConfig.targetLayer
       );



        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(enemyContext.enemyConfig.rollingDamage);
                hitPlayer = true;

            }

        }


    }
    private void OnDrawGizmosSelected()
    {
        if (enemyContext == null || enemyContext.enemyConfig == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            enemyContext.enemyConfig.rollingRadius
        );

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyContext.enemyConfig.RrangeRaidus);
    }
}