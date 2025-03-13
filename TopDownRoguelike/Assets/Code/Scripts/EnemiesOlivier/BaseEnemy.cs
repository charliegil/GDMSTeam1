using System.ComponentModel;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public abstract class BaseEnemy : MonoBehaviour
{
[SerializeField] protected float maxHP;
[SerializeField] protected float CurrentHp;
protected float currentSpeed = 0;
[SerializeField] protected int damage;
public event Action EventDeath;
[SerializeField] protected float attackRange = 10;
[SerializeField] protected float maxSpeed = 10;
[SerializeField] protected float acceleration = 1;
[SerializeField] protected Animator animator;
[SerializeField] protected Rigidbody2D body;

[Range (0,360f)] [SerializeField] protected int FOV; // the FOV of the enemy. If the player is not in the FOV of the enemy, the enemy cannot see him. need to implement with pathfinding

[SerializeField] protected int viewDistance; // the enemy can see the player when the distance between them is smaller than viewDistance. Need to implement with pathfinding
protected static GameObject player;

protected float timer;

    public abstract void Attack();
    public void TakeDamage(float damage){
        animator.SetTrigger("takeDamage");
        CurrentHp-=damage;

        if(CurrentHp < 0) OnDeath();
    }

    public void Start()
    {
        CurrentHp = maxHP;
    }

    public abstract void move();

    public void OnDeath(){
        EventDeath?.Invoke();
        // spawn objects that player can pick up
    }
    // public void takeDamage()
    // {
    //     CurrentHp-=damage;
    //     throw new NotImplementedException();
    // }













    private bool IsPlayerInlineOfSight(){
        
        if(isWallBetweenPlayer(player)) return false; // if there is a wall between the player and the enemy

        Vector2 direction = (transform.position - player.transform.position);
        float distanceToPlayer = direction.magnitude;
        
        if(distanceToPlayer > viewDistance) return false;
        
        direction.Normalize();
        
        float angle = Vector2.Angle(transform.right, direction);
        
        if (angle < (FOV/2)) return false;
        return true;
    }
    private float getDistanceToPlayer(){
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
    protected bool IsPlayerInAttackRange(){
        return getDirectionToPlayer().magnitude < attackRange;
    }

    protected bool IsPlayerInDetectionRange(){
        return getDirectionToPlayer().magnitude < viewDistance;
    }
    protected Vector2 getDirectionToPlayer(){
        return (player.transform.position - transform.position);
    }





}
