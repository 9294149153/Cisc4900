using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDetection : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private Transform detectionPoint;
    [SerializeField] private float radius = 0.3f;
    [SerializeField] private float distance = 2.0f;
    [SerializeField] private LayerMask colorInteractableLayer;


    //
    public IColorInteractable FindClosestDifferentColor(ColorIdentity playerCurrentColor)
    {
        //Detect all ColorInterableObject if in range
        RaycastHit[] colorInteractableHits = Physics.SphereCastAll(detectionPoint.position, radius,detectionPoint.forward, distance, colorInteractableLayer);

        if(colorInteractableHits.Length==0 ) return null;

        Array.Sort(colorInteractableHits, (a, b) => a.distance.CompareTo(b.distance));


         foreach (RaycastHit hit in colorInteractableHits)
        {
            IColorInteractable target = hit.collider.GetComponentInParent<IColorInteractable>();
            if (target == null) continue;

            //Hit object has different color than player 
            if (target.GetColorIdentity() !=playerCurrentColor )
                return target; 
        }

        return null;
    }

    
}
