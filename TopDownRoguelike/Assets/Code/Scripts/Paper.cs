using UnityEngine;

public class Paper : Core
{
    public PaperAttackHei attack;
    public ChaseState follow;
    public playerControl playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("NPC Start() is running...");
        machine = new StateMachine(); // Ensure `machine` exists
        SetupInstances();
        
        if (follow == null)
        {
            Debug.LogError("Patrol state is NULL in NPC!");
            body.linearVelocity = new Vector2(0,0);
            return;
        }
        //patrol.SetCore(core);
        Set(follow);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(state == follow){
            if(machine.state.time>3){
                Debug.Log("hey");
                
            }
        }
        
       // if(state.isComplete){
           
        if(state!=null){
            state.DoBranch();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="player"){
            playerScript.dmgPlayer(1);
            Destroy(this);
        }
    }
    
    void FixedUpdate()
    {
     state.FixedDoBranch();   
    }
}
