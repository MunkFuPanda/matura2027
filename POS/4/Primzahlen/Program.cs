using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

class PrimeWorker
{
    private int id;
    private int start;
    private int end;
    private List<int> primes;

    public List<int> Primes => primes;

    public PrimeWorker(int id, int start, int end)
    {
        this.id = id;
        this.start = start;
        this.end = end;
        this.primes = new List<int>();
    }

    public void WorkerThreadMethod()
    {
        // Kleine Optimierung: Gerade Zahlen ignorieren
        if (start <= 2 && end >= 2)
            primes.Add(2);

        int localStart = Math.Max(3, start);
        if (localStart % 2 == 0) localStart++;

        for (int i = localStart; i <= end; i += 2)
        {
            bool isPrime = true;
            int limit = (int)Math.Sqrt(i);
            for (int j = 2; j <= limit; j++)
            {
                if (i % j == 0) { isPrime = false; break; }
            }
            if (isPrime) primes.Add(i);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        int threadCount = 10;
        int maxValue = 1600000;

        Console.WriteLine($"Starte Primzahlsuche bis {maxValue} mit {threadCount} Threads...");

        Stopwatch sw = Stopwatch.StartNew();

        // Bereiche für Threads aufteilen
        int chunk = maxValue / threadCount;
        Thread[] threads = new Thread[threadCount];
        PrimeWorker[] workers = new PrimeWorker[threadCount];

        for (int i = 0; i < threadCount; i++)
        {
            int start = i * chunk + 1;
            int end = (i == threadCount - 1) ? maxValue : (i + 1) * chunk;
            workers[i] = new PrimeWorker(i + 1, start, end);
            threads[i] = new Thread(new ThreadStart(workers[i].WorkerThreadMethod));
            threads[i].Start();
        }

        // Auf alle Threads warten
        for (int i = 0; i < threadCount; i++)
        {
            threads[i].Join();
        }

        sw.Stop();

        // Ergebnisse zusammenführen
        var allPrimes = workers.SelectMany(w => w.Primes).Distinct().OrderBy(x => x).ToList();

        Console.WriteLine($"Gefundene Primzahlen: {allPrimes.Count}");
        Console.WriteLine($"Höchste Primzahl: {allPrimes.Last()}");
        Console.WriteLine($"Laufzeit: {sw.ElapsedMilliseconds} ms");
    }
}
