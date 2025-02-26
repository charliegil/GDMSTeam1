using UnityEngine;
using UnityEngine.UIElements;

public class RandomPatrol : State
{

    /*
    Modified the default patrol state to be able to set as many anchors as possible,
    as well as how much time the enemy passes at each anchor
    */
   // for this one, will omit the navigate and idle state, handle everything here because it is very simple

    [SerializeField] float findSpeed = 4;

    private float timer;

    [SerializeField] float RateOfChangeDirection = 3; // specifies when to change direction

    [SerializeField] float ZoneOfOperation;

    
    private Vector3 randomMovement;

    private Vector3 StartPosition;


    // for that random movement, it is not neccessary for pathfinding
    //public AnimationClip anim;


    // when in this state, we already assume that the enemy doesnt see the player
    // still need to adjust the line renderer from the change in rotation
    public void Start(){
        StartPosition = body.transform.position;
        Debug.Log("in start function from randomPatrol");
    }

    void GoToNextDestination(){
        randomMovement = UnityEngine.Random.insideUnitCircle.normalized;
        timer = RateOfChangeDirection;  
    }
    public override void Enter()
    {
        StartPosition = new Vector3(0,0,0); // for now, figure it later
        GoToNextDestination();
        //animator.Play("Patrol");
    }
    public override void Do()
    {
        if (timer<=0 || (transform.position-StartPosition).magnitude >= ZoneOfOperation){ // need to pick a new direction to go in
           GoToNextDestination();
        }
        timer-= Time.deltaTime;
        body.linearVelocity= body.transform.right*findSpeed;
        

    }
    private void AdjustRotation(){
        float angle = Mathf.Atan2(randomMovement.y, randomMovement.x) * Mathf.Rad2Deg; 

        float newAngle = Mathf.LerpAngle(body.transform.rotation.eulerAngles.z, angle, findSpeed * Time.deltaTime);
        body.transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }
    public override void Exit()
    {
        
    }
}
