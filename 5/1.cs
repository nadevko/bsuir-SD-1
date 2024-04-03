using System.Collections;

namespace sd1._5;

internal class Queueue<T> : IEnumerable<T> where T : IComparable<T>
{
    protected List<T> Container = [];
    protected bool Uniq;

    public Queueue(bool uniq = false) => Uniq = uniq;

    public void Push(T element)
    {
        Container.Add(element);
        if (Uniq) RemoveDublicates();
    }

    public T Pop()
    {
        if (Lenght() == 0) throw new Exception("Queue is empty");
        var element = Container[0];
        Container.RemoveAt(0);
        return element;
    }

    public T? Find(Predicate<T> predicat)
    {
        foreach (var i in Container)
            if (predicat(i))
                return i;
        return default;
    }

    public void Sort(Comparison<T> comparison) => Container.Sort(comparison);

    public void Reverse() => Container.Reverse();

    public T this[int i]
    {
        get
        {
            if (Lenght() <= i)
                throw new Exception(
                    $"Can't access element {i} in queue of lenght {Lenght()}");
            return Container[i];
        }
        set
        {
            if (Lenght() <= i)
                throw new Exception(
                    $"Can't access element {i} in queue of lenght {Lenght()}");
            if (Uniq) RemoveDublicates();
            Container[i] = value;
        }
    }

    public void ReplaceAll(T from, T to) =>
        Container = Container
            .Select(element => Equals(element, from) ? to : element)
            .ToList();

    public T Max() => Container.Max()!;

    public void RemoveDublicates() =>
        Container = Container.Aggregate(new List<T>(), (list, item) =>
        {
            if (!list.Contains(item)) list.Add(item);
            return list;
        });

    public int Lenght() => Container.Count;

    public IEnumerator<T> GetEnumerator() => new QueueueEnumerator<T>(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString() => string.Join(", ", Container);

    public List<T> ToList() => Container;

    public Queueue<T> Clone() =>
        this.MemberwiseClone() as Queueue<T> ?? new Queueue<T>();
}

internal class QueueueEnumerator<T> : IEnumerator<T> where T : IComparable<T>
{
    private readonly Queueue<T> _queue;
    private int _index = -1;

    public QueueueEnumerator(Queueue<T> queue) => _queue = queue;

    public bool MoveNext()
    {
        var newIndex = _index + 1;
        var success = newIndex < _queue.Lenght();
        if (success) _index = newIndex;
        return success;
    }

    public void Reset() => _index = -1;

    public T Current => _queue[_index];

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}

internal class Program
{
    private static int GetLength()
    {
        try
        {
            Console.Write("\tКоличество: ");
            var i = Convert.ToInt32(Console.ReadLine());
            if (i < 2) throw new InvalidDataException();
            return i;
        }
        catch
        {
            Console.WriteLine("Укажите число не менее 2");
            return GetLength();
        }
    }

    private static Queueue<T> _try<T>(Func<Queueue<T>, Queueue<T>> func,
        Queueue<T> queueue) where T : IComparable<T>
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

    private static Queueue<T> Try<T>(Func<Queueue<T>, Queueue<T>> func,
        Queueue<T>? queueue) where T : IComparable<T> =>
        _try(func, (queueue == null) ? new Queueue<T>() : queueue);

    public static void Test<T>(Queueue<T>? q) where T : IComparable<T>
    {
        q = Try(queueue =>
        {
            Console.WriteLine($"({typeof(T)}) добавляем элементы");
            foreach (var i in Enumerable.Range(1, GetLength()))
            {
                Console.Write($"\t[{i}] := ");
                queueue.Push((T)Convert.ChangeType(Console.ReadLine() ?? "",
                    typeof(T)));
            }

            return queueue;
        }, q ?? null);
        Console.WriteLine(q);

        Try(queueue =>
        {
            Console.Write("Найти по индексу: ");
            Console.WriteLine(q[Convert.ToInt32(Console.ReadLine()) - 1]);
            return queueue;
        }, q);
        Console.WriteLine($"Удаляем первый элемент...({q.Pop()})\n{q}");
        Try(queueue =>
        {
            Console.Write("Заменяем\n\tвсе: ");
            var from = (T)Convert.ChangeType(Console.ReadLine() ?? "", typeof(T));
            Console.Write("\tна: ");
            var to = (T)Convert.ChangeType(Console.ReadLine() ?? "", typeof(T));
            queueue.ReplaceAll(from, to);
            return queueue;
        }, q);
        Console.WriteLine(q);
        q.RemoveDublicates();
        Console.WriteLine(
            $"Удаляем дубликаты...\n{q}\nНаходим наибольшее...\n{q.Max()}");

        q.Reverse();
        Console.WriteLine($"Обратный порядок: {q}");
        q.Sort(((a, b) => a.CompareTo(b)));
        Console.WriteLine($"Отсортированный порядок: {q}");
    }

    private static void Main(string[] args)
    {
        Test<int>(null);
        Test<double>(null);
        Test<string>(null);
    }
}
