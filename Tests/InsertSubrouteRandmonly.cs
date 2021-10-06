using Microsoft.VisualStudio.TestTools.UnitTesting;
using OmarFirstTask;

namespace Tests {
    [TestClass]
    public class InsertSubrouteRandmonly {
        private DistributionNetwork _net;
        private NbhGenerator _nbhGen;
        private Ivns _runner;

        [TestInitialize]
        public void Init() {
            var center = new Client(1, default, default);
            _net = new DistributionNetwork(
                new[] { 
                    center,
                    new Client(2, default, default),
                    new Client(3, default, default),
                    new Client(4, default, default),
                    new Client(5, default, default),
                    new Client(6, default, default),
                    new Client(7, default, default),
                    new Client(8, default, default),
                    new Client(9, default, default),
                    new Client(10, default, default),
                },
                35,
                center
            );
            _nbhGen = new NbhGenerator(new[] { typeof(InsertClient), typeof(SwapClients), typeof(OmarFirstTask.Commands.InsertSubrouteRandmonly) }, 1);
            _runner = new Ivns(3000, 3000);
        }

        [TestMethod]
        public void Main() {
            _runner.Optimize(_net, _nbhGen, _runner.TimeTracker);

            Assert.IsTrue(true);
        }
    }
}
