namespace sd1._8;

internal partial class Program1
{
    static void Main()
    {
        Thread numbers = new(() => PrintRange(1, 10));
        Thread letters = new(() => PrintRange('A', 'J'));

        numbers.Start();
        letters.Start();

        numbers.Join();
        letters.Join();
    }

    static void PrintRange(dynamic current, dynamic finish)
    {
        while (current.CompareTo(finish) <= 0)
        {
            Console.WriteLine(current++);
            Thread.Sleep(1000);
        }
    }
}
