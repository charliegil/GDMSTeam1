using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

    public class treeNode
    {
        public float X { get; set; }
        public int Y { get; set; }
        public float Mod { get; set; }
        public treeNode Parent { get; set; }
        public List<treeNode> Children { get; set; }

        public float Width { get; set; }
        public int Height { get; set; }

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

       
    }

