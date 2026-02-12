using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour,IColorObjectInterface
{
    private ColorState colorState;
    private MeshRenderer meshRenderer;
    [SerializeField] private Collider blockCollider;
    [SerializeField] private Collider detectionCollider;

   

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        colorState=ColorState.Blue;
        if(meshRenderer != null && colorState == ColorState.Blue)
        {
            meshRenderer.material.color = Color.blue;

        }
        else
        {
            meshRenderer.material.color= Color.red;
        }
    }
    public ColorState ColorObjectState()
    {
        return colorState;
    }
    public void ColorSwap()
    {
        if (colorState == ColorState.Red)
        {
            colorState=ColorState.Blue;
            meshRenderer.material.color = Color.blue;
            return;
        }
        else
        {
            colorState = ColorState.Red;
            meshRenderer.material.color = Color.red;
        }
        
    }

    public ColorState GetColor()
    {
        return colorState;
    }

    public void SetBlockColliderDisable()
    {
        blockCollider.enabled = false;
    }

    public void setBlockColliderEnable()
    {
        blockCollider.enabled = true;
    }
}
