using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Windows;


public class Player : MonoBehaviour
{
    private PlayerInputSystem playerInputSystem;
    private Vector2 input;

    [SerializeField] private PlayerAnimationBrain playerAnimationBrain;
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
        input = playerInputSystem.GetMovementVectorNormalized();

        //  player did not move do ignore the later code
        if (input == Vector2.zero) {
            playerAnimationBrain.PlayIdle();
            return; 
        } 
       

        playerAnimationBrain.PlayRun();
     
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        float speed = 10f;
        float playerHeight = 2f;
        float playerRadius = .7f;
        float moveDistance = speed * Time.deltaTime;
        
       
       bool canMove=!Physics.CapsuleCast(transform.position,transform.position+Vector3.up*playerHeight,playerRadius,moveDir,moveDistance);
        
        transform.position += moveDir * speed * Time.deltaTime;

        //movement fluent 
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);


    }


    public Vector3 PlayerMoveDir()
    {
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        return moveDir;
    }
  


}
