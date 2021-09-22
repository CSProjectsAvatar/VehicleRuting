using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public class SelectClient : Command
    {
        public static int caso = 0;
        public static int inicio = 0;
        public SelectClient() : base(new[] { typeof(SelectRoute) }, false)
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
            int init = 0;
            if (quarter.routes.Count == 1 && quarter.clients.Count == 1)//Estoy escogiendo 2 de la misma V
                init = quarter.clients[0].Item2;// sin 1

            for (int i = init; i < rout.Clients.Count; i++)//Cliente que voy a escoger
            {
                var client = rout.Clients[i];
                rout.Remove(i, false);
                quarter.clients.Add(new Tuple<Client, int>(client, i));//Meto en la lista
                yield return center;

                quarter.clients.RemoveAt(quarter.clients.Count - 1);//Lo saco
                rout.Insert(i, client, false);//Lo vuelvo a meter
            }
        }
    }
}
