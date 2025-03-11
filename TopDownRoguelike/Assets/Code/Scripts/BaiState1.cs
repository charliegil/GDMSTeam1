using UnityEngine;

public class BaiState1 : State
{
    public GameObject sang;
    public Transform player_transform;
    private GameObject prev;
    //public AnimationClip anim;
    public override void Enter()
    {
        InvokeRepeating("SpawnEnemy", 2.0f, 2f);
        //animator.Play("Patrol");
    }
    void SpawnEnemy(){
        
        GameObject current = Instantiate(sang, player_transform.position, Quaternion.identity);
        if(prev!=null){
            Destroy(prev);
        }
        prev = current;
    }
    public override void Do()
    {


    }
    public override void Exit()
    {
        CancelInvoke("SpawnEnemy");
        if(prev!=null){
            Destroy(prev);
        }
        
    }
}
