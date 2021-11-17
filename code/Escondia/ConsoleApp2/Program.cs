using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmarFirstTask;

namespace ConsoleApp2
{
    class Ivns
    {
        static void Start(){
            
            IList<Client> list = Utils.ReadFile(args[0], out Client center, out int capacity);

            DistributionNetwork net = new DistributionNetwork(list, 9, capacity, center);
            //DistributionNetwork net = new DistributionNetwork(list, Utils.ReadRoutes("opt-A-n65-k9"), 9, capacity, center);

            int i = 0;
            int aux = 0;
            Stopwatch crono = new Stopwatch();
            DistributionNetwork bestNet = (DistributionNetwork)net.Clone();

            crono.Start();

            int numV = 0;
            int max = 1;
            int n = 2000000;
            while (true)
            {
                numV++;
                NbhGenerator generator = new NbhGenerator(new[] { typeof(InsertClient), typeof(SwapClients) }, numV);

                foreach (var nbh in generator.GetNeighborhoods())
                {
                    Utils.PrintNbh(nbh);
                    bestNet = nbh.GetBest(bestNet);// Era aki lo del cluster

                    i += nbh.combinaciones_analizadas;
                    aux += nbh.combinaciones_analizadas;
                    if (aux / n > 0)
                    {
                        aux /= n;
                        Utils.PrintNet(bestNet, bestNet.Center.Point);
                        Console.WriteLine(bestNet.TotalDistance);
                    }

                    Console.WriteLine(i + " Combinaciones Analizadas en : " + crono.ElapsedMilliseconds);
                }
                if (numV == max)
                {
                    max++;
                    numV = 0;
                    Console.WriteLine("Comienzo Pequenno//////////////////////////////////////////////////////////////////////////////////////////");
                }
                net = bestNet;
                Console.WriteLine();
                Console.WriteLine(bestNet);
                Console.WriteLine("Cost: " + bestNet.TotalDistance);
                
                long seg = crono.ElapsedMilliseconds / 1000;
                long min = seg / 60;
                long prom = i / (seg == 0 ? 1 : seg);
                Console.WriteLine(i + " Combinaciones Analizadas en : " +  seg + "seg, " + min + "min, En PROMEDIO: " + prom + " comb analizadas por segundo");
                Console.WriteLine("Agrando Vecindad----------------------------------------------------------------------------------");
            }

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            TestingSkipList();
            #region Antigua prueba

            ////Para 4 clientes, contando el almacen(5)
            //double[,] dist =
            //{
            //    {0, 1, 2, 3, 3 },
            //    {1, 0, 2, 5, 5 },
            //    {2, 2, 0, 8, 7 },
            //    {3, 5, 8, 0, 9 },
            //    {3, 5, 7, 9, 0 }
            //};

            //Route route1 = new Route(new[] { new Client(1), new Client(2) ,new Client(3)}, 0);
            //Route route2 = new Route(new[] { new Client(4) }, 1);

            //IEnumerable<Vehicle> vehcs = new[] { new Vehicle(route1), new Vehicle(route2) };
            #endregion

            //QuickList<int> ql = new QuickList<int>(new int[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15});
            //ql.PrintTree();
            //TestingSkipList();

            //TestingQuickList();

            IList<Client> list = Utils.ReadFile(args[0], out Client center, out int capacity);

            DistributionNetwork net = new DistributionNetwork(list, 9, capacity, center);
            //DistributionNetwork net = new DistributionNetwork(list, Utils.ReadRoutes("opt-A-n65-k9"), 9, capacity, center);


            for (int m = 0; m < net.distances.GetLength(0); m++)
            {
                for (int k = 0; k < net.distances.GetLength(1); k++)
                {
                    Console.Write((int)net.distances[m, k] + "".PadRight(3));
                }
                Console.WriteLine();
            }

            Console.WriteLine("//////////////////////////////");
            Console.WriteLine(net + "\n" + net.TotalDistance);
            Console.WriteLine("//////////////////////////////");
            Utils.PrintNet(net, net.Center.Point);
            Console.WriteLine();



            //Neighborhood nbhTest = new Neighborhood(new[] { new Quarter(new SwapClients()), new Quarter(new SwapClients(), true) });

            int i = 0;
            int aux = 0;
            Stopwatch crono = new Stopwatch();
            DistributionNetwork bestNet = (DistributionNetwork)net.Clone();

            crono.Start();

            int numV = 0;
            int max = 1;
            int n = 2000000;
            while (true)
            {
                numV++;
                NbhGenerator generator = new NbhGenerator(new[] { typeof(InsertClient), typeof(SwapClients) }, numV);

                foreach (var nbh in generator.GetNeighborhoods())
                {
                    Utils.PrintNbh(nbh);
                    bestNet = nbh.GetBest(bestNet);

                    i += nbh.combinaciones_analizadas;
                    aux += nbh.combinaciones_analizadas;
                    if (aux / n > 0)
                    {
                        aux /= n;
                        Console.WriteLine();
                        Utils.PrintNet(bestNet, bestNet.Center.Point);
                        Console.WriteLine(bestNet.TotalDistance);
                    }

                    Console.WriteLine(i + " Combinaciones Analizadas en : " + crono.ElapsedMilliseconds);
                }
                if (numV == max)
                {
                    max++;
                    numV = 0;
                    Console.WriteLine("Comienzo Pequenno//////////////////////////////////////////////////////////////////////////////////////////");
                }
                net = bestNet;
                Console.WriteLine();
                Console.WriteLine(bestNet);
                Console.WriteLine("Cost: " + bestNet.TotalDistance);
                
                long seg = crono.ElapsedMilliseconds / 1000;
                long min = seg / 60;
                long prom = i / (seg == 0 ? 1 : seg);
                Console.WriteLine(i + " Combinaciones Analizadas en : " +  seg + "seg, " + min + "min, En PROMEDIO: " + prom + " comb analizadas por segundo");
                Console.WriteLine("Agrando Vecindad----------------------------------------------------------------------------------");
            }

            Console.ReadLine();
        }

