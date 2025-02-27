using UnityEngine;

public class AttackState : State
{
    public override void Enter()
    {
        // instead, decelerate
        body.linearVelocity  = new Vector2(0, 0);
    }
    public override void Do()
    {
         body.linearVelocity  = new Vector2(0, 0);
    }
    public override void Exit()
    {
        
    }
}
