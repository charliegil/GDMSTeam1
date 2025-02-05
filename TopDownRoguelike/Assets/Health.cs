using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float currentHealth = 100f;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentHealth);
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        Debug.Log(currentHealth);
    }
}
