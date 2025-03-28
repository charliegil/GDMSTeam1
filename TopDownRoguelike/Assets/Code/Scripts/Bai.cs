using UnityEngine;

public class Bai : Core
{
    //public Transform target;
    //public PatrolState patrol;
    //public BaiState3 state3;
    //public BaiState2 state2;
    public BaiState1 state1;
    public ChaseState chase;
    //private int maxHp = 100;
    //[SerializeField] private int hp;

   
    //private float detectionRange = 4f;
    //[SerializeField] float returnRange = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //hp = maxHp;
        machine = new StateMachine(); // Ensure `machine` exists
        SetupInstances();
        target = GameObject.FindWithTag("Player").transform;
        Set(chase);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("current state hei: "+machine.state);

        if(CloseEnough(target.position)){
            Debug.Log("state 1 hei");
            Set(state1);
        }
        else if(machine.state!=chase){
            Debug.Log("chase hei");
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
       
        float playerDistance = Vector2.Distance(body.position, targetPos);
         Debug.Log("bai close detection Range"+detectionRange+"playerDistance "+playerDistance);
        return playerDistance <= detectionRange;
    }
    void FixedUpdate()
    {
     state.FixedDoBranch();   
    }
}
