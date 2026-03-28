using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObjectVisual : MonoBehaviour
{
    private MeshRenderer meshRenderer;

   [SerializeField] private ColorObject colorObject;

    private void Awake()
    {
       
        meshRenderer = GetComponent<MeshRenderer>();
       

    }
    private void OnEnable()
    {
        colorObject.OnColorVisualChange += ColorObject_OnColorVisualChange;

    }
    private void OnDisable()
    {
        colorObject.OnColorVisualChange -= ColorObject_OnColorVisualChange;
    }
   

    private void ColorObject_OnColorVisualChange(object sender, ColorObject.OnColorVisualChangeEventArg e)
    {
        meshRenderer.material.color=e.color.displayColor;
    }

    
}
