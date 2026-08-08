using UnityEngine;

public class Bai : Core
{
    public BaiState1 state1;
    public ChaseState chase;
    
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
        //float currentSpeed = rb.linearVelocity.magnitude;
        //animator.SetFloat("speed" , currentSpeed);

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
        // Debug.Log("bai close detection Range"+detectionRange+"playerDistance "+playerDistance);
        return playerDistance <= detectionRange;
    }
    void FixedUpdate()
    {
     state.FixedDoBranch();   
    }
}
