using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameLibrary
{
    public partial class _11X5:Lottery
    {
        protected virtual string[][] GetCodeString2_3(string codes)
        {
            string[] temp = codes.Split('|');
            string[][] result = new string[][] { temp[0].Split('&'), temp[1].Split('&'), temp[2].Split('&') };
            return result;
        }
        protected virtual string[][] GetCodeString2_2(string codes)
        {
            string[] temp = codes.Split('|');
            string[][] result = new string[][] { temp[0].Split('&'), temp[1].Split('&')};
            return result;
        }
        protected virtual string[] GetCodeString(string codes)
        {
            string[] temp = codes.Split('&');
            return temp;
        }
        protected virtual bool IsNoRepeatCodes(string codes)
        {
            string[] codesArr = GetCodeString(codes);
            return codesArr.Length == codesArr.Distinct().Count();
        }
        protected virtual bool IsNoRepeatCodesDS(string codes)
        {
            string[] codesArr = GetCodeString(codes);
            foreach (var item in codesArr)
            {
                var temp = item.Split(' ');
                if (temp.Distinct().Count() != temp.Count())
                {
                    return false;
                }
            }
            return true; 
        }
        protected virtual bool IsNoRepeatCodes2_3(string codes)
        {
            string[][] codesArr = GetCodeString2_3(codes);
            foreach (string[] item in codesArr)
            {
                if (item.Length != item.Distinct().Count())
                {
                    return false;
                }
            }
            return true;
        }
        protected virtual bool IsNoRepeatCodes2_2(string codes)
        {
            string[][] codesArr = GetCodeString2_2(codes);
            foreach (string[] item in codesArr)
            {
                if (item.Length != item.Distinct().Count())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
