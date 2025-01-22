using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Player : MonoBehaviour, IControllable
{
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
        //inputHandler = GameObject.Find("PlayerInputHandler").GetComponent<PlayerInputHandler>();
    }
    public void ActivateInput(){
        Debug.Log("isActive from Player" + isActive);
        isActive = true;
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
                //TryGetComponent<IControllable>(out var currentControl);
                DisactivateInput();
            }
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster")){
            Debug.Log($"Monster '{other.gameObject.name}' is nearby, transformation enabled.");
            Debug.Log("monster near able transform");
            TargetTransformPlayer = other.gameObject;
            CanTransform = true;
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster")){
            Debug.Log("monster near not able transform");
            CanTransform = false;
            TargetTransformPlayer = null;
        }
    }
    
    
}
