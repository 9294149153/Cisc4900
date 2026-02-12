using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    
    private MeshRenderer meshRenderer;
    [SerializeField] private ColorSwap colorSwap;

    private void Awake()
    {
        
        meshRenderer=GetComponent<MeshRenderer>();
        
    }
    private void Start()
    {
       
        colorSwap.OnColorVisualChange += ColorSwap_OnColorVisualChange;
        meshRenderer.material.color = Color.red;
    }

    private void ColorSwap_OnColorVisualChange(object sender, ColorSwap.OnColorVisualChangeEventArgs e)
    {
        if (e.colorState == ColorState.Red)
        {
            meshRenderer.material.color = Color.red;
        }
        else
        {
            meshRenderer.material.color = Color.blue;
        }
    }

   
    
}
