using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public class NbhGenerator
    {
        private readonly IEnumerable<Type> finalCmdChoices;
        private readonly int choicesAmount;

        public NbhGenerator(IEnumerable<Type> finalCmdChoices, int choicesAmount)
        {
            this.finalCmdChoices = finalCmdChoices;
            this.choicesAmount = choicesAmount;
        }

        public IEnumerable<Neighborhood> GetNeighborhoods()
        {/* devuelve todas las vecindades válidas q pueden ser creadas centradas en <center> con
            <choicesAmount> comandos, utilizando <finalCmdChoices> */

            return GetNeighborhoods(choicesAmount, new List<Quarter>());
        }

        private IEnumerable<Neighborhood> GetNeighborhoods(int cmdsLeft, IList<Quarter> quarters)
        {/* crea vecindades válidas, recursivamente, poniendo y quitando un comando de <finalCmdChoices> */

            if (cmdsLeft == 0)
            {// ya está hecha la lista de barrios pa la vecindad nueva
                yield return new Neighborhood(quarters.ToList());
            }
            else
            {
                foreach (var cmdType in this.finalCmdChoices)
                {// por cada comando en IEnumerable<Type> finalCmdChoices

                    FinalCommand finalCmd = Utils.ToFinalCmd(cmdType);

                    Quarter newQuarter;
                    if (Utils.CanCompress(finalCmd))
                    {
                        newQuarter = new Quarter(finalCmd, true);
                        quarters.Add(newQuarter);

                        // recurrencia: tomando el comando y su cadena comprimida
                        foreach (var result in GetNeighborhoods(cmdsLeft - 1, quarters))
                            yield return result;

                        // deshaciendo
                        quarters.Remove(newQuarter);
                    }

                    newQuarter = new Quarter(finalCmd);
                    quarters.Add(newQuarter);

                    // recurrencia: tomando el comando y su cadena
                    foreach (var result in GetNeighborhoods(cmdsLeft - 1, quarters))
                    {
                        yield return result;

                    }

                    // deshaciendo
                    quarters.Remove(newQuarter);
                }
            }
        }

    }
}
