using System;
using System.Collections.Generic;

namespace OmarFirstTask
{
    public abstract class Command
    {
        protected internal Quarter quarter;

        /* Enumerable con los tipos de los comandos q tienen q estar antes q el comando actual, en
         una sucesión lógica de instrucciones. NO SON TODOS, SOLO LOS Q NECESITA EN UN PASO PREVIO INMEDIATO.
         */
        internal ICollection<Type> PreviousCommands;

        /* Determinan si el comando puede ser inicio o fin de una secuencia lógica de instrucciones,
         respectivamente. */
        internal bool InitialCmd, FinalCmd;

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="previousCommands">A collection containing the commands that must be before
        /// this one, in the neighborhood.</param>
        /// <param name="finalCommand">Determines whether this is a possible final command in a
        /// neighborhood.</param>
        public Command(ICollection<Type> previousCommands, bool finalCommand)
        {
            this.PreviousCommands = previousCommands;

            // es un comando inicial si no tiene comandos previos
            this.InitialCmd = (previousCommands == null || previousCommands.Count == 0);

            this.FinalCmd = finalCommand;
        }

        /// <summary>
        /// Creates a new command. This is, by default, a possible initial command.
        /// </summary>
        /// <param name="finalCommand">Determines whether this is a possible final command in a
        /// neighborhood.</param>
        public Command(bool finalCommand) : this(null, finalCommand) { }

        public void SetQuarter(Quarter quarter)
        {
            this.quarter = quarter;
        }

        /* Ejecuta el comando. Devuelve false si no se puede hacer una nueva red con esta operación
         *  en <sourceNet>. Utiliza las propiedades <AvailableVehicle> y <AvailableClient> para
         *  emitir este criterio.
         */
        //protected abstract bool Execute();
        public abstract IEnumerable<DistributionNetwork> Execute(DistributionNetwork center);
        //protected abstract void Desexecute();

        //public bool Work()
        //{
        //    //Desexecute();
        //    //return Execute();
        //}
    }
}