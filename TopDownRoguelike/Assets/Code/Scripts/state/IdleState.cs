using UnityEngine;

public class IdleState : State
{
    //public AnimationClip anim;
   
    public override void Enter()
    {
        //animator.Play(anim.name);
    }
    public override void Do()
    {
        isComplete = true;
        Debug.Log("is idle");
        //if(ex:!input.grounded () where cannot be in this state){
           //isComplete = true;
        //}
        //do the action
        //isComplete (have finish the state)

    }
    public override void Exit()
    {
        
    }
}
