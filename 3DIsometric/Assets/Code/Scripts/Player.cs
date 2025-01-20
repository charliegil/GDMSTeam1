using UnityEngine;

public abstract class Player : MonoBehaviour, IControllable
{
    [SerializeField] protected int hp;
    [SerializeField] protected int attackDmg;

    
    public abstract void PerformAttack();
    public abstract void HandleInput();
}
