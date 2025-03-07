
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
        
        //Debug.Log("the distance is "+Vector2.Distance(core.transform.position, destination));
        if(Vector2.Distance(core.transform.position, destination)<threshold){
            
            Debug.Log("navigate is "+isComplete);
            isComplete = true;
        }   
        //FaceDestination();
    }
    public override void FixedDo()
    {
        Vector2 direction = (destination - (Vector2)core.transform.position).normalized;
        body.linearVelocity = new Vector2(direction.x*speed, direction.y*speed);
    }
    void FaceDestination(){
        core.transform.localScale = new Vector3(Mathf.Sign(body.linearVelocityX),body.linearVelocityY,1);
    }
}
