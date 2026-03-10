using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    
    private MeshRenderer meshRenderer;
    [SerializeField] private PlayerColor playerColor;

    private void Awake()
    {
        
        meshRenderer=GetComponent<MeshRenderer>();
        playerColor.OnColorVisualChange += PlayerColor_OnColorVisualChange;

    }
   

    private void PlayerColor_OnColorVisualChange(object sender, PlayerColor.OnColorChanageEventArgs e)
    {
       meshRenderer.material.color=e.color.displayColor;
    }

    

   
    
}
