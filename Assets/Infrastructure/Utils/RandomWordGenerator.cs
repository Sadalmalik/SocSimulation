using System.Globalization;
using UnityEngine;

public class RandomWordGenerator
{
    public static readonly string charsSoft = "aeiouy";
    public static readonly string charsHard = "bcdfghjklmnpqrstvwxz";

    public static char RandomChar(string chars)
    {
        return chars[Random.Range(0, chars.Length-1)];
    }

    public static string GetRandomChars(string chars, int min, int max)
    {
        string result= "";
        int length = Random.Range(min, max);
        for (int i = 0; i < length; i++)
            result += RandomChar(chars);
        return result;
    }

    public static string GenerateSyllable()
    {
        return GetRandomChars(charsHard,1,2) + GetRandomChars(charsSoft, 1, 3);
    }

    public static string GenerateWord(int min=1,int max=5)
    {
        int length = Random.Range(min, max);
        string result = "";
        for (int i = 0; i < length; i++)
            result += GenerateSyllable();
        return result;
    }

    public static string GenerateName(int min=1, int max=3)
    {
        int length = Random.Range(min, max);
        string result = "";
        for (int i = 0; i < length; i++)
            result += (i > 0 ? " " : "") + FirstCharToUpper(GenerateWord());
        return result;
    }

    public static string FirstCharToUpper(string s)
    {
        // Check for empty string.  
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        // Return char and concat substring.  
        return char.ToUpper(s[0]) + s.Substring(1);
    }
}
