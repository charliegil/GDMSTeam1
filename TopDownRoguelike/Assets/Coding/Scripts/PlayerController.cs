using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // REFERENCES
    private InputSystem_Actions playerInputActions;
    private Rigidbody2D rb;
    [SerializeField] TextMeshProUGUI healthText;

    // MODIFIABLE
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float phaseDuration = 1f;
    [SerializeField] private float phaseSpeed = 7.5f;
    [SerializeField] private float phaseCooldown = 5f;

    // TESTING
    [SerializeField] private GameObject enemy;
    [SerializeField] private bool canPhase = true;
    
    // LOCAL
    private Vector2 movementDirection;
    private float phaseInput;
    private Vector2 currentMovement;
    private bool isDodging = false;
    private float dodgeTimer;
    private float dodgeCooldown;
    private float currentSpeed;
    private float phaseTimer;
    private float health = 100;

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
        currentSpeed = moveSpeed;
        phaseTimer = phaseCooldown;
    }

    private void OnEnable() {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
        playerInputActions.Player.Phase.performed += OnPhase;
        playerInputActions.Player.Phase.canceled += OnPhase;
    }

    private void OnDisable() {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMove;
        playerInputActions.Player.Phase.performed -= OnPhase;
        playerInputActions.Player.Phase.canceled -= OnPhase;
    }

    private void OnMove(InputAction.CallbackContext context) {
        movementDirection = context.ReadValue<Vector2>();
        animator.SetFloat("moveX", movementDirection.x);
        animator.SetFloat("moveY", movementDirection.y);
    }

    private void OnPhase(InputAction.CallbackContext context) {
        phaseInput = context.ReadValue<float>();
    }

    private void Update() {
        // Update phase cooldown timer
        // if (phaseTimer <= 0) {
        //     canPhase = true;
        // } else {
        //     phaseTimer -= Time.deltaTime;

        //     if (phaseInput > 0) {
        //         Debug.Log("Can't phase yet!");
        //         phaseInput = 0;  // TO DEBUG
        //     }
        // }

        AdjustPlayerDirection();
        if (phaseInput > 0) {
            StartCoroutine(Phase());
            phaseInput = 0;  // DEBUG: this should be done automatically since we are subscribed to Phase.canceled
        } else {
            Move();
        }
    }

    private void Move() {
        currentMovement = movementDirection * currentSpeed;
        rb.linearVelocity = currentMovement;
    }

    private void AdjustPlayerDirection() {
        spriteRenderer.flipX = movementDirection.x < 0;
    }


    // DEBUG: Pressing phase while currently phasing breaks
    private IEnumerator Phase() {
        // Adjust transparency
        Color oldColor = spriteRenderer.color;  // Store old color
        float alpha = 0.5f;  // Modify transparency
        spriteRenderer.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);

        // Increase movement speed
        currentSpeed = phaseSpeed;

        // Allow player to phase through enemies but not environment
        enemy.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(phaseDuration);

        // Return player to normal state
        spriteRenderer.color = oldColor;
        currentSpeed = moveSpeed;
        enemy.GetComponent<BoxCollider2D>().enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Projectile")) {
            Debug.Log("I'm hit!");
            TakeDamage(1);
        }
    }

    void TakeDamage(float damage) {
        health -= damage;
        healthText.SetText(health.ToString());
    }
}
