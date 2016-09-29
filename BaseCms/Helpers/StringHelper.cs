namespace BaseCms.Helpers
{
    public static class StringHelper
    {
        public static string ClearAlias(this string str)
        {
            str = str.Replace("!", "_");
                str = str.Replace("\"", "_");
                str = str.Replace("#", "_");
                str = str.Replace("%", "_");
                str = str.Replace("&", "_");
                str = str.Replace("'", "_");
                str = str.Replace("*", "_");
                str = str.Replace(",", "_");
                str = str.Replace(".", "_");
                str = str.Replace(";", "_");
                str = str.Replace(":", "_");
                str = str.Replace("<", "_");
                str = str.Replace(">", "_");
                str = str.Replace("=", "_");
                str = str.Replace("?", "_");
                str = str.Replace("[", "_");
                str = str.Replace("]", "_");
                str = str.Replace("^", "_");
                str = str.Replace("`", "_");
                str = str.Replace("{", "_");
                str = str.Replace("}", "_");
                str = str.Replace("|", "_");
                str = str.Replace(" ", "_");
            return str;
        }
    }
}
