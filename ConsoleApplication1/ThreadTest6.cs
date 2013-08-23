using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplication1
{
    public class ThreadTest6
    {
        public ThreadTest6()
        {
            Thread t = new Thread(Go);
            t.Start();
            //t.Join();
            Console.WriteLine("Thread t has ended!");
        }

        private static void Go()
        {
            for (int i = 0; i < 1000; i++) Console.Write("y");
        }
    }
}
