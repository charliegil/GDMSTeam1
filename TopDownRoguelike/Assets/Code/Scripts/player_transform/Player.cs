using UnityEngine;

public abstract class Player : MonoBehaviour, IControllable
{
    public static Player ActivePlayer{get;private set;}
    public static int playerHp = 10;
    [SerializeField] protected int hp;
    [SerializeField] protected int attackDmg;

    
    public bool CanTransform {get; private set;} = false;
    public GameObject TargetTransformPlayer {get; private set;}
    
    protected PlayerInputHandler inputHandler;
    public abstract void PerformAttack();
    public abstract void HandleInput();
    public abstract void HandleMovement();
    
    public void ActivateInput(){
        SetAsActive();
    }
    public void SetAsActive(){
        ActivePlayer = this;
        Debug.Log($"{gameObject.name} is now the active player.");
    }
        public void HandleTransform(){
        if(inputHandler.TransformTriggered && CanTransform){
            if (TargetTransformPlayer!=null && TargetTransformPlayer.TryGetComponent<IControllable>(out var controllable))
            {
                Debug.Log("Activated IControllable on another");
                controllable.ActivateInput();
                TargetTransformPlayer = null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other){

        if(other.gameObject != ActivePlayer.gameObject && other.gameObject.layer == LayerMask.NameToLayer("Monster")){
            Debug.Log("current Monster active is"+ActivePlayer.gameObject);
            Debug.Log($"Monster '{other.gameObject.name}' is nearby, transformation enabled.");
            TargetTransformPlayer = other.gameObject;
            CanTransform = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster")){
            Debug.Log("monster near not able transform");
            CanTransform = false;
            TargetTransformPlayer = null;
        }
    }
    
    
}
