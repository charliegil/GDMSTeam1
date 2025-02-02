using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NPC_Controller : MonoBehaviour
{
    public Node currentNode;
    public List<Node> path;

    public PlayerController player;
    private float speed = 3;

    public enum StateMachine {
        Patrol,
        Engage,
        Evade
    }

    public StateMachine currentState;

    private void Start() {
        currentState = StateMachine.Patrol;
    }

    [System.Obsolete]
    private void Update() {
        switch(currentState) {
            case StateMachine.Patrol:
                Patrol();
                break;
            case StateMachine.Engage:
                Engage();
                break;
            case StateMachine.Evade:
                Evade();
                break;
        }

        bool playerSeen = Vector2.Distance(transform.position, player.transform.position) < 5.0f;

        if(playerSeen == false && currentState != StateMachine.Patrol) {
            currentState = StateMachine.Patrol;
            path.Clear();
        }
        else if(playerSeen == true && currentState != StateMachine.Engage) {
            currentState = StateMachine.Engage;
            path.Clear();
        }
        /*else if(currentState != StateMachine.Evade) {
            currentState = StateMachine.Evade;
            path.Clear();
        }*/

        CreatePath();
    }

    [System.Obsolete]
    void Patrol() {
        if(path.Count == 0) {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.NodesInScene()[Random.Range(0, AStarManager.instance.NodesInScene().Length)]);
        }
    }

    [System.Obsolete]
    void Engage() {
        if(path.Count == 0) {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindNearestNode(player.transform.position));
        }
    }

    [System.Obsolete]
    void Evade() {
        if(path.Count == 0) {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindFurthestNode(player.transform.position));
        }
    }

    void CreatePath() {
        if(path.Count > 0) {
            int x = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[x].transform.position.x, path[x].transform.position.y, -2),
                speed * Time.deltaTime);

            if(Vector2.Distance(transform.position, path[x].transform.position) < 0.1f) {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }
    }
}
