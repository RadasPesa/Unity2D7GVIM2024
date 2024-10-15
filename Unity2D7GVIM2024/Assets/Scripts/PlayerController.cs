using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerInput playerInput;

    private InputAction movementAction;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    [SerializeField]
    private float playerSpeed;

    void Awake()
    {
        inputManager = InputManager.instance;
        playerInput = GetComponent<PlayerInput>();

        movementAction = playerInput.actions["Movement"];
        movementAction.performed += Move;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDirection = movementAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * (playerSpeed * Time.fixedDeltaTime));
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        Debug.Log("Movement: " + ctx.phase);
    }
    
    
}
