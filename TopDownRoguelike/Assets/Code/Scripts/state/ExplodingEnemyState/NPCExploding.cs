using UnityEngine;

public class NPCExploding : Core
{

    public IdleState idle;
    public ChaseState chase;

    public StraightChaseState straightChase;


    [SerializeField] public bool usePathFinding = false;

    [SerializeField] public float MaxAliveTime = 4;
    

    public AttackState attack;
    // will need to attach the start position, as well as attach the line Renderer for the slime patrol
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("NPC Start() is running...");
        machine = new StateMachine(); // Ensure `machine` exists
        SetupInstances();
        
        if (idle == null)
        {
            Debug.LogError("idle state is NULL in NPC exploding!");
            return;
        }
        //patrol.SetCore(core);
        Set(idle);
    }

    // Update is called once per frame
    // this NPC destroys after one attack, so no need for some advanced ai thing
    void Update()
    {
        if(IsPlayerInAttackRange(target.transform.position) || MaxAliveTime <= 0){
            Set(attack);
        }
        else if(CloseEnough(target.position)){
            if(machine.state!=chase){
                Set(chase);
            }
        }
        

       // if(state.isComplete){
       
       if((machine.state == chase)|| (machine.state == straightChase)){
        MaxAliveTime-= Time.deltaTime;
       }
           
        if(state!=null){
            state.DoBranch();
        }
    }

    void FixedUpdate()
    {
     state.FixedDoBranch();   
    }
}

