namespace sd1._1;

internal class Program3
{
    private static double GetDouble()
    {
        ConsoleKeyInfo input;
        int? fractional = null;
        uint positive = 2;
        double value = 0;
        while ((input = Console.ReadKey()).KeyChar != '\r')
            try
            {
                switch (input.KeyChar)
                {
                    case var i when i is '-' && positive == 2:
                        positive -= 2;
                        break;
                    case var i when char.IsNumber(i):
                        if (fractional != null)
                        {
                            fractional--;
                            value += char.GetNumericValue(i) *
                                     Pow(i, (int)fractional);
                        }
                        else
                        {
                            value *= 10;
                            value += char.GetNumericValue(i);
                        }

                        if (positive == 2) positive = 1;
                        if (value > 100)
                            throw new Exception(
                                "Absolute value is {value}, but should be <= 100");
                        break;
                    case var i when i is '.' or ',' && fractional == null:
                        fractional = 0;
                        if (positive == 2) positive = 1;
                        break;
                    default:
                        throw new Exception(
                            "{input.Key} is not allowed. Pass correct double");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e.Message}");
                Environment.Exit(1);
            }

        if (positive == 0) value *= -1;
        Console.WriteLine();
        return value;
    }

    private static int GetInteger()
    {
        ConsoleKeyInfo input;
        uint positive = 2;
        var value = 0;
        while ((input = Console.ReadKey()).KeyChar != '\r')
            try
            {
                switch (input.KeyChar)
                {
                    case var i when i is '-' && positive == 2:
                        positive -= 2;
                        break;
                    case var i when char.IsNumber(i):
                        value *= 10;
                        value += (int)char.GetNumericValue(i);
                        if (positive == 2) positive--;
                        if (value > 10)
                            throw new Exception(
                                $"Absolute value is {value}, but should be <= 10");
                        break;
                    default:
                        throw new Exception(
                            $"'{input.Key}' is not allowed. Pass correct integer");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n{e.Message}");
                Environment.Exit(1);
            }

        if (positive == 0) value *= -1;
        Console.WriteLine();
        return value;
    }

    private static double Pow(double x, int y)
    {
        double result = 1;
        for (var i = 0; i < Math.Abs(y); i++)
            result *= x;
        return y < 0 ? 1 / result : result;
    }

    private static void Main(string[] args)
    {
        Console.Write("Возведение числа X в Y степень\n\tЧисло X: ");
        var x = GetDouble();
        Console.Write("\tСтепень Y: ");
        var y = GetInteger();
        Console.WriteLine($"Результат: {Pow(x, y)}");
    }
}
