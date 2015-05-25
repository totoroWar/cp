using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCommon
{
    public static class IDHelper
    {
        private static Int64 seed = Int64.Parse(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString("0"));

        public static Int64 Get()
        {
            return Interlocked.Increment(ref seed);
        }

        public static string GetSerial()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + IDHelper.Get().ToString().Substring(8);
        }

    }
}
