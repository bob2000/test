using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplication1
{
    public class ThreadTest2
    {
        public ThreadTest2()
        {
            new Thread(Go).Start();      // Call Go() on a new thread
            Go();   
        }

        private static void Go()
        {
            // Declare and use a local variable - 'cycles'
            for (int cycles = 0; cycles < 5; cycles++) Console.Write('?');

            Console.Write(' ');
        }
    }
}
