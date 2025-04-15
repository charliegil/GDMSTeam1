using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    public GameObject projectilePrefab;
    private Animator anim;

/// <summary>
/// the total projectiles to lauch. -1 means its infinite
/// </summary>
    [SerializeField] private int numberOfProjectiles = 5; 
    [SerializeField] private float timeBetweenProjectile = 1; // the time between each projectile

/// <summary>
/// when chasing towards the player, will stop at that range
/// </summary>
    [SerializeField] private float approachingRange = 10;


    public bool isRetreating = false;

    private Vector2 destination;

    private int projectilesLauched = 0;

    private bool hasReachedDestination = false;

    


    private void Shoot() {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 projectileDirection = (player.transform.position - transform.position).normalized;
        projectile.GetComponent<EnemyProjectile>().SetDirection(projectileDirection);
        projectilesLauched++;
        timer = timeBetweenProjectile;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        // Find player
        
        //PerformAttack();
        timer = timeBetweenProjectile;
    }

    // Update is called once per frame
    private void Update()
    {
        if(!isRetreating && getDirectionToPlayer().magnitude<2){
            destination = -getDirectionToPlayer().normalized;
            isRetreating = true;
            Debug.Log("is retreating");
            //TakeDamage(12);
        }
        
        if(!IsPlayerInAttackRange()){
            isRetreating = false;
            move();
            timer = 0.2f;
            hasReachedDestination = false;
        }
        else{
           if(!IsPlayerInApproachingRange() && !hasReachedDestination){
                move();
           }
           else{
            hasReachedDestination = true;
           }
            if(hasReachedDestination && !isRetreating) {
                
                body.linearVelocity = new Vector2(0,0);
                currentSpeed = 0.4f;
            }

        
            
           if(projectilesLauched< numberOfProjectiles && timer < 0) Attack();

        }
        if(isRetreating){
            move();
        }
        if(!IsPlayerInDetectionRange()){
            body.linearVelocity = new Vector2(0,0);
        }
        

         timer -= Time.deltaTime;
    }
    public override void move(){
        currentSpeed = Mathf.Lerp(currentSpeed,maxSpeed,acceleration);
        if(isRetreating){
            body.linearVelocity = destination.normalized * maxSpeed;
        }
        else{
        body.linearVelocity = getDirectionToPlayer().normalized * currentSpeed;
        }
        
    }
    private bool IsPlayerInApproachingRange(){
        return getDirectionToPlayer().magnitude < approachingRange;
    }
    
    
    public override void Attack()
    {
        anim.SetTrigger("attack");
        Shoot();
    }

    public override void updateStatsFromCurrentWave(){
        
    }
}
