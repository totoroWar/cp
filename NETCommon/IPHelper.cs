using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NETCommon
{
    public class RequestHelper
    {
        public static string GetUserIP(HttpRequestBase request)
        {
            #region
            string ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ip == null || ip.Length == 0 || ip.ToLower().IndexOf("unknown") > -1)
            {
                ip = request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                if (ip.IndexOf(',') > -1)
                {
                    ip = ip.Substring(0, ip.IndexOf(','));
                }
                if (ip.IndexOf(';') > -1)
                {
                    ip = ip.Substring(0, ip.IndexOf(';'));
                }
            }

            Regex regex = new Regex("[^0-9.]");
            if (ip == null || ip.Length == 0 || regex.IsMatch(ip))
            {
                ip = request.UserHostAddress;
                if (ip == null || ip.Length == 0 || regex.IsMatch(ip))
                {
                    //ip = "0.0.0.0";
                }
            }
            return ip;
            #endregion
        }

        public static string GetPort(HttpRequestBase request)
        {
            return request.ServerVariables["REMOTE_PORT"];
        }
    }

    public class IPPhyLoc
    {
        FileStream ipFile;
        long ip;
        string ipfilePath;

        ///<summary>
        /// 构造函数
        ///</summary>
        ///<param name="ipfilePath">纯真IP数据库路径</param>
        public IPPhyLoc(string ipfilePath)
        {
            this.ipfilePath = ipfilePath;
        }

        public string GetIPAll(string ip)
        {
            if (Regex.IsMatch(ip, "((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])(.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])(.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])(.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])(.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])(.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])(.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])(.(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])){3}))|:)))(%.+)?"))
            {
                return ip;
            }
            var loc = GetIPLocation(ip);
            var result = loc.country + loc.area;
            result = result.Replace("CZ88.NET", "");
            return result;
        }
        ///<summary>
        /// 地理位置,包括国家和地区
        ///</summary>
        public struct IPLocation
        {
            public string country, area;
        }
        ///<summary>
        /// 获取指定IP所在地理位置
        ///</summary>
        ///<param name="strIP">要查询的IP地址</param>
        ///<returns></returns>
        public IPLocation GetIPLocation(string strIP)
        {
            ip = IPToLong(strIP);
            ipFile = new FileStream(ipfilePath, FileMode.Open, FileAccess.Read);
            long[] ipArray = BlockToArray(ReadIPBlock());
            long offset = SearchIP(ipArray, 0, ipArray.Length - 1) * 7 + 4;
            ipFile.Position += offset;//跳过起始IP
            ipFile.Position = ReadLongX(3) + 4;//跳过结束IP

            IPLocation loc = new IPLocation();
            int flag = ipFile.ReadByte();//读取标志
            if (flag == 1)//表示国家和地区被转向
            {
                ipFile.Position = ReadLongX(3);
                flag = ipFile.ReadByte();//再读标志
            }
            long countryOffset = ipFile.Position;
            loc.country = ReadString(flag);

            if (flag == 2)
            {
                ipFile.Position = countryOffset + 3;
            }
            flag = ipFile.ReadByte();
            loc.area = ReadString(flag);

            ipFile.Close();
            ipFile = null;
            return loc;
        }
        ///<summary>
        /// 将字符串形式的IP转换位long
        ///</summary>
        ///<param name="strIP"></param>
        ///<returns></returns>
        public long IPToLong(string strIP)
        {
            byte[] ip_bytes = new byte[8];
            string[] strArr = strIP.Split(new char[] { '.' });
            for (int i = 0; i < 4; i++)
            {
                ip_bytes[i] = byte.Parse(strArr[3 - i]);
            }
            return BitConverter.ToInt64(ip_bytes, 0);
        }
        ///<summary>
        /// 将索引区字节块中的起始IP转换成Long数组
        ///</summary>
        ///<param name="ipBlock"></param>
        long[] BlockToArray(byte[] ipBlock)
        {
            long[] ipArray = new long[ipBlock.Length / 7];
            int ipIndex = 0;
            byte[] temp = new byte[8];
            for (int i = 0; i < ipBlock.Length; i += 7)
            {
                Array.Copy(ipBlock, i, temp, 0, 4);
                ipArray[ipIndex] = BitConverter.ToInt64(temp, 0);
                ipIndex++;
            }
            return ipArray;
        }
        ///<summary>
        /// 从IP数组中搜索指定IP并返回其索引
        ///</summary>
        ///<param name="ipArray">IP数组</param>
        ///<param name="start">指定搜索的起始位置</param>
        ///<param name="end">指定搜索的结束位置</param>
        ///<returns></returns>
        int SearchIP(long[] ipArray, int start, int end)
        {
            int middle = (start + end) / 2;
            if (middle == start)
                return middle;
            else if (ip < ipArray[middle])
                return SearchIP(ipArray, start, middle);
            else
                return SearchIP(ipArray, middle, end);
        }
        ///<summary>
        /// 读取IP文件中索引区块
        ///</summary>
        ///<returns></returns>
        byte[] ReadIPBlock()
        {
            long startPosition = ReadLongX(4);
            long endPosition = ReadLongX(4);
            long count = (endPosition - startPosition) / 7 + 1;//总记录数
            ipFile.Position = startPosition;
            byte[] ipBlock = new byte[count * 7];
            ipFile.Read(ipBlock, 0, ipBlock.Length);
            ipFile.Position = startPosition;
            return ipBlock;
        }
        ///<summary>
        /// 从IP文件中读取指定字节并转换位long
        ///</summary>
        ///<param name="bytesCount">需要转换的字节数，主意不要超过8字节</param>
        ///<returns></returns>
        long ReadLongX(int bytesCount)
        {
            byte[] _bytes = new byte[8];
            ipFile.Read(_bytes, 0, bytesCount);
            return BitConverter.ToInt64(_bytes, 0);
        }
        ///<summary>
        /// 从IP文件中读取字符串
        ///</summary>
        ///<param name="flag">转向标志</param>
        ///<returns></returns>
        string ReadString(int flag)
        {
            if (flag == 1 || flag == 2)//转向标志
                ipFile.Position = ReadLongX(3);
            else
                ipFile.Position -= 1;

            List<byte> list = new List<byte>();
            byte b = (byte)ipFile.ReadByte();
            while (b > 0)
            {
                list.Add(b);
                b = (byte)ipFile.ReadByte();
            }
            return Encoding.Default.GetString(list.ToArray());
        }
    }
}
