using UnityEngine;

public class enemy_anim : MonoBehaviour
{
     private Vector2 movementDirection;
     private Rigidbody2D body;
     private GameObject player;
    [SerializeField] private Animator anim;
    [SerializeField] private bool flipR = true;
    [SerializeField] private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        AdjustPlayerDirection();
    }
    private void AdjustPlayerDirection() {
        movementDirection = (player.transform.position - transform.position);
        float currentSpeed = body.linearVelocity.magnitude;
        anim.SetFloat("speed" , currentSpeed);
        //Debug.Log("hey should be");
        //Debug.Log("bai movementDirection"+movementDirection);
        if(flipR==true){
            spriteRenderer.flipX = movementDirection.x > 0;
        }else{
            spriteRenderer.flipX = movementDirection.x < 0;
        }
        

    }
}
