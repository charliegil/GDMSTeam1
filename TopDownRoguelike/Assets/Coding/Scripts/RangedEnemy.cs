using UnityEngine;

public class RangedEnemy : Player
{
    private GameObject player;
    [SerializeField] GameObject projectilePrefab;

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
        PerformAttack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
