using System;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmarFirstTask;

namespace TestingCApp {
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new Ivns();

            runner.FileName = "Problems/A-n64-k9.vrp";

            runner.Run();

            System.Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}
