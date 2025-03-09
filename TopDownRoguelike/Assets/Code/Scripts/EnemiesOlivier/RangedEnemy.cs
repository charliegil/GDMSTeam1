using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    [SerializeField] GameObject projectilePrefab;


    [SerializeField] int numberOfProjectiles = 5; // the total projectiles to lauch. -1 means its infinite

    [SerializeField] float timeBetweenProjectile = 1; // the time between each projectile

    [SerializeField] float approachingRange = 10;

    [SerializeField] float detectionRange = 100;


    private bool isRetreating = false;

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
    void Start()
    {
        // Find player
        player = GameObject.FindGameObjectWithTag("Player");
        //PerformAttack();
        timer = timeBetweenProjectile;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isRetreating && (player.transform.position-transform.position).magnitude<2){
            destination = -getDirectionToPlayer().normalized;
            isRetreating = true;
            Debug.Log("is retreating");
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
    bool IsPlayerInAttackRange(){
        return getDirectionToPlayer().magnitude < attackRange;
    }

    bool IsPlayerInApproachingRange(){
        return getDirectionToPlayer().magnitude < approachingRange;
    }
    bool IsPlayerInDetectionRange(){
        return getDirectionToPlayer().magnitude < detectionRange;
    }
    Vector2 getDirectionToPlayer(){
        return (player.transform.position - transform.position);
    }
    

    public void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        isRetreating = true;
    }

    public override void OnDeath()
    {
        throw new System.NotImplementedException();
    }
    
    public override void Attack()
    {
        
        Shoot();
    }

}
