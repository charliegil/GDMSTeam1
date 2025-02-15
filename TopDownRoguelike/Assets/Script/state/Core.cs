using UnityEngine;

public abstract class Core : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    //public Horse_inputControl input; //need change which input not horse input but more the chatacter
    public StateMachine machine;
    public void SetupInstances(){
        machine = new StateMachine();
        State[] allChildStates = GetComponentsInChildren<State>();
        //any game state have this state in its gameobject hierachie
        foreach(State state in allChildStates){
            state.SetCore(this);
        }
    }
}
