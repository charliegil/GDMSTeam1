using UnityEngine;

public class tongControl : Player, IControllable
{

    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3.0f;

    private Rigidbody2D rb;
    private Vector2 currentMvt;
    
    private void Awake(){
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
        if(ActivePlayer==this){
            Vector2 inputDirection = new Vector2(inputHandler.MoveInput.x, inputHandler.MoveInput.y);
            currentMvt = inputDirection.normalized * walkSpeed;
        }
    }

   private void FixedUpdate(){
        rb.linearVelocity = currentMvt;
   }
   
}
