using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private InputSystem_Actions playerInputActions;
    private Vector2 movementInput;
    private Vector2 currentMovement;
    private CharacterController characterController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        playerInputActions = new InputSystem_Actions();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable() {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable() {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context) {
        movementInput = context.ReadValue<Vector2>();
        movementInput.Normalize();
    }

    private void Update() {
        Move();
    }

    private void Move() {
        currentMovement = movementInput * moveSpeed * Time.deltaTime;
        characterController.Move(currentMovement);
    }
}
