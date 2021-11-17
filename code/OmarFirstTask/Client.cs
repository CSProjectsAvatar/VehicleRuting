using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public class Client : ICloneable
    {
        /// <summary>
        /// Unique identity number.
        /// </summary>
        public readonly int ID;
        public Point Point { get; }
        public int AskedWeight { get; internal set; }
        public Route Route
        {
            get { return routeStack.Peek(); }
            set { routeStack.Push(value); }
        }
        public void RouteBack()
        {
            routeStack.Pop();
        }
        private Route route;
        private Stack<Route> routeStack;

        public Client(int id, Point p, int askedWeight)
        {
            this.ID = id;
            this.Point = p;
            AskedWeight = askedWeight;
            routeStack = new Stack<Route>();
        }
        public override string ToString()
        {
            return ID + "";
        }

        public object Clone()
        {
            return new Client(ID, new Point(Point.x, Point.y), AskedWeight);
        }
    }

    #region Cosas
    //public class NormalClient : Client
    //{
    //    public NormalClient(int time, int distance) : base(time, distance) { Priority = false; }
    //}
    //public class PriorityClient : Client
    //{
    //    public PriorityClient(int time, int distance) : base(time, distance) { Priority = true; }
    //}
    //public class Clients : Client
    //{
    //    public Client Client;
    //    public List<Client> Customers = new List<Client>();

    //    public Clients(bool priority, int time, int distance) : base(time, distance)
    //    {
    //        if (priority)
    //        {
    //            Client = new PriorityClient(time, distance);
    //        }
    //        else
    //        {
    //            Client = new NormalClient(time, distance);
    //        }
    //    }

    //    public int Times { get { return Client.Time; } }
    //    public void Add()
    //    {
    //        if (Customers.Count == 0)
    //            Customers.Add(Client);
    //        else
    //        {
    //            if (Client.Priority)
    //            {
    //                int count = 0;
    //                foreach (var x in Customers)
    //                {
    //                    if (x.Priority)
    //                    {
    //                        Customers.Insert(count - 1, x);
    //                        count = 0;
    //                        break;
    //                    }
    //                    count++;
    //                }
    //                if (count == Customers.Count)
    //                {
    //                    Customers.Add(Client);
    //                    count = 0;
    //                }
    //            }
    //        }
    //    }
    //    public void Remove()
    //    {
    //        Customers.RemoveAt(0);
    //    }
    //}
    #endregion
}
