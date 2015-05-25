using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using _NWC = NETCommon;
using GameServices;
using System.Text.RegularExpressions;


/// <summary>
/// UserMsg 的摘要说明
/// </summary>
[WebService(Namespace = "WAPService")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class UserMsg : System.Web.Services.WebService {
    GameServices.Finance serFinance = new GameServices.Finance();
    GameServices.IUser serUser = new GameServices.User();
    GameServices.System serSystem = new GameServices.System();

    public UserMsg () {
    }

    [WebMethod]
    public string GetMsgTxt(int UserID, int PIndex, string Code)
    {
        return "OK";
    }



    [WebMethod]
    public string GetMsgCount(int UserID, string Code)
    {
        return "OK";
    }

    [WebMethod]
    public AbotList GetAboutList(int UserID, string SCode)
    {
        try
        {
            AbotList al = new AbotList();
            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                al.Code = "0024";
                al.Message = "用户登陆错误";
                return al;
            }
            al.Scode = newScode;
            al.GlobalNotify = new WAPHelpers().GetKV("SYS_NOTIFY_CONTENT", false).cfg003;


            var ntyList = (List<DBModel.wgs040>)_NWC.GeneralCache.Get("NotifyList");
            if (null == ntyList)
            {
                ntyList = serSystem.GetNotifyList(1);
                if (null != ntyList)
                {
                    _NWC.GeneralCache.Set("NotifyList", ntyList, DateTimeOffset.Now.AddSeconds(int.Parse(new WAPHelpers().GetKV("AGU_NOTIFY_CACHE_TIME", false).cfg003)));
                }
            }

            AbotType at;
            List<AbotType> atl = new List<AbotType>();
            foreach (var item in ntyList)
            {
                at = new AbotType();
                at.MID = item.nt001;
                at.MTitle = item.nt002;
                at.MTxt = item.nt003;
                at.TTime = item.nt004;
                atl.Add(at);
            }

            al.ActLists = atl;

            al.Code = "9999";
            al.Message = "OK";

            return al;
        }
        catch (Exception ex)
        {
            AbotList al = new AbotList();
            al.Code = "9999";
            al.Message = ex.Message.ToString();

            return al;
        }
    }

    [WebMethod]
    public HelloWordInfo GetHelloWord(int UserID, string Code)
    {
        try
        {
            HelloWordInfo wi = new HelloWordInfo();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(Code, UserID);
            if (newScode == "-1")
            {
                wi.Code = "0024";
                wi.Message = "用户登陆错误";
                return wi;
            }
            wi.Scode = newScode;

            


            return wi;
        }
        catch (Exception ex)
        {
            HelloWordInfo wi = new HelloWordInfo();
            wi.Code = "9999";
            wi.Message = ex.Message.ToString();
            return wi;
        }
    }

    [WebMethod]
    public HelloWordInfo UPHelloWord(int UserID, string Code, string TheTxt)
    {
        try
        {
            HelloWordInfo wi = new HelloWordInfo();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(Code, UserID);
            if (newScode == "-1")
            {
                wi.Code = "0024";
                wi.Message = "用户登陆错误";
                return wi;
            }
            wi.Scode = newScode;

            string login_message = TheTxt;
            if (_NWC.GeneralValidate.IsNullOrEmpty(login_message))
            {
                wi.Code = "0061";
                wi.Message = "问候语不能为空";
                return wi;
            }

            MR mr = serUser.UpdateGUALM(UserID, login_message);
            if (1 == mr.Code)
            {
                wi.Code = "0000";
                wi.Message = "OK";
            }
            else
            {
                wi.Code = "9999";
                wi.Message = mr.Message;
            }


            return wi;
        }
        catch (Exception ex)
        {
            HelloWordInfo wi = new HelloWordInfo();
            wi.Code = "9999";
            wi.Message = ex.Message.ToString();
            return wi;
        }
    }


    
}
