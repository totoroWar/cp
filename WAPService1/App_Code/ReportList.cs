using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using GameServices;
using _NWC = NETCommon;
/// <summary>
/// Report 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class ReportList : System.Web.Services.WebService {

    public ReportList()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public ObjMsg GetBearkList(int userID, string SCode, DateTime STime, DateTime ETime)
    {
        GameServices.IFinance serFinance = new GameServices.Finance();
        Reports ob = new Reports();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, userID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        var pageURL = string.Empty;
        List<DBModel.wgs042> dayReportList = serFinance.GetDayReport(userID, string.Empty, -1, (DateTime?)STime, (DateTime?)ETime);
        
        List<Report> reportList = null;
        foreach (var item in dayReportList)
        {
            Report report = new Report();
            report.MName = item.u002.Trim();
            report.MUpTime = item.dr003;
            report.MAddMoney = item.dr005;
            report.MPAYMoney = item.dr004;
            report.MBonus = item.dr006;
            report.MBack = item.dr007;
            report.MGetMoney = item.dr010;
            report.MPostMoney = item.dr011;
            report.MBonusMoney = item.dr016;
            report.MGoInMoney = item.dr017;
            report.MGoOutoney = item.dr018;
            report.MGetPints = item.dr008;
            report.MConsumePints = item.dr013;
            report.MProfit = (item.dr006) - (item.dr004);
            reportList.Add(report);
        }
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        ob.ReportList = reportList;
        return ob;
    }
     [WebMethod]
    public ObjMsg GetBearkDownList(int userID, string SCode, DateTime STime, DateTime ETime, int SelectType, string UCode, int UPayType, decimal UPayMoney, int UAddType, decimal UAddMoney, int UWinType, decimal UWinMoney)
    {
        GameServices.IUser serUser = new GameServices.User();
        GameServices.IFinance serFinance = new GameServices.Finance();
        Reports ob = new Reports();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, userID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        var _dts = _NWC.GeneralValidate.IsDatetime(STime.ToString()) ? DateTime.Parse(STime.ToString()) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00");
        var _dte = _NWC.GeneralValidate.IsDatetime(ETime.ToString()) ? DateTime.Parse(ETime.ToString()) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59.999");
        var _type = SelectType;
        var _acct = UCode;
        var _om = UPayType;
        var _omm = UPayMoney;
        var _cm = UAddType;
        var _cmm = UAddMoney;
        var _pm = UWinType;
        var _pmm = UWinMoney;
        List<DBModel.wgs042> details = null;
        var curNewUser = serUser.GetAGU(userID);
        if (4 == SelectType)
        {
            details = serFinance.GetDRExt(0, userID, _dts, _dte, _type, _acct, 0, string.Empty, userID, _om, _omm, _cm, _cmm, _pm, _pmm);
        }
        else if (5 == SelectType)
        {
            details = serFinance.GetDRExt(0, userID, STime, ETime, SelectType, string.Empty, 0, curNewUser.u002.Trim(), 0, _om, _omm, _cm, _cmm, _pm, _pmm);
        }
        var drdList = details;
        var total = from dl in drdList
                    group dl by new { dl.u002, dl.u001 } into ndl
                    select new
                    {
                        u001 = ndl.Key.u001,
                        u002 = ndl.Key.u002,
                        dr004 = ndl.Sum(exp => exp.dr004),
                        dr005 = ndl.Sum(exp => exp.dr005),
                        dr006 = ndl.Sum(exp => exp.dr006),
                        dr007 = ndl.Sum(exp => exp.dr007),
                        dr008 = ndl.Sum(exp => exp.dr008),
                        dr009 = ndl.Sum(exp => exp.dr009),
                        dr010 = ndl.Sum(exp => exp.dr010),
                        dr011 = ndl.Sum(exp => exp.dr011),
                        dr012 = ndl.Sum(exp => exp.dr012),
                        dr013 = ndl.Sum(exp => exp.dr013),
                        dr014 = ndl.Sum(exp => exp.dr014),
                        dr015 = ndl.Sum(exp => exp.dr015),
                        dr016 = ndl.Sum(exp => exp.dr016),
                        dr017 = ndl.Sum(exp => exp.dr017),
                        dr018 = ndl.Sum(exp => exp.dr018)
                    };
        
        List<Report> reportList = null;
        if (null != total)
        {
            total = total.OrderByDescending(exp => exp.dr004).ToList();
            
            foreach (var item in total)
            {
                Report report = new Report();
                report.MName = item.u002.Trim();
                //report.MUpTime = item.dr003;
                report.MAddMoney = item.dr005;
                report.MPAYMoney = item.dr004;
                report.MBonus = item.dr006;
                report.MBack = item.dr007;
                report.MGetMoney = item.dr010;
                report.MPostMoney = item.dr011;
                report.MBonusMoney = item.dr016;
                report.MGoInMoney = item.dr017;
                report.MGoOutoney = item.dr018;
                report.MGetPints = item.dr008;
                report.MConsumePints = item.dr013;
                report.MProfit = (item.dr006) - (item.dr004);
                reportList.Add(report);
            }
        }
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        ob.ReportList = reportList;
        return ob;
    }
}
