using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connections;
    public float gScore; //how many moves takes to get each node
    public float hScore; //cheapest cost from current to end node
    public float FScore(){
        return gScore + hScore; 
    }
    // private void OnDrawGizmos(){
    //     Gizmos.color = Color.blue;
    //     if(connections.Count>0){
    //         for(int i=0; i< connections.Count; i++){
    //             Gizmos.DrawLine(transform.position, connections[i].transform.position);
    //         }
    //     }
    // }
    
}
