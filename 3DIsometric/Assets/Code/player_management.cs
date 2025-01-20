using UnityEngine;

public class player_management : MonoBehaviour
{
    public GameObject player;
    private bool isAbleTransform = false;

    // Update is called once per frame
    void Update()
    {
        if(isAbleTransform == true && Input.GetKeyDown("E")){
            GameEvents.current.PressETransform();
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
