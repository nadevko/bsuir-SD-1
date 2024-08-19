namespace sd1._1;

internal class Program2
{
    private static void Main(string[] args)
    {
        Console.Write("Номер дня недели: ");
        try
        {
            switch (Convert.ToInt32(Console.ReadLine()) % 7)
            {
                case 1:
                case -6:
                    Console.WriteLine("Понедельник");
                    break;
                case 2:
                case -5:
                    Console.WriteLine("Вторник");
                    break;
                case 3:
                case -4:
                    Console.WriteLine("Среда");
                    break;
                case 4:
                case -3:
                    Console.WriteLine("Четверг");
                    break;
                case 5:
                case -2:
                    Console.WriteLine("Пятница");
                    break;
                case 6:
                case -1:
                    Console.WriteLine("Суббота");
                    break;
                case 0:
                    Console.WriteLine("Воскресенье");
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
