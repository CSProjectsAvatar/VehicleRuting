using System;
using System.Collections.Generic;

namespace OmarFirstTask
{
    public abstract class RandomCommand: Command
    {
        public int Times { get; set; }
        public Random R { get; set; }
        protected RandomCommand(ICollection<Type> previousCommands, bool finalCommand, int times = 5) : base(previousCommands, finalCommand)
        {
            Times = times;
            R = new Random();
        }

        protected RandomCommand(bool finalCommand, int times = 5) : base(finalCommand)
        {
            Times = times;
            R = new Random();
        }
      
    }
}