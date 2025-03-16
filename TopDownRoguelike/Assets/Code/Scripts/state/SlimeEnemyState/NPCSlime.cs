using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class NPCSlime : Core
{
    public RandomPatrol patrol;
    public ChaseStateSlime chase;


    public AttackState attack;


    // will need to attach the start position, as well as attach the line Renderer for the slime patrol
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        base.Start();
        // if(showFOV){
        //     DrawFOV lines = FOVLines.GetComponent<DrawFOV>();
        //     if (lines == null) lines = FOVLines.AddComponent<DrawFOV>();
        
        //     FOV = 90;
        //     lines.drawLines(detectionRange , FOV);
        // }
        
        Debug.Log("NPC Start() is running...");
        machine = new StateMachine(); // Ensure `machine` exists
        SetupInstances();
        
        if (patrol == null)
        {
            Debug.LogError("Patrol state is NULL in NPC!");
            return;
        }
        patrol.ZoneOfOperation = ZoneOfOperation;
        chase.detectionRange = detectionRange;
        //patrol.SetCore(core);
        Set(patrol);
    }

    // Update is called once per frame
    private void Update()
    {
        // can modify that
        // still need to determine when to enter the attackState
        // when player attacks slime, enter immideatly the attack state
        
        
        bool isInAttackRange = IsPlayerInAttackRange(target.transform.position);
        bool isInLineOfSight = IsPlayerInlineOfSight(); // this methods already check the same thing as close enough
        bool isInZone = IsPlayerInZoneOfOperation(target.transform.position);

       
         
        
        if ((!isInLineOfSight || !isInZone)  /*&& state.isComplete*/) {
            SetState(patrol);
        }
        else if (isInAttackRange) {
            SetState(attack);
        }
        else if (isInLineOfSight && isInZone) {
            SetState(chase);
        } 
       
       // if(state.isComplete){
           
        if(state!=null){
            state.DoBranch();
        }
    }


    private void FixedUpdate()
    {
     state.FixedDoBranch();   
    }
}
