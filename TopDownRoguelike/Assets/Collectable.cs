using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectable : MonoBehaviour
{
    // this script is for identifying objects that can be collected. It defines how you can collect them. 
  
    [SerializeField] float collectRadius;
    [SerializeField] bool CollectAutomatically = false;
    [SerializeField] KeyCode keyToObtain = KeyCode.E; // specifies which key must be pressed in order to collect that item


    [SerializeField] float movementDuration= 0.5f;


    private CircleCollider2D CollectableCollider;
    private Renderer CollectableRenderer;
    
    
    
    // TODO
    // some sort of effect that indicates that you can pick them up
    //  when picked up, either make the animation linear or non linear
    
    void Start()
    {
       CollectableCollider = gameObject.AddComponent<CircleCollider2D>();
       
       CollectableRenderer = gameObject.GetComponent<Renderer>();

        
        if(CollectableRenderer == null){
            Debug.LogError("there is no renderer attached to this GameObject");
            return;
        }
       CollectableCollider.radius = collectRadius;
       CollectableCollider.isTrigger = true;
       CollectableRenderer.material.color = Color.green;
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerExit2D(Collider2D obj){
        if( obj.tag.Contains("Player") || obj.name.Contains("Player")) {    
            CollectableRenderer.material.color = Color.green;
        }
    }
    private void OnTriggerStay2D(Collider2D obj){
        bool WallBetweenPlayer = isWallBetweenPlayer(obj);
        if (obj.tag.Contains("Player") || obj.name.Contains("Player")){
            if(Input.GetKey(keyToObtain) && !WallBetweenPlayer){
                
                
                handleCollect(obj.gameObject);
            }  
            if(WallBetweenPlayer) CollectableRenderer.material.color = Color.green;
            else CollectableRenderer.material.color = Color.red;
        }


    }
    private void OnTriggerEnter2D(Collider2D obj){
        if((obj.name.Contains("Player") || obj.tag.Contains("Player") )&& !isWallBetweenPlayer(obj)){
            if(CollectAutomatically || Input.GetKey(keyToObtain)){
                handleCollect(obj.gameObject);
            }
            CollectableRenderer.material.color = Color.red;
        }
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,direction,collectRadius, Physics2D.DefaultRaycastLayers);
        Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
        return hits;
    }
    private void handleCollect(GameObject Player){
        Vector2 direction = (Player.transform.position - transform.position).normalized;
        CollectableCollider.enabled = false;
        StartCoroutine(moveTowardsPlayerLog(Player, transform.position));
    }
    
    private IEnumerator moveTowardsPlayerLog(GameObject Player,Vector2 start){
        float elapsedTime = 0;

        while (elapsedTime < movementDuration)
        {
            transform.position = Vector2.Lerp(start, Player.transform.position, elapsedTime / movementDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = Player.transform.position;
        Destroy(gameObject);
    }
    



}
    

    

