using UnityEngine;

public class wallDestroy : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer!=9){
            Destroy(collision.gameObject);
        }
    }
}
