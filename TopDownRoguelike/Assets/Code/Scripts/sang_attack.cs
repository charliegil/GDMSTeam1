using UnityEngine;

public class sang_attack : MonoBehaviour
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag=="Player"){
            player_script.dmgPlayer(10);
        }
    }
}
