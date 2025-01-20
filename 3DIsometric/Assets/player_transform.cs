using UnityEngine;

public class TransformHandler : MonoBehaviour
{
    public bool isActive = false;
    private bool isAbleTransform = false;
    public GameObject currentPlayer;
    private GameObject tempPlayer;
    //private IControllable inputHandler;
    private PlayerInputHandler inputHandler;
    
    private void Awake(){
        inputHandler = PlayerInputHandler.Instance;
        Debug.Log(inputHandler != null ? "InputHandler assigned successfully!" : "InputHandler is null!");
    }
    private void Update(){
        HandleTransform();
    }
    void HandleTransform(){
        if(inputHandler.TransformTriggered&&isAbleTransform){
            //currentPlayer = tempPlayer;
            Debug.Log("Pressed E for transform and abled");
            // if(currentPlayer.TryGetComponent<IControllable>(out var controllable)){
            //     inputHandler.SetControllable(controllable);
            //     Debug.Log("Pressed E for transform and abled");
            // }
            

        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster")){
            Debug.Log("monster near able transform");
            //tempPlayer = other.gameObject;
            isAbleTransform = true;
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster")){
            Debug.Log("monster near not able transform");
            isAbleTransform = false;
        }
    }
}
