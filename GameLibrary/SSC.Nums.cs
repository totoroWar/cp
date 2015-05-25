using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace GameLibrary
{
    public partial class SSC
    {
        protected int GetNumsS4(string codes)
        {
            return Combination(GetCodeString2(codes).Length, 4);
        }
        protected int GetNumsS5(string codes)
        {
            return Combination(GetCodeString2(codes).Length, 5);
        }
        protected int GetNumsS1q1(string codes)
        {
            string[] temp = GetCodeString3(codes);
            int n = Contains(temp[0], temp[1]);
            int result = Combination(temp[0].Length, 1) * Combination(temp[1].Length, 1);
            if (n > 0)
            {
                result -= Combination(n, 1);
            }
            return result;
        }
        protected int GetNumsS1q2(string codes)
        {
            string[] temp = GetCodeString3(codes);
            int n = Contains(temp[0], temp[1]);
            int result = Combination(temp[0].Length, 1) * Combination(temp[1].Length, 2);
            if (n > 0)
            {
                result -= Combination(n, 1) * Combination(temp[1].Length - 1, 1);
            }
            return result;
        }
        protected int GetNumsS1q3(string codes)
        {
            string[] temp = GetCodeString3(codes);
            int n = Contains(temp[0], temp[1]);
            int result = Combination(temp[0].Length, 1) * Combination(temp[1].Length, 3);
            if (n > 0)
            {
                result -= Combination(n, 1) * Combination(temp[1].Length - 1, 2);
            }
            return result;
        }
        protected int GetNumsS2q1(string codes)
        {
            string[] temp = GetCodeString3(codes);
            int n = Contains(temp[0], temp[1]);
            int result = Combination(temp[0].Length, 2) * Combination(temp[1].Length, 1);
            if (n > 0)
            {
                result -= Combination(n, 2)
                                             * Combination(2, 1);
                if (temp[0].Length - n > 0)
                {
                    result -= Combination(n, 1) * Combination(temp[0].Length - n, 1);
                }
            }
            return result;
        }
        protected int GetNumsZH(string codes)
        {
            string[] temp = GetCodeString3(codes);
            return ArrayItemLenghtProduct(temp) * temp.Length;
        }
        protected int GetNumsTSH(string codes)
        {
            return GetCodeString2(codes).Length;
        }
        protected int GetNumsRX2_ZHI_XFS(string codes)
        {
            string[] temp = GetCodeString3(codes);
            int result = 0;
            for (int i = 0; i < temp.Length - 1; i++)
            {
                for (int j = i + 1; j < temp.Length; j++)
                {
                    result += temp[i].Length * temp[j].Length;
                }
            }
            return result;
        }
        protected int GetNumsRX_ZHI_XDS(string codes, string position)
        {
            string[] temp = GetCodeString2(codes);
            return temp.Length * Combination(GetCodeString2(position).Length, temp[0].Length);
        }
        protected int GetNumsRX2_ZHI_XHZ(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 2) * GetNumsZHI_XHZ2(codes);
        }
        protected int GetNumsRX3_ZHI_XHZ(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 3) * GetNumsZHI_XHZ3(codes);
        }
        protected int GetNumsRX2_ZU_XFS(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 2) * Combination(GetCodeString2(codes).Length, 2);
        }
        protected int GetNumsRX3_ZU3_XFS(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 3) * Combination(GetCodeString2(codes).Length, 2) * 2;
        }
        protected int GetNumsRX2_ZU_XDS(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 2) * GetCodeString2(codes).Length;
        }
        protected int GetNumsRX2_ZU_XHZ(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 2) * GetNumsZU_XHZ2(codes);
        }
        protected int GetNumsRX3_ZHI_XFS(string codes)
        {
            string[] temp = GetCodeString3(codes);
            int result = 0;
            for (int i = 0; i < temp.Length - 2; i++)
            {
                for (int j = i + 1; j < temp.Length - 1; j++)
                {
                    for (int z = j + 1; z < temp.Length; z++)
                    {
                        result += temp[i].Length * temp[j].Length * temp[z].Length;
                    }
                }
            }
            return result;
        }
        protected int GetNumsRX4_ZHI_XFS(string codes)
        {
            string[] temp = GetCodeString3(codes);
            int result = 0;
            for (int y = 0; y < temp.Length - 3; y++)
            {
                for (int i = y + 1; i < temp.Length - 2; i++)
                {
                    for (int j = i + 1; j < temp.Length - 1; j++)
                    {
                        for (int z = j + 1; z < temp.Length; z++)
                        {
                            result += temp[y].Length * temp[i].Length * temp[j].Length * temp[z].Length;
                        }
                    }
                }
            }
            return result;
        }
        protected int GetNumsC3S3(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 3) * Combination(GetCodeString2(codes).Length, 3);
        }
        protected int GetNumsC4S4(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 4) * Combination(GetCodeString2(codes).Length, 4);
        }
        protected int GetNumsC3I3(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 3) * (GetCodeString2(codes).Length);
        }
        protected int GetNumsRX3_ZU_XHZ3(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 3) * GetNumsZU_XHZ3(codes);
        }
        protected int GetNumsC4S1q2(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 4) * GetNumsS1q2(codes);
        }
        protected int GetNumsC4S2(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 4) * GetNumsS2(codes);
        }
        protected int GetNumsC4S1q1(string codes, string position)
        {
            return Combination(GetCodeString2(position).Length, 4) * GetNumsS1q1(codes);
        }
        public int Nums30()
        {
            return GetNumsZHI_XFS(Codes);
        }
        public int Nums31()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums32()
        {
            return GetNumsZH(Codes);
        }
        public int Nums33()
        {
            return GetNumsS5(Codes);
        }
        public int Nums34()
        {
            return GetNumsS1q3(Codes);
        }
        public int Nums35()
        {
            return GetNumsS2q1(Codes);
        }
        public int Nums36()
        {
            return GetNumsS1q2(Codes);
        }
        public int Nums37()
        {
            return GetNumsS1q1(Codes);
        }
        public int Nums38()
        {
            return GetNumsS1q1(Codes);
        }
        public int Nums39()
        {
            return GetCodeString2(Codes).Length;
        }
        public int Nums40()
        {
            return GetCodeString2(Codes).Length;
        }
        public int Nums41()
        {
            return GetCodeString2(Codes).Length;
        }
        public int Nums42()
        {
            return GetCodeString2(Codes).Length;
        }
        public int Nums43()
        {
            return GetNumsZHI_XFS(Codes);
        }
        public int Nums44()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums45()
        {
            return GetNumsZH(Codes);
        }
        public int Nums46()
        {
            return GetNumsS4(Codes);
        }
        public int Nums47()
        {
            return GetNumsS1q2(Codes);
        }
        public int Nums48()
        {
            return GetNumsS2(Codes);
        }
        public int Nums49()
        {
            return GetNumsS1q1(Codes);
        }
        public int Nums50()
        {
            return GetNumsZHI_XFS(Codes);
        }
        public int Nums51()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums52()
        {
            return GetNumsZH(Codes);
        }
        public int Nums53()
        {
            return GetNumsZHI_XHZ3(Codes);
        }
        public int Nums54()
        {
            return GetNumsZHI_XHKD3(Codes);
        }
        public int Nums55()
        {
            return GetNumsZU3_FS(Codes);
        }
        public int Nums56()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums57()
        {
            return GetNumsS3(Codes);
        }
        public int Nums58()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums59()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums60()
        {
            return GetNumsZU_XHZ3(Codes);
        }
        public int Nums61()
        {
            return 54;
        }
        public int Nums62()
        {
            return GetNumsS1(Codes);
        }
        public int Nums63()
        {
            return GetNumsTSH(Codes);
        }
        public int Nums64()
        {
            return GetNumsZHI_XFS(Codes);
        }
        public int Nums65()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums66()
        {
            return GetNumsZH(Codes);
        }
        public int Nums67()
        {
            return GetNumsZHI_XHZ3(Codes);
        }
        public int Nums68()
        {
            return GetNumsZHI_XHKD3(Codes);
        }
        public int Nums69()
        {
            return GetNumsZU3_FS(Codes);
        }
        public int Nums70()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums71()
        {
            return GetNumsS3(Codes);
        }
        public int Nums72()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums73()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums74()
        {
            return GetNumsZU_XHZ3(Codes);
        }
        public int Nums75()
        {
            return 54;
        }
        public int Nums76()
        {
            return GetCodeString2(Codes).Length;
        }
        public int Nums77()
        {
            return GetNumsTSH(Codes);
        }
        public int Nums78()
        {
            return GetNumsZHI_XFS(Codes);
        }
        public int Nums79()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums80()
        {
            return GetNumsZHI_XHZ2(Codes);
        }
        public int Nums81()
        {
            return GetNumsZHI_XHKD2(Codes);
        }
        public int Nums82()
        {
            return GetNumsS2(Codes);
        }
        public int Nums83()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums84()
        {
            return GetNumsZU_XHZ2(Codes);
        }
        public int Nums85()
        {
            return 9;
        }
        public int Nums86()
        {
            return GetNumsZHI_XFS(Codes);
        }
        public int Nums87()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums88()
        {
            return GetNumsZHI_XHZ2(Codes);
        }
        public int Nums89()
        {
            return GetNumsZHI_XHKD2(Codes);
        }
        public int Nums90()
        {
            return GetNumsS2(Codes);
        }
        public int Nums91()
        {
            return GetNumsZHI_XDS(Codes);
        }
        public int Nums92()
        {
            return GetNumsZU_XHZ2(Codes);
        }
        public int Nums93()
        {
            return 9;
        }
        public int Nums94()
        {
            return GetNumsDWD(Codes);
        }
        public int Nums95()
        {
            return GetNumsS1(Codes);
        }
        public int Nums96()
        {
            return GetNumsS1(Codes);
        }
        public int Nums97()
        {
            return GetNumsS2(Codes);
        }
        public int Nums98()
        {
            return GetNumsS2(Codes);
        }
        public int Nums99()
        {
            return GetNumsS1(Codes);
        }
        public int Nums100()
        {
            return GetNumsS2(Codes);
        }
        public int Nums101()
        {
            return GetNumsS2(Codes);
        }
        public int Nums102()
        {
            return GetNumsS3(Codes);
        }
        public int Nums21()
        {
            return GetNumsZHI_XFS(Codes);
        }
        public int Nums22()
        {
            return GetNumsZHI_XFS(Codes);
        }
        public int Nums23()
        {
            return GetNumsZHI_XFS(Codes);
        }
        public int Nums103()
        {
            return GetNumsZHI_XFS(Codes);
        }
        public int Nums104()
        {
            return GetNumsRX2_ZHI_XFS(Codes);
        }
        public int Nums105()
        {
            return GetNumsRX_ZHI_XDS(Codes, Position);
        }
        public int Nums106()
        {
            return GetNumsRX2_ZHI_XHZ(Codes, Position);
        }
        public int Nums107()
        {
            return GetNumsRX2_ZU_XFS(Codes, Position);
        }
        public int Nums108()
        {
            return GetNumsRX2_ZU_XDS(Codes, Position);
        }
        public int Nums109()
        {
            return GetNumsRX2_ZU_XHZ(Codes, Position);
        }
        public int Nums110()
        {
            return GetNumsRX3_ZHI_XFS(Codes);
        }
        public int Nums111()
        {
            return GetNumsRX_ZHI_XDS(Codes, Position);
        }
        public int Nums112()
        {
            return GetNumsRX3_ZHI_XHZ(Codes, Position);
        }
        public int Nums113()
        {
            return GetNumsRX3_ZU3_XFS(Codes, Position);
        }
        public int Nums114()
        {
            return GetNumsRX_ZHI_XDS(Codes, Position);
        }
        public int Nums115()
        {
            return GetNumsC3S3(Codes, Position);
        }
        public int Nums116()
        {
            return GetNumsC3I3(Codes, Position);
        }
        public int Nums117()
        {
            return GetNumsC3I3(Codes, Position);
        }
        public int Nums118()
        {
            return GetNumsRX3_ZU_XHZ3(Codes, Position);
        }
        public int Nums119()
        {
            return GetNumsRX4_ZHI_XFS(Codes);
        }
        public int Nums120()
        {
            return GetNumsRX_ZHI_XDS(Codes, Position);
        }
        public int Nums121()
        {
            return GetNumsC4S4(Codes, Position);
        }
        public int Nums122()
        {
            return GetNumsC4S1q2(Codes, Position);
        }
        public int Nums123()
        {
            return GetNumsC4S2(Codes, Position);
        }
        public int Nums124()
        {
            return GetNumsC4S1q1(Codes, Position);
        }
    }
}
