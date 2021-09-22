using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        private class Test : IEnumerable<int>
        {
            public int Num { get; set; }

            public IEnumerator<int> GetEnumerator()
            {
                for (int i = 0; i < Num; i++)
                {
                    yield return i;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        static void Main(string[] args)
        {
            var bla = new Test();
            bla.Num = 6;

            foreach (var elem in bla)
            {
                if (elem == 4)
                    bla.Num = 10;

                Console.WriteLine(elem);

            }
        }
    }
}
