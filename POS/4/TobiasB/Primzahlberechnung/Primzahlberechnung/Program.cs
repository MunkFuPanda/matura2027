using System.Diagnostics;
using System.Runtime.CompilerServices;

internal class Program
{

    private static int max = 0;
    private static int maxPrim = 1600000;
    private static int number = 0;
    private static int tests = 0;
    public static List<int> Prims { get; set; } = new List<int>() { 2, 3 };

    static void Main(string[] args)
    {

        Stopwatch watch = new Stopwatch();
        //int maxPrim = 0;
        //int number = 0;
        //int tests = 0;
        //watch.Start();
        //Prim(1600000, out maxPrim, out number, out tests);
        //watch.Stop();
        //Console.WriteLine("Es wurden {0} Primzahlen gefunden", number);
        //Console.WriteLine("Die höchste gefundene Primzahl ist {0}", maxPrim);
        //Console.WriteLine("Die Laufzeit betrug {0:F0} Millisekungen", watch.ElapsedMilliseconds);
        //Console.WriteLine("Es wurden {0} Vergleiche durchgeführt", tests);

        watch.Restart();

        // Thread version

        int max = int.Parse(args[0]);
        int thread_count = int.Parse(args[1]);

        ThreadPool.QueueUserWorkItem(new WaitCallback(Prim), false);
        
    }

    public static void Prim(object obj)
    {
        List<int> prims = new List<int>();
        int i = 5;
        tests = 0;
        prims.Add(2);
        prims.Add(3);
        while (i < max)
        {
            int maxTeiler = (int)Math.Sqrt(i) + 1;
            int j = 0;
            while (true)
            {
                int n = prims[j];
                int rest = (i % n);
                ++tests;
                if (rest == 0)
                    break; //keine Primzahl
                if (n >= maxTeiler)
                {
                    prims.Add(i);
                    break;
                }
                ++j;
            }
            i += 2;
        }
        number = prims.Count;
        maxPrim = prims[number - 1];
 
    }
}

