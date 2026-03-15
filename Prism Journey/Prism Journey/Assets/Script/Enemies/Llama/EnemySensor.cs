using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    private EnemyContext enemyContext;
    private float distanceToPlayer;



    public float DisToPlayer => distanceToPlayer;


    private void Awake()
    {
        if(enemyContext == null)enemyContext = GetComponent<EnemyContext>();
    }


    private void Update()
    {
        DistanceToPlayer();
    }

    void DistanceToPlayer()
    {
       Vector3 local = transform.position;
        Vector3 player = enemyContext.player.transform.position;
        local.y = 0;
        player.y= 0;
        distanceToPlayer = Vector3.Distance(local, player);
    }

   public bool InDisRange(float dis)
    {
        return dis >= distanceToPlayer;
    }
}
