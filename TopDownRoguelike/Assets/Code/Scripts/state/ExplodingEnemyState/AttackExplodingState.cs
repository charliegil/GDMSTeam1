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

    }
    public override void Do()
    {
        Instantiate(DamageZonePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        body.linearVelocity  = new Vector2(0, 0);

    }
    public override void Exit()
    {
        
    }
}
