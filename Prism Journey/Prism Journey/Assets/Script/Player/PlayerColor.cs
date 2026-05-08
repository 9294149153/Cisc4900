using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ColorObject;

public class PlayerColor : MonoBehaviour
{
    
    

   [SerializeField] private ColorIdentity[] colorIdentity;
     private ColorIdentity currentColor;
    


    //Event For playerVisual Change that passing colorState Value  
    public event EventHandler<OnColorChanageEventArgs> OnColorVisualChange;
    public event EventHandler<OnColorChanageEventArgs> OnWallColliderDetection;
    public class OnColorChanageEventArgs : EventArgs
    {
        public  ColorIdentity color;
    }




    private void Awake()
    {
        
    }

    private void Start()
    {

        if (colorIdentity != null && colorIdentity.Length>0)
        {
            currentColor = colorIdentity[0];


            OnColorVisualChange?.Invoke(this, new OnColorChanageEventArgs { color = currentColor });
            OnWallColliderDetection?.Invoke(this, new OnColorChanageEventArgs { color = currentColor });

        }
       

        


    }

  

    public ColorIdentity GetCurrentColorIdentity()
    {
        return currentColor;
    }

    public void SetColor(ColorIdentity color)
    {
        if (currentColor!=color)
        {
            currentColor = color;
            
            OnColorVisualChange?.Invoke(this, new OnColorChanageEventArgs { color = currentColor });
            OnWallColliderDetection?.Invoke(this, new OnColorChanageEventArgs { color = currentColor });

            

        }
    }

    
}
