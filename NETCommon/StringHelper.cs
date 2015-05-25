using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETCommon
{
    public class StringHelper
    {
        public static string FormatNumber(ulong value)
        {
            if (value < 4 * 1024)
            {
                return string.Format("{0} Bytes", value);
            }
            else if (value < (long)4 * 1024 * 1024)
            {
                return string.Format("{0} KB", (value / (double)((long)1024)).ToString("N"));
            }
            else if (value < (long)4 * 1024 * 1024 * 1024)
            {
                return string.Format("{0} MB", (value / (double)((long)1024 * 1024)).ToString("N"));
            }
            else if (value < (long)4 * 1024 * 1024 * 1024 * 1024)
            {
                return string.Format("{0} GB", (value / (double)((long)1024 * 1024 * 1024)).ToString("N"));
            }
            else
            {
                return string.Format("{0} TB", (value / (double)((long)1024 * 1024 * 1024 * 1024)).ToString("N"));
            }
        }

        public static string GetGBKCode(string text)
        {
            return System.Text.Encoding.GetEncoding("gb2312").GetString(System.Text.Encoding.Default.GetBytes(text));
        }

        public static string GetShortString(string value, int subLen, int len,string connect)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            if (value.Length >= subLen)
            {
                return value.Substring(0, len)+connect;
            }
            return value;
        }
    }
}
