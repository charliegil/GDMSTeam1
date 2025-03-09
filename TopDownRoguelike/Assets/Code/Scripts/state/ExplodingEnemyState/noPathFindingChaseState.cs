using UnityEngine;
using Pathfinding;
public class noPathFindingChaseState : State
{
    public Transform target;
    [SerializeField] float targetSpeed = 10;

    [SerializeField] private float acceleration = 2f; // Adjust as needed

    private Vector3 direction;

    private float currentSpeed = 0;

    // this is a simple chase state that is used for the kamikaze enemy, running in a simple line very fast
    public override void Enter() //only once
    {
        body.linearVelocity = Vector3.zero; // Start from zero velocity
        
    }
    public override void Do() //update
    {
        currentSpeed = Mathf.Lerp(currentSpeed,targetSpeed,acceleration* Time.deltaTime);
        direction = (target.position - body.transform.position).normalized * currentSpeed;
        body.linearVelocity = direction;
    }
    
    

    public override void Exit()
    {
        
    }
    // Update is called once per frame

}
