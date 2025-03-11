using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class makeTree : MonoBehaviour
{
    public int minNumOfChildren = 0;
    public int maxNumOfChildren = 3;
    public int totalNodes = 20;

    public int probabilityZeroChildren = 50;
    public bool enableCoolerTrees = true;

    public int spaceBetweenNodesX = 3;
    public int spaceBetweenNodesY= 2;

    public int initialPositionRootX = 0;
    public int initialPositionRootY = 0;

    public int lineWidth = 0;

    public Sprite lineSprite;

    
    public float NodeSize = 0.5f;
    
    public GameObject panel;
    public int seed = 1250;

    public Sprite circleSprite; 
    public Sprite SpriteLocked;
    public Sprite SpriteUnlocked;

    public GameObject textAttributes;


    // solution: each node has a fixed length that his Children can take. the length is determined by most left and most right. 

    private void Start()
    {
        skillNode.SpriteLocked = SpriteLocked;
        skillNode.SpriteUnlocked = SpriteUnlocked;
        skillNode.textAttributes = textAttributes;

        if(enableCoolerTrees) minNumOfChildren = Math.Max(minNumOfChildren, 1);
        treeNode root = setTree();
        printTree(root);
        TreeHelpers.CalculateNodePositions(root);
        DrawTree(root);
        //Camera.main.transform.position = new Vector3(root.X*spaceBetweenNodesX  ,(float)root.Y* -spaceBetweenNodesY -12 ,-10 );
        //panel.SetActive(false);

    }
    private void Update(){
        /*if(Input.GetKey(KeyCode.E)){
            panel.SetActive(true);
        }
        if(Input.GetKey(KeyCode.Q)){
            panel.SetActive(false);
        }*/

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
    public void DrawTree(treeNode root) {
        GameObject nodeObject = new GameObject("node");
        
        Vector2 positionNode =new Vector2(-spaceBetweenNodesX*(float)root.X, -spaceBetweenNodesY*(float)root.Y)+ new Vector2(initialPositionRootX, initialPositionRootY);
        
        nodeObject.AddComponent<Image>();
        //nodeObject.GetComponent<Image>().sprite = SpriteLocked;
        root.setUpgrade(new skillTreeUpgrade());
        skillNode SkillNode = nodeObject.AddComponent<skillNode>();
        SkillNode.setTreeNode(root);
        
        
        RectTransform rectTransform = nodeObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(NodeSize, NodeSize);  // Example size: 200x200
        rectTransform.anchoredPosition = positionNode;
    
        // dont forget to add a specific skillTreeUpgrade to the root/node
        foreach (treeNode child in root.Children) {
            Vector2 positionChild = new Vector2(-spaceBetweenNodesX*(float)child.X , -spaceBetweenNodesY*(float)child.Y) + new Vector2(initialPositionRootX, initialPositionRootY);
            CreateEdge(positionNode,positionChild);
            DrawTree(child);
        }
        nodeObject.transform.SetParent(panel.transform,true);
    }

    private void CreateEdge(Vector2 start, Vector2 end) {
        
        GameObject lineObject = new GameObject("line");

        // Add LineRenderer component
        Image lineRenderer = lineObject.AddComponent<Image>();

        float distance = Vector2.Distance(start, end);
        float angle = Vector2.SignedAngle((end-start).normalized, new Vector2(1, 0));
        
        RectTransform rectTransform = lineRenderer.GetComponent<RectTransform>();
        
        rectTransform.anchoredPosition = (start + end) / 2;
        //rectTransform.anchoredPosition = end;
        rectTransform.rotation = Quaternion.Euler(0, 0, -angle);
        
        rectTransform.sizeDelta = new Vector2(distance, lineWidth);
        
        lineRenderer.sprite = lineSprite;
        
        // Configure LineRenderer for 2D
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineObject.transform.SetParent(panel.transform,true);
        //rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }
    private void CreateEdge2(Vector2 start, Vector2 end) {
    
    GameObject lineObject = new GameObject("Line");

    // Add SpriteRenderer instead of Image
    SpriteRenderer lineRenderer = lineObject.AddComponent<SpriteRenderer>();
    lineRenderer.sprite = lineSprite;

    // Calculate position, angle, and scale
    Vector2 midpoint = (start + end) / 2;
    float distance = Vector2.Distance(start, end);
    float angle = Vector2.SignedAngle((end - start).normalized, Vector2.right);

    // Apply transformations
    lineObject.transform.position = start;  // Set world-space position
    lineObject.transform.rotation = Quaternion.Euler(0, 0, angle); // Rotate correctly
    lineObject.transform.localScale = new Vector3(1, 1, 1); // Scale properly

    // Set parent (optional)
    lineObject.transform.parent = panel.transform; 
}

    

}

