using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SphereAttackControl : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] private Transform moveObject;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stopDistance = 0.05f;

    private bool onStartPosition;

    private void Start()
    {
        onStartPosition = false;
    }

    private void Update()
    {
        if (!onStartPosition)
        {
            MoveTo(moveObject, startPosition, endPosition, moveSpeed);
            return;
        }


    }

    public void MoveTo(Transform obj,Transform startPo, Transform targetP, float speed)
    {
        if (Vector3.Distance(obj.position, startPo.position) > stopDistance)
        {
            obj.position =Vector3.Slerp(obj.position, startPo.position, speed*Time.deltaTime);

        }
        else
        {
            obj.position = startPosition.position;
            onStartPosition=true;
        }

    }

}
