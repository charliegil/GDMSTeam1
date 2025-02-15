using UnityEngine;

public class PatrolState : State
{
    public AnimationClip anim;
    public float maxXSpeed;
    public override void Enter()
    {
        //animator.Play("Patrol");
    }
    public override void Do()
    {
        //do the action
        //isComplete (have finish the state)
        //animator.speed = Helpers.Map(maXSpeed, 0, 1, 0, 1.6f, true)

    }
    public override void Exit()
    {
        
    }
}
