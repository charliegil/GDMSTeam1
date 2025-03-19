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

    public skillTreeUpgrade(int rarity){
        this.rarity = rarity;
        this.value = 1f+ UnityEngine.Random.Range(0.05f*rarity, 0.095f*rarity);
        price = UnityEngine.Random.Range(rarity, rarity+1);
        
        type = (upgradeType)UnityEngine.Random.Range(0, 10);
        
        description = $"Upgrade of type {type.ToString()} with value {value}"; 
    }
    public skillTreeUpgrade(int rarity , float value, String description , upgradeType Stype){
        
        this.rarity = rarity;
        if(rarity == -1){
            this.rarity = UnityEngine.Random.Range(1, 4);
        }
        price = UnityEngine.Random.Range(rarity, rarity+1);
        if(Stype == upgradeType.Random){
            type = (upgradeType)UnityEngine.Random.Range(0, 10);
        }
        else{
           type =  Stype;
        }

        this.value = value;
        if(value == -1){
            this.value = 1f+ UnityEngine.Random.Range(0.05f*rarity, 0.095f*rarity);
        }
        if(description.Contains(":")){
            string[] desc = description.Split(':'); // used for inserting the value
            this.description = desc[0] + value + desc[1];
        }
        else if(Stype == upgradeType.Random){
            description = $"Upgrade of type {type.ToString()} with value {value}"; 
        }
        else{
            this.description = description;
        }
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
    /// <summary>
    /// For all skill tree upgrades that unlocks a new attack
    /// </summary>
    WeaponUnlocked,

    // ========= CUSTOM UPGRADES HERE =========
    /// <summary>
    /// upgrade that will make you full health when you finish a wave
    /// </summary>
    fullHealthAtWaveEnd,
    SkillPointAtWaveEnd,
    allCritiqualHitBelowCertainHp,
    InstantKillBelowCertainHp,

    Random,



}



