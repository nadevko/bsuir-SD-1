using System.Collections.Concurrent;
using System.Diagnostics;

namespace sd1._10;

internal partial class Program2
{
    static readonly int max = 100_000;

    static void Main()
    {
        var sw = new Stopwatch();
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        Console.WriteLine("Start parallel task...");
        Task.Run(() =>
        {
            Console.ReadKey();
            cts.Cancel();
        });
        try
        {
            sw.Start();
            var task = Task.Run(() => EratostheneParallel(max, token), token);
            Console.WriteLine($"ID: {task.Id}\nStatus: {task.Status}\nPress any key to cancel...");
            task.Wait();
            sw.Stop();
            Console.WriteLine($"Find {task.Result.Count} numbers. Max: {task.Result.Max()}");
        }
        catch (AggregateException exs)
        {
            sw.Stop();
            if (exs.InnerExceptions.Any(e => e is TaskCanceledException))
                Console.WriteLine("Operation cancelled");
            else
                throw;
        }
        Console.WriteLine($"Parallel task completed in {sw.ElapsedMilliseconds} ms");

        Console.WriteLine("Start sequential task...");
        sw.Restart();
        var sequentialPrimes = EratostheneSequential(max);
        sw.Stop();

        Console.WriteLine($"Sequential task completed in {sw.ElapsedMilliseconds} ms");
        Console.WriteLine($"Find {sequentialPrimes.Count} numbers. Max: {sequentialPrimes.Max()}");

        Console.WriteLine($"Start benchmarking...");
        long parallels = 0,
            sequentials = 0;

        int n = 1;
        for (; n <= 10; n++)
        {
            sw.Restart();
            Task.Run(() => EratostheneParallel(max, token), token).Wait();
            sw.Stop();
            Console.WriteLine($"Parallel test #{n}: {sw.ElapsedMilliseconds}");
            parallels += sw.ElapsedMilliseconds;

            sw.Restart();
            EratostheneSequential(max);
            sw.Stop();
            Console.WriteLine($"Sequential test #{n}: {sw.ElapsedMilliseconds}");
            sequentials += sw.ElapsedMilliseconds;
        }
        Console.WriteLine($"Parallel: {parallels / n} ms average");
        Console.WriteLine($"Sequential: {sequentials / n} ms average");
    }

    static List<int> EratostheneParallel(int maxNumber, CancellationToken token)
    {
        var isPrime = new bool[maxNumber + 1];
        Array.Fill(isPrime, true, 2, maxNumber - 1);

        Parallel.For(
            2,
            (int)Math.Sqrt(maxNumber) + 1,
            new ParallelOptions { CancellationToken = token },
            i =>
            {
                if (isPrime[i])
                    for (int j = i * i; j <= maxNumber; j += i)
                    {
                        token.ThrowIfCancellationRequested();
                        isPrime[j] = false;
                    }
            }
        );

        ConcurrentBag<int> primes = [];
        Parallel.For(
            2,
            maxNumber + 1,
            new ParallelOptions { CancellationToken = token },
            i =>
            {
                if (isPrime[i])
                {
                    token.ThrowIfCancellationRequested();
                    primes.Add(i);
                }
            }
        );

        return [.. primes.OrderBy(x => x)];
    }

    static List<int> EratostheneSequential(int maxNumber)
    {
        bool[] isPrime = new bool[maxNumber + 1];
        Array.Fill(isPrime, true, 2, maxNumber - 1);

        isPrime[0] = isPrime[1] = false;

        for (int i = 2; i * i < isPrime.Length; i++)
            if (isPrime[i])
                for (int j = i * i; j < isPrime.Length; j += i)
                    isPrime[j] = false;

        return IsPrimeToPrimes(isPrime);
    }

    static List<int> IsPrimeToPrimes(bool[] isPrime)
    {
        List<int> primes = [];
        for (int i = 2; i < isPrime.Length; i++)
            if (isPrime[i])
                primes.Add(i);

        return primes;
    }
}
