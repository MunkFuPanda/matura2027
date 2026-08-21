using System.Collections;
using System.Diagnostics.Metrics;

class SimpleThreadApp

    // Zeigt das Fehler bei Threads enstehen können das bei Modulo auf einmal etwas überbleibt was es aber nicht der Fall sein sollte
{
    public int id;

    private static int counter = 0;

    private int count_how_much = 0;
    public SimpleThreadApp(int id)
    {
        this.id = id;
    }
    public void WorkerThreadMethod()
    {
        for (int i = 0; i < count_how_much; i++)
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
        // argumente überprüfen, anzahl ob es zahlen sind

        int thread_count = 0;
        int how_much = 0;

        if (args.Length != 2)
        {
            Console.WriteLine("Please input 2 Arguments");
            return;
        }

        try
        {
            thread_count = int.Parse(args[0]);
            how_much = int.Parse(args[1]);
        }
        catch (Exception e)
        {
            Console.WriteLine("Wrong input");
            return;
        }
        

        List<Thread> threads = new List<Thread>();

        for (int i = 2; i < thread_count + 2; i++)
        {
            SimpleThreadApp app = new SimpleThreadApp(i);
            app.count_how_much = how_much;
            ThreadStart worker = new ThreadStart(app.WorkerThreadMethod);
            Thread thread = new Thread(worker);
            threads.Add(thread);
            //thread.Start();
        }

        for (int i = 0; i < thread_count; i++)
        {
            threads[i].Start();
        }
    }
}