using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Core : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    //public Horse_inputControl input; //need change which input not horse input but more the chatacter
    public StateMachine machine;
#pragma warning disable IDE1006 // Naming Styles
    [SerializeField] protected State state => machine.state;
#pragma warning restore IDE1006 // Naming Styles
    [SerializeField] protected int detectionRange = 4;// the range at which the enemy detects the player

    public float reloadTime = 1;
    [SerializeField] protected Transform target; // according to the scene, it should be the player

    [SerializeField] protected float returnRange = 5f; 

     [SerializeField] protected int ZoneOfOperation = 5; // the zone of operation. enemy cannot go outside of this radius of 

     [SerializeField] protected int attackRange = 5;
     // his starting position

    [SerializeField] protected int FOV = 360;
    [SerializeField] protected bool showFOV = true;

    private Vector3 StartPosition;


    [SerializeField] public GameObject FOVLines;



    public void Start()
    {
        StartPosition = transform.position;

    }
    private void LateUpdate(){
        if(showFOV){ 
            FOVLines.transform.rotation = body.transform.rotation;  
        }   
    }
    protected void Set(State newState, bool forceReset = false)
    {
        if (machine == null)
        {
            Debug.LogError("Machine is null in Core!");
            return;
        }
        machine.Set(newState, forceReset) ;
    }
    public void SetupInstances(){
        if (machine == null) machine = new StateMachine();  // Initialize machine if not done yet
        Debug.Log("New StateMachine created for " + gameObject.name);
        
        machine = new StateMachine();
        State[] allChildStates = GetComponentsInChildren<State>();
        //any game state have this state in its gameobject hierachie
        foreach(State state in allChildStates){
            state.SetCore(this);
        }
        
    }
    protected void SetState(State newState) {
    if (machine.state != newState) {
        Set(newState);
    }
    }
    private void OnDawGizmos()
    {
        #if UNITY_EDITOR
        if(Application.isPlaying){
            List<State> states = machine.GetActiveStateBranch();
            UnityEditor.Handles.Label(transform.position, "Active States:"+ string.Join(">", states));
        }
        #endif
        
    }

    
    
    
    protected bool IsPlayerInDetectionRange(Vector2 targetPos){
        float playerDistance = Vector2.Distance(body.position, targetPos);
        return playerDistance <= detectionRange;
    }
    protected bool FarEnough(Vector2 targetPos){
        float playerDistance = Vector2.Distance(body.position, targetPos);
        return playerDistance > returnRange;
    }
    protected bool IsPlayerInAttackRange(Vector2 targetPos){
        float playerDistance = Vector2.Distance(body.position, targetPos);
        return playerDistance <= attackRange;
    }
    protected bool IsPlayerInZoneOfOperation(Vector2 targetPos){
        Vector2 direction = (StartPosition - target.position);
        float distanceToStart = direction.magnitude;
        
        return distanceToStart <= ZoneOfOperation;
    }
    
    protected bool IsPlayerInlineOfSight(){
        
        if(isWallBetweenPlayerAndEnemy(detectionRange)) return false; // if there is a wall between the player and the enemy

        Vector2 direction = ((Vector2)target.position - (Vector2)transform.position);
        float distanceToPlayer = direction.magnitude;
        
        if(distanceToPlayer > detectionRange) return false;
        
        direction.Normalize();
        
        float angle = Vector2.Angle(body.transform.right, direction);
        
        return angle < (FOV / 2);
    }
    
    // methods to detect player----------------------------------------------
#pragma warning disable IDE1006 // Naming Styles
    private RaycastHit2D[] castRayAndGetCollider(Vector2 direction, float range ){
        
        Debug.DrawRay(transform.position, direction);
        //Ray ray = new Ray(transform.position,direction);      
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,direction,range, Physics2D.DefaultRaycastLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        return hits;
    }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
    private bool isWallBetweenPlayerAndEnemy(float range){
        Vector2 direction = target.transform.position - transform.position;
        RaycastHit2D[] hit = castRayAndGetCollider(direction,range);
        foreach(RaycastHit2D col in hit){
            if(col.transform.gameObject.tag == "Finish") return true;
            else if (col.transform.gameObject.tag == "Player" || col.transform.gameObject.name.Contains("Player")) return false;
        }
        return false;

    }
#pragma warning restore IDE1006 // Naming Styles

    
}
