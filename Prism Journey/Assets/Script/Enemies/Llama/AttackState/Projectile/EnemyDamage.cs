using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private IDamageable damage;
    private EnemyContext enemyContext;




    public void Init(EnemyContext refference)
    {
       enemyContext = refference;
    }


   

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (enemyContext != null )
            {
                damageable.TakeDamage(enemyContext.enemyConfig.spitDamage);
                Destroy(gameObject);
            }
            else
            {
                damageable.TakeDamage(10f);
                Destroy(gameObject);
            }
                
        }
    }


}
