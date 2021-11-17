using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    internal static class IListExtensors
    {
        /// Insert in the current list and update the distances in the network the clients are in.
        internal static void InsertAndUpdate(this IList<Client> list, int index, Client client)
        {
            if (index < 0 || index > list.Count)
                throw new IndexOutOfRangeException("CLIENT LIST INDEX OUT OF RANGE");

            var net = client.Route.myVehicle.myNet;
            var d = net.distances;

            int idBef = net.Center.ID, idAft = net.Center.ID;
            int idAct = client.ID;

            if (index == list.Count)
                list.Add(client);

            else
            {
                list.Insert(index, client);
                idAft = list[index + 1].ID;
            }

            if (index != 0)
                idBef = list[index - 1].ID;
            // quitando distancia directa
            net.TotalDistance -= d[idBef, idAft];

            // sumando nuevo dueto de distancias
            net.TotalDistance += d[idBef, idAct] + d[idAct, idAft];
        }

        internal static void RemoveAtAndUpdate(this IList<Client> list, int index)
        {
            if (index < 0 || index >= list.Count)
                throw new IndexOutOfRangeException("CLIENT LIST INDEX OUT OF RANGE");

            Client client = list[index];

            var net = client.Route.myVehicle.myNet;
            var d = net.distances;

            int idBef = net.Center.ID, idAft = net.Center.ID;
            int idAct = list[index].ID;

            if (index + 1 != list.Count)
                idAft = list[index + 1].ID;

            if (index != 0)
                idBef = list[index - 1].ID;
            // eliminando dueto de distancias
            net.TotalDistance -= d[idBef, idAct] + d[idAct, idAft];

            // agregando distancia directa
            net.TotalDistance += d[idBef, idAft];

            list.RemoveAt(index);
        }

        internal static void AddAndUpdate(this IList<Client> list, Client client)
        {
            InsertAndUpdate(list, list.Count, client);
        }

        internal static void RemoveLast<T>(this IList<T> list) {
            list.RemoveAt(list.Count - 1);
        }
    }
}
