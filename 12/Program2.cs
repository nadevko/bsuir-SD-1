using System.Text;
using System.Text.Json;

namespace sd1._12;

internal class Program2
{
    static void Main(string[] args)
    {
        try
        {
            var path = Path.GetFullPath(args[0]);
            string? input;
            var sb = new StringBuilder();
            Console.WriteLine("Enter strings to write to the file");
            Console.WriteLine("To finish input, leave the string empty and press Enter\n");
            while (!string.IsNullOrEmpty(input = Console.ReadLine()))
                sb.AppendLine(input);
            var data = JsonDocument.Parse(sb.ToString());
            File.WriteAllText(
                path,
                JsonSerializer.Serialize(data, new JsonSerializerOptions() { WriteIndented = true })
            );
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
