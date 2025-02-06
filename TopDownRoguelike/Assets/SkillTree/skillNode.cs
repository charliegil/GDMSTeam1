using UnityEngine;

public class skillNode : MonoBehaviour
{
    // this is just a wrapper class to be able to hold a treeNode object inside a gameobject

    // 
    
    private treeNode node;
    
    public static Sprite spriteImage;

   

    private SpriteRenderer spriteRenderer;

    public static Color colorAfterBuy; // color that is shown when you bought the upgrade
    public static Color colorBeforeBuy; // color that is shown when can buy the upgrade
    
    void Awake()
    {
       
      
    
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteImage;
        spriteRenderer.color = colorBeforeBuy;


        gameObject.AddComponent<CircleCollider2D>();
    }
    public void setTreeNode(treeNode tree){
        node = tree;
    }

    // is the method and script responsible to change the appearance of the gameObject
    public int buySkill(int skillPoints){
        int points = node.buyUpgrade(skillPoints);
        Debug.Log("is node null" + (node ==null));
        Debug.Log("is renderer null" + (spriteRenderer ==null));
        Debug.Log("is colorAfterBuy null" + (colorAfterBuy ==null));

        if (skillPoints != points ) gameObject.GetComponent<SpriteRenderer>().color = colorAfterBuy;
        return points;
    }
    public int sellSkill(int skillPoints){
        //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        int points = node.sellUpgrade(skillPoints); 
        Debug.Log("is node null" + (node ==null));
        Debug.Log("is renderer null" + (spriteRenderer ==null));
        Debug.Log("is colorBeforeBuy null" + (colorBeforeBuy ==null));
        
        if (skillPoints != points ) gameObject.GetComponent<SpriteRenderer>().color = colorBeforeBuy;
        return points;
    }
    public override string ToString(){
        return node.upgrade.ToString();
    }
    public treeNode getNode(){
        return node;
    }
}
