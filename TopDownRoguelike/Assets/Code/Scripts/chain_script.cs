using UnityEngine;

public class chain_script : MonoBehaviour
{
    Rigidbody2D rb;
    //public Transform playerPos;
    public float chainSpeed = 0f;
    HingeJoint2D hingeJoint;
    JointMotor2D motor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
        motor = hingeJoint.motor;
        motor.motorSpeed = chainSpeed;
        hingeJoint.motor = motor;


        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
       // rb.MovePosition(playerPos.position * Time.fixedDeltaTime *m_Speed);
       //motor.motorSpeed = -100;
       //hingeJoint.momtor = motor;
        
    }
    public void setSpeed(float speed){
        chainSpeed = speed;
        motor.motorSpeed = chainSpeed;
        hingeJoint.motor = motor;
    }
}
