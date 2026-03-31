using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BubbleControl : MonoBehaviour
{
    private PlayerHealth playerHealth;
    [SerializeField] private Collider sphCollider;
    [SerializeField] private ColorIdentity color;
    [Inspectable] private BossContext context;

    IDamageable target;

    private Vector3 targetPostion;

    private float moveSpeed;
    private bool reachTarget=false;

    private float destroyTimer;
  


    private void Awake()
    {
       if(sphCollider==null) sphCollider = GetComponent<Collider>();

       sphCollider.isTrigger=true;

        playerHealth = FindObjectOfType<PlayerHealth>();

    }

    private void Start()
    {
        destroyTimer = 0f;
    }
    private void Update()
    {

        if (reachTarget==false){

            transform.position = Vector3.Lerp(transform.position, targetPostion, moveSpeed*Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPostion) < 0.05f)
            {
                reachTarget = true;
               
            }
        }
        OnDestroy();

    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            PlayerColor player = other.GetComponentInParent<PlayerColor>();
            if (player != null)
            {
                // Deal damage to boss if player has the same color as this object
                if(player.GetCurrentColorIdentity() == color)
                {
                    context.bossHealth.TakeDamage(context.bubbleDamageToBoss);
                    Destroy(gameObject);
                }
                else
                {
                    // Play has different color then this deal damage to player
                     target = other.GetComponentInParent<IDamageable>();
                    if (target == null)
                    {
                        Debug.LogError("Player does NOT have IDamageable component!");
                        return;
                    }
                    target.TakeDamage(context.bubbleDamageToPlayer);
                    Destroy(gameObject);
                }   
            }
           
        }
    }

    public void MoveToTarget(Vector3 targetPos ,float speed)
    {

        moveSpeed= speed;
        targetPostion=targetPos;
    }
    public void SetContext(BossContext context)
    {
        this.context = context;
    }

    private void OnDestroy()
    {
        if (destroyTimer >30f)
        {
            if (playerHealth != null)
            {
                playerHealth.OnTakeDamageWithNoColdown(5f);
            }
            Destroy(gameObject);
            
         
        }
        else
        {
            destroyTimer += Time.deltaTime;
        }
    }

}
