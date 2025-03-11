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
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("chain collision "+collision.gameObject.tag);
        if(collision.gameObject.tag=="Player"){
            Debug.Log("bell collision IS INN"+collision.gameObject.tag);
            
            player_script.dmgPlayer(10);
        }
        
    }
    
}
