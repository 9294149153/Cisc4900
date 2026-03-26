using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossHealth : MonoBehaviour
{



    private float maxHealth=100;

    private float currentHP; // boss current health



    public float CurrentHP => currentHP;

    private void Start()
    {
        currentHP=maxHealth; // assing health
    }

    private void Update()
    {
        float testminHp = 50f;
        if (currentHP >= testminHp)
        {
            currentHP-=Time.deltaTime *0.8f;
           
        }
       //Debug.Log(currentHP);
    }

}