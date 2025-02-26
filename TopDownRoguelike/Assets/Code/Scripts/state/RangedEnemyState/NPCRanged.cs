using UnityEngine;

public class NPCRanged : Core
{
    public RoutePatrol patrol;
    public ChaseState chase;

    public AttackState attack;
    // will need to attach the start position, as well as attach the line Renderer for the slime patrol
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        Debug.Log("NPC Start() is running...");
        machine = new StateMachine(); // Ensure `machine` exists
        SetupInstances();
        
        if (patrol == null)
        {
            Debug.LogError("Patrol state is NULL in NPC!");
            return;
        }
        //patrol.SetCore(core);
        Set(patrol);
    }

    // Update is called once per frame
    void Update()
    {
        // can modify that
        // still need to determine when to enter the attackState
        if(CloseEnough(target.position)){
            if(machine.state!=chase){
                Set(chase);
            }
        }else if(FarEnough(target.position)&&state.isComplete){ 

            if(machine.state!=patrol){
                Set(patrol);
            }

        }

       // if(state.isComplete){
           
        if(state!=null){
            state.DoBranch();
        }
    }

    void FixedUpdate()
    {
     state.FixedDoBranch();   
    }
}
