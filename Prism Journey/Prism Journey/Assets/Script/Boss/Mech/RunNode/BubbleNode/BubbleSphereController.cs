using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(SphereCollider))]
public class BubbleSphereController : MonoBehaviour, IAttackActor
{


    [SerializeField] private Collider sphCollider;
    [SerializeField] private ColorIdentity color;
     private BossContext context;
    private PlayerHealth playerHealth;
    Transform IAttackActor.Transform => transform;

    IDamageable target;
    private Vector3 targetPostion;

    float destroyTimer = 0f;

    private void Awake()
    {
        if (sphCollider == null) sphCollider = GetComponent<Collider>();

        sphCollider.isTrigger = true;

        playerHealth = FindObjectOfType<PlayerHealth>();

    }

    // Update is called once per frame

    void Update()
    {
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
                if (player.GetCurrentColorIdentity() == color)
                {
                    context.bossHealth.TakeDamage(context.bubbleDamageToBoss);
                    DestoryObject();
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

                    if (context.remoteConfig.bubleAttackConfig != null)
                    {
                        target.TakeDamage(context.remoteConfig.bubleAttackConfig.damageDeal);
                        DestoryObject();
                    }
                    else
                    {
                        target.TakeDamage(5f);
                        DestoryObject();
                    }
                   
                }
            }

        }
    }
    void IAttackActor.Cleanup()
    {
        Destroy(gameObject);
    } 
    bool IAttackActor.HasReached(Vector3 targetPosition, float threshold)
    {
        if(Vector3.Distance(transform.position, targetPostion) < threshold)
        {
            return true;
        }
        return false;
    }

    void IAttackActor.Initialize(Vector3 position, Quaternion rotation)
    {
        transform.localPosition = position;
        transform.localRotation = rotation;
    }

    void IAttackActor.MoveToward(Vector3 targetPosition, float speed)
    {
         transform.position= Vector3.Lerp(transform.position, targetPosition, speed*Time.deltaTime);
    }

    void IAttackActor.SetScaleOverTime(float speed)
    {
        transform.localScale += Vector3.one * speed*Time.deltaTime;
    }

    public void SetContext(BossContext context)
    {
        this.context = context;
    }
    public void OnDestroy()
    {
        if (destroyTimer > 18f)
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

   public void DestoryObject()
    {
        Destroy(gameObject);
    }

}
