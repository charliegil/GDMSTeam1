using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    private float currentHealth;
    [Header("Scale Health based on wave settings")]
    [SerializeField] private float baseHealth = 20f;
    [SerializeField] private float milestone = 7;
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float increasePerWave = 2;
    private ParticleSystem deathParticlesInstance;
 
    [HideInInspector] public bool isTargeted = false;
    
    [SerializeField] private int enemyValue = 1;

    private bool isDead = false;
 
    [Header("References")]
    [SerializeField] Animator animator;

    [SerializeField] private ParticleSystem deathParticles;
    public GameObject damagePopupPrefab;

    private static PlayerController playerController;

   
    void Start()
    {
        if(playerController==null) playerController = GameObject.Find("Player")?.GetComponent<PlayerController>();
        currentHealth = baseHealth;
    }

    void Update(){   
    
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
            popup.transform.GetChild(0).GetComponent<TextMesh>().text = ""+damage.ToString();
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
    return Math.Abs(value - 1f) < 0.0001f;
    }
    private void SpawnDeathInstance(){
       deathParticlesInstance = Instantiate(deathParticles, transform.position, Quaternion.identity);
    }
    public void TakeDamageOverTime(float damage, float time, float step) {
    
        
    }

    public void modifyHealthFromWaveNumber(int waveNumber){
        currentHealth = baseHealth + (waveNumber * increasePerWave);
        currentHealth *= Mathf.Pow(scaleFactor , (int)waveNumber/milestone);
    }
    
    
    
}

