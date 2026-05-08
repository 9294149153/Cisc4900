using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using static UnityEngine.UI.Image;


public class PlayerInputSystem : MonoBehaviour
{
    
    PlayerInputAction playerInputAction;
    Vector2 inputVector;


    bool isDashing;
    public bool IsDashing => isDashing;

    float dashTransitionTimer = 0f;

    bool interactionButtonPress;
    public bool interactionButton => interactionButtonPress;
    private void Awake()
    {

        playerInputAction = new PlayerInputAction();    
    }
    private void OnEnable()
    {
        playerInputAction.Player.Enable();

        // movement subscribe
        playerInputAction.Player.Movement.performed += PlayerMovement;
        playerInputAction.Player.Movement.canceled += PlayerMovement;

        //Dash Movement subscribe
        playerInputAction.Player.Dashing.started += PlayerDashingMovement;

        //ColorSwap  subscribe
        playerInputAction.Player.ColorSwap.started += PlayerInteraction ;
        playerInputAction.Player.ColorSwap.canceled += PlayerInteraction;
    }
    private void OnDisable()
    {
        //remove the blinding link
        playerInputAction.Player.Movement.performed -= PlayerMovement;
        playerInputAction.Player.Movement.canceled -= PlayerMovement;
        playerInputAction.Player.Disable();

    }

    private void Update()
    {
        if (GetMovementVectorNormalized() == Vector2.zero)
        {
            if (dashTransitionTimer <= 0.3f)
            {
                dashTransitionTimer += Time.deltaTime;
                return;

            }
            isDashing = false;
            dashTransitionTimer = 0f;
        }
    }

    void PlayerDashingMovement(InputAction.CallbackContext context)
    {
        isDashing = true;
    }
    private void PlayerMovement(InputAction.CallbackContext context)
    {
        inputVector=context.ReadValue<Vector2>().normalized; // Store value each time when pressed


    }

    private void PlayerInteraction(InputAction.CallbackContext context)
    {
        interactionButtonPress = context.ReadValueAsButton();
        
    }
    public Vector2 GetMovementVectorNormalized()
    {
        
        return inputVector;
       
    }
   


}