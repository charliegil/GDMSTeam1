
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool isComplete {get; protected set;}
    protected float startTime;
    public float time => Time.time - startTime;

    protected Core core;
    protected Rigidbody2D body => core.body; //to not have to write core.body but only body
    protected Animator animator => core.animator;
    //public Horse_inputControl input => core.input; //need change which input not horse input but more the chatacter

    public StateMachine machine;
    protected StateMachine parent;
    public State state => machine.state;
    protected void Set(State newState, bool forceReset = false)
    {
        machine.Set(newState, forceReset) ;
    }
    public void SetCore(Core _core){
        //Debug.Log("SetCore() called in " + this.GetType().Name);
         if (machine == null) 
        {
            //Debug.Log("Creating a new StateMachine in " + this.GetType().Name);
            machine = new StateMachine();
        }
        core = _core;
    }
    public virtual void Enter(){
        startTime = Time.time;
    }
    public virtual void Do(){}
    public virtual void FixedDo(){}
    public virtual void Exit(){}
    
    public void DoBranch(){
        Do(); //if child branch, it will do all the way down the branch
        state?.DoBranch();
    }
    public void FixedDoBranch(){
        FixedDo();
        state?.FixedDoBranch();
    }

    public void Initialize(StateMachine _parent)
    {
        parent = _parent;
        isComplete = false;
        startTime = Time.time;
        
    }
}
