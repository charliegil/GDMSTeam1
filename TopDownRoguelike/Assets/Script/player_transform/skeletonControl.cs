using UnityEngine;
using UnityEngine.InputSystem.XInput;

public class skeletonControl : Player, IControllable
{


    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3.0f;
    private EnemyAI enemyAI;
    private Rigidbody2D rb;
    private Vector2 currentMvt;
    public enum EnemyState
    {
        Patrol,
        Engage,
        Evade,
    }
    bool stateComplete;
    EnemyState currentState;
   
    private void Awake(){
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputHandler>();
    }
    private void Update(){
        HandleMovement();
        HandleTransform();
        if(stateComplete){
            SelectState();
        }
        UpdateState();
    }
    void SelectState(){
        stateComplete = false;
        //StartPatrol() where it would have anim.Play("Patrol")
    }
    void UpdateState(){
        switch(currentState){
            case EnemyState.Patrol:
            UpdatePatrol();
            break;
            case EnemyState.Engage:
            UpdateEngage();
            break;
            case EnemyState.Evade:
            UpdateEvade();
            break;
            
        }
    }
    void UpdatePatrol(){
        //stateComplete = true;
    }
    void UpdateEngage(){
        
    }
    void UpdateEvade(){
        
    }
    public override void PerformAttack(){}
    public override void HandleInput(){}
   
    public override void HandleMovement(){
        if(ActivePlayer==this){
             enemyAI.enabled = false;
            Vector2 inputDirection = new Vector2(inputHandler.MoveInput.x, inputHandler.MoveInput.y);
            currentMvt = inputDirection.normalized * walkSpeed;
         }else{
            enemyAI.enabled = true;
         }
        // if(isActive){
        // //if(ActivePlayer){
        //     enemyAI.enabled = false;
        //     Vector2 inputDirection = new Vector2(inputHandler.MoveInput.x, inputHandler.MoveInput.y);
        //     currentMvt = inputDirection.normalized * walkSpeed;
        // }else{
        //     enemyAI.enabled = true;
        //     //patrol
        //     //engage
        // }
       
    }

   private void FixedUpdate(){
        if(ActivePlayer==this){
            rb.linearVelocity = currentMvt;
        }
   }
   
}
