using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplication1
{
    public class ThreadTest4
    {
        static bool done;    // Static fields are shared between all threads

        public ThreadTest4()
        {
            new Thread(Go).Start();
            Go();
        }

        private static void Go()
        {
            if (!done) { Console.WriteLine("Done"); done = true; }
        }
    }
}
