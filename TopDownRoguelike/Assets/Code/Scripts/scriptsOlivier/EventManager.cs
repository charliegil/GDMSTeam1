
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

    public static event Action<float> OnPhaseDurationBoost;
    public static event Action<float> OnPhaseCooldownBoost;

    public static event Action<float> OnSpeedBoost;
    public static event Action<float> OnCriticalHitBoost;
    public static event Action<float> OnIncreaseMaxHealth;

    /// <summary>
    /// (gameObjectID , damage). Enemy will compare the ID with their own, taking damage if it is their own
    /// </summary>
    public static event Action<int, float> OnEnemyTakeDamage;

/// <summary>
/// event that fires when you gain a skill Point that you can soend in the skill tree
/// </summary>
    public static event Action<int> OnSkillPointAcquired;

    public static event Action<float, CollectableType> OnDurationBoostStarted; 

    

    private static void TriggerEvent(Action action) => action?.Invoke();
    private static void TriggerEvent<T>(Action<T> action, T param) => action?.Invoke(param);
    private static void TriggerEvent<T1, T2>(Action<T1, T2> action, T1 param1, T2 param2) => action?.Invoke(param1, param2);

    public static void PlayerDied() => TriggerEvent(OnPlayerDied);
    public static void EnemyDied() => TriggerEvent(OnEnemyDied);
    public static void PlayerTakeDamage(float damage) => TriggerEvent(OnPlayerTakeDamage, damage);
    public static void BuySkill(skillNode node) => TriggerEvent(OnBuySkill, node);
    public static void SellSkill(skillNode node) => TriggerEvent(OnSellSkill, node);
    public static void SpawnCollectible(Vector2 position) => TriggerEvent(OnSpawnCollectible, position);
    public static void EnemyTakeDamage(int id, float damage) => TriggerEvent(OnEnemyTakeDamage, id, damage);
    public static void SkillPointAcquired(int value) => TriggerEvent(OnSkillPointAcquired, value);
    public static void AttackBoost(float value) => TriggerEvent(OnAttackBoost, value);
    public static void DefenceBoost(float value) => TriggerEvent(OnDefenceBoost, value);
    public static void PhaseDurationBoost(float value) => TriggerEvent(OnPhaseDurationBoost, value);
    public static void PhaseCooldownBoost(float value) => TriggerEvent(OnPhaseCooldownBoost, value);

    public static void SpeedBoost(float value) => TriggerEvent(OnSpeedBoost, value);
    public static void CriticalHitBoost(float value) => TriggerEvent(OnCriticalHitBoost, value);
    public static void IncreaseMaxHealth(float value) => TriggerEvent(OnIncreaseMaxHealth, value);
    public static void DurationBoostStarted(float duration, CollectableType type) => TriggerEvent(OnDurationBoostStarted, duration, type);

    
    
}