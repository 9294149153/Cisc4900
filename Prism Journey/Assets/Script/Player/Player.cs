using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;


public class Player : MonoBehaviour
{
    private PlayerInputSystem playerInputSystem;
   
    [SerializeField] private PlayerAnimationBrain[] playerAnimationBrain;
    private PlayerSoundManager playerSoundManager;

    CharacterController characterController;
    private PlayerHealth playerHealth;

    [Header("Move Setting")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float dashSpeed = 1.3f;
    [SerializeField] private float rotateSpeed = 10f;

    [Header("Gravity Setting")]
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedStickForce = -2f;


    private Vector2 input;
    private float verticalVelocity;


    [Header("FallToVoidSetting")]
    [SerializeField] private float fallMaxAllow = 20f;
    private float fallValuecount;
    private Vector3 savePosition;
    [SerializeField] private float rayCastDistance;
    [SerializeField] private LayerMask ignoreLayer; // ColorInteraction
    [SerializeField] private LayerMask standLayer;// All Layer Except ColorInNteraciton
    [SerializeField] private float groundCheckHeight = 0.15f;
    [SerializeField] private Vector3 groundCheckSize = new Vector3(0.45f, 0.08f, 0.45f);
    [SerializeField] private int fallDamage;
    [SerializeField] private float timeBeforeNextSave;
    private float nextSaveTimer;

    private void Awake()
    {
        playerInputSystem = GetComponent<PlayerInputSystem>();
        characterController = GetComponent<CharacterController>();
        playerSoundManager = GetComponent<PlayerSoundManager>();
        playerHealth = GetComponentInParent<PlayerHealth>();

    }
    private void Start()
    {
        nextSaveTimer = 0f;
        fallValuecount = 0f;
        savePosition = transform.position;
    }
    
        
    private void Update()
    {
       
        PlayerMovement();
        HandleFallRespawn();

        if (UnityEngine.Input.GetKeyDown(KeyCode.P))
        {
            CinmachieShake.Instance.ShakeCamera(5f, 0.4f);
        }
    }

    private void HandleFallRespawn()
    {
        int safeGroundMask = ~ignoreLayer;

       // Vector3 checkCenter = transform.position + Vector3.down * groundCheckHeight;
        Vector3 checkCenter = transform.position + Vector3.down * Mathf.Abs(groundCheckHeight);
        bool onAnyGround = Physics.CheckBox(
            checkCenter,
            groundCheckSize,
            Quaternion.identity,
            standLayer,
            QueryTriggerInteraction.Ignore
        );

        bool onSafeGround = Physics.CheckBox(
       checkCenter,
       groundCheckSize,
       Quaternion.identity,
       safeGroundMask,
       QueryTriggerInteraction.Ignore
   );

        if (onAnyGround)
        {
            // player is NOT falling (even on color object)
            fallValuecount = 0f;

            if (onSafeGround)
            {
                // only update save position on real ground
                nextSaveTimer += Time.deltaTime;

                if (nextSaveTimer >= timeBeforeNextSave)
                {
                    savePosition = transform.position;
                    nextSaveTimer = 0f;

                    Debug.Log("new Save Position " + savePosition);
                }
            }

            return;
        }
        
        nextSaveTimer = 0f;

        fallValuecount += Time.deltaTime;

        if (fallValuecount >= fallMaxAllow)
        {
            characterController.enabled = false;

            if (playerHealth != null)
            {
                playerHealth.TakeDamage((float)fallDamage);

            }
            CinmachieShake.Instance.ShakeCamera(5f, .1f);

            transform.position = savePosition;
            characterController.enabled = true;

            fallValuecount = 0f;
        }


    }

    public void PlayerMovement()
    {
        input = playerInputSystem.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(input.x, 0, input.y);

        // ---------------- GRAVITY ----------------
        // If grounded and currently moving downward,
        // keep a small negative value so controller stays attached to ground

        if (characterController.isGrounded && verticalVelocity <0f)
        {
            verticalVelocity = groundedStickForce;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // ---------------- SPEED ----------------
        float currentSpeed = moveSpeed;

        if (playerInputSystem.IsDashing)
        {
            currentSpeed *= dashSpeed;
        }


        // ---------------- FINAL MOVE ----------------


        Vector3 finalMove = moveDir * currentSpeed;
        finalMove.y = verticalVelocity;
        characterController.Move(finalMove * Time.deltaTime);
        

        //------------------PlayerCharacterAniamtion-----------------
        if (moveDir == Vector3.zero)
        {
            for(int i = 0; i < playerAnimationBrain.Length; i++)
            {
                playerAnimationBrain[i].PlayIdle();
            }
            
        }
        else
        {
            if (playerInputSystem.IsDashing)
            {
                for (int i = 0; i < playerAnimationBrain.Length; i++)
                {
                    playerAnimationBrain[i].PlayDash();
                }
                
            }
            else
            {
                for (int i = 0; i < playerAnimationBrain.Length; i++)
                {
                    playerAnimationBrain[i].PlayRun();
                }
                
            }
        }

        //-------------------Rotation----------------------
        //movement fluent  with rotation 
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }
    public void SetScirptActive(bool value)
    {
        this.enabled= value;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 checkCenter = transform.position + Vector3.down * Mathf.Abs(groundCheckHeight);

        Gizmos.DrawWireCube(checkCenter, groundCheckSize * 2f);
    }
}
