using JetBrains.Rider.Unity.Editor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector3 currentMovement;

    private InputSystem_Actions playerInputActions;
    private Vector2 movementInput;
    [SerializeField] private CharacterController characterController;

    private void Awake()
    {
        playerInputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMove;
    }

    private void OnDestroy()
    {
        if (playerInputActions != null)
        {
            playerInputActions.Dispose();
        }
    }

    private void Update() {
        Move();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        Debug.Log(movementInput);
    }

    private void Move() {
        currentMovement = new Vector3(movementInput.x, 0, movementInput.y) * Time.deltaTime;
        characterController.Move(currentMovement);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Debug.Log(currentMovement);
        Gizmos.DrawLine(transform.position, transform.position + currentMovement * 50);
    }
}
