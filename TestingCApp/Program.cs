﻿using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmarFirstTask;

namespace TestingCApp
{
    class Ivnp
    {
        public Ivnp(long totalTimeInSeconds = 60, long divingTimeInSeconds = 30)
        {
            TimeTracker = new TimeTracker(totalTimeInSeconds, divingTimeInSeconds);
            SolsVisited = new List<Tuple<Neighborhood, DistributionNetwork>>();
            // MaxNeighborhoodDeph = maxNeighborhoodDeph;
        }
        
        public string FileName { get; set; }
        public TimeTracker TimeTracker{ get; set; }
        public float[,] DistanceMatrix { get; set; }
        public DistributionNetwork InitialNet { get; set; }
        public DistributionNetwork BestNet { get; set; }
        public int InitialSearchDeph { get; set; } = 1; // numV antes
        public int SolVisitedPerCicle { get; set; } = 2000000;
        public List<Tuple<Neighborhood, DistributionNetwork>> SolsVisited { get; set; }
        public int AnalizedComb { get; set; }
        
        public void LoadNet(){
            IList<Client> list = Utils.ReadFile(FileName, out Client center, out int capacity);
            InitialNet = new DistributionNetwork(list, capacity, center);

        }
        //DistributionNetwork net = new DistributionNetwork(list, Utils.ReadRoutes("A-n64-k9"), 9, capacity, center);
        public void Run()
        {
            LoadNet();
            var currentNet = (DistributionNetwork)InitialNet.Clone();

            TimeTracker.RestartGlobalCrono();
            while (!TimeTracker.ExaustedTotalTime)
            {
                NbhGenerator generator = new NbhGenerator(new[] { typeof(InsertClient), typeof(SwapClients) }, InitialSearchDeph);
                
                var solsVCount = SolsVisited.Count;
                currentNet = Optimize(currentNet, generator, TimeTracker);

                if (solsVCount != SolsVisited.Count)//si cambie de solucion empiezo a buscar por sus vecindades mas cercanas 
                    InitialSearchDeph = 0;

                InitialSearchDeph++;
            }
            BestNet = currentNet;
        }
        public void Run(IList<Type> fcomands){
            LoadNet();
            var currentNet = (DistributionNetwork)InitialNet.Clone();
            
            var nbh = new Neighborhood(fcomands);

            System.Console.Write("Analizing - ");
            Utils.PrintNbh(nbh);

            BestNet = Optimize(currentNet, nbh, TimeTracker);
        }
        
        // Dado una distribucion inicial de clientes, y un generador de vecindades devuelve la mejor distribucion encontrada en un tiempo menor a 'DivingTimeInSeconds'
        public DistributionNetwork Optimize(DistributionNetwork currentNet, NbhGenerator generator, TimeTracker tt)
        {
            tt.RestartDivingCrono();
            foreach (var nbh in generator.GetNeighborhoods())
            {
                if (tt.ExaustedDivingTime || tt.ExaustedTotalTime)
                    break;

                var prevBest = currentNet.TotalDistance;
                System.Console.Write("Analizing - ");
                Utils.PrintNbh(nbh);

                currentNet = nbh.GetBest(currentNet, tt);
                var actualBest = currentNet.TotalDistance;
                
                if(prevBest != actualBest)
                    SolsVisited.Add(new Tuple<Neighborhood, DistributionNetwork>(nbh, currentNet));

                AnalizedComb += nbh.combinaciones_analizadas;

                System.Console.WriteLine(actualBest);
            }
            return currentNet;
        }
 
        // Dado una distribucion inicial de clientes, y un generador de vecindades devuelve la mejor distribucion encontrada en un tiempo menor a 'DivingTimeInSeconds'
        public DistributionNetwork Optimize(DistributionNetwork currentNet, Neighborhood nbh, TimeTracker tt)
        {
            tt.RestartDivingCrono();

            currentNet = nbh.GetBest(currentNet, tt);

            AnalizedComb += nbh.combinaciones_analizadas;
            return currentNet;
        }
 
        
    }
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new Ivnp();

            runner.FileName = "Problems/A-n64-k9.vrp";

            runner.RunOne();

            System.Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}
