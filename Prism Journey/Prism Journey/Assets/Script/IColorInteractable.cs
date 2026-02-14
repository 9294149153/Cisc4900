using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IColorInteractable
{
   

   // public void ColorSwap();
  

    public ColorState GetColor();

    public void SetColor(ColorState color);
}
