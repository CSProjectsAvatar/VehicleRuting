using System;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmarFirstTask;
using System.Collections.Generic;

namespace TestingCApp {
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new Ivns(400, 300);

            runner.FileName = "Problems/A-n64-k9.vrp";
            
            var commands = new List<Type>(){typeof(InsertClient)};
            runner.Run(commands);

            System.Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }
}
