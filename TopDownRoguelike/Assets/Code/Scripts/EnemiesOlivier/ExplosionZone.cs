using UnityEngine;

public class ExplosionZone : MonoBehaviour
{
    // next step : first, make the skill tree implementation
    // then, make the ennemy that shoots random zones in patrol mode, and targets the player in alert more.
    // but before all of that, make a basic enemy that shoots at the ennemy

    // maybe instead of applying velocity, do a acceleration

    public float damage;

    public float  lifetime = -1;

    private CircleCollider2D damageCollider; // if you are in this collider, you will get pulled towards the center

    private bool hasPlayerTakenDamage = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    { 
        if(lifetime!= -1) Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D obj){

    }
    private void OnTriggerStay2D(Collider2D obj){
      
        if(!(obj.name.Contains("Player") || obj.tag.Contains("Player") ) ) return; 
        
        
        if(hasPlayerTakenDamage) return;

        hasPlayerTakenDamage = true;
        takeDamage();


    
    }
    private void takeDamage(){    
        EventManager.PlayerTakeDamage(damage);
    }

}
