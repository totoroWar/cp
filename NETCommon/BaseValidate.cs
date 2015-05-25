using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NETCommon
{
    public class GeneralValidate
    {
        public static bool BaseCheck(string input)
        {
            if (input == null)
            {
                return false;
            }
            if (input == string.Empty)
            {
                return false;
            }
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 是否合法账号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAccount(string input)
        {
            if (!BaseCheck(input))
            {
                return false;
            }
            if (!Regex.IsMatch(input, "[1-9a-zA-Z]+"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是不是数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            if (false == BaseCheck(input))
            {
                return false;
            }
            if (!Regex.IsMatch(input, @"[-]?\d+"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 能不能转为日期格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDatetime(string input)
        {
            if (!BaseCheck(input))
            {
                return false;
            }
            try
            {
                DateTime dt = DateTime.Parse(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否空的字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string input)
        {
            if (!BaseCheck(input))
            {
                return true;
            }
            if (false == Regex.IsMatch(input, @"^[^\s]+"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 密码检测
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsAvailablePassword(string input)
        {
            if (!BaseCheck(input))
            {
                return false;
            }
            if (false == Regex.IsMatch(input, @"[0-9]+") || false == Regex.IsMatch(input, @"[a-zA-Z]+"))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 大写字母
        /// </summary>
        /// <returns></returns>
        public static bool IsUpperWord(string input)
        {
            if (!BaseCheck(input))
            {
                return false;
            }
            if (false == Regex.IsMatch(input, @"[A-Z]+"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 大写字母
        /// </summary>
        /// <returns></returns>
        public static bool IsLowerWord(string input)
        {
            if (!BaseCheck(input))
            {
                return false;
            }
            if (false == Regex.IsMatch(input, @"[a-z]+"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 是不是浮点
        /// </summary>
        public static bool IsDecimal(string input)
        {
            if (!BaseCheck(input))
            {
                return false;
            }
            try
            {
                decimal.Parse(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
