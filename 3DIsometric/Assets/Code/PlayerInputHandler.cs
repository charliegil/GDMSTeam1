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
    //add other action

    private InputAction transformAction;
    //add other action

    public bool TransformTriggered{ get; private set;}
    //add other action
    public static PlayerInputHandler Instance {get; private set;}

    private void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        } 
        transformAction = playerControls.FindActionMap(actionMapName).FindAction(transformS);
        //add other action

        RegisterInputActions();
    }
    void RegisterInputActions(){
        transformAction.performed += context => TransformTriggered = true;
         transformAction.canceled += context => TransformTriggered = false;
         //all other action
    }
   
    private void OnEnabled(){
        transformAction.Enable();
        //and all other action
    }
    private void OnDisable(){
        transformAction.Disable();
    }

    
}
