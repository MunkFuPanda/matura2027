using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

internal class Primthread
{
    // id 0 1 2 3 4
    // i = 5 + id * 2
    // i += 2 mal thread anzahl

    // thread join in seperater schleife (foreach) und die generierten threads in einer liste zwischenspeichern


    public int id;

    private static int thread_count;



    public int M { get => max; set { max = value; } }
    public int MP { get => maxPrim; }
    public int N { get => number; }
    public int T { get => tests; }

    public int TC { get => thread_count; set { thread_count = value; } }

    public Primthread(int id)
    {
        this.id = id;
    }

    public void Prim()
    {
        List<int> prims = // Prims Eigenschaft hier einfügen
        int i = 5;SocketShutdown -Half
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