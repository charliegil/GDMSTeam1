using System.ComponentModel;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public abstract class BaseEnemy : MonoBehaviour
{
int hp;
int curHp;
int damage;
float speed; // the speed of the enemy
float AttackRange; 
int FOV; // the FOV of the enemy. If the player is not in the FOV of the enemy, the enemy cannot see him. need to implement with pathfinding
int viewDistance; // the enemy can see the player when the distance between them is smaller than viewDistance. Need to implement with pathfinding
float reactionTime = 0; // when the player enter the radius of the attack, the enemy will immediately attack after a certain time
private static GameObject Player;

private LineRenderer FOVLines;
 /* starting next week, I will make this class the base class for all the enemies. integrating this with the pathfinding program, we will be able
 to design enemies faster and reuse components


 */

public abstract void PerformAttack();

public void dealDamage(){
    // in this class, get the health component of the player and deal him damage. Will need to make a coroutine in the health script to deal 
    // damage over time

}

   

    
    
    bool IsPlayerInlineOfSight(){
        
        if(isWallBetweenPlayer(Player)) return false; // if there is a wall between the player and the enemy

        Vector2 direction = (transform.position - Player.transform.position);
        float distanceToPlayer = direction.magnitude;
        
        if(distanceToPlayer > viewDistance) return false;
        
        direction.Normalize();
        
        float angle = Vector2.Angle(transform.right, direction);
        
        if (angle < (FOV/2)) return false;
        return true;
    }
    float getDistanceToPlayer(){
        return(transform.position- Player.transform.position).magnitude;
    }
        public bool isWallBetweenPlayer(GameObject obj ){
        Vector2 direction = obj.transform.position - transform.position;
        RaycastHit2D[] hit = castRayAndGetCollider(direction);
        foreach(RaycastHit2D col in hit){
            if(col.transform.gameObject.tag == "Finish") return true;
            else if (col.transform.gameObject.tag == "Player" || col.transform.gameObject.name.Contains("Player")) return false;
        }
        return false;

    }
    private RaycastHit2D[] castRayAndGetCollider(Vector2 direction ){
        
        Debug.DrawRay(transform.position, direction);
        //Ray ray = new Ray(transform.position,direction);      
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,direction, viewDistance, Physics2D.DefaultRaycastLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        return hits;
    }
    private void setLineRenderer(){
        // Start of the FOV renderer
        float lineWidth = 0.05f;
        FOVLines = GetComponent<LineRenderer>();
        if (FOVLines == null) FOVLines = gameObject.AddComponent<LineRenderer>();
        FOVLines.positionCount = 4; 
        FOVLines.startWidth = lineWidth;
        FOVLines.endWidth = lineWidth;
        FOVLines.useWorldSpace = false;
        Vector3 start = transform.position;
        Vector3 dir1 = RotateVector(Vector3.right, FOV/2);
        Vector3 dir2 = RotateVector(Vector3.right, -FOV/2);

        FOVLines.SetPosition(0, start);
        FOVLines.SetPosition(1, start + dir1 * viewDistance);
        FOVLines.SetPosition(2, start);
        FOVLines.SetPosition(3, start + dir2 * viewDistance);
        // End of the FOV renderer
    }
    private Vector3 RotateVector(Vector3 v, float degrees){
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector3(v.x*cos-v.y*sin, v.x*sin + v.y*cos);
    }




}
public enum MovementType{
    /*This will be implemented with pathfinding. for each of the states of the enemy, we will assign a movement type. 
    If its follow player, will use the pathfinding method to see the player. Will need to see if the behaviour of the player will need to be
    influenced by the FOV and the view distance
    */
    rangedEnemy, 
    RandomMovement,
    FollowPlayer, 
    EscapePlayer,
    Static,
    Perpendicular,
}