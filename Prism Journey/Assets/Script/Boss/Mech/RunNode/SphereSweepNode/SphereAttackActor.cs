using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SphereAttackActor : MonoBehaviour , IAttackActor
{
    public Transform Transform => transform;

    private Collider myCollider;


    [SerializeField] private ColorIdentity currentColor;

    private void Awake()
    {
        myCollider=GetComponent<Collider>();
        myCollider.isTrigger = true;
    }
    public void Cleanup()
    {
       Destroy(gameObject);
    }

    public bool HasReached(Vector3 targetPosition, float threshold)
    {

        if (Vector3.Distance(transform.position, targetPosition) <threshold)
        {
            return true;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 10f*Time.deltaTime);

        return false;
    }

    public void Initialize(Vector3 position, Quaternion rotation)
    {
       transform.position = position;
        transform.rotation = rotation;
    }

    public void MoveToward(Vector3 targetPosition, float speed)
    {
        throw new System.NotImplementedException();
    }

    public void SetScaleOverTime(float speed)
    {
        transform.localScale += Vector3.one * speed * Time.deltaTime;
    }

}
