namespace sd1._8;

internal partial class Program5
{
    static string source = string.Empty;
    static string destination = string.Empty;
    static string data = string.Empty;
    static readonly object @lock = new();

    static void Main(string[] args)
    {
        if (args.Length < 2)
            throw new ArgumentException("Invalid arguments. Expected: <source> <destination>");

        source = args[0];
        destination = args[1];

        if (!File.Exists(source))
            throw new FileNotFoundException($"File not found: {source}");

        Thread readThread = new(ReadFromFile);
        Thread writeThread = new(WriteToFile);

        readThread.Start();
        writeThread.Start();

        readThread.Join();
        writeThread.Join();
    }

    static void ReadFromFile()
    {
        lock (@lock)
            data = new StreamReader(source).ReadToEnd();
        Console.WriteLine($"Read {data.Length} characters from {source}");
    }

    static void WriteToFile()
    {
        Thread.Sleep(1000);
        lock (@lock)
        {
            StreamWriter writer = new(destination, false);
            writer.Write(data);
            writer.Close();
        }
        Console.WriteLine($"Wrote {data.Length} characters to {destination}");
    }
}
