using System;
using System.Collections.Generic;
using System.Text;

namespace ItemFix
{
    internal static class Extensions
    {
        /// <summary>
        /// Gets an ID from a the link provided
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static string GetID(this string link)
        {
            return link.Replace("https://www.itemfix.com/v?t=", "");
        }

        public static int GetUserTokenOffset(this string whole)
        {
            string a = whole.Replace("list?user_token=ss1iq4pjr7&offset=", "");

            if (string.IsNullOrEmpty(a))
                return -1;

            return int.Parse(a);
        }

        public static int GetNumCount(this int nums)
        {
            string n = nums.ToString();

            int i = 0;
            foreach (char item in n)
            {
                if (char.IsDigit(item))
                    i++;
            }

            return i;
        }
    }
}
