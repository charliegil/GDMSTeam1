using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D body; //to see
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = body.linearVelocity.magnitude;
        animator.SetFloat("speed" , currentSpeed);
    }
}
