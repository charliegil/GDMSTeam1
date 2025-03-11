using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SkillTreeHandler : MonoBehaviour
{
    public int skillPoints = 100;
    private List<skillTreeUpgrade> upgradesOwned = new List<skillTreeUpgrade>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        
    }

    // will call the method buy or sell depending on mouse Input and inside these method, it will be responsible to add the upgrade to the list,
    // update number of skill points, and sending updates to other scripts to apply these upgrades. will do these action if the skillpoint returned


    // this is the method responsible to change the appearance of the gameObject.


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            HandleSkillPurchase();

        }
        else if (Input.GetMouseButtonDown(1)){
            HandleSkillSell();
        }

    }
    private void HandleSkillPurchase(){
        skillNode skillnode = getNodeClickedUI();
            if (skillnode == null) return;
            int original  = skillPoints;
            skillPoints = skillnode.buySkill(skillPoints);
            
            if(original != skillPoints){
                upgradesOwned.Add(skillnode.getNode().GetUpgrade());
                GetComponent<AudioSource>().Play();
                Debug.Log("you bought" + skillnode);
            }
    }

    private void HandleSkillSell(){
        skillNode skillnode = getNodeClickedUI();
            if (skillnode == null) return;
            int original  = skillPoints;
            skillPoints = skillnode.sellSkill(skillPoints);
            
            if(original != skillPoints){ // the upgrade was already purchased
                upgradesOwned.Remove(skillnode.getNode().GetUpgrade());
                Debug.Log("you sold" + skillnode.getNode());
            }
    }

    private skillNode getNodeCLicked(){
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("the position: " + Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        
        if (hit.collider == null) return null;

        Debug.Log("this is the name: 2" + hit.collider.gameObject.name+ "2");
        if(hit.collider.gameObject.name == "node") return hit.collider.gameObject.GetComponent<skillNode>();
            
        return null;
       
    }
    private skillNode getNodeClickedUI()
{
    PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
    {
        position = Input.mousePosition
    };

    RaycastResult raycastResult = new RaycastResult();
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(pointerEventData, results);

    foreach (RaycastResult result in results)
    {
        if (result.gameObject.name == "node")
        {
            return result.gameObject.GetComponent<skillNode>();
        }
    }

    return null;
}


}
