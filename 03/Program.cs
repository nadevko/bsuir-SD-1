namespace sd1._03;

internal class Program
{
    protected University<Student> University = null!;

    public void GameLoop()
    {
        Console.Write(">> ");
        switch (Console.ReadLine())
        {
            case "инспекция":
                Console.WriteLine(University);
                break;
            case "набрать":
                University.Fill();
                break;
            case "зачислить":
                University.Add();
                break;
            case "отчислить":
                Console.Write(">> ");
                University.Remove(Convert.ToUInt32(Console.ReadLine()));
                Console.WriteLine();
                break;
            case "сессия":
                var oldLenght = University.Lenght();
                University.Update();
                Console.WriteLine($"Окончивших университет: {oldLenght - University.Lenght()}");
                break;
            case "бабах":
                Console.WriteLine(
                    "Вы взорвали университет\nПлохое решение, но не мне судить\nСтроим новый..."
                );
                University = new University<Student>();
                break;
            case "уволиться":
                Console.WriteLine("Вы покинули свою должность");
                return;
            default:
                Console.WriteLine(
                    "помощь\t\tОткрыть это руководство\n"
                        + "инспекция\tПросмотреть список студентов\n"
                        + "набрать\t\tОбъявить новый набор студентов\n"
                        + "зачислить\tЗачислить нового студента\n"
                        + "отчислить\tОтчислить студента\n"
                        + "сессия\t\tПовысить студентов на 1 курс и отчислить 4-курсников\n"
                        + "бабах\t\tОтчислить всех (перезапуск)\n"
                        + "уволиться\tПокинуть должность (выход)"
                );
                break;
        }
        GameLoop();
    }

    private static void Main()
    {
        Console.WriteLine(
            "Поздравляем с вступлением в должность ректора!\nДавайте заполним университет"
        );
        Program program = new() { University = new University<Student>() };
        Console.WriteLine("Для просмотра списка приказов напишите \"помощь\"");
        program.GameLoop();
    }
}

// хехе, cyrillic
internal enum Факультэт
{
    ФКП,
    ФИТУ,
    ФРЭ,
    ФКСиС,
    ФИБ,
    ИЭФ,
    ВФ,
}

internal class Student
{
    public string Name;
    public uint Id;
    public uint Group;
    public Факультэт Faculty;
    public uint Cource;

    protected Student(string name, uint id, uint group, Факультэт faculty, uint cource) =>
        (Name, Id, Group, Faculty, Cource) = (name, id, group, faculty, cource);

    public override string ToString() =>
        $"{Name}: {Cource}-курсник, {Id} в {Group} группе {Faculty}";

    public static Student Read()
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
            return new Student(name ?? "Неизвестный", id, group, faculty, cource);
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message + "\nПопробуйте снова");
            return Read();
        }
    }
}

internal class University<T>
    where T : Student
{
    protected T[] Students;

    public University() => Students = Gen();

    protected virtual T ReadStudent() => (T)Student.Read();

    public T[] Gen(int offset = 0)
    {
        T[] students = [];
        Console.Write("Количество студентов\n>> ");
        var n = Convert.ToInt32(Console.ReadLine());
        for (var i = 1 + offset; i <= n + offset; i++)
        {
            Console.WriteLine($"Студент №{i}");
            students = students.Append(ReadStudent()).ToArray();
        }
        return [.. students];
    }

    public void Fill() => Students = Students.Concat(Gen(Students.Length)).ToArray();

    public override string ToString() =>
        string.Join("\n", Students.Select(student => student.ToString()));

    public void Remove(uint n)
    {
        Students = Students.Where((_, i) => i != n - 1).ToArray();
        Console.Write($"Студент №{n} был отчислен");
    }

    public void Add()
    {
        Students = (T[])Students.Append(Student.Read()).ToArray();
        Console.WriteLine($"Студент №{Lenght()} успешно добавлен");
    }

    public void Update() =>
        Students = Students
            .Where(student => student.Cource < 4)
            .Select(student =>
            {
                student.Cource += 1;
                return student;
            })
            .ToArray();

    public int Lenght() => Students.Length;
}
