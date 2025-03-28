using UnityEngine;

public class sang_attack : MonoBehaviour
{
    

    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("white hit "+other.gameObject.tag);
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("white hit player yeh!");
            EventManager.PlayerTakeDamage(5);
        }
    }
}
