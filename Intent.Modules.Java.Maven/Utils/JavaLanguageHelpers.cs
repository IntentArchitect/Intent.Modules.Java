namespace Intent.Modules.Java.Maven.Utils;

internal static class JavaLanguageHelpers
{
    /// <summary>
    /// Implementation of a Java's <c>string.SubString(...)</c>.
    /// </summary>
    public static string JavaSubstring(this string s, int beginIndex, int? endIndex = null)
    {
        return !endIndex.HasValue
            ? s[beginIndex..]
            : s.Substring(beginIndex, endIndex.Value - beginIndex);
    }
}