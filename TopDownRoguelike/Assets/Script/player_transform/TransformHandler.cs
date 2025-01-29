
using UnityEngine;

public class TransformHandler : MonoBehaviour
{
    public bool isActive = false;
    private bool isAbleTransform = false;
    public GameObject currentPlayer;
    private GameObject tempPlayer;
    private PlayerInputHandler inputHandler;
    
    private void Awake(){
        inputHandler = GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputHandler>();
    }
    private void Update(){
        HandleTransform();
    }
    protected void HandleTransform(){
        if(inputHandler.TransformTriggered&&isAbleTransform){
            currentPlayer = tempPlayer;
            Debug.Log("Pressed E for transform and abled");
            if (currentPlayer.TryGetComponent<IControllable>(out var controllable))
            {
                // Activate or perform the required action on the IControllable
                controllable.ActivateInput();
                TryGetComponent<IControllable>(out var currentControl);
                currentControl.DisactivateInput();
                
                Debug.Log("Activated IControllable on another");
            }
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster")){
            Debug.Log("monster near able transform");
            tempPlayer = other.gameObject;
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
