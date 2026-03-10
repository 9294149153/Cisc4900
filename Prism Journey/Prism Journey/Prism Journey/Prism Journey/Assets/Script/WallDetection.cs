using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
     private ColorObject colorObject;
     [SerializeField]private Collider wallCollider;
    [SerializeField] private PlayerColor playerColor;


    private void Awake()
    {
        colorObject=GetComponent<ColorObject>();
       


    }

    private void OnEnable()
    {
        if (playerColor == null)
        {
            Debug.LogError("WallDetection: playerColor not assigned", this);
            return;
        }
        playerColor.OnWallColliderDetection += PlayerColor_OnWallColliderDetection;
    }

    private void OnDisable()
    {
        if (playerColor != null)
            playerColor.OnWallColliderDetection -= PlayerColor_OnWallColliderDetection;
    }


    //collider enable and disable each time player change color and for inital collider enable disable
    private void PlayerColor_OnWallColliderDetection(object sender, PlayerColor.OnColorChanageEventArgs e)
    {
        bool canPass=(e.color ==colorObject.GetColorIdentity());
        wallCollider.isTrigger=canPass;
    }
}
