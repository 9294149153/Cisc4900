using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
[RequireComponent(typeof(SphereCollider))]
public class SphereAttackControl : MonoBehaviour
{
    private Collider myCollider;


    [SerializeField] private ColorIdentity currentColor;
    private void Awake()
    {
        myCollider = GetComponent<Collider>();
        
        if(currentColor == null)
        {
            Debug.LogError($"[SphereAttackControl]| {gameObject.name} ColorIdentity Reference are missing ", this);
        }
    }

    private void Start()
    {
        if (myCollider == null)
        {
            Debug.LogError($"SphereAttackControl |{myCollider} = missing", this);
        }
        myCollider.isTrigger=true;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable;
        PlayerColor playerColor;
        if (other.CompareTag("Player"))
        {

            damageable = other?.GetComponent<PlayerHealth>();
            playerColor = other?.GetComponent<PlayerColor>();
            if (playerColor != null )
            {
                damageable.TakdeDamageWithColor(currentColor, 15f);
            }
        }

    }

    public void SetSphereAttackScale(float speed)
    {
        transform.localScale += Vector3.one * Time.deltaTime * speed;
    }

    public void MoveToTarget(Vector3 target,float speed)
    {
        transform.position = Vector3.MoveTowards(
        transform.position,
        target,
        speed * Time.deltaTime);
    }
}
