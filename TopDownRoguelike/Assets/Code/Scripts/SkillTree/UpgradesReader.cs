using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpgradesReader{
    public static List<skillTreeUpgrade> readValues(){
        string path = Application.dataPath + "/Code/Scripts/scriptsOlivier/upgradesGenerated.txt";
        List<skillTreeUpgrade> upgrades = new List<skillTreeUpgrade>();
        if (File.Exists(path)){
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines){
                if(line.Equals("")) break;
                string[] values = line.Split(';');
                // first field is value, second field is rarity, thrid is description and last is type
                
                float value = -1;
                if (float.TryParse(values[0], out float number)){
                    //Debug.Log("Parsed successfully: " + number);
                    value = number;
                }
                int rarity = -1;
                if (int.TryParse(values[1], out int num)){
                    //Debug.Log("Parsed successfully: " + number);
                    rarity = num;
                }
                string description = values[2];
                upgradeType type;
                if (Enum.TryParse(values[3], out upgradeType upgrade)){
                    //Debug.Log("Parsed successfully: " + upgrade);
                    type = upgrade;
                }
                else{
                    type = upgradeType.Random;
                    //Debug.LogError("Invalid enum value " + values[3] );
                }
                
                upgrades.Add(new skillTreeUpgrade(rarity,value,description,type));
            }
        }
        else{
            Debug.LogError("File not found: " + path);
        }
        return upgrades;
    }
}