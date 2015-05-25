using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameLibrary
{
    public partial class _11X5
    {
        protected bool IsTDCodes(string codes)
        {
            string[][] temp = GetCodeString2_2(codes);
            foreach (var item in temp[0])
            {
                if (temp[1].Contains(item))
                {
                    return false;
                }
            }
            return true;
        }
        public bool Check1007()
        {
            if (reg1007.IsMatch(Codes)&&IsNoRepeatCodes2_3(Codes)) { return true; } return false;
        }
        public bool Check1008() { if (reg1008.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1009() { if (reg1009.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1010() { if (reg1010.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1011() { if (reg1011.IsMatch(Codes) && IsNoRepeatCodes2_2(Codes)) { return true; } return false; }
        public bool Check1012() { if (reg1012.IsMatch(Codes) && IsNoRepeatCodes2_2(Codes)) { return true; } return false; }
        public bool Check1013() { if (reg1013.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1014() { if (reg1014.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1015() { if (reg1015.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1016() { if (reg1016.IsMatch(Codes) && IsNoRepeatCodes2_2(Codes) && IsTDCodes(Codes)) { return true; } return false; }
        public bool Check1019() { if (reg1019.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1020() { if (reg1020.IsMatch(Codes) && IsNoRepeatCodes2_3(Codes)) { return true; } return false; }
        public bool Check1021() { if (reg1021.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1022() { if (reg1022.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1023() { if (reg1023.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1024() { if (reg1024.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1025() { if (reg1025.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1026() { if (reg1026.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1027() { if (reg1027.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1028() { if (reg1028.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1029() { if (reg1029.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1030() { if (reg1030.IsMatch(Codes) && IsNoRepeatCodes(Codes)) { return true; } return false; }
        public bool Check1031() { if (reg1031.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1032() { if (reg1032.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1033() { if (reg1033.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1034() { if (reg1034.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1035() { if (reg1035.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1036() { if (reg1036.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1037() { if (reg1037.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1038() { if (reg1038.IsMatch(Codes) && IsNoRepeatCodesDS(Codes)) { return true; } return false; }
        public bool Check1039() { if (reg1039.IsMatch(Codes) && IsNoRepeatCodes2_2(Codes) && IsTDCodes(Codes)) { return true; } return false; }
        public bool Check1040() { if (reg1040.IsMatch(Codes) && IsNoRepeatCodes2_2(Codes) && IsTDCodes(Codes)) { return true; } return false; }
        public bool Check1041() { if (reg1041.IsMatch(Codes) && IsNoRepeatCodes2_2(Codes) && IsTDCodes(Codes)) { return true; } return false; }
        public bool Check1042() { if (reg1042.IsMatch(Codes) && IsNoRepeatCodes2_2(Codes) && IsTDCodes(Codes)) { return true; } return false; }
        public bool Check1043() { if (reg1043.IsMatch(Codes) && IsNoRepeatCodes2_2(Codes) && IsTDCodes(Codes)) { return true; } return false; }
        public bool Check1044() { if (reg1044.IsMatch(Codes) && IsNoRepeatCodes2_2(Codes) && IsTDCodes(Codes)) { return true; } return false; }
        public bool Check1045() { if (reg1045.IsMatch(Codes) && IsNoRepeatCodes2_2(Codes) && IsTDCodes(Codes)) { return true; } return false; }
    }
}
