using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public class Vehicle : IEnumerable<Client>, ICloneable
    {
        public int Capacity { get; set; }
        //internal Route Route;
        public Route Route;
        internal DistributionNetwork myNet;

        public Vehicle(Route route)
        {
            this.Route = (Route)route.Clone();//Uno de los dos esta mal, puse clone yo
            this.Route.myVehicle = this;
            this.Capacity = int.MaxValue;//Si no lo seteas no te interesa
        }
        public Vehicle(Route route, int capacity)
        {
            this.Route = (Route)route.Clone();
            this.Route.myVehicle = this;
            this.Capacity = capacity;
        }

        public Vehicle() : this(null) { }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
            //return GetEnumerator();???
        }

        IEnumerator<Client> IEnumerable<Client>.GetEnumerator()
        {
            return Route.Clients.GetEnumerator();
        }

        public object Clone()
        {
            return new Vehicle(Route, Capacity);
        }
    }
}
