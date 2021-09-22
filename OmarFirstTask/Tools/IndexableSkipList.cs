using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public class IndexableSkipList<T> : IList<T>
    {
        private double p;
        private int maxLevel => head.Next.Count;
        private IdxSkipListNode<T> head;
        private Random rand;

        public IndexableSkipList(ICollection<T> collection, double p)
        {
            this.p = p;
            if (collection != null)
                foreach (var item in collection)
                    Add(item);

            head = new IdxSkipListNode<T>();
            head.Next.Add(null);
            head.Width.Add(0);
            this.Count = 0;
            this.rand = new Random(Environment.TickCount);
        }

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IndexableSkipList(double p = 0.5) : this(null, p)
        {
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int idx, T x)
        {
            Tuple<IdxSkipListNode<T>, int>[] marks = Mark(idx);
            int l = GetLevel();
            IdxSkipListNode<T> node = new IdxSkipListNode<T>(x);

            for (int i = 0; i < l; i++)
            {
                var pred = head; // predecesor del nodo en el nivel i-ésimo
                int predIdx = -1; 

                if (i < maxLevel)/* si no me he pasado del mayor nivel entonces actualizo referencias al sigte */
                {
                    pred = marks[i].Item1;
                    predIdx = marks[i].Item2;

                    node.Next.Add(pred.Next[i]);
                    pred.Next[i] = node;
                }
                else/* de lo contraaaario: cabeza apunta a node y node a la cola */
                {
                    head.Next.Add(node);
                    head.Width.Add(idx);
                    node.Next.Add(null);
                }
                int prevWidth = pred.Width[i];// tamaño de la referencia (------>) anterior
                pred.Width[i] = idx - predIdx;// tamaño de la primera subreferencia (-->|)
                node.Width.Add(prevWidth - pred.Width[i] + 1);// tamaño de la segunda subreferencia (|-->)
            }

            for (int i = l; i < maxLevel; i++)// actualizando referencias superiores a l
                marks[i].Item1.Width[i]++;// TA MALLLLLL, SI APUNTA A NIL SE DEJA EN 0

            this.Count++;
        }

        public void RemoveAt(int idx)
        {
            var marks = Mark(idx);
            IdxSkipListNode<T> node = marks[0].Item1.Next[0];
            int l = node.Level;

            for (int i = maxLevel - 1; i >= l; i--)// actualizando referencias superiores a l
                marks[i].Item1.Width[i]--;

            for (int i = l - 1; i >= 0; i--)
            {
                var pred = marks[i].Item1;// predecesor del nodo en el nivel i-ésimo
                int predIdx = marks[i].Item2;

                pred.Next[i] = node.Next[i];// mi antecesor apunta a mi sucesor
                pred.Width[i] += node.Width[i] - 1;

                if (pred.Width[i] == Count - 1 && i != 0)// reduzco maxLevel
                {
                    pred.Next.RemoveAt(i);
                    pred.Width.RemoveAt(i);
                }
            }
            this.Count--;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();

                var prevNode = Mark(index)[0].Item1;
                return prevNode.Next[0].Value;
            }
            set
            {
                if (index < 0 || index >= Count)
                    throw new IndexOutOfRangeException();

                var prevNode = Mark(index)[0].Item1;
                prevNode.Next[0].Value = value;
            }
        }

        //private bool Delete(T x)
        //{
        //    var marks = Mark(x);
        //    int l = marks[0].Next.Count;
        //    var node = marks[0].Next[0];
        //    for (int i = 0; i < l; i++)
        //    {
        //        marks[i].Next[i] = node.Next[i];
        //    }

        //    while (maxLevel > 0 && head.Next[maxLevel - 1] == tail)
        //    {
        //        head.Next.RemoveAt(maxLevel - 1);
        //    }

        //    this.Count--;
        //    return true;
        //}

        private int GetLevel()
        {
            int l = 1;
            while (rand.NextDouble() < this.p)// DEFINE A MaxLevel VARIABLE IN ORDER THIS ALGORITHM CAN FINISH
                l++;

            return l;
        }

        public Tuple<IdxSkipListNode<T>, int>[] Mark(int idx)
        {
            var marks = new Tuple<IdxSkipListNode<T>, int>[maxLevel];
            var current = head;// empiezo en la cabeza
            int acum = -1;
            int level = maxLevel - 1;// último nivel
            while (level >= 0)// no lo he encontrado
            {
                if (current.Next[level] != null && current.Width[level] + acum < idx)// me puedo mover
                {
                    acum += current.Width[level];
                    current = current.Next[level];
                }
                else// desciendo y marco
                {
                    marks[level] = new Tuple<IdxSkipListNode<T>, int>(current, acum);
                    level--;
                }
            }
            return marks;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = head;

            while (current.Next[0] != null)
            {
                current = current.Next[0];
                yield return current.Value;
            }
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get; }

        public override string ToString()
        {
            string cad = "";
            for (int i = maxLevel - 1; i >= 0; i--)
            {
                var curr = head;
                while (curr != null)
                {
                    cad += curr.Value;
                    for (int j = 0; j < curr.Width[i]; j++)
                    {
                        cad += "-";
                    }
                    curr = curr.Next[i];
                }
                cad += "\n";
            }
            return cad;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class IdxSkipListNode<T>
    {
        public IdxSkipListNode() : this(default(T))
        {
        }

        public IdxSkipListNode(T x)
        {
            this.Value = x;
            Next = new List<IdxSkipListNode<T>>();

            // Width[i] := la cantd de pasos hasta Next[i]
            Width = new List<int>();
        }

        /// <summary>
        /// Width[i] := number of steps to Next[i].
        /// </summary>
        public List<int> Width { get; }

        /// <summary>
        /// Next[i] := next node in the ith level.
        /// </summary>
        public List<IdxSkipListNode<T>> Next { get; }
        public T Value { get; set; }
        public int Level => Next.Count;
    }
}
