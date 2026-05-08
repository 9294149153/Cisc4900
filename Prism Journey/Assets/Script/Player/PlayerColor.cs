using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ColorObject;

public class PlayerColor : MonoBehaviour
{

   [SerializeField] private ColorIdentity[] colorIdentity;
   [SerializeField] private ColorIdentity currentColor;

    private PlayerVisualFromMatieral playerMaterialVisual;
  

    //Event telling those ColorObject check their collision trigger set active or not  
    public event EventHandler<OnColorChanageEventArgs> OnWallColliderDetection;// 
    public class OnColorChanageEventArgs : EventArgs
    {
        public  ColorIdentity color;
    }



    
    private void Awake()
    {
        if(playerMaterialVisual == null) playerMaterialVisual = GetComponentInChildren<PlayerVisualFromMatieral>();

    }
    private void Start()
    {

        //Player did  not have the color identity   
        if (colorIdentity == null || colorIdentity.Length == 0)
        {
            Debug.LogError("PlayerColor: no color identity assigned");
            return;
        }

        currentColor = colorIdentity[0]; // set initial color identity for during game play


        //Set PLayerVisual Matiral During game Start
        Material mat = playerMaterialVisual.GetColorMaterial(currentColor);
        if (mat == null)
        {
            Debug.LogError($"No material found for color: {currentColor}");
            return;
        }

        playerMaterialVisual.SetPlayerMatiral(mat);


        //force those attach walldetection script to sync collision
        WallDetection[] colorWalls = FindObjectsOfType<WallDetection>();
        foreach (WallDetection wall in colorWalls)
        {
            wall.RefreshTriggerState(currentColor);
        }


        //Fire colorObject Collision Change event 
        OnWallColliderDetection?.Invoke(this, new OnColorChanageEventArgs { color = currentColor });

    }


    public ColorIdentity GetCurrentColorIdentity()
    {
        return currentColor;
    }

    public void SetColor(ColorIdentity color)
    {
        if (currentColor == color) {     
            return; 
        }

        currentColor = color;
        PlayerSoundManager.PlaySound(PlayerSoundType.InteractSuccess);
        Material mat = playerMaterialVisual.GetColorMaterial(currentColor);
        if (mat == null)
        {
            Debug.LogError($"No material found for color: {currentColor}");
            return;
        }
        playerMaterialVisual.SetPlayerMatiral(mat);

       // playerMaterialVisual.SetPlayerMatiral(mat);

        WallDetection[] colorWalls = FindObjectsOfType<WallDetection>();
        foreach (WallDetection wall in colorWalls)
        {
            wall.RefreshTriggerState(currentColor);
        }

        OnWallColliderDetection?.Invoke(this, new OnColorChanageEventArgs { color = currentColor });

    }

    public void SetColorOnLoad (ColorIdentity color)
    {
      
        currentColor = color;
        Material mat = playerMaterialVisual.GetColorMaterial(currentColor);
        if (mat == null)
        {
            Debug.LogError($"No material found for color: {currentColor}");
            return;
        }
        playerMaterialVisual.SetPlayerMatiral(mat);

        WallDetection[] colorWalls = FindObjectsOfType<WallDetection>();
        foreach (WallDetection wall in colorWalls)
        {
            wall.RefreshTriggerState(currentColor);
        }

        OnWallColliderDetection?.Invoke(this, new OnColorChanageEventArgs { color = currentColor });

    }

    //Passing Variable Refference

    public ColorIdentity PlayerCurrentColor =>currentColor;

    
}
