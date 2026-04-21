using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallDetection : MonoBehaviour
{


    [Inspectable]private ColorObject colorObject;

    [SerializeField] private Collider wallCollider;
    [SerializeField] private PlayerColor playerColor;

    private void Awake()
    {
        colorObject = GetComponent<ColorObject>();

        if (wallCollider == null)
            wallCollider = GetComponent<Collider>();

        playerColor = FindAnyObjectByType<PlayerColor>();

        if(wallCollider != null)
        {
            wallCollider.isTrigger=true;
        }
    }

    private void OnEnable()
    {
        if (playerColor != null)
            playerColor.OnWallColliderDetection += PlayerColor_OnWallColliderDetection;
    }

    private void OnDisable()
    {
        if (playerColor != null)
            playerColor.OnWallColliderDetection -= PlayerColor_OnWallColliderDetection;
    }

    public void RefreshTriggerState(ColorIdentity playerCurrentColor)
    {
        if (colorObject == null || wallCollider == null) return;

        bool canPass = colorObject.GetColorIdentity() != playerCurrentColor;
        wallCollider.isTrigger = canPass;

    }

    private void PlayerColor_OnWallColliderDetection(object sender, PlayerColor.OnColorChanageEventArgs e)
    {
        RefreshTriggerState(e.color);
    }






















    /*
    [Inspectable]
     private ColorObject colorObject;
     [SerializeField]private Collider wallCollider;
    [SerializeField] private PlayerColor playerColor;


    private void Awake()
    {
        colorObject=GetComponent<ColorObject>();
        if (wallCollider == null)
            wallCollider = GetComponent<BoxCollider>();

    }


    private void OnEnable()
    {
        if (playerColor != null)
        {
            playerColor.OnWallColliderDetection += PlayerColor_OnWallColliderDetection;
        }
    }

    private void OnDisable()
    {
        if (playerColor != null)
        {
            playerColor.OnWallColliderDetection -= PlayerColor_OnWallColliderDetection;
        }
    }

    private void Start()
    {
        if (playerColor == null)
        {
            Debug.LogError("WallDetection: playerColor not assigned", this);    
            return;
        }

        if (colorObject == null) return; // if the object has not identity instance than  do nothing

        if (colorObject == null || wallCollider == null) return;

        RefreshTriggerState(playerColor.PlayerCurrentColor);

    }



    //collider enable and disable each time player change color and for inital collider enable disable
    private void PlayerColor_OnWallColliderDetection(object sender, PlayerColor.OnColorChanageEventArgs e)
    {
        RefreshTriggerState(e.color);
    }

    private void RefreshTriggerState(ColorIdentity playerCurrentColor)
    {
        if (colorObject == null || wallCollider == null) return;

        bool canPass = colorObject.GetColorIdentity() == playerCurrentColor;
        wallCollider.isTrigger = canPass;
    }*/
}
