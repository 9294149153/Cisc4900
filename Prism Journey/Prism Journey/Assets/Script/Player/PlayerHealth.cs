using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour,IDamageable
{

    private float maxHealth=100f;
    private float currentHealth;

   

    private void Start()
    {
        currentHealth=maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth-=damage;
            if(currentHealth < 0)
            {
                currentHealth=0;
            }
            Debug.Log(currentHealth);
        }
    }

}
