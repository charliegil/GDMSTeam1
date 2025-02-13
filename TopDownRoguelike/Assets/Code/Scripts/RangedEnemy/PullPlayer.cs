using System;
using UnityEngine;

public class PullPlayer : MonoBehaviour
{
    // next step : first, make the skill tree implementation
    // then, make the ennemy that shoots random zones in patrol mode, and targets the player in alert more.
    // but before all of that, make a basic enemy that shoots at the ennemy

    public Sprite spritePull;
    // will need to add sprites for when the zone is pulling, vs not pulling
    public Color colorPullZone = Color.white;
    public Color colorDamagezone = Color.red;

    public float pullRadius; // when enter that radius, will start pulling you from that center
    public float pullVelocity; // the velocity at which you get pulled towards the center

    public float damageRadius; // the radius at which you need to be in whhich you take damage
    public int damagePerTick; // number of damage you take per tick, while being in the zone (damage radius)
    public float tickRate; // in seconds, the tick rate at which you take damage

    public float stopsAfterTime; // when you start to get pulled, it will stop after the time indicated by this variable

    public float reloadtime; // used with variable on top, specifies the reload time before it can start pulling things again

    public int  lifetime = -1; // is used to specify the lifetime of the object. if not -1, will destroy after x seconds

    public bool canMoveWhilePulled = true; // will need to implement this

    private bool canPull = true;

    private float timePassedPull = 0; // indicates for how long the object has spent pulling the player
    
    public bool canEscapeWithDash; // if true, the velocity of the worm will have no effect while dashing. need to implement that
    //public newHealth UIHealth;

    private CircleCollider2D PullCollider; // if you are in this collider, you will get pulled towards the center


    private float TimeInDamageZone = 0;

    private float reloadCounter = 0;

    private PlayerController playerController;

    private SpriteRenderer PullRenderer;
    private SpriteRenderer DamageRenderer; 
    

    private Vector2 currentDirection = new Vector2(0, 0);

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        PullCollider = gameObject.AddComponent<CircleCollider2D>();
        PullCollider.radius = pullRadius;
        PullCollider.isTrigger = true;
        
        PullRenderer = gameObject.AddComponent<SpriteRenderer>();
        PullRenderer.sprite  = spritePull;
        PullRenderer.color = colorPullZone;

        PullRenderer.drawMode = SpriteDrawMode.Sliced; // Options: Simple, Sliced, Tiled
        PullRenderer.size = new Vector2(2*pullRadius,2* pullRadius);

        if(lifetime!= -1) Destroy(gameObject, lifetime);

        GameObject damageZone = new GameObject("zoneDamage");
        damageZone.transform.SetParent(transform);
        damageZone.transform.localPosition = Vector3.zero;

        DamageRenderer = damageZone.AddComponent<SpriteRenderer>();
        DamageRenderer.sprite = spritePull;

        DamageRenderer.drawMode = SpriteDrawMode.Sliced; 
        DamageRenderer.size = new Vector2(2*damageRadius, 2*damageRadius);
        DamageRenderer.color = colorDamagezone;

    }

    void Update(){
        if(reloadCounter > reloadtime && canPull == false) canPull = true; // maybe issue with this why its not working properly
        else {reloadCounter+=Time.deltaTime;}


    }
    // the enemy will send a projectile a certain distance, then when it reach that distance, it will turn into a worm hole that pull the player
    // will need to separate the two colliders, preferably one in a child object. or just use some length to see if in damage

    private void OnTriggerEnter2D(Collider2D obj){
        if(!(obj.name.Contains("Player") || obj.tag.Contains("Player") ) || isWallBetweenPlayer(obj)) return; 
        
        if(playerController== null) playerController = obj.GetComponent<PlayerController>();

        if(!canPull) return;

        playerController.setCanMove(canMoveWhilePulled);

        /*playerController.addExternalVelocity(-currentDirection);
        currentDirection = pullVelocity*(transform.position - obj.transform.position).normalized;
        playerController.addExternalVelocity(currentDirection); */

        

    
    }
    // the conditions for the reload time to embark are these:
    // only if the object has pulled for x amount of time
    private void OnTriggerExit2D(Collider2D obj)
    { // issue with this
        if(!(obj.name.Contains("Player") || obj.tag.Contains("Player") ) || isWallBetweenPlayer(obj)) return; 
        Debug.Log("the player has left the pull zone");
        if(!canPull) return;
        EndPull();
        
    }

    private void EndPull(){ // called when the zone stops pulling the player. for various reasons
        playerController.addExternalVelocity(-currentDirection);
        currentDirection = new Vector2(0,0);
        canPull = false;
        reloadCounter = 0; 
        timePassedPull = 0;
        playerController.setCanMove(true);

    }

    private void OnTriggerStay2D(Collider2D obj){
        // do nothing if there is a wall between the player and object, or if its not the player
        if(!(obj.name.Contains("Player") || obj.tag.Contains("Player") ) || isWallBetweenPlayer(obj)) return; 

        // check if the player is inside the damage zone
        if(Vector2.Distance(transform.position, obj.transform.position) <=damageRadius ){
            if(TimeInDamageZone == -1 || TimeInDamageZone > tickRate){
                TimeInDamageZone = 0;
                takeDamage();
            }
            TimeInDamageZone+=Time.deltaTime;
        }
        else{
            TimeInDamageZone = -1;
        }
        // end of the damage handler
        if(!canPull) return; // can move this part to the top to say that if the object is not pulling the player, the damage zone does nothing
        // next, handle the pull player part
        timePassedPull+= Time.deltaTime;
        
        
        if(timePassedPull > stopsAfterTime){
            EndPull();
            return;
        }
        

        playerController.addExternalVelocity(-currentDirection);
        if((transform.position - obj.transform.position).magnitude > 0.1){
        currentDirection = pullVelocity*(transform.position - obj.transform.position).normalized;
        playerController.addExternalVelocity(currentDirection);
        }
        else{
            currentDirection = new Vector2(0,0);
        }


    
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,direction, pullRadius, Physics2D.DefaultRaycastLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        return hits;
    }
    private void takeDamage(){
        Debug.Log("spent some time in damage zone, taking damage");
    }

}
