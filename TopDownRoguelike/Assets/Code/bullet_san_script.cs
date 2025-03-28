using UnityEngine;

public class bullet_san_script : MonoBehaviour
{
    

    // Update is called once per frame
     void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("bullet hell got something"+other.gameObject.tag);
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("bullet sang hit Player");
            EventManager.PlayerTakeDamage(1);
            Destroy(gameObject);
        }
    }
}
