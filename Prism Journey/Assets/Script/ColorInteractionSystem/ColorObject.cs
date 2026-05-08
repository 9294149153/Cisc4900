using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColorObject : MonoBehaviour, IColorInteractable
{

    [SerializeField] private ColorIdentity[] colorIdentity;
    [Inspectable] private ColorIdentity currentColor;


    public event EventHandler<OnColorVisualChangeEventArg> OnColorVisualChange;
    public class OnColorVisualChangeEventArg { public ColorIdentity color; }


    private void Awake()
    {
        if (colorIdentity == null || colorIdentity.Length == 0)
        {
            Debug.LogError($"{gameObject.name}: no colorIdentity assigned", this);
            return;
        }

        currentColor = colorIdentity[0];

    }

    private void Start()
    {
        if (colorIdentity.Length>0) 
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
