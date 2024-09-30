namespace sd1._8;

internal partial class Program3
{
    static void Main()
    {
        for (int i = 0; i < 100; i++)
            ThreadPool.QueueUserWorkItem(PrintThreadInfo, i);
        Thread.Sleep(1000);
    }

    static readonly WaitCallback PrintThreadInfo = (index) =>
        Console.WriteLine(
            $"Thread: {Thread.CurrentThread.Name ?? "undefined"}; id: {
            Environment.CurrentManagedThreadId}; index: {index}"
        );
}
