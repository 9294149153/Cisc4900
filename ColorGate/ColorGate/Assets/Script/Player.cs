using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputSystem playerInputSystem;
    

    private void Awake()
    {
        playerInputSystem=GetComponent<PlayerInputSystem>();
    }


    private void Update()
    {
        Movement();
        
    }

    public void Movement()
    {
        Vector2 input = playerInputSystem.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        float speed = 10f;
        float playerHeight = 2f;
        float playerRadius = .7f;
        float moveDistance = speed * Time.deltaTime;
        
       
       bool canMove=!Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight,playerRadius,moveDir,moveDistance);
        

       

        if (!canMove)
        {
            //cannot move toward moveDir 
            //Attemp only X movement 
            Vector3 moveDirX = new Vector3(moveDir.x,0,0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if ((canMove))
            {
                // can move only x
                moveDir = moveDirX;

            }
            else
            {
                //cannot move only on the X
                //Attemp only Z movement
                Vector3 moveDirZ = new Vector3(0 ,0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove) {
                    moveDir = moveDirZ;
                }
                else
                {
                    // cannot move with any direction
                }

            }


        }

        if (canMove)
        {
            transform.position += moveDir * speed * Time.deltaTime;

        }

        float rotateSpeed = 10f;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);


    }



}
