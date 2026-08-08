using System.Collections;
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
        player_transform = GameObject.FindWithTag("Player").transform;
        //animator.Play("Patrol");
    }
    void SpawnEnemy(){

        StartCoroutine(SpanKusang());
    }
    private IEnumerator SpanKusang(){
        
        Vector3 position = player_transform.position;
        yield return new WaitForSeconds(0.15f);
        animator.SetTrigger("attack");
        GameObject current = Instantiate(sang, position, Quaternion.identity);
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

    public void OnDestroy()
    {
        if(prev!=null){
            Destroy(prev);
        }
    }
}
