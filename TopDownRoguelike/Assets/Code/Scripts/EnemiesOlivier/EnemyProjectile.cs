using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private float speed = 5f;
    [SerializeField] private GameObject damageZonePrefab;

    public int range = 10; // will not stop until it has traveled this amount of ground
    
    

    private Vector3 StartPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Debug.Log("Hello from projectile");
        StartPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        Debug.Log("Collision");
        if (!other.CompareTag("Enemy"))
        {
            Debug.Log(other.tag);
            Debug.Log("Hit Something");
            // Damage player here

            // Get point of collision
            Vector3 pointOfContact = GetComponent<Collider2D>().ClosestPoint(other.transform.position);

            GameObject damageZone = Instantiate(damageZonePrefab, pointOfContact, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
