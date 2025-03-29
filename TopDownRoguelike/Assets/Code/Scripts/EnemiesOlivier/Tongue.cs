using UnityEngine;

public class Tongue : MonoBehaviour
{
    [SerializeField] private BoxCollider2D BoxCollider;

    public int damage;

  

    private PlayerController playerController;

    private GameObject parent;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(!collision.gameObject.CompareTag("Player")) return;
        
        EventManager.PlayerTakeDamage(damage);

        
        
    }
}
