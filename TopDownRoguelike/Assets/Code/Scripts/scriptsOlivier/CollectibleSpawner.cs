using TMPro;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour , IEventListener
{
    /// <summary>
    /// the list of prefabs used to spawn random items, that help the player. 
    /// </summary>
    public PowerUp [] PowerUps;

    [SerializeField] [Range(0f, 1f)] private float EditorDropRate = 1; // Unity does not serialize static fields

    [Range(0f, 1f)] public static float DropRate = 1.0f; 

    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DropRate = EditorDropRate;
        subscribe(); 
        fixProbability();  
    }

    void spawnRandomItem(Vector2 position){
        float randomValue = UnityEngine.Random.value;
        if(randomValue >  DropRate) return;
        GameObject powerUp = GetRandomPowerUp();
        Debug.Log(powerUp.name);
        Instantiate(powerUp,position,Quaternion.Euler(0,0,0));
    }
    public GameObject GetRandomPowerUp(){
        float randomValue = UnityEngine.Random.value; // Value between 0 and 1
        
        float cumulative = 0f;

        foreach (var entry in PowerUps){
            cumulative += entry.probability;
            
            if (randomValue <= cumulative) return entry.powerUp;
            
        }

        return null;
    }
    void fixProbability(){
        foreach (var entry in PowerUps){
            if(float.Equals(entry.probability,0f)) entry.probability = 1f/PowerUps.Length; 
            
        }
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
[System.Serializable]
public class PowerUp{
    public GameObject powerUp;
    [Range(0f, 1f)] public float probability;
}
