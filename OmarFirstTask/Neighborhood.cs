﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    /* Criterio q define los cambios a realizar en una red para obtener otras redes, las cuales
     son "vecinas" de la primera. */
    public class Neighborhood
    {
        public int combinaciones_analizadas = 0;//Esto se puede quitar cuando se desee
        public readonly IList<Quarter> Quarters;

        public Neighborhood(IList<Quarter> quarters)
        {
            this.Quarters = quarters;
        }

        public Neighborhood(IList<Type> finalCmdTypes) : this(MakeQuarters(finalCmdTypes))
        {

        }

        public Neighborhood()
        {
            this.Quarters = null;
        }

        private static IList<Quarter> MakeQuarters(IList<Type> finalCmdTypes)
        {
            List<Quarter> result = new List<Quarter>();
            foreach (var type in finalCmdTypes)
            {
                result.Add(new Quarter(Utils.ToFinalCmd(type)));
            }

            return result;
        }


        public IEnumerable<DistributionNetwork> GetNeighbors(DistributionNetwork center)
        {/* obtiene todas las redes q se pueden generar aplicando esta vecindad, centrada en <center> */

            return GetNeighbors(0, center);
        }

        private IEnumerable<DistributionNetwork> GetNeighbors(int quarterIdx, DistributionNetwork net)
        {
            if (quarterIdx == Quarters.Count)
            //ya pasé x todos los barrios?
            {
                //retorno la red final
                yield return net;
            }
            else
            {
                var actualQuarter = Quarters[quarterIdx];

                foreach (var neighbor in actualQuarter.GetNeighbors(net))
                {/* x cada vecino q resulte de aplicar el barrio <actualQuarter> a <net> */
                    foreach (var result in GetNeighbors(quarterIdx + 1, neighbor))
                    {/* x cada resultado del subárbol */

                        yield return result;
                    }
                }
            }
        }
        public DistributionNetwork GetBest(DistributionNetwork center)
        {
            DistributionNetwork best = center;

            foreach (var neigh in GetNeighbors((DistributionNetwork)best.Clone()))
            {
                combinaciones_analizadas++;
                if (neigh.TotalDistance < best.TotalDistance)
                {
                    best = (DistributionNetwork)neigh.Clone();
                    Console.WriteLine(best + "\n" + best.TotalDistance);

                    //Utils.PrintNet(best, best.Center.Point);
                    Console.WriteLine("Combinaciones analizadas en Vecindad: " + combinaciones_analizadas);
                    Console.WriteLine("//////////////////////////////");
                }
            }
            return best;
        }
    }
}