using UnityEngine;

public class AttackSpawn : State
{
    public GameObject enemy1;
    public Transform HTScene;
    public Transform HBScene;
    public Transform WRScene;
    public Transform WLScene;
    //public AnimationClip anim;
    public override void Enter()
    {
        InvokeRepeating("SpawnEnemy", 2.0f, 2f);
        //animator.Play("Patrol");
    }
    void SpawnEnemy(){
        Instantiate(enemy1, RandomPos(), Quaternion.identity);
    }
    Vector3 RandomPos(){
        float posX = Random.Range(WLScene.position.x, WRScene.position.x);
        float posY = Random.Range(HBScene.position.y, HTScene.position.y);
        Vector3 spawnPos = new Vector3(posX, posY,0);
        return spawnPos;
    }
    public override void Do()
    {

    }
    public override void Exit()
    {
        
    }
}
