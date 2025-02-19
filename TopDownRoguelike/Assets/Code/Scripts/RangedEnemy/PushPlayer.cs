using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PushPlayer : MonoBehaviour
{
    public int radius; // specifies the that when it enters it, the player will get ejected

    [SerializeField] KeyCode keyToObtain = KeyCode.E;

    private CircleCollider2D colliderCircle;

    private bool isBeingPushed = false;

    public bool canMoveWhileEjected = false;



    public float ejectRadius = 4; // 

    public float ejectionTime = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderCircle = gameObject.AddComponent<CircleCollider2D>();
        //colliderCircle = gameObject.GetComponent<CircleCollider2D>();
        colliderCircle.radius = radius;
       colliderCircle.isTrigger = true;
    }

    // Update is called once per frame
    
    
    private void OnTriggerStay2D(Collider2D obj){
        
        if((obj.name.Contains("Player") || obj.tag.Contains("Player") ) && !isWallBetweenPlayer(obj) && Input.GetKey(keyToObtain) && !isBeingPushed){
            Debug.Log("attempting to push");
            StartCoroutine(handlePush(obj.gameObject));
            }
    }

    private IEnumerator handlePush(GameObject Player){  // still with bug. move the pull call into the on trigger enter. Then also
     // make a while loop to constantly adjust the velocity direction because the player can move 
     // also make a parameter that enables the player to escape the velocity by dashing
        
        Vector2 direction = Player.transform.position - transform.position;
       
        direction.Normalize();
        
        float ejectSpeed = ejectRadius/ejectionTime;
        
        
        isBeingPushed = true;
        
        PlayerController playerControl = Player.GetComponent<PlayerController>();
        float elapsedTime = 0;
       

        playerControl.setExternalVelocity(direction * ejectSpeed);
        

        yield return new WaitForSeconds(ejectionTime);
        
        playerControl.setExternalVelocity(new Vector2(0,0));
        isBeingPushed = false;
    }
    public bool isWallBetweenPlayer(Collider2D obj ){
        Vector2 direction = obj.transform.position - transform.position;
        RaycastHit2D[] hit = castRayAndGetCollider(direction);
        foreach(RaycastHit2D col in hit){
            if(col.transform.gameObject.tag == "Finish") return true;
            else if (col.transform.gameObject.tag == "Player" || col.transform.gameObject.name.Contains("Player")) return false;
        }
        return false;

    }
    RaycastHit2D[] castRayAndGetCollider(Vector2 direction){
        
        Debug.DrawRay(transform.position, direction);
        //Ray ray = new Ray(transform.position,direction);      
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,direction,radius, Physics2D.DefaultRaycastLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        return hits;
    }
    
}
