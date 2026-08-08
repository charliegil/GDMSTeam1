using UnityEngine;

public class AttackPaperFollow : State
{
    public GameObject paper;
    public GameObject hei;
    public override void Enter()
    {
        InvokeRepeating("SpawnPaper", 2.0f, 1f);
        //animator.Play("Patrol");
    }
    void SpawnPaper(){
        Instantiate(paper, hei.transform.position , Quaternion.identity);
    }
    public override void Do()
    {

    }
    public override void Exit()
    {
        
    }
}
