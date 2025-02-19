using System.Collections.Generic;
using UnityEngine;

public abstract class Core : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    //public Horse_inputControl input; //need change which input not horse input but more the chatacter
    public StateMachine machine;
    public State state => machine.state;
    protected void Set(State newState, bool forceReset = false)
    {
        if (machine == null)
        {
            Debug.LogError("Machine is null in Core!");
            return;
        }
        machine.Set(newState, forceReset) ;
    }
    public void SetupInstances(){
        if (machine == null) machine = new StateMachine();  // Initialize machine if not done yet
        Debug.Log("New StateMachine created for " + gameObject.name);
        
        machine = new StateMachine();
        State[] allChildStates = GetComponentsInChildren<State>();
        //any game state have this state in its gameobject hierachie
        foreach(State state in allChildStates){
            state.SetCore(this);
        }
        
    }
    private void OnDawGizmos()
    {
        #if UNITY_EDITOR
        if(Application.isPlaying){
            List<State> states = machine.GetActiveStateBranch();
            UnityEditor.Handles.Label(transform.position, "Active States:"+ string.Join(">", states));
        }
        #endif
        
    }
}
