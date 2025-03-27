using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;

public class SkillTreeHandler : MonoBehaviour , IEventListener
{
    public int skillPoints = 0;
    public TextMeshProUGUI skillPointsText;

    private List<skillTreeUpgrade> upgradesOwned = new List<skillTreeUpgrade>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        skillPointsText.text = "skill points "+skillPoints;
        subscribe();
    }



    // Update is called once per frame
    
    private void HandleSkillPurchase(skillNode skillnode){
        
        if (skillnode == null) return;
        int original  = skillPoints;
        skillPoints = skillnode.buySkill(skillPoints);
            
        if(original != skillPoints){
            upgradesOwned.Add(skillnode.getNode().GetUpgrade());
            AudioManager.instance.PlaySound("skillBought");
            //Debug.Log("you bought" + skillnode);
            skillPointsText.text = "skill points "+skillPoints;
            applyUpgrade(skillnode.getNode().GetUpgrade(),false);
            
        }
    }
    private void HandleSkillPointAcquired(int value){
        skillPoints+=value;
        skillPointsText.text = "skill points "+skillPoints;
    }

    private void HandleSkillSell(skillNode skillnode){
        
        if (skillnode == null) return;
        int original  = skillPoints;
        skillPoints = skillnode.sellSkill(skillPoints);
            
        if(original != skillPoints){ // the upgrade was already purchased
            upgradesOwned.Remove(skillnode.getNode().GetUpgrade());
            Debug.Log("you sold" + skillnode.getNode());
            skillPointsText.text = "skill points "+skillPoints;
            applyUpgrade(skillnode.getNode().GetUpgrade(),true);
        }
    }

/// <summary>
/// apply the upgrade to reflect on the game experience
/// </summary>
/// <param name="upgrade"></param>
/// <param name="undo"> specifies if you have to apply(false) or undo(true) the upgrade</param>
    private void applyUpgrade(skillTreeUpgrade upgrade, bool undo){
        
        float value = upgrade.value;
        upgradeType type = upgrade.type;
        if(value != 0 && undo){
            value = 1/value;
        }
        
        Debug.Log(type.ToString());
        switch (type)
        {
            case upgradeType.Attack:
                EventManager.AttackBoost(value);
                break;

            case upgradeType.Defence:
                EventManager.DefenceBoost(value);
                break;

            case upgradeType.Health:
                Debug.Log("enhancing health");
                EventManager.IncreaseMaxHealth(value);
                break;

            case upgradeType.PhaseCooldown:
                EventManager.PhaseCooldownBoost(value);
                break;

            case upgradeType.PhaseDuration:
                EventManager.PhaseDurationBoost(value);
                break;

            case upgradeType.CritiqualHit:
                EventManager.CriticalHitBoost(value);
                break;

            case upgradeType.DropRate:
                CollectibleSpawner.DropRate = Mathf.Min(1,CollectibleSpawner.DropRate+value);
                break;

            case upgradeType.PowerUpEffectMultiplier:
                Collectable.effectMultiplier *= value;
                break;

            case upgradeType.PowerUpDurationtMultiplier:
                Collectable.durationMultiplier /= value;
                break;

            case upgradeType.WeaponUnlocked:
                Debug.Log("You have got a new weapon");
                break;
            case upgradeType.FullHealthAtWaveEnd:
                WaveSystem.gainFullHealthOnEnd = true;
                break;
            case upgradeType.SkillPointAtWaveEnd:
                WaveSystem.skillPointsOnEnd = (int)value;
                break;
            case upgradeType.AllCritiqualHitBelowCertainHp:
                PlayerController.allCritiqualHits = (int)value;
                break;

            case upgradeType.InstantKillBelowCertainHp:
                PlayerController.InstantKillHP = (int)value;
                break;
            case upgradeType.RangedAttackCooldown:
                PlayerShoot.ReduceTimeBetweenShots(value);
                break;
            case upgradeType.BeamAttackCooldown:
                EventManager.BeamAttackCooldown(value);
                break;
            case upgradeType.BeamAttackDuration:
                EventManager.BeamAttackDuration(value);
                break;
            case upgradeType.BeamDamageIncrease:
                EventManager.BeamAttackDamage(value);
                break;
            case upgradeType.BeamTickRate:
                EventManager.BeamDamageTickDelay(value);
                break;
            case upgradeType.BeamAddTarget:
                EventManager.AddMoreTargets();
                break;
            case upgradeType.Revival:
                PlayerController.oneMoreChance =  true;
                break;
            
            default:
                Debug.LogWarning("not recognized upgrade type: " + type);
                break; 
        }   
    }




    public void subscribe()
    {
        EventManager.OnBuySkill += HandleSkillPurchase;
        EventManager.OnSellSkill += HandleSkillSell;
        EventManager.OnSkillPointAcquired += HandleSkillPointAcquired;
    }

    public void unsubscribe()
    {
        EventManager.OnBuySkill -= HandleSkillPurchase;
        EventManager.OnSellSkill -= HandleSkillSell;
        EventManager.OnSkillPointAcquired -= HandleSkillPointAcquired;
    }

    void OnDisable()
    {
      unsubscribe();
    }

}
