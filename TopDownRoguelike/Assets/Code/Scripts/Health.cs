using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float currentHealth = 100f;

    [SerializeField] float Totalhealth = 0;

    [SerializeField] newHealth UIHealth;


    // will need to integrate the UI health bar with This

    // // Update is called once per frame
    // void Update()
    // {
    //     if (currentHealth <= 0) {
    //         Destroy(gameObject);
    //     }
    // }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
    public void TakeDamageOverTime(float damage , float time, float step){

    }
    private IEnumerator TakePoisonDamage(float damage, float time, float step){
        
        // deals y damage for x time, with taking damage every z step in seconds
        float stepDmg = damage / time;
        if(step ==  0){
            UIHealth.addHP(damage * time , true);
            yield return null;
            time = -1;
        }
        while(time> 0){
            TakeDamage(step);
            time-=step;
            UIHealth.addHP(stepDmg, false);
            yield return new WaitForSeconds(step);
        }
        
    }
}
