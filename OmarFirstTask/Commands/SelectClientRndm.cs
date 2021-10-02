using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public class SelectClientRndm : Command
    {
        public static int caso = 0;
        public static int inicio = 0;
        public SelectClientRndm() : base(new[] { typeof(SelectRouteRndm) }, false)
        {
        }

        /// <summary>
        /// Escoge un cliente de la ultima ruta escogida
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        public override IEnumerable<DistributionNetwork> Execute(DistributionNetwork center)
        {
            if (quarter.routes.Count == 0)
            {
                throw new FormatException("La lista de rutas esta vacia");
                yield break;
            }

            var rout = quarter.routes[quarter.routes.Count - 1];
            if(rout.Clients.Count == 0)
                yield break;// La ruta elegida no tiene clientes a escoger

            for (int _ = 0; _ < RandomCommand.Times; _++)//Cliente que voy a escoger
            {
                var index = RandomCommand.R.Next(0, rout.Clients.Count);
                var client = rout.Clients[index];

                rout.Remove(index, false);

                quarter.clients.Add(new Tuple<Client, int>(client, index));//Meto en la lista de clientes elegidos
                yield return center;

                quarter.clients.RemoveAt(quarter.clients.Count - 1);//Lo saco
                rout.Insert(index, client, false);//Lo vuelvo a meter en la ruta
            }
        }
    }
}
