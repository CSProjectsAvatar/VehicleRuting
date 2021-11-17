using System;

namespace OmarFirstTask
{
    /// <summary>
    /// A Binary Tree node that holds an element and references to other tree nodes
    /// </summary>
    public class BinaryTreeNode<T>
        where T : IComparable
    {
        private T value;
        private BinaryTreeNode<T> leftChild;
        private BinaryTreeNode<T> rightChild;
        private BinaryTreeNode<T> parent;
        private BinaryTree<T> tree;
        private int size;
        private int height;

        /// <summary>
        /// The value stored at the node
        /// </summary>
        public virtual T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Gets or sets the left child node
        /// </summary>
        public virtual BinaryTreeNode<T> LeftChild
        {
            get { return leftChild; }
            set
            {
                leftChild = value;
                if (leftChild != null)
                {
                    leftChild.Parent = this;
                    //leftChild.Tree = this.Tree;
                }

                //UpdateCount();
                UpdateHeightAndCount();
            }
        }

        /// <summary>
        /// Gets or sets the right child node
        /// </summary>
        public virtual BinaryTreeNode<T> RightChild
        {
            get { return rightChild; }
            set
            {
                rightChild = value;
                if (rightChild != null)
                {
                    rightChild.Parent = this;
                    //rightChild.Tree = this.Tree;
                }

                //UpdateCount();
                UpdateHeightAndCount();
            }
        }

        private void UpdateHeightAndCount()
        {
            // updating count
            UpdateCount();

            // updating height
            int lftHeight = (this.LeftChild is null) ? -1 : this.LeftChild.Height;
            int rgtHeight = (this.RightChild is null) ? -1 : this.RightChild.Height;
            this.Height = Math.Max(lftHeight, rgtHeight) + 1;
        }

        private void UpdateCount()
        {
            int lftCount = (this.LeftChild is null) ? 0 : this.LeftChild.Count;
            int rgtCount = (this.RightChild is null) ? 0 : this.RightChild.Count;
            this.Count = lftCount + rgtCount + 1;
        }

        /// <summary>
        /// Gets or sets the parent node
        /// </summary>
        public virtual BinaryTreeNode<T> Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                //if (parent != null)
                //    parent.Tree = this.Tree;
            }
        }

        /// <summary>
        /// Gets or sets the Binary Tree the node belongs to
        /// </summary>
        public virtual BinaryTree<T> Tree
        {
            get { return tree; }
            set { tree = value; }
        }

        /// <summary>
        /// Gets whether the node is a leaf (has no children)
        /// </summary>
        public virtual bool IsLeaf
        {
            get { return this.ChildCount == 0; }
        }

        /// <summary>
        /// Gets whether the node is internal (has children nodes)
        /// </summary>
        public virtual bool IsInternal
        {
            get { return this.ChildCount > 0; }
        }

        /// <summary>
        /// Gets whether the node is the left child of its parent
        /// </summary>
        public virtual bool IsLeftChild
        {
            get { return this.Parent != null && this.Parent.LeftChild == this; }
        }

        /// <summary>
        /// Gets whether the node is the right child of its parent
        /// </summary>
        public virtual bool IsRightChild
        {
            get { return this.Parent != null && this.Parent.RightChild == this; }
        }

        /// <summary>
        /// Gets the number of children this node has
        /// </summary>
        public virtual int ChildCount
        {
            get
            {
                int count = 0;

                if (this.LeftChild != null)
                    count++;

                if (this.RightChild != null)
                    count++;

                return count;
            }
        }

        /// <summary>
        /// Gets whether the node has a left child node
        /// </summary>
        public virtual bool HasLeftChild
        {
            get { return (this.LeftChild != null); }
        }

        /// <summary>
        /// Gets whether the node has a right child node
        /// </summary>
        public virtual bool HasRightChild
        {
            get { return (this.RightChild != null); }
        }

        /// <summary>
        /// Create a new instance of a Binary Tree node
        /// </summary>
        public BinaryTreeNode(T value) : this(value, null, null)
        {
        }

        /// <summary>
        /// Create a new instance of a Binary Tree node
        /// </summary>
        public BinaryTreeNode(T value, BinaryTreeNode<T> left, BinaryTreeNode<T> right)
        {
            this.value = value;
            this.LeftChild = left;
            this.RightChild = right;
            this.Height = 0;
            //this.Count = 1;
        }

        /// <summary>
        /// Gets the number of elements stored in the subtree this node is root of.
        /// </summary>
        public virtual int Count
        {
            get => size;
            internal set
            {
                size = value;
                //Parent?.UpdateCount();
            }
        }

        /// <summary>
        /// Returns the height of the subtree rooted at the parameter node
        /// </summary>
        public int Height
        {
            get => height;

            private set
            {
                height = value;
                Parent?.UpdateHeightAndCount();
            }
        }

        public override string ToString()
        {
            return string.Format("|- ({0}, C = {1}, H = {2})\n", this.Value, this.Count, this.Height);

        }
    }
}