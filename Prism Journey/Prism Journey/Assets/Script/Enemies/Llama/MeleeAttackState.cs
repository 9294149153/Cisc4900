using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : MonoBehaviour, IEnemyStates
{
    public EnemyState StateType => EnemyState.MeleeAttack;
    private EnemyContext enemyContext;
    public float Timer;


    private void Awake()
    {
        if(enemyContext == null) enemyContext = GetComponent<EnemyContext>();

    }
    public void Enter()
    {
        Debug.Log("MeleeAttack");
        enemyContext.enemyMotor.StopAgent();
        enemyContext.anim.GetStateForAnimation(StateType);
        enemyContext.enemyMotor.FaceTarget(enemyContext.player.position);
        DoMeleeHit();
        Timer = 0f;
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        enemyContext.enemyMotor.StopAgent();
        while (enemyContext.enemyConfig.meleeExitTimerMax > Timer)
        {
            Timer += Time.deltaTime;
            Debug.Log(Timer);
        }
        enemyContext.enemyBrain.ChangeState(EnemyState.Chase);

    }

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

            
        }
    }

}
