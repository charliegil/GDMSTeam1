using UnityEngine;

public class FollowState : State
{
    public AnimationClip anim;
    public override void Enter()
    {
        //animator.Play(anim.name);
    }
    public override void Do()
    {
        //do the action
        //animator.Play(anim.name, 0, time);
        //isComplete (have finish the state)

    }
    public override void Exit()
    {
        
    }
}
