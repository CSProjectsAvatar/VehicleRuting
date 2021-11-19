using System;
using System.Collections.Generic;

namespace OmarFirstTask
{
    public static class RandomCommand
    {
        public static int Times { get; } = 4;
        public static Random R { get; } = new Random(Environment.TickCount);
      
    }
}