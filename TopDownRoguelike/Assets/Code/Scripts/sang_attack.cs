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

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("white hit "+other.gameObject.tag);
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("white hit player ");
            EventManager.PlayerTakeDamage(5);
        }
    }
}
