using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Color/Color Identity")]
public class ColorIdentity : ScriptableObject
{
    public string currentColorName;
    public Color displayColor;
    [SerializeField ] private ColorIdentity swapIdentityColor;



    public ColorIdentity GetSwapColor()
    {
        return swapIdentityColor;
    }

}
