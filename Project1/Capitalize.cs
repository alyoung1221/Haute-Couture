using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project1
{
    public static class Capitalize
    {
        public static string CapitalizeWords(this string s)
        {
            // Check for empty string.  
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            var words = s.Split(' ');

            var t = "";
            foreach (var word in words)
            {
                t += char.ToUpper(word[0]) + word.Substring(1) + ' ';
            }
            return t.Trim();
        }
    }
}