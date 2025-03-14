
using System;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Defines event that other classes can listen to. This removes a lot of dependencies from one script to another. 
/// no longer need to reference a script or gameobject in some classes
/// </summary>

public static class EventManager
{
    public static event Action OnPlayerDied;

    public static event Action OnEnemyDied;

    public static event Action<float> OnPlayerTakeDamage;

    public static event Action<skillNode> OnBuySkill;

    public static event Action<skillNode> OnSellSkill;

    public static event Action<Vector2> OnSpawnCollectible;

    public static event Action<float> OnAttackBoost;

    public static event Action<float> OnDefenceBoost;

    public static event Action<float> OnDashBoost;
    public static event Action<float> OnSpeedBoost;
    public static event Action<float> OnCriticalHitBoost;




    /// <summary>
    /// (gameObjectID , damage). Enemy will compare the ID with their own, taking damage if it is their own
    /// </summary>
    public static event Action<int, float> OnEnemyTakeDamage;

/// <summary>
/// event that fires when you gain a skill Point that you can soend in the skill tree
/// </summary>
    public static event Action<int> OnSkillPointAcquired;

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
    public static void SkillPointAcquired(int value){
        OnSkillPointAcquired?.Invoke(value);
    }
    public static void AttackBoost(float value){
        OnAttackBoost?.Invoke(value);
    }
    public static void DefenceBoost(float value){
        OnDefenceBoost?.Invoke(value);
    }
    public static void DashBoost(float value) => OnDashBoost?.Invoke(value);
    public static void SpeedBoost(float value) => OnSpeedBoost?.Invoke(value);
    public static void CriticalHitBoost(float value) => OnCriticalHitBoost?.Invoke(value);

    
    
}