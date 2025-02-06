using UnityEngine;

public class CamerController : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }
}
