namespace sd1._12;

internal partial class Program1
{
    static void Main(string[] args)
    {
        var path = Path.GetFullPath(args[0]);

        try
        {
            StreamWriter writer = new(path);
            string? input;

            Console.WriteLine("Enter strings to write to the file");
            Console.WriteLine("To finish input, leave the string empty and press Enter\n");

            while (!string.IsNullOrEmpty(input = Console.ReadLine()))
                writer.WriteLine(input);
            writer.WriteLine();
            writer.Close();
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error: {exception.Message}");
            Environment.Exit(1);
        }

        Console.WriteLine($"File written successfully to {path}");
    }
}
