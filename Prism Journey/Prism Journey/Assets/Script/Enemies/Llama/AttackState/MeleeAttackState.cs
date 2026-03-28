using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : MonoBehaviour, IEnemyStates
{
    public EnemyState StateType => EnemyState.MeleeAttack;
    private EnemyContext enemyContext;
    private float timer;


    private void Awake()
    {
        if(enemyContext == null) enemyContext = GetComponent<EnemyContext>();

    }
    public void Enter()
    {
        
        enemyContext.enemyMotor.StopAgent();
        enemyContext.anim.GetStateForAnimation(StateType);
        enemyContext.enemyMotor.FaceTarget(enemyContext.player.position);
        DoMeleeHit();
        timer = 0f;
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        enemyContext.enemyMotor.StopAgent();
        while (enemyContext.enemyConfig.meleeExitTimerMax > timer)
        {
            timer += Time.deltaTime;
            
        }
        enemyContext.enemyBrain.ChangeState(EnemyState.Chase);

    }

    //Will only take Check Layer = Player
    private void DoMeleeHit()
    {
        Vector3 center = transform.position + transform.forward * enemyContext.enemyConfig.boxOffset.z
                                           + transform.right * enemyContext.enemyConfig.boxOffset.x
                                           + transform.up * enemyContext.enemyConfig.boxOffset.y;

        Collider[] hits = Physics.OverlapBox(
            center,
           enemyContext.enemyConfig. boxHalfExtents,
            transform.rotation,
            enemyContext.enemyConfig.targetLayer
        );

        foreach (Collider hit in hits)
        {
            Debug.Log("Hit: " + hit.name);
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(enemyContext.enemyConfig.meleeDamage);
            }


        }
    }

    // Visual Size the hitbox
    private void OnDrawGizmosSelected()
    {
        if (enemyContext == null || enemyContext.enemyConfig == null) return;

        Vector3 center = transform.position
                       + transform.forward * enemyContext.enemyConfig.boxOffset.z
                       + transform.right * enemyContext.enemyConfig.boxOffset.x
                       + transform.up * enemyContext.enemyConfig.boxOffset.y;

        Gizmos.color = Color.red;

        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);

        Gizmos.DrawWireCube(Vector3.zero, enemyContext.enemyConfig.boxHalfExtents * 2f);

        Gizmos.matrix = oldMatrix;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, enemyContext.enemyConfig.meleeRangeRadius);
    }

}
