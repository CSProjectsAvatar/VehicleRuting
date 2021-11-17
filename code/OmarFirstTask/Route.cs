using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public class Route:ICloneable
    {
        public int Route_ID { get; set; }
        //private IUndoList<Client> clientList;
        private IList<Client> clientList;
        internal Vehicle myVehicle;

        public IList<Client> Clients
        {
            get { return clientList; }
            set { clientList = (List<Client>) value; }
        }

        public int Weight { get; private set; }

        public Route(IEnumerable<Client> clients, int id)
        {
            Route_ID = id;
            clientList = new List<Client>();
            foreach (var client in clients)
            {
                var c = (Client)client.Clone();
                c.Route = this;
                Clients.Add(c);
                Weight += c.AskedWeight;
            }
        }

        public Route(int id)
        {
            Route_ID = id;
            Clients = new List<Client>();
        }

        internal bool Accept(Client client)
        {
            if (Weight + client.AskedWeight > myVehicle.Capacity)
                return false;
            return true;
        }

        internal void Remove(int i, bool routeBack)
        {
            Weight -= Clients[i].AskedWeight;

            var client = Clients[i];
            Clients.RemoveAtAndUpdate(i);
            if(routeBack)
                client.RouteBack();
        }

        public void Insert(int i, Client client)
        {
            Insert(i, client, true);
        }

        internal void Remove(int i)
        {
            Remove(i, true);
        }

        internal void Insert(int i, Client client, bool addRoute)
        {
            if(addRoute)
                client.Route = this;
            Clients.InsertAndUpdate(i, client);
            Weight += client.AskedWeight;
        }

        internal void SelectAndRemove(int i)
        {
            Weight -= Clients[i].AskedWeight;
            var client = Clients[i];
            Clients.RemoveAtAndUpdate(i);
        }


        public override string ToString()
        {
            string s = "";

            foreach (var client in clientList)
            {
                s += (client.ID - 1) + " ";
            }
            s = s.PadRight(30);
            s += "weight:" + Weight;
            return s;
        }

        public object Clone()
        {
            return new Route(Clients, Route_ID);
        }

        internal void Insert(Client client)
        {
            Insert(this.Clients.Count, client);
        }
    }
}
