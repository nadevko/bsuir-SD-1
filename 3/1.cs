namespace sd1._3;

internal class Program
{
    private static University _university = null!;

    private static void Main(string[] args)
    {
        Console.WriteLine("Поздравляем с вступлением в должность ректора!\nДавайте заполним университет");
        _university = new University();
        Console.WriteLine("Для просмотра списка приказов напишите \"помощь\"");
        while (true)
        {
            Console.Write(">> ");
            switch (Console.ReadLine())
            {
                case "инспекция":
                    Console.WriteLine(_university);
                    break;
                case "набрать":
                    _university.Fill();
                    break;
                case "зачислить":
                    _university.Add();
                    break;
                case "отчислить":
                    Console.Write(">> ");
                    _university.Remove(
                        Convert.ToUInt32(Console.ReadLine())
                        );
                    Console.WriteLine();
                    break;
                case "сессия":
                    var oldLenght = _university.Lenght();
                    _university.Update();
                    Console.WriteLine(
                        $"Окончивших университет: {oldLenght - _university.Lenght()}");
                    break;
                case "бабах":
                    Console.WriteLine(
                        "Вы взорвали университет\nПлохое решение, но не мне судить\nСтроим новый...");
                    _university = new University();
                    break;
                case "уволиться":
                    Console.WriteLine("Вы покинули свою должность");
                    return;
                default:
                    Console.WriteLine(
                        "помощь\t\tОткрыть это руководство\n" +
                        "инспекция\tПросмотреть список студентов\n" +
                        "набрать\t\tОбъявить новый набор студентов\n" +
                        "зачислить\tЗачислить нового студента\n" +
                        "отчислить\tОтчислить студента\n" +
                        "сессия\t\tПовысить студентов на 1 курс и отчислить 4-курсников\n" +
                        "бабах\t\tОтчислить всех (перезапуск)\n" +
                        "уволиться\tПокинуть должность (выход)"
                    );
                    break;
            }
        }
    }
}

// хехе, cyrillic
internal enum Факультет
{
    ФКП,
    ФИТУ,
    ФРЭ,
    ФКСиС,
    ФИБ,
    ИЭФ,
    ВФ
}

internal class Student
{
    public string Name;
    public uint Id;
    public uint Group;
    public Факультет Faculty;
    public uint Cource;

    protected Student(string name, uint id, uint group, Факультет faculty,
        uint cource) =>
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
            var faculty =
                (Факультет)Enum.Parse(typeof(Факультет), Console.ReadLine()!);
            Console.Write("Курс: ");
            var cource = Convert.ToUInt32(Console.ReadLine());
            return new Student(name ?? "Неизвестный", id, group, faculty,
                cource);
        }
        catch (Exception exc)
        {
            Console.WriteLine(exc.Message + "\nПопробуйте снова");
            return Read();
        }
    }
}

internal class University
{
    private Student[] _students;

    public University()
    {
        _students = Gen();
    }

    public Student[] Gen(int offset = 0)
    {
        Student[] students = [];
        Console.Write("Количество студентов\n>> ");
        var n = Convert.ToInt32(Console.ReadLine());
        for (var i = 1 + offset; i <= n + offset; i++)
        {
            Console.WriteLine($"Студент №{i}");
            students = students.Append(Student.Read()).ToArray();
        }

        return students.ToArray();
    }

    public void Fill()
    {
        _students = _students.Concat(Gen(_students.Length)).ToArray();
    }

    public override string ToString() =>
        string.Join("\n",
            _students.Select(student => student.ToString()));

    public void Remove(uint n)
    {
        _students = _students.Where((_, i) => i != n - 1).ToArray();
        Console.Write($"Студент №{n} был отчислен");
    }

    public void Add()
    {
        _students = _students.Append(Student.Read()).ToArray();
        Console.WriteLine($"Студент №{Lenght()} успешно добавлен");
    }

    public void Update() =>
        _students = _students.Where(student => student.Cource < 4).Select(
            student =>
            {
                student.Cource += 1;
                return student;
            }).ToArray();

    public int Lenght() =>
        _students.Length;
}
