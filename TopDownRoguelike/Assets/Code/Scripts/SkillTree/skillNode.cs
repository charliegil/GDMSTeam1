using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class skillNode : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    // this is just a wrapper class to be able to hold a treeNode object inside a gameobject
    private treeNode node;
    
    public static Sprite SpriteLocked;
    public static Sprite SpriteUnlocked;

    private SpriteRenderer spriteRenderer;

    public static GameObject textAttributes;

    

    private void Awake()
    {
    
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = SpriteLocked;

        
       


        
    }
    public void setTreeNode(treeNode tree){
        node = tree;
    }

    // is the method and script responsible to change the appearance of the gameObject
    public int buySkill(int skillPoints){
        int points = node.buyUpgrade(skillPoints);
        Debug.Log("is node null" + (node ==null));
        Debug.Log("is renderer null" + (spriteRenderer ==null));
       

        if (skillPoints != points ) gameObject.GetComponent<SpriteRenderer>().sprite = SpriteUnlocked;
        return points;
    }
    public int sellSkill(int skillPoints){
        //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        int points = node.sellUpgrade(skillPoints); 
        Debug.Log("is node null" + (node ==null));
        Debug.Log("is renderer null" + (spriteRenderer ==null));
        
        
        if (skillPoints != points ) gameObject.GetComponent<SpriteRenderer>().sprite = SpriteLocked;
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
        Debug.Log("on mouse over");

        textAttributes.GetComponent<TextMeshProUGUI>().text = node.GetUpgrade().ToString();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(textAttributes == null ? "textAttributes is NULL!" : "textAttributes exists: " + textAttributes.name);
        Debug.Log(textAttributes?.GetComponent<TextMeshProUGUI>() == null ? "TextMeshPro component is MISSING!" : "TextMeshPro found.");
         Debug.Log("on mouse dxit");

        textAttributes.GetComponent<TextMeshProUGUI>().text = "Hover to see attributes";
    }
    

}
