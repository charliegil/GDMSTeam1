using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvents : MonoBehaviour {

    [SerializeField] private List<KeyEvent> keyEvents = new List<KeyEvent>();

    private void Update()
    {
        foreach (var keyEvent in keyEvents)
        {
            if (Input.GetKeyDown(keyEvent.key))
            {
                TriggerEvent(keyEvent);
            }
        }
    }

    private void TriggerEvent(KeyEvent keyEvent)
    {
        GameEventType eventType = keyEvent.eventType;

        switch (eventType)
        {
            case GameEventType.PlayerDied:
                EventManager.PlayerDied();
                break;
            case GameEventType.EnemyDied:
                EventManager.EnemyDied();
                break;
            case GameEventType.PlayerTakeDamage:
                EventManager.PlayerTakeDamage(keyEvent.value);
                break;
            case GameEventType.BuySkill:
                EventManager.BuySkill(null); 
                break;
            case GameEventType.SellSkill:
                EventManager.SellSkill(null);
                break;
            case GameEventType.SpawnCollectible:
                EventManager.SpawnCollectible(Vector2.zero);
                break;
            case GameEventType.EnemyTakeDamage:
                EventManager.EnemyTakeDamage(1, 10f);
                break;
            case GameEventType.SkillPointAcquired:
                EventManager.SkillPointAcquired((int)keyEvent.value);
                break;
            case GameEventType.AttackBoost:
                EventManager.AttackBoost(keyEvent.value);
                break;
            case GameEventType.DefenceBoost:
                EventManager.DefenceBoost(keyEvent.value);
                break;
            case GameEventType.PhaseDurationBoost:
                EventManager.PhaseDurationBoost(keyEvent.value);
                break;
            case GameEventType.PhaseCooldownBoost:
                EventManager.PhaseCooldownBoost(keyEvent.value);
                break;
            case GameEventType.SpeedBoost:
                EventManager.SpeedBoost(keyEvent.value);
                break;
            case GameEventType.CriticalHitBoost:
                EventManager.CriticalHitBoost(keyEvent.value);
                break;
            case GameEventType.IncreaseMaxHealth:
                EventManager.IncreaseMaxHealth(keyEvent.value);
                break;
            case GameEventType.DurationBoostStarted:
                StartCoroutine(testDurationUI(keyEvent));
                break;
            default:
                Debug.LogWarning($"Unhandled event: {eventType}");
                break;
        }
    }
    private IEnumerator testDurationUI(KeyEvent keyEvent){
        EventManager.DurationBoostStarted(keyEvent.value, CollectableType.SpeedBoost);
        yield return new WaitForSeconds(1);
        EventManager.DurationBoostStarted(keyEvent.value, CollectableType.AttackBoost);
        yield return new WaitForSeconds(1);
        EventManager.DurationBoostStarted(keyEvent.value, CollectableType.DefenceBoost);
        yield return new WaitForSeconds(1);
        EventManager.DurationBoostStarted(keyEvent.value, CollectableType.Invicible);
        yield return new WaitForSeconds(1);
        EventManager.DurationBoostStarted(keyEvent.value, CollectableType.CritiqualHit);
    }
}
[System.Serializable]
public class KeyEvent
{
    public KeyCode key;
    public GameEventType eventType;

    public float value; 
}

public enum GameEventType
{
    PlayerDied,
    EnemyDied,
    PlayerTakeDamage,
    BuySkill,
    SellSkill,
    SpawnCollectible,
    EnemyTakeDamage,
    SkillPointAcquired,
    AttackBoost,
    DefenceBoost,
    PhaseDurationBoost,
    PhaseCooldownBoost,
    SpeedBoost,
    CriticalHitBoost,
    IncreaseMaxHealth,
    DurationBoostStarted
}