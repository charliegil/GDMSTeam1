using UnityEngine;

public class player_transform : MonoBehaviour
{
    private bool isAbleTransform = false;
    private PlayerInputHandler inputHandler;
    private void Awake(){
        inputHandler = PlayerInputHandler.Instance;
    }
    private void Update(){
        HandleTransform();
    }
    void HandleTransform(){
        if(inputHandler.TransformTriggered&&isAbleTransform){
            Debug.Log("Pressed E for transform and abled");
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster")){
            Debug.Log("monster near able transform");
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
