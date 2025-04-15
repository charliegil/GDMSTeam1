
using UnityEngine;

public class small_chain_script : MonoBehaviour
{
    private playerControl player_script;

    public int damage =3 ; 

    private bool didDamage = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<playerControl>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("chain collision "+collision.gameObject.tag);
        if(collision.gameObject.CompareTag("Player") && !didDamage){
            Debug.Log("chain collision IS INN"+collision.gameObject.tag);
            didDamage = true;
            if(damage != 0) EventManager.PlayerTakeDamage(damage);
            
            //player_script.addEffect();
        }
        // }else{
        //     Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        //     Debug.Log("Chain ignored enemy collision");
        // }
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) didDamage = false;
    }
}
