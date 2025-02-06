using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

    public class treeNode
    {
        public float X { get; set; }
        public int Y { get; set; }
        public float Mod { get; set; }
        public treeNode Parent { get; set; }
        public List<treeNode> Children { get; set; }

        public float Width { get; set; }
        public int Height { get; set; }


        public skillTreeUpgrade upgrade;
        public int value;

        public treeNode( treeNode parent)
        {
            
            this.Parent = parent;
            this.Children = new List<treeNode>();
        }

        public bool IsLeaf()
        {
            return this.Children.Count == 0;
        }

        public bool IsLeftMost()
        {
            if (this.Parent == null)
                return true;

            return this.Parent.Children[0] == this;
        }

        public bool IsRightMost()
        {
            if (this.Parent == null)
                return true;

            return this.Parent.Children[this.Parent.Children.Count - 1] == this;
        }

        public treeNode GetPreviousSibling()
        {
            if (this.Parent == null || this.IsLeftMost())
                return null;

            return this.Parent.Children[this.Parent.Children.IndexOf(this) - 1];
        }

        public treeNode GetNextSibling()
        {
            if (this.Parent == null || this.IsRightMost())
                return null;

            return this.Parent.Children[this.Parent.Children.IndexOf(this) + 1];
        }

        public treeNode GetLeftMostSibling()
        {
            if (this.Parent == null)
                return null;

            if (this.IsLeftMost())
                return this;

            return this.Parent.Children[0];
        }

        public treeNode GetLeftMostChild()
        {
            if (this.Children.Count == 0)
                return null;

            return this.Children[0];
        }

        public treeNode GetRightMostChild()
        {
            if (this.Children.Count == 0)
                return null;

            return this.Children[Children.Count - 1];
        }

        public int buyUpgrade( int skillPoints){
            // if this node is the root or his parent ability has been bought, is eligible for buying this one
            if ( Parent == null || !Parent.canBeBought()) return upgrade.doUpgrade(skillPoints); 
            return skillPoints;
            

        }
        public int sellUpgrade(int skillPoints){
            if ( canBeRefund() ) return upgrade.undoUpgrade(skillPoints); 
                return skillPoints;
            // will see if the parent has its upgrade bought. If so, will call undoUpgrade from the skillTreeUpgrade and will return that value
            // will also change the appearance of the gameObject
            // to know if need to change the apppearance, check if the method undoUpgrade returned same amount as skillPoints
        }
        public bool canBeRefund(){ // problem with the way the logic behind if you can buy something or not
            for(int i = 0; i< Children.Count; i++){
                if (Children[i].canBeBought() == false) return false;
            }
            return true;
        }
        public bool canBeBought(){
            return !upgrade.Isbought();
        }
       
    }

