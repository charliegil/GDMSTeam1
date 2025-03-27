using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class skillNode : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler,  IPointerClickHandler
{
    // this is just a wrapper class to be able to hold a treeNode object inside a gameobject
    private treeNode node;
    
    public static Sprite SpriteLocked;
    public static Sprite SpriteUnlocked;

    private SpriteRenderer spriteRenderer;

    public static GameObject InfoPanel;

    private TextMeshProUGUI descriptionText;

    private TextMeshProUGUI priceText;

    private TextMeshProUGUI valueText;

    

    private void Awake()
    {
    
        Image spriteRenderer = gameObject.GetComponent<Image>();
        if (spriteRenderer == null) spriteRenderer = gameObject.AddComponent<Image>();
        spriteRenderer.sprite = SpriteLocked;

        descriptionText = InfoPanel.transform.Find("Desc").GetComponent<TextMeshProUGUI>();
        priceText = InfoPanel.transform.Find("Price").GetComponent<TextMeshProUGUI>();
        valueText = InfoPanel.transform.Find("Value").GetComponent<TextMeshProUGUI>();
        
    }
    public void setTreeNode(treeNode tree){
        node = tree;
    }

    // is the method and script responsible to change the appearance of the gameObject
    public int buySkill(int skillPoints){
        int points = node.buyUpgrade(skillPoints);
        //Debug.Log("is node null" + (node ==null));
        //Debug.Log("is renderer null" + (spriteRenderer ==null));
       

        if (skillPoints != points ) gameObject.GetComponent<Image>().sprite = SpriteUnlocked;
        return points;
    }
    public int sellSkill(int skillPoints){
        //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        int points = node.sellUpgrade(skillPoints); 
        Debug.Log("is node null" + (node ==null));
        Debug.Log("is renderer null" + (spriteRenderer ==null));
        
        
        if (skillPoints != points ) gameObject.GetComponent<Image>().sprite = SpriteLocked;
        return points;
    }
    public override string ToString(){
        return node.GetUpgrade().ToString();
    }
    public treeNode getNode(){
        return node;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("on mouse over");
        string upgradeDesc = node.GetUpgrade().ToString();
        string[] split = upgradeDesc.Split(';');   

        descriptionText.text = split[0];
        priceText.text = "Price: " + split[1];
        valueText.text = "Boost: " + split[2];
        //return description + ";" + price + ";" + value+";"+rarity; // to string method in skilltreeUpgrade
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
         

        descriptionText.text = "Hover to see attributes";
        priceText.text = "Price:";
        valueText.text = "Boost:";
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        /*if (eventData.button == PointerEventData.InputButton.Right){
            EventManager.SellSkill(this);
        }*/
        EventManager.BuySkill(this);
    }
    

}
