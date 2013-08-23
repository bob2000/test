using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApplication1
{
    public class ThreadTest3
    {
        bool done;

        public void Fire()
        {
            ThreadTest3 tt = new ThreadTest3();   // Create a common instance
            new Thread(tt.Go).Start();
            tt.Go();
        }

        // Note that Go is now an instance method
        public void Go()
        {
            if (!done) { done = true; Console.WriteLine("Done"); }
        }
    }
}
