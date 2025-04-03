using UnityEngine;

public class StraightChaseState : State
{
    public Transform target;
    [SerializeField] private float speed = 10;

    [SerializeField] private float acceleration = 2f; // Adjust as needed

    private Vector3 targetVelocity;

    // this is a simple chase state that is used for the kamikaze enemy, running in a simple line very fast
    public override void Enter() //only once
    {
        targetVelocity = (target.position - body.transform.position).normalized * speed;
        body.linearVelocity = Vector3.zero; // Start from zero velocity
        
    }
    public override void Do() //update
    {
        body.linearVelocity = Vector3.MoveTowards(body.linearVelocity, targetVelocity, acceleration * Time.deltaTime);
    }
    
    

    public override void Exit()
    {
        
    }
    // Update is called once per frame

}
