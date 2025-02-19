using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject damageZonePrefab;

    public int range = 10; // will not stop until it has traveled this amount of ground
    
    

    private Vector3 StartPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Hello from projectile");
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - StartPosition).magnitude > range){
            Debug.Log("the projectile has reached its range. transforming into a damage zone now");
            Destroy(gameObject);
            GameObject damageZone = Instantiate(damageZonePrefab, transform.position, Quaternion.identity);
        }
        else{
        transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void SetDirection(Vector3 direction) {
        this.direction = direction;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit player");
            // Damage player here
            Destroy(gameObject);

            // Get point of collision
            Vector3 pointOfContact = GetComponent<Collider2D>().ClosestPoint(other.transform.position);

            GameObject damageZone = Instantiate(damageZonePrefab, pointOfContact, Quaternion.identity);
        }
    }
}
