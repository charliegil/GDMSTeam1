using UnityEngine;

public class RoutePatrol : State
{

    /*
    Modified the default patrol state to be able to set as many anchors as possible,
    as well as how much time the enemy passes at each anchor
    */
    // for some reason it is not seeing this script when attached to a patrol object
    public Navigate navigate;
    public IdleState idle;
    public Transform []  anchors;
    [SerializeField] private Vector2 IdleInterval = new Vector2(1,5);
    private void OnValidate() {if (IdleInterval.x > IdleInterval.y) IdleInterval.x = IdleInterval.y;} 

    private float IdleTime= 1;

    private int currentIndex = 0;
    

    //public AnimationClip anim;
    public float maxXSpeed;

    private void GoToNextDestination(){
          
            currentIndex = (currentIndex+1) % anchors.Length;
            
            navigate.destination = (Vector2)anchors[currentIndex].transform.position;
            Set(navigate, true);
    }
    public override void Enter()
    {
        navigate.speed = maxXSpeed;
        GoToNextDestination();
        //animator.Play("Patrol");
    }
    public override void Do()
    {
        //do the action
        //isComplete (have finish the state)
        //animator.speed = Helpers.Map(maXSpeed, 0, 1, 0, 1.6f, true)
        if(machine.state == navigate){
            if(navigate.isComplete){
                //navigate.isComplete = false;
                IdleTime = Random.Range(IdleInterval.x, IdleInterval.y);
                Set(idle, true);
                body.linearVelocity = new Vector2(0, body.linearVelocityY);
                
            }
        }
        else if(machine.state == idle){
            Debug.Log("you are in idle and waiting for next");
            GoToNextDestination();
            if(machine.state.time>IdleTime){
                Debug.Log("you should now move to the other side");
                GoToNextDestination();
            }
            Debug.Log(machine.state.time);
        }

    }
    public override void Exit()
    {
        
    }
}
