using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class makeTree : MonoBehaviour
{
    public int minNumOfChildren = 0;
    public int maxNumOfChildren = 3;
    public int totalNodes = 20;

    public int probabilityZeroChildren = 50;
    public bool enableCoolerTrees = true;

    public float spaceBetweenNodesX = 3;
    public float spaceBetweenNodesY= 2;

    public float initialPositionRootX = 0;
    public float initialPositionRootY = 0;

    public float lineWidth = 0;

    public Sprite lineSprite;

    
    public float NodeSize = 0.5f;
    
    public GameObject panel;
    public int seed = 1250;

    public Sprite circleSprite; 
    public Sprite SpriteLocked;
    public Sprite SpriteUnlocked;

    public GameObject textAttributes;

    private List<skillTreeUpgrade> possibleUpgrades;


    // solution: each node has a fixed length that his Children can take. the length is determined by most left and most right. 

    private void Start()
    {
        possibleUpgrades = UpgradesReader.readValues();
        foreach (skillTreeUpgrade upgrade in possibleUpgrades ){
            Debug.Log(upgrade.ToString());
            
        }
        skillNode.SpriteLocked = SpriteLocked;
        skillNode.SpriteUnlocked = SpriteUnlocked;
        skillNode.textAttributes = textAttributes;

        if(enableCoolerTrees) minNumOfChildren = Math.Max(minNumOfChildren, 1);
        treeNode root = setTree();
        printTree(root);
        TreeHelpers.CalculateNodePositions(root);
        DrawTree(root,0);

    }

    private skillTreeUpgrade getRandomUpgrade(int rarity){
       
        rarity = Math.Clamp(rarity,1, 5);
        
        if(possibleUpgrades.Count == 0) return new skillTreeUpgrade(rarity);
        
        List<skillTreeUpgrade> list = new List<skillTreeUpgrade>(possibleUpgrades);
        
        int index = UnityEngine.Random.Range(0,list.Count);
        skillTreeUpgrade current = list[index];
        
        
        while (list.Count > 0){
            index = UnityEngine.Random.Range(0, list.Count);
            current = list[index];
            if (Math.Abs(current.getRarity() - rarity) <= 1) break;
        
            list.RemoveAt(index);
        }
        possibleUpgrades.Remove(current);
        return current;
    }


    // Update is called once per frame

    private treeNode setTree(){
        Queue<treeNode> queue = new Queue<treeNode>();
        
        System.Random random = new System.Random(seed);
        
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
    public void DrawTree(treeNode root, int depth) {
        GameObject nodeObject = new GameObject("node");
        
        Vector2 positionNode =new Vector2(-spaceBetweenNodesX*(float)root.X, -spaceBetweenNodesY*(float)root.Y)+ new Vector2(initialPositionRootX, initialPositionRootY);
        
        nodeObject.AddComponent<Image>();
        //nodeObject.GetComponent<Image>().sprite = SpriteLocked;
        root.setUpgrade(getRandomUpgrade(depth));

        skillNode SkillNode = nodeObject.AddComponent<skillNode>();
        SkillNode.setTreeNode(root);
        
        
        RectTransform rectTransform = nodeObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.sizeDelta = new Vector2(NodeSize, NodeSize);  
        //nodeObject.transform.localScale = new Vector2(NodeSize, NodeSize);  
        rectTransform.anchoredPosition = positionNode;
    
        // dont forget to add a specific skillTreeUpgrade to the root/node
        foreach (treeNode child in root.Children) {
            Vector2 positionChild = new Vector2(-spaceBetweenNodesX*(float)child.X , -spaceBetweenNodesY*(float)child.Y) + new Vector2(initialPositionRootX, initialPositionRootY);
            CreateEdge(positionNode,positionChild);
            DrawTree(child,depth+1);
        }
        nodeObject.transform.SetParent(panel.transform,true);
    }


    private void CreateEdge(Vector2 start, Vector2 end) {
        
        GameObject lineObject = new GameObject("line");

       
        Image lineRenderer = lineObject.AddComponent<Image>();

        float distance = Vector2.Distance(start, end);
        float angle = Vector2.SignedAngle((end-start).normalized, new Vector2(1, 0));
        
        RectTransform rectTransform = lineRenderer.GetComponent<RectTransform>();
        
        rectTransform.anchoredPosition = (start + end) / 2;
        
        rectTransform.rotation = Quaternion.Euler(0, 0, -angle);
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        
        
        lineRenderer.transform.localScale = new Vector2(distance/100, lineWidth);
        lineRenderer.sprite = lineSprite;
        
       
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineObject.transform.SetParent(panel.transform,true);
       
    }
    private void CreateEdge2(Vector2 start, Vector2 end) {
    
    GameObject lineObject = new GameObject("Line");

    
    SpriteRenderer lineRenderer = lineObject.AddComponent<SpriteRenderer>();
    lineRenderer.sprite = lineSprite;

    
    Vector2 midpoint = (start + end) / 2;
    float distance = Vector2.Distance(start, end);
    float angle = Vector2.SignedAngle((end - start).normalized, Vector2.right);

   
    lineObject.transform.position = start;  // Set world-space position
    lineObject.transform.rotation = Quaternion.Euler(0, 0, angle); // Rotate correctly
    lineObject.transform.localScale = new Vector3(1, 1, 1); // Scale properly

   
    lineObject.transform.parent = panel.transform; 
}

    

}

