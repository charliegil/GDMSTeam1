using UnityEngine;
using System;

public class skillTreeUpgrade
{
    string description;

    float value;

    upgradeType type;

    bool bought = false;

    int price;

    int id;



   
   public skillTreeUpgrade(string desc , float val , upgradeType typ , int prix , int id ){
    description = desc;
    value = val;
    type = typ;
    price = prix;
    this.id = id;
   }

   public skillTreeUpgrade()
    {
        System.Random random = new System.Random();

        price = random.Next(1,5); 
        value = random.Next(1, 10) * 0.5f; 
        description = $"Upgrade {price}"; 
   
        type = (upgradeType)random.Next(0, Enum.GetValues(typeof(upgradeType)).Length);
    }
  
    public int doUpgrade(int skillPoints){ 
        if (skillPoints < price || bought) return  skillPoints;
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
    public int getId(){ return id;}
}

public enum upgradeType{
    attack,
    health,
    transform,
    dash,
    defence,
    critiqualHit,
    levelStart
    
}



