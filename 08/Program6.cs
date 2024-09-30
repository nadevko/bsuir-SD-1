namespace sd1._8;

internal partial class Program6
{
    static int times;

    static void Main()
    {
        var timer = new Timer(TimedFunction, times, 0, 2000);
        Console.WriteLine("Press any key to stop the timer...");
        Console.ReadKey();
        timer.Dispose();
        Console.WriteLine("fin.");
    }

    static void TimedFunction(object? state)
    {
        Console.WriteLine(
            $"#{++times:d2} ({Environment.CurrentManagedThreadId}): {DateTime.Now:T}"
        );
    }
}
