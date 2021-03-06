using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OmarFirstTask
{
    public class DistributionNetwork : IEnumerable<Vehicle>, IComparable<DistributionNetwork>, ICloneable
    {
        public List<Vehicle> Vehicles { get; set; }
        public double TotalDistance { get; internal set; }
        public Client Center { get; set; }

        public double[,] distances;

        public DistributionNetwork(IEnumerable<Vehicle> vehicles)
        {
            this.Vehicles = new List<Vehicle>();
            foreach (var vehicle in vehicles)
            {
                var vehic = (Vehicle)vehicle.Clone();
                this.Vehicles.Add(vehic);
                vehic.myNet = this;
            }
        }

        public DistributionNetwork(IEnumerable<Vehicle> vehicles, double[,] distances) : this(vehicles)
        {
            this.distances = distances;
            //Esto se puede quitar pq se hace en O(n)
            this.TotalDistance = Evaluate();
        }
        public DistributionNetwork(IEnumerable<Vehicle> vehicles, double[,] distances, double total)
            : this(vehicles)
        {
            this.distances = distances;
            this.TotalDistance = total;
        }

        public void SetRoutes(IList<Route> routes)
        {
            List<Client> clients = new List<Client>();
            var cap = Vehicles[0].Capacity;
            Vehicles = new List<Vehicle>();

            foreach (var r in routes)
            {
                Route rout = (Route)r.Clone();
                AddVehicle(new Vehicle(rout, cap));
            }
        }

        /// <summary>
        /// Se asume que el almacen es el primer cliente por defecto
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="minVehicles"></param>
        /// <param name="capacity"></param>
        public DistributionNetwork(IList<Client> clients, int capacity, Client center, int minVehicles = 7)
        {
            Center = center;
            distances = Utils.ComputeDistanceMatrix(clients);

            this.Vehicles = new List<Vehicle>();
            int cantxRout = (clients.Count / minVehicles - 3 > 0) ? clients.Count / minVehicles - 3
                                                                  : clients.Count / minVehicles + 1;
            int id = 0;
            Route rout = new Route(id++);
            Vehicle vehc = new Vehicle(rout, capacity);
            vehc.myNet = this;

            clients.RemoveAt(0);
            var r = new Random(Environment.TickCount);
            while (clients.Count != 0)
            {
                var i = r.Next(clients.Count);
                var client = clients[i];

                if (vehc.Route.Accept(client) && vehc.Route.Clients.Count <= cantxRout)
                {// el tipo va en esta ruta
                    vehc.Route.Insert(client);
                    clients.RemoveAt(i);
                }
                else
                {
                    AddVehicle(vehc);
                    rout = new Route(id++);
                    vehc = new Vehicle(rout, capacity);
                    vehc.myNet = this;
                }
            }
            AddVehicle(vehc);
        }


        /// <summary>
        /// Tiene en cuenta una ruta ya preextablecida como punto de partida
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="routIndexClients"></param>
        /// <param name="minVehicles"></param>
        /// <param name="capacity"></param>
        public DistributionNetwork(IList<Client> clients, List<List<int>> routIndexClients, int capacity, Client center)
        {
            Center = center;
            distances = Utils.ComputeDistanceMatrix(clients);

            this.Vehicles = new List<Vehicle>();
            int id = 0;
            Route rout = new Route(id++);
            Vehicle vehc = new Vehicle(rout, capacity);
            vehc.myNet = this;

            foreach (var r in routIndexClients)//Por cada ruta
            {
                for (int i = 0; i < r.Count; i++)//Voy por los clientes
                {
                    var client = clients[r[i]];//Ojo con el - 1,para todos menos la mia hay q hacer r[i] - 1

                    if (vehc.Route.Accept(client))
                    {// el tipo va en esta ruta
                        vehc.Route.Insert(client);
                    }
                }
                AddVehicle(vehc);
                rout = new Route(id++);
                vehc = new Vehicle(rout, capacity);
                vehc.myNet = this;
            }
            AddVehicle(vehc);
        }

        public DistributionNetwork(IEnumerable<Vehicle> vehicles, double[,] distances, double total, Client  center) : this(vehicles, distances, total)
        {
            Center = center;
        }

        private void AddVehicle(Vehicle vehc)
        {
            vehc.myNet = this;
            Vehicles.Add(vehc);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Vehicle> GetEnumerator()
        {
            return Vehicles.GetEnumerator();
        }

        public override string ToString()
        {
            string s = "";
            s += "\n";
            foreach (var vheic in Vehicles)
            {
                s += "-" + vheic.Route;
                s += "\n";
            }
            return s;
        }

        private double Evaluate()
        {
            double distTotal = 0;
            foreach (var item in this.Vehicles)
            {//Empiezo con el almacen y termino tb
                if (item.Route.Clients.Count != 0)
                {
                    distTotal += distances[0, item.Route.Clients[0].ID];
                    distTotal += distances[item.Route.Clients[item.Route.Clients.Count - 1].ID, 0];
                }
                for (int i = 0; i < item.Route.Clients.Count - 1; i++)//Sumo la distancia entre caada par
                {
                    distTotal += distances[item.Route.Clients[i].ID, item.Route.Clients[i + 1].ID];
                }
            }
            return distTotal;
        }

        public int CompareTo(DistributionNetwork other)
        {
            if (TotalDistance < other.TotalDistance)
                return -1;
            if (TotalDistance > other.TotalDistance)
                return 1;
            return 0;
        }

        public object Clone()
        {
            return new DistributionNetwork(Vehicles, distances, TotalDistance, Center);
        }

        /// <summary>
        /// Elimina rutas vacias
        /// </summary>
        /// <param name="number">numero de rutas vacias que deja</param>
        public void LeaveEmptyVehicles(int number = 0){
            var acc = 0;
            for (int i = 0; i < Vehicles.Count; i++)
            {
                if(Vehicles[i].Route.Clients.Count == 0){
                    acc++;
                    if(acc > number){
                        Vehicles.RemoveAt(i);
                    }
                }
            }
        }
    }
}
