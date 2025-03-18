using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Health : MonoBehaviour
{
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private float Totalhealth = 0;

    [SerializeField] private int enemyValue = 1;


    [SerializeField] Animator animator;
 

    public GameObject damagePopupPrefab;

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
        Totalhealth = currentHealth;
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        animator.SetTrigger("takeDamage");
        
        if(gameObject.name.Contains("Ranged")){
            Debug.Log("i am a ranged enemy");
            gameObject.GetComponent<RangedEnemy>().isRetreating = true;
        }
        if (damagePopupPrefab != null) {
            GameObject popup = Instantiate(damagePopupPrefab, transform.position , Quaternion.identity) as GameObject;//+ new Vector3(0,1,0)
            popup.transform.GetChild(0).GetComponent<TextMesh>().text = ""+damage;
            popup.transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 10;
            //DamagePopup damagePopup = popup.GetComponent<DamagePopup>();
            //damagePopup.Setup(damage);
        }
        if(currentHealth<=0) OnDeath();
    }
    public void OnDeath(){
        
        EventManager.EnemyDied();
        if (ScoreManager.Instance != null) {
            ScoreManager.Instance.AddScore(enemyValue);
        }
        EventManager.SpawnCollectible(transform.position);
        Destroy(gameObject);
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

