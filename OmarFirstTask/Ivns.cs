using OmarFirstTask.Commands;
using System;
using System.Collections.Generic;

namespace OmarFirstTask {
    public class Ivns
    {
        public Ivns(long totalTimeInSeconds = 60, long divingTimeInSeconds = 30)
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
                NbhGenerator generator = new NbhGenerator(
                    new[] { typeof(InsertSubrouteRandmonly), typeof(InsertClient), typeof(SwapClients),  }, 
                    InitialSearchDeph);
                
                var solsVCount = SolsVisited.Count;
                currentNet = Optimize(currentNet, generator, TimeTracker);

                if (solsVCount != SolsVisited.Count)//si cambie de solucion empiezo a buscar por sus vecindades mas cercanas 
                    InitialSearchDeph = 0;

                InitialSearchDeph++;
            }
            BestNet = currentNet;
        }

        public RunOutcome Run(IList<Type> initNbh, uint times) {
            return RepeatRunning(times, () => Run(initNbh));
        }

        /// <summary>
        /// Repeat the given action the given number of times.
        /// </summary>
        /// <param name="times"></param>
        /// <param name="runAction">After finished, must set <see cref="BestNet"/>.</param>
        /// <returns></returns>
        private RunOutcome RepeatRunning(uint times, Action runAction) {
            double minSol = double.MaxValue,
                maxSol = double.MinValue,
                sumSol = 0;

            var sols = new double[times];

            for (int i = 0; i < times; i++) {
                Console.WriteLine($"Corrida #{i + 1}:\n");

                runAction();

                Console.WriteLine();

                var sol = BestNet.TotalDistance;
                minSol = Math.Min(minSol, sol);
                maxSol = Math.Max(maxSol, sol);
                sumSol += sol;

                sols[i] = sol;
            }
            return new RunOutcome {
                MinSolution = minSol,
                MaxSolution = maxSol,
                MeanSolution = sumSol / times,
                Solutions = sols
            };
        }

        public RunOutcome Run(uint times) {
            return RepeatRunning(times, () => Run());
        }

        public void Run(IList<Type> fcomands){
            LoadNet();
            var currentNet = (DistributionNetwork)InitialNet.Clone();
            
            var nbh = new Neighborhood(fcomands);

            System.Console.Write("Analizando - ");
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
                System.Console.Write("Analizando - ");
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

        public class RunOutcome {
            public double MinSolution { get; internal set; }
            public double MaxSolution { get; internal set; }
            public double MeanSolution { get; internal set; }
            public double[] Solutions { get; internal set; }
        }
    }
}
