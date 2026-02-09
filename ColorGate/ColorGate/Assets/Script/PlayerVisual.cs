using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    
    private MeshRenderer meshRenderer;
    [SerializeField] private Player player;

    private void Awake()
    {
        
        meshRenderer=GetComponent<MeshRenderer>();
        
    }
    private void Start()
    {
        player.OnColorVisualChange += Player_OnColorVisualChange;
        meshRenderer.material.color = Color.red;
    }

    private void Player_OnColorVisualChange(object sender, Player.OnColorVisualChangeEventArgs e)
    {
        if ( e.colorState== ColorState.Red)
        {
            meshRenderer.material.color = Color.red;
        }
        else
        {
            meshRenderer.material.color = Color.blue;
        }
    }
    
}
