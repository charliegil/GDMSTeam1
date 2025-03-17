using UnityEngine;

public class explosiveEnemy : BaseEnemy
{

    
   
     public bool usePathFinding = false;

    public float MaxAliveTime = 4;
    
    public GameObject DamageZonePrefab;  
    private Vector2 destination;

    private bool hasChoosenDirection = false;
 

    public override void Attack()
    {
        Instantiate(DamageZonePrefab, transform.position, Quaternion.identity);
        GetComponent<Health>().OnDeath();
        
    }

    public override void move(){
       currentSpeed = Mathf.Lerp(currentSpeed,maxSpeed,acceleration*Time.deltaTime);
       if(usePathFinding) followPlayer();
       else straightMovement();
    }
    public void followPlayer(){
        destination = getDirectionToPlayer();
        body.linearVelocity = destination * currentSpeed;
    
    }
    public void straightMovement(){
        if(!hasChoosenDirection) {
            destination = getDirectionToPlayer();
            hasChoosenDirection = true;
        }
        body.linearVelocity = destination * currentSpeed;
    }


    // will need to attach the start position, as well as attach the line Renderer for the slime patrol
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        
        base.Start();
    }

    // Update is called once per frame
    // this NPC destroys after one attack, so no need for some advanced ai thing
    public void Update()
    {
        if(IsPlayerInAttackRange() || MaxAliveTime<=0) Attack();

        else if(IsPlayerInDetectionRange()) move();
        
        else{
            body.linearVelocity = new Vector2(0,0);
            currentSpeed = 0;
            hasChoosenDirection = false;

        }
        MaxAliveTime-=Time.deltaTime;

    }
    

    
}

