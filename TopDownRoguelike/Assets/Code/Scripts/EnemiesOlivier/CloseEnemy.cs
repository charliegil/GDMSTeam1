using System.ComponentModel;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Reflection;
using Pathfinding.Util;



public class CloseEnemy : BaseEnemy
{
    [SerializeField] private float maxHp = 100;
    //[SerializeField] private int damage = 5;
    public float radiusCircularAttack;

    public float frontAttackRange;
    public float trackingSpeed; // the rotation speed of the enemy

    public int lineOfSightAngle = 180; // Is the FOV of the enemy

    public float attackReload = 1;  // the time between each attack

    public float reactionTime = 0;

    public float movingSpeed = 1; // the speed when he sees you

    public float findSpeed  =1; // the speed when he doesnt see you

    public float attackDuration = 0.2f; // the duration of the attack


    public float RateOfChangeDirection = 2f; // Time before changing direction
    

    public GameObject tongue;
    public SpriteRenderer tongueRenderer;
    private Sprite tongueSprite; 

    private CircleCollider2D attackCollider; // t
    private Collider2D PlayerCollider;
    
    private Quaternion targetRotation;
    private Vector2 randomMovement = new Vector2( 0,0);
    
    
    private bool IsAttacking = false; //beause of the coroutines
    private float TimeBeforeAttack; 
    
    
    

    private bool isChasing =false;



    private void Start()
    {
        base.Start();
        TimeBeforeAttack = attackReload;
        attackCollider = gameObject.AddComponent<CircleCollider2D>();
        attackCollider.radius = frontAttackRange;
        attackCollider.isTrigger = true;
        //gameObject.AddComponent<SpriteRenderer>().sprite = tongueSprite; 

        player = GameObject.FindGameObjectWithTag("Player");
        
        PlayerCollider = player.GetComponent<Collider2D>();
        
        
        gameObject.AddComponent<DrawFOV>().drawLines(viewDistance,FOV);
        
        tongueRenderer.enabled = false;
        
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        if(IsAttacking || IsPlayerInAttackRange()) {
            body.linearVelocity = new Vector2(0,0);
            return;
        }
        
        move();
    }

    private void LateUpdate()
    {
        if (IsPlayerInlineOfSight()){
            Vector2 direction = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            randomMovement  =direction;
            timer = RateOfChangeDirection;

            float newAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, trackingSpeed);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
        else{
            float angle = Mathf.Atan2(randomMovement.y, randomMovement.x) * Mathf.Rad2Deg; 

            float newAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, trackingSpeed);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }

    }
    private void followLastKnownPosition(){
        isChasing = false;
        timer = RateOfChangeDirection+3;
        randomMovement = (player.transform.position - transform.position).normalized;
    }

    private void dontSeePlayer(){ // called when the enemy doesnt see the player 
        
       
           if (timer<=0){ // need to pick a new direction to go in
                randomMovement = UnityEngine.Random.insideUnitCircle.normalized;
                timer = RateOfChangeDirection;  
            }
            timer-= Time.deltaTime;
            
            
            body.linearVelocity= transform.right*findSpeed;
    
    }


    private void followPlayer(){
        body.linearVelocity= transform.right*movingSpeed;
        isChasing = true;
    }

    private bool IsPlayerInlineOfSight(){
        
        if(isWallBetweenPlayer(player)) return false; // if there is a wall between the player and the enemy

        Vector2 direction = (player.transform.position - transform.position);
        float distanceToPlayer = direction.magnitude;
        
        if(distanceToPlayer > viewDistance) return false;
        
        direction.Normalize();
        
        float angle = Vector2.Angle(transform.right, direction);
        
        if (angle > (lineOfSightAngle/2)) return false;
        return true;
    }


    private float getDistanceToPlayer(){
        return(transform.position- player.transform.position).magnitude;
    }




    public override void Attack(){
        
        TimeBeforeAttack = attackReload;
        if( getDistanceToPlayer() < radiusCircularAttack){
            IsAttacking = true;
            //StartCoroutine(FrontAttack());
            StartCoroutine(CircularAttack());
        }
        else if (IsPlayerInlineOfSight()){
            Debug.Log("doing front attack");
            IsAttacking = true;
            StartCoroutine(FrontAttack());
        }
        
        
    }
   
    private IEnumerator FrontAttack(){
        
        //tongue.transform.eulerAngles= new Vector3(0,0,-90); 
        float duration = attackDuration;
        float speedRate = 2 * frontAttackRange / duration; 
        tongue.transform.localScale = new Vector3(1, 0, 0); 
        tongueRenderer.enabled = true;
        bool reachEnd = false;
        
        while(duration > 0){
            
            if(reachEnd) tongue.transform.localScale -= new Vector3(0, speedRate * Time.deltaTime, 0);
            else {tongue.transform.localScale += new Vector3(0, speedRate * Time.deltaTime, 0);}
            
            if (tongue.transform.localScale.y >= frontAttackRange){
                reachEnd = true;
                tongue.transform.localScale = new Vector3(tongue.transform.localScale.x, frontAttackRange, tongue.transform.localScale.z); // Clamp y to frontAttackRange
            }

            if(tongue.transform.localScale.y < 0) break;
            
            duration -= Time.deltaTime;
            yield return null;
        
        }
        //tongue.transform.eulerAngles= new Vector3(0,0,-90); 
        TimeBeforeAttack = attackReload;
        IsAttacking = false;
        tongueRenderer.enabled = false;
        tongue.transform.localScale =new Vector3(1, radiusCircularAttack,0); // return it to normal
    }
    private IEnumerator CircularAttack(){
        float duration  = attackDuration;
        tongueRenderer.enabled = true;
        float step = 360f / attackDuration;
   
        while(duration > 0){
            float angle = step * Time.deltaTime;
            tongue.transform.Rotate(new Vector3(0,0,angle));
            

            duration -= Time.deltaTime;
           
            yield return null;
        }
        TimeBeforeAttack = attackReload;
        IsAttacking = false;
        tongueRenderer.enabled = false;
        tongue.transform.localRotation = Quaternion.Euler(0, 0, -90);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsAttacking) return;
        
        if(!collision.gameObject.CompareTag("Player")) return;
        
        Invoke("Attack", reactionTime); 
        
    }
    private void OnTriggerStay2D(Collider2D collision) // when enemy sees the player and in line of sight, instantly attack. if was already in 
    // line of sight, there is a counter that will be counted
    {
        if(IsAttacking) return;
        
        if(TimeBeforeAttack <= 0 ){
            Attack();

        }
        else TimeBeforeAttack -= Time.deltaTime;
    }

    private RaycastHit2D[] castRayAndGetCollider(Vector2 direction ){
        
        Debug.DrawRay(transform.position, direction);
        //Ray ray = new Ray(transform.position,direction);      
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,direction, viewDistance, Physics2D.DefaultRaycastLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        return hits;
    }


    

    public override void move()
    {
        if(IsPlayerInlineOfSight()) {
            
            followPlayer();
            Debug.Log("is in sight");
        }
        else{
            if(isChasing){
                followLastKnownPosition();
            }
            dontSeePlayer();
            Debug.Log("dont see it");
        }
    }

   
    
}
