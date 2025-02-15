using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool isComplete {get; protected set;}
    protected float startTime;
    public float time => Time.time - startTime;

    protected Core core;
    public Rigidbody2D body => core.body; //to not have to write core.body but only body
    public Animator animator => core.animator;
    //public Horse_inputControl input => core.input; //need change which input not horse input but more the chatacter

    public virtual void Enter(){}
    public virtual void Do(){}
    public virtual void FixedDo(){}
    public virtual void Exit(){}
    public void SetCore(Core _core){
        core = _core
    }

    public void Initialize()
    {
        isComplete = false;
        startTime = Time.time;
        
    }
}
