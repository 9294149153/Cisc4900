using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour
{
    private ColorState colorState;
    private MeshRenderer meshRenderer;
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
}
