using UnityEngine;

public class HeiState1 : State
{
    //public AttackSpawn spawnEnemy;
    public IdleState idle;
    public chain_script chain_script;
    //public GameObject enemy1;

    //public AnimationClip anim;
    public override void Enter()
    {
        chain_script.setSpeed(-200);
        //Set(spawnEnemy, true);
        //animator.Play("Patrol");
    }
    
    public override void Do()
    {
        // if(machine.state == navigate){
        //     if(navigate.isComplete){
        //         //navigate.isComplete = false;
        //         Set(idle, true);
        //         body.linearVelocity = new Vector2(0, body.linearVelocityY);;
                
        //     }
        // }else if(machine.state == idle){
        //     Debug.Log("you are in idle and waiting for next");
        //     if(machine.state.time>1){
        //         Debug.Log("you should now move to the other side");
        //         GoToNextDestination();
        //     }
        // }
    }
    public override void Exit()
    {
        chain_script.setSpeed(0); 
    }
}
