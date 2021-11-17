using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmarFirstTask
{
    public class SelectSubroute : Command
    {
        public SelectSubroute() : base(new [] { typeof(SelectRoute) }, false)
        {
        }

        public override IEnumerable<DistributionNetwork> Execute(DistributionNetwork center)
        {
            throw new NotImplementedException();
        }
    }
}