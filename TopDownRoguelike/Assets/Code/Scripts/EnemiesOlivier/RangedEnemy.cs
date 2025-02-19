using UnityEngine;

public class RangedEnemy : Player
{
    private GameObject player;
    [SerializeField] GameObject projectilePrefab;

    [SerializeField] int numberOfProjectiles = 5; // the total projectiles to lauch. -1 means its infinite

    [SerializeField] float timeBetweenProjectile = 1; // the time between each projectile

    private float time =  0;

    private int projectilesLauched = 0;

    public override void HandleInput()
    {
        throw new System.NotImplementedException();
    }

    public override void HandleMovement()
    {
        throw new System.NotImplementedException();
    }

    public override void PerformAttack()
    {
        Debug.Log(player.transform.position);
        Shoot();
    }

    private void Shoot() {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Vector2 projectileDirection = (player.transform.position - transform.position).normalized;
        projectile.GetComponent<EnemyProjectile>().SetDirection(projectileDirection);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Find player
        player = GameObject.FindGameObjectWithTag("Player");
        //PerformAttack();
        time = timeBetweenProjectile;
    }

    // Update is called once per frame
    void Update()
    {
        if(time >= timeBetweenProjectile && ((projectilesLauched < numberOfProjectiles)  || numberOfProjectiles ==-1) ){
            time = 0;
            PerformAttack();
            projectilesLauched++;
        }
        else{
            time+=Time.deltaTime;
        }
    }
}
