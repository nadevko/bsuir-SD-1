using sd1._03;

namespace sd1._04;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Давайте создадим случайное расписание студента");
        Console.WriteLine(StudentRandom.Read());
        Console.WriteLine("А теперь с ручным заданием расписания");
        Console.WriteLine(StudentDefined.Read());
    }
}

internal class StudentWithSchedule(string name, uint id, uint group, Факультэт faculty, uint cource)
    : Student(name, id, group, faculty, cource)
{
    protected Dictionary<TimeOnly, string> Schedule = new();

    protected virtual string GetSchedule()
    {
        throw new Exception("GetSchedule() not defined");
    }

    public override string ToString()
    {
        return $"Расписание для: {Name} с {Cource} курса, {Id} в {Group} группе {Faculty}\n{GetSchedule()}";
    }
}

internal class StudentRandom : StudentWithSchedule
{
    protected override string GetSchedule() =>
        string.Join(
            "\n",
            Schedule.Select(schedule => $"{schedule.Key.ToShortTimeString()} {schedule.Value}")
        );

    protected string[] Events =
    [
        "Хихи",
        "ОАиП",
        "КПО",
        "МА",
        "ФизК",
        "Физика",
        "ЧМ",
        "АиСД",
        "БелЯз",
        "ИнЯз",
    ];

    public StudentRandom(string name, uint id, uint group, Факультэт faculty, uint cource)
        : base(name, id, group, faculty, cource)
    {
        var rand = new Random();
        var time = new TimeOnly().AddMinutes(rand.Next());
        foreach (var i in Enumerable.Range(1, rand.Next(2, 5)))
        {
            time = time.AddMinutes(rand.Next(120, 150));
            Schedule.Add(time, Events[rand.Next(0, Events.Length - 1)]);
        }
    }

    public static new StudentRandom Read()
    {
        try
        {
            Console.Write("Имя: ");
            var name = Console.ReadLine();
            Console.Write("Номер: ");
            var id = Convert.ToUInt32(Console.ReadLine());
            Console.Write("Группа: ");
            var group = Convert.ToUInt32(Console.ReadLine());
            Console.Write("Факультет: ");
            var faculty = (Факультэт)Enum.Parse(typeof(Факультэт), Console.ReadLine()!);
            Console.Write("Курс: ");
            var cource = Convert.ToUInt32(Console.ReadLine());
            return new StudentRandom(name ?? "Неизвестный", id, group, faculty, cource);
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc + "\nПопробуйте снова");
            return Read();
        }
    }
}

internal class StudentDefined(string name, uint id, uint group, Факультэт faculty, uint cource)
    : StudentWithSchedule(name, id, group, faculty, cource)
{
    public static new StudentDefined Read()
    {
        try
        {
            Console.Write("Имя: ");
            var name = Console.ReadLine();
            Console.Write("Номер: ");
            var id = Convert.ToUInt32(Console.ReadLine());
            Console.Write("Группа: ");
            var group = Convert.ToUInt32(Console.ReadLine());
            Console.Write("Факультет: ");
            var faculty = (Факультэт)Enum.Parse(typeof(Факультэт), Console.ReadLine()!);
            Console.Write("Курс: ");
            var cource = Convert.ToUInt32(Console.ReadLine());
            return Read(new StudentDefined(name ?? "Неизвестный", id, group, faculty, cource));
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message + "\nПопробуйте снова");
            return Read();
        }
    }

    protected static TimeOnly[] Time =
    [
        new TimeOnly(9, 0),
        new TimeOnly(10, 35),
        new TimeOnly(12, 25),
        new TimeOnly(14, 0),
        new TimeOnly(15, 50),
    ];

    protected override string GetSchedule() =>
        string.Join(
            "\n",
            Schedule
                .Where(couple => couple.Value != string.Empty)
                .Select(couple => $"{couple.Key.ToShortTimeString()} {couple.Value}")
        );

    public static StudentDefined Read(StudentDefined student)
    {
        try
        {
            Console.WriteLine("Расписание");
            foreach (var i in Enumerable.Range(student.Schedule.Count, Time.Length))
            {
                Console.Write($"\t{i + 1}-ая пара: ");
                student.Schedule.Add(Time[i], Console.ReadLine() ?? string.Empty);
            }
            return student;
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message + "\nПопробуйте снова");
            return Read(student);
        }
    }
}
