namespace sd1._02;

internal class Program
{
    private static bool Validate(string pin) => pin.Length is 4 or 6 && pin.All(char.IsDigit);

    private static void Main()
    {
        Console.Write("Введите пин-код: ");
        Console.WriteLine(Validate(Console.ReadLine() ?? "") ? "Это пин-код" : "Что-то не то");
    }
}
