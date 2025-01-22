using UnityEngine;

public class tongControl : Player, IControllable
{


    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3.0f;

    private CharacterController characterController;
    //private PlayerInputHandler inputHandler;
    //private PlayerInputHandler inputHandler;
    private Vector3 currentMvt;
    private void Awake(){
        characterController = GetComponent<CharacterController>();
        inputHandler = GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputHandler>();
        //HandleInput();
        //inputHandler = PlayerInputHandler.Instance;
    }
    private void Update(){
        HandleMovement();
        //HandleTransform();
    }
    public override void PerformAttack(){}
    public override void HandleInput(){}
   
    public override void HandleMovement(){
        if(isActive){
            Vector3 inputDirection = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y);
            Vector3 worldDirection = transform.TransformDirection(inputDirection);
            worldDirection.Normalize();

            currentMvt.x = worldDirection.x * walkSpeed;
            currentMvt.z = worldDirection.z * walkSpeed;
            characterController.Move(currentMvt * Time.deltaTime);  
        }
       
    }

   
}
