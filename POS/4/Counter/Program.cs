using System;
using System.Diagnostics.Metrics;
using System.Threading;
class Counter
{
    private int id;
    private static int counter = 0;
    private static int count = 0;
    public Counter(int id)
    {
        this.id = id;
    }
    public void WorkerThreadMethod()
    {
        for (int i = 0; i < count; i++)
        {
            counter++;
            if (counter % id == 0)
            {
                Console.WriteLine("ID: {0,3} Counter: {1,8} Modulo: {2}", id, counter, counter % id);
            }
        }
    }

    public static void Main(String[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: SimpleThreadApp <thread_count> <count_to>");
            return;
        }

        int threadCount;
        if (!int.TryParse(args[0], out threadCount) || !int.TryParse(args[1], out count))
        {
            Console.WriteLine("Invalid arguments. Please provide two integers.");
            return;
        }

        for (int i = 0; i < threadCount; i++)
        {
            Counter counterThread = new Counter(i + 2);
            Thread thread = new Thread(new ThreadStart(counterThread.WorkerThreadMethod));
            thread.Start();
        }
    }
}