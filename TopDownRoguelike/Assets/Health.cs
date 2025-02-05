using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float currentHealth = 100f;

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
}
