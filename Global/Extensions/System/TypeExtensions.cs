namespace Global.Extensions.System;

public static class TypeExtensions
{
    public static object? GetDefault(this Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type) ?? default;
        }

        return null;
    }

    public static List<T> GetNFromList<T>(this IEnumerable<T> list, int n)
    {
        var result = new List<T>();
        var random = new Random();

        List<T> availableOptions = list.ToList(); // 'clone' the original list to avoid affecting it

        for (int i = 0; i < n; i++)
        {
            T selected = Get1FromList(random, availableOptions);
            result.Add(selected);
            availableOptions = availableOptions.Where(x => Equals(availableOptions, selected) == false).Distinct().ToList();
        }

        return result;
    }

    private static T Get1FromList<T>(Random random, List<T> availableOptions)
    {
        int limit = availableOptions.Count - 1;
        int position = random.Next(0, limit);

        T selected = availableOptions[position];
        return selected;
    }

    public static T Get1FromList<T>(this IEnumerable<T> list)
    {
        var random = new Random();
        return Get1FromList(random, list.ToList());
    }
}
