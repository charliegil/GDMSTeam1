
using UnityEngine;

public class Navigate : State
{
    public Vector2 destination;
    public float speed = 1;
    private float threshold = 1.5f;
    //public State animation;
    public override void Enter()
    {
        isComplete = false;
        //Set(animation, true);
    }
    public override void Do()
    {
        if(Vector2.Distance(core.transform.position, destination)<threshold){
            isComplete = true;
        }   
        //FaceDestination();
    }
    public override void FixedDo()
    {
        Vector2 direction = (destination - (Vector2)core.transform.position).normalized;
        body.linearVelocity = new Vector2(direction.x*speed, direction.y*speed);
    }
    private void FaceDestination(){
        core.transform.localScale = new Vector3(Mathf.Sign(body.linearVelocityX),body.linearVelocityY,1);
    }
}
