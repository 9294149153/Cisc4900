using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTesting : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent agent;

    private void Start()
    {
        

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit hitInfo)) 
            {
                Vector3 pos =hitInfo.point;
                pos=new Vector3(pos.x,0,pos.z);
                agent.SetDestination(pos);
            }
        }
    }
}
