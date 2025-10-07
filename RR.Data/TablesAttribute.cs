namespace RR.Data;

public class TablesAttribute(string name) : TableAttribute(Pluralize(name))
{
    static bool IsVowel(char c) => "aeiouAEIOU".Contains(c);
    public static string Pluralize(string name)
    {
        if (name.EndsWith("DBO"))
            name = name[..^3];
        if (name.EndsWith('s'))
            return name + "es";
        else if (name.EndsWith('y'))
            return name[..^1] + "ies";
        else
            return name + "s";
    }
}
