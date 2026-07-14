
using System.Collections.Generic;

public class skillTreeUpgrade
{
    private string flavorText;

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
        this.flavorText = description;

    }
/// <summary>
/// For upgrades that are common
/// </summary>
    public skillTreeUpgrade(string description, upgradeType type){
        this.flavorText = description;
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
        
        flavorText = $"Upgrade of type {type.ToString()} with value {value}"; 
    }
    

    private float getValueFromRarity(){
        return UnityEngine.Random.Range(0.08f*rarity, 0.12f*rarity);
    }
    private upgradeType getRandomUpgrade(){
         return (upgradeType)UnityEngine.Random.Range(0, 13);
    }
    private int getPriceFromRarity(){
        int adding = rarity/2;
        return UnityEngine.Random.Range(rarity+adding, 1+rarity+ adding);
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

    public string valueToString(float valeur){
        string valueString = valeur.ToString("0.##");
  
        if(valueString.EndsWith(".00")) valueString = valueString.Split(".")[0];
        return valueString;
    }
    public string descriptionToString(){
        if(! UpgradeToCategory.ContainsKey(type)) return type.ToString();
         
         string typeDescription = UpgradeToCategory[type];
         if(typeDescription.Contains("]")){
            string [] part = typeDescription.Split("]");
            string valueText = valueToString(value);
            if(part[1].Contains("%")){
                valueText = valueToString(value*100);
                part[1] = " percent " + part[1].Split("%")[1];
            }
            return part[0]  + valueToString(value) + part[1];
         }
         else return typeDescription;
    }
    public override string ToString(){
        
        string typeDescription = type.ToString();
        string valueString = valueToString(value);
        
        if(UpgradeToCategory.ContainsKey(type)) typeDescription= UpgradeToCategory[type];
        return flavorText + ";" + price + ";" + valueString +";"+rarity + ";" + descriptionToString();
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
            return 30f;
           
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
            return UnityEngine.Random.Range(0.01f*rarity, 0.02f*rarity);
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
        { upgradeType.Attack, "multiplies attack damage by ]x" },
        { upgradeType.Defence, "Decrease damage taken by ]x" },
        { upgradeType.PhaseCooldown, "Phase takes less time to recharge" },
        { upgradeType.PhaseDuration, "Make phase last longer by ]x" },
        { upgradeType.Health, "Increase max health by ]x, and heal completely" },
        { upgradeType.CritiqualHit, "Increase chance of critiqual hits by ]x" },
        { upgradeType.Speed, "increase speed by ]x" },
        { upgradeType.DropRate, "increase chance of getting boosts from dying enemies by ]%" },
        { upgradeType.PowerUpEffectMultiplier, "the effects of the boosts are increased by ]x" },
        { upgradeType.PowerUpDurationtMultiplier, "Boost duration" },
        
        
        { upgradeType.BeamAttackCooldown, "the cooldown of the laser takes ]x less time" },
        { upgradeType.BeamAttackDuration, "the duration of the laser is increased by ]x" },
        { upgradeType.BeamTickRate, "the dps of the laser is increase by ]x" },
        { upgradeType.BeamDamageIncrease, "Increase laser damage by ]x" },
        { upgradeType.BeamAddTarget, "you can now shoot one more laser" },


        { upgradeType.WeaponUnlocked, "Weapon" },

       
        { upgradeType.FullHealthAtWaveEnd, "gain full health when wave ends" },
        { upgradeType.SkillPointAtWaveEnd, "gain 3 Skill points when wave ends" },
        { upgradeType.AllCritiqualHitBelowCertainHp, "below 30hp, all your hits are critiqual" },
        { upgradeType.InstantKillBelowCertainHp, "below 3hp, all your hits kill the enemy" },
        { upgradeType.Revival, "when you die, you get respawned with half of your max health" },
        { upgradeType.Random, "random" },

        { upgradeType.RangedAttackCooldown, "the time between shooting is reduced by ] seconds" },
        { upgradeType.RangedAttackSpeed, "your bullets gain ] more speed" },
        { upgradeType.RangedNumberProjectile , "each click spawns one more bullet"},
        { upgradeType.RangedAttackDmg , "your bullets deal ] more damage"},
        { upgradeType.RangedAttackAdd , "the gods dont know what to do with this power up"},


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
    
    RangedNumberProjectile,
    
    RangedAttackAdd,
    RangedAttackDmg
}





