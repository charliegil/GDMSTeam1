using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class sang_spawn_projectiles : MonoBehaviour
{
    [SerializeField]
    int numberOfProjectiles;

    [SerializeField]
    GameObject projectile;
    public Vector2 startPoint;
    float radius, moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        radius = 5f;
        moveSpeed = 5f;
        SpawnProjectiles(numberOfProjectiles);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void SpawnProjectiles(int numberOfProjectiles){
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;
        for (int i=0; i<= numberOfProjectiles - 1; i++){
            float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI)/180) * radius;
            float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI)/180) * radius;

            Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
            Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * moveSpeed;
            var proj = Instantiate(projectile, startPoint, Quaternion.identity);
            proj.transform.Rotate(0, 0, Mathf.Atan2(projectileMoveDirection.y, projectileMoveDirection.x) * Mathf.Rad2Deg);
            proj.GetComponent<Rigidbody2D>().linearVelocity = new Vector2 (projectileMoveDirection.x, projectileMoveDirection.y);
            angle += angleStep;

        }
    }
}
