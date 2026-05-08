using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObjectVisual : MonoBehaviour
{
    [SerializeField]private MeshRenderer meshRenderer;
   [SerializeField] private ColorObject colorObject;

   
    private void Awake()
    {
       

        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        if (colorObject == null)
            colorObject = GetComponentInParent<ColorObject>();

       
    }
    private void OnEnable()
    {
           if (colorObject != null)
            colorObject.OnColorVisualChange += ColorObject_OnColorVisualChange;
       

    }
    private void OnDisable()
    {

        if (colorObject != null)
            colorObject.OnColorVisualChange -= ColorObject_OnColorVisualChange;
    }


    private void Start()
    {
        if(meshRenderer != null)
        {
            meshRenderer.enabled = true;
            meshRenderer.material=colorObject.GetColorIdentity().material;
        }
        
    }

   
    private void ColorObject_OnColorVisualChange(object sender, ColorObject.OnColorVisualChangeEventArg e)
    {
        if (colorObject == null) { Debug.LogError($"[ColorObjectVisual] + colorObject reference are missing  colorObject={colorObject}", this); return; }
        meshRenderer.material=colorObject.GetColorIdentity().material;
    }

    
    

    
}
