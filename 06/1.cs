namespace sd1._6;

class Queueue<T>(bool uniq = false) : _5.Queueue<T>(uniq)
    where T : IComparable<T>
{
    public IEnumerable<int> FindAll(Predicate<T> predicat)
    {
        var indexes = new List<int>();
        for (var i = 0; i < Lenght(); i++)
            if (predicat(this[i]))
                indexes.Add(i);
        return indexes.ToArray();
    }

    public Queueue<T> Where(Predicate<T> predicat)
    {
        var q = new Queueue<T>(Uniq);
        foreach (var value in FindAll(predicat))
            q.Push(this[value]);
        return q;
    }

    internal delegate void AddHandler(T value);

    public event AddHandler? AddNotify;

    internal delegate void RemoveHandler(T value);

    public event RemoveHandler? RemoveNotify;

    internal delegate void UpdateHandler(int index, T value);

    public event UpdateHandler? UpdateNotify;

    internal delegate void ChangeHandler();

    public event ChangeHandler? ChangeNotify;

    public new void Push(T element)
    {
        base.Push(element);
        AddNotify?.Invoke(element);
    }

    public new T Pop()
    {
        var element = base.Pop();
        RemoveNotify?.Invoke(element);
        return element;
    }

    public new void Sort(Comparison<T> comparison)
    {
        base.Sort(comparison);
        ChangeNotify?.Invoke();
    }

    public new void Reverse()
    {
        base.Reverse();
        ChangeNotify?.Invoke();
    }

    public new T this[int i]
    {
        get
        {
            if (Lenght() <= i)
                throw new Exception($"Can't access element {i} in queue of lenght {Lenght()}");
            return Container[i];
        }
        set
        {
            if (Lenght() <= i)
                throw new Exception($"Can't access element {i} in queue of lenght {Lenght()}");
            if (Uniq)
                RemoveDublicates();
            UpdateNotify?.Invoke(i, this[i]);
            Container[i] = value;
        }
    }

    public new void ReplaceAll(T from, T to)
    {
        base.ReplaceAll(from, to);
        ChangeNotify?.Invoke();
    }

    public new void RemoveDublicates()
    {
        base.RemoveDublicates();
        ChangeNotify?.Invoke();
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        Test<int>(null);
        Test<string>(null);
    }

    private static int GetLength()
    {
        try
        {
            Console.Write("\tКоличество: ");
            var i = Convert.ToInt32(Console.ReadLine());
            if (i < 2)
                throw new InvalidDataException();
            return i;
        }
        catch
        {
            Console.WriteLine("Укажите число не менее 2");
            return GetLength();
        }
    }

    private static Queueue<T> _try<T>(Func<Queueue<T>, Queueue<T>> func, Queueue<T> queueue)
        where T : IComparable<T>
    {
        try
        {
            return func(queueue);
        }
        catch
        {
            Console.WriteLine("Неверный ввод. Попробуйте снова");
            return Try(func, queueue);
        }
    }

    internal static Queueue<T> Try<T>(Func<Queueue<T>, Queueue<T>> func, Queueue<T>? queueue)
        where T : IComparable<T> => _try(func, (queueue == null) ? new Queueue<T>() : queueue);

    public static Queueue<T> Test<T>(Queueue<T>? q)
        where T : IComparable<T>
    {
        q = new Queueue<T>();
        q.AddNotify += (value) => Console.WriteLine($"Добавлен элемент {value}");
        q.RemoveNotify += (value) => Console.WriteLine($"Удален элемент {value}");
        q.ChangeNotify += () => Console.WriteLine("Чую нешта дрэннае");
        q.UpdateNotify += (i, value) => Console.WriteLine($"Изменен элемент\n\t[{i}] := {value}");
        Try(
            queueue =>
            {
                Console.WriteLine($"({typeof(T)}) добавляем элементы");
                foreach (var i in Enumerable.Range(1, GetLength()))
                {
                    Console.Write($"\t[{i}] := ");
                    queueue.Push((T)Convert.ChangeType(Console.ReadLine() ?? "", typeof(T)));
                }

                return queueue;
            },
            q
        );
        Console.WriteLine(q);

        Try(
            queueue =>
            {
                Console.Write("Найти по индексу: ");
                Console.WriteLine(q[Convert.ToInt32(Console.ReadLine()) - 1]);
                return queueue;
            },
            q
        );
        Console.WriteLine($"Удаляем первый элемент...({q.Pop()})\n{q}");
        Try(
            queueue =>
            {
                Console.Write("Заменяем\n\tвсе: ");
                var from = (T)Convert.ChangeType(Console.ReadLine() ?? "", typeof(T));
                Console.Write("\tна: ");
                var to = (T)Convert.ChangeType(Console.ReadLine() ?? "", typeof(T));
                queueue.ReplaceAll(from, to);
                return queueue;
            },
            q
        );
        Console.WriteLine(q);
        q.RemoveDublicates();
        Console.WriteLine($"Удаляем дубликаты...\n{q}\nНаходим наибольшее...\n{q.Max()}");

        q.Reverse();
        Console.WriteLine($"Обратный порядок: {q}");
        q.Sort(((a, b) => a.CompareTo(b)));
        Console.WriteLine($"Отсортированный порядок: {q}");
        Try(
            queueue =>
            {
                Console.Write("Найти по значению: ");
                var target = (T)Convert.ChangeType(Console.ReadLine() ?? "", typeof(T));
                Console.WriteLine(q.Find(value => value.Equals(target)));
                return queueue;
            },
            q
        );
        Try(
            queueue =>
            {
                Console.Write("Отфильтровать по значениям: ");
                var target = (T)Convert.ChangeType(Console.ReadLine() ?? "", typeof(T));
                Console.WriteLine(q.Where(value => value.Equals(target)));
                return queueue;
            },
            q
        );
        return q;
    }
}
