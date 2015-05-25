using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameLibrary
{
    public class PL0_9 : Lottery
    {
        protected int Contains(string string1, string string2)
        {
            int n = 0;
            foreach (char c1 in string1)
            {
                foreach (char c2 in string2)
                {
                    if (c1 == c2)
                    {
                        n++;
                        break;
                    }
                }
            }
            return n;
        }
        protected string BubblingSort(string strNums)
        {
            char[] nums = strNums.ToCharArray();
            for (int i = 0; i < nums.Length - 1; i++)
            {
                for (int j = 0; j < nums.Length - 1 - i; j++)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        char temp = nums[j + 1];
                        nums[j + 1] = nums[j];
                        nums[j] = temp;
                    }
                }
            }
            StringBuilder strResult = new StringBuilder();
            foreach (char item in nums)
            {
                strResult.Append(item);
            }
            return strResult.ToString();
        }
        protected int[] BubblingSort(int[] nums)
        {
            for (int i = 0; i < nums.Length - 1; i++)
            {
                for (int j = 0; j < nums.Length - 1 - i; j++)
                {
                    if (nums[j] > nums[j + 1])
                    {
                        int temp = nums[j + 1];
                        nums[j + 1] = nums[j];
                        nums[j] = temp;
                    }
                }
            }
            return nums;
        }
        protected virtual string GetPositionString(string position)
        {
            string temp = GetCodeString(position);
            if (temp == BubblingSort(temp))
            {
                return temp;
            }
            return "";
        }
        protected virtual string GetResultString()
        {
            return Result.Replace(",", string.Empty);
        }
        protected virtual string GetResultString(string result)
        {
            return result.Replace(",", string.Empty);
        }
        protected virtual string GetResultSortStyle()
        {
            char[] cArr = "abcde".ToArray();
            int[] intArr = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (char item in GetResultString())
            {
                intArr[int.Parse(item.ToString())]++;
            }
            intArr = BubblingSort(intArr);
            StringBuilder strResult = new StringBuilder();
            for (int i = 5, n = 0; i < intArr.Length; i++)
            {
                for (int j = 0; j < intArr[i]; j++)
                {
                    strResult.Append(cArr[n]);
                }
                if (intArr[i] != 0)
                {
                    n++;
                }
            }
            return strResult.ToString();
        }
        protected virtual string GetResultSortStyle(string result)
        {
            string temp = GetResultString(result);
            char[] cArr = "abcde".ToArray();
            int[] intArr = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (char item in temp)
            {
                intArr[int.Parse(item.ToString())]++;
            }
            intArr = BubblingSort(intArr);
            StringBuilder strResult = new StringBuilder();
            for (int i = 5, n = 0; i < intArr.Length; i++)
            {
                for (int j = 0; j < intArr[i]; j++)
                {
                    strResult.Append(cArr[n]);
                }
                if (intArr[i] != 0)
                {
                    n++;
                }
            }
            return strResult.ToString();
        }
        protected virtual string GetResultSort(string result)
        {
            string strResult = string.Empty;
            return strResult;
        }
        protected virtual string[] GetCodeString3(string codes)
        {
            string[] temp = codes.Split('|');
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = temp[i].Replace("&", string.Empty);
            }
            return temp;
        }
        protected virtual string[] GetCodeString2(string codes)
        {
            string[] result = codes.Split('&');
            if (result == null)
            {
                return new string[] { codes };
            }
            else
            {
                return result;
            }
        }
        protected virtual string[] GetCodeString3()
        {
            string[] temp = Codes.Split('|');
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = temp[i].Replace("&", string.Empty);
            }
            return temp;
        }
        protected virtual string[] GetCodeString2()
        {
            string[] result = Codes.Split('&');
            if (result == null)
            {
                return new string[] { Codes };
            }
            else
            {
                return result;
            }
        }
        protected virtual string GetCodeString(string codes)
        {
            return codes.Replace("&", string.Empty);
        }
        protected virtual string GetCodeString()
        {
            return Codes.Replace("&", string.Empty);
        }
        protected virtual string GetNoRepeatString(string str)
        {
            string s = string.Empty;
            foreach (var item in str.Distinct())
            {
                s += item;
            }
            return s;
        }
        protected virtual bool IsNoRepeatString(string str)
        {
            return str.Length == str.Distinct().Count();
        }
        protected virtual bool IsNoRepeatCodes(string codes)
        {
            string[] codesArr = GetCodeString2(codes);
            return codesArr.Length == codesArr.Distinct().Count();
        }
        protected virtual bool IsNoRepeatCodes()
        {
            string[] codesArr = GetCodeString2(Codes);
            return codesArr.Length == codesArr.Distinct().Count();
        }
        protected virtual bool IsNoRepeatCodes2(string codes)
        {
            string[] codesArr = GetCodeString3(codes);
            foreach (var item in codesArr)
            {
                if (!IsNoRepeatString(item))
                {
                    return false;
                }
            }
            return true;
        }
        protected virtual bool IsNoRepeatCodes2()
        {
            string[] codesArr = GetCodeString3(Codes);
            foreach (var item in codesArr)
            {
                if (!IsNoRepeatString(item))
                {
                    return false;
                }
            }
            return true;
        }
        static int[] ZHI_XHZ3 = new int[28] { 1, 3, 6, 10, 15, 21, 28, 36, 45, 55, 63, 69, 73, 75, 75, 73, 69, 63, 55, 45, 36, 28, 21, 15, 10, 6, 3, 1 };
        static int[] ZHI_XHZ2 = new int[19] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        static int[] ZU_XHZ3 = new int[26] { 1, 2, 2, 4, 5, 6, 8, 10, 11, 13, 14, 14, 15, 15, 14, 14, 13, 11, 10, 8, 6, 5, 4, 2, 2, 1 };
        static int[] ZU_XHZ2 = new int[17] { 1, 1, 2, 2, 3, 3, 4, 4, 5, 4, 4, 3, 3, 2, 2, 1, 1 };
        static int[] ZHI_XKD3 = new int[10] { 10, 54, 96, 126, 144, 150, 144, 126, 96, 54 };
        static int[] ZHI_XKD2 = new int[10] { 10, 18, 16, 14, 12, 10, 8, 6, 4, 2 };
        protected int GetNumsS1(string codes)
        {
            return GetCodeString2(codes).Length;
        }
        protected int GetNumsS2(string codes)
        {
            return Combination(GetCodeString2(codes).Length, 2);
        }
        protected int GetNumsS3(string codes)
        {
            return Combination(GetCodeString2(codes).Length, 3);
        }
        protected int GetNumsZU3_FS(string codes)
        {
            return Combination(GetCodeString2(codes).Length, 2) * 2;
        }
        protected int GetNumsDWD(string codes)
        {
            int result = 0;
            foreach (string item in GetCodeString3(codes))
            {
                result += item.Length;
            }
            return result;
        }
        protected int GetNumsZHI_XHKD2(string codes)
        {
            int result = 0;
            foreach (string s in GetCodeString2(codes))
            {
                result += ZHI_XKD2[int.Parse(s)];
            }
            return result;
        }
        protected int GetNumsZHI_XHKD3(string codes)
        {
            int result = 0;
            foreach (string s in GetCodeString2(codes))
            {
                result += ZHI_XKD3[int.Parse(s.ToString())];
            }
            return result;
        }
        protected int GetNumsZHI_XHZ3(string codes)
        {
            int result = 0;
            foreach (string s in GetCodeString2(codes))
            {
                result += ZHI_XHZ3[int.Parse(s)];
            }
            return result;
        }
        protected int GetNumsZHI_XHZ2(string codes)
        {
            int result = 0;
            foreach (string s in GetCodeString2(codes))
            {
                result += ZHI_XHZ2[int.Parse(s.ToString())];
            }
            return result;
        }
        protected int GetNumsZU_XHZ3(string codes)
        {
            int result = 0;
            foreach (string s in GetCodeString2(codes))
            {
                result += ZU_XHZ3[int.Parse(s) - 1];
            }
            return result;
        }
        protected int GetNumsZU_XHZ2(string codes)
        {
            int result = 0;
            foreach (string s in GetCodeString2(codes))
            {
                result += ZU_XHZ2[int.Parse(s) - 1];
            }
            return result;
        }
        protected int GetNumsZHI_XFS(string codes)
        {
            return ArrayItemLenghtProduct(GetCodeString3(codes));
        }
        protected int GetNumsZHI_XDS(string codes)
        {
            return GetCodeString2(codes).Length;
        }
        protected int GetResultZHI_XFS(string codes, string result)
        {
            result = GetResultString(result);
            string[] codeArr3 = GetCodeString3(codes);
            for (int i = 0; i < result.Length; i++)
            {
                if (!codeArr3[i].Contains(result[i]))
                {
                    return 0;
                }
            }
            return 1;
        }
        protected int GetResultZHI_XDS(string codes, string result)
        {
            result = GetResultString(result);
            int n = 0;
            string[] codeArr2 = GetCodeString2(codes);
            foreach (string item in codeArr2)
            {
                if (item.Equals(result))
                {
                    if (Change)
                    {
                        bool ChangeOk = true;
                        int temp = int.Parse(codeArr2[0]);
                        string tempcode = string.Empty;
                        do
                        {
                            temp++;
                            int flag = result.Length - temp.ToString().Length;
                            if (flag == 0)
                            {
                                tempcode = temp.ToString();
                            }
                            if (flag > 0)
                            {
                                string sflag = string.Empty;
                                for (int i = 0; i < flag; i++)
                                {
                                    sflag += "0";
                                }
                                tempcode = sflag + temp.ToString();
                            }
                            if (flag < 0)
                            {
                                ChangeOk = false;
                                break;
                            }
                        }
                        while (codeArr2.Contains(tempcode));
                        if (false == ChangeOk)
                        {
                            ChangeOk = true;
                            temp = int.Parse(codeArr2[0]); 
                            do
                            {
                                temp--;
                                int flag = result.Length - temp.ToString().Length;
                                if (flag == 0)
                                {
                                    tempcode = temp.ToString();
                                }
                                if (flag > 0)
                                {
                                    string sflag = string.Empty;
                                    for (int i = 0; i < flag; i++)
                                    {
                                        sflag += "0";
                                    }
                                    tempcode = sflag + temp.ToString();
                                }
                                if (temp==-1)
                                {
                                    ChangeOk = false;
                                    break;
                                }
                            }
                            while (codeArr2.Contains(tempcode));
                        }
                        if(ChangeOk)
                        {
                           string[] changeCodeTemp= GetCodeString2(Codes.Replace(item, tempcode));
                           CodeChange = string.Empty;
                           foreach (var item2 in changeCodeTemp.OrderBy(p => p))
                           {
                               CodeChange += item2 + "&";
                           }
                           CodeChange = CodeChange.Substring(0, CodeChange.Length - 1);
                           UserCode = UserCode.Split(' ')[0] +" "+ CodeChange.Replace('&', '|');
                        }
                        else
                        {
                            return 1;
                        }
                        return 0;
                    }
                    else
                    {
                        n++;
                    }
                }
            }
            return n;
        }
        protected int GetResultZHI_XHZ(string codes, string result)
        {
            string[] hzcode = GetCodeString2(codes);
            int n = 0;
            foreach (char item in GetResultString(result))
            {
                n += int.Parse(item.ToString());
            }
            if (hzcode.Contains(n.ToString()))
            {
                return 1;
            }
            return 0;
        }
        protected int GetResultZHI_KD(string codes, string result)
        {
            string[] kdcodes = GetCodeString2(codes);
            string temp = BubblingSort(GetResultString(result));
            int n = Math.Abs(int.Parse(temp[0].ToString()) - int.Parse(temp[temp.Length - 1].ToString()));
            if (kdcodes.Contains(n.ToString()))
            {
                return 1;
            }
            return 0;
        }
        protected int GetResultZU6_DS(string codes, string result)
        {
            int n = 0;
            if (GetResultSortStyle(result) == "abc")
            {
                result = BubblingSort(GetResultString(result));
                string[] codesArr = GetCodeString2(codes);
                foreach (string item in codesArr)
                {
                    if (BubblingSort(item) == result)
                    {
                        n++;
                    }
                }
            }
            return n;
        }
        protected int GetResultZU6_FS(string codes, string result)
        {
            if (GetResultSortStyle(result) == "abc")
            {
                codes = GetCodeString(codes);
                result = GetResultString(result);
                foreach (char item in result)
                {
                    if (!codes.Contains(item))
                    {
                        return 0;
                    }
                }
                return 1;
            }
            return 0;
        }
        protected int GetResultZU2_DS(string codes, string result)
        {
            int n = 0;
            if (GetResultSortStyle(result) == "ab")
            {
                result = BubblingSort(GetResultString(result));
                string[] codesArr = GetCodeString2(codes);
                foreach (string item in codesArr)
                {
                    if (BubblingSort(item) == result)
                    {
                        n++;
                    }
                }
            }
            return n;
        }
        protected int GetResultZU3_FS(string codes, string result)
        {
            if (GetResultSortStyle(result) == "abb")
            {
                result = BubblingSort(GetResultString(result));
                if (codes.Contains(result[0]) && codes.Contains(result[2]))
                {
                    return 1;
                }
            }
            return 0;
        }
        protected int GetResultZU3_DS(string codes, string result)
        {
            int n = 0;
            if (GetResultSortStyle(result) == "abb")
            {
                result = BubblingSort(GetResultString(result));
                string[] codesArr = GetCodeString2(codes);
                foreach (string item in codesArr)
                {
                    if (BubblingSort(item) == result)
                    {
                        n++;
                    }
                }
            }
            return n;
        }
        protected int GetResultZU2_FS(string codes, string result)
        {
            if (GetResultSortStyle(result) == "ab")
            {
                result = BubblingSort(GetResultString(result));
                if (codes.Contains(result[0]) && codes.Contains(result[1]))
                {
                    return 1;
                }
            }
            return 0;
        }
        protected int GetResultZU6_XHZ(string codes, string result)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            int sum = int.Parse(result[0].ToString()) + int.Parse(result[2].ToString()) + int.Parse(result[4].ToString());
            if (GetResultSortStyle(result) == "abc")
            {
                foreach (string item in codesArr)
                {
                    if (int.Parse(item) == sum)
                    {
                        n++;
                    }
                }
            }
            return n;
        }
        protected int GetResultZU3_XHZ(string codes, string result)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            int sum = int.Parse(result[0].ToString()) + int.Parse(result[2].ToString()) + int.Parse(result[4].ToString());
            if (GetResultSortStyle(result) == "abb")
            {
                foreach (string item in codesArr)
                {
                    if (int.Parse(item) == sum)
                    {
                        n++;
                    }
                }
            }
            return n;
        }
        protected int GetResultZU2_XHZ(string codes, string result)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            int sum = int.Parse(result[0].ToString()) + int.Parse(result[2].ToString());
            if (GetResultSortStyle(result) == "ab")
            {
                foreach (string item in codesArr)
                {
                    if (int.Parse(item) == sum)
                    {
                        n++;
                    }
                }
            }
            return n;
        }
        protected int GetResultDWD(string codes, string result)
        {
            result = GetResultString(result);
            int n = 0;
            string[] codeArr3 = GetCodeString3(codes);
            for (int i = 0; i < result.Length; i++)
            {
                foreach (char item in codeArr3[i])
                {
                    if (result[i] == item)
                    {
                        n++;
                        break;
                    }
                }
            }
            return n;
        }
        protected int GetResultS1p1_1_1(string codes, string result)
        {
            int n = 0;
            string codeArr = GetCodeString(codes);
            foreach (char item in codeArr)
            {
                if (result.Contains(item))
                {
                    n++;
                }
            }
            return n;
        }
        protected int GetResultS2p1_1_1(string codes, string result)
        {
            int n = 0;
            string codeArr = GetCodeString(codes);
            foreach (char item in codeArr)
            {
                if (result.Contains(item))
                {
                    n++;
                }
            }
            return Combination(n, 2);
        }
        protected string[] GetResultDXDS(string result)
        {
            result = GetResultString(result);
            string temp = string.Empty;
            foreach (char item in result)
            {
                if (int.Parse(item.ToString()) >= 5)
                {
                    temp += "大";
                }
                else
                {
                    temp += "小";
                }
                if (int.Parse(item.ToString()) % 2 != 0)
                {
                    temp += "单";
                }
                else
                {
                    temp += "双";
                }
                temp += "|";
            }
            return temp.Substring(0, temp.Length - 1).Split('|');
        }
        protected int GetResultDXDSp1_1(string codes, string result)
        {
            string[] DXDSResult = GetResultDXDS(result);
            string[] codeArr = GetCodeString3(codes);
            int[] dxds = new int[2] { 0, 0 };
            for (int i = 0; i < DXDSResult.Length; i++)
            {
                if (codeArr[i].Contains(DXDSResult[i][0]))
                {
                    dxds[i]++;
                }
                if (codeArr[i].Contains(DXDSResult[i][1]))
                {
                    dxds[i]++;
                }
            }
            int temp = 1;
            foreach (int item in dxds)
            {
                temp *= item;
            }
            return temp;
        }
        protected int GetResultDXDSp1_1_1(string codes, string result)
        {
            string[] DXDSResult = GetResultDXDS(result);
            string[] codeArr = GetCodeString3(codes);
            int[] dxds = new int[] { 0, 0, 0 };
            for (int i = 0; i < DXDSResult.Length; i++)
            {
                if (codeArr[i].Contains(DXDSResult[i][0]))
                {
                    dxds[i]++;
                }
                if (codeArr[i].Contains(DXDSResult[i][1]))
                {
                    dxds[i]++;
                }
            }
            int temp = 1;
            foreach (int item in dxds)
            {
                temp *= item;
            }
            return temp;
        }
    }
}
