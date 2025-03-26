using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private ParticleSystem deathParticles;
    private ParticleSystem deathParticlesInstance;
 
    [HideInInspector] public bool isTargeted = false;
    
    [SerializeField] private int enemyValue = 1;


    [SerializeField] Animator animator;

    private bool isDead = false;
 

    public GameObject damagePopupPrefab;

    private static PlayerController playerController;

    // will need to integrate the UI health bar with This

    // // Update is called once per frame
    // void Update()
    // {
    //     if (currentHealth <= 0) {
    //         Destroy(gameObject);
    //     }
    // }
    void Start()
    {
        if(playerController==null) playerController = GameObject.Find("Player")?.GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage) {
    
        float attackMultiplier = playerController.getAttackMultiplier();
        float critiqual = playerController.IsAttackCritiqual();
        damage =  (damage*critiqual*attackMultiplier);
        currentHealth -= damage;
        AudioManager.instance.PlaySound("EnemyHurt");
        //Debug.Log(currentHealth);
        animator.SetTrigger("takeDamage");
        
        
        if (damagePopupPrefab != null) {

            GameObject popup = Instantiate(damagePopupPrefab, transform.position , Quaternion.identity) as GameObject;//+ new Vector3(0,1,0)
            popup.transform.GetChild(0).GetComponent<TextMesh>().text = ""+damage.ToString("F1");
            popup.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 10;
            if(!IsOne(critiqual)) popup.transform.GetChild(0).GetComponent<TextMesh>().color = Color.red;
            //DamagePopup damagePopup = popup.GetComponent<DamagePopup>();

            //damagePopup.Setup(damage);
        }
        if(currentHealth<=0 && !isDead) OnDeath();
    }
    public void OnDeath(){
        isDead = true;
        SpawnDeathInstance();
        EventManager.EnemyDied();
        if (ScoreManager.Instance != null) {
            ScoreManager.Instance.AddScore(enemyValue);
        }
        EventManager.SpawnCollectible(transform.position);
        Destroy(gameObject);
    }
    bool IsOne(float value){
    return Math.Abs(value - 1f) < 0.0001f; // Tolerance for floating-point precision errors
    }
    private void SpawnDeathInstance(){
       deathParticlesInstance = Instantiate(deathParticles, transform.position, Quaternion.identity);
    }
    public void TakeDamageOverTime(float damage, float time, float step) {
    
        
    }
    
    
    private IEnumerator TakePoisonDamage(float damage, float time, float step)
    {   
        // deals y damage for x time, with taking damage every z step in seconds
        float stepDmg = damage / time;
        if (step == 0)
        {
            
            yield return null;
            time = -1;
        }
        while (time > 0)
        {
            TakeDamage(step);
            time -= step;
            
            yield return new WaitForSeconds(step);
        }
    }
}

