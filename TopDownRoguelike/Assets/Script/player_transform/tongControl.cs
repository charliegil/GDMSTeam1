using UnityEngine;

public class tongControl : Player, IControllable
{
    public GameObject pointA;
    public GameObject pointB;
    private Transform currentPoint;
    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3.0f;

    private Rigidbody2D rb;
    private Vector2 currentMvt;
    
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;

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
        }else{
            Debug.Log("hey should move");
            Vector2 point = currentPoint.position - transform.position;
            if(currentPoint == pointB.transform){
                rb.linearVelocity = new Vector2(walkSpeed, 0);
            }else{
                rb.linearVelocity = new Vector2(-walkSpeed, 0);
            }
            if(Vector2.Distance(transform.position, currentPoint.position)< 1f && currentPoint == pointB.transform){
                currentPoint = pointA.transform;
                Debug.Log("B");
            }
            if(Vector2.Distance(transform.position, currentPoint.position)< 1f && currentPoint == pointA.transform){
                currentPoint = pointB.transform;
                 Debug.Log("A");
            }
        }
    }

   private void FixedUpdate(){
    if(ActivePlayer==this){
        rb.linearVelocity = currentMvt;
    }
   }
   
}
