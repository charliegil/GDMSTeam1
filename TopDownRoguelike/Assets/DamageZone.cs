using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public bool playerInside = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player Entered");
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Player Left");
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Rigidbody2D rb = other.attachedRigidbody;
            Vector3 pullDirection = (transform.position - other.transform.position).normalized;
            float pullStrength = 20;
            Vector3 pullVelocity = pullDirection * pullStrength;

            rb.linearVelocity = pullVelocity;
        }
    }
}
