using OmarFirstTask.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    /* Contiene métodos útiles que la solución entera usa. */
    public /*internal*/ static class Utils
    {
        /* determina si he pasado x un comando a la hora de generar la cadena.
         Esto es para generar cadenas comprimidas, del tipo "raac" */
        private static bool[] cmdMark;

        public static IEnumerable<Command> GetCmdChain(Command cmd, bool compressed)
        {/* devuelve la cadena de comandos q termina en cmd, garantizando crear una vecindad q funcione
            correctamente, donde a cada comando le preceden los comandos q necesita */

            if (compressed && cmd is FinalCommand)
                cmdMark = new bool[200];

            if (!cmd.InitialCmd)
                foreach (var prevCommand in cmd.PreviousCommands)
                {
                    Command command = ToCommand(prevCommand);
                    char c = GetCmdChar(command);

                    if (compressed && cmdMark[c])
                        yield return command;

                    else
                    {
                        foreach (var chainElem in GetCmdChain(command, compressed))
                        {// retornar la recurrencia en los previos a command
                            yield return chainElem;
                        }

                        if (compressed)
                            cmdMark[c] = true;
                    }
                }

            yield return cmd;
        }

        public static Command ToCommand(Type typeCmd)
        {// revisar si es un <Command> e instanciar

            var construct = typeCmd.GetConstructor(new Type[] { });
            if (construct == null || !(construct.Invoke(null) is Command cmd))
                throw new ArgumentException();

            return cmd;
        }

        public static FinalCommand ToFinalCmd(Type typeCmd)
        {// revisar si es un <FinalCommand> e instanciar

            var construct = typeCmd.GetConstructor(new Type[] { });
            if (construct == null || !(construct.Invoke(null) is FinalCommand finalCmd))
                throw new ArgumentException();

            return finalCmd;
        }

        public static IEnumerable<Vehicle> GetVehicles(int vehicAmount, int clientsAmount)
        {
            throw new NotImplementedException();

            int id = 0;
            for (int i = 0; i < vehicAmount; i++)
            {
                QuickList<Client> clients = new QuickList<Client>();
                for (int j = 0; j < clientsAmount / vehicAmount; j++)
                {
                    //clients.Add(new Client(id++));
                }
                yield return new Vehicle(new Route(clients, i));
            }
        }

        public static string GetNbhStr(Neighborhood nbh)
        {
            var res = "";
            foreach (var nbhQuarter in nbh.Quarters)
            {
                res += GetQuarterStr(nbhQuarter);
            }
            return res;
        }

        private static string GetQuarterStr(Quarter quarter)
        {
            var res = "";
            foreach (var cmd in quarter.Commands)
            {
                char c = GetCmdChar(cmd);
                res += c;
            }
            return res;
        }

        private static char GetCmdChar(Command cmd)
        {/* retorna el char unívoco q identifica a <cmd> */

            char c = ' ';

            if (cmd is SelectRoute)
                c = 'r';

            else if (cmd is SelectClient)
                c = 'a';

            else if (cmd is SwapClients)
                c = 'c';

            else if (cmd is SelectSubrouteRndm)
                c = 's';

            else if (cmd is InsertClient)
                c = 'b';

            else if (cmd is InsertSubrouteRndm) {
                c = 'u';

            } else if (cmd is SelectClientRndm) {
                c = 'e';

            } else if (cmd is SelectRouteRndm) {
                c = 't';

            } else if (cmd is InsertClientRndm) {
                c = 'm';
            } else {
                throw new NotImplementedException();
            }

            return c;
        }

        public static bool CanCompress(FinalCommand finalCmd)
        {/* determina si la cadena de <finalCmd> se puede comprimir, i.e, si tiene
            al menos 2 comandos iguales en <PreviousCommands> */

            bool[] mark = new bool[200];
            foreach (var prevCmd in finalCmd.PreviousCommands)
            {
                Command cmd = ToCommand(prevCmd);

                char c = GetCmdChar(cmd);
                if (mark[c])
                    return true;

                mark[c] = true;
            }
            return false;
        }

        /// <summary>
        /// Returns the matrix with all the distances, the wherehouse should be at the beguining
        /// </summary>
        /// <param name="clients">estan mapeados: client id = client.id - 1</param>
        /// <returns></returns>
        public static double[,] ComputeDistanceMatrix(IList<Client> clients)
        {
            double[,] matrix = new double[clients.Count + 1, clients.Count + 1];
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                matrix[i, i] = 0;
                for (int k = i + 1; k < matrix.GetLength(1); k++)
                {
                    matrix[i, k] = Point.EuclideanDistance(clients[i - 1].Point, clients[k - 1].Point);
                    matrix[k, i] = matrix[i, k];
                }
            }
            return matrix;
        }

        public static IList<Client> ReadFile(string fileName, out Client center, out int capacity)
        {
            string filepath = System.IO.Path.GetFullPath(fileName);
            if (!File.Exists(filepath))
                throw new FileLoadException();

            string[] lines = File.ReadAllLines(filepath);
            capacity = int.Parse(lines[5].Split()[2]);


            List<Client> clients = new List<Client>();
            int index = 7;
            while (index < lines.Length)//Empiezan en la linea 8
            {
                var actL = lines[index].Split();
                if (actL[0] == "DEMAND_SECTION")//Paro cuando vea esto
                    break;
                int i = 0;
                if (actL[0] == "")
                    i = 1;

                clients.Add(new Client(int.Parse(actL[0 + i]),
                            new Point(double.Parse(actL[1 + i]), double.Parse(actL[2 + i])), 0));
                index++;
            }

            //index++;// Me salto el primero por el defasaje que hay en la bibliografia(almacen es el ultimo) 
            //Asigno las demandas
            for (int i = 0; i <= clients.Count; i++)//Ojo con el <= ese
            {
                index++;

                var actL = RemoveSpaces(lines[index]);
                if (actL[0] == "DEPOT_SECTION")//Paro cuando vea esto
                {
                    index++;
                    break;
                }
                var cl = clients[i];
                cl.AskedWeight = int.Parse(actL[1]);
            }
            var line = lines[index].Split();
            int idCenter = int.Parse(line[1]);
            center = clients[idCenter - 1];
            return clients;
        }

        private static string[] RemoveSpaces(string line)
        {
            var sol = from item in line.Split() where item != "" select item;
            return sol.ToArray();
        }

        public static List<List<int>> ReadRoutes(string fileName)
        {
            string filepath = System.IO.Path.GetFullPath(fileName);
            if (!File.Exists(filepath))
                throw new FileLoadException();

            string[] lines = File.ReadAllLines(filepath);

            List<List<int>> rutas = new List<List<int>>();

            foreach (var l in lines)
            {
                List<int> estaRuta = new List<int>();
                var lsplit = l.Split();
                if (lsplit.Length == 0 || lsplit[0] == "" || lsplit[0] == "cost")
                    break;

                for (int i = 2; i < lsplit.Length; i++)
                {
                    if (lsplit[i] == "")
                        break;
                    estaRuta.Add(int.Parse(lsplit[i]));//-1 se debe quitar depende de la interpretacion
                }
                rutas.Add(estaRuta);
            }
            return rutas;
        }

        private static void GraphicPoints(List<Route> routes, int maxXY, int minXY,  Point center)
        {
            //Console.BackgroundColor = ConsoleColor.White;
            Tuple<int, int>[,] cartesians = new Tuple<int, int>[maxXY - minXY + 1, maxXY - minXY + 1];
            int suma = minXY < 0 ? minXY : -minXY;
            for (int i = 0; i < routes.Count; i++)//Asigno los puntos
            {
                foreach (var cl in routes[i].Clients)
                {
                    cartesians[(int)cl.Point.y + suma, (int)cl.Point.x + suma] = new Tuple<int, int>(cl.ID, routes[i].Route_ID);
                }
            }
            int[] c = new int[15];
            for (int i = 0; i < cartesians.GetLength(0); i++)//Dibujo
            {
                Console.WriteLine();
                int col = 0;
                for (int k = 0; k < cartesians.GetLength(1); k++)
                {
                    //col++;
                    if (i == center.y + suma && k == center.x + suma)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("@");
                        continue;
                    }
                    if (cartesians[i, k] is null)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" ");
                        continue;
                    }
                    switch (cartesians[i, k].Item2)
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(c[0]++);
                            break;
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(c[1]++);

                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(c[2]++);

                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(c[3]++);

                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(c[4]++);

                            break;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(c[5]++);

                            break;
                        case 6:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(c[6]++);

                            break;
                        case 7:
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(c[7]++);

                            break;
                        case 8:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write(c[8]++);

                            break;
                        case 9:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(c[9]++);

                            break;
                        case 10:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(c[10]++);

                            break;
                        case 11:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(c[11]++);

                            break;
                        case 12:
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write(c[12]++);

                            break;
                        case 13:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(c[13]++);

                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(c);

                            break;
                    }
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public static void PrintNet(DistributionNetwork net, Point center)
        {
            List<Route> rout = new List<Route>();
            foreach (var v in net.Vehicles)
            {
                rout.Add(v.Route);
            }
            var tuple = MaxCoord(net);
            Utils.GraphicPoints(rout, tuple.Item1, tuple.Item2, center);
        }

        private static Tuple<int, int> MaxCoord(DistributionNetwork net)
        {
            int max = int.MinValue;
            int min = int.MaxValue;
            foreach (var v in net.Vehicles)
            {
                foreach (var c in v.Route.Clients)
                {
                    max = (int)Math.Max(Math.Max(c.Point.x, c.Point.y), max);
                    min = (int)Math.Min(Math.Min(c.Point.x, c.Point.y), min);
                }
            }
            return new Tuple<int, int>(max,min);
        }
    }

    public struct Point
    {
        public double x, y;
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public static double EuclideanDistance(Point a, Point b)
        {
            return Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
        }
    }
}
