using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameLibrary
{
    public partial class _11X5
    {
        protected int GetNumsDWD(string codes)
        {
            int result = 0;
            foreach (string[] item in GetCodeString2_3(codes))
            {
                if (item[0]!=string.Empty)
                {
                    result += item.Length;     
                }
            }
            return result;
        }
        protected int GetNumsZHI_XDS(string codes)
        {
            return GetCodeString(codes).Length;
        }
        protected int GetNumsTD(string codes,int n)
        {
           var tempCodes= GetCodeString2_2(codes);
           return Combination(tempCodes[1].Length,n - tempCodes[0].Length);
        }
        protected int GetNumsZHI_XFS_3(string codes)
        {
            string[][] data_sel = GetCodeString2_3(codes);
            int nums = 0;
            if (data_sel[0].Length > 0 && data_sel[1].Length > 0 && data_sel[2].Length > 0)
            {
                for (int i = 0; i < data_sel[0].Length; i++)
                {
                    for (int j = 0; j < data_sel[1].Length; j++)
                    {
                        for (int k = 0; k < data_sel[2].Length; k++)
                        {
                            if (data_sel[0][i] != data_sel[1][j] && data_sel[0][i] != data_sel[2][k] && data_sel[1][j] != data_sel[2][k])
                            {
                                nums++;
                            }
                        }
                    }
                }
            }
            return nums;
        }
        protected int GetNumsZHI_XFS_2(string codes)
        {
            string[][] data_sel = GetCodeString2_2(codes);
            int nums = 0;
            if (data_sel[0].Length > 0 && data_sel[1].Length > 0 )
            {
                for (int i = 0; i < data_sel[0].Length; i++)
                {
                    for (int j = 0; j < data_sel[1].Length; j++)
                    {
                        if (data_sel[0][i] != data_sel[1][j])
                        {
                            nums++;
                        }
                    }
                }
            }
            return nums;
        }
        public int Nums1007()
        {
           return GetNumsZHI_XFS_3(Codes);
        }
        public int Nums1008() { return GetNumsZHI_XDS(Codes); }
        public int Nums1009() { return Combination(GetCodeString(Codes).Length,3); }
        public int Nums1010() { return GetNumsZHI_XDS(Codes); }
        public int Nums1011() { return GetNumsTD(Codes,3); }
        public int Nums1012() { return GetNumsZHI_XFS_2(Codes); }
        public int Nums1013() { return GetNumsZHI_XDS(Codes); }
        public int Nums1014() { 
            return Combination(GetCodeString(Codes).Length,2);
        }
        public int Nums1015() { return GetNumsZHI_XDS(Codes); }
        public int Nums1016() { return GetNumsTD(Codes,2); }
        public int Nums1019() { return GetCodeString(Codes).Length; }
        public int Nums1020() { return GetNumsDWD(Codes); }
        public int Nums1021() { return GetCodeString(Codes).Length; }
        public int Nums1022() { return GetCodeString(Codes).Length; }
        public int Nums1023() { return GetCodeString(Codes).Length; }
        public int Nums1024() { return Combination( GetCodeString(Codes).Length,2); }
        public int Nums1025() { return Combination(GetCodeString(Codes).Length, 3); }
        public int Nums1026() { return Combination(GetCodeString(Codes).Length, 4); }
        public int Nums1027() { return Combination(GetCodeString(Codes).Length, 5); }
        public int Nums1028() { return Combination(GetCodeString(Codes).Length, 6); }
        public int Nums1029() { return Combination(GetCodeString(Codes).Length, 7); }
        public int Nums1030() { return Combination(GetCodeString(Codes).Length, 8); }
        public int Nums1031() { return GetNumsZHI_XDS(Codes); }
        public int Nums1032() { return GetNumsZHI_XDS(Codes); }
        public int Nums1033() { return GetNumsZHI_XDS(Codes); }
        public int Nums1034() { return GetNumsZHI_XDS(Codes); }
        public int Nums1035() { return GetNumsZHI_XDS(Codes); }
        public int Nums1036() { return GetNumsZHI_XDS(Codes); }
        public int Nums1037() { return GetNumsZHI_XDS(Codes); }
        public int Nums1038() { return GetNumsZHI_XDS(Codes); }
        public int Nums1039() { return GetNumsTD(Codes, 2); }
        public int Nums1040() { return GetNumsTD(Codes, 3); }
        public int Nums1041() { return GetNumsTD(Codes, 4); }
        public int Nums1042() { return GetNumsTD(Codes, 5); }
        public int Nums1043() { return GetNumsTD(Codes, 6); }
        public int Nums1044() { return GetNumsTD(Codes, 7); }
        public int Nums1045() { return GetNumsTD(Codes, 8); }
    }
}
