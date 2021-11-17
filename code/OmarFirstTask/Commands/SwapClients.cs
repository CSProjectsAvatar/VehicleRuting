using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public class SwapClients : FinalCommand
    {
        public static int casoSwap = 0;
        public SwapClients() : base(new[] { typeof(SelectClient), typeof(SelectClient) })
        {

        }

        /// <summary>
        /// Intercambia los ultimos clientes escogidos
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DistributionNetwork> Execute(DistributionNetwork center)
        {
            if (quarter.clients.Count <= 1)
            {
                throw new FormatException("La lista de clientes no tiene la cantidad suficiente para intercambiar");
                yield break;
            }
            var tupleC1 = quarter.clients[quarter.clients.Count - 1];
            var tupleC2 = quarter.clients[quarter.clients.Count - 2];

            if (!Swap(tupleC1, tupleC2))
                yield break;

            yield return center;

            Deswap(tupleC1, tupleC2);
        }

        private void Deswap(Tuple<Client, int> tupleC1, Tuple<Client, int> tupleC2)
        {
            #region Initialize
            var posC1 = tupleC1.Item2;
            var posC2 = tupleC2.Item2;
            var client1 = tupleC1.Item1;
            var client2 = tupleC2.Item1;
            #endregion

            //client2.RouteBack();
            //client1.RouteBack();

            client1.Route.Remove(posC2);// ruta 2
            client2.Route.Remove(posC1);// ruta 1
        }

        private bool Swap(Tuple<Client, int> tupleC1, Tuple<Client, int> tupleC2)
        {//Intercambio los dos clientes en sus respectivas rutas
            #region Initialize
            var posC1 = tupleC1.Item2;
            var posC2 = tupleC2.Item2;

            var client1 = tupleC1.Item1;
            var client2 = tupleC2.Item1;
            var routeC1 = client1.Route;
            var routeC2 = client2.Route;
            #endregion
            if (!routeC1.Accept(client2) || !routeC2.Accept(client1))//Si alguna no acepta salgo
                return false;

            casoSwap++;
            routeC1.Insert(posC1, client2);
            routeC2.Insert(posC2, client1);

            //Actualizo ruta
            //client1.Route = routeC2;
            //client2.Route = routeC1;
            return true;
        }
    }
}