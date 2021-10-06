using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask.Commands {
    public class InsertSubrouteRndm : FinalCommand {
        public InsertSubrouteRndm() : base(new[] { typeof(SelectSubrouteRndm), typeof(SelectRouteRndm) }) {
        }

        public override IEnumerable<DistributionNetwork> Execute(DistributionNetwork center) {
            var route = quarter.routes[^1];

            for (int _ = 0; _ < RandomCommand.Times; _++) {
                var idx = RandomCommand.R.Next(0, route.Clients.Count);

                for (int k = 0; k < quarter.subroutes[^1].Count; k++) {
                    var client = quarter.subroutes[^1][k];

                    if (!route.Accept(client)) {
                        Undo(route, idx, k);
                        yield break;
                    } else {
                        route.Insert(idx + k, client);
                    }
                }
                yield return center;

                Undo(route, idx, quarter.subroutes[^1].Count);
            }
        }

        private void Undo(Route route, int idx, int total) {
            for (int __ = 0; __ < total; __++) {
                route.Remove(idx);
            }
        }
    }
}
