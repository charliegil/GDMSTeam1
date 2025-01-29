using UnityEngine;

public class skeletonControl : Player, IControllable
{


    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3.0f;
    private EnemyAI enemyAI;
    private Rigidbody2D rb;
    private Vector2 currentMvt;
    
    private void Awake(){
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputHandler>();
    }
    private void Update(){
        HandleMovement();
        HandleTransform();
    }
    public override void PerformAttack(){}
    public override void HandleInput(){}
   
    public override void HandleMovement(){
        if(isActive){
            enemyAI.enabled = false;
            Vector2 inputDirection = new Vector2(inputHandler.MoveInput.x, inputHandler.MoveInput.y);
            currentMvt = inputDirection.normalized * walkSpeed;
        }else{
            enemyAI.enabled = true;
            //patrol
            //engage
        }
       
    }

   private void FixedUpdate(){
        if(isActive){
            rb.linearVelocity = currentMvt;
        }
   }
   
}
