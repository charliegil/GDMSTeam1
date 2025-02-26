using System.ComponentModel;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System.Reflection;



public class CloseEnemy : MonoBehaviour
{
    public int damage;
    public float radiusCircularAttack;

    public float frontAttackRange;
    public float trackingSpeed; // the rotation speed of the enemy

    public int hp =  100;

    public int lineOfSightAngle = 180; // Is the FOV of the enemy

    public int viewDistance = 5; 
    public float attackReload = 1;  // the time between each attack
    private float TimeBeforeAttack; 

    
    public float reactionTime = 0;

    public float movingSpeed = 1; // the speed when he sees you

    public float findSpeed  =1; // the speed when he doesnt see you

    public float attackDuration = 0.2f; // the duration of the attack

    private CircleCollider2D attackCollider; // t

    public GameObject player;

    public GameObject tongue;
    public SpriteRenderer tongueRenderer;
    private Sprite tongueSprite; 

    private LineRenderer FOVLines;
    private Collider2D PlayerCollider;
    
    private bool IsAttacking = false; //beause of the coroutines
    public float RateOfChangeDirection = 2f; // Time before changing direction
    private float timer;
    
    private Quaternion targetRotation;

    private Vector2 randomMovement = new Vector2( 0,0);

    private Rigidbody2D rb;

    //private float currentTime = 0;

    // when the player is in the line of sight of the enemy, the enemy will follow him and face him forward. 
    // when the player is in the attack radius and in view of the enemy, the enemy will do an attack. 
    // if the enemy doesnt see the player, it will walk in different directions, and face in these directions

    // The enemy has two attacks. when in field of view, perform front attack. When not in field of view but in the attack radius, perform the circular attack
    
    
    /* the description of the enemy is this: this enemy is aware of his surroundings. if he sees you, he will follow you and use his 
    tongue has a spear when you are close to him. His ears are very good. if you are close to him and he doesnt see you, he will know.
    he will then use his tongue and make a circular attack all around him

    
    could implement the fact that enemies cant see you when you are dashing

    Still need to implement the cone that tells us the FOV of the enemy

    will need to make this class a child of BaseEnemy and implement Pathfinding
    */
    void Start()
    {
       
        TimeBeforeAttack = attackReload;
        attackCollider = gameObject.AddComponent<CircleCollider2D>();
        attackCollider.radius = frontAttackRange;
        attackCollider.isTrigger = true;
        //gameObject.AddComponent<SpriteRenderer>().sprite = tongueSprite; 

        player = GameObject.FindGameObjectWithTag("Player");
        
        PlayerCollider = player.GetComponent<Collider2D>();

        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        
        
        setLineRenderer();
        
        
        
        tongueRenderer.enabled = false;
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        if(IsAttacking) {
            rb.linearVelocity = new Vector2(0,0);
            return;
        }
        
        if(IsPlayerInlineOfSight()) {
            
            followPlayer();
            Debug.Log("is in sight");
        }
        else{
         
            dontSeePlayer();
            Debug.Log("dont see it");
        }
    }

    void LateUpdate()
    {
        if (IsPlayerInlineOfSight()){
            Vector2 direction = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            randomMovement  =direction;
            timer = RateOfChangeDirection;

            float newAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, trackingSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
}
        else{
            float angle = Mathf.Atan2(randomMovement.y, randomMovement.x) * Mathf.Rad2Deg; 

            float newAngle = Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, trackingSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }

    }

    void dontSeePlayer(){ // called when the enemy doesnt see the player 
        
       
           if (timer<=0){ // need to pick a new direction to go in
                randomMovement = UnityEngine.Random.insideUnitCircle.normalized;
                timer = RateOfChangeDirection;  
            }
            timer-= Time.deltaTime;
            
            
            rb.linearVelocity= transform.right*findSpeed;
    
    }


    void followPlayer(){
        rb.linearVelocity= transform.right*movingSpeed;
    }

    bool IsPlayerInlineOfSight(){
        
        if(isWallBetweenPlayer(player)) return false; // if there is a wall between the player and the enemy

        Vector2 direction = (player.transform.position - transform.position);
        float distanceToPlayer = direction.magnitude;
        
        if(distanceToPlayer > viewDistance) return false;
        
        direction.Normalize();
        
        float angle = Vector2.Angle(transform.right, direction);
        
        if (angle > (lineOfSightAngle/2)) return false;
        return true;
    }


    float getDistanceToPlayer(){
        return(transform.position- player.transform.position).magnitude;
    }




