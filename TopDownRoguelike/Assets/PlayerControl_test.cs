using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControl_test : MonoBehaviour
{
    // private AgentAnimations agentAnimations;
    // private AgentMover agentMover;
    // private Vector2 pointerInput, movementInput;
    // public Vector2 Pointer => pointerInput;
    // private WeaponParent weaponParent;

    // [SerializeField]
    // private InputActionReference movement, attack, pointerPosition;


    // private void Awake()
    // {
    //     agentAnimations = GetComponentInChildren<AgentAnimations>();
    //     weaponParent = GetComponentInChildren<WeaponParent>();
    //     agentMover = GetComponent<AgentMover>();
    // }
    // private void AnimateCharacter(){
    //     Vector2 lookDirection = pointerInput - (Vector2) transform.position;
    //     if(weaponParent.WeaponRotationStopped == false)
    //     agentAnimations.RotateToPoinnter(lookDirection);
    //     agentAnimations.PlayAnimation(movementInput);
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     PointerInput = GetPointerInput();
    //     movementInput = movement.action.ReadValue<Vector2>();
    //     agentMover.MovementInput = movementInput;
    //     AnimateCharacter();
    //     if(Input.GetMouseButtonDown(0)){
    //         WeaponParent.PerformAnAttack();
    //     }

        
    // }
    // private Vector2 GetPointer(){
    //     Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
    //     mousePos.z = Camera.main.nearClipPlane;
    //     return Camera.main.ScreenToWorldPoint(mousePos);
    // }
}
