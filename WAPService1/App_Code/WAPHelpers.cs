using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

/// <summary>
/// WAPHelpers 的摘要说明
/// </summary>
public class WAPHelpers
{

    string WapKey = "078A7F113A23E4061A1D312B5E7A315D";
    int LogTimeOut = 2; //2小时登陆时效

    GameServices.System serSystem = new GameServices.System();

    public void ReWritJSON(object oth)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Charset = "UTF-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
        HttpContext.Current.Response.Write(js.Serialize(oth));
        HttpContext.Current.Response.End();
    }


    public int PageSize
    {
        get
        {
            if (null == DicKV)
            {
                return 30;
            }
            return int.Parse(GetKV("AGU_PAGE_SIZE", true).cfg003);
        }
    }


    public Dictionary<string, DBModel.wgs027> DicKV
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

    public DBModel.wgs027 GetKV(string key, bool cache)
    {
        if (cache)
        {
            return DicKV[key];
        }
        var result = serSystem.GetKeyValue(key);
        return result;
    }



    /// <summary>
    /// 创建一个新的SCode
    /// </summary>
    /// <param name="uid">用户ID</param>
    /// <returns></returns>
    public string GetNewSCode(int uid)
    {
        string temp = "";
        temp = new Random().Next().ToString() + "|" +  uid.ToString() + "|" + DateTime.Now.AddHours(LogTimeOut).ToString("yyyy-MM-dd HH:mm:ss") + "|";
        string xtemp = "";
        xtemp = NETCommon.DEncrypt.Encrypt(temp, WapKey);
        xtemp = Regex.Replace(xtemp, @"/", "@");
        xtemp = Regex.Replace(xtemp, @"\+", "*");
        return xtemp;
    }

    /// <summary>
    /// 校验scode,若校验失败，则返回-1
    /// </summary>
    /// <param name="oldscode">要校验的scode</param>
    /// <param name="uid">连同旧scode一起传回的uid</param>
    /// <returns></returns>
    public string CheckSCode(string oldscode, int uid)
    {
        try
        {

            string xtemp = "";
            xtemp = oldscode;
            xtemp = Regex.Replace(xtemp, "@", @"/");
            xtemp = Regex.Replace(xtemp, @"\*", @"+");

            string temp = "";
            temp = NETCommon.DEncrypt.Decrypt(xtemp, WapKey);

            
            
            DateTime sTime = new DateTime();
            sTime = Convert.ToDateTime(Regex.Matches(temp, @".*?(\|)")[2].Value.Replace("|", ""));

            if (DateTime.Now > sTime) return "-1"; //登陆超时

            if (Regex.Matches(temp, @".*?(\|)")[1].Value.Replace("|", "") != uid.ToString()) return "-1"; //用户ID校验不一致

            return GetNewSCode(uid);
        }
        catch
        {
            return "-1"; //其它意外错误
        }

    }

    public ObjMsg getMst(string msg)
    {
        ObjMsg ob = new ObjMsg();
        ob.Code = "9999";
        ob.Message = msg;
        return ob;

    }

    public ObjMsg loginError(string msg)
    {
        ObjMsg ob = new ObjMsg();
        ob.Code = "0024";
        ob.Message = msg;
        return ob;

    }
}