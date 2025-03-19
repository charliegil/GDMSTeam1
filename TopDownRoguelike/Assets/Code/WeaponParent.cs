using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponParent : MonoBehaviour
{
    private Vector2 pointerPosition;
    private Camera mainCam;
    private PlayerInput playerInput;
    private InputAction pointerAction;

    private void Awake()
    {
        // mainCam = Camera.main;
        // playerInput = GetComponent<PlayerInput>();
        // pointerAction = playerInput.actions["PointerPosition"]; // Ensure this matches your Input Action name
       // Debug.Log()
    }

    private void Update()
    {
        Debug.Log("Mouse Position: " + pointerPosition);
        // Read the mouse position from the Input System
        
        pointerPosition = pointerAction.ReadValue<Vector2>();

        // Convert screen position to world position
        Vector3 worldPosition = mainCam.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, mainCam.nearClipPlane));
        worldPosition.z = 0; // Keep it in 2D

        // Compute rotation
        Vector3 difference = worldPosition - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }
    //  private Vector2 pointerPosition;
    // private Camera mainCam;
    
    // private void Awake()
    // {
    //     mainCam = Camera.main;
    // }

    // public void OnLook(InputAction.CallbackContext context)
    // {
    //     pointerPosition = context.ReadValue<Vector2>(); // Get mouse position from new input system
    // }

    // private void Update()
    // {
    //     Vector3 worldPosition = mainCam.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, mainCam.nearClipPlane));
    //     Vector3 difference = worldPosition - transform.position;
    //     difference.z = 0; // Keep it in 2D

    //     float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    //     transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    // }
// {
//    public Vector2 Pointerposition {get; set;}

//     private void Update()
//     {
//      transform.right = (Pointerposition-(Vector2)transform.position).normalized;   
//      Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position;
//      difference.Normalize();
//      float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
//      transform.rotation = Quaternion.Euler(0f, 0f, rotation_z + offset);
//     }
}
