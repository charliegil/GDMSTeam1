using System.Collections;
using UnityEngine;

public class playerControl : Player, IControllable
{


    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3.0f;
    private Rigidbody2D rb;
    private Vector2 currentMvt;
    public int hp = 100;
    
    public int maxEffect = 5;
    public int currentEffect = 0;
    private float speedEffect = 0.5f;

    public void addEffect(){
        if(currentEffect<maxEffect){
            Debug.Log("hey, you add an Effect of slowness");
            walkSpeed -= speedEffect;
            currentEffect++;
            StartCoroutine(FinishEffect(3.0f));
        }
    }
    public int getEffect(){
        return currentEffect;
    }
    private IEnumerator FinishEffect(float waitTime){
        yield return new WaitForSeconds(waitTime);
        Debug.Log("finish effect");
        currentEffect--;
        walkSpeed+=speedEffect;
    }


    public void dmgPlayer(int dmg){
        hp-=dmg;
    }
    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        inputHandler = GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputHandler>();
    }
    private void Start(){
        SetAsActive();
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
