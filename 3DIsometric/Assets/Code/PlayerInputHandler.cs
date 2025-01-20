using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //private IControllable currentControllable;

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;
    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string transformS = "Interact";
    //[SerializeField] private string move = "Move";

    //add other action

    private InputAction transformAction;
    //private InputAction moveAction;
    //add other action
    
    public bool TransformTriggered{ get; private set;}
    //public Vector2 MoveInput{ get; private set;}
    //add other action
    public static PlayerInputHandler Instance {get; private set;}

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("??"+this);
            Debug.Log("PlayerInputHandler instance set!");
        }
        else{
            Destroy(gameObject);
        } 
        transformAction = playerControls.FindActionMap(actionMapName).FindAction(transformS);
        //moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        //add other action

        RegisterInputActions();
    }
    // public void SetControllable(IControllable controllable){
        
    //     currentControllable = controllable;
    // }

    // private void Update(){
    //     if(currentControllable!=null){
    //         currentControllable.HandleInput();
    //     }
    // }
    
    void RegisterInputActions(){
        transformAction.performed += context => TransformTriggered = true;
        transformAction.canceled += context => TransformTriggered = false;
        //all other action
        // moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        // moveAction.canceled += context =>  MoveInput = Vector2.zero;
    }
   
    private void OnEnabled(){
        transformAction.Enable();
        //moveAction.Enable();
        //and all other action
    }
    private void OnDisable(){
        transformAction.Disable();
        //moveAction.Disable();
    }

    
}
