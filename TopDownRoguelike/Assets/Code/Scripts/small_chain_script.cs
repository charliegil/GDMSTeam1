using Unity.VisualScripting;
using UnityEngine;

public class small_chain_script : MonoBehaviour
{
    private playerControl player_script;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<playerControl>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("chain collision "+collision.gameObject.tag);
        if(collision.gameObject.tag=="Player"){
            Debug.Log("chain collision IS INN"+collision.gameObject.tag);
            EventManager.PlayerTakeDamage(1);
            //player_script.addEffect();
        }else{
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            Debug.Log("Chain ignored enemy collision");
        }
        
    }
}
