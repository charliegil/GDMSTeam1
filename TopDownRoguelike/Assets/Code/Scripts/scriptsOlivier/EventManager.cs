
using System;
using Unity.VisualScripting;
using UnityEngine;
public static class EventManager
{
    public static event Action OnPlayerDied;

    public static event Action OnEnemyDied;

    public static event Action<float> OnPlayerTakeDamage;

    public static event Action<skillNode> OnBuySkill;

    public static event Action<skillNode> OnSellSkill;

    public static event Action<Vector2> OnSpawnCollectible;

    /// <summary>
    /// (gameObjectID , damage). Enemy will compare the ID with their own, taking damage if it is their own
    /// </summary>
    public static event Action<int, float> OnEnemyTakeDamage;

    

    public static void PlayerDied()
    {
        OnPlayerDied?.Invoke();
    }

    public static void EnemyDied()
    {
        OnEnemyDied?.Invoke();
    }

    public static void PlayerTakeDamage(float damage)
    {
        Debug.Log("take damage");
        OnPlayerTakeDamage?.Invoke(damage);
    }
    public static void BuySkill(skillNode node){
        OnBuySkill?.Invoke(node);

    }
    public static void SellSkill(skillNode node){
        OnSellSkill?.Invoke(node);

    }

    public static void SpawnCollectible(Vector2 position){
        OnSpawnCollectible?.Invoke(position);
    }

    public static void EnemyTakeDamage(int id, float damage){
        OnEnemyTakeDamage?.Invoke(id,damage);
    }

    
}