        private static void TestingAVL()
        {
            AVLTree<int> avl = new AVLTree<int>();
            List<int> l = new List<int>();
            QuickList<int> ql = new QuickList<int>();
            Stopwatch lStopwatch = new Stopwatch();
            Stopwatch qlStopwatch = new Stopwatch();
            double lTime = 0, qlTime = 0;

            int n = 1000000;
            for (int i = 0; i < n; i++)
            {
                lStopwatch.Restart();
                l.Insert(0, i);
                lStopwatch.Stop();
                lTime += lStopwatch.ElapsedMilliseconds;

                qlStopwatch.Restart();
                ql.Insert(0, i);
                qlStopwatch.Stop();
                qlTime += qlStopwatch.ElapsedMilliseconds;
            }

            double lTime2 = 0, qlTime2 = 0;
            for (int i = 0; i < l.Count; i++)
            {
                lStopwatch.Restart();
                l.RemoveAt(0);
                lStopwatch.Stop();
                lTime2 += lStopwatch.ElapsedMilliseconds;

                qlStopwatch.Restart();
                ql.RemoveAt(0);
                qlStopwatch.Stop();
                qlTime2 += qlStopwatch.ElapsedMilliseconds;
            }

            Console.WriteLine("======INSERCIÓN=====\n" + lTime);
            Console.WriteLine(qlTime);

            Console.WriteLine("======ELIMINACIÓN=====");
            Console.WriteLine(lTime2);
            Console.WriteLine(qlTime2);
            CheckQL(l, ql);
            //Console.WriteLine(avl);
        }
        private static void TestingSkipList()
        {
            AVLTree<int> avl = new AVLTree<int>();
            List<int> l = new List<int>();
            QuickList<int> ql = new QuickList<int>();
            IndexableSkipList<int> sl = new IndexableSkipList<int>(0.9);
            Stopwatch lStopwatch = new Stopwatch();
            Stopwatch qlStopwatch = new Stopwatch();
            Stopwatch slStopwatch = new Stopwatch();
            double lTime = 0, qlTime = 0, slTime = 0;

            int n = 20;
            for (int i = 0; i < n; i++)
            {
                int idx = ql.Count / 2;
                //// List
                lStopwatch.Restart();
                l.Insert(idx, i);
                lStopwatch.Stop();
                lTime += lStopwatch.ElapsedMilliseconds;

                // AVL
                qlStopwatch.Restart();
                ql.Insert(idx, i);
                qlStopwatch.Stop();
                qlTime += qlStopwatch.ElapsedMilliseconds;

                // SkipList
                slStopwatch.Restart();
                sl.Insert(idx, i);
                slStopwatch.Stop();
                slTime += slStopwatch.ElapsedMilliseconds;
            }

            double lTime2 = 0, qlTime2 = 0, slTime2 = 0;
            for (int i = 0; i < l.Count; i++)
            {
                int idx = ql.Count / 2;

                lStopwatch.Restart();
                l.RemoveAt(idx);
                lStopwatch.Stop();
                lTime2 += lStopwatch.ElapsedMilliseconds;

                qlStopwatch.Restart();
                ql.RemoveAt(idx);
                qlStopwatch.Stop();
                qlTime2 += qlStopwatch.ElapsedMilliseconds;

                slStopwatch.Restart();
                sl.RemoveAt(idx);
                slStopwatch.Stop();
                slTime2 += slStopwatch.ElapsedMilliseconds;
            }

            Console.WriteLine("======INSERCIÓN=====\nList: " + lTime);
            Console.WriteLine("AVL: " + qlTime);
            Console.WriteLine("SL: " + slTime);

            Console.WriteLine("======ELIMINACIÓN=====");
            Console.WriteLine("List: " + lTime2);
            Console.WriteLine("AVL: " + qlTime2);
            Console.WriteLine("SL: " + slTime2);
            CheckQL(l, ql);
            CheckQL(l, sl);
            //Console.WriteLine(avl);
        }

