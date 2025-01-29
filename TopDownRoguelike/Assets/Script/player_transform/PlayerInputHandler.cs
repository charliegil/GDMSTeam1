using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;


    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";


    [Header("Action Name References")]
    [SerializeField] private string transformS = "Interact";
    [SerializeField] private string move = "Move";

    //add other action
    private InputAction transformAction;
    private InputAction moveAction;

    //add
    public bool TransformTriggered{ get; private set;}
    public Vector2 MoveInput{ get; private set;}
    //add other action
    public static PlayerInputHandler Instance {get; private set;}

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("??"+Instance);
            Debug.Log("PlayerInputHandler instance set!");
        }
        else{
            Destroy(gameObject);
        } 
        transformAction = playerControls.FindActionMap(actionMapName).FindAction(transformS);
        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        //add other action

        RegisterInputActions();
    }
   
    void RegisterInputActions(){
        transformAction.performed += context => TransformTriggered = true;
        transformAction.canceled += context => TransformTriggered = false;
        //all other action
         moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
         moveAction.canceled += context =>  MoveInput = Vector2.zero;
    }
   
    private void OnEnable(){
        transformAction.Enable();
        moveAction.Enable();
        //and all other action
    }
    private void OnDisable(){
        transformAction.Disable();
        moveAction.Disable();
    }

    
}
