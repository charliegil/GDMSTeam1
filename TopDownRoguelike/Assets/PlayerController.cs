using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // REFERENCES
    private InputSystem_Actions playerInputActions;
    private Rigidbody2D rb;

    // MODIFIABLE
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashDistance = 1f;
    
    // LOCAL
    private Vector2 movementDirection;
    private float dodgeInput;
    private Vector2 currentMovement;
    private bool isDodging = false;
    private float dodgeTimer;
    private float dodgeCooldown;

    private enum PlayerState {
        Normal,
        Rolling
    }

    private PlayerState playerState;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        playerInputActions = new InputSystem_Actions();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
        playerInputActions.Player.Dodge.performed += OnDodge;
        playerInputActions.Player.Dodge.canceled += OnDodge;
    }

    private void OnDisable() {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMove;
        playerInputActions.Player.Dodge.performed -= OnDodge;
        playerInputActions.Player.Dodge.canceled -= OnDodge;
    }

    private void OnMove(InputAction.CallbackContext context) {
        movementDirection = context.ReadValue<Vector2>();
        animator.SetFloat("moveX", movementDirection.x);
        animator.SetFloat("moveY", movementDirection.y);
    }

    private void OnDodge(InputAction.CallbackContext context) {
        dodgeInput = context.ReadValue<float>();
    }

    private void Update() {
    }

    private void FixedUpdate() {
        AdjustPlayerDirection();
        Move();
    }

    private void Move() {
        currentMovement = movementDirection * moveSpeed;
        rb.linearVelocity = currentMovement;
    }

    private void AdjustPlayerDirection() {
        spriteRenderer.flipX = movementDirection.x < 0;
    }
}
