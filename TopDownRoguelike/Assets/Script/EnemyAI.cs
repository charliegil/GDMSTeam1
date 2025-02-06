using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering.Universal.Internal;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 400f;
    public float nextWaypointDistance = 3f;
    [SerializeField] float detectionRange = 4f;
    [SerializeField] float returnRange = 5f;
    public Transform enemyGFX;
    private Vector2 startPos;
    private float maxSpeed = 3;

    
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    private bool returnPos = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        Debug.Log("startPos"+startPos);
        target = Player.ActivePlayer.transform;
        InvokeRepeating("UpdatePath",0f, .5f);
        
    }
    void UpdatePath(){
        float playerDistance = Vector2.Distance(rb.position, target.position);
        Debug.Log("the distance: "+Vector2.Distance(rb.position, target.position));
        if(playerDistance <= detectionRange){
            returnPos = false;
            if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }else if(playerDistance>returnRange && !returnPos){
            Debug.Log("here");
            returnPos = true;
            if(seeker.IsDone())
            seeker.StartPath(rb.position, startPos, OnPathComplete);
        }
        
    }
    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }
    void Update(){
        target = Player.ActivePlayer.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //target = Player.ActivePlayer.transform;
        if(path == null) {return;}
        if(currentWaypoint>=path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        }else{
            reachedEndOfPath = false;
        }
        
        
            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force);
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if(distance < nextWaypointDistance){
                currentWaypoint++;
            }
            if(force.x >= 0.01f){
                enemyGFX.localScale = new Vector3(-1f,1f,1f);
            }else if(force.x<=-0.01f){
                enemyGFX.localScale = new Vector3(1f,1f,1f);
            }
        
        
        
        
    }
}
