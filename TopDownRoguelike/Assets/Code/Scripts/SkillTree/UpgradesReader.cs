using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpgradesReader{
    public static List<skillTreeUpgrade> CreateCommonUpgradesFromFile(){
        List<skillTreeUpgrade> upgrades = new List<skillTreeUpgrade>();
        TextAsset textAsset = Resources.Load<TextAsset>("UpgradesCommon");
        
        if (textAsset != null){
            string[] lines = textAsset.text.Split('\n'); // Split by new line
        
            foreach (string line in lines){
                Debug.Log(line);
                if(line.Equals(lines[0])) continue;
                
                if(line.Trim().Equals("")) break;
                string[] values = line.Split(';');
                // first field is description, second field is type
                
                string description = values[0];
                upgradeType type;
                if (Enum.TryParse(values[1], out upgradeType upgrade)){
                 
                    type = upgrade;
                }
                else{
                    type = upgradeType.Random;
                    //Debug.LogError("Invalid enum value " + values[3] );
                }
                upgrades.Add(new skillTreeUpgrade(description,type));
            }
        }
        else{
            Debug.LogError("text file not found");
        }
        return upgrades;
    }
    public static List<skillTreeUpgrade> CreateSpecialUpgradesFromFile(){
        
        
        
        List<skillTreeUpgrade> upgrades = new List<skillTreeUpgrade>();
        TextAsset textAsset = Resources.Load<TextAsset>("UpgradesSpecial");
        
        if (textAsset != null){
            string[] lines = textAsset.text.Split('\n'); // Split by new line
            
            foreach (string line in lines){
                Debug.Log(line);
                if(line.Equals(lines[0])) continue;
                if(line.Trim().Equals("")) break;
                string[] values = line.Split(';');
                // rarity,price,description,type
                int rarity = 2;
                
                if (int.TryParse(values[0], out int number)){
                    rarity = number;
                }
                int price = 3;
                if (int.TryParse(values[1], out number)){
                    price = number;
                }
                
                string description = values[2];
                
                upgradeType type = upgradeType.Random;
                if (Enum.TryParse(values[3], out upgradeType upgrade)){
                    type = upgrade;
                }
                upgrades.Add(new skillTreeUpgrade(rarity,price,description,type));
            }
        }
        else{
            Debug.LogError("text file not found");
        }
        return upgrades;
    }
    


}