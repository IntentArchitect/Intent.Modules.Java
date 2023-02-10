namespace Intent.Modules.Java.SpringDoc.OpenApi
{
    internal static class Utils
    {
        public static string EscapeJavaString(this string @string)
        {
            return @string
                    .Replace("\\", "\\\\")
                    .Replace("\t", "\\t")
                    .Replace("\b", "\\b")
                    .Replace("\n", "\\n")
                    .Replace("\r", "\\r")
                    .Replace("\f", "\\f")
                    .Replace("\'", "\\'")
                    .Replace("\"", "\\\"")
                ;
        }
    }
}
