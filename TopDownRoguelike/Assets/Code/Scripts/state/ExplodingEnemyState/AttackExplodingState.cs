using UnityEngine;

public class AttackExplodingState : State
{
    public int damage;
    public int DamageArea; // the radius of the explosion

    public GameObject DamageZonePrefab;     
    public override void Enter()
    {
        // instead, decelerate
        body.linearVelocity  = new Vector2(0, 0);
        Instantiate(DamageZonePrefab, transform.position, Quaternion.identity);

    }
    public override void Do()
    {
        //Instantiate(DamageZonePrefab, transform.position, Quaternion.identity);
        //body.linearVelocity  = new Vector2(0, 0);

    }
    public override void Exit()
    {
        
    }
}
