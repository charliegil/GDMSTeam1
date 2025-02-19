using UnityEngine;

public class NPC : Core
{
    public Transform target;
    public PatrolState patrol;
    public ChaseState chase;
   
    private float detectionRange = 4f;
    [SerializeField] float returnRange = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
    bool CloseEnough(Vector2 targetPos){
        float playerDistance = Vector2.Distance(body.position, targetPos);
        return playerDistance <= detectionRange;
    }
    bool FarEnough(Vector2 targetPos){
         float playerDistance = Vector2.Distance(body.position, targetPos);
        return playerDistance > returnRange;
    }

    void FixedUpdate()
    {
     state.FixedDoBranch();   
    }
}
