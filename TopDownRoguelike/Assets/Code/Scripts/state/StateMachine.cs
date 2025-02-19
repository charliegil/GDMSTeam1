using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class StateMachine
{
    public State state;
    
    public void Set(State newState, bool forceReset = false){
        Debug.Log("Switching to new state: " + newState.GetType().Name);
        if(state!=newState||forceReset){
        Debug.Log("should be in StateMachine");
        state?.Exit();
        state = newState;
        Debug.Log("here is statemachine: " +state);
        state.Initialize(this);
        state.Enter();
        }
    }
    public List<State> GetActiveStateBranch(List<State> list = null){
        if(list == null){
            list = new List<State>();
        }
        if(state == null){
            return list;
        }else{
            list.Add(state);
            return state.machine.GetActiveStateBranch(list);
        }
    }
}
