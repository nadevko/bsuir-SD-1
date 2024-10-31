namespace sd1._08;

internal partial class Program
{
    private static int counter;
    private static readonly object @lock = new();
    private static bool stop = false;

    static void Main()
    {
        Thread t1 = new(IncrementCounter) { Name = "Thread 1" };
        Thread t2 = new(IncrementCounter) { Name = "Thread 2" };

        t1.Start();
        t2.Start();

        Thread.Sleep(38);
        stop = true;

        t1.Join();
        t2.Join();

        Console.WriteLine("Final value: " + counter);
    }

    static void IncrementCounter()
    {
        while (true)
            lock (@lock)
                if (stop)
                    break;
                else
                    Console.WriteLine($"{Thread.CurrentThread.Name}: {++counter}");
    }
}
