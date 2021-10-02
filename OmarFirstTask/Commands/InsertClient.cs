using System;
using System.Collections.Generic;
using System.Linq;

namespace OmarFirstTask
{
    public class InsertClient : FinalCommand
    {
        public InsertClient() : base(new [] { typeof(SelectClient), typeof(SelectRouteRndm) })
        {
        }

        /// <summary>
        /// Inserta el ultimo cliente escogido en la ultima vecindad escogida
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<DistributionNetwork> Execute(DistributionNetwork center)
        {
            var route = quarter.routes[quarter.routes.Count - 1];
            var client = quarter.clients[quarter.clients.Count - 1].Item1;

            for (int i = 0; i <= route.Clients.Count; i++)//Pos en la que lo voy a ubicar
            {
                if (!route.Accept(client))
                    continue;
                route.Insert(i, client);
                //client.Route = route;

                yield return center;

                route.Remove(i);
                //client.RouteBack();
            }
        }
    }
}