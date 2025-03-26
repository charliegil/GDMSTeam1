using UnityEditor.Tilemaps;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Sprite;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D body; 
    
    private float localScaleX;
    private void Start()
    {
        localScaleX=transform.localScale.x;
    }


    // Update is called once per frame
    private void Update()
    {
        float currentSpeed = body.linearVelocity.magnitude;
        //Debug.Log("what is currentSpeed"+currentSpeed);
        animator.SetFloat("speed" , currentSpeed);//currentSpeed);
        if(body.linearVelocityX<0.01){
           transform.localScale= new Vector3(-localScaleX, transform.localScale.y, transform.localScale.z);
        }else if(body.linearVelocityX>0.0){
            transform.localScale= new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        }
        
    }
}
