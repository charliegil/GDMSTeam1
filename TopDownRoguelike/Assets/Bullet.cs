using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision){
        
        if(collision.gameObject.layer==3 && collision is CapsuleCollider2D){

            Debug.Log("detect enemy ");
            Destroy(gameObject);
            collision.gameObject.GetComponent<Health>().TakeDamage(10);
            //Destroy(collision.gameObject);
        }
    }

}
