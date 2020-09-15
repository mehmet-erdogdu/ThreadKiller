using System;
using System.Diagnostics;
using System.Threading;

namespace ThreadKiller
{
    class Program
    {
        static void Main(string[] args)
        {
            var thread = new InterruptExample();
            int count = 0;
            while (true)
            {
                Process currentProcess = Process.GetCurrentProcess();
                Console.WriteLine("process count: " + currentProcess.Threads.Count);


                count++;
                Console.WriteLine("Main Thread Running: " + count);
                Thread.Sleep(250);
                if (count % 5 == 0)
                {
                    thread.Kill(5);
                    thread = new InterruptExample();
                }
            }
        }

        public class InterruptExample
        {
            public Thread job;

            public InterruptExample()
            {
                job = new Thread(() =>
                {
                    Do();
                });
                job.Start();
            }

            public void Do()
            {
                try
                {
                    var count = 0;
                    while (true)
                    {
                        Thread.Sleep(250);
                        Console.WriteLine("FIRST THREAD running... " + count++);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    /* Clean up. */
                }
            }

            public void Kill(int timeout = 0)
            {
                job.Interrupt();
                job.Join(timeout);
            }
        }

    }
}
