using System.Collections;

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
    /// <summary>
    /// specifies which key must be pressed in order to collect that item
    /// </summary>
    [Tooltip("Specifies which key must be pressed in order to collect that item")]
    [SerializeField] private KeyCode keyToObtain = KeyCode.E; 

    [Tooltip("This is the multiplier of the boost")] [SerializeField] private float value;
/// <summary>
/// specifies how much time the boost is gonna last. not used for Life and skillPoint
/// </summary>
    [SerializeField] private float duration;

    public static float durationMultiplier =1;

    public static float effectMultiplier = 1;

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
       
        if(type == CollectableType.GreenFarm){
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()){
            sr.enabled = true;
        }
        }

    }


    // Update is called once per frame
    private void Update()
    {
        
    }
    private void OnTriggerExit2D(Collider2D obj){
        if( obj.tag.Contains("Player") || obj.name.Contains("Player")) {    
            //CollectableRenderer.material.color = Color.green;
        }
    }
    private void OnTriggerStay2D(Collider2D obj){
        bool WallBetweenPlayer = isWallBetweenPlayer(obj);
        if (obj.tag.Contains("Player") || obj.name.Contains("Player")){
            if(Input.GetKey(keyToObtain) && !WallBetweenPlayer){
                
                
                handleCollect(obj.gameObject);
            }  
            if(WallBetweenPlayer) CollectableRenderer.material.color = Color.green;
            //else CollectableRenderer.material.color = Color.red;
        }


    }
    private void OnTriggerEnter2D(Collider2D obj){
        if((obj.name.Contains("Player") || obj.tag.Contains("Player") )&& !isWallBetweenPlayer(obj)){
            if(CollectAutomatically || Input.GetKey(keyToObtain)){
                handleCollect(obj.gameObject);
            }
            //CollectableRenderer.material.color = Color.red;
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
        
    }



    protected void OnPlayerCollect(GameObject Player){
        
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()){
            sr.enabled = false;
        }
        //Debug.Log("the collectible has reached the player");
        AudioManager.instance.PlaySound("Collect");
        if (type == CollectableType.Life){
            EventManager.PlayerTakeDamage(-value*effectMultiplier);

        }
        else if (type == CollectableType.SkillPoint){
            EventManager.SkillPointAcquired((int)value); 
            Destroy(gameObject);
        }
        else if (type == CollectableType.AttackBoost){
            EventManager.DurationBoostStarted(duration * durationMultiplier,type);
            StartCoroutine(ApplyTemporaryBoost(EventManager.AttackBoost));
        }
        else if (type == CollectableType.DefenceBoost){
            StartCoroutine(ApplyTemporaryBoost(EventManager.DefenceBoost));
            EventManager.DurationBoostStarted(duration * durationMultiplier,type);
        }
        else if (type == CollectableType.CritiqualHit){
            StartCoroutine(ApplyTemporaryBoost(EventManager.CriticalHitBoost));
            EventManager.DurationBoostStarted(duration * durationMultiplier,type);
        }
        else if (type == CollectableType.SpeedBoost){
            StartCoroutine(ApplyTemporaryBoost(EventManager.SpeedBoost));
            EventManager.DurationBoostStarted(duration * durationMultiplier,type);
        }
        else if (type == CollectableType.Invicible){ 
            StartCoroutine(applyInvicibility());
            EventManager.DurationBoostStarted(duration * durationMultiplier,type);
        }
        else if (type == CollectableType.GreenFarm){ 

            value = 8f;
            StartCoroutine(ApplyTemporaryBoost(EventManager.AttackBoost));
            StartCoroutine(ApplyTemporaryBoost(EventManager.DefenceBoost));
            EventManager.DurationBoostStarted(duration * durationMultiplier,CollectableType.AttackBoost);
            EventManager.DurationBoostStarted(duration * durationMultiplier,CollectableType.DefenceBoost);
            ScoreManager.Instance.AddScore(50);
            EventManager.PlayerTakeDamage(-1000);
            EventManager.SkillPointAcquired((int)20); 
        }
        
    }

        private IEnumerator ApplyTemporaryBoost(Action<float> EventToCall){
            
            float time = duration * durationMultiplier;
            float val = value *effectMultiplier;
            EventToCall(val);
            yield return new WaitForSeconds(time);
            EventToCall(1f/val);
            Destroy(gameObject);
        }

        private IEnumerator applyInvicibility(){
           
            float time = duration * durationMultiplier;
            
            while(time> 0){
                PlayerController.Invincible = true;  
                time-=0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            //Debug.Log("Invincible Ended");
            PlayerController.Invincible = false;  
            Destroy(gameObject);

        }

        
    }

    public enum CollectableType {
        Life,
        AttackBoost,
        DefenceBoost,
        SkillPoint,
        CritiqualHit,
        SpeedBoost,
        Invicible,
        GreenFarm
    }


    

    

