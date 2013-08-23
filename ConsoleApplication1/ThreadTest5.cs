using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplication1
{
    public class ThreadTest5
    {
        static bool done;    // Static fields are shared between all threads
        static readonly object locker = new object();

        public ThreadTest5()
        {
            new Thread(Go).Start();
            Go();
        }

        private static void Go()
        {
            lock (locker)
            {
                if (!done) { Console.WriteLine("Done"); done = true; }
            }
        }
    }
}
