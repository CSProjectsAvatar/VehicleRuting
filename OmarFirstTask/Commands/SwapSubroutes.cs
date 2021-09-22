using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public class SwapSubroutes : FinalCommand
    {
        public SwapSubroutes() : base(new [] { typeof(SelectSubroute), typeof(SelectSubroute) })
        {
        }

        public override IEnumerable<DistributionNetwork> Execute(DistributionNetwork center)
        {
            throw new NotImplementedException();
        }
    }
}