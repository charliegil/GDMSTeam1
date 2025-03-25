using UnityEngine;

public class enemy_anim_script : MonoBehaviour
{
    public Animator anim;
    private Vector2 direction;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        anim.SetFloat("x", direction.x);
        anim.SetFloat("y", direction.y);
        
    }
}
