using UnityEngine;
using System;
using UnityEditor.U2D.Aseprite;
using System.Collections.Generic;
using System.Collections;


public class skillTreeUpgrade
{
    private string description;

    public float value;

    public upgradeType type;

    private bool bought = false;

    private int price;
    private int rarity;

   

    public skillTreeUpgrade(): this(UnityEngine.Random.Range(1, 4))
    {}

/// <summary>
/// For special upgrades that are not common
/// </summary>
    public skillTreeUpgrade(int rarity ,int price, string description , upgradeType type){
        this.rarity = rarity;
        this.price = price;
        this.value = getValueFromUpgrade(type);
        this.type = type;
        this.description = description;

    }
/// <summary>
/// For upgrades that are common
/// </summary>
    public skillTreeUpgrade(string description, upgradeType type){
        this.description = description;
        this.type = type;
        if(this.type == upgradeType.Random) type = getRandomUpgrade();
        rarity = getRandomRarity();
        price = getPriceFromRarity();
        value = getValueFromUpgrade(type);
        if(rarity == 5){
            rarity = UnityEngine.Random.Range(5, 8);
            price =  getPriceFromRarity();
            value = getValueFromUpgrade(type);
            rarity = 5;
        }
    }

    public skillTreeUpgrade(int rarity){ // for common upgrades
        this.rarity = rarity;
        this.value = getValueFromUpgrade(type);
        price = getPriceFromRarity();
        
        type = getRandomUpgrade();
        
        description = $"Upgrade of type {type.ToString()} with value {value}"; 
    }
    

    private float getValueFromRarity(){
        return UnityEngine.Random.Range(0.08f*rarity, 0.12f*rarity);
    }
    private upgradeType getRandomUpgrade(){
         return (upgradeType)UnityEngine.Random.Range(0, 13);
    }
    private int getPriceFromRarity(){
        return UnityEngine.Random.Range(rarity*2, 1+rarity*2);
    }
    private int getRandomRarity(){
        return  UnityEngine.Random.Range(1, 6);
    }




  
    public int doUpgrade(int skillPoints){ 
        if (skillPoints < price || bought) return  skillPoints; // if you dont have enough skill points to buy that upgrade
        bought = true;
        return skillPoints-price;
    }
    public int undoUpgrade(int skillPoints){ 
        if (!bought ) return  skillPoints;
        bought = false;
        return skillPoints+price;
    }
    public override string ToString(){
        Debug.Log(type);
        string typeDescription = type.toString();
        if(UpgradeToCategory.ContainsKey(type)) typeDescription= UpgradeToCategory[type];
        return description + ";" + price + ";" +value.ToString("F2") +";"+rarity + ";" + typeDescription;
    }
    public bool Isbought(){
        return bought;
    }
    public int getRarity(){return rarity;}

    private float getValueFromUpgrade(upgradeType typeUpgrade){
        
        switch (typeUpgrade)
    {
        case upgradeType.FullHealthAtWaveEnd:
            return 1f;
            
        case upgradeType.SkillPointAtWaveEnd:
            return 3f;
           
        case upgradeType.AllCritiqualHitBelowCertainHp:
            return 10f;
           
        case upgradeType.InstantKillBelowCertainHp:
            return 3f;   
        case upgradeType.RangedAttackCooldown:
            return 0.05f * rarity;
        
        case upgradeType.Health:
            return 1f + UnityEngine.Random.Range(0.02f*rarity, 0.06f*rarity);
        
        case upgradeType.PowerUpDurationtMultiplier:
            return 1f + UnityEngine.Random.Range(0.03f*rarity, 0.8f*rarity);
        
        case upgradeType.PowerUpEffectMultiplier:
            return 1f + + UnityEngine.Random.Range(0.05f*rarity, 0.1f*rarity);
        case upgradeType.Revival:
            return 1f;
        case upgradeType.BeamAddTarget:
            return 1f;
        case upgradeType.DropRate:
            return 1f  + UnityEngine.Random.Range(0.01f*rarity, 0.02f*rarity);
        case upgradeType.RangedNumberProjectile:
            return 1f;
        case upgradeType.RangedAttackSpeed:
            return 2f;
        case upgradeType.RangedAttackDmg:
            return 5f;

    }
    return 1f + getValueFromRarity(); 
    }
    public static readonly Dictionary<upgradeType, string> UpgradeToCategory = new Dictionary<upgradeType, string>
    {
        { upgradeType.Attack, "Attack" },
        { upgradeType.Defence, "Defense" },
        { upgradeType.PhaseCooldown, "Phase cooldown" },
        { upgradeType.PhaseDuration, "Phase duration" },
        { upgradeType.Health, "max health" },
        { upgradeType.CritiqualHit, "critiqual hit" },
        { upgradeType.Speed, "speed" },
        { upgradeType.DropRate, "Boost drop rate" },
        { upgradeType.PowerUpEffectMultiplier, "Boost effect" },
        { upgradeType.PowerUpDurationtMultiplier, "Boost duration" },
        
        
        { upgradeType.BeamAttackCooldown, "Beam" },
        { upgradeType.BeamAttackDuration, "Beam" },
        { upgradeType.BeamTickRate, "Beam" },
        { upgradeType.BeamDamageIncrease, "Beam" },
        { upgradeType.BeamAddTarget, "Beam" },


        { upgradeType.WeaponUnlocked, "Weapon" },

       
        { upgradeType.FullHealthAtWaveEnd, "Wave" },
        { upgradeType.SkillPointAtWaveEnd, "Wave" },
        { upgradeType.AllCritiqualHitBelowCertainHp, "Critiqual hits" },
        { upgradeType.InstantKillBelowCertainHp, "Lethality" },
        { upgradeType.Revival, "Survival" },
        { upgradeType.Random, "random" },

        { upgradeType.RangedAttackCooldown, "Ranged attack cooldown" },
        { upgradeType.RangedAttackSpeed, "Ranged" },
        { upgradeType.RangedNumberProjectile , "Ranged"}
    };

  
}

public enum upgradeType{
    Attack,
    Defence,
    /// <summary>
    /// increase Maximum Health by a percentage
    /// </summary>
    PhaseCooldown,
    PhaseDuration,
    Health,
    CritiqualHit,
    Speed,
    /// <summary>
    /// influences the drop rate of power ups of the enemies when they die
    /// </summary>
    DropRate,
    /// <summary>
    /// influences the effect multiplier of the powers up that the enemies drop when they die
    /// </summary>
    PowerUpEffectMultiplier,
    /// <summary>
    /// influences the duration multiplier of the powers up that the enemies drop when they die
    /// </summary>
    PowerUpDurationtMultiplier,
    
    // ============ Upgrades about the beam here
    BeamAttackCooldown,
    BeamAttackDuration,
    BeamTickRate,
    BeamDamageIncrease,

    BeamAddTarget,
    /// <summary>
    /// For all skill tree upgrades that unlocks a new attack
    /// </summary>
    WeaponUnlocked,

    // ========= CUSTOM UPGRADES HERE =========
    /// <summary>
    /// upgrade that will make you full health when you finish a wave
    /// </summary>
    FullHealthAtWaveEnd,
    SkillPointAtWaveEnd,
    AllCritiqualHitBelowCertainHp,
    InstantKillBelowCertainHp,
    Revival,
    Random,

    // ============ Upgrades about the ranged attack here
    RangedAttackCooldown,
    RangedAttackSpeed,
    
    RangedNumberProjectile
    
    RangedAttackAdd,
    RangedAttackDmg
}





