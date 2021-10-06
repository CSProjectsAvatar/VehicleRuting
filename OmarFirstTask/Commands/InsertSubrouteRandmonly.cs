using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask.Commands {
    public class InsertSubrouteRandmonly : FinalCommand {
        public InsertSubrouteRandmonly() : base(new[] { typeof(SelectRandomSubroute), typeof(SelectRouteRndm) }) {
        }

        public override IEnumerable<DistributionNetwork> Execute(DistributionNetwork center) {
            var route = quarter.routes[^1];
            var idxs = new List<int>();

            for (int i = 0; i < RandomCommand.Times; i++) {
                foreach (var client in quarter.subroutes[^1]) {
                    if (!route.Accept(client)) {
                        yield break;
                    } else {
                        var idx = RandomCommand.R.Next(0, route.Clients.Count);
                        route.Insert(idx, client);

                        idxs.Add(idx);  // saving indexes in order to undo later
                    }
                }
                yield return center;

                for (int k = idxs.Count - 1; k >= 0; k--) {
                    route.Remove(idxs[k]);
                }
                idxs.Clear();
            }
        }
    }
}
