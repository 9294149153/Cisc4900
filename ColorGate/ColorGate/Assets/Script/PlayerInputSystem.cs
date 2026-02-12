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
    
    private void Awake()
    {


        playerInputAction = new PlayerInputAction();
       
    }
    private void OnEnable()
    {
        playerInputAction.Player.Enable();

        // subscribe
        playerInputAction.Player.Movement.performed += PlayerMovement;
        playerInputAction.Player.Movement.canceled += PlayerMovement;

        //ColorSwap 
       // playerInputAction.Player.ColorSwap.performed += PlayerMovement;
        
    }
    private void OnDisable()
    {
        
        playerInputAction.Player.Movement.performed -= PlayerMovement;
        playerInputAction.Player.Movement.canceled -= PlayerMovement;
        playerInputAction.Player.Disable();

        //ColorSwap
       // playerInputAction.Player.ColorSwap.performed -= PlayerMovement;
    }

    
    private void PlayerMovement(InputAction.CallbackContext context)
    {
        inputVector=context.ReadValue<Vector2>().normalized; // Store value each time press


    }
    public Vector2 GetMovementVectorNormalized()
    {
        
        return inputVector;
       
    }
   


}
