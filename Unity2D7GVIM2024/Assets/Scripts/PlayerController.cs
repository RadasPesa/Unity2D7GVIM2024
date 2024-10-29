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
    [SerializeField]
    private Animator animator;

    private PlayerState playerState;

    void Awake()
    {
        inputManager = InputManager.instance;
        playerInput = GetComponent<PlayerInput>();

        movementAction = playerInput.actions["Movement"];
        movementAction.performed += Move;
        rb = GetComponent<Rigidbody2D>();
        
        playerState = PlayerState.Move;
    }

    void Update()
    {
        moveDirection = Vector2.zero;
        if (playerState == PlayerState.Move)
        {
            moveDirection = movementAction.ReadValue<Vector2>();
            animator.SetFloat("Horizontal", moveDirection.x);
            animator.SetFloat("Vertical", moveDirection.y);
            animator.SetFloat("Speed", moveDirection.sqrMagnitude);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * (playerSpeed * Time.fixedDeltaTime));
    }

    public void Move(InputAction.CallbackContext ctx)
    {
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        StartCoroutine(AttackCo());
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("Attacking", true);
        playerState = PlayerState.Attack;
        yield return null;
        animator.SetBool("Attacking", false);
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttackDown"))
        {
            yield return null;
        }
        playerState = PlayerState.Move;
    }
}

public enum PlayerState
{
    Move,
    Attack,
    Interact
}
