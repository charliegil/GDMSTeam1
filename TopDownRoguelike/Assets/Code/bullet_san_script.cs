using UnityEngine;

public class bullet_san_script : MonoBehaviour
{
    

    // Update is called once per frame
     void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag=="Player"){
            EventManager.PlayerTakeDamage(1);
        }
    }
}
