using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake(){
        current = this;
    }

    public event Action OnPressETransform;
    public void PressETransform(){
        Debug.Log("transform");
        if(OnPressETransform!=null){
            OnPressETransform();
        }
    }


}
