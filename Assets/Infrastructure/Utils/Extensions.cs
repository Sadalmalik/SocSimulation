using System;
using System.Collections.Generic;

public static class Extensions
{
    public static void SafeInvoke(this Action action)
    {
        if (action != null) action();
    }

    public static void SafeInvoke<T>(this Action<T> action, T t)
    {
        if (action != null) action(t);
    }

    public static void SafeInvoke<T, B>(this Action<T, B> action, T t, B b)
    {
        if (action != null) action(t, b);
    }

    public static void SafeInvoke<T, B, F>(this Action<T, B, F> action, T t, B b, F f)
    {
        if (action != null) action(t, b, f);
    }

    public static void SafeInvoke<T, B, F, X>(this Action<T, B, F, X> action, T t, B b, F f, X x)
    {
        if (action != null) action(t, b, f, x);
    }

    public static int CountLines(this string text)
    {
        int count = 0;
        if (!string.IsNullOrEmpty(text))
        {
            count = text.Length - text.Replace("\n", string.Empty).Length;

            // if the last char of the string is not a newline, make sure to count that line too
            if (text[text.Length - 1] != '\n')
            {
                ++count;
            }
        }

        return count;
    }

    public static void RemoveRange<T>(this List<T> self, List<T>list)
    {
        foreach (var element in list)
            self.Remove(element);
    }
}