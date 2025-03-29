using UnityEngine;
using Pathfinding;
public class ChaseState : State
{
    public Transform target;
    private Vector3 startPos;
    public AnimationClip anim;
    public float speed = 400f;
    public float nextWaypointDistance = 3f;
    [SerializeField] float detectionRange = 100f;
    
    public Transform enemyGFX;
    
    private float maxSpeed = 30;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool returnPos = false;
    //public Navigate navigate;
    //public IdleState idle;

    private Seeker seeker;
    // should be the primary chase state we will use
    public override void Enter() //only once
    {
        seeker = GetComponent<Seeker>();
        startPos = transform.position;
        target = GameObject.FindWithTag("Player").transform;
        InvokeRepeating("UpdatePath",0f, .2f);
        //transform.position = startPos;
        //animator.Play(anim.name);
    }
    public override void Do() //update
    {
        if(returnPos){
            float playerDistance = Vector2.Distance(body.position, startPos);
            //Debug.Log("the player distance to start point"+playerDistance+" with current pos "+body.position+" startPos: "+startPos.transform.position);
            //Debug.Log("the returnPos"+returnPos);
            if(playerDistance<=0.7){
                returnPos = false;
                //Debug.Log("you have returned home");
                isComplete = true;
            }
        }
        
        
       
        
        //do the action
        //animator.Play(anim.name, 0, time);
        //isComplete (have finish the state)

    }
    private bool CloseEnough(Vector2 targetPos){
        Debug.Log("isClosed Enough");
        float playerDistance = Vector2.Distance(body.position, targetPos);
        if(playerDistance <= detectionRange){
            return true;
        }

        return false;
    }
    private void UpdatePath(){
        
        if(CloseEnough(target.position)){
            returnPos = false;
            if(seeker.IsDone())
            seeker.StartPath(body.position, target.position, OnPathComplete);
        }else if(!CloseEnough(target.position) && !returnPos){
            returnPos = true;
            if(seeker.IsDone())
            seeker.StartPath(body.position, startPos, OnPathComplete);
        }
        
    }
    private void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    public override void Exit()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {   //Debug.Log("reachedEndOfPath"+reachedEndOfPath);
        //target = Player.ActivePlayer.transform;
        if(path == null) {return;}
        if(currentWaypoint>=path.vectorPath.Count){
        //float stopDistance = 0.5f; // Adjust as needed
        ///if (returnPos && Vector2.Distance(body.position, startPos.transform.position) <= stopDistance){
            reachedEndOfPath = true;
            return;
        }else{
            reachedEndOfPath = false;
        }
        
        
        
            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - body.position).normalized;
            float distanceToTarget = Vector2.Distance(body.position, startPos);
            // Reduce speed when approaching home
            float dynamicSpeed = speed * Mathf.Clamp(distanceToTarget / 2f, 0.3f, 1f);

            Vector2 force = direction * dynamicSpeed * Time.deltaTime;
            body.AddForce(force);
            if (body.linearVelocity.magnitude > maxSpeed)
            {
                body.linearVelocity = body.linearVelocity.normalized * maxSpeed;
            }
            float distance = Vector2.Distance(body.position, path.vectorPath[currentWaypoint]);
            if(distance < nextWaypointDistance){
                currentWaypoint++;
            }
            if(force.x >= 0.01f){
                //enemyGFX.localScale = new Vector3(-1f,1f,1f);
            }else if(force.x<=-0.01f){
                //enemyGFX.localScale = new Vector3(1f,1f,1f);
            }
    }
}
