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
    [HideInInspector] public Sprite iconSprite;

    private SpriteRenderer spriteRenderer;

    public static GameObject InfoPanel;

    private static TextMeshProUGUI effectText;
    private static TextMeshProUGUI priceText;
    private static TextMeshProUGUI valueText;
    private static TextMeshProUGUI flavorText;


    

    private void Awake()
    {
    
        Image spriteRenderer = gameObject.GetComponent<Image>();
        if (spriteRenderer == null) spriteRenderer = gameObject.AddComponent<Image>();
        spriteRenderer.sprite = SpriteLocked;
      

        if(effectText == null) effectText = InfoPanel.transform.Find("Desc").GetComponent<TextMeshProUGUI>();
        if(priceText == null) priceText = InfoPanel.transform.Find("Price").GetComponent<TextMeshProUGUI>();
        if(valueText == null) valueText = InfoPanel.transform.Find("Value").GetComponent<TextMeshProUGUI>();
        if(flavorText == null) flavorText = InfoPanel.transform.Find("FlavorText").GetComponent<TextMeshProUGUI>();

       

        
    }
  



    public void setTreeNode(treeNode tree){
        node = tree;
    }


    // this is the method and script responsible to change the appearance of the gameObject
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

        effectText.text = split[4];
        priceText.text = "Price: " + split[1];
        valueText.text = "Boost: " + split[2];
        
        flavorText.text = split[0];
        //return description + ";" + price + ";" + value+";"+rarity; // to string method in skilltreeUpgrade
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        effectText.text = "Hover to see attributes";
        priceText.text = "Price:";
        valueText.text = "Boost:";
        flavorText.text = "";
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        /*if (eventData.button == PointerEventData.InputButton.Right){
            EventManager.SellSkill(this);
        }*/
        EventManager.BuySkill(this);
    }
    
    

}
