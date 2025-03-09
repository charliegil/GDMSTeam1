using System.ComponentModel;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public abstract class BaseEnemy : MonoBehaviour
{
protected int hp;
protected int CurrentHp;
protected float currentSpeed = 0;
[SerializeField] protected int damage;

[SerializeField] protected float attackRange = 10;
[SerializeField] protected float maxSpeed = 10;
[SerializeField] protected float acceleration = 1;
[SerializeField] protected Animator animator;
[SerializeField] protected Rigidbody2D body;
int FOV; // the FOV of the enemy. If the player is not in the FOV of the enemy, the enemy cannot see him. need to implement with pathfinding
int viewDistance; // the enemy can see the player when the distance between them is smaller than viewDistance. Need to implement with pathfinding
protected static GameObject player;

protected float timer;

 /* starting next week, I will make this class the base class for all the enemies. integrating this with the pathfinding program, we will be able
 to design enemies faster and reuse components


 */
    public abstract void Attack();
    public void TakeDamage(int damage){
        animator.SetTrigger("takeDamage");
        CurrentHp-=damage;

        if(CurrentHp < 0) OnDeath();
    }

    public abstract void move();

    public abstract void OnDeath();


    // in this class, get the health component of the player and deal him damage. Will need to make a coroutine in the health script to deal 
    // damage over time



   

    
    
    
    
    
    bool IsPlayerInlineOfSight(){
        
        if(isWallBetweenPlayer(player)) return false; // if there is a wall between the player and the enemy

        Vector2 direction = (transform.position - player.transform.position);
        float distanceToPlayer = direction.magnitude;
        
        if(distanceToPlayer > viewDistance) return false;
        
        direction.Normalize();
        
        float angle = Vector2.Angle(transform.right, direction);
        
        if (angle < (FOV/2)) return false;
        return true;
    }
    float getDistanceToPlayer(){
        return(transform.position- player.transform.position).magnitude;
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





}
