using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{

    private BossContext bossContext;

    private float maxHealth;

    private float currentHP; // boss current health



    public float CurrentHP => currentHP;


    private void Awake()
    {
        if(bossContext == null) bossContext=GetComponent<BossContext>();

    }
    private void Start()
    {
       maxHealth=bossContext.bossConfig.maxHealth; // assing health from universal Config so later can apply remote config stats
        currentHP=maxHealth; // assing health
    }

}