using System;

namespace Model
{
    public static class Common
    {
        public static string ConvertDate(DateTime date)
        {
            return date.Date.ToString().Split(' ', (char) StringSplitOptions.None)[0].Replace("/", ".");
        }
    }
}
