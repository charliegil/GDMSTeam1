using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float currentHealth = 100f;

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        //Debug.Log(currentHealth);

        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
}
