using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class SpitAttackState : MonoBehaviour, IEnemyStates
{
    public EnemyState StateType => EnemyState.SpitAttack;
    private EnemyContext enemyContext;

    private bool hasShot;
    private float timer;

    private void Awake()
    {
        if (enemyContext == null)
            enemyContext = GetComponent<EnemyContext>();
    }

    public void Enter()
    {
        Debug.Log("SpitAttackState");

        hasShot = false;
        timer = 0f;

        enemyContext.anim.PlaySpit();
        enemyContext.enemyMotor.StopAgent();
        enemyContext.enemyMotor.FaceTarget(enemyContext.player.position);
    }

    public void Exit()
    {
    }

    public void Tick()
    {
        timer += Time.deltaTime;

        enemyContext.enemyMotor.FaceTarget(enemyContext.player.position);

        if (!hasShot)
        {
            ShootProjectile();
            hasShot = true;
        }

        if (timer >= enemyContext.enemyConfig.spitDuration)
        {
            enemyContext.enemyBrain.ChangeState(EnemyState.Chase);
            return;
        }
    }

    private void ShootProjectile()
    {
        GameObject proj = Instantiate(
            enemyContext.enemyConfig.projectilePrefab,
            enemyContext.projectileSpwanPosition.position,
            enemyContext.projectileSpwanPosition.rotation
        );

        proj.transform.localScale=enemyContext.enemyConfig.attackScale;

        if (proj.TryGetComponent<EnemyDamage>(out var enemyDamage))
        {
            enemyDamage.Init(enemyContext);
        }

        if (proj.TryGetComponent<SphereProjectile>(out var sphereProjectile))
        {
            sphereProjectile.Init(
                enemyContext,
                enemyContext.projectileSpwanPosition.forward,
                enemyContext.player
            );
        }
    }
}
