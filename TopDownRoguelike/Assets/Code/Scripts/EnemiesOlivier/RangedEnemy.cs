using UnityEngine;

public class RangedEnemy : MonoBehaviour , IEnemyBehaviour
{
    private GameObject player;
    [SerializeField] GameObject projectilePrefab;

    [SerializeField] Rigidbody2D body;

    [SerializeField] int numberOfProjectiles = 5; // the total projectiles to lauch. -1 means its infinite

    [SerializeField] float timeBetweenProjectile = 1; // the time between each projectile

    [SerializeField] float attackRange = 10;

    [SerializeField] float detectionRange = 100;

    [SerializeField] float speed;
    
    private bool isInAttackRange = false;
    private bool isRetreating = false;

    private Vector2 destination;

    private float timer =  0;

    private int projectilesLauched = 0;

    public void Attack()
    {
        Debug.Log(player.transform.position);
        Shoot();
    }

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
        if(IsPlayerInAttackRange()){
            if(!isRetreating) body.linearVelocity = new Vector2(0,0);
            
            if(projectilesLauched< numberOfProjectiles && timer < 0) Attack();
        }
        else{
            isRetreating = false;
            move();
            timer = 0.2f;
        }
        timer -= Time.deltaTime;
    }
    public void move(){
        if(isRetreating){
            body.linearVelocity = destination.normalized * speed;
        }
        body.linearVelocity = getDirectionToPlayer().normalized * speed;
    }
    bool IsPlayerInAttackRange(){
        return getDirectionToPlayer().magnitude < attackRange;
    }
    Vector2 getDirectionToPlayer(){
        return (player.transform.position - transform.position);
    }
    

    public void takeDamage()
    {
        destination = -getDirectionToPlayer().normalized;
        isRetreating = true;
    }

    public void OnDeath()
    {
        throw new System.NotImplementedException();
    }
}
