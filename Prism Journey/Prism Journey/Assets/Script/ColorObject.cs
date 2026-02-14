using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour, IColorInteractable
{

    private MeshRenderer meshRenderer;
    private ColorState colorState;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        colorState=ColorState.Blue;
    }


    private void Start()
    {
        meshRenderer.material.color=Color.blue;
    }
    public ColorState GetColor()
    {
        return colorState;
    }

    public void SetColor(ColorState colorState)
    {
        this.colorState = colorState;
        if (colorState == ColorState.Red)
        {
            meshRenderer.material.color=Color.red;

        }
        else
        {
            meshRenderer.material.color = Color.blue;
        }
    }
}
