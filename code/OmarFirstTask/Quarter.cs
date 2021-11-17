using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    /* Barrio: secuencia de comandos constituída por UN SOLO COMANDO FINAL y los comandos que
        este necesita. Es un concepto similar a subvecindad, ya que <Neighborhood> es un conjunto
        de <Quarter>s*/
    public class Quarter
    {
        public IList<Command> Commands { get; private set; }

        /* Rutas y clientes que esta vecindad está usando para ejecutarse. Estas listas serán
         modificadas por los comandos q se ejecuten en este barrio. */
        internal List<Route> routes;
        internal List<Tuple<Client, int>> clients;
        internal IList<IList<Client>> subroutes;

        public Quarter(FinalCommand finalCommand, bool compressed)
        {
            this.Commands = new List<Command>();
            foreach (var cmd in Utils.GetCmdChain(finalCommand, compressed))
            {
                this.Commands.Add(cmd);
            }
            this.routes = new List<Route>();
            this.clients = new List<Tuple<Client, int>>();
            this.subroutes = new List<IList<Client>>();
        }
        public Quarter(FinalCommand finalCommand) : this(finalCommand, false) { }
        public IEnumerable<DistributionNetwork> GetNeighbors(DistributionNetwork center)
        {/* obtiene todas las redes q se pueden generar aplicando este barrio, centrado en <center> */

            return GetNeighbors(0, center);
        }

        private IEnumerable<DistributionNetwork> GetNeighbors(int cmdIdx, DistributionNetwork net)
        {
            Commands[cmdIdx].SetQuarter(this);
            if (cmdIdx == Commands.Count - 1)
            //ejecute to los no finales
            {
                /* ejecuta el comando final y devuelve cada red resultante */
                foreach (var finalNet in Commands[cmdIdx].Execute(net))
                {
                    yield return finalNet;
                }
            }
            else
            {
                var actualCmd = Commands[cmdIdx];

                foreach (var newNet in actualCmd.Execute(net))
                /* por cada red creada por este comando intermedio
                 */
                {
                    //obtengo las soluciones de este subárbol
                    foreach (var result in GetNeighbors(cmdIdx + 1, newNet))
                    {
                        yield return result;
                    }
                }
            }
        }
        public DistributionNetwork GetBest(DistributionNetwork center)
        {
            double min = center.TotalDistance;
            DistributionNetwork sol = center;
            //Console.WriteLine("De:\n" + sol + "$" + sol.TotalDistance + "$");
            foreach (var net in GetNeighbors(center))
            {
                if (net.TotalDistance < min)
                {
                    //Console.WriteLine("\nA:\n" + net + "\n" + "$" + net.TotalDistance + "$");
                    sol = (DistributionNetwork)net.Clone();
                    min = net.TotalDistance;
                }
            }
            return sol;
        }
    }
}
