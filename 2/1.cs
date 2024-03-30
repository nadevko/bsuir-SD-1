using System.Text.RegularExpressions;

namespace sd1._2;

internal class Program
{
    private static bool Validate(string pin) =>
        new Regex(@"^\d{4}(\d{2})?$").IsMatch(pin);

    private static void Main(string[] args)
    {
        Console.Write("Введите строку: ");
        Console.WriteLine(
            Validate(Console.ReadLine() ?? "")
                ? "Это пин-код"
                : "Что-то не то");
    }
}
