using UnityEngine;

public class bell_script : MonoBehaviour
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
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("chain collision "+collision.gameObject.tag);
        if(collision.gameObject.CompareTag("Player")){
            Debug.Log("bell collision IS INN"+collision.gameObject.tag);
            EventManager.PlayerTakeDamage(30);
            //player_script.dmgPlayer(10);
        
        }
        // }else{
        //     Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        //     Debug.Log("Chain ignored enemy collision");
        // }
        
    }
    
}
