using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

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

    
    public float NodeSize = 1;
    
    public GameObject panel;
    public int seed = 1250;

    public Sprite circleSprite; 
    public Color colorAfterBuy = Color.red;
    public Color ColorBeforeBuy = Color.yellow;

    public TextMeshPro textAttributes;

    
    // solution: each node has a fixed length that his Children can take. the length is determined by most left and most right. 
    
    void Start()
    {
        skillNode.spriteImage = circleSprite;
        skillNode.colorAfterBuy = colorAfterBuy;
        skillNode.colorBeforeBuy = ColorBeforeBuy;
        skillNode.textAttributes = textAttributes;

        if(enableCoolerTrees) minNumOfChildren = Math.Max(minNumOfChildren, 1);
        treeNode root = setTree();
        printTree(root);
        TreeHelpers.CalculateNodePositions(root);
        DrawTree(root);
        //Camera.main.transform.position = new Vector3(root.X*spaceBetweenNodesX  ,(float)root.Y* -spaceBetweenNodesY -12 ,-10 );
        //panel.SetActive(false);

    }
    void Update(){
        if(Input.GetKey(KeyCode.E)){
            panel.SetActive(true);
        }
        if(Input.GetKey(KeyCode.Q)){
            panel.SetActive(false);
        }

    }

    // Update is called once per frame

    treeNode setTree(){
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

    void printTree(treeNode node){
        for(int i=0;i<node.Children.Count;i++){
            Debug.Log("node " + node.value + " has Children: " + +node.Children[i].value);
        }
        for(int i=0;i<node.Children.Count;i++){
            printTree(node.Children[i]);
        }
    }
    public void DrawTree(treeNode root) {
        GameObject nodeObject = new GameObject("node");
        
        Vector2 positionNode =new Vector2(root.X*spaceBetweenNodesX-initialPositionRootX , -spaceBetweenNodesY*(float)root.Y-initialPositionRootY);
        nodeObject.transform.position = positionNode;
        nodeObject.AddComponent<SpriteRenderer>();
 
        root.setUpgrade(new skillTreeUpgrade());
        skillNode SkillNode = nodeObject.AddComponent<skillNode>();
        SkillNode.setTreeNode(root);
        
        
        nodeObject.transform.localScale = new Vector3(NodeSize,NodeSize,NodeSize);
        
        // dont forget to add a specific skillTreeUpgrade to the root/node
        foreach (treeNode child in root.Children) {
            Vector2 positionChild = new Vector2(child.X*spaceBetweenNodesX-initialPositionRootX  , -spaceBetweenNodesY*(float)child.Y-initialPositionRootY);
            CreateEdge(positionChild,positionNode);
            DrawTree(child);
        }
        nodeObject.transform.SetParent(panel.transform);
    }

    private void CreateEdge(Vector2 start, Vector2 end) {
        
        GameObject lineObject = new GameObject("line");

        // Add LineRenderer component
        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

        // Configure LineRenderer for 2D
        lineRenderer.startWidth = 0.25f;  // Line width at the start
        lineRenderer.endWidth = 0.25f;    // Line width at the end
        lineRenderer.useWorldSpace = true; // Use world space coordinates
        lineRenderer.numCapVertices = 2;  // Rounded edges for the line

        // Assign a default 2D material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color of the line
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        // Set positions of the line
        lineRenderer.positionCount = 2; // The line consists of 2 points
        lineRenderer.SetPosition(0, start); // Start point
        lineRenderer.SetPosition(1, end); // End point

        lineObject.transform.SetParent(panel.transform);
    }
    

}

