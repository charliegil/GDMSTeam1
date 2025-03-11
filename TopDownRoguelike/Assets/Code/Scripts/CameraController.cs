using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    private void Update()
    {
        // player = Player.ActivePlayer.gameObject;
        // if (player != null){
        //  transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        // }
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
    
}
