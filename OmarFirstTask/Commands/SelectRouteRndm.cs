using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OmarFirstTask
{
    public class SelectRouteRndm : Command
    {
        public SelectRouteRndm() : base(false)
        {

        }

        /// <summary>
        /// Escoge una ruta
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        public override IEnumerable<DistributionNetwork> Execute(DistributionNetwork center)
        {
            var upperLimit = center.Vehicles.Count - quarter.routes.Count;// Si ya escogi alguna ruta el max es Count menos a elegir
            var routes = GetRoutesIndexes(center);

            for (int i = 0; i < RandomCommand.Times; i++)
            {
                var res = RandomCommand.R.Next(0, upperLimit);
                var routeNumber = routes[res];

                quarter.routes.Add(center.Vehicles[routeNumber].Route);

                yield return center;
                quarter.routes.RemoveAt(quarter.routes.Count - 1);
            }

        }

        private List<int> GetRoutesIndexes(DistributionNetwork center)
        {
            var routes = new List<int>();
            for (int k = 0; k < center.Vehicles.Count; k++)
            {//Si ya esta escogida una ruta, la remuevo para no escogerla otra vez
                if (quarter.routes.Count > 0 &&
                    quarter.routes[quarter.routes.Count - 1].Equals(center.Vehicles[k].Route))
                    continue;

                routes.Add(k);
            }
            return routes;
        }
    }
}