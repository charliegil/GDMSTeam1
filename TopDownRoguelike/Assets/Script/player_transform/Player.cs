using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Player : MonoBehaviour, IControllable
{
    public static Player ActivePlayer{get;private set;}
    public static int playerHp = 10;
    [SerializeField] protected bool isActive = false;
    [SerializeField] protected int hp;
    [SerializeField] protected int attackDmg;

    
    public bool CanTransform {get; private set;} = false;
    public GameObject TargetTransformPlayer {get; private set;}
    
    protected PlayerInputHandler inputHandler;
    public abstract void PerformAttack();
    public abstract void HandleInput();
    public abstract void HandleMovement();
    
    private void Start(){
    }
    public void ActivateInput(){
        isActive = true;
        SetAsActive();
    }
    public void SetAsActive(){
        ActivePlayer = this;
        isActive = true;
        Debug.Log($"{gameObject.name} is now the active player.");

    }
    public void DisactivateInput(){
        isActive = false;
    }
    public void HandleTransform(){
        if(inputHandler.TransformTriggered && CanTransform){
            Debug.Log("Pressed E for transform and abled");
            if (TargetTransformPlayer!=null && TargetTransformPlayer.TryGetComponent<IControllable>(out var controllable))
            {
                Debug.Log("Activated IControllable on another");
                // Activate or perform the required action on the IControllable
                controllable.ActivateInput();
                //controllable.SetAsActive();
                //TryGetComponent<IControllable>(out var currentControl);
                DisactivateInput();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
       Debug.Log("Active player null? " + (ActivePlayer == null));
        if(other.gameObject != ActivePlayer.gameObject &&other.gameObject.layer == LayerMask.NameToLayer("Monster")){
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
