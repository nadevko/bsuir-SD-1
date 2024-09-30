namespace sd1._8;

internal partial class Program4
{
    static readonly int N = 10;

    static void Main()
    {
        var threads = new Thread[N];
        for (int i = 0; i < N; i++)
            (threads[i] = new Thread(Factorial)).Start();
        foreach (var thread in threads)
            thread.Join();
    }

    static void Factorial()
    {
        int number = new Random().Next(1, 11);
        Console.WriteLine(
            $"{Environment.CurrentManagedThreadId:d2}: {number}! = {Factorial(number)}"
        );
    }

    static int Factorial(int n) => (n <= 1) ? 1 : n * Factorial(n - 1);
}
