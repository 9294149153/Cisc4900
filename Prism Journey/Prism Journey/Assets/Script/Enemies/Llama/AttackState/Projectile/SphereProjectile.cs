using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SphereProjectile : MonoBehaviour
{
    private EnemyContext enemyContext;
    private Vector3 direction;
    private Transform playerPosition;
    [SerializeField] private float speed = 10f;
    private float timerMax = 4f;

   
    public void Init(EnemyContext context, Vector3 dir,Transform player)
    {
        enemyContext = context;
        direction = dir.normalized  ;
        playerPosition = player;
       
    }

    private void Update()
    {
      Destroy(gameObject,timerMax);
    }
}

