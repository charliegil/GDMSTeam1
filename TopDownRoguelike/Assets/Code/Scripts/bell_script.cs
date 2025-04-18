using UnityEngine;

public class bell_script : MonoBehaviour
{
    private playerControl player_script;

    private bool DidDamage = false;

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
        if(collision.gameObject.CompareTag("Player") && !DidDamage){
            Debug.Log("bell collision IS INN"+collision.gameObject.tag);
            DidDamage = true;
            EventManager.PlayerTakeDamage(30);
            Debug.Log("the type of the collider: " + collision.GetComponent<Collider2D>().GetType().ToString());
            AudioManager.instance.PlaySound("BossBell");
            //player_script.dmgPlayer(10);
        
        }
        // }else{
        //     Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        //     Debug.Log("Chain ignored enemy collision");
        // }
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) DidDamage = false;
    }

}
