namespace sd1._12;

internal class Program1
{
    static void Main(string[] args)
    {
        try
        {
            var path = Path.GetFullPath(args[0]);
            StreamWriter writer = new(path);
            string? input;

            Console.WriteLine("Enter strings to write to the file");
            Console.WriteLine("To finish input, leave the string empty and press Enter\n");

            while (!string.IsNullOrEmpty(input = Console.ReadLine()))
                writer.WriteLine(input);
            writer.Close();
            Console.WriteLine($"File written successfully to {path}");
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("Error: No file path provided");
            Environment.Exit(1);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error: {exception.Message}");
            Environment.Exit(1);
        }
    }
}
