using UnityEngine;

public class sang_attack : MonoBehaviour
{
    
    private bool didDamage = false;
    // Update is called once per frame

    void Start()
    {
        GetComponent<Collider2D>().enabled = false;
        Invoke("setColliderActive", 0.2f);
    }
    private void setColliderActive(){
         GetComponent<Collider2D>().enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("white hit "+other.gameObject.tag);
        if(other.gameObject.CompareTag("Player") && !didDamage){
            Debug.Log("white hit player yeh!");
            EventManager.PlayerTakeDamage(5);
            didDamage = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        
        if(other.gameObject.CompareTag("Player")){
            didDamage = false;
            
        }
    }
}
