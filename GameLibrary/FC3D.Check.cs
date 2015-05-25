using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameLibrary
{
    public partial class FC3D
    { 
        protected bool IsCodesZU2(string codes)
        {
            string[] codesArr = GetCodeString2(codes);
            foreach (var item in codesArr)
            {
                if (!(item.Distinct().Count() == 2))
                {
                    return false;
                }
            }
            return true;
        }
        protected bool IsCodesZU2()
        {
            string[] codesArr = GetCodeString2(Codes);
            foreach (var item in codesArr)
            {
                if (!(item.Distinct().Count() == 2))
                {
                    return false;
                }
            }
            return true;
        }
        protected bool IsCodesZU3(string codes)
        {
            string[] codesArr = GetCodeString2(codes);
            foreach (var item in codesArr)
            {
                if (!(item.Distinct().Count() == 2))
                {
                    return false;
                }
            }
            return true;
        }
        protected bool IsCodesZU3()
        {
            string[] codesArr = GetCodeString2(Codes);
            foreach (var item in codesArr)
            {
                if (!(item.Distinct().Count() == 2))
                {
                    return false;
                }
            }
            return true;
        }
        protected bool IsCodesZU3orZU6(string codes)
        {
            string[] codesArr = GetCodeString2(codes);
            foreach (var item in codesArr)
            {
                if (!(item.Distinct().Count() >= 2))
                {
                    return false;
                }
            }
            return true;
        }
        protected bool IsCodesZU3orZU6()
        {
            string[] codesArr = GetCodeString2(Codes);
            foreach (var item in codesArr)
            {
                if (!(item.Distinct().Count() >= 2))
                {
                    return false;
                }
            }
            return true;
        }
        protected bool IsCodesZU6(string codes)
        {
            string[] codesArr = GetCodeString2(codes);
            foreach (var item in codesArr)
            {
                if (!(item.Distinct().Count() == 3))
                {
                    return false;
                }
            }
            return true;
        }
        protected bool IsCodesZU6()
        {
            string[] codesArr = GetCodeString2(Codes);
            foreach (var item in codesArr)
            {
                if (!(item.Distinct().Count() == 3))
                {
                    return false;
                }
            }
            return true;
        }
        public bool Check1047()
        {
            if (reg1047.IsMatch(Codes) && IsNoRepeatCodes2())
            {
                return true;
            }
            return false;
        }
        public bool Check1048()
        {
            if (reg1048.IsMatch(Codes) && IsNoRepeatCodes())
            {
                return true;
            }
            return false;
        }
        public bool Check1049()
        {
            if (reg1049.IsMatch(Codes) && IsNoRepeatCodes())
            {
                return true;
            }
            return false;
        }
        public bool Check1051()
        {
            if (reg1051.IsMatch(Codes) && IsNoRepeatCodes())
            {
                return true;
            }
            return false;
        }
        public bool Check1052()
        {
            if (reg1052.IsMatch(Codes) && IsNoRepeatCodes() && IsCodesZU3())
            {
                return true;
            }
            return false;
        }
        public bool Check1053()
        {
            if (reg1053.IsMatch(Codes) && IsNoRepeatCodes())
            {
                return true;
            }
            return false;
        }
        public bool Check1054()
        {
            if (reg1054.IsMatch(Codes) && IsNoRepeatCodes() && IsCodesZU6())
            {
                return true;
            }
            return false;
        }
        public bool Check1055()
        {
            if (reg1055.IsMatch(Codes) && IsNoRepeatCodes() && IsCodesZU3orZU6())
            {
                return true;
            }
            return false;
        }
        public bool Check1056()
        {
            if (reg1056.IsMatch(Codes) && IsNoRepeatCodes())
            {
                return true;
            }
            return false;
        }
        public bool Check1058()
        {
            if (reg1058.IsMatch(Codes) && IsNoRepeatCodes2())
            {
                return true;
            }
            return false;
        }
        public bool Check1059()
        {
            if (reg1059.IsMatch(Codes) && IsNoRepeatCodes())
            {
                return true;
            }
            return false;
        }
        public bool Check1060()
        {
            if (reg1060.IsMatch(Codes) && IsNoRepeatCodes2())
            {
                return true;
            }
            return false;
        }
        public bool Check1061()
        {
            if (reg1061.IsMatch(Codes) && IsNoRepeatCodes())
            {
                return true;
            }
            return false;
        }
        public bool Check1063()
        {
            if (reg1063.IsMatch(Codes) && IsNoRepeatCodes2())
            {
                return true;
            }
            return false;
        }
        public bool Check1064()
        {
            if (reg1064.IsMatch(Codes) && IsNoRepeatCodes() && IsCodesZU2())
            {
                return true;
            }
            return false;
        }
        public bool Check1065()
        {
            if (reg1065.IsMatch(Codes) && IsNoRepeatCodes2())
            {
                return true;
            }
            return false;
        }
        public bool Check1066()
        {
            if (reg1066.IsMatch(Codes) && IsNoRepeatCodes() && IsCodesZU2())
            {
                return true;
            }
            return false;
        }
        public bool Check1068()
        {
            if (reg1068.IsMatch(Codes) && IsNoRepeatCodes2())
            {
                return true;
            }
            return false;
        }
        public bool Check1070()
        {
            if (reg1070.IsMatch(Codes) && IsNoRepeatCodes())
            {
                return true;
            }
            return false;
        }
        public bool Check1071()
        {
            if (reg1071.IsMatch(Codes) && IsNoRepeatCodes())
            {
                return true;
            }
            return false;
        }
        public bool Check1073()
        {
            if (reg1073.IsMatch(Codes) && IsNoRepeatCodes2())
            {
                return true;
            }
            return false;
        }
        public bool Check1074()
        {
            if (reg1074.IsMatch(Codes) && IsNoRepeatCodes2())
            {
                return true;
            }
            return false;
        }
    }
}
