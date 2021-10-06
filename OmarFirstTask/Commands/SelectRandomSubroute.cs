using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask.Commands {
    class SelectRandomSubroute : Command {
        public SelectRandomSubroute() : base(new[] { typeof(SelectRouteRndm) }, false) {

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

                for (int c = startIdx + size - 1; c >= startIdx; c--) {  // using reverse loop to remove
                    quarter.subroutes[^1].Add(route.Clients[c]);
                    route.Remove(c, false);
                }

                yield return center;

                for (int c = startIdx; c - startIdx < size; c++) {
                    route.Insert(c, quarter.subroutes[^1][^1], false);
                    quarter.subroutes[^1].RemoveLast();
                }

                quarter.subroutes.RemoveLast();
            }
        }
    }
}
