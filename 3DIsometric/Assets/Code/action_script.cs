using Unity.VisualScripting;
using UnityEngine;

public class action_script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEvents.current.OnPressETransform += OnTransform;
    }

   
    void OnTransform(){
        Debug.Log("Action");
    }
}
