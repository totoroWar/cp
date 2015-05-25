using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameLibrary
{
    public partial class _11X5
    {
        protected string[] GetResultString(string result)
        {
            return result.Split(',');
        }
        protected int GetResultZHI_XFS_p1_1_1(string codes, string result)
        {
            string[] resultArr = GetResultString(result);
            string[][] codeArr3 = GetCodeString2_3(codes);
            for (int i = 0; i < resultArr.Length; i++)
            {
                if (!codeArr3[i].Contains(resultArr[i]))
                {
                    return 0;
                }
            }
            return 1;
        }
        protected int GetResultZHI_XFS_p1_1(string codes, string result)
        {
            string[] resultArr = GetResultString(result);
            string[][] codeArr3 = GetCodeString2_2(codes);
            for (int i = 0; i < resultArr.Length; i++)
            {
                if (!codeArr3[i].Contains(resultArr[i]))
                {
                    return 0;
                }
            }
            return 1;
        }
        protected int GetResultZHI_XDS(string codes, string result)
        {
            int n = 0;
            string[] codeArr2 = GetCodeString(codes);
            foreach (string item in codeArr2)
            {
                if (item.Replace(' ',',').Equals(result))
                {
                    n++;
                }
            }
            return n;
        }
        protected int GetResultZU_XFS_p1_1_1(string codes, string result)
        { 
          string[] resultArr= GetResultString(result);
          string[] tempCodes = GetCodeString(codes);
          if (tempCodes.Contains(resultArr[0])&&tempCodes.Contains(resultArr[1])&&tempCodes.Contains(resultArr[2]))
          {
              return 1;
          }
          return 0;
        }
        protected int GetResultZU_XFS_p1_1(string codes, string result)
        {
            string[] resultArr = GetResultString(result);
            string[] tempCodes = GetCodeString(codes);
            if (tempCodes.Contains(resultArr[0]) && tempCodes.Contains(resultArr[1]))
            {
                return 1;
            }
            return 0;
        }
        protected int GetResultZU_XDS_p1_1_1(string codes, string result)
        {
            string[] resultArr = GetResultString(result);
            foreach (var item in GetCodeString(codes))
            {
                string[] temp = item.Split(' ');
                if (resultArr.Contains(temp[0])&&resultArr.Contains(temp[1])&&resultArr.Contains(temp[2]))
                {
                    return 1;
                }
            }
            return 0;
        }
        protected int GetResultZU_XDS_p1_1(string codes, string result)
        {
            string[] resultArr = GetResultString(result);
            foreach (var item in GetCodeString(codes))
            {
                string[] temp = item.Split(' ');
                if (resultArr.Contains(temp[0]) && resultArr.Contains(temp[1]) )
                {
                    return 1;
                }
            }
            return 0;
        }
        protected int GetResultZU_XTD_p1_1_1(string codes, string result)
        {
            string[] codestemp = codes.Split('|');
            string[] codesDM = codestemp[0].Split('&');
            string[] codesTM = codestemp[1].Split('&');
            string[] resultArr = GetCodeString(result);
            switch (codesDM.Length)
            {
                case 1:
                    if (result.Contains(codesDM[0]))
                    {
                        int n = 0;
                        foreach (var item in codesTM)
                        {
                            if (result.Contains(item))
                            {
                                n++;
                            }
                        }
                        return n == 2 ? 1 : 0;
                    }
                    return 0;
                case 2:
                    if (result.Contains(codesDM[0]) && result.Contains(codesDM[1]))
                    {
                        int n = 0;
                        foreach (var item in codesTM)
                        {
                            if (result.Contains(item))
                            {
                                n++;
                            }
                        }
                        return n==1 ? 1 : 0;
                    }
                    return 0;
                default:
                    return 0;
            }
        }
        protected int GetResultZU_XTD_p1_1(string codes, string result)
        {
            string[] codestemp = codes.Split('|');
            string codesDM = codestemp[0];
            string[] codesTM = codestemp[1].Split('&');
            string[] resultArr = GetResultString(result);
            if (resultArr.Contains(codesDM))
            {
                if (codesTM.Contains(resultArr.Where(p => p != codesDM).ToList()[0]))
                {
                    return 1;
                }
            }
            return 0;
        }
        protected int GetResultDWDp1_1_1(string codes, string result)
        {
            int n=0;
            string[] resultArr = GetResultString(result);
            string[][] tempCodes = GetCodeString2_3(Codes);
            for (int i = 0; i < resultArr.Length; i++)
            {
                if (tempCodes[i].Contains(resultArr[i]))
                {
                    n++;
                }
            }
            return n;
        }
        protected int GetResultRXFS(string codes,string result,int n,int m)
        {
            int flag = 0;
            string[] codesArr = GetCodeString(codes);
            foreach (var item in GetResultString(result))
            {
                if (codesArr.Contains(item))
                {
                    flag++;    
                }
            }
           return Combination(flag, m)*Combination(codesArr.Length-m,n-m);
        }
        protected int GetResultRXDS(string codes, string result, int num)
        {
            int n = 0;
            string[] codesArr = GetCodeString(codes);
            string[] resultArr = GetResultString(result);
            foreach (var item in codesArr)
            {
                int flag = 0;
                string[] tempCode = item.Split(' ');
                foreach (var itemcode in tempCode)
                {
                    flag+=resultArr.Contains(itemcode) ? 1 : 0;
                }
                n += flag >= num ? 1 : 0;
            }
            return n;
        }
        protected int GetResultRXTD(string codes, string result, int n, int m)
        {
            string[][] codesArr = GetCodeString2_2(codes);
            string[] resultArr = GetResultString(result);
            int DM = 0, TM = 0;
            foreach (var item in resultArr)
            {
                if (codesArr[0].Contains(item))
                {
                    DM++;
                }
                if (codesArr[1].Contains(item))
                {
                    TM++;
                }
            }
            if (DM + TM >= m)
            {
                if (n == m && DM==0)
                {
                    return 0;
                }
                return Combination(TM, m - DM) * Combination(codesArr[1].Length-TM,n - codesArr[0].Length - (m - DM));
            }
            return 0;
        }
        public string Result1007()
        {
            return string.Format("106:{0}", GetResultZHI_XFS_p1_1_1(Codes, Result.Substring(0, 8))); 
        }
        public string Result1008() { return string.Format("106:{0}", GetResultZHI_XDS(Codes, Result.Substring(0, 8))); }
        public string Result1009() { return string.Format("108:{0}", GetResultZU_XFS_p1_1_1(Codes, Result.Substring(0, 8))); }
        public string Result1010() { return string.Format("108:{0}", GetResultZU_XDS_p1_1_1(Codes, Result.Substring(0, 8))); }
        public string Result1011() { return string.Format("108:{0}", GetResultZU_XTD_p1_1_1(Codes, Result.Substring(0, 8))); }
        public string Result1012() { return string.Format("110:{0}", GetResultZHI_XFS_p1_1(Codes, Result.Substring(0, 5))); }
        public string Result1013() { return string.Format("110:{0}", GetResultZHI_XDS(Codes, Result.Substring(0, 5))); }
        public string Result1014() { return string.Format("112:{0}", GetResultZU_XFS_p1_1(Codes, Result.Substring(0, 5))); }
        public string Result1015() { return string.Format("112:{0}", GetResultZU_XDS_p1_1(Codes, Result.Substring(0, 5))); }
        public string Result1016() { return string.Format("112:{0}", GetResultZU_XTD_p1_1(Codes, Result.Substring(0, 5))); }
        public string Result1019() {
            int n = 0;
            foreach (var item in GetResultString(Result.Substring(0,8)))
            {
                if (GetCodeString(Codes).Contains(item))
                {
                    n++;
                }
            }
            return string.Format("114:{0}",n); } 
        public string Result1020() { return string.Format("116:{0}", GetResultDWDp1_1_1(Codes, Result.Substring(0, 8))); }
        public string Result1021() {
            int d = 0;
            foreach (var item in GetResultString(Result))
            {
                d += int.Parse(item) % 2 == 0 ? 0 : 1;
            }
            if (GetCodeString(Codes).Contains(d + "单" + (5 - d) + "双"))
            {
                switch (d + "单" + (5 - d) + "双")
                {
                    case "0单5双":
                        return "118:1";
                    case "5单0双":
                        return "119:1";
                    case "1单4双":
                        return "120:1";
                    case "4单1双":
                        return "121:1";
                    case "2单3双":
                        return "122:1";
                    case "3单2双":
                        return "123:1";
                }
            }
            return "";
        }
        public string Result1022() {
            List<string> tempResult = GetResultString(Result).OrderBy(p => p).ToList();
            string[] tempCodes = GetCodeString(Codes);
            if (tempCodes.Contains(int.Parse(tempResult[2]).ToString()))
            {
                switch (int.Parse(tempResult[2]))
                {
                    case 3:
                    case 9:
                        return "125:1";
                    case 4:
                    case 8:
                        return "126:1";
                    case 5:
                    case 7:
                        return "127:1";
                    case 6:
                        return "128:1";
                }
            }
            return "";
        }
        public string Result1023() { return string.Format("130:{0}", GetResultRXFS(Codes, Result, 1,1)); }
        public string Result1024() { return string.Format("132:{0}", GetResultRXFS(Codes, Result, 2,2)); }
        public string Result1025() { return string.Format("134:{0}", GetResultRXFS(Codes, Result, 3,3)); }
        public string Result1026() { return string.Format("136:{0}", GetResultRXFS(Codes, Result, 4,4)); }
        public string Result1027() { return string.Format("138:{0}", GetResultRXFS(Codes, Result, 5,5)); }
        public string Result1028() { return string.Format("140:{0}", GetResultRXFS(Codes, Result, 6,5)); }
        public string Result1029() { return string.Format("142:{0}", GetResultRXFS(Codes, Result, 7,5)); }
        public string Result1030() { return string.Format("144:{0}", GetResultRXFS(Codes, Result, 8,5)); }
        public string Result1031() { return string.Format("130:{0}", GetResultRXDS(Codes, Result, 1)); }
        public string Result1032() { return string.Format("132:{0}", GetResultRXDS(Codes, Result, 2)); }
        public string Result1033() { return string.Format("134:{0}", GetResultRXDS(Codes, Result, 3)); }
        public string Result1034() { return string.Format("136:{0}", GetResultRXDS(Codes, Result, 4)); }
        public string Result1035() { return string.Format("138:{0}", GetResultRXDS(Codes, Result, 5)); }
        public string Result1036() { return string.Format("140:{0}", GetResultRXDS(Codes, Result, 5)); }
        public string Result1037() { return string.Format("142:{0}", GetResultRXDS(Codes, Result, 5)); }
        public string Result1038() { return string.Format("144:{0}", GetResultRXDS(Codes, Result, 5)); }
        public string Result1039() { return string.Format("132:{0}", GetResultRXTD(Codes, Result, 2, 2)); }
        public string Result1040() { return string.Format("134:{0}", GetResultRXTD(Codes, Result, 3, 3)); }
        public string Result1041() { return string.Format("136:{0}", GetResultRXTD(Codes, Result, 4, 4)); }
        public string Result1042() { return string.Format("138:{0}", GetResultRXTD(Codes, Result, 5, 5)); }
        public string Result1043() { return string.Format("140:{0}", GetResultRXTD(Codes, Result, 6, 5)); }
        public string Result1044() { return string.Format("142:{0}", GetResultRXTD(Codes, Result, 7, 5)); }
        public string Result1045() { return string.Format("144:{0}", GetResultRXTD(Codes, Result, 8, 5)); }
    }
}
