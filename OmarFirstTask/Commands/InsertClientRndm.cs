using System;
using System.Collections.Generic;
using System.Linq;

namespace OmarFirstTask
{
    public class InsertClientRndm : FinalCommand
    {
        public InsertClientRndm() : base(new [] { typeof(SelectClientRndm), typeof(SelectRouteRndm) })
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

            for (int _ = 0; _ < RandomCommand.Times ; _++)
            {
                if (!route.Accept(client))
                    continue;

                var index = RandomCommand.R.Next(0, route.Clients.Count+1);// lim Sup abierto
                route.Insert(index, client);

                yield return center;

                route.Remove(index);
            }
        }
    }
}