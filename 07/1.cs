using sd1._3;

namespace sd1._7;

interface ISelfNamed<T>
{
    void WriteClass();

    string ThisName
    {
        get => typeof(T).ToString();
        set => throw new NotImplementedException();
    }

    void EmanSiht();
}

class University<T> : _3.University<T>, ISelfNamed<University<T>>
    where T : Student
{
    public override string ToString()
    {
        EmanSiht();
        return string.Join(
            ",\n",
            Students.Select(student => student.ToString()).Prepend($"[{ThisName}]")
        );
    }

    public void WriteClass() => Console.WriteLine(this);

    public string ThisName { get; set; } = "7/Student";

    public void EmanSiht() => ThisName = new string(ThisName.Reverse().ToArray());

    protected override T ReadStudent() => (T)Student.Read();

    public List<T> ToList() => Students.ToList();
}

class Student(string name, uint id, uint group, Факультет faculty, uint cource)
    : _3.Student(name, id, group, faculty, cource),
        ISelfNamed<Student>
{
    public override string ToString() =>
        $"{{\n\t{string.Join("\n\t",
        typeof(Student).GetFields().Select(field => $"{field.Name}: {field.GetValue(this)}"))}\n}}";

    public void WriteClass() => Console.WriteLine(this);

    public string ThisName { get; set; } = "7/Student";

    public void EmanSiht() => ThisName = new string(ThisName.Reverse().ToArray());

    public static new Student Read()
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
            var faculty = (Факультет)Enum.Parse(typeof(Факультет), Console.ReadLine()!);
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

internal class Iterator<T> : ISelfNamed<Iterator<T>>
{
    private List<Student> _collection;
    private bool _isReversed;

    public Iterator(List<Student> collection, bool isReversed = false) =>
        (_collection, _isReversed) = (collection, isReversed);

    public override string ToString()
    {
        if (_isReversed)
            EmanSiht();
        return string.Join(
            ",\n",
            _collection
                .Select(student =>
                {
                    if (_isReversed)
                        student.EmanSiht();
                    return student.ThisName + student;
                })
                .Prepend($"[{ThisName}]")
        );
    }

    public void WriteClass() => Console.WriteLine(this);

    public string ThisName { get; set; } = "Iterator";

    public void EmanSiht()
    {
        var chars = ThisName.ToCharArray();
        for (int i = 0, len = ThisName.Length - 1; i < len; i++, len--)
        {
            chars[i] ^= chars[len];
            chars[len] ^= chars[i];
            chars[i] ^= chars[len];
        }

        ThisName = new string(chars);
    }
}

internal class Program : _3.Program
{
    public new University<Student> University = null!;

    private static void Main(string[] args)
    {
        Console.WriteLine(
            "Поздравляем с вступлением в должность ректора!\nДавайте заполним университет"
        );
        var program = new Program();
        program.University = new University<Student>();
        program.University.WriteClass();
        Console.WriteLine(
            "\nА тепер это будет...\n"
                + "Научный Итеративный Институт \x1b[9mимени\x1b[0m неймспейса sd1._3\n"
                + new Iterator<Student>(program.University.ToList())
                + "\n3_.1ds асйепсмйен \x1b[9mинеми\x1b[0m Тутитсни Йынвитарети Йынчуан\n"
                + new Iterator<Student>(program.University.ToList(), true)
        );
    }
}