    void Attack(){
        
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
        float duration  = attackDuration;
        float speedRate =2*frontAttackRange / duration;
        tongue.transform.localScale = new Vector3(0.5f,0,0);
        tongueRenderer.enabled = true;
        bool reachEnd = false;
        
        while(duration > 0){
            if(reachEnd) tongue.transform.localScale-=new Vector3(0,speedRate*Time.deltaTime,0);
            else {tongue.transform.localScale+=new Vector3(0,speedRate*Time.deltaTime,0);}
            if(tongue.transform.localScale.y >= frontAttackRange) {
                reachEnd = true;
                Vector3.ClampMagnitude(tongue.transform.localScale,frontAttackRange);
            }
            if(tongue.transform.localScale.y < 0) break;
            
            duration -= Time.deltaTime;
           
            yield return null;
        }
        //tongue.transform.eulerAngles= new Vector3(0,0,-90); 
        TimeBeforeAttack = attackReload;
        IsAttacking = false;
        tongueRenderer.enabled = false;
        tongue.transform.localScale =new Vector3(0.5f , radiusCircularAttack,0); // return it to normal
    }
    private IEnumerator CircularAttack(){
        float duration  = attackDuration;
        tongueRenderer.enabled = true;
        while(duration > 0){
            tongue.transform.Rotate(new Vector3(0,0,25));

            duration -= Time.deltaTime;
           
            yield return null;
        }
        TimeBeforeAttack = attackReload;
        IsAttacking = false;
        tongueRenderer.enabled = false;
        tongue.transform.localRotation = Quaternion.Euler(0, 0, -90);
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsAttacking) return;
        
        if(!collision.gameObject.CompareTag("Player")) return;
        
        Invoke("Attack", reactionTime); 
        
    }
    void OnTriggerStay2D(Collider2D collision) // when enemy sees the player and in line of sight, instantly attack. if was already in 
    // line of sight, there is a counter that will be counted
    {
        if(IsAttacking) return;
        
        if(TimeBeforeAttack <= 0 ){
            Attack();

        }
        else TimeBeforeAttack -= Time.deltaTime;
    }
    public bool isWallBetweenPlayer(GameObject obj ){
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,direction, viewDistance, Physics2D.DefaultRaycastLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        return hits;
    }
    Vector3 RotateVector(Vector3 v, float degrees){
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector3(v.x*cos-v.y*sin, v.x*sin + v.y*cos);
    }
    private void setLineRenderer(){
        // Start of the FOV renderer
        float lineWidth = 0.08f;
        FOVLines = GetComponent<LineRenderer>();
        if (FOVLines == null) FOVLines = gameObject.AddComponent<LineRenderer>();
        FOVLines.startWidth = lineWidth;
        FOVLines.endWidth = lineWidth;
        FOVLines.useWorldSpace = false;
        FOVLines.sortingLayerName = "Default";  
        FOVLines.sortingOrder = 10;

        Vector3 start = transform.position;
        Vector3 dir1 = RotateVector(Vector3.right, lineOfSightAngle/2);
        Vector3 dir2 = RotateVector(Vector3.right, -lineOfSightAngle/2);

        Vector3[] arcPoints = GenerateArc(start, start + dir1, start + dir2, 20);
        FOVLines.positionCount = 4 + arcPoints.Length; 
        
        FOVLines.SetPosition(0, start);
        FOVLines.SetPosition(1, start + dir2 * viewDistance);
        FOVLines.SetPosition(2, start);
        FOVLines.SetPosition(3, start + dir1 * viewDistance);
        
        for (int i = 0; i < arcPoints.Length; i++)
        {
            FOVLines.SetPosition(i + 4, arcPoints[i]);
        }
        // End of the FOV renderer
    }
    
    
    Vector3[] GenerateArc(Vector3 center, Vector3 pointA, Vector3 pointB, int resolution){
        float radius = (pointA-center).magnitude;
        float startAngle = 0;
        float endAngle = Vector3.SignedAngle((pointA-center), (pointB - center), transform.forward);
        

        float step = (endAngle -startAngle)/ resolution;
        float angle = startAngle;
        Vector3[] arcPoints = new Vector3[resolution];
        for(int i = 0; i < resolution; i++){
            angle += step;
            if(startAngle > endAngle)Debug.Log("there is a problem with circle generation");
            arcPoints[i] = viewDistance * (RotateVector((pointA) , angle)); 
        }

        return arcPoints;
    }
}
