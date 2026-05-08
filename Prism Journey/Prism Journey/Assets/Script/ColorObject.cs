using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorObject : MonoBehaviour, IColorInteractable
{

    
   

    [SerializeField] private ColorIdentity[] colorIdentity;
     private ColorIdentity currentColor;


    public event EventHandler<OnColorVisualChangeEventArg> OnColorVisualChange;
    public class OnColorVisualChangeEventArg { public ColorIdentity color; }



    private void Start()
    {
        if (colorIdentity != null) 
        {
            currentColor = colorIdentity[0];
            OnColorVisualChange?.Invoke(this, new OnColorVisualChangeEventArg { color = currentColor });

        }
       
    
    }

  public ColorIdentity GetColorIdentity()
    {
        return currentColor;
    }

    public void SetColor(ColorIdentity swapColor)
    {
        currentColor = swapColor;
        OnColorVisualChange?.Invoke(this, new OnColorVisualChangeEventArg{ color = currentColor });
    }
}
