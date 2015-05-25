using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace GameServices
{
    public class MyException
    {
        public static Exception GetInnerException(Exception error)
        {
            return (error.InnerException == null) ? error : GetInnerException(error.InnerException); 
        }
        public static string Message(string message)
        {
            var match = Regex.Match(message, @"DBE.[^]]+.");
            if (0 < match.Captures.Count)
            {
                return match.Value.ToString();
            }
            return message;
        }
    }
}
