using UnityEngine;

public class PatrolState : State
{
    public Navigate navigate;
    public IdleState idle;
    public Transform anchor1;
    public Transform anchor2;

    //public AnimationClip anim;
    public float maxXSpeed;
    
    void GoToNextDestination(){
        if(navigate.destination == (Vector2)anchor1.position){
            navigate.destination = anchor2.position; 
        }else{
            navigate.destination = anchor1.position;
        }
        // float randomSpot = Random.Range(anchor1.position.x, anchor2.position.x);
        // navigate.destination = new Vector2(randomSpot, core.transform.position.y); //destination of our navigate state
        Set(navigate, true);
    }
    public override void Enter()
    {
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
                Set(idle, true);
                body.linearVelocity = new Vector2(0, body.linearVelocityY);;
                
            }
        }else if(machine.state == idle){
            Debug.Log("you are in idle and waiting for next");
            if(machine.state.time>1){
                Debug.Log("you should now move to the other side");
                GoToNextDestination();
            }
        }

    }
    public override void Exit()
    {
        
    }
}
