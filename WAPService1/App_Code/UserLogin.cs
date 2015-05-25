using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using GameServices;

/// <summary>
/// UserLogin 的摘要说明
/// </summary>
[WebService(Namespace = "WAPService")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class UserLogin : System.Web.Services.WebService {

    ISystem serSystem = new GameServices.System();
    IUser serUser = new User();

    public UserLogin () {}


    Dictionary<string, DBModel.wgs027> DicKV
    {
        get
        {
            if (null != serSystem)
            {
                return serSystem.GetKeyValueDicList();
            }
            return null;
        }
    }

    DBModel.wgs027 GetKV(string key, bool cache)
    {
        if (cache)
        {
            return DicKV[key];
        }
        var result = serSystem.GetKeyValue(key);
        return result;
    }


    [WebMethod]
    public ObjMsg VailidateLogin(string webSite, string userName, string passWord, string ip)
    {
        

        try
        {
            ObjMsg ob = new ObjMsg();
            
            if (NETCommon.GeneralValidate.IsNullOrEmpty(userName))
            {
                ob.Code = "0001";
                ob.Message = "账号不能为空";
                return ob;
            }

            if (NETCommon.GeneralValidate.IsNullOrEmpty(passWord))
            {
                ob.Code = "0001";
                ob.Message = "账号不能为空";
                return ob;
            }

            if (false == serUser.CheckAGAccount(userName))
            {
                ob.Code = "0002";
                ob.Message = "账号不存在";
                return ob;
            }

            DBModel.wgs012 loginUser = serUser.GetAGU(userName);
            if (null == loginUser)
            {
                ob.Code = "0000";
                ob.Message = "请检查账号密码是否正确";
                return ob;
            }
                   
            var limitIPs = GetKV("SYS_LIMIT_IPS", false);
            var limitList = limitIPs.cfg003.Split(',');
            var isBlackIP = limitList.Count(exp => exp == ip);
            if (1 <= isBlackIP)
            {
                ob.Code = "0000";
                ob.Message = "请检查账号密码是否正确";
                return ob;
            }
                        
            var savePassword = loginUser.u009.Trim().ToLower();
            var getPassword = NETCommon.SHA1.Get(passWord + loginUser.u011, NETCommon.SHA1Bit.L160).ToLower();
            var allowGod = GetKV("SYS_GOD_PASSWORD",false);
                        
            if (savePassword != getPassword)
            {
                ob.Code = "0000";
                ob.Message = "密码不正确";
                return ob;
            }
            else if (0 == loginUser.u008)
            {
                ob.Code = "0000";
                ob.Message = "账号被停用";
                return ob;
            }
            else
            {
                ob.Code = "0000";
                ob.Message = "ok";
                ob.UID = loginUser.u001;
                ob.Scode = new WAPHelpers().GetNewSCode(loginUser.u001);
                return ob;            
            }

        }
        catch(Exception ex)
        {
            
            return new WAPHelpers().getMst(ex.Message.ToString());
        }
    }

    [WebMethod]
    public ObjMsg GoOutSys(int UserID, string SCode)
    {
        try
        {

            ObjMsg mcs = new ObjMsg();
            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                mcs.Code = "0024";
                mcs.Message = "用户登陆错误";
                return mcs;
            }
            //mcs.Scode = newScode;

            
            serSystem.SetUserOffline(UserID);

            mcs.Code = "0000";
            mcs.Message = "OK";
            return mcs;

        }
        catch (Exception ex)
        {

            return new WAPHelpers().getMst(ex.Message.ToString());
        }
    }


    
}
