using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class makeTree : MonoBehaviour
{
    [Header("tree settings")]
    public int minNumOfChildren = 0;
    public int maxNumOfChildren = 3;
    public int totalNodes = 20;
    public int probabilityZeroChildren = 50;
    public bool enableCoolerTrees = true;
    public int seed = 1250;

    [Header("Node spacing settings")]
    private float spaceBetweenNodesX = 3;
    private float spaceBetweenNodesY = 2;

    public float initialPositionRootX = 0;
    public float initialPositionRootY = 0;

    public float lineWidth = 0;


    [Header("UI settings")]
    public float NodeSize = 0.5f;
    public Sprite lineSprite;
    public GameObject panel;
    public GameObject InfoPanel;
    public Color edgeColor;
    public Sprite SpriteLocked;
    public Sprite SpriteUnlocked;

    private List<skillTreeUpgrade> possibleCommonUpgrades;
    private List<skillTreeUpgrade> possibleSpecialUpgrades;



    // solution: each node has a fixed length that his Children can take. the length is determined by most left and most right. 

    private void Start()
    {
        possibleCommonUpgrades = UpgradesReader.CreateCommonUpgradesFromFile();
        possibleSpecialUpgrades = UpgradesReader.CreateSpecialUpgradesFromFile();
        
        skillNode.SpriteLocked = SpriteLocked;
        skillNode.SpriteUnlocked = SpriteUnlocked;
        skillNode.InfoPanel = InfoPanel;

        if(enableCoolerTrees) minNumOfChildren = Math.Max(minNumOfChildren, 1);
        treeNode root = setTree();
        printTree(root);
        TreeHelpers.CalculateNodePositions(root);
        
        // ============ Make sure the tree is fiting in the image  ===========
        spaceBetweenNodesY = 28f / (getHeight(root));
        spaceBetweenNodesX = 45f / (getWidth(root));
        //Debug.Log("the withs is : "+getWidth(root));
        float max =  getMaxWidth(root,true)-root.X;
        float min = root.X - getMaxWidth(root,false);
        float offset = max-min;
        initialPositionRootX+=offset*spaceBetweenNodesX/2;
        // ============= end layout positioning ==========================


        //Debug.Log("root pos:"  +root.X + " " + root.Y);
        DrawTree(root,0,root);
        // -60

    }

    private skillTreeUpgrade getRandomUpgrade(int rarity){
       if(UnityEngine.Random.Range(0,1)==0){
            skillTreeUpgrade upgrade = getRandomUpgrade(rarity,false);
            if(upgrade!=null) return upgrade;
       }
        return getRandomUpgrade(rarity,true);
    }
    
    private skillTreeUpgrade getRandomUpgrade(int rarity,bool common){
        rarity = Math.Clamp(rarity,1, 5);
        List<skillTreeUpgrade> lst = possibleSpecialUpgrades;
        if(common) lst = possibleCommonUpgrades;  
        
        List<skillTreeUpgrade> list = new List<skillTreeUpgrade>(possibleSpecialUpgrades);
        if(common) list = new List<skillTreeUpgrade>(possibleCommonUpgrades);
        
        if(lst.Count == 0){
            if(common) return new skillTreeUpgrade(rarity);
            return null;
        }
        
        int index = UnityEngine.Random.Range(0,list.Count);
        skillTreeUpgrade current = list[index];
        
        
        while (list.Count > 0){
            index = UnityEngine.Random.Range(0, list.Count);
            current = list[index];
            if (Math.Abs(current.getRarity() - rarity) <= 1) break;
        
            list.RemoveAt(index);

        }
        lst.Remove(current);
        return current;
    }
    



    // Update is called once per frame

    private treeNode setTree(){
        Queue<treeNode> queue = new Queue<treeNode>();
        
        System.Random random = new System.Random(seed);
        if(seed==-1){
            random = new System.Random();
        }
        
        treeNode root = new treeNode(null);
        root.value = -1;
        queue.Enqueue(root);
        int counter  =0;
        int current = totalNodes;

      
        while(current != 0){
            
          
            
            int numChild = 0;
            if(!enableCoolerTrees || random.Next(0,101) > probabilityZeroChildren || queue.Count == 1){
                int minChild = (queue.Count == 1 && minNumOfChildren == 0) ? 1 : minNumOfChildren;
                
                numChild = random.Next(minChild, Math.Min(maxNumOfChildren, current) + 1);
            }

           
            treeNode node = queue.Dequeue();
           
            
            for(int i=0; i<numChild;i++){
                treeNode child = new treeNode(node);
                node.Children.Add(child);
                child.value = counter;
                
                counter++;
                queue.Enqueue(child);
                
            }
                current-= numChild;
                counter++;
                
        }
        return root;
    }

    private void printTree(treeNode node){
        for(int i=0;i<node.Children.Count;i++){
           // Debug.Log("node " + node.value + " has Children: " + +node.Children[i].value);
        }
        for(int i=0;i<node.Children.Count;i++){
            printTree(node.Children[i]);
        }
    }
    public void DrawTree(treeNode node, int depth,treeNode root) {
        GameObject nodeObject = new GameObject("node");
        
        Vector2 positionNode =new Vector2(-spaceBetweenNodesX*(float)(node.X - root.X), -spaceBetweenNodesY*(float)(node.Y-root.Y))+ new Vector2(initialPositionRootX, initialPositionRootY);

        
        nodeObject.AddComponent<Image>();
        //nodeObject.GetComponent<Image>().sprite = SpriteLocked;
        node.setUpgrade(getRandomUpgrade(depth));

        skillNode SkillNode = nodeObject.AddComponent<skillNode>();
        SkillNode.setTreeNode(node);
        
        
        RectTransform rectTransform = nodeObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.sizeDelta = new Vector2(NodeSize, NodeSize);  
        //nodeObject.transform.localScale = new Vector2(NodeSize, NodeSize);  
        rectTransform.anchoredPosition = positionNode;
    
        // dont forget to add a specific skillTreeUpgrade to the root/node
        foreach (treeNode child in node.Children) {
            Vector2 positionChild = new Vector2(-spaceBetweenNodesX*(float)(child.X- root.X) , -spaceBetweenNodesY*(float)(child.Y-root.Y)) + new Vector2(initialPositionRootX, initialPositionRootY);
            CreateEdge(positionNode,positionChild);
            DrawTree(child,depth+1,root);
        }
        nodeObject.transform.SetParent(panel.transform,true);
    }


    private void CreateEdge(Vector2 start, Vector2 end) {
        
        GameObject lineObject = new GameObject("line");

       
        Image image = lineObject.AddComponent<Image>();
        image.color = edgeColor;

        float distance = Vector2.Distance(start, end);
        float angle = Vector2.SignedAngle((end-start).normalized, new Vector2(1, 0));
        
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        
        rectTransform.anchoredPosition = (start + end) / 2;
        
        rectTransform.rotation = Quaternion.Euler(0, 0, -angle);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        
        
        image.transform.localScale = new Vector2(distance/100, lineWidth);
        image.sprite = lineSprite;
        
       
        image.material = new Material(Shader.Find("Sprites/Default"));
        lineObject.transform.SetParent(panel.transform,true);
       
    }
    private float getHeight(treeNode node){
        float max = 0;
        
        foreach (treeNode child in node.Children){
            max = Mathf.Max(max, getHeight(child));
        }
        return 1+max;
    }
    private float getWidth(treeNode node){
        return getMaxWidth(node,true) - getMaxWidth(node,false);
    }
    private float getMaxWidth(treeNode node, bool maximum){
        float max = node.X;
        foreach (treeNode child in node.Children){
            if (maximum) max = MathF.Max(max, getMaxWidth(child,maximum));
            else max = MathF.Min(max, getMaxWidth(child,maximum));
        }
        return max;
    }
    
    
    


    

}

