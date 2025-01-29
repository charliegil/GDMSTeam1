
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{   
    [SerializeField] float playerDistanceSeen = 5;
    public int maxHealth = 100;
    public int curHealth;
    public int panicMultiplier = 1;

    public Node currentNode;
    public List<Node> path = new List<Node>();

    public enum StateMachine
    {
        Patrol,
        Engage,
        Evade,
        //InputControl
    }

    public StateMachine currentState;

    public playerControl player; //not sure 

    public float speed = 3f;

    private void Start()
    {
        // currentState = StateMachine.Patrol;
        curHealth = maxHealth;
    }

    private void Update()
    {
        switch (currentState)
        {
            case StateMachine.Patrol:
                Patrol();
                break;
            case StateMachine.Engage: 
                Engage(); 
                break;
            case StateMachine.Evade: 
                Evade(); 
                break;
            // case StateMachine.InputControl:
            //     InputControl();
            //     break;
        }
        
        bool playerSeen = Vector2.Distance(transform.position, player.transform.position) < playerDistanceSeen;

        if(!playerSeen && currentState != StateMachine.Patrol && curHealth > (maxHealth * 20) / 100)
        {
            currentState = StateMachine.Patrol;
            path.Clear();
        }else if(playerSeen && currentState != StateMachine.Engage && curHealth > (maxHealth * 20) / 100)
        {
            currentState = StateMachine.Engage;
            path.Clear();
        }else if(currentState != StateMachine.Evade && curHealth <= (maxHealth * 20) / 100) //don't so need
        {
            panicMultiplier = 2;
            currentState = StateMachine.Evade;
            path.Clear();
        }
        // }else if(currentState != StateMachine.InputControl){
        //     currentState = StateMachine.InputControl;
        //     path.Clear();
        // }

        CreatePath();
    }

    void Patrol()
    {
         if (path == null)
        {
            Debug.LogWarning("path was null, initializing it.");
            path = new List<Node>();
        }
        if (AStarManager.instance == null)
        {
            Debug.LogError("AStarManager.instance is null! Cannot generate path.");
            return;
        }
        Node[] nodes = AStarManager.instance.NodesInScene();
        if (nodes == null || nodes.Length == 0)
        {
            Debug.LogError("No nodes found in scene! Cannot generate path.");
            return;
        }
         if (currentNode == null)
        {
            Debug.LogError("currentNode is null! Cannot generate path.");
            return;
        }
        Debug.Log("path"+path);

        if(path.Count == 0)
        {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.NodesInScene()[Random.Range(0, AStarManager.instance.NodesInScene().Length)]);
        }
    }

    void Engage()
    {
        if (path.Count == 0)
        {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindNearestNode(player.transform.position));
        }
    }

    void Evade()
    {
        if (path.Count == 0)
        {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindFurthestNode(player.transform.position));
        }
    }

    void InputControl(){

    }
    public void CreatePath()
    {
         if (path == null || path.Count == 0)
        {
            Debug.LogWarning("Path is empty, skipping movement.");
            return; // Prevent null reference exception
        }
        if (path.Count > 0)
        {
            int x = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[x].transform.position.x, path[x].transform.position.y, -2), speed * panicMultiplier * Time.deltaTime);

            if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
            {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }else{

        }
    }
}