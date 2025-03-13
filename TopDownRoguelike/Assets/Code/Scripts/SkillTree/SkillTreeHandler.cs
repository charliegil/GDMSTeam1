using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SkillTreeHandler : MonoBehaviour , IEventListener
{
    public int skillPoints = 100;
    private List<skillTreeUpgrade> upgradesOwned = new List<skillTreeUpgrade>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
        subscribe();
    }

    // will call the method buy or sell depending on mouse Input and inside these method, it will be responsible to add the upgrade to the list,
    // update number of skill points, and sending updates to other scripts to apply these upgrades. will do these action if the skillpoint returned


    // this is the method responsible to change the appearance of the gameObject.


    // Update is called once per frame
    
    private void HandleSkillPurchase(skillNode skillnode){
        
        if (skillnode == null) return;
        int original  = skillPoints;
        skillPoints = skillnode.buySkill(skillPoints);
            
        if(original != skillPoints){
            upgradesOwned.Add(skillnode.getNode().GetUpgrade());
            GetComponent<AudioSource>().Play();
            Debug.Log("you bought" + skillnode);
            
        }
    }

    private void HandleSkillSell(skillNode skillnode){
        
        if (skillnode == null) return;
        int original  = skillPoints;
        skillPoints = skillnode.sellSkill(skillPoints);
            
        if(original != skillPoints){ // the upgrade was already purchased
            upgradesOwned.Remove(skillnode.getNode().GetUpgrade());
            Debug.Log("you sold" + skillnode.getNode());
        }
    }

    public void subscribe()
    {
        EventManager.OnBuySkill += HandleSkillPurchase;
        EventManager.OnSellSkill += HandleSkillSell;
    }

    public void unsubscribe()
    {
        EventManager.OnBuySkill -= HandleSkillPurchase;
        EventManager.OnSellSkill -= HandleSkillSell;
    }

}
