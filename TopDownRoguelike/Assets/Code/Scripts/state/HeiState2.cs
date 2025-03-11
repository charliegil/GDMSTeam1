using UnityEngine;

public class HeiState2 : State
{
    public AttackControl control;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Set(control, true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
