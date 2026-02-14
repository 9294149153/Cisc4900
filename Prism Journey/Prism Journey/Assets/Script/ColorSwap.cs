using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Player;

public class ColorSwap : MonoBehaviour
{
    private PlayerColor playerColor;
    private ColorDetection playerColorDetection;

    public event EventHandler<EventArgs> OnPlayerColorChange;
    private void Awake()
    {
        playerColor=GetComponent<PlayerColor>();
        playerColorDetection=GetComponent<ColorDetection>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TrySwap();
        }
    }


    public void TrySwap()
    {
        var target = playerColorDetection.FindClosestDifferentColor(playerColor.GetColor());
        if (target == null) return;


        ColorState oldColor = playerColor.GetColor();
        playerColor.SetColor(target.GetColor());
        target.SetColor(oldColor);  
        OnPlayerColorChange?.Invoke(this , EventArgs.Empty);
        
    }





}
