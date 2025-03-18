using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public float damage = 10;

    private Coroutine destroyObject = null;
    void OnTriggerEnter2D(Collider2D collision){
        
        if(collision.gameObject.layer==3 && collision is CapsuleCollider2D){
            
            Debug.Log("detect enemy ");
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
            //Destroy(collision.gameObject);
        }
    }
    public void Start(){
        Destroy(gameObject, 10f);
    }
    

}
