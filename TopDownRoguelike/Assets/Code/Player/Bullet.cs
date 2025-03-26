
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start(){
        transform.localScale = new Vector3(BulletState.size,BulletState.size, BulletState.size);
        Destroy(gameObject, 10f);
    }
    void OnTriggerEnter2D(Collider2D collision){
        
        if(collision.gameObject.layer == 3 && collision is CapsuleCollider2D){
            collision.gameObject.GetComponent<Health>().TakeDamage(BulletState.damage);
            Destroy(gameObject);
        }
    }

}