        private static void TestingQuickList()
        {
            // initializing
            List<int> l = new List<int>();
            Random rand = new Random(Environment.CurrentManagedThreadId);
            Stopwatch randWatch = new Stopwatch();
            Stopwatch lWatch = new Stopwatch();
            Stopwatch qlWatch = new Stopwatch();
            double lTime = 0, qlTime = 0;

            int len = 20000, time = 0;
            for (int i = 0; i < len; i++)
            {
                l.Add(i * 30 - 15);
            }
            QuickList<int> ql = new QuickList<int>(l);

            // doing
            int queries = 10000;
            for (int i = 0; i < queries; i++)
            {
                int idx1 = rand.Next(len);
                int val1 = rand.Next(-30, 752);

                lWatch.Restart();
                l.Insert(idx1, val1);
                lWatch.Stop();
                lTime += lWatch.ElapsedMilliseconds;

                qlWatch.Restart();
                ql.Insert(idx1, val1);
                qlWatch.Stop();
                qlTime += qlWatch.ElapsedMilliseconds;

                randWatch.Restart();
                while (randWatch.ElapsedMilliseconds < time) ;

                int idx2 = rand.Next(len);
                int val2 = rand.Next(90, 145);

                lWatch.Restart();
                l[idx2] = val2;
                lWatch.Stop();
                lTime += lWatch.ElapsedMilliseconds;

                qlWatch.Restart();
                ql[idx2] = val2;
                qlWatch.Stop();
                qlTime += qlWatch.ElapsedMilliseconds;

                randWatch.Restart();
                while (randWatch.ElapsedMilliseconds < time) ;

                int idx3 = rand.Next(len);
                if ((i & 1) == 1)
                {
                    lWatch.Restart();
                    l.RemoveAt(idx3);
                    lWatch.Stop();
                    lTime += lWatch.ElapsedMilliseconds;

                    qlWatch.Restart();
                    ql.RemoveAt(idx3);
                    qlWatch.Stop();
                    qlTime += qlWatch.ElapsedMilliseconds;
                }
                else
                {
                    lWatch.Restart();
                    l.Remove(l[idx3]);
                    lWatch.Stop();
                    lTime += lWatch.ElapsedMilliseconds;

                    qlWatch.Restart();
                    ql.Remove(ql[idx3]);
                    qlWatch.Stop();
                    qlTime += qlWatch.ElapsedMilliseconds;
                }

                randWatch.Restart();
                while (randWatch.ElapsedMilliseconds < time) ;
            }
            // displaying times
            Console.WriteLine("=========TIMES=========");
            Console.WriteLine("List: {0} ms", lTime);
            Console.WriteLine("QuickList: {0} ms", qlTime);

            CheckQL(l, ql);
        }

        private static void CheckQL(IList<int> l, IList<int> ql)
        {
            // checking
            if (l.Count != ql.Count)
                Console.WriteLine("l.Count = {0}\t ql.Count = {1}", l.Count, ql.Count);

            for (int i = 0; i < ql.Count; i++)
            {
                if (l[i] != ql[i])
                    Console.WriteLine("l[{0}] = {1}\t ql[{0}] = {2}", i, l[i], ql[i]);
            }
        }
    }
}
