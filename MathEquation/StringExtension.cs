using System;
using System.Collections.Generic;
using System.Text;

namespace MathEquation
{
    public static class StringExtension
    {
        public static string Trim(this string str, int length)
        {
            int end = 0;
            if (length > str.Length)
                end = str.Length;
            else end = str.IndexOf(' ', length);
            if (end == -1) end = str.Length;

            return str.Substring(0, end);
        }
    }
}
