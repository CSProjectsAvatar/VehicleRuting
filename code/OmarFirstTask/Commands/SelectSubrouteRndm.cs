using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask.Commands {
    class SelectSubrouteRndm : Command {
        public SelectSubrouteRndm() : base(new[] { typeof(SelectRouteRndm) }, false) {

        }

        public override IEnumerable<DistributionNetwork> Execute(DistributionNetwork center) {
            var route = quarter.routes[^1];

            for (int i = 0; i < RandomCommand.Times; i++) {
                var startIdx = RandomCommand.R.Next(
                    0, 
                    Math.Max(1, route.Clients.Count - 2));  // at least 2 free indices if possible
                var size = RandomCommand.R.Next(2, 4);
                size = Math.Min(size, route.Clients.Count - startIdx);  // avoiding out of range

                quarter.subroutes.Add(new List<Client>());

                for (int c = startIdx, cant = 0; cant < size; cant++) {
                    quarter.subroutes[^1].Add(route.Clients[c]);
                    route.Remove(c, false);
                }

                yield return center;

                for (int c = startIdx, subIdx = 0; subIdx < size; c++, subIdx++) {
                    route.Insert(c, quarter.subroutes[^1][subIdx], false);
                }

                quarter.subroutes.RemoveLast();
            }
        }
    }
}
