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
        var target = playerColorDetection.FindClosestDifferentColor(playerColor.GetCurrentColorIdentity());
        if (target == null) return;

        ColorIdentity oldColor=target.GetColorIdentity();
        target.SetColor(playerColor.GetCurrentColorIdentity());
        playerColor.SetColor(oldColor);
       
        
    }





}
