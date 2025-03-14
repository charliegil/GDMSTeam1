using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Collectable : a class that defines objects that can be collected by the player, automatically or not
/// </summary>
public class Collectable : MonoBehaviour
{
    // this script is for identifying objects that can be collected. It defines how you can collect them
  
    [SerializeField] private float collectRadius;
    [SerializeField] private bool CollectAutomatically = false;
    [SerializeField] private KeyCode keyToObtain = KeyCode.E; // specifies which key must be pressed in order to collect that item

    [SerializeField] private float value;
/// <summary>
/// specifies how much time the boost is gonna last. not used for Life and skillPoint
/// </summary>
    [SerializeField] private float duration;

    [SerializeField] private CollectableType type;
    [SerializeField] private float movementDuration= 0.5f;

    private CircleCollider2D CollectableCollider;
    private Renderer CollectableRenderer;



    // TODO
    // some sort of effect that indicates that you can pick them up
    //  when picked up, either make the animation linear or non linear

    private void Start()
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
    private void Update()
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
    private RaycastHit2D[] castRayAndGetCollider(Vector2 direction){
        
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
        OnPlayerCollect(Player);
        transform.position = Player.transform.position;
        Destroy(gameObject);
    }



    protected void OnPlayerCollect(GameObject Player){
        
        Debug.Log("the collectible has reached the player");
        
        if (type == CollectableType. Life){
            EventManager.PlayerTakeDamage(-value);

        }
        else if (type == CollectableType.SkillPoint){
            EventManager.SkillPointAcquired((int)value); 
        }
        else if (type == CollectableType.AttackBoost){
            ApplyTemporaryBoost(EventManager.AttackBoost);
        }
        else if (type == CollectableType.DefenceBoost){
            ApplyTemporaryBoost(EventManager.DefenceBoost);
        }
        
        else if (type == CollectableType.CritiqualHit){ 
            ApplyTemporaryBoost(EventManager.CriticalHitBoost);
        }
        else if (type == CollectableType.SpeedBoost){ 
            ApplyTemporaryBoost(EventManager.SpeedBoost);
        }

    }

        private IEnumerator ApplyTemporaryBoost(Action<float> EventToCall){

            EventToCall(value);
            yield return new WaitForSeconds(duration);
            EventToCall(1/value);
    }
    }

    public enum CollectableType {
        Life,
        AttackBoost,
        DefenceBoost,
    
        SkillPoint,
        CritiqualHit,
        SpeedBoost
    }


    

    

