namespace NotesBackend.Extensions;

public static class StringExtensions
{
    public static string AddPrefix(this string s, string prefix)
    {
        return $"{prefix}-{s}";
    }
}