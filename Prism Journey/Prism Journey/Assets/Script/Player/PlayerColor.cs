using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    
    private ColorState currentColor;
    

  //Event For playerVisual Change that passing colorState Value  
    public event EventHandler<OnColorVisualChangeEventArgs> OnColorVisualChange;
    public class OnColorVisualChangeEventArgs : EventArgs
    {
        public ColorState colorState;
    }


   


    private void Start()
    {
        currentColor = ColorState.Red;


       
        OnColorVisualChange?.Invoke(this, new OnColorVisualChangeEventArgs { colorState = currentColor });
    }

    public ColorState GetColor()
    {
        return currentColor;
    }

    public void SetColor(ColorState colorState)
    {
        if (colorState != currentColor)
        {

            currentColor = colorState;
            OnColorVisualChange?.Invoke(this, new OnColorVisualChangeEventArgs { colorState = currentColor });

        }
    }
}
