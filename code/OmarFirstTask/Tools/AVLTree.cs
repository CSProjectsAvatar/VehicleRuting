//====================================================
//| Downloaded From                                  |
//| Visual C# Kicks - http://www.vcskicks.com/       |
//| License - http://www.vcskicks.com/license.html   |
//====================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace OmarFirstTask
{
    /// <summary>
    /// AVL Tree data structure
    /// </summary>
    public class AVLTree<T> : BinaryTree<T>
        where T : IComparable
    {
        /// <summary>
        /// Returns the AVL Node of the tree
        /// </summary>
        public new AVLTreeNode<T> Root
        {
            get { return (AVLTreeNode<T>)base.Root; }
            set { base.Root = value; }
        }

        /// <summary>
        /// Returns the AVL Node corresponding to the given value
        /// </summary>
        public new AVLTreeNode<T> Find(T value)
        {
            return (AVLTreeNode<T>)base.Find(value);
        }

        /// <summary>
        /// Insert a value in the tree and rebalance the tree if necessary.
        /// </summary>
        public override void Add(T value)
        {
            AVLTreeNode<T> node = new AVLTreeNode<T>(value);

            base.Add(node); //add normally

            //Balance every node going up, starting with the parent
            AVLTreeNode<T> parentNode = node.Parent;

            while (parentNode != null)
            {
                int balance = this.getBalance(parentNode);
                if (Math.Abs(balance) == 2) //-2 or 2 is unbalanced
                {
                    //Rebalance tree
                    this.balanceAt(parentNode, balance);
                }

                parentNode = parentNode.Parent; //keep going up
            }
        }

        /// <summary>
        /// Removes a given value from the tree and rebalances the tree if necessary.
        /// </summary>
        public override bool Remove(T value)
        {
            AVLTreeNode<T> valueNode = this.Find(value);
            return this.Remove(valueNode);
        }

        /// <summary>
        /// Wrapper method for removing a node within the tree
        /// </summary>
        protected internal new bool Remove(BinaryTreeNode<T> removeNode)
        {
            return this.Remove((AVLTreeNode<T>)removeNode);
        }

        /// <summary>
        /// Removes a given node from the tree and rebalances the tree if necessary.
        /// </summary>
        public bool Remove(AVLTreeNode<T> valueNode)
        {
            //Save reference to the parent node to be removed
            AVLTreeNode<T> parentNode = valueNode.Parent;

            //Remove the node as usual
            bool removed = base.Remove(valueNode);

            if (!removed)
                return false; //removing failed, no need to rebalance
            else
            {
                //Balance going up the tree
                while (parentNode != null)
                {
                    int balance = this.getBalance(parentNode);

                    if (Math.Abs(balance) == 1) //1, -1
                        break; //height hasn't changed, can stop
                    else if (Math.Abs(balance) == 2) //2, -2
                    {
                        //Rebalance tree
                        this.balanceAt(parentNode, balance);
                    }

                    parentNode = parentNode.Parent;
                }

                return true;
            }
        }

        /// <summary>
        /// Balances an AVL Tree node
        /// </summary>
        protected internal virtual void balanceAt(AVLTreeNode<T> node, int balance)
        {
            if (balance == 2) //right outweighs
            {
                int rightBalance = getBalance(node.RightChild);

                if (rightBalance == 1 || rightBalance == 0)
                {
                    //Left rotation needed
                    rotateLeft(node);
                }
                else if (rightBalance == -1)
                {
                    //Right rotation needed
                    rotateRight(node.RightChild);

                    //Left rotation needed
                    rotateLeft(node);
                }
            }
            else if (balance == -2) //left outweighs
            {
                int leftBalance = getBalance(node.LeftChild);
                if (leftBalance == 1)
                {
                    //Left rotation needed
                    rotateLeft(node.LeftChild);

                    //Right rotation needed
                    rotateRight(node);
                }
                else if (leftBalance == -1 || leftBalance == 0)
                {
                    //Right rotation needed
                    rotateRight(node);
                }
            }
        }

        /// <summary>
        /// Determines the balance of a given node
        /// </summary>
        protected internal virtual int getBalance(AVLTreeNode<T> root)
        {
            //Balance = right child's height - left child's height
            return GetHeight(root.RightChild) - GetHeight(root.LeftChild);
        }

        private int GetHeight(AVLTreeNode<T> node)
        {
            return (node is null) ? 0 : node.Height;
        }

        /// <summary>
        /// Rotates a node to the left within an AVL Tree
        /// </summary>
        protected virtual void rotateLeft(AVLTreeNode<T> root)
        {
            if (root == null)
                return;

            AVLTreeNode<T> pivot = root.RightChild;

            if (pivot == null)
                return;
            else
            {
                AVLTreeNode<T> rootParent = root.Parent; //original parent of root node
                bool isLeftChild = (rootParent != null) && rootParent.LeftChild == root; //whether the root was the parent's left node
                bool makeTreeRoot = root.Tree.Root == root; //whether the root was the root of the entire tree

                //Rotate
                root.RightChild = pivot.LeftChild;
                pivot.Parent = rootParent; // update parent before setting LeftChild property, 'cause the Count computation
                pivot.LeftChild = root;

                //Update parents
                //root.Parent = pivot;  NOT NECESSARY

                if (root.RightChild != null)
                    root.RightChild.Parent = root;

                //Update the entire tree's Root if necessary
                if (makeTreeRoot)
                    pivot.Tree.Root = pivot;

                //Update the original parent's child node
                if (isLeftChild)
                    rootParent.LeftChild = pivot;
                else
                    if (rootParent != null)
                        rootParent.RightChild = pivot;
            }
        }

        /// <summary>
        /// Rotates a node to the right within an AVL Tree
        /// </summary>
        protected virtual void rotateRight(AVLTreeNode<T> root)
        {
            if (root == null)
                return;

            AVLTreeNode<T> pivot = root.LeftChild;

            if (pivot == null)
                return;
            else
            {
                AVLTreeNode<T> rootParent = root.Parent; //original parent of root node
                bool isLeftChild = (rootParent != null) && rootParent.LeftChild == root; //whether the root was the parent's left node
                bool makeTreeRoot = root.Tree.Root == root; //whether the root was the root of the entire tree

                //Rotate
                root.LeftChild = pivot.RightChild;
                pivot.Parent = rootParent; // update parent before setting RightChild property, 'cause the Count computation
                pivot.RightChild = root;

                //Update parents
                //root.Parent = pivot;      NOT NECESSARY

                if (root.LeftChild != null)
                    root.LeftChild.Parent = root;

                //Update the entire tree's Root if necessary
                if (makeTreeRoot)
                    pivot.Tree.Root = pivot;

                //Update the original parent's child node
                if (isLeftChild)
                    rootParent.LeftChild = pivot;
                else
                    if (rootParent != null)
                        rootParent.RightChild = pivot;
            }
        }

        /// <summary>
        /// Gets the node at given index in the inorder traversal (ordered sequence).
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public AVLTreeNode<T> Select(int idx)
        {
            if (idx < 0 || idx >= this.Count)
                throw new IndexOutOfRangeException("CAN'T SELECT AT THE GIVEN INDEX");

            return Select(idx, this.Root, 0);

        }

        private AVLTreeNode<T> Select(int idx, AVLTreeNode<T> current, int accum)
        {
            if (current.IsLeaf)
                return current;

            // how many nodes are before the current one
            int befCount = accum + ((current.HasLeftChild) ? current.LeftChild.Count : 0);

            if (idx == befCount)
                return current;

            if (idx < befCount)
                return Select(idx, current.LeftChild, accum);

            return Select(idx, current.RightChild, befCount + 1);

        }

        /// <summary>
        /// Determines the number of elements in the tree lower than the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Rank(T value)
        {
            return Rank(value, this.Root);

        }

        private int Rank(T value, AVLTreeNode<T> curr)
        {
            if (curr == null)
                return 0;

            if (CompareElements(value, curr.Value) <= 0)
                // value <= curr.Value
                return Rank(value, curr.LeftChild);

            // elements before curr.Value
            int befCount = (curr.HasLeftChild) ? curr.LeftChild.Count : 0;

            return Rank(value, curr.RightChild) + befCount + 1;
        }

        internal new AVLTreeNode<T> GetSuccessor(BinaryTreeNode<T> node)
        {
            return (AVLTreeNode<T>)base.GetSuccessor(node);

        }
    }
}
