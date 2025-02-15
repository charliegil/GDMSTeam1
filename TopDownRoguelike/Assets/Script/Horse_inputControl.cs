
using UnityEngine;

public class Horse_inputControl : Core
{
    public State patrolState;
    public State attackState;
    public State followState;
    
    public State idleState;
    public bool grounded { get; private set;}
    public float xInput { get; private set;}

    public float yInput { get; private set;}

    void Start()
    {
        SetupInstances();
       // machine = new StateMachine();
        machine.Set(idleState);
    }

    void Update()
    {
        CheckInput();
        //if(state.isComplete){
            SelectState();
        //}
        machine.state.Do();
    }
    void CheckInput(){
        //don't need
        //xInput = Input.GetAxis("horizontal");
        //yInput = Input.GetAxis("Vertical");
    }
    void FixedUpdate()
    {
        
    }
    //when enter in 
    void SelectState(){
        
        //logic of which to select
        if(){
            machine.Set(idleState);
            machine.Set(patrolState);
            machine.Set(followState);
            machine.Set(attackState);
            
        }
        
        //isComplete allow use same animation 
        
        
    }

}
