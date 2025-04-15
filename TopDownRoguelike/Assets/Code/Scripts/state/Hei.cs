using UnityEngine;

public class Hei : Core
{
    //public PatrolState patrol;
    public HeiState3 state3;
    public HeiState2 state2;
    public HeiState1 state1;
    public ChaseState chase;
    //[SerializeField] private int hp;

   
    //private float detectionRange = 4f;
    //[SerializeField] float returnRange = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //hp = maxHp;
        target = GameObject.FindWithTag("Player").transform;
        machine = new StateMachine(); // Ensure `machine` exists
        SetupInstances();
        Set(chase);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("hei state: "+machine.state);
        // Debug.Log("current state hei: "+machine.state);

        if(CloseEnough(target.position)){
            //Debug.Log("state 1 hei");
            Set(state1);
            
        }
        else if(machine.state!=chase){
            //Debug.Log("chase hei");
            Set(chase);
        }

        // if(hp<=0){

        // }else if(hp<maxHp/3){
        //     if(machine.state!=state3){
        //     Set(state3);
        //     }
        // }else if(hp<maxHp*2/3){
        //     if(machine.state!=state2){
        //         Set(state2);
        //     }
        // }else{
        //     if(machine.state!=state1){
        //         Set(state1);
        //     }
        // }

       // if(state.isComplete){
           
        if(state!=null){
            state.DoBranch();
        }
    }
    
    bool CloseEnough(Vector2 targetPos){
        Debug.Log("hei close");
        float playerDistance = Vector2.Distance(body.position, targetPos);
        return playerDistance <= detectionRange;
    }
    void FixedUpdate()
    {
     state.FixedDoBranch();   
    }
}
