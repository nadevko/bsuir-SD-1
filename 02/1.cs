using System.Text.RegularExpressions;

namespace sd1._2;

internal partial class Program
{
    private static bool Validate(string pin) =>
        Regex().IsMatch(pin);

    private static void Main(string[] args)
    {
        Console.Write("Введите пин-код: ");
        Console.WriteLine(
            Validate(Console.ReadLine() ?? "")
                ? "Это пин-код"
                : "Что-то не то");
    }

    [GeneratedRegex(@"^\d{4}(\d{2})?$")]
    private static partial Regex Regex();
}
