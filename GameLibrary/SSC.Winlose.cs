using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameLibrary
{
    public partial class SSC
    {     
        protected int GetResultS1_1_1_1_1p1_1_1_1_1(string codes, string result)
        {
            return GetResultZHI_XFS(codes,result);
        }
        protected int GetResultS1_1_1_1p0_1_1_1_1(string codes, string result)
        {
            result = GetResultString(result).Substring(1);
            return GetResultZHI_XFS(codes, result);
        }
        protected int GetResultS1_1_1p0_0_1_1_1(string codes, string result)
        {
            result = GetResultString(result).Substring(2);
            return GetResultZHI_XFS(codes, result);
        }
        protected int GetResultS1_1_1p1_1_1_0_0(string codes, string result)
        {
            result = GetResultString(result).Substring(0, 3);
            return GetResultZHI_XFS(codes, result);
        }
        protected int GetResultS1_1p0_0_0_1_1(string codes, string result)
        {
            result = GetResultString(result).Substring(3);
            return GetResultZHI_XFS(codes, result);
        }
        protected int GetResultS1_1p1_1_0_0_0(string codes, string result)
        {
            result = GetResultString(result).Substring(0, 2);
            return GetResultZHI_XFS(codes, result);
        }
        protected int GetResultS1_1p0_1_1_0_0(string codes, string result)
        {
            result = GetResultString(result).Substring(1, 2);
            return GetResultZHI_XFS(codes, result);
        }
        protected int GetResultS1p0_0_1_0_0(string codes, string result)
        {
            result = result[4].ToString();
            if (codes.Contains(result))
            {
                return 1;
            }
            return 0;
        }
        protected int GetResultS1p0_0_0_0_1(string codes, string result)
        {
            result = result[8].ToString();
            if (codes.Contains(result))
            {
                return 1;
            }
            return 0;
        }
        protected int GetResultS1p0_0_1_1_1(string codes, string result)
        {
            result = GetResultString(result.Substring(4));
            return GetResultS1p1_1_1(codes,result);
        }
        protected int GetResultS1p1_1_1_0_0(string codes, string result)
        {
            result = GetResultString(result.Substring(0, 5));
            return GetResultS1p1_1_1(codes, result);
        }
        protected int GetResultS2p0_0_1_1_1(string codes, string result)
        {
            result = GetResultString(result.Substring(4));
            return GetResultS2p1_1_1(codes,result);
        }
        protected int GetResultS2p1_1_1_0_0(string codes, string result)
        {
            result = GetResultString(result.Substring(0, 5));
            return GetResultS2p1_1_1(codes, result);
        }
        protected int GetResultS1p0_1_1_1_1(string codes, string result)
        {
            int n = 0;
            result = GetResultString(result.Substring(2));
            codes = GetCodeString(codes);
            foreach (char item in codes)
            {
                if (result.Contains(item))
                {
                    n++;
                }
            }
            return n;
        }
        protected int GetResultS2p0_1_1_1_1(string codes, string result)
        {
            int n = 0;
            result = GetResultString(result.Substring(2));
            codes = GetCodeString(codes);
            foreach (char item in codes)
            {
                if (result.Contains(item))
                {
                    n++;
                }
            }
            return Combination(n, 2);
        }
        protected int GetResultS2p1_1_1_1_1(string codes, string result)
        {
            int n = 0;
            result = GetResultString(result);
            codes = GetCodeString(codes);
            foreach (char item in codes)
            {
                if (result.Contains(item))
                {
                    n++;
                }
            }
            return Combination(n, 2);
        }
        protected int GetResultS3p1_1_1_1_1(string codes, string result)
        {
            int n = 0;
            result = GetResultString(result);
            codes = GetCodeString(codes);
            foreach (char item in codes)
            {
                if (result.Contains(item))
                {
                    n++;
                }
            }
            return Combination(n, 3);
        }
        protected int GetResultDXDSp1_1_0_0_0(string codes, string result)
        {
            result = GetResultString(result).Substring(0, 2);
          return  GetResultDXDSp1_1( codes,  result);
        }
        protected int GetResultDXDSp0_0_0_1_1(string codes, string result)
        {
            result = GetResultString(result).Substring(3);
            return GetResultDXDSp1_1(codes, result);
        }
        protected int GetResultDXDSp0_0_1_1_1(string codes, string result)
        {
            result = GetResultString(result).Substring(2);
            return GetResultDXDSp1_1_1(codes,result);
        }
        protected int GetResultDXDSp1_1_1_0_0(string codes, string result)
        {
            result = GetResultString(result).Substring(0, 3);
            return GetResultDXDSp1_1_1(codes, result);
        }
        protected int GetResultS5x1p1_1_1_1_1(string codes, string result)
        {
            if (GetResultSortStyle(result) == "abcde")
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
        protected int GetResultS4x1p0_1_1_1_1(string codes, string result)
        {
            result = result.Substring(2);
            if (GetResultSortStyle(result) == "abcd")
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
        protected int GetResultS3x1p0_0_1_1_1(string codes, string result)
        {
            result = result.Substring(4);
            return GetResultZU6_FS(codes, result);
        }
        protected int GetResultS3x1p1_1_1_0_0(string codes, string result)
        {
            result = result.Substring(0, 5);
            return GetResultZU6_FS(codes, result);
        }
        protected int GetResultS1x2q3x1(string codes, string result)
        {
            if (GetResultSortStyle(result) == "abcdd")
            {
                result = BubblingSort(GetResultString(result));
                string[] codesArr = GetCodeString3(codes);
                foreach (char item in codesArr[0])
                {
                    if (result.Contains(item.ToString() + item))
                    {
                        result = result.Replace(item.ToString(), string.Empty);
                        if (codesArr[1].Contains(result[0]) &&
                            codesArr[1].Contains(result[1]) &&
                            codesArr[1].Contains(result[2]))
                        {
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }
        protected int GetResultS2x2q1x1(string codes, string result)
        {
            if (GetResultSortStyle(result) == "abbcc")
            {
                result = BubblingSort(GetResultString(result));
                string[] codesArr = GetCodeString3(codes);
                foreach (char item in codesArr[0])
                {
                    if (result.Contains(item.ToString() + item))
                    {
                        result = result.Replace(item.ToString(), string.Empty);
                        if (result.Length == 1 && codesArr[1].Contains(result[0]))
                        {
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }
        protected int GetResultS1x3q2x1(string codes, string result)
        {
            if (GetResultSortStyle(result) == "abccc")
            {
                result = BubblingSort(GetResultString(result));
                string[] codesArr = GetCodeString3(codes);
                foreach (char item in codesArr[0])
                {
                    if (result.Contains(item.ToString() + item + item))
                    {
                        result = result.Replace(item.ToString(), string.Empty);
                        if (codesArr[1].Contains(result[0]) && codesArr[1].Contains(result[1]))
                        {
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }
        protected int GetResultS1x3q1x2(string codes, string result)
        {
            if (GetResultSortStyle(result) == "aabbb")
            {
                result = BubblingSort(GetResultString(result));
                string[] codesArr = GetCodeString3(codes);
                foreach (char item in codesArr[0])
                {
                    if (result.Contains(item.ToString() + item + item))
                    {
                        result = result.Replace(item.ToString(), string.Empty);
                        if (codesArr[1].Contains(result[0]))
                        {
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }
        protected int GetResultS1x4q1x1(string codes, string result)
        {
            if (GetResultSortStyle(result) == "abbbb")
            {
                result = BubblingSort(GetResultString(result));
                string[] codesArr = GetCodeString3(codes);
                foreach (char item in codesArr[0])
                {
                    if (result.Contains(item.ToString() + item + item + item))
                    {
                        result = result.Replace(item.ToString(), string.Empty);
                        if (codesArr[1].Contains(result[0].ToString()))
                        {
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }
        protected int GetResultS1x2q2x1p0_1_1_1_1(string codes, string result)
        {
            result = result.Substring(2);
            if (GetResultSortStyle(result) == "abcc")
            {
                result = BubblingSort(GetResultString(result));
                string[] codesArr = GetCodeString3(codes);
                foreach (char item in codesArr[0])
                {
                    if (result.Contains(item.ToString() + item))
                    {
                        result = result.Replace(item.ToString(), string.Empty);
                        if (codesArr[1].Contains(result[0]) && codesArr[1].Contains(result[1]))
                        {
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }
        protected int GetResultS2x2p0_1_1_1_1(string codes, string result)
        {
            result = result.Substring(2);
            if (GetResultSortStyle(result) == "aabb")
            {
                result = BubblingSort(GetResultString(result));
                codes = GetCodeString(codes);
                if (codes.Contains(result[0]) && codes.Contains(result[3]))
                {
                    return 1;
                }
            }
            return 0;
        }
        protected int GetResultS1x3q1x1p0_1_1_1_1(string codes, string result)
        {
            result = result.Substring(2);
            if (GetResultSortStyle(result) == "abbb")
            {
                result = BubblingSort(GetResultString(result));
                string[] codesArr = GetCodeString3(codes);
                if (codesArr[0].Contains(result[2]) && codesArr[1].Contains(result[0]))
                {
                    return 1;
                }
            }
            return 0;
        }
        protected int GetResultS1x1(string codes, string result)
        {
            int n = 0;
            codes = GetCodeString(codes);
            foreach (char item in GetResultString(result))
            {
                if (codes.Contains(item))
                {
                    codes = codes.Replace(item.ToString(), string.Empty);
                    n++;
                }
            }
            return n;
        }
        protected int GetResultS1x2(string codes, string result)
        {
            int n = 0;
            codes = GetCodeString(codes);
            result = BubblingSort(GetResultString(result));
            foreach (char item in codes)
            {
                if (result.Contains(item + item.ToString()))
                {
                    result = result.Replace(item.ToString(), string.Empty);
                    n++;
                }
            }
            return n;
        }
        protected int GetResultS1x3(string codes, string result)
        {
            codes = GetCodeString(codes);
            result = BubblingSort(GetResultString(result));
            foreach (char item in codes)
            {
                if (result.Contains(item + item.ToString() + item))
                {
                    return 1;
                }
            }
            return 0;
        }
        protected int GetResultS1x4(string codes, string result)
        {
            codes = GetCodeString(codes);
            result = BubblingSort(GetResultString(result));
            foreach (char item in codes)
            {
                if (result.Contains(item + item.ToString() + item + item))
                {
                    return 1;
                }
            }
            return 0;
        }
        protected int GetResultZHI_XHZp1_1_1_0_0(string codes, string result)
        {
            result = GetResultString(result).Substring(0, 3);
            return GetResultZHI_XHZ(codes, result);
        }
        protected int GetResultZHI_XHZp0_0_1_1_1(string codes, string result)
        {
            result = GetResultString(result).Substring(2);
            return GetResultZHI_XHZ(codes, result);
        }
        protected int GetResultZHI_XHZp0_0_0_1_1(string codes, string result)
        {
            result = GetResultString(result).Substring(3);
            return GetResultZHI_XHZ(codes, result);
        }
        protected int GetResultZHI_XHZp1_1_0_0_0(string codes, string result)
        {
            result = GetResultString(result).Substring(0, 2);
            return GetResultZHI_XHZ(codes, result);
        }
        protected int GetResultZHI_KDp0_0_1_1_1(string codes, string result)
        {
            result = result.Substring(4);
            return GetResultZHI_KD(codes, result);
        }
        protected int GetResultZHI_KDp1_1_1_0_0(string codes, string result)
        {
            result = result.Substring(0, 5);
            return GetResultZHI_KD(codes, result);
        }
        protected int GetResultZHI_KDp0_0_0_1_1(string codes, string result)
        {
            result = result.Substring(6);
            return GetResultZHI_KD(codes, result);
        }
        protected int GetResultZHI_KDp1_1_0_0_0(string codes, string result)
        {
            result = result.Substring(0, 3);
            return GetResultZHI_KD(codes, result);
        }
        protected int GetResultI3p0_0_1_1_1(string codes, string result)
        {
            result = result.Substring(4);
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
        protected int GetResultI2p0_0_0_1_1(string codes, string result)
        {
            result = result.Substring(6);
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
        protected int GetResultI3p1_1_1_0_0(string codes, string result)
        {
            result = result.Substring(0, 5);
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
        protected int GetResultZU3_FS0_0_1_1_1(string codes, string result)
        {
            result = result.Substring(4);
            return GetResultZU3_FS(codes, result);
        }
        protected int GetResultZU3_DS0_0_1_1_1(string codes, string result)
        {
            result = result.Substring(4);
            return GetResultZU3_DS(codes,result);
        }
        protected int GetResultZU3_FS1_1_1_0_0(string codes, string result)
        {
            result = result.Substring(0, 5);
            return GetResultZU3_FS(codes, result);
        }
        protected int GetResultZU2_FS1_1_0_0_0(string codes, string result)
        {
            result = result.Substring(0, 3);
            return GetResultZU2_FS(codes,result);
        }
        protected int GetResultZU2_FS0_0_0_1_1(string codes, string result)
        {
            result = result.Substring(6);
            return GetResultZU2_FS(codes, result);
        }
        protected int GetResultZU3_DS1_1_1_0_0(string codes, string result)
        {
            result = result.Substring(0, 5);
            return GetResultZU3_DS(codes,result);
        }
        protected int GetResultZU2_DS1_1_0_0_0(string codes, string result)
        {
            result = result.Substring(0, 3);
            return GetResultZU2_DS(codes,result);
        }
        protected int GetResultZU2_DS0_0_0_1_1(string codes, string result)
        {
            result = result.Substring(6);
            return GetResultZU2_DS(codes, result);
        }
        protected int GetResultZU6_XHZp0_0_1_1_1(string codes, string result)
        {
            result = result.Substring(4);
            return GetResultZU6_XHZ(codes,result);
        }
        protected int GetResultZU3_XHZp0_0_1_1_1(string codes, string result)
        {
            result = result.Substring(4);
            return GetResultZU3_XHZ(codes, result);
        }
        protected int GetResultZU2_XHZp0_0_0_1_1(string codes, string result)
        {
            result = result.Substring(6);
            return GetResultZU2_XHZ(codes,result);
        }
        protected int GetResultZU2_XHZp1_1_0_0_0(string codes, string result)
        {
            result = result.Substring(0, 3);
            return GetResultZU2_XHZ(codes, result);
        }
        protected int GetResultZU6_XHZp1_1_1_0_0(string codes, string result)
        {
            result = result.Substring(0, 5);
            return GetResultZU6_XHZ(codes,result);
        }
        protected int GetResultZU3_XHZp1_1_1_0_0(string codes, string result)
        {
            result = result.Substring(0, 5);
            return GetResultZU3_XHZ(codes, result);
        }
        protected int GetResultHZWS3p1_1_1_0_0(string codes, string result)
        {
            int n = 0;
            int ws = (int.Parse(result[0].ToString()) + int.Parse(result[2].ToString()) + int.Parse(result[4].ToString())) % 10;
            codes = GetCodeString(codes);
            foreach (char item in codes)
            {
                if (int.Parse(item.ToString()) == ws)
                {
                    n++;
                }
            }
            return n;
        }
        protected int GetResultHZWS3p0_0_1_1_1(string codes, string result)
        {
            int n = 0;
            int ws = (int.Parse(result[8].ToString()) + int.Parse(result[6].ToString()) + int.Parse(result[4].ToString())) % 10;
            codes = GetCodeString(codes);
            foreach (char item in codes)
            {
                if (int.Parse(item.ToString()) == ws)
                {
                    n++;
                }
            }
            return n;
        }
        protected int GetResultRX2_ZHI_XFS(string codes, string result)
        {
            int n = 0;
            string[] codeArr = GetCodeString3(codes);
            result = GetResultString(result);
            for (int i = 0; i < result.Length; i++)
            {
                if (codeArr[i].Contains(result[i]))
                {
                    n++;
                }
            }
            if (n >= 2)
            {
                n = Combination(n, 2);
            }
            else
            {
                n = 0;
            }
            return n;
        }
        protected int GetResultRX3_ZHI_XFS(string codes, string result)
        {
            int n = 0;
            string[] codeArr = GetCodeString3(codes);
            result = GetResultString(result);
            for (int i = 0; i < result.Length; i++)
            {
                if (codeArr[i].Contains(result[i]))
                {
                    n++;
                }
            }
            if (n >= 3)
            {
                n = Combination(n, 3);
            }
            else
            {
                n = 0;
            }
            return n;
        }
        protected int GetResultRX4_ZHI_XFS(string codes, string result)
        {
            int n = 0;
            string[] codeArr = GetCodeString3(codes);
            result = GetResultString(result);
            for (int i = 0; i < result.Length; i++)
            {
                if (codeArr[i].Contains(result[i]))
                {
                    n++;
                }
            }
            if (n >= 4)
            {
                n = Combination(n, 4);
            }
            else
            {
                n = 0;
            }
            return n;
        }
        protected int One_ResultRX2_ZHI_XDS(string code, string result, string position)
        {
            int n = 0;
            for (int i = 0; i < result.Length - 1; i++)
            {
                if (!position.Contains(i.ToString()) || result[i] != code[0])
                {
                    continue;
                }
                for (int j = i + 1; j < result.Length; j++)
                {
                    if (!position.Contains(j.ToString()) || result[j] != code[1])
                    {
                        continue;
                    }
                    n++;
                }
            }
            return n;
        }
        protected int GetResultRX2_ZHI_XDS(string codes, string result, string position)
        {
            bool changeX = false;
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
            foreach (string item in codesArr)
            {
                int n1=One_ResultRX2_ZHI_XDS(item, result, position);
               CodeChange = Codes;
                if (Change&&n1>0)
                {  
                    bool ChangeOk = true;
                    int temp = int.Parse(codesArr[0]);
                    string tempcode = codesArr[0];
                    do
                    {
                        temp++;
                        int flag = 2 - temp.ToString().Length;
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
                    while (CodeChange.Contains(tempcode) || One_ResultRX2_ZHI_XDS(tempcode, result, position) > 0);
                    if (false == ChangeOk)
                    {
                        ChangeOk = true;
                        temp = int.Parse(codesArr[0]);
                        do
                        {
                            temp--;
                            int flag = 2 - temp.ToString().Length;
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
                            if (temp == -1)
                            {
                                ChangeOk = false;
                                break;
                            }
                        }
                        while (codesArr.Contains(tempcode) || One_ResultRX2_ZHI_XDS(tempcode, result, position) > 0);
                    }
                    if (ChangeOk)
                    {
                        changeX = true;
                        string[] changeCodeTemp = GetCodeString2(CodeChange.Replace(item, tempcode));
                        CodeChange = string.Empty;
                        foreach (var item2 in changeCodeTemp.OrderBy(p => p))
                        {
                            CodeChange += item2 + "&";
                        }
                        CodeChange = CodeChange.Substring(0, CodeChange.Length - 1);
                        UserCode = UserCode.Split(' ')[0] + " " + CodeChange.Replace('&', '|');
                    }
                    else
                    {
                        n = n + One_ResultRX2_ZHI_XDS(item, result, position);
                    }
                }
                else
                {
                    n = n + n1;
                }
            }
            if (!changeX)
            {
                CodeChange = string.Empty;
            }
            return n;
        }
        protected int GetResultRX2_ZU_XDS(string codes, string result, string position)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
            foreach (string item in codesArr)
            {
                for (int i = 0; i < result.Length - 1; i++)
                {
                    if (!position.Contains(i.ToString()))
                    {
                        continue;
                    }
                    for (int j = i + 1; j < result.Length; j++)
                    {
                        if (!position.Contains(j.ToString()))
                        {
                            continue;
                        }
                        if (result[i] != result[j]
                            && item.Contains(result[i])
                            && item.Contains(result[j]))
                        {
                            n++;
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX2_ZHI_XHZ(string codes, string result, string position)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
            foreach (string item in codesArr)
            {
                for (int i = 0; i < result.Length - 1; i++)
                {
                    if (!position.Contains(i.ToString()))
                    {
                        continue;
                    }
                    for (int j = i + 1; j < result.Length; j++)
                    {
                        if (!position.Contains(j.ToString()))
                        {
                            continue;
                        }
                        if ((int.Parse(result[i].ToString()) + int.Parse(result[j].ToString())) == int.Parse(item))
                        {
                            n++;
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX2_ZU_XHZ(string codes, string result, string position)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
            foreach (string item in codesArr)
            {
                for (int i = 0; i < result.Length - 1; i++)
                {
                    if (!position.Contains(i.ToString()))
                    {
                        continue;
                    }
                    for (int j = i + 1; j < result.Length; j++)
                    {
                        if (!position.Contains(j.ToString()))
                        {
                            continue;
                        }
                        if (result[i] != result[j] && (int.Parse(result[i].ToString()) + int.Parse(result[j].ToString())) == int.Parse(item))
                        {
                            n++;
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX2_ZU_XFS(string codes, string result, string position)
        {
            int n = 0;
            codes = GetCodeString(codes);
            result = GetResultString(result);
            for (int i = 0; i < result.Length - 1; i++)
            {
                if (!position.Contains(i.ToString()))
                {
                    continue;
                }
                for (int j = i + 1; j < result.Length; j++)
                {
                    if (!position.Contains(j.ToString()))
                    {
                        continue;
                    }
                    if (codes.Contains(result[i]) && codes.Contains(result[j]) && result[i] != result[j])
                    {
                        n++;
                    }
                }
            }
            return n;
        }
        protected int One_ResultRX3_ZHI_XDS(string code,string result,string position)
        {
            int n = 0;
            for (int i = 0; i < result.Length - 2; i++)
            {
                if (!position.Contains(i.ToString()) || result[i] != code[0])
                {
                    continue;
                }
                for (int j = i + 1; j < result.Length - 1; j++)
                {
                    if (!position.Contains(j.ToString()) || result[j] != code[1])
                    {
                        continue;
                    }
                    for (int z = j + 1; z < result.Length; z++)
                    {
                        if (!position.Contains(z.ToString()) || result[z] != code[2])
                        {
                            continue;
                        }
                        n++;
                    }
                }
            }
            return n;
        }
        protected int GetResultRX3_ZHI_XDS(string codes, string result, string position)
        {
            bool changeX = false;
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
            CodeChange = Codes;
            foreach (string item in codesArr)
            {
                int n1 = One_ResultRX3_ZHI_XDS(item, result, position);
                if (Change && n1 > 0)
                {
                    bool ChangeOk = true;
                    int temp = int.Parse(codesArr[0]);
                    string tempcode = codesArr[0];
                    do
                    {
                        temp++;
                        int flag = 3 - temp.ToString().Length;
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
                    while (CodeChange.Contains(tempcode) || One_ResultRX3_ZHI_XDS(tempcode, result, position) > 0);
                    if (false == ChangeOk)
                    {
                        ChangeOk = true;
                        temp = int.Parse(codesArr[0]);
                        do
                        {
                            temp--;
                            int flag = 3 - temp.ToString().Length;
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
                            if (temp == -1)
                            {
                                ChangeOk = false;
                                break;
                            }
                        }
                        while (codesArr.Contains(tempcode) || One_ResultRX3_ZHI_XDS(tempcode, result, position) > 0);
                    }
                    if (ChangeOk)
                    {
                        changeX = true;
                        string[] changeCodeTemp = GetCodeString2(CodeChange.Replace(item, tempcode));
                        CodeChange = string.Empty;
                        foreach (var item2 in changeCodeTemp.OrderBy(p => p))
                        {
                            CodeChange += item2 + "&";
                        }
                        CodeChange = CodeChange.Substring(0, CodeChange.Length - 1);
                        UserCode = UserCode.Split(' ')[0] + " " + CodeChange.Replace('&', '|');
                    }
                    else
                    {
                        n = n + One_ResultRX3_ZHI_XDS(item, result, position);
                    }
                }
                else
                {
                    n = n + n1;
                }
            }
            if (!changeX)
            {
                CodeChange = "";
            }
            return n;
        }
        protected int GetResultRX3_ZU3_DS(string codes, string result, string position)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
            foreach (string item in codesArr)
            {
                for (int i = 0; i < result.Length - 2; i++)
                {
                    if (!position.Contains(i.ToString()))
                    {
                        continue;
                    }
                    for (int j = i + 1; j < result.Length - 1; j++)
                    {
                        if (!position.Contains(j.ToString()))
                        {
                            continue;
                        }
                        for (int z = j + 1; z < result.Length; z++)
                        {
                            if (!position.Contains(z.ToString()))
                            {
                                continue;
                            }
                            string resultflag = "" + result[i] + result[j] + result[z];
                            string codeflag = "" + item[0] + item[1] + item[2];
                            if (GetResultSortStyle(resultflag) == "abb"
                            && GetResultSortStyle(codeflag) == "abb"
                            &&BubblingSort(resultflag)==BubblingSort(codeflag))
                            {
                                n++;
                            }
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX3_ZHI_XHZ(string codes, string result, string position)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
            foreach (string item in codesArr)
            {
                for (int i = 0; i < result.Length - 2; i++)
                {
                    if (!position.Contains(i.ToString()))
                    {
                        continue;
                    }
                    for (int j = i + 1; j < result.Length - 1; j++)
                    {
                        if (!position.Contains(j.ToString()))
                        {
                            continue;
                        }
                        for (int z = j + 1; z < result.Length; z++)
                        {
                            if (!position.Contains(z.ToString()))
                            {
                                continue;
                            }
                            if ((int.Parse(result[i].ToString()) + int.Parse(result[j].ToString()) + int.Parse(result[z].ToString())) == int.Parse(item))
                            {
                                n++;
                            }
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX3_ZU3_XHZ(string codes, string result, string position)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
            foreach (string item in codesArr)
            {
                for (int i = 0; i < result.Length - 2; i++)
                {
                    if (!position.Contains(i.ToString()))
                    {
                        continue;
                    }
                    for (int j = i + 1; j < result.Length - 1; j++)
                    {
                        if (!position.Contains(j.ToString()))
                        {
                            continue;
                        }
                        for (int z = j + 1; z < result.Length; z++)
                        {
                            if (!position.Contains(z.ToString()))
                            {
                                continue;
                            }
                            if (GetResultSortStyle(result[i] + "," + result[j] + "," + result[z]) == "abb")
                            {
                                if ((int.Parse(result[i].ToString()) + int.Parse(result[j].ToString()) + int.Parse(result[z].ToString())) == int.Parse(item))
                                {
                                    n++;
                                }
                            }
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX3_ZU6_XHZ(string codes, string result, string position)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
                for (int i = 0; i < result.Length - 2; i++)
                {
                    if (!position.Contains(i.ToString()))
                    {
                        continue;
                    }
                    for (int j = i + 1; j < result.Length - 1; j++)
                    {
                        if (!position.Contains(j.ToString()))
                        {
                            continue;
                        }
                        for (int z = j + 1; z < result.Length; z++)
                        {
                            if (!position.Contains(z.ToString()))
                            {
                                continue;
                            }
                            if (GetResultSortStyle(result[i] + "," + result[j] + "," + result[z]) == "abc")
                            {
                                if (codesArr.Contains(((int.Parse(result[i].ToString()) + int.Parse(result[j].ToString()) + int.Parse(result[z].ToString()))).ToString()))
                                {
                                    n++;
                                }
                            }
                        }
                    }
            }
            return n;
        }
        protected int GetResultRX3_ZU6_FS(string codes, string result, string position)
        {
            int n = 0;
            codes = GetCodeString(codes);
            result = GetResultString(result);
            for (int i = 0; i < result.Length - 2; i++)
            {
                if (!position.Contains(i.ToString()))
                {
                    continue;
                }
                for (int j = i + 1; j < result.Length - 1; j++)
                {
                    if (!position.Contains(j.ToString()))
                    {
                        continue;
                    }
                    for (int z = j + 1; z < result.Length; z++)
                    {
                        if (!position.Contains(z.ToString()))
                        {
                            continue;
                        }
                        if (GetResultSortStyle(result[i] + "," + result[j] + "," + result[z]) == "abc"
                         && codes.Contains(result[i])
                         && codes.Contains(result[j])
                         && codes.Contains(result[z]))
                        {
                            n++;
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX3_ZU6_DS(string codes, string result, string position)
        {
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
            foreach (string item in codesArr)
            {
                for (int i = 0; i < result.Length - 2; i++)
                {
                    if (!position.Contains(i.ToString()))
                    {
                        continue;
                    }
                    for (int j = i + 1; j < result.Length - 1; j++)
                    {
                        if (!position.Contains(j.ToString()))
                        {
                            continue;
                        }
                        for (int z = j + 1; z < result.Length; z++)
                        {
                            if (!position.Contains(z.ToString()))
                            {
                                continue;
                            }
                            if (GetResultSortStyle(result[i] + "," + result[j] + "," + result[z]) == "abc"
                             && GetResultSortStyle(item[0] + "," + item[1] + "," + item[2]) == "abc"
                             && item.Contains(result[i])
                             && item.Contains(result[j])
                             && item.Contains(result[z]))
                            {
                                n++;
                            }
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX3_ZU3_FS(string codes, string result, string position)
        {
            int n = 0;
            codes = GetCodeString(codes);
            result = GetResultString(result);
            for (int i = 0; i < result.Length - 2; i++)
            {
                if (!position.Contains(i.ToString()))
                {
                    continue;
                }
                for (int j = i + 1; j < result.Length - 1; j++)
                {
                    if (!position.Contains(j.ToString()))
                    {
                        continue;
                    }
                    for (int z = j + 1; z < result.Length; z++)
                    {
                        if (!position.Contains(z.ToString()))
                        {
                            continue;
                        }
                        if (GetResultSortStyle(result[i] + "," + result[j] + "," + result[z]) == "abb"
                            && codes.Contains(result[i])
                            && codes.Contains(result[j])
                            && codes.Contains(result[z]))
                        {
                            n++;
                        }
                    }
                }
            }
            return n;
        }
        protected int One_ResultRX4_ZHI_XDS(string code, string result, string position)
        {
            int n = 0;
            for (int i = 0; i < result.Length - 3; i++)
            {
                if (!position.Contains(i.ToString()) || result[i] != code[0])
                {
                    continue;
                }
                for (int j = i + 1; j < result.Length - 2; j++)
                {
                    if (!position.Contains(j.ToString()) || result[j] != code[1])
                    {
                        continue;
                    }
                    for (int z = j + 1; z < result.Length - 1; z++)
                    {
                        if (!position.Contains(z.ToString()) || result[z] != code[2])
                        {
                            continue;
                        }
                        for (int y = z + 1; y < result.Length; y++)
                        {
                            if (!position.Contains(y.ToString()) || result[y] != code[3])
                            {
                                continue;
                            }
                            n++;
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX4_ZHI_XDS(string codes, string result, string position)
        {
            bool changeX = false;
            int n = 0;
            string[] codesArr = GetCodeString2(codes);
            result = GetResultString(result);
            CodeChange = Codes;
            foreach (string item in codesArr)
            {
                int n1 = One_ResultRX4_ZHI_XDS(item, result, position);
                if (Change && n1 > 0)
                {
                    bool ChangeOk = true;
                    int temp = int.Parse(codesArr[0]);
                    string tempcode = codesArr[0];
                    do
                    {
                        temp++;
                        int flag = 4 - temp.ToString().Length;
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
                    while (CodeChange.Contains(tempcode) || One_ResultRX4_ZHI_XDS(tempcode, result, position) > 0);
                    if (false == ChangeOk)
                    {
                        ChangeOk = true;
                        temp = int.Parse(codesArr[0]);
                        do
                        {
                            temp--;
                            int flag = 4 - temp.ToString().Length;
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
                            if (temp == -1)
                            {
                                ChangeOk = false;
                                break;
                            }
                        }
                        while (codesArr.Contains(tempcode) || One_ResultRX4_ZHI_XDS(tempcode, result, position) > 0);
                    }
                    if (ChangeOk)
                    {
                        changeX = true;
                        string[] changeCodeTemp = GetCodeString2(CodeChange.Replace(item, tempcode));
                        CodeChange = string.Empty;
                        foreach (var item2 in changeCodeTemp.OrderBy(p => p))
                        {
                            CodeChange += item2 + "&";
                        }
                        CodeChange = CodeChange.Substring(0, CodeChange.Length - 1);
                        UserCode = UserCode.Split(' ')[0] + " " + CodeChange.Replace('&', '|');
                    }
                    else
                    {
                        n = n + One_ResultRX4_ZHI_XDS(item, result, position);
                    }
                }
                else
                {
                    n = n + n1;
                }
            }
            if (!changeX)
            {
                CodeChange = "";
            }
            return n;
        }
        protected int GetResultRX4_ZU24(string codes, string result, string position)
        {
            int n = 0;
            codes = GetCodeString(codes);
            result = GetResultString(result);
            for (int i = 0; i < result.Length - 3; i++)
            {
                if (!position.Contains(i.ToString()))
                {
                    continue;
                }
                for (int j = i + 1; j < result.Length - 2; j++)
                {
                    if (!position.Contains(j.ToString()))
                    {
                        continue;
                    }
                    for (int z = j + 1; z < result.Length - 1; z++)
                    {
                        if (!position.Contains(z.ToString()))
                        {
                            continue;
                        }
                        for (int y = z + 1; y < result.Length; y++)
                        {
                            if (!position.Contains(y.ToString()))
                            {
                                continue;
                            }
                            if (GetResultSortStyle(result[i] + "," + result[j] + "," + result[z] + "," + result[y]) == "abcd"
                             && codes.Contains(result[i])
                             && codes.Contains(result[j])
                             && codes.Contains(result[z])
                             && codes.Contains(result[y]))
                            {
                                n++;
                            }
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX4_ZU12(string codes, string result, string position)
        {
            int n = 0;
            string[] codesArr = GetCodeString3(codes);
            result = GetResultString(result);
            for (int i = 0; i < result.Length - 3; i++)
            {
                if (!position.Contains(i.ToString()))
                {
                    continue;
                }
                for (int j = i + 1; j < result.Length - 2; j++)
                {
                    if (!position.Contains(j.ToString()))
                    {
                        continue;
                    }
                    for (int z = j + 1; z < result.Length - 1; z++)
                    {
                        if (!position.Contains(z.ToString()))
                        {
                            continue;
                        }
                        for (int y = z + 1; y < result.Length; y++)
                        {
                            if (!position.Contains(y.ToString()))
                            {
                                continue;
                            }
                            if (GetResultSortStyle(result[i] + "," + result[j] + "," + result[z] + "," + result[y]) == "abcc")
                            {
                                string temp = BubblingSort(result[i].ToString() + result[j] + result[z] + result[y]);
                                foreach (char item in codesArr[0])
                                {
                                    if (temp.Contains(item + item.ToString()))
                                    {
                                        temp = temp.Replace(item.ToString(), string.Empty);
                                        if (codesArr[1].Contains(temp[0]) && codesArr[1].Contains(temp[1]))
                                        {
                                            n++;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX4_ZU6(string codes, string result, string position)
        {
            int n = 0;
            codes = GetCodeString(codes);
            result = GetResultString(result);
            for (int i = 0; i < result.Length - 3; i++)
            {
                if (!position.Contains(i.ToString()))
                {
                    continue;
                }
                for (int j = i + 1; j < result.Length - 2; j++)
                {
                    if (!position.Contains(j.ToString()))
                    {
                        continue;
                    }
                    for (int z = j + 1; z < result.Length - 1; z++)
                    {
                        if (!position.Contains(z.ToString()))
                        {
                            continue;
                        }
                        for (int y = z + 1; y < result.Length; y++)
                        {
                            if (!position.Contains(y.ToString()))
                            {
                                continue;
                            }
                            if (GetResultSortStyle(result[i] + "," + result[j] + "," + result[z] + "," + result[y]) == "aabb")
                            {
                                string temp = BubblingSort(result[i].ToString() + result[j] + result[z] + result[y]);
                                if (codes.Contains(temp[0]) && codes.Contains(temp[2]))
                                {
                                    n++;
                                }
                            }
                        }
                    }
                }
            }
            return n;
        }
        protected int GetResultRX4_ZU4(string codes, string result, string position)
        {
            int n = 0;
            string[] codesArr = GetCodeString3(codes);
            result = GetResultString(result);
            for (int i = 0; i < result.Length - 3; i++)
            {
                if (!position.Contains(i.ToString()))
                {
                    continue;
                }
                for (int j = i + 1; j < result.Length - 2; j++)
                {
                    if (!position.Contains(j.ToString()))
                    {
                        continue;
                    }
                    for (int z = j + 1; z < result.Length - 1; z++)
                    {
                        if (!position.Contains(z.ToString()))
                        {
                            continue;
                        }
                        for (int y = z + 1; y < result.Length; y++)
                        {
                            if (!position.Contains(y.ToString()))
                            {
                                continue;
                            }
                            if (GetResultSortStyle(result[i] + "," + result[j] + "," + result[z] + "," + result[y]) == "abbb")
                            {
                                string temp = BubblingSort(result[i].ToString() + result[j] + result[z] + result[y]);
                                foreach (char item in codesArr[0])
                                {
                                    if (temp.Contains(item.ToString() + item + item))
                                    {
                                        temp = temp.Replace(item.ToString(), string.Empty);
                                        if (codesArr[1].Contains(temp[0]))
                                        {
                                            n++;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return n;
        }

        public string Result30()
        {
            return string.Format("1:{0}", GetResultS1_1_1_1_1p1_1_1_1_1(Codes, Result));
        }
        public string Result31()
        {
            return string.Format("1:{0}", GetResultZHI_XDS(Codes, Result));
        }
        public string Result32()
        {
            StringBuilder temp = new StringBuilder();
            string tempCode = Codes;
            temp.Append("2:" + GetResultS1_1_1_1_1p1_1_1_1_1(tempCode, Result) + "|");
            tempCode = tempCode.Substring(tempCode.IndexOf('|') + 1);
            temp.Append("3:" + GetResultS1_1_1_1p0_1_1_1_1(tempCode, Result) + "|");
            tempCode = tempCode.Substring(tempCode.IndexOf('|') + 1);
            temp.Append("4:" + GetResultS1_1_1p0_0_1_1_1(tempCode, Result) + "|");
            tempCode = tempCode.Substring(tempCode.IndexOf('|') + 1);
            temp.Append("5:" + GetResultS1_1p0_0_0_1_1(tempCode, Result) + "|");
            temp.Append("6:" + GetResultS1p0_0_0_0_1(tempCode, Result));
            return temp.ToString();
        }
        public string Result33()
        {
            return string.Format("7:{0}", GetResultS5x1p1_1_1_1_1(Codes, Result));
        }
        public string Result34()
        {
            return string.Format("8:{0}", GetResultS1x2q3x1(Codes, Result));
        }
        public string Result35()
        {
            return string.Format("9:{0}", GetResultS2x2q1x1(Codes, Result));
        }
        public string Result36()
        {
            return string.Format("10:{0}", GetResultS1x3q2x1(Codes, Result));
        }
        public string Result37()
        {
            return string.Format("11:{0}", GetResultS1x3q1x2(Codes, Result));
        }
        public string Result38()
        {
            return string.Format("12:{0}", GetResultS1x4q1x1(Codes, Result));
        }
        public string Result39()
        {
            return string.Format("55:{0}", GetResultS1x1(Codes, Result));
        }
        public string Result40()
        {
            return string.Format("56:{0}", GetResultS1x2(Codes, Result));
        }
        public string Result41()
        {
            return string.Format("57:{0}", GetResultS1x3(Codes, Result));
        }
        public string Result42()
        {
            return string.Format("58:{0}", GetResultS1x4(Codes, Result));
        }
        public string Result43()
        {
            return string.Format("13:{0}", GetResultS1_1_1_1p0_1_1_1_1(Codes, Result));
        }
        public string Result44()
        {
            return string.Format("13:{0}", GetResultZHI_XDS(Codes, GetResultString(Result).Substring(1)));
        }
        public string Result45()
        {
            StringBuilder temp = new StringBuilder();
            temp.Append("14:" + GetResultS1_1_1_1p0_1_1_1_1(Codes, Result) + "|");
           string tempCodes = Codes.Substring(Codes.IndexOf('|') + 1);
           temp.Append("15:" + GetResultS1_1_1p0_0_1_1_1(tempCodes, Result) + "|");
           tempCodes = tempCodes.Substring(tempCodes.IndexOf('|') + 1);
           temp.Append("16:" + GetResultS1_1p0_0_0_1_1(tempCodes, Result) + "|");
           tempCodes = tempCodes.Substring(tempCodes.IndexOf('|') + 1);
            temp.Append("17:" + GetResultS1p0_0_0_0_1(tempCodes, Result));
            return temp.ToString();
        }
        public string Result46()
        {
            return string.Format("18:{0}", GetResultS4x1p0_1_1_1_1(Codes, Result));
        }
        public string Result47()
        {
            return string.Format("19:{0}", GetResultS1x2q2x1p0_1_1_1_1(Codes, Result));
        }
        public string Result48()
        {
            return string.Format("20:{0}", GetResultS2x2p0_1_1_1_1(Codes, Result));
        }
        public string Result49()
        {
            return string.Format("21:{0}", GetResultS1x3q1x1p0_1_1_1_1(Codes, Result));
        }
        public string Result50()
        {
            return string.Format("22:{0}", GetResultS1_1_1p0_0_1_1_1(Codes, Result));
        }
        public string Result51()
        {
            return string.Format("22:{0}", GetResultZHI_XDS(Codes, GetResultString(Result).Substring(2)));
        }
        public string Result52()
        {
            StringBuilder temp = new StringBuilder();
            temp.Append("23:" + GetResultS1_1_1p0_0_1_1_1(Codes, Result) + "|");
           string tempCodes = Codes.Substring(Codes.IndexOf('|') + 1);
           temp.Append("24:" + GetResultS1_1p0_0_0_1_1(tempCodes, Result) + "|");
           tempCodes = tempCodes.Substring(tempCodes.IndexOf('|') + 1);
           temp.Append("25:" + GetResultS1p0_0_0_0_1(tempCodes, Result));
            return temp.ToString();
        }
        public string Result53()
        {
            return string.Format("22:{0}", GetResultZHI_XHZ(Codes, Result.Substring(4)));
        }
        public string Result54()
        {
            return string.Format("22:{0}", GetResultZHI_KDp0_0_1_1_1(Codes, Result));
        }
        public string Result55()
        {
            return string.Format("26:{0}", GetResultZU3_FS0_0_1_1_1(Codes, Result));
        }
        public string Result56()
        {
            return string.Format("26:{0}", GetResultZU3_DS0_0_1_1_1(Codes, Result));
        }
        public string Result57()
        {
            return string.Format("27:{0}", GetResultS3x1p0_0_1_1_1(Codes, Result));
        }
        public string Result58()
        {
            return string.Format("27:{0}", GetResultI3p0_0_1_1_1(Codes, Result));
        }
        public string Result59()
        {
            return "26:" + GetResultZU3_DS0_0_1_1_1(Codes, Result) + "|" + "27:" + GetResultI3p0_0_1_1_1(Codes, Result);
        }
        public string Result60()
        {
            return string.Format("26:{0}|27:{1}", GetResultZU3_XHZp0_0_1_1_1(Codes, Result), GetResultZU6_XHZp0_0_1_1_1(Codes, Result));
        }
        public string Result61()
        {
            int iZU3 = 0, iZU6 = 0;
           string tempResult = Result.Substring(4);
           string tempCodes = GetCodeString(Codes);
           if (GetResultSortStyle(tempResult) == "abb")
            {
                foreach (char item in tempCodes)
                {
                    if (tempResult.Contains(item))
                    {
                        iZU3++;
                    }
                }
            }
           else if (GetResultSortStyle(tempResult) == "abc")
            {
                foreach (char item in tempCodes)
                {
                    if (tempResult.Contains(item))
                    {
                        iZU6++;
                    }
                }
            }
            return string.Format("26:{0}|27:{1}", iZU3, iZU6);
        }
        public string Result62()
        {
            int n = 0;
            int ws = (int.Parse(Result[8].ToString()) + int.Parse(Result[6].ToString()) + int.Parse(Result[4].ToString())) % 10;
           string tempCodes = GetCodeString(Codes);
           foreach (char item in tempCodes)
            {
                if (int.Parse(item.ToString()) == ws)
                {
                    n++;
                }
            }
            return string.Format("28:{0}", n);
        }
        public string Result63()
        {
            int aaa = 0, abb = 0, abc = 0;
           string tempResult = Result.Substring(4);
           string resultStyle = GetResultSortStyle(tempResult);
            if (resultStyle == "aaa")
            {
                aaa = Codes.Contains("豹子") ? 1 : 0;
            }
            else if (resultStyle == "abb")
            {
                abb = Codes.Contains("对子") ? 1 : 0;
            }
            else 
            {
                tempResult = BubblingSort(GetResultString(tempResult));
                if (tempResult[2] - tempResult[1] == 1 && tempResult[1] - tempResult[0] == 1 && Codes.Contains("顺子"))
                {
                    abc = 1;
                }
            }
            return string.Format("29:{0}|30:{1}|31:{2}", aaa, abc, abb);
        }
        public string Result64()
        {
            return string.Format("32:{0}", GetResultS1_1_1p1_1_1_0_0(Codes, Result));
        }
        public string Result65()
        {
            return string.Format("32:{0}", GetResultZHI_XDS(Codes, GetResultString(Result).Substring(0, 3)));
        }
        public string Result66()
        {
            StringBuilder str = new StringBuilder();
            str.Append(string.Format("33:{0}|", GetResultS1_1_1p1_1_1_0_0(Codes, Result)));
           string tempCodes = Codes.Substring(Codes.IndexOf('|') + 1);
           str.Append(string.Format("34:{0}|", GetResultS1_1p0_1_1_0_0(tempCodes, Result)));
           tempCodes = tempCodes.Substring(tempCodes.IndexOf('|') + 1);
            str.Append(string.Format("35:{0}", GetResultS1p0_0_1_0_0(tempCodes, Result)));
            return str.ToString();
        }
        public string Result67()
        {
            return string.Format("32:{0}", GetResultZHI_XHZp1_1_1_0_0(Codes, Result));
        }
        public string Result68()
        {
            return string.Format("32:{0}", GetResultZHI_KDp1_1_1_0_0(Codes, Result));
        }
        public string Result69()
        {
            return string.Format("36:{0}", GetResultZU3_FS1_1_1_0_0(Codes, Result));
        }
        public string Result70()
        {
            return string.Format("36:{0}", GetResultZU3_DS1_1_1_0_0(Codes, Result));
        }
        public string Result71()
        {
            return string.Format("37:{0}", GetResultS3x1p1_1_1_0_0(Codes, Result));
        }
        public string Result72()
        {
            return string.Format("37:{0}", GetResultI3p1_1_1_0_0(Codes, Result));
        }
        public string Result73()
        {
            return string.Format("36:{0}", GetResultZU3_DS1_1_1_0_0(Codes, Result)) + "|" + string.Format("37:{0}", GetResultI3p1_1_1_0_0(Codes, Result));
        }
        public string Result74()
        {
            return string.Format("36:{0}|37:{1}", GetResultZU3_XHZp1_1_1_0_0(Codes, Result), GetResultZU6_XHZp1_1_1_0_0(Codes, Result));
        }
        public string Result75()
        {
            int iZU3 = 0, iZU6 = 0;
           string tempResult = Result.Substring(0, 5);
           string tempCodes = GetCodeString(Codes);
           if (GetResultSortStyle(tempResult) == "abb")
            {
                foreach (char item in tempCodes)
                {
                    if (tempResult.Contains(item))
                    {
                        iZU3++;
                    }
                }
            }
           else if (GetResultSortStyle(tempResult) == "abc")
            {
                foreach (char item in tempCodes)
                {
                    if (tempResult.Contains(item))
                    {
                        iZU6++;
                    }
                }
            }
            return string.Format("36:{0}|37:{1}", iZU3, iZU6);
        }
        public string Result76()
        {
            return string.Format("38:{0}", GetResultHZWS3p1_1_1_0_0(Codes, Result));
        }
        public string Result77()
        {
            int aaa = 0, abb = 0, abc = 0;
           string tempResult = Result.Substring(0, 5);
           string resultStyle = GetResultSortStyle(tempResult);
            if (resultStyle == "aaa")
            {
                aaa = Codes.Contains("豹子") ? 1 : 0;
            }
            else if (resultStyle == "abb")
            {
                abb = Codes.Contains("对子") ? 1 : 0;
            }
            else 
            {
                tempResult = BubblingSort(GetResultString(tempResult));
                if (tempResult[2] - tempResult[1] == 1 && tempResult[1] - tempResult[0] == 1 && Codes.Contains("顺子"))
                {
                    abc = 1;
                }
            }
            return string.Format("39:{0}|40:{1}|41:{2}", aaa, abc, abb);
        }
        public string Result78()
        {
            return string.Format("42:{0}", GetResultS1_1p0_0_0_1_1(Codes, Result));
        }
        public string Result79()
        {
            return string.Format("42:{0}", GetResultZHI_XDS(Codes, GetResultString(Result).Substring(3)));
        }
        public string Result80()
        {
            return string.Format("42:{0}", GetResultZHI_XHZp0_0_0_1_1(Codes, Result));
        }
        public string Result81()
        {
            return string.Format("42:{0}", GetResultZHI_KDp0_0_0_1_1(Codes, Result));
        }
        public string Result82()
        {
            return string.Format("43:{0}", GetResultZU2_FS0_0_0_1_1(Codes, Result));
        }
        public string Result83()
        {
            return string.Format("43:{0}", GetResultI2p0_0_0_1_1(Codes, Result));
        }
        public string Result84()
        {
            return string.Format("43:{0}", GetResultZU2_XHZp0_0_0_1_1(Codes, Result));
        }
        public string Result85()
        {
            int iZU = 0;
            string tempResult = Result.Substring(6);
           string tempCodes = GetCodeString(Codes);
           if (GetResultSortStyle(tempResult) == "ab")
            {
                foreach (char item in tempCodes)
                {
                    if (tempResult.Contains(item))
                    {
                        iZU++;
                    }
                }
            }
            return string.Format("43:{0}", iZU);
        }
        public string Result86()
        {
            return string.Format("44:{0}", GetResultS1_1p1_1_0_0_0(Codes, Result));
        }
        public string Result87()
        {
            return string.Format("44:{0}", GetResultZHI_XDS(Codes, GetResultString(Result).Substring(0, 2)));
        }
        public string Result88()
        {
            return string.Format("44:{0}", GetResultZHI_XHZp1_1_0_0_0(Codes, Result));
        }
        public string Result89()
        {
            return string.Format("44:{0}", GetResultZHI_KDp1_1_0_0_0(Codes, Result));
        }
        public string Result90()
        {
            return string.Format("45:{0}", GetResultZU2_FS1_1_0_0_0(Codes, Result));
        }
        public string Result91()
        {
            return string.Format("45:{0}", GetResultZU2_DS1_1_0_0_0(Codes, Result));
        }
        public string Result92()
        {
            return string.Format("45:{0}", GetResultZU2_XHZp1_1_0_0_0(Codes, Result));
        }
        public string Result93()
        {
            int iZU = 0;
           string tempResult = Result.Substring(0, 3);
           string tempCodes = GetCodeString(Codes);
           if (GetResultSortStyle(tempResult) == "ab")
            {
                foreach (char item in tempCodes)
                {
                    if (tempResult.Contains(item))
                    {
                        iZU++;
                    }
                }
            }
            return string.Format("45:{0}", iZU);
        }
        public string Result94()
        {
            return string.Format("46:{0}", GetResultDWD(Codes, Result));
        }
        public string Result95()
        {
            return string.Format("47:{0}", GetResultS1p0_0_1_1_1(Codes, Result));
        }
        public string Result96()
        {
            return string.Format("47:{0}", GetResultS1p1_1_1_0_0(Codes, Result));
        }
        public string Result97()
        {
            return string.Format("48:{0}", GetResultS2p0_0_1_1_1(Codes, Result));
        }
        public string Result98()
        {
            return string.Format("48:{0}", GetResultS2p1_1_1_0_0(Codes, Result));
        }
        public string Result99()
        {
            return string.Format("49:{0}", GetResultS1p0_1_1_1_1(Codes, Result));
        }
        public string Result100()
        {
            return string.Format("50:{0}", GetResultS2p0_1_1_1_1(Codes, Result));
        }
        public string Result101()
        {
            return string.Format("51:{0}", GetResultS2p1_1_1_1_1(Codes, Result));
        }
        public string Result102()
        {
            return string.Format("52:{0}", GetResultS3p1_1_1_1_1(Codes, Result));
        }
        public string Result21()
        {
            return string.Format("53:{0}", GetResultDXDSp1_1_0_0_0(Codes, Result));
        }
        public string Result22()
        {
            return string.Format("53:{0}", GetResultDXDSp0_0_0_1_1(Codes, Result));
        }
        public string Result23()
        {
            return string.Format("54:{0}", GetResultDXDSp1_1_1_0_0(Codes, Result));
        }
        public string Result103()
        {
            return string.Format("54:{0}", GetResultDXDSp0_0_1_1_1(Codes, Result));
        }
        public string Result104()
        {
            return string.Format("59:{0}", GetResultRX2_ZHI_XFS(Codes, Result));
        }
        public string Result105()
        {
            return string.Format("59:{0}", GetResultRX2_ZHI_XDS(Codes, Result, Position));
        }
        public string Result106()
        {
            return string.Format("59:{0}", GetResultRX2_ZHI_XHZ(Codes, Result, Position));
        }
        public string Result107()
        {
            return string.Format("60:{0}", GetResultRX2_ZU_XFS(Codes, Result, Position));
        }
        public string Result108()
        {
            return string.Format("60:{0}", GetResultRX2_ZU_XDS(Codes, Result, Position));
        }
        public string Result109()
        {
            return string.Format("60:{0}", GetResultRX2_ZU_XHZ(Codes, Result, Position));
        }
        public string Result110()
        {
            return string.Format("61:{0}", GetResultRX3_ZHI_XFS(Codes, Result));
        }
        public string Result111()
        {
            return string.Format("61:{0}", GetResultRX3_ZHI_XDS(Codes, Result, Position));
        }
        public string Result112()
        {
            return string.Format("61:{0}", GetResultRX3_ZHI_XHZ(Codes, Result, Position));
        }
        public string Result113()
        {
            return string.Format("62:{0}", GetResultRX3_ZU3_FS(Codes, Result, Position));
        }
        public string Result114()
        {
            return string.Format("62:{0}", GetResultRX3_ZU3_DS(Codes, Result, Position));
        }
        public string Result115()
        {
            return string.Format("63:{0}", GetResultRX3_ZU6_FS(Codes, Result, Position));
        }
        public string Result116()
        {
            return string.Format("63:{0}", GetResultRX3_ZU6_DS(Codes, Result, Position));
        }
        public string Result117()
        {
            return string.Format("62:{0}|63:{1}", GetResultRX3_ZU3_DS(Codes, Result, Position), GetResultRX3_ZU6_DS(Codes, Result, Position));
        }
        public string Result118()
        {
            return string.Format("62:{0}|63:{1}", GetResultRX3_ZU3_XHZ(Codes, Result, Position), GetResultRX3_ZU6_XHZ(Codes, Result, Position));
        }
        public string Result119()
        {
            return string.Format("64:{0}", GetResultRX4_ZHI_XFS(Codes, Result));
        }
        public string Result120()
        {
            return string.Format("64:{0}", GetResultRX4_ZHI_XDS(Codes, Result, Position));
        }
        public string Result121()
        {
            return string.Format("65:{0}", GetResultRX4_ZU24(Codes, Result, Position));
        }
        public string Result122()
        {
            return string.Format("66:{0}", GetResultRX4_ZU12(Codes, Result, Position));
        }
        public string Result123()
        {
            return string.Format("67:{0}", GetResultRX4_ZU6(Codes, Result, Position));
        }
        public string Result124()
        {
            return string.Format("68:{0}", GetResultRX4_ZU4(Codes, Result, Position));
        }
    }
}
