using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D body; 
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        float currentSpeed = body.linearVelocity.magnitude;
        animator.SetFloat("speed" , currentSpeed);
    }
}
