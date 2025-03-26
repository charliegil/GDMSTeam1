using UnityEngine;
using System;

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
        if(this.type == upgradeType.Random) type = getRandomUpgrade();
        rarity = getRandomRarity();
        price = getPriceFromRarity();
        value = 1f + getValueFromRarity();
        if(rarity == 5){
            rarity = UnityEngine.Random.Range(5, 8);
            price =  getPriceFromRarity();
            value = 1f + getValueFromRarity();
            rarity = 5;
        }
    }

    public skillTreeUpgrade(int rarity){ // for common upgrades
        this.rarity = rarity;
        this.value = 1f+ getValueFromRarity();
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
        return UnityEngine.Random.Range(rarity, rarity+1);
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
        return description + ";" + price + ";" +value.ToString("F2") +";"+rarity;
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
    }
    return getValueFromRarity(); 
    }


  
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



}



