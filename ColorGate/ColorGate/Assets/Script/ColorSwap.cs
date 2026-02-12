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
    private ColorState colorState;
    private Player player;
    private Vector3 lastMoveDir;
    

    public event EventHandler<OnColorVisualChangeEventArgs> OnColorVisualChange;
    public class OnColorVisualChangeEventArgs : EventArgs
    {
        public ColorState colorState;
    }
    private void Awake()
    {
        colorState = ColorState.Red;
        player = GetComponent<Player>();
    }
    private void Update()
    {



        PassingColorObject();


        if (Input.GetKeyDown(KeyCode.E) )
        {
            
            ColorObjectArray();
        }
        

    }

    public void ColorObjectArray()
    {
        Vector3 body =transform.position;
        Vector3 head =transform.position+Vector3.up * 1.8f;
        float radius = 0.5f;
        float distance = 1.5f;
        Vector3 moveDir = player.PlayerMoveDir();
        if (moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
        }

        bool wasColorObject = Physics.CapsuleCast(body, head, radius, lastMoveDir, out RaycastHit rayCastHit, distance);

        if (!wasColorObject)
        {
            Debug.Log("CapsuleCast didn't hit anything");
            return;
        }

        if (rayCastHit.collider.TryGetComponent<IColorObjectInterface>(out IColorObjectInterface iColorObject) )
        {

            iColorObject.ColorSwap();
            ColorSwaps();
            OnColorVisualChange?.Invoke(this, new OnColorVisualChangeEventArgs { colorState = colorState });
        }
        else
        {
            Debug.Log("raycast didnt hit object");
        }
        
       



    }


    public void PassingColorObject()
    {
        Vector3 body = transform.position;
        Vector3 head = transform.position + Vector3.up * 1.8f;
        float radius = 0.5f;
        float distance = 1.5f;


        bool hasColor = Physics.CapsuleCast(body, head, radius, lastMoveDir, out RaycastHit rayCastHit, distance);
        //Point to an object
        if (!hasColor)
        {
            return;
        }
        GameObject hitObject = rayCastHit.collider.gameObject;

        //Check is the object contain IColorInterface
        if ((hitObject.TryGetComponent<IColorObjectInterface>(out IColorObjectInterface iColorObject)))
        {

            //Object Contain ColorObject Script
            if (hitObject.TryGetComponent<ColorObject>(out ColorObject colorObject)) 
            {
                //Have different color than the player
                if (colorState != colorObject.GetColor())
                {
                    colorObject.SetBlockColliderDisable();
                }
                else
                {
                    //Has same Color as Player
                    colorObject.setBlockColliderEnable();
                }
               


            }
            else
            {
                
            }
        }
        else
        {

        }

    }

   


    public void ColorSwaps()
    {
        if (colorState != ColorState.Blue)
        {
            //Color Currently not blueS
            colorState = ColorState.Blue;

        }
        else
        {
            colorState = ColorState.Red;
        }

        

    }
}
