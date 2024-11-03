using System.Collections.Concurrent;
using System.Diagnostics;

namespace sd1._09;

internal class EratostheneParallelException(List<int> Result) : Exception("Operation cancelled.")
{
    public readonly List<int> Result = Result;
}

internal class Program
{
    private static void Main()
    {
        StatusMain();
        ContinuationMain();
        BenchmarkMain();
        CancellableMain(false);
        MainWarehouse();
    }

    private static List<int> Eratosthene(int max)
    {
        var isNotPrime = new bool[++max];
        isNotPrime[0] = isNotPrime[1] = true;
        for (int i = 2; i * i < max; i++)
        {
            if (isNotPrime[i])
                continue;
            for (int j = i * i; j < max; j += i)
                isNotPrime[j] = true;
        }
        List<int> primes = [];
        for (int i = 2; i < max; i++)
            if (!isNotPrime[i])
                primes.Add(i);
        return primes;
    }

    private static List<int> EratostheneParallel(int max)
    {
        var isNotPrime = new bool[++max];
        isNotPrime[0] = isNotPrime[1] = true;
        Parallel.For(
            2,
            (int)Math.Sqrt(max) + 1,
            i =>
            {
                if (isNotPrime[i])
                    return;
                Parallel.For(
                    i * i,
                    max,
                    j =>
                    {
                        if (j % i == 0)
                            isNotPrime[j] = true;
                    }
                );
            }
        );
        return isNotPrime
            .AsParallel()
            .Select((isNotPrime, i) => !isNotPrime ? i : -1)
            .Where(i => i != -1)
            .ToList();
    }

    private static List<int> EratostheneParallel(
        int max,
        CancellationToken ct,
        bool withoutIntermediate = true
    )
    {
        var isNotPrime = new bool[++max];
        isNotPrime[0] = isNotPrime[1] = true;
        try
        {
            Parallel.For(
                2,
                (int)Math.Sqrt(max) + 1,
                new ParallelOptions { CancellationToken = ct },
                i =>
                {
                    if (isNotPrime[i])
                        return;
                    Parallel.For(
                        i * i,
                        max,
                        new ParallelOptions { CancellationToken = ct },
                        j =>
                        {
                            if (j % i == 0)
                                isNotPrime[j] = true;
                        }
                    );
                }
            );
        }
        catch (OperationCanceledException)
        {
            if (withoutIntermediate)
                throw;
        }
        var primes = isNotPrime
            .AsParallel()
            .Select((isNotPrime, i) => !isNotPrime ? i : -1)
            .Where(i => i != -1)
            .ToList();
        if (ct.IsCancellationRequested)
            throw new EratostheneParallelException(primes);
        return primes;
    }

    private static void BenchmarkMain(int n = 10)
    {
        Console.WriteLine("Benchmarking unparallel...");
        object @lock = new();
        var results = new List<TimeSpan>();
        Parallel.Invoke(
            Enumerable.Range(1, n).Select(_ => new Action(() => Bench(results, @lock))).ToArray()
        );
        Console.WriteLine(string.Join("\n", results.Select((ts, i) => $"#{i + 1:D2}: {ts}")));
        Console.WriteLine(
            $"Average elapsed time: {new TimeSpan((long)results.Average(ts => ts.Ticks))}"
        );
    }

    private static void Bench(List<TimeSpan> timeSpans, object @lock, int max = 100_000_000)
    {
        var sw = new Stopwatch();
        sw.Start();
        Eratosthene(max);
        sw.Stop();
        lock (@lock)
            timeSpans.Add(sw.Elapsed);
    }

    private static void CancellableMain(bool withoutIntermediate = true, int max = 100_000_000)
    {
        Console.WriteLine($"Search primes up to {max}");
        var unparallel = Task.Run(() =>
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = Eratosthene(max);
            sw.Stop();
            return Tuple.Create(sw, result);
        });
        Console.WriteLine("Press any key to cancel...");
        var cts = new CancellationTokenSource();
        Task.Run(() =>
        {
            Console.ReadKey(true);
            cts.Cancel();
        });
        var sw = new Stopwatch();
        try
        {
            sw.Start();
            EratostheneParallel(max, cts.Token, withoutIntermediate);
            sw.Stop();
        }
        catch (EratostheneParallelException ex)
        {
            sw.Stop();
            if (!unparallel.IsCompleted)
            {
                Console.WriteLine("Wait for unparallel...");
                unparallel.Wait();
            }
            Console.WriteLine(
                $"{100 * unparallel.Result.Item2.Count / ex.Result.Count}% of result are primes"
            );
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation cancelled.");
        }
        if (!unparallel.IsCompleted)
        {
            Console.WriteLine("Wait for unparallel...");
            unparallel.Wait();
        }
        Console.WriteLine($"Unparallel elapsed time: {unparallel.Result.Item1.Elapsed}");
        Console.WriteLine($"Parallel elapsed time: {sw.Elapsed}");
    }

    private static void StatusMain(int max = 100_000)
    {
        var task = Task.Run(() => EratostheneParallel(max));
        while (!task.IsCompleted)
        {
            Thread.Sleep(3);
            Console.WriteLine(
                $"[{task.Id}]: {task.Status}. IsCompleted: {(task.IsCompleted ? "yes" : "no")}"
            );
        }
    }

    private static void ContinuationMain(int multiplier = 1_000)
    {
        var tasks = Enumerable
            .Range(1, 3)
            .Select(i => Task.Run(() => EratostheneParallel(i * multiplier)))
            .ToList();
        Task.WhenAll(tasks)
            .ContinueWith(t =>
            {
                Console.WriteLine("All tasks are completed. ContinueWith is called.");
                Parallel.ForEach(
                    tasks,
                    task => Console.WriteLine($"[{task.Id}]: {task.Result.Count} primes")
                );
            })
            .Wait();
        tasks = Enumerable
            .Range(1, 3)
            .Select(i => Task.Run(() => EratostheneParallel(i * multiplier)))
            .ToList();
        var results = Task.WhenAll(tasks).GetAwaiter().GetResult();
        Console.WriteLine("All tasks are completed. GetAwaiter().GetResult() is called.");
        Parallel.ForEach(
            tasks,
            task => Console.WriteLine($"[{task.Id}]: {task.Result.Count} primes")
        );
    }

    private static readonly BlockingCollection<string> warehouse = [];

    private static void MainWarehouse()
    {
        List<Task> tasks = [];
        for (int i = 1; i <= 5; i++)
            tasks.Add(Task.Run(() => Supplier(i)));
        for (int i = 1; i <= 10; i++)
            tasks.Add(Task.Run(() => Customer(i)));
        Task.WhenAll(tasks).ContinueWith(t => warehouse.CompleteAdding()).Wait();
        Console.WriteLine("All tasks are completed.");
    }

    private static readonly Random random = new();

    private static void Supplier(int id, string prefix = "product#")
    {
        string product = $"{prefix}{id}";
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(random.Next(500, 1500));
            warehouse.Add(product);
            Console.WriteLine($"Supplier add {product} to warehouse.");
            Console.WriteLine("Warehouse state: " + string.Join(", ", warehouse));
        }
    }

    private static void Customer(int id)
    {
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(random.Next(300, 1000));
            if (warehouse.TryTake(out var product))
                Console.WriteLine($"Customer {id} bought {product}.");
            else
                Console.WriteLine($"Customer {id} arrived, but no products are available.");
        }
    }
}
