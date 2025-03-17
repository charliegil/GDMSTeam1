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

   public skillTreeUpgrade(string desc , float val , upgradeType typ , int prix , int id ){
    description = desc;
    value = val;
    type = typ;
    price = prix;
   }

   public skillTreeUpgrade()
    {
        System.Random random = new System.Random();

        price = random.Next(1,5); 
        value = random.Next(1, 10) * 0.5f; 
        description = $"Upgrade {price}"; 
   
        type = (upgradeType)random.Next(0, Enum.GetValues(typeof(upgradeType)).Length);
    }
    public skillTreeUpgrade(int rarity , int value, String description , String Stype){
        
        this.rarity = rarity;
        this.value = value;
        price = UnityEngine.Random.Range(rarity, rarity+1);
        type = (upgradeType)Enum.Parse(typeof(upgradeType), Stype);

        if(value == -1){
            this.value = UnityEngine.Random.Range(0.05f*((float)rarity), 0.09f*((float)rarity));
        }
        string[] desc = description.Split(':'); // used for inserting the value
        this.description = desc[0] + value + desc[1];
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
        return description + " price: " + price + " modififer: " + value;
    }
    public bool Isbought(){
        return bought;
    }
  
}

public enum upgradeType{
    Attack,
    Defence,
    /// <summary>
    /// increase Maximum Health by a percentage
    /// </summary>
    Health,
    PhaseCooldown,
    PhaseDuration,
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



}



