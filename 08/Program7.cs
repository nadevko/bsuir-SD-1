namespace sd1._8;

internal partial class Program7
{
    static readonly int N = 10;
    static readonly Stack<string> errors =
        new(
            [
                "Шось мацнулі і ўсё... Знікла",
                "Галя, адмена!",
                "Прабачце, праграмісты зараз збіраюць бульбу",
                "сегментайшнфаўглтгвлт",
                "ААААААААААА! У нас стэк не локнуты)",
            ]
        );

    static void Main()
    {
        var threads = new Thread[N];

        for (int i = 0; i < threads.Length; i++)
            (threads[i] = new Thread(() => ExeptiveFunction(i))).Start();

        foreach (var thread in threads)
            thread.Join();

        Console.WriteLine("All threads have finished");
    }

    static void ExeptiveFunction(int i)
    {
        try
        {
            Console.WriteLine($"Thread {i} started");
            Thread.Sleep(500);
            if (errors.Count > 0)
                throw new Exception(errors.Pop());
            Console.WriteLine($"Thread {i} finished");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in thread {i}: {ex.Message}");
        }
    }
}
