using System;
using UnityEngine;

public class SlowPlayerZone : MonoBehaviour
{
    // next step : first, make the skill tree implementation
    // then, make the ennemy that shoots random zones in patrol mode, and targets the player in alert more.
    // but before all of that, make a basic enemy that shoots at the ennemy

    // maybe instead of applying velocity, do a acceleration

    // will need to add sprites for when the zone is pulling, vs not pullin

    public float zoneRadius; // when enter that radius, will start pulling you from that center
    public float SlowMultiplier; // the velocity at which you get pulled towards the center


    public int damagePerTick; // number of damage you take per tick, while being in the zone (damage radius)
    public float tickRate; // in seconds, the tick rate at which you take damage


    public int  lifetime = -1; // is used to specify the lifetime of the object. if not -1, will destroy after x seconds
    
    public bool canEscapeWithDash; // if true, the velocity of the worm will have no effect while dashing. need to implement that
    //public newHealth UIHealth;
    public Sprite sprite;
    public Color spriteColor;
    private CircleCollider2D ZoneCollider; // if you are in this collider, you will get pulled towards the center

    private SpriteRenderer spriteRenderer;

    private float TimeInDamageZone = 0;

    private float reloadCounter = 0;

    private PlayerController playerController;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        ZoneCollider = gameObject.AddComponent<CircleCollider2D>();
        ZoneCollider.radius = zoneRadius;
        ZoneCollider.isTrigger = true;
        
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite  = sprite;
        spriteRenderer.color = spriteColor;

        spriteRenderer.drawMode = SpriteDrawMode.Sliced; // Options: Simple, Sliced, Tiled
        spriteRenderer.size = new Vector2(2*zoneRadius,2* zoneRadius);

        if(lifetime != -1) Destroy(gameObject, lifetime);


    }
    // the enemy will send a projectile a certain distance, then when it reach that distance, it will turn into a worm hole that pull the player
    // will need to separate the two colliders, preferably one in a child object. or just use some length to see if in damage

    private void OnTriggerEnter2D(Collider2D obj){
        if(!(obj.name.Contains("Player") || obj.tag.Contains("Player") ) || isWallBetweenPlayer(obj)) return; 
        
        if(playerController== null) playerController = obj.GetComponent<PlayerController>();

        playerController.applySpeedModifier(SlowMultiplier);

    
    }
    // the conditions for the reload time to embark are these:
    // only if the object has pulled for x amount of time
    private void OnTriggerExit2D(Collider2D obj)
    { // issue with this
        if(!(obj.name.Contains("Player") || obj.tag.Contains("Player") ) || isWallBetweenPlayer(obj)) return; 
        Debug.Log("the player has left the pull zone");
        StopSlowZone();
        
    }

    private void StopSlowZone(){ // called when the zone stops pulling the player. for various reasons
        
        reloadCounter = 0; 
        playerController.applySpeedModifier(1/SlowMultiplier);

    }

    private void OnTriggerStay2D(Collider2D obj){
        // do nothing if there is a wall between the player and object, or if its not the player
        if(!(obj.name.Contains("Player") || obj.tag.Contains("Player") ) || isWallBetweenPlayer(obj)) return; 

        // check if the player is inside the damage zone
        
        if(TimeInDamageZone == -1 || TimeInDamageZone > tickRate){
                TimeInDamageZone = 0;
                takeDamage();
        }
        TimeInDamageZone+=Time.deltaTime;
        
        // end of the damage handler
        // next, handle the pull player par
    }
    
    
    public bool isWallBetweenPlayer(Collider2D obj ){
        Vector2 direction = obj.transform.position - transform.position;
        RaycastHit2D[] hit = castRayAndGetCollider(direction);
        foreach(RaycastHit2D col in hit){
            if(col.transform.gameObject.tag == "Finish") return true;
            else if (col.transform.gameObject.tag == "Player" || col.transform.gameObject.name.Contains("Player")) return false;
        }
        return false;

    }
    RaycastHit2D[] castRayAndGetCollider(Vector2 direction ){
        
        Debug.DrawRay(transform.position, direction);
        //Ray ray = new Ray(transform.position,direction);      
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,direction, zoneRadius, Physics2D.DefaultRaycastLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        return hits;
    }
    private void takeDamage(){
        Debug.Log("spent some time in damage zone, taking damage");
    }

}
