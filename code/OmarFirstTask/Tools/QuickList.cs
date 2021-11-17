using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    /// <summary>
    /// An auxiliary class to fool the AVL structure.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Comparable<T> : IComparable
    {
        public T Value { get; set; }

        public Comparable(T value)
        {
            this.Value = value;
        }

        public int CompareTo(object obj)
        {
            return -1;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    /// <summary>
    /// An AVL based list, which can performs Insert, RemoveAt and indexing operations in logarithmic time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QuickList<T> : IList<T>
    {
        private AVLTree<Comparable<T>> avl;

        public QuickList()
        {
            avl = new AVLTree<Comparable<T>>();
        }

        public QuickList(IEnumerable<T> collection) : this()
        {
            foreach (var item in collection)
                Add(item);
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();

                return GetAt(index);
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();

                Change(index, value);

            }
        }

        private void Change(int index, T value)
        {
            var node = avl.Select(index);
            var newNode = new AVLTreeNode<Comparable<T>>(new Comparable<T>(value))
            {
                LeftChild = node.LeftChild,
                RightChild = node.RightChild,
                Tree = avl
            };
            if (node.Parent != null)
                // has parent?
            {
                if (node.IsLeftChild)
                    node.Parent.LeftChild = newNode;

                else
                    node.Parent.RightChild = newNode;

            }
            else
            {
                avl.Root = newNode;
            }
        }

        private T GetAt(int index)
        {
            return avl.Select(index).Value.Value;

        }

        public int Count => avl.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (Count == 0)
                avl.Add(new Comparable<T>(item));

            else
                Insert(Count, item);
        }

        public void Clear()
        {
            avl.Clear();
        }

        public bool Contains(T item)
        {
            foreach (var elem in this)
            {
                if (Equals(elem, item))
                    return true;
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var iter = GetEnumerator();
            for (int i = arrayIndex; i < array.Length && iter.MoveNext(); i++)
                array[i] = iter.Current;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var iter = avl.GetInOrderEnumerator();
            while (iter.MoveNext())
            {
                yield return iter.Current.Value;
            }
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();

        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index > Count)
                throw new IndexOutOfRangeException();

            AVLTreeNode<Comparable<T>> newNode = new AVLTreeNode<Comparable<T>>(new Comparable<T>(item))
            {
                Tree = avl
            };
            if (Count == 0)
            {
                avl.Root = newNode;
                return;
            }

            AVLTreeNode<Comparable<T>> node;
            AVLTreeNode<Comparable<T>> succ = null;
            if (index == 0)
                node = avl.Select(0);

            else
            {
                node = avl.Select(index - 1);

                // find the successor which is the father of the node to create
                succ = avl.GetSuccessor(node);
            }
            
            // insert the new value, either as first element or immediate successor of element at index - 1 position
            if (succ == node)
                // node has not a successor in its subtree
                node.RightChild = newNode;

            else
            {
                if (index != 0)
                    node = succ; // now node is its own successor

                node.LeftChild = newNode;
            }

            // getting balanced
            Balance(node);
        }

        private void Balance(AVLTreeNode<Comparable<T>> node)
        {
            var foo = node;
            while (node != null)
            {
                int balance = avl.getBalance(node);
                if (Math.Abs(balance) == 2) //-2 or 2 is unbalanced
                {
                    //Rebalance tree
                    avl.balanceAt(node, balance);
                }

                node = node.Parent; //keep going up
            }

        }

        public bool Remove(T item)
        {
            int i = 0;
            foreach (var elem in this)
            {
                if (Equals(elem, item))
                    break;

                i++;
            }
            if (i == Count)
                return false;

            RemoveAt(i);
            return true;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException();

            avl.Remove(avl.Select(index));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();

        }

        public void PrintTree()
        {
            Console.WriteLine(avl);
        }
    }
}
