using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public abstract class FinalCommand : Command
    {
        public FinalCommand(ICollection<Type> previousCommands) : base(previousCommands, true)
        {
        }
    }
}
