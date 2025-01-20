using UnityEngine;

public class Tong : Player
{
    private void Awake(){
        attackDmg = 7;
    }
    public override void PerformAttack(){
        Debug.Log("Tong do some attack");
    }
    public override void HandleInput(){
        
    }
    
}
