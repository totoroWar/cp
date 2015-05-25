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
        protected bool IsRX2Position()
        {
            if (regP.IsMatch(Position) && GetPositionString(Position).Length >= 2 && IsNoRepeatCodes(Position))
            {
                return true;
            }
            return false;
        }
        protected bool IsRX3Position()
        {
            if (regP.IsMatch(Position) && GetPositionString(Position).Length >= 3 && IsNoRepeatCodes(Position))
            {
                return true;
            }
            return false;
        }
        protected bool IsRX4Position()
        {
            if (regP.IsMatch(Position) && GetPositionString(Position).Length >= 4 && IsNoRepeatCodes(Position))
            {
                return true;
            }
            return false;
        }
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
        public bool Check30()
        {
            if (reg30.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check31()
        {
            return reg31.IsMatch(Codes);
        }
        public bool Check32()
        {
            if (reg32.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check33()
        {
            if (reg33.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check34()
        {
            if (reg34.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check35()
        {
            if (reg35.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check36()
        {
            if (reg36.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check37()
        {
            if (reg37.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check38()
        {
            if (reg38.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check39()
        {
            if (reg39.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check40()
        {
            if (reg40.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check41()
        {
            if (reg41.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check42()
        {
            if (reg42.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check43()
        {
            if (reg43.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check44()
        {
            return reg44.IsMatch(Codes);
        }
        public bool Check45()
        {
            if (reg45.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check46()
        {
            if (reg46.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check47()
        {
            if (reg47.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check48()
        {
            if (reg48.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check49()
        {
            if (reg49.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check50()
        {
            if (reg50.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check51()
        {
            if (reg51.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check52()
        {
            if (reg52.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check53()
        {
            if (reg53.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check54()
        {
            if (reg54.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check55()
        {
            if (reg55.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check56()
        {
            if (reg56.IsMatch(Codes) && IsCodesZU3(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check57()
        {
            if (reg57.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check58()
        {
            if (reg58.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsCodesZU6(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check59()
        {
            if (reg59.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsCodesZU3orZU6(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check60()
        {
            if (reg60.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check61()
        {
            return reg61.IsMatch(Codes);
        }
        public bool Check62()
        {
            if (reg62.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check63()
        {
            if (reg63.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check64()
        {
            if (reg64.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check65()
        {
            if (reg65.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check66()
        {
            if (reg66.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check67()
        {
            if (reg67.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check68()
        {
            if (reg68.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check69()
        {
            if (reg69.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check70()
        {
            if (reg70.IsMatch(Codes) && IsCodesZU3(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check71()
        {
            if (reg71.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check72()
        {
            if (reg72.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsCodesZU6(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check73()
        {
            if (reg73.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsCodesZU3orZU6(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check74()
        {
            if (reg74.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check75()
        {
            return reg75.IsMatch(Codes);
        }
        public bool Check76()
        {
            if (reg76.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check77()
        {
            if (reg77.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check78()
        {
            if (reg78.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check79()
        {
            if (reg79.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check80()
        {
            if (reg80.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check81()
        {
            if (reg81.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check82()
        {
            if (reg82.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check83()
        {
            if (reg83.IsMatch(Codes) && IsCodesZU2(Codes)&&IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check84()
        {
            if (reg84.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check85()
        {
            return reg85.IsMatch(Codes);
        }
        public bool Check86()
        {
            if (reg86.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check87()
        {
            if (reg87.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check88()
        {
            if (reg88.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check89()
        {
            if (reg89.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check90()
        {
            if (reg90.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check91()
        {
            if (reg91.IsMatch(Codes) && IsNoRepeatCodes(Codes)&&IsCodesZU2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check92()
        {
            if (reg92.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check93()
        {
            return reg93.IsMatch(Codes);
        }
        public bool Check94()
        {
            if (reg94.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check95()
        {
            if (reg95.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check96()
        {
            if (reg96.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check97()
        {
            if (reg97.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check98()
        {
            if (reg97.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check99()
        {
            if (reg99.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check100()
        {
            if (reg100.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check101()
        {
            if (reg101.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check102()
        {
            if (reg102.IsMatch(Codes) && IsNoRepeatCodes(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check21()
        {
            if (reg21.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check22()
        {
            if (reg22.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check23()
        {
            if (reg23.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check103()
        {
            if (reg103.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check104()
        {
            if (reg104.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check105()
        {
            if (reg105.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX2Position())
            {
                return true;
            }
            return false;
        }
        public bool Check106()
        {
            if (reg106.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX2Position())
            {
                return true;
            }
            return false;
        }
        public bool Check107()
        {
            if (reg107.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX2Position())
            {
                return true;
            }
            return false;
        }
        public bool Check108()
        {
            if (reg108.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsCodesZU2(Codes) && IsRX2Position())
            {
                return true;
            }
            return false;
        }
        public bool Check109()
        {
            if (reg109.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX2Position())
            {
                return true;
            }
            return false;
        }
        public bool Check110()
        {
            if (reg110.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check111()
        {
            if (reg111.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX3Position())
            {
                return true;
            }
            return false;
        }
        public bool Check112()
        {
            if (reg112.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX3Position())
            {
                return true;
            }
            return false;
        }
        public bool Check113()
        {
            if (reg113.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX3Position())
            {
                return true;
            }
            return false;
        }
        public bool Check114()
        {
            if (reg114.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsCodesZU3(Codes) && IsRX3Position())
            {
                return true;
            }
            return false;
        }
        public bool Check115()
        {
            if (reg115.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX3Position())
            {
                return true;
            }
            return false;
        }
        public bool Check116()
        {
            if (reg116.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsCodesZU6(Codes) && IsRX3Position())
            {
                return true;
            }
            return false;
        }
        public bool Check117()
        {
            if (reg117.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsCodesZU3orZU6(Codes) && IsRX3Position())
            {
                return true;
            }
            return false;
        }
        public bool Check118()
        {
            if (reg118.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX3Position())
            {
                return true;
            }
            return false;
        }
        public bool Check119()
        {
            if (reg119.IsMatch(Codes) && IsNoRepeatCodes2(Codes))
            {
                return true;
            }
            return false;
        }
        public bool Check120()
        {
            if (reg120.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX4Position())
            {
                return true;
            }
            return false;
        }
        public bool Check121()
        {
            if (reg121.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX4Position())
            {
                return true;
            }
            return false;
        }
        public bool Check122()
        {
            if (reg122.IsMatch(Codes) && IsNoRepeatCodes2(Codes) && IsRX4Position())
            {
                return true;
            }
            return false;
        }
        public bool Check123()
        {
            if (reg123.IsMatch(Codes) && IsNoRepeatCodes(Codes) && IsRX4Position())
            {
                return true;
            }
            return false;
        }
        public bool Check124()
        {
            if (reg124.IsMatch(Codes) && IsNoRepeatCodes2(Codes)&&IsRX4Position())
            {
                return true;
            }
            return false;
        }
    }
}
