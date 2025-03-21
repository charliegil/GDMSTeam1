
using UnityEngine;

public class FloatingTextHangler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 1f);
        transform.localPosition+= new Vector3(0, 1f,0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
