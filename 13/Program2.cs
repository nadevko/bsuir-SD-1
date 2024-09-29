using System.Text;
using System.Text.Json;

namespace sd1._13;

internal partial class Program2
{
    static void Main(string[] args)
    {
        var path = Path.GetFullPath(args[0]);

        try
        {
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
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Error: {exception.Message}");
            Environment.Exit(1);
        }

        Console.WriteLine($"File written successfully to {path}");
    }
}
