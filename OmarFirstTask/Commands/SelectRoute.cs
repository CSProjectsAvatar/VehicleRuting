using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OmarFirstTask
{
    public class SelectRoute : Command
    {
        public SelectRoute() : base(false)
        {

        }

        /// <summary>
        /// Escoge una ruta
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        public override IEnumerable<DistributionNetwork> Execute(DistributionNetwork center)
        {
            //Para ir escogiendo solo para delante, para no repetir
            int init = quarter.routes.Count == 0 ? 0 : quarter.routes[0].Route_ID + 1;

            for (int i = init; i < center.Vehicles.Count; i++)//Ruta que voy a escoger
            {//quiero diferenciar las rutas distintas de las iguales
                if (quarter.routes.Count > 0 &&
                    quarter.routes[quarter.routes.Count - 1].Equals(center.Vehicles[i].Route))
                    continue;
                 quarter.routes.Add(center.Vehicles[i].Route);

                yield return center;
                quarter.routes.RemoveAt(quarter.routes.Count - 1);
            }
        }
    }
}