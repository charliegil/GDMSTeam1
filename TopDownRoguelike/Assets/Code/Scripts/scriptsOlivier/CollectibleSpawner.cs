using UnityEngine;

public class CollectibleSpawner : MonoBehaviour , IEventListener
{
    /// <summary>
    /// the list of prefabs used to spawn random items, that help the player. 
    /// </summary>
    public GameObject [] PowerUps;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     subscribe();   
    }

    void spawnRandomItem(Vector2 position){
        int randomItem = Random.Range(0, PowerUps.Length);
        Instantiate(PowerUps[randomItem],position,Quaternion.Euler(0,0,0));
    }

    /*void spawnSpecificItem(Vector2 position){

    }*/

    void OnDisable()
    {
        unsubscribe();
    }

    public void subscribe()
    {
        EventManager.OnSpawnCollectible += spawnRandomItem;
    }

    public void unsubscribe()
    {
        EventManager.OnSpawnCollectible -= spawnRandomItem;
    }
}
