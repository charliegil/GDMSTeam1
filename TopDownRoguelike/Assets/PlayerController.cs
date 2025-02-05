using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // ===================== REFERENCES =====================
    private InputSystem_Actions playerInputActions;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // ===================== MOVEMENT =====================
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 movementDirection;
    private Vector2 currentMovement;
    private float currentSpeed;

    // ===================== PHASING =====================
    [SerializeField] private float phaseDuration = 1f;
    [SerializeField] private float phaseSpeed = 7.5f;
    [SerializeField] private float phaseCooldown = 5f;
    private bool isPhasing = false;
    private float phaseInput;

    // ===================== DODGING =====================
    private bool isDodging = false;
    private float dodgeTimer;
    private float dodgeCooldown;

    // ===================== ATTACKING =====================
    [SerializeField] private float attackRange = 3f;
    private float attackInput;
    private GameObject targetEnemy;
    private Coroutine attackCoroutine;

    // ===================== DEBUG & TESTING =====================
    [SerializeField] private GameObject enemy;

    // ===================== UNITY CALLBACKS =====================
    private void Awake() {
        playerInputActions = new InputSystem_Actions();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed;
    }

    private void OnEnable() {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
        playerInputActions.Player.Phase.performed += OnPhase;
        playerInputActions.Player.Phase.canceled += OnPhase;
        playerInputActions.Player.Attack.performed += OnAttack;
        playerInputActions.Player.Attack.canceled += OnAttack;
    }

    private void OnDisable() {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMove;
        playerInputActions.Player.Phase.performed -= OnPhase;
        playerInputActions.Player.Phase.canceled -= OnPhase;
        playerInputActions.Player.Attack.performed -= OnAttack;
        playerInputActions.Player.Attack.canceled -= OnAttack;
    }

    // ===================== INPUT HANDLING =====================
    private void OnMove(InputAction.CallbackContext context) {
        movementDirection = context.ReadValue<Vector2>();
        animator.SetFloat("moveX", movementDirection.x);
        animator.SetFloat("moveY", movementDirection.y);
    }

    private void OnPhase(InputAction.CallbackContext context) {
        phaseInput = context.ReadValue<float>();
    }

    private void OnAttack(InputAction.CallbackContext context) {
        attackInput = context.ReadValue<float>();
        Debug.Log(attackInput);
    }

    // ===================== GAMEPLAY LOGIC =====================
    private void Update() {
        AdjustPlayerDirection();
        if (phaseInput > 0 && !isPhasing) {
            StartCoroutine(Phase());
        } else {
            Move();
            Attack();
        }
    }

    private void Move() {
        currentMovement = movementDirection * currentSpeed;
        rb.linearVelocity = currentMovement;
    }

    private void Attack() {
        if (attackInput > 0) {
            if (targetEnemy == null) {
                if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange) {
                    targetEnemy = enemy;  // Modify to find closest enemy in range
                }
            }

            if (targetEnemy != null && attackCoroutine == null) {
                attackCoroutine = StartCoroutine(DamageOverTime());
            }
        } 
        
        // Attack button released
        else {
            if (attackCoroutine != null || Vector3.Distance(transform.position, enemy.transform.position) > attackRange) {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }

            targetEnemy = null;
        }
    }

    private void AdjustPlayerDirection() {
        spriteRenderer.flipX = movementDirection.x < 0;
    }

    // ===================== COROUTINES =====================
    private IEnumerator Phase() {
        isPhasing = true;

        // Adjust transparency
        Color oldColor = spriteRenderer.color;
        float alpha = 0.5f;
        spriteRenderer.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);

        // Increase movement speed
        currentSpeed = phaseSpeed;

        // Allow player to phase through enemies but not environment
        // TODO: See if there is a better way to do this
        enemy.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(phaseDuration);

        // Return player to normal state
        spriteRenderer.color = oldColor;
        currentSpeed = moveSpeed;
        enemy.GetComponent<BoxCollider2D>().enabled = true;

        isPhasing = false;
    }

    private IEnumerator DamageOverTime() {
        // TODO potentially make player unable to attack while phasing
        while (true) {
            // Damage enemy
            targetEnemy.GetComponent<Health>().TakeDamage(10);
            yield return new WaitForSeconds(0.5f);
        }
    }

    // ===================== DEBUGGING =====================
    void OnDrawGizmos() {
        Handles.DrawWireDisc(transform.position, Vector3.forward, attackRange);

        if (attackCoroutine != null) {
            Gizmos.DrawLine(transform.position, targetEnemy.transform.position);
        }
    }
}
