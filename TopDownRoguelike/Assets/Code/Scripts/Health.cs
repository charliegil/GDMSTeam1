using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private float Totalhealth = 0;
    [SerializeField] private HealthManager UIHealth;

    public GameObject damagePopupPrefab;

    // will need to integrate the UI health bar with This

    // // Update is called once per frame
    // void Update()
    // {
    //     if (currentHealth <= 0) {
    //         Destroy(gameObject);
    //     }
    // }

    
    void Update()
    {
        if (currentHealth <= 0) {
         
            if (ScoreManager.Instance != null) {
                ScoreManager.Instance.AddScore(1);
            }
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        if (damagePopupPrefab != null) {

            GameObject popup = Instantiate(damagePopupPrefab, transform.position + new Vector3(0,1,0), Quaternion.identity);
            DamagePopup damagePopup = popup.GetComponent<DamagePopup>();
            damagePopup.Setup(damage);
        }
    }
    
    public void TakeDamageOverTime(float damage, float time, float step) {
    
        
    }
    
    private IEnumerator TakePoisonDamage(float damage, float time, float step)
    {   
        // deals y damage for x time, with taking damage every z step in seconds
        float stepDmg = damage / time;
        if (step == 0)
        {
            UIHealth.addHP(damage * time, true);
            yield return null;
            time = -1;
        }
        while (time > 0)
        {
            TakeDamage(step);
            time -= step;
            UIHealth.addHP(stepDmg, false);
            yield return new WaitForSeconds(step);
        }
    }
}

