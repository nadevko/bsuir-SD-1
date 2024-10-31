namespace sd1._01;

internal class Program1
{
    private static void Main()
    {
        var money = 0;
        Console.Write("Товар стоит\n\tРублей: ");
        money -= 100 * Convert.ToInt32(Console.ReadLine());
        Console.Write("\tКопеек: ");
        money -= Convert.ToInt32(Console.ReadLine());
        Console.Write("Было оплачено\n\tРублей: ");
        money += 100 * Convert.ToInt32(Console.ReadLine());
        Console.Write("\tКопеек: ");
        money += Convert.ToInt32(Console.ReadLine());
        Console.WriteLine(
            $"{(money < 0 ? "Задолженность" : "Остаток")
            }:\n\tРублей: {Math.Abs(money / 100)
            }\n\tКопеек: {Math.Abs(money % 100)}"
        );
    }
}
