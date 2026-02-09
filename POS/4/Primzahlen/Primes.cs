
namespace PrimeNumbers
{
    class Primes
    {
        public int id;
        private static int[] progress = new int[Environment.ProcessorCount];
        public static int MaxValue;
        public static int MaxPrime;
        public static List<int> primes = new List<int>();


        public Primes(int id)
        {
            this.id = id;
        }

        public void WorkerThreadMethod()
        {
            primes.Add(2);
            primes.Add(3);

            int localStart = 3;
            if (localStart % 2 == 0) localStart++;

            for (int i = localStart; i <= MaxValue; i += 2)
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
}
