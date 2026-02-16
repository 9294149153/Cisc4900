using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColorInteractable
{
   

   // public void ColorSwap();
  

    public ColorIdentity GetColorIdentity();

    public void SetColor(ColorIdentity color);
}
