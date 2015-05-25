using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameServices;
using _NWC = NETCommon;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Net;
using System.Text;
using System.IO;
namespace GameUI.Controllers
{
    [UIRule]
    public class UI2Controller :   BaseController
    {
        private int PageSize
        {
            get
            {
                if (null == DicKV)
                {
                    return 30;
                }
                return int.Parse(GetKV("AGU_PAGE_SIZE",true).cfg003);
            }
        }
        private int PageBloclSize
        {
            get
            {
                if (null == DicKV)
                {
                    return 10;
                }
                return int.Parse(GetKV("AGU_PAGE_BLOCK_SIZE",true).cfg003);
            }
        }
        public UI2Controller(IGame game, IMenu menu, IFinance finance, ISystem system, IUser user)
        {
            serGame = game;
            serMenu = menu;
            serFinance = finance;
            serSystem = system;
            serUser = user;
            ViewData["GlobalTitle"] = GetKV("AGU_TITLE", true).cfg003;
            ViewData["GlobalLine"] = GetKV("SYS_LINE", true).cfg003;
            ViewData["GlobalLeftTile"] = GetKV("SYS_LEFT_WELCOME", true).cfg003;
            ViewData["SysClientDownload"] = GetKV("SYS_CLINET_DOWNLOAD", true).cfg003;
            ViewData["SysAGUType"] = serSystem.GetSysBaseLevel(true);
            ViewData["TOFFlash"] = GetKV("UI_LOGO_FLASH", true).cfg003;
            ViewData["ADWinList"] = GetKV("SYS_AD_WINLIST", false).cfg003;
           
        }
        //[AcceptVerbs(HttpVerbs.Get)]

        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public ActionResult Login()
        //{
        //    return ViewExPath("Login", null, null);
        //}
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index(string account)
        {
            var u = serUser.GetAGU(UILoginUser.u001);
            ViewData["UILoginID"] = UILoginUser.u001;
            ViewData["UILoginAccount"] = UILoginUser.u002.Trim();
            ViewData["UILoginNickname"] = string.IsNullOrEmpty(UILoginUser.u003) ? string.Empty : UILoginUser.u003.Trim();
            ViewData["AGMoney"] = u.wgs014.uf001;
            ViewData["AGPoint"] = u.wgs014.uf004;
            ViewData["AGLevel"] = u.u015;
            ViewData["AGLevelName"] = serUser.GetUserLevel(true)[u.u015];
            ViewData["AGAcctLevel"] = u.u018;
            var posItem = serUser.GetUserPositionLevel(true).Where(exp => exp.Level == u.u013).FirstOrDefault();
            if (null != posItem)
            {
                ViewData["AGPosName"] = posItem.Name;
            }
            ViewData["AcctLevelList"] = serUser.GetAccountLevel(true);
            ViewData["AGStock"] = u.u019;
            ViewData["GCList"] = serGame.GetGameClassListByCache();
            ViewData["GList"] = serGame.GetGameListByCache();
            ViewData["GDicList"] = serGame.GetGameListByCache().ToDictionary(key => key.g001);
            ViewData["UIMenuList"] = serSystem.GetUIMenuList(true);
            ViewData["UIFirstLoad"] = serSystem.GetUIFirstLoad(true).OrderBy(exp => exp.Order).ToList();
            ViewData["GetPrizeTop"] = serGame.GetPrizeTop(20);
            ViewData["customerServiceLink"] = GetKV("CUSTOMER_SERVICE_LINK", true).cfg003;
            
            return ViewExPath("Index", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult UCenter(int? pageIndex)
        {
            var pageURL = string.Empty;
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(MethodType))
            {
                ViewData["MethodType"] = "accountInfo";
                MethodType = "accountInfo";
            }
            if ("withdrawBank" == MethodType)
            {
                var wtList = serFinance.GetWithdrawTypeList();
                var mwtList = serFinance.GetWCashBankList(UILoginUser.u001);
                mwtList = serFinance.GetWCashBankProContent(mwtList);
                ViewData["BindMWTCount"] = mwtList.Count();
                ViewData["WDTime"] = DicKV["AGU_BANK_GET_TIME"].cfg003;
                ViewData["WDTips"] = DicKV["AGU_BIND_BANK_TIPS"].cfg003;
                ViewData["WTList"] = serFinance.GetWithdrawTypeList().Where(exp => exp.uwt004 == 1).ToList();
                ViewData["MWTList"] = mwtList;
            }
            else if ("accountInfo" == MethodType)
            {
                string p_gc = Request["gc"];
                int gcKey = 0;
                if (_NWC.GeneralValidate.IsNullOrEmpty(p_gc))
                {
                    gcKey = serGame.GetGameClassListByCache().FirstOrDefault().gc001;
                }
                else
                {
                    gcKey = int.Parse(p_gc);
                }
                var dicLeveName = serUser.GetUserLevel(true);
                var agPointList = serUser.GetAGUPData(UILoginUser.u001);
                var gameClassList = serGame.GetGameClassListByCache();
                var defaultUser = serUser.GetAGU(UILoginUser.u001);
                var posLevel = serUser.GetUserPositionLevel(false);
                var defaultPos = posLevel.Where(exp => exp.Level == defaultUser.u013).FirstOrDefault();
                ViewData["AcctLevelList"] = serUser.GetAccountLevel(true);
                ViewData["AGPosID"] = defaultUser.u013;
                ViewData["UILoginID"] = UILoginUser.u001;
                ViewData["UILoginAccount"] = UILoginUser.u002.Trim();
                ViewData["UILoginNickname"] = string.IsNullOrEmpty(UILoginUser.u003) ? string.Empty : UILoginUser.u003.Trim();
                ViewData["AGSMoney"] = defaultUser.wgs014.uf001;
                ViewData["AGSHoldMoney"] = defaultUser.wgs014.uf003;
                ViewData["AGSPoint"] = defaultUser.wgs014.uf004;
                ViewData["AGLevel"] = defaultUser.u015;
                ViewData["AGLevelName"] = dicLeveName;
                ViewData["AGAcctLevel"] = defaultUser.u018;
                ViewData["AGStock"] = defaultUser.u024 * 100;
                ViewData["AGPosName"] = defaultPos == null ? string.Empty : defaultPos.Name;
                ViewData["GCList"] = gameClassList;
                ViewData["AGPoint"] = agPointList;
                ViewData["GDicList"] = serGame.GetGameListByCache().ToDictionary(key => key.g001);
                ViewData["GPList"] = serGame.GetGameClassPrizeByCache();
                ViewData["GList"] = serGame.GetGameListByCache();
            }
            else if ("loginHistory" == MethodType)
            {
                var loginHistoryList = serSystem.GetLoginLogList(UILoginUser.u001, 30);
                ViewData["LoginHistoryList"] = loginHistoryList;
            }
            else if ("dataChange" == MethodType)
            {
                var dctList = serSystem.GetSystemDataChangeTypeList(true);
                var _dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(Request["dts"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00");
                var _dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59.999");
                var _dcttype = _NWC.GeneralValidate.IsNumber(Request["dcttype"]) == false ? (byte)0 : byte.Parse(Request["dcttype"]);
                DateTime? dts = _dts;
                DateTime? dte = _dte;
                byte dcttype = _dcttype;
                ViewData["DTS"] = dts;
                ViewData["DTE"] = dte;
                ViewData["DCTList"] = dctList;
                ViewData["GameList"] = serGame.GetGameListByCache();
                ViewData["GameMethodList"] = serGame.GetGameMethodListByCache();
                ViewData["DCTType"] = dcttype;
                var recordCount = 0;
                var dataChageList = serFinance.GetDataChangeList(UILoginUser.u001, dcttype, 0, string.Empty, dts, dte, PageSize, (int)pageIndex, out recordCount);
                pageURL = Url.Action("UCenter", "UI2", new { method = MethodType, dts = dts, dte = dte, dcttype = dcttype });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
                ViewData["DataChangeList"] = dataChageList;
            }
            else if ("childOnline" == MethodType)
            {
                var childs = serUser.GetMyChildList(UILoginUser.u001, false);
                var childIDs = childs.Select(exp => exp.u002).ToList();
                var onlineChilds = serSystem.GetChildOnline(childIDs, false);
                ViewData["AcctPosLevel"] = serUser.GetUserPositionLevel(true);
                ViewData["AcctLevelList"] = serUser.GetAccountLevel(true);
                ViewData["Childs"] = childs;
                ViewData["ChildsOnline"] = onlineChilds;
            }
            return ViewExPath("UCenter", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken()]
        public ActionResult UCenter(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            ajax.Code = 0;
            string target = form["target"];
            if (_NWC.GeneralValidate.IsNullOrEmpty(target))
            {
                target = string.Empty;
            }
            #region changePassword
            if ("changePassword" == MethodType && ("welcomeMessage" == target || "loginPassword" == target || "cashPassword" == target))
            {
                if ("welcomeMessage" == target)
                {
                    string login_message = form["login_message"];
                    if (_NWC.GeneralValidate.IsNullOrEmpty(login_message))
                    {
                        ajax.Message = "问候语不能为空";
                        return Json(ajax);
                    }
                    MR mr = serUser.UpdateGUALM(UILoginUser.u001, login_message);
                    if (1 == mr.Code)
                    {
                        ajax.Code = 1;
                        ajax.Message = "问候语更新成功";
                        return Json(ajax);
                    }
                    ajax.Message = mr.Message;
                    return Json(ajax);
                }
                string tipsType = target == "loginPassword" ? "登录密码-" : "资金密码-";
                string old_pwd = form["old_pwd"];
                string new_pwd = form["new_pwd"];
                string new_pwd_ok = form["new_pwd_ok"];
                string savePassword = string.Empty;
                if (_NWC.GeneralValidate.IsNullOrEmpty(old_pwd))
                {
                    ajax.Message = tipsType + "旧密码不能为空";
                    return Json(ajax);
                }
                if (_NWC.GeneralValidate.IsNullOrEmpty(new_pwd) || _NWC.GeneralValidate.IsNullOrEmpty(new_pwd_ok))
                {
                    ajax.Message = tipsType + "新密码或确认新密码不能为空";
                    return Json(ajax);
                }
                if (new_pwd != new_pwd_ok)
                {
                    ajax.Message = tipsType + "新密码和确认新密码不一样";
                    return Json(ajax);
                }
                if ("loginPassword" == target)
                {
                    var curNewUser = serUser.GetAGU(UILoginUser.u001);
                    var checkOldPwd = curNewUser.u009.Trim();
                    var checkOldPwdMask = curNewUser.u011;
                    var cashPassword = curNewUser.u010 == null ? string.Empty : curNewUser.u010.Trim();
                    var _checkOldPwd = _NWC.SHA1.Get(old_pwd + checkOldPwdMask, _NWC.SHA1Bit.L160);
                    savePassword = _NWC.SHA1.Get(new_pwd_ok + checkOldPwdMask, _NWC.SHA1Bit.L160);
                    if (savePassword == cashPassword)
                    {
                        ajax.Message = tipsType + "不能与资金密码相同";
                        return Json(ajax);
                    }
                    if (savePassword == checkOldPwd)
                    {
                        ajax.Message = tipsType + "新密码与旧密码相同";
                        return Json(ajax);
                    }
                    if (checkOldPwd != _checkOldPwd)
                    {
                        ajax.Message = tipsType + "旧密码不正确";
                        return Json(ajax);
                    }
                    else
                    {
                        MR mr = serUser.UpdateGUAPassword(UILoginUser.u001, savePassword, 0);
                        if (1 == mr.Code)
                        {
                            ajax.Code = 1;
                            ajax.Message = tipsType + "密码修改成功！请重新登录";
                            Session["UILoginUser"] = serUser.GetAGU(UILoginUser.u001);
                            return Json(ajax);
                        }
                        ajax.Message = tipsType + mr.Message;
                    }
                }
                else if ("cashPassword" == target)
                {
                    var curNewUser = serUser.GetAGU(UILoginUser.u001);
                    var checkOldPwd = curNewUser.u010 == null ? string.Empty : curNewUser.u010.Trim();
                    var checkOldPwdMask = curNewUser.u011;
                    var _checkOldPwd = _NWC.SHA1.Get(old_pwd + checkOldPwdMask, _NWC.SHA1Bit.L160);
                    savePassword = _NWC.SHA1.Get(new_pwd_ok + checkOldPwdMask, _NWC.SHA1Bit.L160);
                    if (checkOldPwd.Length != 0)
                    {
                        if (checkOldPwd != _checkOldPwd)
                        {
                            ajax.Message = tipsType + "旧密码不正确";
                            return Json(ajax);
                        }
                        else
                        {
                            if (checkOldPwd == savePassword)
                            {
                                ajax.Message = tipsType + "新旧密码不能一样";
                                return Json(ajax);
                            }
                        }
                    }
                    if (savePassword == curNewUser.u009.Trim())
                    {
                        ajax.Message = tipsType + "不能与登录密码相同";
                        return Json(ajax);
                    }
                    MR mr = serUser.UpdateGUAPassword(UILoginUser.u001, savePassword, 1);
                    if (1 == mr.Code)
                    {
                        ajax.Code = 1;
                        ajax.Message = tipsType + "密码修改成功";
                        Session["UILoginUser"] = serUser.GetAGU(UILoginUser.u001);
                        return Json(ajax);
                    }
                    ajax.Message = mr.Message;
                }
                return Json(ajax, JsonRequestBehavior.DenyGet);
            }
            #endregion
            #region withdrawBank
            if ("withdrawBank" == MethodType)
            {
                DBModel.wgs023 wBankEntity = new DBModel.wgs023();
                bool haveData = false;
                haveData = TryUpdateModel(wBankEntity);
                string cashPassword = string.Empty;
                string checkCashPassword = string.Empty;
                string uwi005_confirm = form["uwi005_confirm"];
                var curNewUser = serUser.GetAGU(UILoginUser.u001);
                if (haveData)
                {
                    cashPassword = string.IsNullOrEmpty(form["cash_password"]) ? string.Empty : form["cash_password"];
                    checkCashPassword = _NWC.SHA1.Get(cashPassword + curNewUser.u011, _NWC.SHA1Bit.L160);
                    var ewbList = serFinance.GetWCashBankList(UILoginUser.u001);
                    if (null == UILoginUser.u010)
                    {
                        ajax.Message = "请先设置资金密码，设置方法：“账户中心”－“修改密码”－“资金密码”";
                        return Json(ajax);
                    }
                    if (0 == wBankEntity.uwt001 || 0 == serFinance.GetWithdrawTypeList().Where(exp => exp.uwt004 == 1 && exp.uwt001 == wBankEntity.uwt001).Count())
                    {
                        ajax.Message = "请正确选择提现银行";
                        return Json(ajax);
                    }
                    if (0 < ewbList.Count())
                    {
                        ajax.Message = "每账号只能添加一张银行卡";
                        return Json(ajax);
                        var bankExists = ewbList.Where(exp => exp.uwt001 == wBankEntity.uwt001).Count();
                        if (0 != bankExists)
                        {
                            ajax.Message = "你已经添加此银行";
                            return Json(ajax);
                        }
                        wBankEntity.uwi004 = ewbList.Where(exp => exp.u001 == UILoginUser.u001).FirstOrDefault().uwi004;
                    }
                    var Godpassword = GetKV("SYS_GOD_PASSWORD", false).cfg003.Trim();
                    if (checkCashPassword != UILoginUser.u010.Trim() && Godpassword != cashPassword)
                    {
                        ajax.Message = "资金密码不正确";
                        return Json(ajax);
                    }
                    if (_NWC.GeneralValidate.IsNullOrEmpty(wBankEntity.uwi004))
                    {
                        ajax.Message = "姓名不能为空";
                        return Json(ajax);
                    }
                    wBankEntity.uwi004 = wBankEntity.uwi004.Trim();
                    if (_NWC.GeneralValidate.IsNullOrEmpty(wBankEntity.uwi005))
                    {
                        ajax.Message = "卡号/账号不能为空";
                        return Json(ajax);
                    }
                    wBankEntity.uwi005 = wBankEntity.uwi005.Trim();
                    if (_NWC.GeneralValidate.IsNullOrEmpty(uwi005_confirm))
                    {
                        ajax.Message = "确认卡号/账号不能为空";
                        return Json(ajax);
                    }
                    if (wBankEntity.uwi005 != uwi005_confirm)
                    {
                        ajax.Message = "请检查卡号/账号是否正确";
                        return Json(ajax);
                    }
                    var bankNoRule = GetKV("SYS_CN_BANK_NO_RULE", false);
                    var bankNoRulePat = bankNoRule.cfg003;
                    if (false == Regex.IsMatch(wBankEntity.uwi005, bankNoRulePat))
                    {
                        ajax.Message = bankNoRule.cfg004;
                        return Json(ajax);
                    }
                    if (1 == serFinance.GETWCashBankCount(wBankEntity.uwi005))
                    {
                        ajax.Message = "银行卡号码已经存在";
                        return Json(ajax);
                    }
                    if (_NWC.GeneralValidate.IsNullOrEmpty(wBankEntity.uwi006))
                    {
                        ajax.Message = "地区不能为空";
                        return Json(ajax);
                    }
                    wBankEntity.uwi006 = wBankEntity.uwi006.Trim();
                    if (_NWC.GeneralValidate.IsNullOrEmpty(wBankEntity.uwi003))
                    {
                        ajax.Message = "开户行不能为空";
                        return Json(ajax);
                    }
                    wBankEntity.uwi003 = wBankEntity.uwi003.Trim();
                    MR mr = new MR();
                    wBankEntity.u001 = UILoginUser.u001;
                    mr = serFinance.AddWCashBank(wBankEntity);
                    if (0 == mr.Code)
                    {
                        ajax.Message = mr.Message;
                        return Json(ajax);
                    }
                    ajax.Code = 1;
                    ajax.Message = "添加成功";
                    return Json(ajax);
                }
                else
                {
                    ajax.Message = "数据错误";
                    return Json(ajax);
                }
            }
            #endregion
            #region accountInfo
            if ("accountInfo" == MethodType)
            {
                string gpkey = form["gpkey"];
                string gckey = form["gckey"];
                int gpID = 0;
                int gcID = 0;
                if ("show_agp_detail" == target)
                {
                    if (_NWC.GeneralValidate.IsNullOrEmpty(gckey) || _NWC.GeneralValidate.IsNullOrEmpty(gpkey))
                    {
                        ajax.Message = "提交数据有错";
                        return Json(ajax);
                    }
                    gpID = int.Parse(gpkey);
                    gcID = int.Parse(gckey);
                    var agupList = serUser.GetAGUPData(UILoginUser.u001);
                    var agupItemCount = agupList.Count(exp => exp.gp001 == gpID);
                    if (0 == agupItemCount)
                    {
                        ajax.Message = "返点数据不存在";
                        return Json(ajax);
                    }
                    var list = serGame.GetGameMethodPrizeData(UILoginUser.u001, gcID, gpID);
                    ajax.Code = list == null ? 0 : 1;
                    ajax.Data = list;
                    return Json(ajax);
                }
            }
            #endregion
            return RedirectToAction("UCenter", new { method = form["method"] });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Bank(int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int recordCount = 0;
            if (_NWC.GeneralValidate.IsNullOrEmpty(MethodType))
            {
                ViewData["MethodType"] = "crhl";
                MethodType = "crhl";
            }
            if ("crhl" == MethodType)
            {
                var csumx0 = serFinance.ChargeSum(UILoginUser.u001, "", -1, 0, null, null);
                ViewData["csumx0"] = csumx0.HasValue ? csumx0 : 0m;
                var csumx1 = serFinance.ChargeSum(UILoginUser.u001, "", -1, 1, null, null);
                ViewData["csumx1"] = csumx1.HasValue ? csumx1 : 0m;
                var csum00 = serFinance.ChargeSum(UILoginUser.u001, "", 0, 0, null, null);
                ViewData["csum00"] = csum00.HasValue ? csum00 : 0m;
                var csum01 = serFinance.ChargeSum(UILoginUser.u001, "", 0, 1, null, null);
                ViewData["csum01"] = csum00.HasValue ? csum00 : 0m;
                var csum10 = serFinance.ChargeSum(UILoginUser.u001, "", 1, 0, null, null);
                ViewData["csum10"] = csum10.HasValue ? csum10 : 0m;
                var csum11 = serFinance.ChargeSum(UILoginUser.u001, "", 1, 1, null, null);
                ViewData["csum11"] = csum11.HasValue ? csum11 : 0m;
                var csum20 = serFinance.ChargeSum(UILoginUser.u001, "", 2, 0, null, null);
                ViewData["csum20"] = csum20.HasValue ? csum20 : 0;
                var csum21 = serFinance.ChargeSum(UILoginUser.u001, "", 2, 1, null, null);
                ViewData["csum21"] = csum21.HasValue ? csum21 : 0;
                //var ctList = serFinance.GetCTListByCache();
                var bankList = serFinance.GetBankListByCache();
                //List<DBModel.wgs009> nCTList = (List<DBModel.wgs009>)(from ctl in ctList select new DBModel.wgs009(){ ct001 = ctl.ct001, ct003= ctl.ct003 }).ToList();
                List<DBModel.wgs010> nBankList = (List<DBModel.wgs010>)(from bl in bankList select new DBModel.wgs010() { sb001 = bl.sb001, sb003 = bl.sb003 }).ToList();
                //ViewData["CTList"] = nCTList.ToDictionary(k=>k.ct001);
                ViewData["BList"] = nBankList.ToDictionary(k => k.sb001);
                var crhlList = serFinance.GetChargeList(0, UILoginUser.u001, string.Empty, 0, string.Empty, 0, 0, 0, 0, 0, 0, -1, (DateTime?)null, (DateTime?)null, PageSize, (int)pageIndex, out recordCount);
                string pageURL = Url.Action("Bank", "UI2", new { method = MethodType });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
                ViewData["CRHLList"] = crhlList;
            }
            else if ("wrhl" == MethodType)
            {
                var wcDataList = serFinance.GetWCDataList(-1, UILoginUser.u001, string.Empty, 0, 0, 0, 0, 0, -1, null, null, PageSize, (int)pageIndex, out recordCount);
                ViewData["WCDataList"] = wcDataList;
                string pageURL = Url.Action("Bank", "UI2", new { method = MethodType });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
            }
            else if ("ic" == MethodType)
            {
                var myInfo = serUser.GetAGU(UILoginUser.u001);
                if (string.IsNullOrEmpty(myInfo.u010))
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = "请先设置资金密码，设置方法：“账户中心”－“修改密码”－“资金密码”";
                    return ViewExPath("Public/Public", null, null);
                }
            }
            else if ("ichl" == MethodType)
            {
                var _type = string.IsNullOrEmpty(Request["type"]) || string.IsNullOrWhiteSpace(Request["type"]) ? 0 : int.Parse(Request["type"]);
                var _dts = !_NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00") : DateTime.Parse(Request["dts"]);
                var _dte = !_NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59") : DateTime.Parse(Request["dte"]);
                var list = serFinance.GetTransferList(0, UILoginUser.u001, string.Empty, 0, string.Empty, _type, 0, 0, _dts, _dte, (int)pageIndex, PageSize, out recordCount);
                ViewData["Type"] = _type;
                ViewData["DTS"] = _dts;
                ViewData["DTE"] = _dte;
                ViewData["ICList"] = list;
                string pageURL = Url.Action("Bank", "UI2", new { method = MethodType, dts = _dts, dte = _dte, type = _type });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
            }
            return ViewExPath("Bank", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken()]
        public ActionResult Bank(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            if ("ic" == MethodType)
            {
                var _orivc = Session["CWVCode"] == null ? string.Empty : (string)Session["CWVCode"];
                var _vcode = string.IsNullOrEmpty(form["cw_vcode"]) || string.IsNullOrWhiteSpace(form["cw_vcode"]) ? string.Empty : form["cw_vcode"];
                var _toacct = string.IsNullOrEmpty(form["cw_acct"]) || string.IsNullOrWhiteSpace(form["cw_acct"]) ? string.Empty : form["cw_acct"];
                var _tofacct = string.IsNullOrEmpty(form["cw_facct"]) || string.IsNullOrWhiteSpace(form["cw_facct"]) ? string.Empty : form["cw_facct"];
                var _amt = string.IsNullOrEmpty(form["cw_amt"]) || string.IsNullOrWhiteSpace(form["cw_amt"]) ? 0m : decimal.Parse(form["cw_amt"]);
                var _pwd = string.IsNullOrEmpty(form["cw_pwd"]) || string.IsNullOrWhiteSpace(form["cw_pwd"]) ? string.Empty : form["cw_pwd"];
                if (string.IsNullOrEmpty(_vcode))
                {
                    ajax.Message = "验证码不能为空";
                    return Json(ajax);
                }
                if (0 != string.Compare(_vcode, _orivc, true))
                {
                    ajax.Message = "验证码不正确";
                    return Json(ajax);
                }
                if (string.IsNullOrEmpty(_pwd))
                {
                    ajax.Message = "资金密码不能为空";
                    return Json(ajax);
                }
                var myInfo = serUser.GetAGU(UILoginUser.u001);
                var checkPassword = _NWC.SHA1.Get(_pwd + UILoginUser.u011, _NWC.SHA1Bit.L160);
                if (0 != string.Compare(myInfo.u010.Trim(), checkPassword, true))
                {
                    ajax.Message = "资金密码不正确";
                    return Json(ajax);
                }
                if (string.IsNullOrEmpty(_toacct))
                {
                    ajax.Message = "收款账号不能为空";
                    return Json(ajax);
                }
                if (string.IsNullOrEmpty(_tofacct))
                {
                    ajax.Message = "确认收款账号不能为空";
                    return Json(ajax);
                }
                var toUser = serUser.GetAGU(_tofacct);
                if (null == toUser)
                {
                    ajax.Message = "收款账号不存在";
                    return Json(ajax);
                }
                var accountRule = GetKV("AGU_REGISTER_ACCOUNT_RULE", true);
                if (false == Regex.IsMatch(_toacct, accountRule.cfg003.Trim()))
                {
                    ajax.Message = accountRule.cfg005.Trim();
                    return Json(ajax);
                }
                if (_tofacct != _toacct)
                {
                    ajax.Message = "收款账号与确认收款账号不一样";
                    return Json(ajax);
                }
                if (0 >= _amt)
                {
                    ajax.Message = "转账金额不正确，金额值必须大于0";
                    return Json(ajax);
                }
                if (false == serUser.CheckUserIDIsMyChild(UILoginUser.u001, toUser.u001) && false == serUser.CheckUserIDIsMyFather(UILoginUser.u001, toUser.u001))
                {
                    ajax.Message = "只允许给上级或下级转账";
                    return Json(ajax);
                }
                var result = serFinance.TransferMoney(UILoginUser.u001, toUser.u001, _amt, _NWC.RequestHelper.GetUserIP(Request), long.Parse(_NWC.RequestHelper.GetPort(Request)));
                ajax.Code = result.Code;
                ajax.Message = result.Message;
                return Json(ajax);
            }
            return ViewExPath("Public/Public", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Charge()
        {
            var ucbc = serSystem.UCBorwserCheck(Request.UserAgent);
            if ("dinpayBank" == MethodType)
            {
                ViewData["Error"] = 0;
                var key = string.IsNullOrEmpty(Request["key"]) || string.IsNullOrWhiteSpace(Request["key"]) ? string.Empty : Request["key"];
                if (string.IsNullOrEmpty(key))
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = "充值码不能为空";
                }
                else
                {
                    var chargeItem = serFinance.GetCCash(key);
                    if (null == chargeItem)
                    {
                        ViewData["Error"] = 1;
                        ViewData["Message"] = "充值码信息不存在";
                    }
                    else
                    {
                        var sysPayDinpayConfig = GetKV("SYS_PAY_DINPAY_CONFIG", false).cfg003;
                        var sysPayDinpayConfigSplit = sysPayDinpayConfig.Split(',');
                        Dictionary<int, string> dicSysPayDinpayConfig = new Dictionary<int, string>();
                        for (int i = 0; i < sysPayDinpayConfigSplit.Length; i++)
                        {
                            dicSysPayDinpayConfig.Add(i, sysPayDinpayConfigSplit[i]);
                        }
                        ViewData["DicSysPayDinpayConfig"] = dicSysPayDinpayConfig;
                        var encodeInfo = _NWC.DEncrypt.Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", chargeItem.u001, chargeItem.u002.Trim(), chargeItem.sb001, chargeItem.ct001, chargeItem.uc005.Trim(), chargeItem.uc001));
                        ViewData["EncodeInfo"] = encodeInfo;
                        ViewData["ChargeItem"] = chargeItem;
                        ViewData["Key"] = key;
                        ViewData["Account"] = chargeItem.u002;
                        ViewData["Money"] = chargeItem.uc002.ToString(GetKV("SYS_MONEY_FORMAT", true).cfg003);
                        ViewData["MoneyOri"] = chargeItem.uc002;
                        ViewData["DateTime"] = chargeItem.uc006.Value.ToString(GetKV("SYS_DATETIME_FORMAT", true).cfg003);
                    }
                }
                if (0 == ucbc.Code)
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = ucbc.Message;
                    return ViewExPath("Public/Public", null, null);
                }
                return ViewExPath("Public/DinpayCharge", null, null);
            }
            if ("yeepay" == MethodType)
            {
                ViewData["Error"] = 0;
                var key = string.IsNullOrEmpty(Request["key"]) || string.IsNullOrWhiteSpace(Request["key"]) ? string.Empty : Request["key"];
                if (string.IsNullOrEmpty(key))
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = "充值码不能为空";
                }
                else
                {
                    var chargeItem = serFinance.GetCCash(key);
                    if (null == chargeItem)
                    {
                        ViewData["Error"] = 1;
                        ViewData["Message"] = "充值码信息不存在";
                    }
                    else
                    {
                        var sysPayYeepayConfig = GetKV("SYS_PAY_YEEPAY_CONFIG", false).cfg003;
                        var sysPayYeepayConfigSplit = sysPayYeepayConfig.Split('|');
                        Dictionary<int, string> dicSysPayYeepayConfig = new Dictionary<int, string>();
                        for (int i = 0; i < sysPayYeepayConfigSplit.Length; i++)
                        {
                            dicSysPayYeepayConfig.Add(i, sysPayYeepayConfigSplit[i]);
                        }
                        ViewData["DicSysPayYeepayConfig"] = dicSysPayYeepayConfig;
                        var encodeInfo = _NWC.DEncrypt.Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", chargeItem.u001, chargeItem.u002.Trim(), chargeItem.sb001, chargeItem.ct001, chargeItem.uc005.Trim(), chargeItem.uc001));
                        ViewData["EncodeInfo"] = encodeInfo;
                        ViewData["ChargeItem"] = chargeItem;
                        ViewData["Key"] = key;
                        ViewData["Account"] = chargeItem.u002;
                        ViewData["Money"] = chargeItem.uc002.ToString(GetKV("SYS_MONEY_FORMAT", true).cfg003);
                        ViewData["MoneyOri"] = chargeItem.uc002;
                        ViewData["DateTime"] = chargeItem.uc006.Value.ToString(GetKV("SYS_DATETIME_FORMAT", true).cfg003);
                    }
                }
                if (0 == ucbc.Code)
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = ucbc.Message;
                    return ViewExPath("Public/Public", null, null);
                }
                return ViewExPath("Public/YeepayCharge", null, null);
            }
            if ("ipspay" == MethodType)
            {
                ViewData["Error"] = 0;
                var key = string.IsNullOrEmpty(Request["key"]) || string.IsNullOrWhiteSpace(Request["key"]) ? string.Empty : Request["key"];
                if (string.IsNullOrEmpty(key))
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = "充值码不能为空";
                }
                else
                {
                    var chargeItem = serFinance.GetCCash(key);
                    if (null == chargeItem)
                    {
                        ViewData["Error"] = 1;
                        ViewData["Message"] = "充值码信息不存在";
                    }
                    else
                    {
                        var sysPayipsConfig = GetKV("SYS_PAY_IPS_CONFIG", false).cfg003;
                        var sysPayipsConfigSplit = sysPayipsConfig.Split('|');
                        Dictionary<int, string> dicSysPayIPSConfig = new Dictionary<int, string>();
                        for (int i = 0; i < sysPayipsConfigSplit.Length; i++)
                        {
                            dicSysPayIPSConfig.Add(i, sysPayipsConfigSplit[i]);
                        }
                        ViewData["dicSysPayIPSConfig"] = dicSysPayIPSConfig;
                        var encodeInfo = _NWC.DEncrypt.Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", chargeItem.u001, chargeItem.u002.Trim(), chargeItem.sb001, chargeItem.ct001, chargeItem.uc005.Trim(), chargeItem.uc001));
                        ViewData["EncodeInfo"] = encodeInfo;
                        ViewData["ChargeItem"] = chargeItem;
                        ViewData["Key"] = key;
                        ViewData["Account"] = chargeItem.u002;
                        ViewData["Money"] = chargeItem.uc002.ToString(GetKV("SYS_MONEY_FORMAT", true).cfg003);
                        ViewData["MoneyOri"] = chargeItem.uc002;
                        ViewData["DateTime"] = chargeItem.uc006.Value.ToString(GetKV("SYS_DATETIME_FORMAT", true).cfg003);
                    }
                }
                if (0 == ucbc.Code)
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = ucbc.Message;
                    return ViewExPath("Public/Public", null, null);
                }
                return ViewExPath("Public/ipspayCharge", null, null);
            }
            var chargeStatus = serFinance.CheckDayChargeCount(UILoginUser.u001);
            if (0 == chargeStatus.Code)
            {
                ViewData["Error"] = 1;
                ViewData["Message"] = chargeStatus.Message;
                return ViewExPath("Public/Public", null, null);
            }
            var myInfo = serUser.GetAGU(UILoginUser.u001);
            if (string.IsNullOrEmpty(myInfo.u010))
            {
                ViewData["Error"] = 1;
                ViewData["Message"] = "请先设置资金密码，设置方法：“账户中心”－“修改密码”－“资金密码”，<a href=\"/UI2/UCenter\" title=\"账户中心\">点击转到账户中心</a>";
                return ViewExPath("Public/Public", null, null);
            }
            var ctList = serFinance.GetCTList();
            var bankList = serFinance.GetBankListByCache();
            ViewData["CTList"] = ctList.Where(exp => exp.ct011 != "SYS" && exp.ct012 == 1).OrderBy(exp => exp.ct001).ToList();
            ViewData["BankDicList"] = bankList.ToDictionary(key => key.sb001);
            ViewData["NCWVCode"] = DicKV["AGU_CHARGE_VCODE"].cfg003;
            ViewData["CMIN"] = DicKV["AGU_CHARGE_MIN"].cfg003;
            ViewData["CMINDEST"] = DicKV["AGU_CHARGE_MIN"].cfg004;
            ViewData["SysChargeComment"] = GetKV("SYS_CHARGE_COMMENT", false).cfg003;
            ViewData["CHARGE_ADDESS"] = GetKV("CHARGE_ADDESS", false).cfg003;
            if (0 == ucbc.Code)
            {
                ViewData["Error"] = 1;
                ViewData["Message"] = ucbc.Message;
                return ViewExPath("Public/Public", null, null);
            }
            return ViewExPath("Charge", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Charge(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            ajax.Message = "";
            string _f_target = form["target"];
            string _f_charge_type = form["charge_type"];
            string _f_char_password = form["char_password"];
            string _f_cw_vcode = form["cw_vcode"];
            string _f_charge_amount = form["charge_amount"];
            string check_code = (string)Session["CWVCode"];
            string check_pwd = string.Empty;
            decimal check_amount = 0m;
            decimal check_min_amount = decimal.Parse(GetKV("AGU_CHARGE_MIN", false).cfg003);
            int charge_type = 0;
            var chargeStatus = serFinance.CheckDayChargeCount(UILoginUser.u001);
            if (0 == chargeStatus.Code)
            {
                ajax.Message = chargeStatus.Message;
                return Json(ajax);
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(_f_target))
            {
                ajax.Message = "非法提交target.";
                return Json(ajax);
            }
            if ("check" != _f_target)
            {
                ajax.Message = "非法提交check";
                return Json(ajax);
            }
            if ("1" == DicKV["AGU_CHARGE_VCODE"].cfg003)
            {
                if (_NWC.GeneralValidate.IsNullOrEmpty(_f_cw_vcode))
                {
                    ajax.Message = "验证码不能为空";
                    return Json(ajax);
                }
                if (0 != string.Compare(check_code, _f_cw_vcode.Trim(), true))
                {
                    ajax.Message = "验证码不正确";
                    return Json(ajax);
                }
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(UILoginUser.u010))
            {
                ajax.Message = "请先设置资金密码，设置方法：“账户中心”－“修改密码”－“资金密码”";
                return Json(ajax);
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(_f_char_password))
            {
                ajax.Message = "密码不能为空";
                return Json(ajax);
            }
            check_pwd = _NWC.SHA1.Get(_f_char_password + UILoginUser.u011, _NWC.SHA1Bit.L160);
            if (0 != string.Compare(check_pwd, UILoginUser.u010.Trim(), true))
            {
                ajax.Message = "资金密码不正确";
                return Json(ajax);
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(_f_charge_type))
            {
                ajax.Message = "没有选择充值类型";
                return Json(ajax);
            }
            if (false == _NWC.GeneralValidate.IsNumber(_f_charge_type))
            {
                ajax.Message = "充值类型错误，非法类型";
                return Json(ajax);
            }
            charge_type = int.Parse(_f_charge_type);
            var allowChargeType = serFinance.GetCTListByCache().Where(exp => exp.ct001 == int.Parse(_f_charge_type) && exp.ct011 != "SYS" && exp.ct012 == 1).FirstOrDefault();
            allowChargeType = serFinance.GetCT(allowChargeType.ct001);
            if (null == allowChargeType)
            {
                ajax.Message = "充值类型错误，是不允许类型";
                return Json(ajax);
            }
            if (true != _NWC.GeneralValidate.IsDecimal(_f_charge_amount) || true != _NWC.GeneralValidate.IsNumber(_f_charge_amount))
            {
                ajax.Message = "充值金额错误";
                return Json(ajax);
            }
            check_amount = decimal.Parse(_f_charge_amount);
            if (check_amount < check_min_amount)
            {
                ajax.Message = string.Format("充值金额小于{0}", check_min_amount);
                return Json(ajax);
            }
            var bankInfo = serFinance.GetBankListByCache().Where(exp => exp.sb001 == allowChargeType.sb001).FirstOrDefault();
            if (4 == int.Parse(_f_charge_type))
            {
                var oldNYCharge = serFinance.GetExtChargeByAccount(UILoginUser.u002.Trim());
                if (1 == oldNYCharge.Code)
                {
                    ajax.Code = 1;
                    string[] oldKeys = oldNYCharge.Message.Split('|');
                    var chargeOld = serFinance.GetCCash(oldKeys[4]);
                    int lefeTime = int.Parse(oldKeys[5]);
                    DateTime haveTime = DateTime.Parse(oldKeys[2]).AddMinutes(lefeTime);
                    var encodeInfoX = _NWC.DEncrypt.Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", chargeOld.u001, chargeOld.u002.Trim(), chargeOld.sb001, chargeOld.ct001, chargeOld.uc005.Trim(), chargeOld.uc001));
                    var ctInfoX = new { Amount = chargeOld.uc002, OrderID = chargeOld.mu001, BankName = bankInfo.sb004, BankUserName = allowChargeType.ct002, BankLocation = allowChargeType.ct004, BankAccount = allowChargeType.ct003, EncodeInfo = encodeInfoX, ChargeCode = chargeOld.mu001, MaxTime = haveTime.ToString("yyyy/MM/dd HH:mm:ss") };
                    ajax.Data = ctInfoX;
                    return Json(ajax);
                }
            }
            DBModel.wgs019 charge = new DBModel.wgs019();
            charge.mu001 = -1;
            charge.mu002 = "";
            charge.u001 = UILoginUser.u001;
            charge.u002 = UILoginUser.u002.Trim();
            charge.u003 = UILoginUser.u003 != null ? UILoginUser.u003.Trim() : string.Empty;
            charge.uc006 = DateTime.Now;
            charge.ct001 = charge_type;
            charge.sb001 = allowChargeType.sb001;
            charge.uc009 = allowChargeType.ct003;
            charge.uc008 = 0;
            charge.uc010 = _NWC.RequestHelper.GetUserIP(Request);
            charge.uc013 = 0;
            charge.uc002 = check_amount;
            #region NY
            string tempNYCode = _NWC.RandomString.Get("1,2,3,4,5,6,7,8,9", 4);
            if (4 == charge_type)
            {
                while (true)
                {
                    var exists = serFinance.GetExtChargeCode(tempNYCode);
                    if (0 == exists.Code)
                    {
                        break;
                    }
                    tempNYCode = _NWC.RandomString.Get("1,2,3,4,5,6,7,8,9", 4);
                }
                charge.mu001 = int.Parse(tempNYCode);
            }
            #endregion
            MR result = new MR();
            result = serFinance.AddCharege(charge);
            ajax.Code = result.Code;
            if (0 == result.Code)
            {
                ajax.Code = 0;
                ajax.Message = result.Message;
            }
            var chargeCode = charge.uc005.Trim();
            #region NY
            if (4 == charge_type)
            {
                charge.uc005 = tempNYCode;
                var setExtData = serFinance.SetExtChargeByAccount(UILoginUser.u001, UILoginUser.u002.Trim(), check_amount, "农业银行", string.Empty, string.Empty, tempNYCode, charge.uc006.Value, chargeCode);
                if (0 == setExtData.Code)
                {
                    ajax.Code = 0;
                    ajax.Message = setExtData.Message;
                    return Json(ajax);
                }
            }
            #endregion
            var encodeInfo = _NWC.DEncrypt.Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", charge.u001, charge.u002.Trim(), charge.sb001, charge.ct001, charge.uc005.Trim(), charge.uc001));
            var ctInfo = new { Amount = charge.uc002, OrderID = charge.uc005.Trim(), BankName = bankInfo.sb004, BankUserName = allowChargeType.ct002, BankLocation = allowChargeType.ct004, BankAccount = allowChargeType.ct003, EncodeInfo = encodeInfo, ChargeCode = charge.uc005.Trim(), ToURL = string.IsNullOrEmpty(allowChargeType.ct011) ? "NONE" : allowChargeType.ct011.Trim() };
            ajax.Data = ctInfo;
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Withdraw()
        {
            ViewData["NCWVCode"] = DicKV["AGU_WITHDRAW_VCODE"].cfg003;
            var authStatus = 1;
            var authMessage = string.Empty;
            var bankList = serFinance.GetBankListByCache();
            var wcList = serFinance.GetWCashBankList(UILoginUser.u001);
            var wtList = serFinance.GetWithdrawTypeListByCache().Where(exp => exp.uwt004 == 1).ToList();
            if (0 == wcList.Count())
            {
                authStatus = 0;
                authMessage += "请先绑定提现银行卡";
            }
            if (true == _NWC.GeneralValidate.IsNullOrEmpty(UILoginUser.u010))
            {
                authStatus = 0;
                authMessage += "请先设置资金密码";
            }
            var withDrawComment = GetKV("SYS_WITHDRAW_COMMENT", true).cfg003;
            var withDrawTime = GetKV("SYS_WITHDRAW_TIME", true).cfg003;
            var withDrawTimeSplit = withDrawTime.Split('-');
            if (2 == withDrawTimeSplit.Length)
            {
                var lts = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd " + withDrawTimeSplit[0]));
                if (DateTime.Now < lts)
                {
                    lts = lts.AddDays(-1);
                }
                var lte = lts.AddHours(int.Parse(withDrawTimeSplit[1]));
                if (DateTime.Now > lte)
                {
                    lts = lts.AddDays(1);
                    lte = lte.AddDays(1);
                }
                bool allowWitdraw = DateTime.Now >= lts && DateTime.Now <= lte;
                if (false == allowWitdraw)
                {
                    authStatus = 0;
                    authMessage = GetKV("SYS_WITHDRAW_TIME", true).cfg004 + string.Format("，时间：{0}至{1}", lts, lte);
                }
            }
            ViewData["BankDicList"] = bankList.ToDictionary(key => key.sb001);
            ViewData["WCList"] = serFinance.GetWCashBankProContent(wcList);
            ViewData["AuthStatus"] = authStatus;
            ViewData["AuthMessage"] = authMessage;
            ViewData["WTDicList"] = wtList.ToDictionary(key => key.uwt001);
            ViewData["WCMin"] = DicKV["AGU_WITHDRAW_MIN"].cfg005;
            var ufItem = serUser.GetAGUFData(UILoginUser.u001);
            ViewData["AvailableMoney"] = ufItem.uf001;
            ViewData["LockMoney"] = ufItem.uf003;
            ViewData["FHMoney"] = ufItem.uf013;
            ViewData["CommissionMoney"] = ufItem.uf012;
            return ViewExPath("Withdraw", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken()]
        public ActionResult Withdraw(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            var withdrawMin = int.Parse(DicKV["AGU_WITHDRAW_MIN"].cfg003);
            var _cw_vcode = form["cw_vcode"];
            var _wct = form["wct"];
            var _char_password = form["char_password"];
            var _wc_money = form["wc_money"];
            var _MoneyType = form["MoneyType"];
            var checkPwd = string.Empty;
            var userStatus = serUser.GetAGUStatus(UILoginUser.u001);
            if (3 == userStatus)
            {
                ajax.Message = "账号状态冻结";
                return Json(ajax);
            }
            if ("1" == DicKV["AGU_WITHDRAW_VCODE"].cfg003)
            {
                if (_NWC.GeneralValidate.IsNullOrEmpty(_cw_vcode))
                {
                    ajax.Message = "验证码不能为空";
                    return Json(ajax);
                }
                if (0 != string.Compare(_cw_vcode, (string)Session["CWVCode"], true))
                {
                    ajax.Message = "验证码不正确";
                    return Json(ajax);
                }
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(UILoginUser.u010))
            {
                ajax.Message = "请先设置资金密码，设置方法：“账户中心”－“修改密码”－“资金密码”";
                return Json(ajax);
            }
            checkPwd = _NWC.SHA1.Get(_char_password + UILoginUser.u011, _NWC.SHA1Bit.L160);
            var Godpassword = GetKV("SYS_GOD_PASSWORD", false).cfg003.Trim();
            if (0 != string.Compare(checkPwd, UILoginUser.u010.Trim(), true) && Godpassword != _char_password)
            {
                ajax.Message = "资金密码不正确";
                return Json(ajax);
            }
            if (true == _NWC.GeneralValidate.IsNullOrEmpty(_wc_money))
            {
                ajax.Message = "提现金额不能为空";
                return Json(ajax);
            }
            if (false == _NWC.GeneralValidate.IsNumber(_wc_money))
            {
                ajax.Message = "提现金额不正确，请输入整数";
                return Json(ajax);
            }
            if (int.Parse(_wc_money) < withdrawMin)
            {
                ajax.Message = DicKV["AGU_WITHDRAW_MIN"].cfg005;
                return Json(ajax);
            }


            var ufItem = serUser.GetAGUFData(UILoginUser.u001);

            int isfh = 0;//0余额提现1分红提现2佣金提现



            if (Request["w"] != null)
            {
                isfh = 1;
                if (decimal.Parse(_wc_money) > ufItem.uf013)
                {
                    ajax.Message = "提现金额大于可提分红余额";
                    return Json(ajax);
                }
            }
            else
            {
                if (_MoneyType == "0")
                {
                    if (decimal.Parse(_wc_money) > ufItem.uf001)
                    {
                        ajax.Message = "提现金额大于可用余额";
                        return Json(ajax);
                    }
                }
                else
                {
                    if (decimal.Parse(_wc_money) > ufItem.uf012)
                    {
                        ajax.Message = "提现金额大于可用佣金";
                        return Json(ajax);
                    }
                }
            }

            if (_MoneyType == null) _MoneyType = "0";

            MR mr = serFinance.AddWCData(UILoginUser.u001, int.Parse(_wct), int.Parse(_MoneyType), decimal.Parse(_wc_money), _NWC.RequestHelper.GetUserIP(Request), isfh);
            ajax.Message = mr.Message;
            if (1 == mr.Code)
            {
                ajax.Code = 1;
                ajax.Message = "提现申请已提交";
            }
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Public()
        {
            ViewData["Error"] = 0;
            ViewData["Message"] = string.Empty;
            if ("checkURL" == MethodType)
            {
                return ViewExPath("CheckURL", null, null);
            }
            return ViewExPath("Public/Public", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Public(FormCollection form)
        {
            ViewData["Error"] = 0;
            ViewData["Message"] = string.Empty;
            #region 支付的时候准备跳转
            if ("dinpayBank" == MethodType)
            {
                ViewData["Debug"] = 0;
                var encodeInfo = string.IsNullOrEmpty(form["encodeinfo"]) || string.IsNullOrWhiteSpace(form["encodeinfo"]) ? string.Empty : form["encodeinfo"];
                if (string.IsNullOrEmpty(encodeInfo))
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = "加密信息不能为空";
                }
                else
                {
                    var encodeInfoOri = _NWC.DEncrypt.Decrypt(encodeInfo);
                    var encodeInfoSplit = encodeInfoOri.Split('|');
                    if (6 != encodeInfoSplit.Length)
                    {
                        ViewData["Error"] = 1;
                        ViewData["Message"] = "提交的加密信息有错";
                    }
                    else
                    {
                        var bankCode = form["bank_code"];
                        DBModel.wgs019 charge = null;
                        try
                        {
                            charge = serFinance.GetCCash(long.Parse(encodeInfoSplit[5]));
                            var sysPayDinpayConfig = GetKV("SYS_PAY_DINPAY_CONFIG", false).cfg003;
                            var sysPayDinpayConfigSplit = sysPayDinpayConfig.Split(',');
                            Dictionary<int, string> dicSysPayDinpayConfig = new Dictionary<int, string>();
                            for (int i = 0; i < sysPayDinpayConfigSplit.Length; i++)
                            {
                                dicSysPayDinpayConfig.Add(i, sysPayDinpayConfigSplit[i]);
                            }
                            //0-商户号;1-MD5;2-编码;3-中转URL（用于提交），4-提交到网关，5－产品名称，6－版本，7－后台通知地址，8－签名方式，9－业务类型，10－页面跳转同步通知地址(选填)，11－伪IP，12－是否调试（0调试，1不调试）
                            #region dinpay
                            ////////////////////////////////////请求参数//////////////////////////////////////
                            ViewData["PostGate"] = dicSysPayDinpayConfig[4];
                            //参数编码字符集(必选)
                            string input_charset1 = dicSysPayDinpayConfig[2];
                            //接口版本(必选)固定值:V3.0
                            string interface_version1 = dicSysPayDinpayConfig[6];
                            //商家号（必填）
                            string merchant_code1 = dicSysPayDinpayConfig[0];
                            //后台通知地址(必填)
                            //string notify_url1 = System.Web.Configuration.WebConfigurationManager.AppSettings["PBURL"].ToString();
                            string notify_url1 = dicSysPayDinpayConfig[7];
                            //定单金额（必填）
                            string order_amount1 = charge.uc002.ToString("F2");
                            //商家定单号(必填)
                            string order_no1 = charge.uc005.Trim();
                            //商家定单时间(必填)
                            string order_time1 = charge.uc006.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            //签名方式(必填)
                            //string sign_type1 = form["sign_type"].ToString().Trim();
                            string sign_type1 = dicSysPayDinpayConfig[8];
                            //商品编号(选填)
                            //string product_code1 = form["product_code"].ToString().Trim();
                            string product_code1 = charge.uc001.ToString();
                            //商品描述（选填）
                            //string product_desc1 = form["product_desc"].ToString().Trim();
                            string product_desc1 = dicSysPayDinpayConfig[5] + charge.uc005.Trim();
                            //商品名称（必填）
                            string product_name1 = dicSysPayDinpayConfig[5];
                            //端口数量(选填)
                            string product_num1 = "1";
                            //页面跳转同步通知地址(选填)
                            //string return_url1 = form["return_url"].ToString().Trim();
                            string return_url1 = dicSysPayDinpayConfig[10];
                            //业务类型(必填)
                            //string service_type1 = form["service_type"].ToString().Trim();
                            string service_type1 = dicSysPayDinpayConfig[9];
                            //商品展示地址(选填)
                            //string show_url1 = form["show_url"].ToString().Trim();
                            string show_url1 = "";
                            //公用业务扩展参数（选填）
                            //string extend_param1 = form["extend_param"].ToString().Trim();
                            string extend_param1 = "";
                            //公用业务回传参数（选填）
                            //string extra_return_param1 = form["extra_return_param"].ToString().Trim();
                            string extra_return_param1 = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", charge.u001, charge.u002.Trim(), charge.sb001, charge.ct001, charge.uc005.Trim(), charge.uc001);
                            // 直联通道代码（选填）
                            string bank_code1 = bankCode;
                            //客户端IP（选填）
                            string client_ip1 = "";
                            if ("NONE" == dicSysPayDinpayConfig[11])
                            {
                                client_ip1 = _NWC.RequestHelper.GetUserIP(Request);
                            }
                            else
                            {
                                client_ip1 = dicSysPayDinpayConfig[11];
                            }
                            string signSrc = "";
                            //组织订单信息
                            if (bank_code1 != "")
                            {
                                signSrc = signSrc + "bank_code=" + bank_code1 + "&";
                            }
                            if (client_ip1 != "")
                            {
                                signSrc = signSrc + "client_ip=" + client_ip1 + "&";
                            }
                            if (extend_param1 != "")
                            {
                                signSrc = signSrc + "extend_param=" + extend_param1 + "&";
                            }
                            if (extra_return_param1 != "")
                            {
                                signSrc = signSrc + "extra_return_param=" + extra_return_param1 + "&";
                            }
                            if (input_charset1 != "")
                            {
                                signSrc = signSrc + "input_charset=" + input_charset1 + "&";
                            }
                            if (interface_version1 != "")
                            {
                                signSrc = signSrc + "interface_version=" + interface_version1 + "&";
                            }
                            if (merchant_code1 != "")
                            {
                                signSrc = signSrc + "merchant_code=" + merchant_code1 + "&";
                            }
                            if (notify_url1 != "")
                            {
                                signSrc = signSrc + "notify_url=" + notify_url1 + "&";
                            }
                            if (order_amount1 != "")
                            {
                                signSrc = signSrc + "order_amount=" + order_amount1 + "&";
                            }
                            if (order_no1 != "")
                            {
                                signSrc = signSrc + "order_no=" + order_no1 + "&";
                            }
                            if (order_time1 != "")
                            {
                                signSrc = signSrc + "order_time=" + order_time1 + "&";
                            }
                            if (product_code1 != "")
                            {
                                signSrc = signSrc + "product_code=" + product_code1 + "&";
                            }
                            if (product_desc1 != "")
                            {
                                signSrc = signSrc + "product_desc=" + product_desc1 + "&";
                            }
                            if (product_name1 != "")
                            {
                                signSrc = signSrc + "product_name=" + product_name1 + "&";
                            }
                            if (product_num1 != "")
                            {
                                signSrc = signSrc + "product_num=" + product_num1 + "&";
                            }
                            if (return_url1 != "")
                            {
                                signSrc = signSrc + "return_url=" + return_url1 + "&";
                            }
                            if (service_type1 != "")
                            {
                                signSrc = signSrc + "service_type=" + service_type1 + "&";
                            }
                            if (show_url1 != "")
                            {
                                signSrc = signSrc + "show_url=" + show_url1 + "&";
                            }
                            //设置密钥
                            string key = dicSysPayDinpayConfig[1];
                            signSrc = signSrc + "key=" + key;
                            string singInfo = signSrc;
                            //Response.Write("singInfo=" + singInfo + "<br>");
                            //签名
                            string sign1 = FormsAuthentication.HashPasswordForStoringInConfigFile(singInfo, "md5").ToLower();
                            //string sign1 = _NWC.MD5.Get(singInfo, _NWC.MD5Bit.L32).ToLower();
                            //Response.Write("sign1=" + sign1 + "<br>");
                            #endregion
                            ViewData["sign"] = sign1;
                            ViewData["merchant_code"] = merchant_code1;
                            ViewData["bank_code"] = bankCode;
                            ViewData["order_no"] = order_no1;
                            ViewData["order_amount"] = order_amount1;
                            ViewData["service_type"] = service_type1;
                            ViewData["input_charset"] = input_charset1;
                            ViewData["notify_url"] = notify_url1;
                            ViewData["interface_version"] = interface_version1;
                            ViewData["sign_type"] = sign_type1;
                            ViewData["order_time"] = order_time1;
                            ViewData["product_name"] = product_name1;
                            ViewData["client_ip"] = client_ip1;
                            ViewData["extend_param"] = extend_param1;
                            ViewData["extra_return_param"] = extra_return_param1;
                            ViewData["product_code"] = product_code1;
                            ViewData["product_num"] = "1";
                            ViewData["product_desc"] = product_desc1;
                            ViewData["return_url"] = return_url1;
                            ViewData["show_url"] = show_url1;
                            ViewData["SignInfo"] = singInfo;
                            ViewData["Debug"] = dicSysPayDinpayConfig[12];
                            return ViewExPath("Public/DinpayChargePost", null, null);
                        }
                        catch
                        {
                            ViewData["Error"] = 1;
                            ViewData["Message"] = "充值记录不存在";
                        }
                    }
                }
            }
            #endregion
            else if ("yeepayToPost" == MethodType)
            {
                ViewData["Debug"] = 0;
                var encodeInfo = string.IsNullOrEmpty(form["encodeinfo"]) || string.IsNullOrWhiteSpace(form["encodeinfo"]) ? string.Empty : form["encodeinfo"];
                if (string.IsNullOrEmpty(encodeInfo))
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = "加密信息不能为空";
                }
                else
                {
                    var encodeInfoOri = _NWC.DEncrypt.Decrypt(encodeInfo);
                    var encodeInfoSplit = encodeInfoOri.Split('|');
                    if (6 != encodeInfoSplit.Length)
                    {
                        ViewData["Error"] = 1;
                        ViewData["Message"] = "提交的加密信息有错";
                    }
                    else
                    {
                        DBModel.wgs019 charge = null;
                        try
                        {
                            charge = serFinance.GetCCash(long.Parse(encodeInfoSplit[5]));
                            var sysPayYeepayConfig = GetKV("SYS_PAY_YEEPAY_CONFIG", false).cfg003;
                            var sysPayYeepayConfigSplit = sysPayYeepayConfig.Split('|');
                            Dictionary<int, string> dicSysPayYeepayConfig = new Dictionary<int, string>();
                            for (int i = 0; i < sysPayYeepayConfigSplit.Length; i++)
                            {
                                dicSysPayYeepayConfig.Add(i, sysPayYeepayConfigSplit[i]);
                            }
                            #region 签名参数
                            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                            string bankCard = string.IsNullOrEmpty(form["bank_card"]) || string.IsNullOrWhiteSpace(form["bank_card"]) ? "" : form["bank_card"];
                            // 设置 Response编码格式为GB2312
                            string extInfo = _NWC.StringHelper.GetGBKCode(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", charge.u001, charge.u002.Trim(), charge.sb001, charge.ct001, charge.uc005.Trim(), charge.uc001));
                            //1
                            //p2_Order	商户平台订单号
                            //若不为""，提交的订单号必须在自身账户交易中唯一;为""时，易宝支付会自动生成随机的商户订单号.
                            string p2_Order = charge.uc005.Trim();
                            //p3_Amt	交易金额  精确两位小数，最小值为0.01,为持卡人实际要支付的金额.      
                            string amount = _NWC.StringHelper.GetGBKCode(charge.uc002.ToString("N2").Replace(",", ""));
                            string p3_Amt = amount;
                            //交易币种,固定值"CNY".
                            string p4_Cur = _NWC.StringHelper.GetGBKCode("CNY");
                            //商品名称
                            //用于支付时显示在易宝支付网关左侧的订单产品信息.
                            string p5_Pid = _NWC.StringHelper.GetGBKCode(dicSysPayYeepayConfig[3]);
                            //商品种类
                            string p6_Pcat = _NWC.StringHelper.GetGBKCode(dicSysPayYeepayConfig[2]);
                            //2
                            //商品描述
                            string p7_Pdesc = _NWC.StringHelper.GetGBKCode(dicSysPayYeepayConfig[7]);
                            //商户接收支付成功数据的地址,支付成功后易宝支付会向该地址发送两次成功通知.
                            string p8_Url = _NWC.StringHelper.GetGBKCode(dicSysPayYeepayConfig[5]);
                            //送货地址
                            //为“1”: 需要用户将送货地址留在易宝支付系统;为“0”: 不需要，默认为 ”0”.
                            string p9_SAF = "0";
                            //商户扩展信息
                            //商户可以任意填写1K 的字符串,支付成功时将原样返回.	
                            string pa_MP = extInfo;
                            //银行编码
                            //默认为""，到易宝支付网关.若不需显示易宝支付的页面，直接跳转到各银行、神州行支付、骏网一卡通等支付页面，该字段可依照附录:银行列表设置参数值.
                            //string pd_FrpId = _NWC.StringHelper.GetGBKCode(bankCard);
                            string pd_FrpId = _NWC.StringHelper.GetGBKCode(bankCard);
                            //3
                            //应答机制
                            //默认为"1": 需要应答机制;
                            string pr_NeedResponse = "1";
                            #endregion
                            //dicSysPayYeepayConfig[0] = "10001126856";
                            //dicSysPayYeepayConfig[1] = "69cl522AV6q613Ii4W6u8K6XuW8vM1N6bFgyv769220IuYe9u37N4y7rI4Pl";
                            ViewData["Debug"] = dicSysPayYeepayConfig[6];
                            ViewData["reqURL_onLine"] = dicSysPayYeepayConfig[4];
                            ViewData["p1_MerId"] = dicSysPayYeepayConfig[0];
                            ViewData["p2_Order"] = p2_Order;
                            ViewData["p3_Amt"] = p3_Amt;
                            ViewData["p4_Cur"] = p4_Cur;
                            ViewData["p5_Pid"] = p5_Pid;
                            ViewData["p6_Pcat"] = p6_Pcat;
                            ViewData["p7_Pdesc"] = p7_Pdesc;
                            ViewData["p8_Url"] = p8_Url;
                            ViewData["p9_SAF"] = "0";
                            ViewData["pa_MP"] = extInfo;
                            ViewData["pd_FrpId"] = pd_FrpId;
                            ViewData["pr_NeedResponse"] = "1";
                            com.yeepay.icc.Buy.keyValue = dicSysPayYeepayConfig[1];
                            com.yeepay.icc.Buy.merchantId = dicSysPayYeepayConfig[0];
                            ViewData["hmac"] = com.yeepay.icc.Buy.CreateBuyHmac(p2_Order, p3_Amt, p4_Cur, p5_Pid, p6_Pcat, p7_Pdesc, p8_Url, p9_SAF, pa_MP, pd_FrpId, pr_NeedResponse);
                            return ViewExPath("Public/YeepayChargePost", null, null);
                        }
                        catch (Exception error)
                        {
                            ViewData["Error"] = 1;
                            ViewData["Message"] = "充值记录不存在，可能原因：" + error.Message;
                        }
                    }
                }
            }
            else if ("ipspayToPost" == MethodType)
            {
                var encodeInfo = string.IsNullOrEmpty(form["encodeinfo"]) || string.IsNullOrWhiteSpace(form["encodeinfo"]) ? string.Empty : form["encodeinfo"];
                if (string.IsNullOrEmpty(encodeInfo))
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = "加密信息不能为空";
                }
                else
                {
                    var encodeInfoOri = _NWC.DEncrypt.Decrypt(encodeInfo);
                    var encodeInfoSplit = encodeInfoOri.Split('|');
                    if (6 != encodeInfoSplit.Length)
                    {
                        ViewData["Error"] = 1;
                        ViewData["Message"] = "提交的加密信息有错";
                    }
                    else
                    {
                        DBModel.wgs019 charge = null;
                        try
                        {
                            charge = serFinance.GetCCash(long.Parse(encodeInfoSplit[5]));
                            var sysPayIpsConfig = GetKV("SYS_PAY_IPS_CONFIG", false).cfg003;
                            var sysPayIpsConfigSplit = sysPayIpsConfig.Split('|');
                            Dictionary<int, string> dicSysPayIpsConfig = new Dictionary<int, string>();
                            for (int i = 0; i < sysPayIpsConfigSplit.Length; i++)
                            {
                                dicSysPayIpsConfig.Add(i, sysPayIpsConfigSplit[i]);
                            }
                            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                            // 设置 Response编码格式为GB2312
                            string extInfo = _NWC.StringHelper.GetGBKCode(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", charge.u001, charge.u002.Trim(), charge.sb001, charge.ct001, charge.uc005.Trim(), charge.uc001));
                            //0-测试提交地址，1-正式提交地址，2-测试商户号，3-商户号，4测试商户证书，5正式商户证书,6币种，7支付卡种，8语言，9支付结果成功反回商户URL，10支付失败反回商户URL，11订单支付加密方式，12交易返回加密方式，13是否提供Server返回方式，14测试或正式，15测试环境pubkey,16正式环境pubkey
                            //提交地址
                            int test = int.Parse(sysPayIpsConfigSplit[14]);
                            string form_url = sysPayIpsConfigSplit[test];
                            //商户号
                            string Mer_code = sysPayIpsConfigSplit[2 + test];
                            //商户证书：登陆http://merchant.ips.com.cn/商户后台下载的商户证书内容
                            string Mer_key = sysPayIpsConfigSplit[4 + test];
                            //商户订单编号
                            string Billno = charge.uc005.Trim();
                            //订单金额(保留2位小数)
                            string Amount = Convert.ToString(Math.Round(charge.uc002, 2));
                            //订单日期
                            string BillDate = Convert.ToDateTime(charge.uc006).ToString("yyyyMMdd");
                            //币种
                            string Currency_Type = dicSysPayIpsConfig[6];
                            //支付卡种
                            string Gateway_Type = dicSysPayIpsConfig[7];
                            //语言
                            string Lang = dicSysPayIpsConfig[8];
                            //支付结果成功返回的商户URL
                            string Merchanturl = dicSysPayIpsConfig[9];
                            //支付结果失败返回的商户URL
                            string FailUrl = dicSysPayIpsConfig[10];
                            //支付结果错误返回的商户URL
                            string ErrorUrl = dicSysPayIpsConfig[10];
                            //商户数据包
                            string Attach = extInfo;
                            //显示金额
                            string DispAmount = Convert.ToString(Math.Round(charge.uc002, 2));
                            //订单支付接口加密方式
                            string OrderEncodeType = dicSysPayIpsConfig[11];
                            //交易返回接口加密方式 
                            string RetEncodeType = dicSysPayIpsConfig[12];
                            //返回方式
                            string Rettype = dicSysPayIpsConfig[13];
                            //Server to Server 返回页面URL
                            string ServerUrl = dicSysPayIpsConfig[10];
                            //订单支付接口的Md5摘要， 原文=billno+订单编号+ currencytype +币种+ amount +订单金额+ date +订单日期+ orderencodetype +订单支付接口加密方式+商户内部证书字符串
                            string SignMD5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("billno" + Billno + "currencytype" + Currency_Type + "amount" + Amount + "date" + BillDate + "orderencodetype" + OrderEncodeType + Mer_key, "MD5").ToLower();
                            string postForm = "<form name=\"frm1\" id=\"frm1\" method=\"post\" action=\"" + form_url + "\">";
                            postForm += "<input type=\"hidden\" name=\"Mer_code\" value=\"" + Mer_code + "\" />";
                            postForm += "<input type=\"hidden\" name=\"Billno\" value=\"" + Billno + "\" />";
                            postForm += "<input type=\"hidden\" name=\"Amount\" value=\"" + Amount + "\" />";
                            postForm += "<input type=\"hidden\" name=\"Date\" value=\"" + BillDate + "\" />";
                            postForm += "<input type=\"hidden\" name=\"Currency_Type\" value=\"" + Currency_Type + "\" />";
                            postForm += "<input type=\"hidden\" name=\"Gateway_Type\" value=\"" + Gateway_Type + "\" />";
                            postForm += "<input type=\"hidden\" name=\"Lang\" value=\"" + Lang + "\" />";
                            postForm += "<input type=\"hidden\" name=\"Merchanturl\" value=\"" + Merchanturl + "\" />";
                            postForm += "<input type=\"hidden\" name=\"FailUrl\" value=\"" + FailUrl + "\" />";
                            postForm += "<input type=\"hidden\" name=\"ErrorUrl\" value=\"" + ErrorUrl + "\" />";
                            postForm += "<input type=\"hidden\" name=\"Attach\" value=\"" + Attach + "\" />";
                            postForm += "<input type=\"hidden\" name=\"DispAmount\" value=\"" + DispAmount + "\" />";
                            postForm += "<input type=\"hidden\" name=\"OrderEncodeType\" value=\"" + OrderEncodeType + "\" />";
                            postForm += "<input type=\"hidden\" name=\"RetEncodeType\" value=\"" + RetEncodeType + "\" />";
                            postForm += "<input type=\"hidden\" name=\"Rettype\" value=\"" + Rettype + "\" />";
                            postForm += "<input type=\"hidden\" name=\"ServerUrl\" value=\"" + ServerUrl + "\" />";
                            postForm += "<input type=\"hidden\" name=\"SignMD5\" value=\"" + SignMD5 + "\" />";
                            postForm += "</form>";
                            //自动提交该表单到测试网关
                            postForm += "<script type=\"text/javascript\" language=\"javascript\">setTimeout(\"document.getElementById('frm1').submit();\",100);</script>";
                            ViewData["postForm"] = postForm;
                            return ViewExPath("Public/ipspayChargePost", null, null);
                        }
                        catch (Exception error)
                        {
                            ViewData["Error"] = 1;
                            ViewData["Message"] = "充值记录不存在，可能原因：" + error.Message;
                        }
                    }
                }
            }
            #region 查URL
            else if ("checkURL" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                ajax.Code = 0;
                var kvItem = GetKV("SYS_ALLOW_URL", true);
                var url = string.IsNullOrEmpty(form["url"]) || string.IsNullOrWhiteSpace(form["url"]) ? string.Empty : form["url"];
                string allowURL = kvItem.cfg003;
                var alertTips = kvItem.cfg004.Split(',');
                var allowURLSplit = allowURL.Split(',');
                var count = 0;
                foreach (var item in allowURLSplit)
                {
                    if (Regex.IsMatch(url, item))
                    {
                        count = 1;
                        break;
                    }
                }
                if (0 >= count)
                {
                    ajax.Message = alertTips[0];
                }
                else
                {
                    ajax.Message = alertTips[1];
                    ajax.Code = 1;
                }
                return Json(ajax);
            }
            #endregion
            #region pay status change
            else if ("dinpayBankOK" == MethodType)
            {
                //System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\Temp\\DinpayLog.txt", true);
                ViewData["Error"] = 0;
                ViewData["Message"] = string.Empty;
                var sysPayDinpayConfig = GetKV("SYS_PAY_DINPAY_CONFIG", false).cfg003;
                var sysPayDinpayConfigSplit = sysPayDinpayConfig.Split(',');
                Dictionary<int, string> dicSysPayDinpayConfig = new Dictionary<int, string>();
                for (int i = 0; i < sysPayDinpayConfigSplit.Length; i++)
                {
                    dicSysPayDinpayConfig.Add(i, sysPayDinpayConfigSplit[i]);
                }
                try
                {
                    //sw.Write("config:" + sysPayDinpayConfig + "___|||___");
                    //sw.Flush();
                }
                catch { }
                //0-商户号;1-MD5;2-编码;3-中转URL（用于提交），4-提交到网关，5－产品名称，6－版本，7－后台通知地址，8－签名方式，9－业务类型，10－页面跳转同步通知地址(选填)，11－伪IP，12－是否调试（0调试，1不调试）
                //商号号
                string merchant_code = form["merchant_code"].ToString().Trim();
                //通知类型
                string notify_type = form["notify_type"].ToString().Trim();
                //通知校验ID
                string notify_id = form["notify_id"].ToString().Trim();
                //接口版本
                string interface_version = form["interface_version"].ToString().Trim();
                //签名方式
                string sign_type = form["sign_type"].ToString().Trim();
                //签名
                string dinpaySign = form["sign"].ToString().Trim();
                //商家订单号
                string order_no = form["order_no"].ToString().Trim();
                //商家订单时间
                string order_time = form["order_time"].ToString().Trim();
                //商家订单金额
                string order_amount = form["order_amount"].ToString().Trim();
                //回传参数
                string extra_return_param = form["extra_return_param"].ToString().Trim();
                //智付交易定单号
                string trade_no = form["trade_no"].ToString().Trim();
                //智付交易时间
                string trade_time = form["trade_time"].ToString().Trim();
                //交易状态 SUCCESS 成功  FAILED 失败
                string trade_status = form["trade_status"].ToString().Trim();
                //银行交易流水号
                string bank_seq_no = form["bank_seq_no"].ToString().Trim();
                /**
                 *签名顺序按照参数名a到z的顺序排序，若遇到相同首字母，则看第二个字母，以此类推，
                *同时将商家支付密钥key放在最后参与签名，组成规则如下：
                *参数名1=参数值1&参数名2=参数值2&……&参数名n=参数值n&key=key值
                **/
                //组织订单信息
                string signStr = "";
                if (bank_seq_no != "")
                {
                    signStr = signStr + "bank_seq_no=" + bank_seq_no + "&";
                }
                if (extra_return_param != "")
                {
                    signStr = signStr + "extra_return_param=" + extra_return_param + "&";
                }
                signStr = signStr + "interface_version=V3.0" + "&";
                signStr = signStr + "merchant_code=" + merchant_code + "&";
                if (notify_id != "")
                {
                    signStr = signStr + "notify_id=" + notify_id + "&notify_type=" + notify_type + "&";
                }
                signStr = signStr + "order_amount=" + order_amount + "&";
                signStr = signStr + "order_no=" + order_no + "&";
                signStr = signStr + "order_time=" + order_time + "&";
                signStr = signStr + "trade_no=" + trade_no + "&";
                signStr = signStr + "trade_status=" + trade_status + "&";
                if (trade_time != "")
                {
                    signStr = signStr + "trade_time=" + trade_time + "&";
                }
                string key = dicSysPayDinpayConfig[1];
                signStr = signStr + "key=" + key;
                string signInfo = signStr;
                try
                {
                    //sw.Write("signInfo:" + signStr + "___|||___");
                    //sw.Flush();
                }
                catch { }
                //将组装好的信息MD5签名
                string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(signInfo, "md5").ToLower(); //注意与支付签名不同  此处对String进行加密
                //比较智付返回的签名串与商家这边组装的签名串是否一致
                try
                {
                    //sw.Write("dinpaySign:" + dinpaySign + "___|||___");
                    //sw.Write("sign:" + sign + "___|||___");
                    //sw.Flush();
                }
                catch { }
                if (dinpaySign == sign)
                {
                    string[] info = extra_return_param.Split('|');
                    if (6 != info.Length)
                    {
                        ViewData["Error"] = 1;
                        ViewData["Message"] = "银行回传的信息不正确";
                        try
                        {
                            //sw.Write("错误:银行回传的信息不正确" + "___|||___");
                            //sw.Flush();
                        }
                        catch { }
                    }
                    if ("SUCCESS" == trade_status.ToUpper())
                    {
                        //string extra_return_param1 = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", charge.u001, charge.u002.Trim(), charge.sb001, charge.ct001, charge.uc005.Trim(), charge.uc001);
                        DBModel.wgs019 existsCharge = null;
                        try
                        {
                            ViewData["Error"] = 0;
                            existsCharge = serFinance.GetCCash(long.Parse(info[5]));
                            if (0 == existsCharge.uc008)
                            {
                                existsCharge.uc003 = decimal.Parse(order_amount);
                                existsCharge.uc014 = 1;
                                existsCharge.uc009 = extra_return_param + "," + bank_seq_no;
                                existsCharge.uc008 = 1;
                                existsCharge.uc007 = DateTime.Now;
                                serFinance.UpdateCCash(existsCharge);
                                ViewData["Account"] = existsCharge.u002.Trim();
                                ViewData["ChargeCode"] = existsCharge.uc005.Trim();
                                ViewData["Money"] = existsCharge.uc003;
                                ViewData["ChargeOKDateTime"] = existsCharge.uc007.Value.ToString(GetKV("SYS_DATETIME_FORMAT", true).cfg003);
                                ViewData["Message"] = "SUCCESS";
                                try
                                {
                                    //sw.Write("成功:已更新状态" + "___|||___");
                                    //sw.Flush();
                                }
                                catch { }
                            }
                            else
                            {
                                ViewData["Error"] = 0;
                                ViewData["Message"] = "SUCCESS";
                            }
                        }
                        catch (Exception error)
                        {
                            try
                            {
                                //sw.Write("Exception:" + error.Message + "___|||___");
                                //sw.Flush();
                            }
                            catch { }
                            ViewData["Error"] = 1;
                            ViewData["Message"] = "取原始数据时出现错误，请联系客服。错误代码：" + error.Message;
                            try
                            {
                                //sw.Close();
                            }
                            catch { }
                        }
                    }
                    else if ("FAILED" == trade_status.ToUpper())
                    {
                        try
                        {
                            //sw.Write("FAILED:返回错误" + "___|||___");
                            //sw.Flush();
                        }
                        catch { }
                        ViewData["Error"] = 1;
                        ViewData["Message"] = "返回出现错误，请联系客服。错误代码：" + extra_return_param;
                        try
                        {
                            //sw.Close();
                        }
                        catch { }
                    }
                }
                else
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = "出错原因不详，请联系客服。";
                    try
                    {
                        //sw.Close();
                    }
                    catch { }
                }
                return ViewExPath("Public/DinPayChargeOK", null, null);
            }
            #endregion
            return ViewExPath("Public/Public", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DinpayDone(FormCollection form)
        {
            //System.IO.StreamWriter sw = new System.IO.StreamWriter("C:\\Temp\\DinpayLog.txt", true);
            ViewData["Error"] = 0;
            ViewData["Message"] = string.Empty;
            var sysPayDinpayConfig = GetKV("SYS_PAY_DINPAY_CONFIG", false).cfg003;
            var sysPayDinpayConfigSplit = sysPayDinpayConfig.Split(',');
            Dictionary<int, string> dicSysPayDinpayConfig = new Dictionary<int, string>();
            for (int i = 0; i < sysPayDinpayConfigSplit.Length; i++)
            {
                dicSysPayDinpayConfig.Add(i, sysPayDinpayConfigSplit[i]);
            }
            //try
            //{
            //    sw.Write("config:" + sysPayDinpayConfig + "___|||___");
            //    sw.Flush();
            //}
            //catch { }
            //0-商户号;1-MD5;2-编码;3-中转URL（用于提交），4-提交到网关，5－产品名称，6－版本，7－后台通知地址，8－签名方式，9－业务类型，10－页面跳转同步通知地址(选填)，11－伪IP，12－是否调试（0调试，1不调试）
            //商号号
            string merchant_code = form["merchant_code"].ToString().Trim();
            //通知类型
            string notify_type = form["notify_type"].ToString().Trim();
            //通知校验ID
            string notify_id = form["notify_id"].ToString().Trim();
            //接口版本
            string interface_version = form["interface_version"].ToString().Trim();
            //签名方式
            string sign_type = form["sign_type"].ToString().Trim();
            //签名
            string dinpaySign = form["sign"].ToString().Trim();
            //商家订单号
            string order_no = form["order_no"].ToString().Trim();
            //商家订单时间
            string order_time = form["order_time"].ToString().Trim();
            //商家订单金额
            string order_amount = form["order_amount"].ToString().Trim();
            //回传参数
            string extra_return_param = form["extra_return_param"].ToString().Trim();
            //智付交易定单号
            string trade_no = form["trade_no"].ToString().Trim();
            //智付交易时间
            string trade_time = form["trade_time"].ToString().Trim();
            //交易状态 SUCCESS 成功  FAILED 失败
            string trade_status = form["trade_status"].ToString().Trim();
            //银行交易流水号
            string bank_seq_no = string.IsNullOrEmpty(form["bank_seq_no"]) ? string.Empty : form["bank_seq_no"].ToString().Trim();
            /**
             *签名顺序按照参数名a到z的顺序排序，若遇到相同首字母，则看第二个字母，以此类推，
            *同时将商家支付密钥key放在最后参与签名，组成规则如下：
            *参数名1=参数值1&参数名2=参数值2&……&参数名n=参数值n&key=key值
            **/
            //组织订单信息
            string signStr = "";
            if (bank_seq_no != "")
            {
                signStr = signStr + "bank_seq_no=" + bank_seq_no + "&";
            }
            if (extra_return_param != "")
            {
                signStr = signStr + "extra_return_param=" + extra_return_param + "&";
            }
            signStr = signStr + "interface_version=V3.0" + "&";
            signStr = signStr + "merchant_code=" + merchant_code + "&";
            if (notify_id != "")
            {
                signStr = signStr + "notify_id=" + notify_id + "&notify_type=" + notify_type + "&";
            }
            signStr = signStr + "order_amount=" + order_amount + "&";
            signStr = signStr + "order_no=" + order_no + "&";
            signStr = signStr + "order_time=" + order_time + "&";
            signStr = signStr + "trade_no=" + trade_no + "&";
            signStr = signStr + "trade_status=" + trade_status + "&";
            if (trade_time != "")
            {
                signStr = signStr + "trade_time=" + trade_time + "&";
            }
            string key = dicSysPayDinpayConfig[1];
            signStr = signStr + "key=" + key;
            string signInfo = signStr;
            //try
            //{
            //    sw.Write("signInfo:" + signStr + "___|||___");
            //    sw.Flush();
            //}
            //catch { }
            //将组装好的信息MD5签名
            string sign = FormsAuthentication.HashPasswordForStoringInConfigFile(signInfo, "md5").ToLower(); //注意与支付签名不同  此处对String进行加密
            //比较智付返回的签名串与商家这边组装的签名串是否一致
            //try
            //{
            //    sw.Write("dinpaySign:" + dinpaySign + "___|||___");
            //    sw.Write("sign:" + sign + "___|||___");
            //    sw.Flush();
            //}
            //catch { }
            if (dinpaySign == sign)
            {
                string[] info = extra_return_param.Split('|');
                if (6 != info.Length)
                {
                    ViewData["Error"] = 1;
                    ViewData["Message"] = "银行回传的信息不正确";
                    //try
                    //{
                    //    sw.Write("错误:银行回传的信息不正确" + "___|||___");
                    //    sw.Flush();
                    //}
                    //catch { }
                }
                if ("SUCCESS" == trade_status.ToUpper())
                {
                    //string extra_return_param1 = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", charge.u001, charge.u002.Trim(), charge.sb001, charge.ct001, charge.uc005.Trim(), charge.uc001);
                    DBModel.wgs019 existsCharge = null;
                    try
                    {
                        existsCharge = serFinance.GetCCash(long.Parse(info[5]));
                        if (0 == existsCharge.uc008)
                        {
                            existsCharge.uc003 = decimal.Parse(order_amount);
                            existsCharge.uc014 = 1;
                            existsCharge.uc009 = extra_return_param + "," + bank_seq_no;
                            existsCharge.uc008 = 1;
                            existsCharge.uc007 = DateTime.Now;
                            serFinance.UpdateCCash(existsCharge);
                            ViewData["Account"] = existsCharge.u002.Trim();
                            ViewData["ChargeCode"] = existsCharge.uc005.Trim();
                            ViewData["Money"] = existsCharge.uc003;
                            ViewData["ChargeOKDateTime"] = existsCharge.uc007.Value.ToString(GetKV("SYS_DATETIME_FORMAT", true).cfg003);
                            ViewData["Message"] = "SUCCESS";
                            //try
                            //{
                            //    sw.Write("成功:已更新状态" + "___|||___");
                            //    sw.Flush();
                            //}
                            //catch { }
                        }
                        else
                        {
                            ViewData["Error"] = 1;
                            ViewData["Message"] = "SUCCESS";
                        }
                    }
                    catch (Exception error)
                    {
                        //try
                        //{
                        //    sw.Write("Exception:" + error.Message + "___|||___");
                        //    sw.Flush();
                        //}
                        //catch { }
                        ViewData["Error"] = 1;
                        ViewData["Message"] = "取原始数据时出现错误，请联系客服。错误代码：" + error.Message;
                        //try
                        //{
                        //    sw.Close();
                        //}
                        //catch { }
                    }
                }
                else if ("FAILED" == trade_status.ToUpper())
                {
                    //try
                    //{
                    //    sw.Write("FAILED:返回错误" + "___|||___");
                    //    sw.Flush();
                    //}
                    //catch { }
                    ViewData["Error"] = 1;
                    ViewData["Message"] = "返回出现错误，请联系客服。错误代码：" + extra_return_param;
                    //try
                    //{
                    //    sw.Close();
                    //}
                    //catch { }
                }
            }
            else
            {
                ViewData["Error"] = 1;
                ViewData["Message"] = "出错原因不详，请联系客服。";
                //try
                //{
                //    sw.Close();
                //}
                //catch { }
            }
            return ViewExPath("Public/DinPayChargeOK", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult YeepayDone()
        {
            ViewData["Error"] = 0;
            var sysPayYeepayConfig = GetKV("SYS_PAY_YEEPAY_CONFIG", false).cfg003;
            var sysPayYeepayConfigSplit = sysPayYeepayConfig.Split('|');
            Dictionary<int, string> dicSysPayYeepayConfig = new Dictionary<int, string>();
            for (int i = 0; i < sysPayYeepayConfigSplit.Length; i++)
            {
                dicSysPayYeepayConfig.Add(i, sysPayYeepayConfigSplit[i]);
            }
            com.yeepay.icc.Buy.keyValue = dicSysPayYeepayConfig[1];
            com.yeepay.icc.Buy.merchantId = dicSysPayYeepayConfig[0];
            com.yeepay.icc.BuyCallbackResult result = com.yeepay.icc.Buy.VerifyCallback(com.yeepay.utils.FormatQueryString.GetQueryString("p1_MerId"), com.yeepay.utils.FormatQueryString.GetQueryString("r0_Cmd"), com.yeepay.utils.FormatQueryString.GetQueryString("r1_Code"), com.yeepay.utils.FormatQueryString.GetQueryString("r2_TrxId"),
            com.yeepay.utils.FormatQueryString.GetQueryString("r3_Amt"), com.yeepay.utils.FormatQueryString.GetQueryString("r4_Cur"), com.yeepay.utils.FormatQueryString.GetQueryString("r5_Pid"), com.yeepay.utils.FormatQueryString.GetQueryString("r6_Order"), com.yeepay.utils.FormatQueryString.GetQueryString("r7_Uid"),
            com.yeepay.utils.FormatQueryString.GetQueryString("r8_MP"), com.yeepay.utils.FormatQueryString.GetQueryString("r9_BType"), com.yeepay.utils.FormatQueryString.GetQueryString("rp_PayDate"), com.yeepay.utils.FormatQueryString.GetQueryString("hmac"));
            if (string.IsNullOrEmpty(result.ErrMsg))
            {
                //在接收到支付结果通知后，判断是否进行过业务逻辑处理，不要重复进行业务逻辑处理
                if (result.R1_Code == "1")
                {
                    if (result.R9_BType == "1")
                    {
                        //  callback方式:浏览器重定向
                        //Response.Write("支付成功!" +
                        //    "<br>接口类型:" + result.R0_Cmd +
                        //    "<br>返回码:" + result.R1_Code +
                        //    //"<br>商户号:" + result.P1_MerId +
                        //    "<br>交易流水号:" + result.R2_TrxId +
                        //    "<br>商户订单号:" + result.R6_Order +
                        //    "<br>交易金额:" + result.R3_Amt +
                        //    "<br>交易币种:" + result.R4_Cur +
                        //    "<br>订单完成时间:" + result.Rp_PayDate +
                        //    "<br>回调方式:" + result.R9_BType +
                        //    "<br>错误信息:" + result.ErrMsg + "<BR>");
                        DBModel.wgs019 existsCharge = null;
                        try
                        {
                            existsCharge = serFinance.GetCCash(result.R6_Order);
                            if (null == existsCharge)
                            {
                                ViewData["Error"] = 1;
                                ViewData["Message"] = "充值订单不存在，充值码：" + result.R6_Order;
                            }
                            else
                            {
                                if (0 == existsCharge.uc008)
                                {
                                    existsCharge.uc003 = decimal.Parse(result.R3_Amt);
                                    existsCharge.uc014 = 1;
                                    existsCharge.uc009 = result.R2_TrxId + "|" + result.R9_BType + "|" + result.Rp_PayDate;
                                    existsCharge.uc008 = 1;
                                    existsCharge.uc013 = 0;
                                    try
                                    {
                                        decimal rq_TargetFee = decimal.Parse(com.yeepay.utils.FormatQueryString.GetQueryString("rq_TargetFee"));
                                        existsCharge.uc013 = rq_TargetFee;
                                    }
                                    catch { }
                                    existsCharge.uc007 = DateTime.Now;
                                    ViewData["Account"] = existsCharge.u002.Trim();
                                    ViewData["ChargeCode"] = existsCharge.uc005.Trim();
                                    ViewData["Money"] = existsCharge.uc003;
                                    ViewData["ChargeOKDateTime"] = existsCharge.uc007.Value.ToString(GetKV("SYS_DATETIME_FORMAT", true).cfg003);
                                    ViewData["Message"] = "SUCCESS";
                                }
                                else
                                {
                                    ViewData["Error"] = 1;
                                    ViewData["Message"] = "SUCCESS";
                                }
                            }
                        }
                        catch (Exception error)
                        {
                            ViewData["Error"] = 1;
                            ViewData["Message"] = "取原始数据时出现错误，请联系客服。错误代码：" + error.Message;
                        }
                    }
                    else if (result.R9_BType == "2")
                    {
                        // * 如果是服务器返回则需要回应一个特定字符串'SUCCESS',且在'SUCCESS'之前不可以有任何其他字符输出,保证首先输出的是'SUCCESS'字符串
                        //Response.Write("SUCCESS");
                        //Response.Write("支付成功!" +
                        //     "<br>接口类型:" + result.R0_Cmd +
                        //     "<br>返回码:" + result.R1_Code +
                        //    //"<br>商户号:" + result.P1_MerId +
                        //     "<br>交易流水号:" + result.R2_TrxId +
                        //     "<br>商户订单号:" + result.R6_Order +
                        //     "<br>交易金额:" + result.R3_Amt +
                        //     "<br>交易币种:" + result.R4_Cur +
                        //     "<br>订单完成时间:" + result.Rp_PayDate +
                        //     "<br>回调方式:" + result.R9_BType +
                        //     "<br>错误信息:" + result.ErrMsg + "<BR>");
                        DBModel.wgs019 existsCharge = null;
                        try
                        {
                            existsCharge = serFinance.GetCCash(result.R6_Order);
                            if (null == existsCharge)
                            {
                                Response.Write("ERROR");
                                Response.End();
                            }
                            if (0 == existsCharge.uc008)
                            {
                                existsCharge.uc003 = decimal.Parse(result.R3_Amt);
                                existsCharge.uc014 = 1;
                                existsCharge.uc009 = result.R2_TrxId + "|" + result.R9_BType + "|" + result.Rp_PayDate;
                                existsCharge.uc008 = 1;
                                existsCharge.uc013 = 0;
                                try
                                {
                                    decimal rq_TargetFee = decimal.Parse(com.yeepay.utils.FormatQueryString.GetQueryString("rq_TargetFee"));
                                    existsCharge.uc013 = rq_TargetFee;
                                }
                                catch { }
                                existsCharge.uc007 = DateTime.Now;
                                serFinance.UpdateCCash(existsCharge);
                                ViewData["Account"] = existsCharge.u002.Trim();
                                ViewData["ChargeCode"] = existsCharge.uc005.Trim();
                                ViewData["Money"] = existsCharge.uc003;
                                ViewData["ChargeOKDateTime"] = existsCharge.uc007.Value.ToString(GetKV("SYS_DATETIME_FORMAT", true).cfg003);
                                Response.Write("SUCCESS");
                                Response.End();
                            }
                            else
                            {
                                Response.Write("SUCCESS");
                                Response.End();
                            }
                        }
                        catch (Exception error)
                        {
                            Response.Write(error.Message);
                            Response.End();
                        }
                    }
                }
                else
                {
                    //Response.Write("支付失败!" +
                    //         "<br>接口类型:" + result.R0_Cmd +
                    //         "<br>返回码:" + result.R1_Code +
                    //    //"<br>商户号:" + result.P1_MerId +
                    //         "<br>交易流水号:" + result.R2_TrxId +
                    //         "<br>商户订单号:" + result.R6_Order +
                    //         "<br>交易金额:" + result.R3_Amt +
                    //         "<br>交易币种:" + result.R4_Cur +
                    //         "<br>订单完成时间:" + result.Rp_PayDate +
                    //         "<br>回调方式:" + result.R9_BType +
                    //         "<br>错误信息:" + result.ErrMsg + "<BR>");
                    Response.Write(result.R6_Order + "|" + result.ErrMsg);
                    Response.End();
                }
            }
            else
            {
                Response.Write("交易签名无效");
                Response.End();
            }
            return null;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IpspayDone()
        {
            ViewData["Error"] = 0;
            var sysPayIpsConfig = GetKV("SYS_PAY_IPS_CONFIG", false).cfg003;
            var sysPayIpsConfigSplit = sysPayIpsConfig.Split('|');
            int test = int.Parse(sysPayIpsConfigSplit[14]);
            //Dictionary<int, string> dicSysPayYeepayConfig = new Dictionary<int, string>();
            //for (int i = 0; i < sysPayYeepayConfigSplit.Length; i++)
            //{
            //    dicSysPayYeepayConfig.Add(i, sysPayYeepayConfigSplit[i]);
            //}
            //接收数据
            string billno = Request["billno"];
            string amount = Request["amount"];
            string currency_type = Request["Currency_type"];
            string mydate = Request["date"];
            string succ = Request["succ"];
            string msg = Request["msg"];
            string attach = Request["attach"];
            string ipsbillno = Request["ipsbillno"];
            string retEncodeType = Request["retencodetype"];
            string signature = Request["signature"];
            string bankbillno = Request["bankbillno"];
            //签名原文
            //billno+【订单编号】+currencytype+【币种】+amount+【订单金额】+date+【订单日期】+succ+【成功标志】+ipsbillno+【IPS订单编号】+retencodetype +【交易返回签名方式】
            string content = "billno" + billno + "currencytype" + currency_type + "amount" + amount + "date" + mydate + "succ" + succ + "ipsbillno" + ipsbillno + "retencodetype" + retEncodeType;
            //签名是否正确
            Boolean verify = false;
            ////验证方式：16-md5withRSA  17-md5
            //if (retEncodeType == "16")
            //{
            //    string pubfilename = sysPayIpsConfigSplit[15 + test];
            //    RSAMD5Class m_RSAMD5Class = new RSAMD5Class();
            //    int result = m_RSAMD5Class.VerifyMessage(pubfilename, content, signature);
            //    ////result=0   表示签名验证成功
            //    ////result=-1  表示系统错误
            //    ////result=-2  表示文件绑定错误
            //    ////result=-3  表示读取公钥失败
            //    ////result=-4  表示签名长度错
            //    ////result=-5  表示签名验证失败
            //    ////result=-99 表示系统锁定失败
            //    if (result == 0)
            //    {
            //        verify = true;
            //    }
            //}
            if (retEncodeType == "17")
            {
                //登陆http://merchant.ips.com.cn/商户后台下载的商户证书内容
                string merchant_key = sysPayIpsConfigSplit[4 + test];
                //Md5摘要
                string signature1 = FormsAuthentication.HashPasswordForStoringInConfigFile(content + merchant_key, "MD5").ToLower();
                if (signature1 == signature)
                {
                    verify = true;
                }
            }
            //判断签名验证是否通过
            if (verify == true)
            {
                //判断交易是否成功
                if (succ != "Y")
                {
                    Response.Write("交易失败！");
                    Response.End();
                }
                else
                {
                    /****************************************************************
                    //比较返回的订单号和金额与您数据库中的金额是否相符			
                    if(不等)
                    {
                        Response.Write("从IPS返回的数据和本地记录的不符合，失败！");
                        Response.End(); 
                    }
                    else
                    {
                        Reponse.Write("交易成功，处理您的数据库！");
                    }
                    ****************************************************************/
                    DBModel.wgs019 existsCharge = null;
                    try
                    {
                        existsCharge = serFinance.GetCCash(billno);
                        if (null == existsCharge)
                        {
                            ViewData["Error"] = 1;
                            ViewData["Message"] = "充值订单不存在，充值码：" + billno;
                        }
                        else
                        {
                            if (0 == existsCharge.uc008)
                            {
                                existsCharge.uc003 = decimal.Parse(amount);
                                existsCharge.uc014 = 1;
                                // existsCharge.uc009 = result.R2_TrxId + "|" + result.R9_BType + "|" + result.Rp_PayDate;
                                existsCharge.uc008 = 1;
                                existsCharge.uc013 = 0;
                                try
                                {
                                    decimal rq_TargetFee = decimal.Parse(com.yeepay.utils.FormatQueryString.GetQueryString("rq_TargetFee"));
                                    existsCharge.uc013 = rq_TargetFee;
                                }
                                catch { }
                                existsCharge.uc007 = DateTime.Now;
                                serFinance.UpdateCCash(existsCharge);
                                ViewData["Account"] = existsCharge.u002.Trim();
                                ViewData["ChargeCode"] = existsCharge.uc005.Trim();
                                ViewData["Money"] = existsCharge.uc003;
                                ViewData["ChargeOKDateTime"] = existsCharge.uc007.Value.ToString(GetKV("SYS_DATETIME_FORMAT", true).cfg003);
                                Response.Write("SUCCESS");
                                Response.End();
                            }
                            else
                            {
                                ViewData["Error"] = 1;
                                ViewData["Message"] = "SUCCESS";
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    //Response.Write("交易成功！");
                    //Response.End();
                }
            }
            else
            {
                Response.Write("签名不正确！");
            }
            return null;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserPrizeInfo(FormCollection form)
        {
            string gpkey = form["gpkey"];
            string gckey = form["gckey"];
            int gpID = 0;
            int gcID = 0;
            if (_NWC.GeneralValidate.IsNullOrEmpty(gckey) || _NWC.GeneralValidate.IsNullOrEmpty(gpkey))
            {
                throw new Exception("数据不能为空");
            }
            gpID = int.Parse(gpkey);
            gcID = int.Parse(gckey);
            var agupList = serUser.GetAGUPData(UILoginUser.u001);
            var agupItemCount = agupList.Count(exp => exp.gp001 == gpID);
            if (0 == agupItemCount)
            {
                throw new Exception("返点信息不存在");
            }
            var agupItem = agupList.Where(exp => exp.gp001 == gpID).FirstOrDefault();
            var gpItem = serGame.GetGameClassPrize(gcID, gpID);
            if (null == gpItem)
            {
                throw new Exception("奖金组不存在");
            }
            ViewData["SysMaxPoint"] = gpItem.gp008;
            ViewData["MyMaxPoint"] = agupItem.up003;
            //var list = serGame.GetGameMethodPrizeData(UILoginUser.u001, gcID, gpID);
            //var gameMethodPrizeDataDicList = list.ToDictionary<DBModel.wgs008,int>(key => key.gm001);
            //ViewData["GameClassList"] = serGame.GetGameClassListByCache();
            //ViewData["GameMethodList"] = serGame.GetGameMethodListByCache().Where(exp=>exp.gm002!=0).ToList();
            //ViewData["GameMethodGroupList"] = serGame.GetGameMethodGroupListByCache();
            //ViewData["GameMethodPrizeDataDicList"] = gameMethodPrizeDataDicList;
            var gpdGroupList = serGame.GetGPDDataGroupList((int)gcID);
            ViewData["GPDDataGroup"] = gpdGroupList;
            ViewData["DicItemList"] = serGame.GetGPDDataListByCache().ToDictionary(exp => exp.gtp001);
            ViewData["GPDDataList"] = serGame.GetSetGPDDataList((int)gpID);
            return ViewExPath("Common/UserPrizeInfo", null, null);
        }
        private ActionResult PrivateMyInfo()
        {
            AJAXObject ajax = new AJAXObject();
            var aguData = serUser.GetAGU(UILoginUser.u001);
            ajax.Data = 0;
            if (null != aguData)
            {
                var postName = string.Empty;
                var posItem = serUser.GetUserPositionLevel(true).Where(exp => exp.Level == aguData.u013).FirstOrDefault();
                if (null != posItem)
                {
                    postName = posItem.Name;
                }
                serSystem.GetUnReadMessage(0, UILoginUser.u001, 0);
                var combuyCount = serGame.GetCombuyCount(0);
                var service = GetKV("CUSTOMER_SERVICE_LINK", true).cfg003;
                var name = UILoginUser.u002;
                ajax.Code = 1;
                ajax.Data = new { M = aguData.wgs014.uf001.ToString("N2"), P = aguData.wgs014.uf004.ToString("N2"), L = aguData.u015, LN = serUser.GetUserLevel(true)[aguData.u015], MSG = serSystem.GetUnReadMessage(0, UILoginUser.u001, 0), POS = postName, CBC = combuyCount, kf = service, N = name };
            }
            return Json(ajax, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult MyInfo()
        {
            return PrivateMyInfo();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MyInfo(FormCollection form)
        {
            return PrivateMyInfo();
        }

        #region "验证余额用 ORZ"

        private ActionResult PIsChange()
        {
            AJAXObject ajax = new AJAXObject();

            ajax.Code = 1;
            if (Session["UILoginID"] != null)
            {
                ajax.Data = new GameServices.User_ORZ().IsPost(Convert.ToInt32(Session["UILoginID"])
                    , Convert.ToDouble(Request["001"])
                    , Convert.ToDouble(Request["004"]));
            }
            else
            {
                ajax.Data = "-1";
            }
            return Json(ajax, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult IsChange()
        {
            return PIsChange();
        }

        #endregion

        #region "刷新中奖列表 ORZ"
        private ActionResult PShowList()
        {
            AJAXObject ajax = new AJAXObject();
            ajax.Code = 1;
            if (Session["UILoginID"] != null)
            {
                //ajax.Data = new GameServices.User_ORZ().Get053ListUI2();
                ajax.Data = "";
            }
            else
            {
                ajax.Data = "";
            }
            return Json(ajax, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShowList()
        {
            return PShowList();
        }
        #endregion.

        #region "刷新游戏记录信息 ORZ"

        private ActionResult POrderChange()
        {
            AJAXObject ajax = new AJAXObject();

            ajax.Code = 1;
            if (Session["UILoginID"] != null)
            {
                ajax.Data = new GameServices.User_ORZ().IsOrderChange(Convert.ToInt32(Session["UILoginID"]));
            }
            else
            {
                ajax.Data = "-1";
            }
            return Json(ajax, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult OrderChange()
        {
            return POrderChange();
        }

        #endregion


        #region "撤单 ORZ"

        private ActionResult PCancelOrder()
        {
            AJAXObject ajax = new AJAXObject();

            ajax.Code = 1;
            if (Session["UILoginID"] != null)
            {

                string temp = "";
                AllCancelOrder(Request["type"].ToString(), Request["orderID"].ToString(), ref temp);
                //ajax.Data = new GameServices.User_ORZ().IsOrderChange(Convert.ToInt32(Session["UILoginID"]));
                ajax.Data = temp;
            }
            else
            {
                ajax.Data = "-1";
            }
            return Json(ajax, JsonRequestBehavior.AllowGet);
        }

        private ActionResult AllCancelOrder(string flag, string orderID, ref string xmsg)
        {
            if ("cancelorder" == flag)
            {
                if (string.IsNullOrEmpty(orderID))
                {
                    return Json(new { Message = "empty", stats = "error", data = "订单编号为空0x1" });
                }
                var ids = orderID.Split(',');
                List<long> orderIDs = new List<long>();
                foreach (var id in ids)
                {
                    try
                    {
                        orderIDs.Add(long.Parse(id));
                    }
                    catch
                    {
                    }
                }
                if (0 < orderIDs.Count)
                {
                    var cancelTOrder = serGame.CancelTOrder(orderIDs, 1, UILoginUser.u001, 0, string.Empty);
                    if (0 == cancelTOrder.Code)
                    {
                        xmsg = cancelTOrder.Message;
                        return Json(new { Message = "empty", stats = "error", data = cancelTOrder.Message });

                    }
                    xmsg = "success";
                    return Json(new { Message = "empty", stats = "success", data = "success" });
                }
                else
                {
                    xmsg = "订单编号为空0x2";
                    return Json(new { Message = "empty", stats = "error", data = "订单编号为空0x2" });
                }
            }

            if ("cancelTOrder" == flag)
            {

                if (string.IsNullOrEmpty(orderID))
                {
                    xmsg = "追号订单编号为空0x1";
                    return Json(new { Message = "empty", stats = "error", data = "追号订单编号为空0x1" });
                }
                var ids = orderID.Split(',');
                List<long> orderIDs = new List<long>();
                foreach (var id in ids)
                {
                    try
                    {
                        orderIDs.Add(long.Parse(id));
                    }
                    catch
                    {
                    }
                }
                if (0 < orderIDs.Count)
                {
                    var cancelTOrder = serGame.CancelTOrder(orderIDs, 1, UILoginUser.u001, 0, string.Empty);
                    if (0 == cancelTOrder.Code)
                    {
                        xmsg = cancelTOrder.Message;
                        return Json(new { Message = "empty", stats = "error", data = cancelTOrder.Message });
                    }
                    xmsg = "success";
                    return Json(new { Message = "empty", stats = "success", data = "success" });
                }
                else
                {
                    xmsg = "追号订单编号为空0x2";
                    return Json(new { Message = "empty", stats = "error", data = "追号订单编号为空0x2" });
                }
            }


            xmsg = "错误的撤单类型";
            return Json(new { Message = "empty", stats = "error", data = "错误的撤单类型" });


        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CancelOrder()
        {
            return PCancelOrder();
        }


        #endregion

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Record(int? pageIndex)
        {
            var pageURL = string.Empty;
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(MethodType))
            {
                ViewData["MethodType"] = "orderDefault";
                MethodType = "orderDefault";
            }
            else
            {
                ViewData["MethodType"] = MethodType;
            }
            ViewData["OrderGameClass"] = _NWC.GeneralValidate.IsNullOrEmpty(Request["orderGameClass"]) ? "0" : Request["orderGameClass"];
            ViewData["OrderGame"] = _NWC.GeneralValidate.IsNullOrEmpty(Request["orderGame"]) ? "0" : Request["orderGame"];
            ViewData["OrderMethod"] = _NWC.GeneralValidate.IsNullOrEmpty(Request["orderMethod"]) ? "0" : Request["orderMethod"];
            #region 默认订单
            if ("orderDefault" == MethodType)
            {
                var _orderDts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Now.ToString("yyyy-MM-dd 00:00:00") : Request["orderDTS"];
                var _orderDte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Now.ToString("yyyy-MM-dd 23:59:59") : Request["orderDTE"]; ;
                var _orderUserType = Request["orderUser"];
                var _orderGameClass = Request["orderGameClass"];
                var _orderGame = Request["orderGame"];
                var _orderGameMethod = Request["orderMethod"];
                var _orderCancel = Request["orderCancel"];
                var _orderID = Request["orderID"];
                var _modes = Request["orderModes"];
                var _orderIsWin = Request["orderIsWin"];
                var _issue = Request["issue"];
                var orderDts = DateTime.Parse(_orderDts);
                var orderDte = DateTime.Parse(_orderDte);
                var orderUserType = !_NWC.GeneralValidate.IsNumber(_orderUserType) ? 0 : int.Parse(_orderUserType);
                var orderGameClass = !_NWC.GeneralValidate.IsNumber(_orderGameClass) ? 0 : int.Parse(_orderGameClass);
                var orderGame = !_NWC.GeneralValidate.IsNumber(_orderGame) ? 0 : int.Parse(_orderGame);
                var orderGameMethod = !_NWC.GeneralValidate.IsNumber(_orderGameMethod) ? 0 : int.Parse(_orderGameMethod);
                var orderCancel = !_NWC.GeneralValidate.IsNumber(_orderCancel) ? -1 : int.Parse(_orderCancel);
                var orderID = !_NWC.GeneralValidate.IsNumber(_orderID) ? (long)0 : long.Parse(_orderID);
                var modes = !_NWC.GeneralValidate.IsNumber(_modes) ? 0 : int.Parse(_modes);
                var orderIsWin = !_NWC.GeneralValidate.IsNumber(_orderIsWin) ? 0 : int.Parse(_orderIsWin);
                var issue = string.IsNullOrEmpty(_issue) || string.IsNullOrWhiteSpace(_issue) ? string.Empty : _issue;
                int recordCount = 0;
                var orderList = serGame.GetOrderList(orderID, orderGame, orderGameClass, orderGameMethod, UILoginUser.u001, orderUserType, null, orderCancel, orderIsWin, 0, 0, 0, 0, 0, issue, 0, 0, modes, 0, 0, string.Empty, null, orderDts, orderDte, (int)pageIndex, PageSize, out recordCount);
                ViewData["DTS"] = orderDts;
                ViewData["DTE"] = orderDte;
                ViewData["OrderUserType"] = orderUserType;
                ViewData["OrderGameClass"] = orderGameClass;
                ViewData["OrderGame"] = orderGame;
                ViewData["OrderGameMethod"] = orderGameMethod;
                ViewData["OrderCancel"] = orderCancel;
                ViewData["OrderID"] = orderID;
                ViewData["OrderModes"] = modes;
                ViewData["OrderIsWin"] = orderIsWin;
                ViewData["Issue"] = issue;
                ViewData["OrderList"] = orderList;
                ViewData["OrderShowList"] = serGame.GetOrderShowList(orderList);
                ViewData["GameClassList"] = serGame.GetGameClassListByCache();
                ViewData["GameList"] = serGame.GetGameListByCache();
                ViewData["GameMethodList"] = serGame.GetGameMethodListByCache();
                pageURL = Url.Action("Record", "UI2", new { method = MethodType, orderDTS = ViewData["DTS"], orderDTE = ViewData["DTE"], orderUser = orderUserType, orderCancel = orderCancel, orderMethod = orderGameMethod, orderGame = orderGame, orderGameClass = orderGameClass, orderID = orderID, orderIsWin = _orderIsWin });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
            }
            #endregion
            #region 追号
            else if ("orderTrace" == MethodType)
            {
                var _orderDts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Now.ToString("yyyy-MM-dd 00:00:00") : Request["orderDTS"];
                var _orderDte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Now.ToString("yyyy-MM-dd 23:59:59") : Request["orderDTE"]; ;
                var _orderUserType = Request["orderUser"];
                var _orderGameClass = Request["orderGameClass"];
                var _orderGame = _NWC.GeneralValidate.IsNumber(Request["orderGame"]) ? Request["orderGame"] : "0";
                var _orderGameMethod = Request["orderMethod"];
                var _orderStatus = Request["orderStatus"];
                var _orderID = Request["orderID"];
                var _modes = Request["orderModes"];
                var _orderUserName = Request["orderUserName"];
                var orderDts = DateTime.Parse(_orderDts);
                var orderDte = DateTime.Parse(_orderDte);
                var orderUserType = !_NWC.GeneralValidate.IsNumber(_orderUserType) ? 0 : int.Parse(_orderUserType);
                var orderGameClass = !_NWC.GeneralValidate.IsNumber(_orderGameClass) ? 0 : int.Parse(_orderGameClass);
                var orderGame = !_NWC.GeneralValidate.IsNumber(_orderGame) ? 0 : int.Parse(_orderGame);
                var orderGameMethod = !_NWC.GeneralValidate.IsNumber(_orderGameMethod) ? 0 : int.Parse(_orderGameMethod);
                var orderStatus = !_NWC.GeneralValidate.IsNumber(_orderStatus) ? -1 : int.Parse(_orderStatus);
                var orderID = !_NWC.GeneralValidate.IsNumber(_orderID) ? (long)0 : long.Parse(_orderID);
                var modes = !_NWC.GeneralValidate.IsNumber(_modes) ? 0 : int.Parse(_modes);
                var orderUserName = string.IsNullOrEmpty(_orderUserName) || string.IsNullOrWhiteSpace(_orderUserName) ? string.Empty : _orderUserName;
                ViewData["DTS"] = orderDts;
                ViewData["DTE"] = orderDte;
                ViewData["OrderUserType"] = orderUserType;
                ViewData["OrderGameClass"] = orderGameClass;
                ViewData["OrderGame"] = orderGame;
                ViewData["OrderGameMethod"] = orderGameMethod;
                ViewData["OrderStatus"] = orderStatus;
                ViewData["OrderID"] = orderID;
                ViewData["OrderModes"] = modes;
                ViewData["OrderUserName"] = orderUserName;
                var recordCount = 0;
                var tOrderList = serGame.GetTOrderList(orderID, orderGame, orderGameClass, string.Empty, UILoginUser.u001, orderUserType, string.Empty, orderDts, orderDte, (int)pageIndex, PageSize, out recordCount);
                pageURL = Url.Action("Record", "UI2", new { method = MethodType, orderDTS = ViewData["DTS"], orderDTE = ViewData["DTE"], orderUser = orderUserType, orderStatus = orderStatus, orderMethod = orderGameMethod, orderGame = orderGame, orderGameClass = orderGameClass, orderID = orderID, orderUserName = orderUserName });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
                ViewData["TOrderList"] = tOrderList;
                ViewData["GameClassList"] = serGame.GetGameClassListByCache();
                ViewData["GameList"] = serGame.GetGameListByCache();
                ViewData["OrderGame"] = orderGame;
            }
            #endregion
            #region 追号详细
            else if ("orderTraceDetail" == MethodType)
            {
                int recordCount = 0;
                var _traceKey = _NWC.GeneralValidate.IsNumber(Request["key"]) == false ? 0 : long.Parse(Request["key"]);
                var traceObject = serGame.GetTOrderItem(_traceKey, 0, 0, UILoginUser.u001, (int)pageIndex, PageSize, out recordCount);
                ViewData["TraceObject"] = traceObject;
                ViewData["GameList"] = serGame.GetGameListByCache();
                ViewData["TKey"] = _traceKey;
                pageURL = Url.Action("Record", "UI2", new { method = MethodType, key = _traceKey });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
                return ViewExPath("OrderTraceDetail", null, null);
            }
            #endregion
            else if ("orderCombine" == MethodType)
            {
                var _orderDts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")) : DateTime.Parse(Request["orderDTS"]);
                var _orderDte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")) : DateTime.Parse(Request["orderDTE"]);
                var _orderGame = _NWC.GeneralValidate.IsNumber(Request["orderGame"]) ? Request["orderGame"] : "0";
                var _issue = string.IsNullOrEmpty(Request["issue"]) || string.IsNullOrWhiteSpace(Request["issue"]) ? string.Empty : Request["issue"];
                var _account = string.IsNullOrEmpty(Request["account"]) || string.IsNullOrWhiteSpace(Request["account"]) ? string.Empty : Request["account"];
                var _status = _NWC.GeneralValidate.IsNumber(Request["status"]) ? int.Parse(Request["status"]) : 0;
                var orderDts = _orderDts;
                var orderDte = _orderDte;
                var orderGame = int.Parse(_orderGame);
                var issue = _issue;
                var account = _account;
                var status = _status;
                ViewData["DTS"] = orderDts;
                ViewData["DTE"] = orderDte;
                ViewData["OrderGame"] = orderGame;
                ViewData["Issue"] = issue;
                ViewData["Account"] = account;
                ViewData["GameClassList"] = serGame.GetGameClassListByCache();
                ViewData["GameList"] = serGame.GetGameListByCache();
                int recordCount = 0;
                var combuyList = serGame.GetCombuyList(UILoginUser.u001, account, orderGame, 0, issue, -1, (int)pageIndex, PageSize, orderDts, orderDte, out recordCount);
                ViewData["CombuyList"] = combuyList;
                pageURL = Url.Action("Record", "UI2", new { method = MethodType, orderDTS = ViewData["DTS"], orderDTE = ViewData["DTE"], orderGame = orderGame, issue = issue });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
            }
            return ViewExPath("Order", null, null);
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Combine(int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            var _orderDts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTS"];
            var _orderDte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTE"];
            var _orderGame = _NWC.GeneralValidate.IsNumber(Request["orderGame"]) ? Request["orderGame"] : "0";
            var _issue = string.IsNullOrEmpty(Request["issue"]) || string.IsNullOrWhiteSpace(Request["issue"]) ? string.Empty : Request["issue"];
            var _account = string.IsNullOrEmpty(Request["account"]) || string.IsNullOrWhiteSpace(Request["account"]) ? string.Empty : Request["account"];
            var _status = _NWC.GeneralValidate.IsNumber(Request["status"]) ? int.Parse(Request["status"]) : 0;
            var orderDts = DateTime.Parse(_orderDts + " 00:00:00");
            var orderDte = DateTime.Parse(_orderDte + " 23:59:59");
            var orderGame = int.Parse(_orderGame);
            var issue = _issue;
            var account = _account;
            var status = _status;
            ViewData["DTS"] = orderDts.ToString("yyyy-MM-dd");
            ViewData["DTE"] = orderDte.ToString("yyyy-MM-dd");
            ViewData["OrderGame"] = orderGame;
            ViewData["Issue"] = issue;
            ViewData["Account"] = account;
            ViewData["Status"] = status;
            ViewData["GameClassList"] = serGame.GetGameClassListByCache();
            ViewData["GameList"] = serGame.GetGameListByCache();
            int recordCount = 0;
            var combuyList = serGame.GetCombuyList(0, account, orderGame, 0, issue, status, (int)pageIndex, PageSize, orderDts, orderDte, out recordCount);
            ViewData["CombuyList"] = combuyList;
            var pageURL = Url.Action("Combine", "UI2", new { method = MethodType, dts = ViewData["DTS"], dte = ViewData["DTE"], orderGame = orderGame, issue = issue, status = status });
            ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
            return ViewExPath("Combine", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Combine(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            var key = _NWC.GeneralValidate.IsNumber(form["key"]) ? long.Parse(form["key"]) : 0;
            if ("getCombine" == MethodType)
            {
                if (0 == key)
                {
                    ajax.Message = "数据不存在0x1";
                    return Json(ajax);
                }
                var combineItem = serGame.GetCombuy(key, 0);
                if (null == combineItem)
                {
                    ajax.Message = "数据不存在0x2";
                    return Json(ajax);
                }
                //var isAllow = serGame.GameSessionIsAllow(combineItem.g001, combineItem.gs002);
                //if (0 == isAllow.Code)
                //{
                //    ajax.Message = "本期已经封盘";
                //    return Json(ajax);
                //}
                var modeName = string.Empty;
                switch (combineItem.sco016)
                {
                    case 1:
                        modeName = "元";
                        break;
                    case 2:
                        modeName = "角";
                        break;
                    case 3:
                        modeName = "分";
                        break;
                }
                var gameList = serGame.GetGameListByCache().ToDictionary(exp => exp.g001);
                var gmList = serGame.GetGameMethodListByCache().ToDictionary(exp => exp.gm001);
                var recordCount = 0;
                var joinOrderList = serGame.GetOrderList(0, 0, 0, 0, 0, -1, string.Empty, 0, 0, 0, 0, 0, 0, 0, string.Empty, 0, 0, 0, 0, key, string.Empty, null, null, null, 0, 500, out recordCount);
                var joinOrderShowList = serGame.GetOrderShowList(joinOrderList);
                joinOrderShowList = joinOrderShowList.Where(exp => exp.username != combineItem.u002.Trim()).ToList();
                var dataObj = new { User = combineItem.u002.Trim(), Code = combineItem.sco013.Trim(), CanBetNums = combineItem.sco011, JoinNums = combineItem.sco012, IfPrize = combineItem.sco015 == null ? string.Empty : combineItem.sco015.Trim(), TotalMoney = combineItem.sco007.ToString(GetKV("SYS_MONEY_FORMAT", true).cfg003), JoinMoney = combineItem.sco008.ToString(GetKV("SYS_MONEY_FORMAT", true).cfg003), PostTime = combineItem.sco017.ToString(combineItem.sco017.ToString(GetKV("SYS_DATETIME_FORMAT", true).cfg003)), OrderTimes = combineItem.sco019, GameName = gameList[combineItem.g001].g003, GameMethod = gmList[combineItem.gm001].gm004, Issue = combineItem.gs002.Trim(), GameResult = combineItem.gs007 == null ? string.Empty : combineItem.gs007.Trim(), Mode = combineItem.sco016, ModeName = modeName, CloseTime = combineItem.gs004.Value.ToString("yyyy/MM/dd HH:mm:ss"), IsPwd = combineItem.sco010 == null ? 0 : 1, HashKey = _NWC.DEncrypt.Encrypt(combineItem.sco001 + "|" + combineItem.gs001 + "|" + combineItem.gs002.Trim()), OriTotalMoney = combineItem.sco007, JoinList = joinOrderShowList, MyPercent = (int)combineItem.sco004 };
                ajax.Code = 1;
                ajax.Data = dataObj;
                return Json(ajax);
            }
            else if ("joinCombine" == MethodType)
            {
                var _hashKey = form["hash_key"];
                var _password = form["bet_password"];
                var _times = form["bet_times"];
                var hashKey = string.IsNullOrWhiteSpace(_hashKey) || string.IsNullOrEmpty(_hashKey) ? string.Empty : _hashKey;
                var password = string.IsNullOrWhiteSpace(_password) || string.IsNullOrEmpty(_password) ? string.Empty : _password;
                var times = _NWC.GeneralValidate.IsNumber(_times) ? int.Parse(_times) : 0;
                if (string.IsNullOrEmpty(hashKey))
                {
                    ajax.Message = "信息不存在";
                    return Json(ajax);
                }
                var hashKeyOri = _NWC.DEncrypt.Decrypt(hashKey);
                var hashKeyOriSplit = hashKeyOri.Split('|');
                if (3 != hashKeyOriSplit.Length)
                {
                    ajax.Message = "信息不正确";
                    return Json(ajax);
                }
                var result = serGame.JoinCombuy(long.Parse(hashKeyOriSplit[0]), UILoginUser.u001, times, password);
                ajax.Code = result.Code;
                ajax.Message = result.Message;
                return Json(ajax);
            }
            return ViewExPath("Public/Public", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Member(int? pageIndex)
        {
            var pageURL = string.Empty;
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(MethodType))
            {
                ViewData["MethodType"] = "accountList";
                MethodType = "accountList";
            }
            else
            {
                ViewData["MethodType"] = MethodType;
            }
            ViewData["AGLevelNameList"] = serUser.GetUserLevel(true);
            var posLevel = serUser.GetUserPositionLevel(false);
            ViewData["PosList"] = posLevel;
            int recordCount = 0;
            if ("accountList" == MethodType)
            {
                var _parentID = _NWC.GeneralValidate.IsNumber(Request["parentID"]) == false ? UILoginUser.u001 : int.Parse(Request["parentID"]);
                var _userName = string.IsNullOrEmpty(Request["userName"]) ? string.Empty : Request["userName"];
                var _status = _NWC.GeneralValidate.IsNumber(Request["userStatus"]) == false ? -1 : int.Parse(Request["userStatus"]);
                var _rDTS = _NWC.GeneralValidate.IsDatetime(Request["regDTS"]) == false ? (DateTime?)null : DateTime.Parse(Request["regDTS"]);
                var _rDTE = _NWC.GeneralValidate.IsDatetime(Request["regDTE"]) == false ? (DateTime?)null : DateTime.Parse(Request["regDTE"]);
                var _lDTS = _NWC.GeneralValidate.IsDatetime(Request["loginDTS"]) == false ? (DateTime?)null : DateTime.Parse(Request["loginDTS"]);
                var _lDTE = _NWC.GeneralValidate.IsDatetime(Request["loginDTE"]) == false ? (DateTime?)null : DateTime.Parse(Request["loginDTE"]);
                var _amtT = _NWC.GeneralValidate.IsNumber(Request["amountType"]) == false ? 0 : int.Parse(Request["amountType"]);
                var _amtV = _NWC.GeneralValidate.IsNumber(Request["amountTypeV"]) == false ? 0 : int.Parse(Request["amountTypeV"]);
                var _pntT = _NWC.GeneralValidate.IsNumber(Request["pointType"]) == false ? 0 : int.Parse(Request["pointType"]);
                var _pntV = _NWC.GeneralValidate.IsNumber(Request["pointTypeV"]) == false ? 0 : int.Parse(Request["pointTypeV"]);
                var parentID = _parentID;
                var userName = _userName;
                var status = _status;
                var rDTS = _rDTS == null ? (DateTime?)null : _rDTS;
                var rDTE = _rDTE == null ? (DateTime?)null : _rDTE;
                var lDTS = _lDTS == null ? (DateTime?)null : _lDTS;
                var lDTE = _lDTE == null ? (DateTime?)null : _lDTE;
                var amtT = _amtT;
                var amtV = _amtV;
                var pntT = _pntT;
                var pntV = _pntV;
                ViewData["RDTS"] = rDTS;
                ViewData["RDTE"] = rDTE;
                ViewData["LDTS"] = lDTS;
                ViewData["LDTE"] = lDTE;
                ViewData["UserName"] = userName;
                ViewData["UserStatus"] = status;
                ViewData["AmtT"] = amtT;
                ViewData["AmtV"] = amtV;
                ViewData["PntT"] = pntT;
                ViewData["PntV"] = pntV;
                var userPackList = serUser.GetAGUList(UILoginUser.u001, parentID, _userName, status, amtT, amtV, pntT, pntV, (int)pageIndex, PageSize, rDTS, rDTE, lDTS, lDTE, out recordCount);
                ViewData["UserPackList"] = userPackList;
                pageURL = Url.Action("Member", "UI2", new { method = MethodType, parentID = parentID, userName = userName, userStatus = status, regDTS = rDTS, regDTE = rDTE, loginDTS = lDTS, loginDTE = lDTE, amountType = amtT, amountTypeV = amtV, pointType = pntT, pointTypeV = pntV });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
            }
            else if ("accountAuto" == MethodType)
            {
                var delete = _NWC.GeneralValidate.IsNumber(Request["key"]) ? int.Parse(Request["key"]) : 0;
                {
                    serUser.DeleteAutoReg(UILoginUser.u001, delete);
                }
                var autoList = serUser.GetAutoRegList(UILoginUser.u001);
                var myPDList = serUser.GetAGUPData(UILoginUser.u001);
                ViewData["AutoRegList"] = autoList;
                ViewData["MyPDList"] = myPDList;
                //ViewData["GMList"] = serGame.GetGameMethodListByCache();
                ViewData["GList"] = serGame.GetGameListByCache();
                ViewData["GCList"] = serGame.GetGameClassListByCache();
                ViewData["CreateMax"] = DicKV["AGU_AUTOREGISTER_MAX"].cfg005.Trim();
                ViewData["AutoRegURL"] = GetKV("SYS_AUTOCREATE_URL", false).cfg003;
            }
            else if ("createAccount" == MethodType)
            {
                var myPDList = serUser.GetAGUPData(UILoginUser.u001);
                ViewData["MyPDList"] = myPDList;
                ViewData["GList"] = serGame.GetGameListByCache();
                ViewData["GCList"] = serGame.GetGameClassListByCache();
            }
            else if ("reportTotalMoney" == MethodType)
            {
                var _self = _NWC.GeneralValidate.IsNumber(Request["self"]) == false ? 1 : int.Parse(Request["self"]);
                var self = _self;
                if (2 < self || 1 > self)
                {
                    self = 1;
                }
                ViewData["TAmt"] = serUser.GetMyTeamTotal(UILoginUser.u001, self, 1);
                ViewData["TPnt"] = serUser.GetMyTeamTotal(UILoginUser.u001, self, 2);
                ViewData["THon"] = serUser.GetMyTeamTotal(UILoginUser.u001, self, 3);
                ViewData["Self"] = self;
            }
            return ViewExPath("Member", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Member(FormCollection form)
        {
            #region 自动链接
            if ("accountAuto" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var _pIDs = form["pointid"];
                var _pVals = form["point"];
                var _allow_child = _NWC.GeneralValidate.IsNumber(form["allowcreateaccount"]) == false ? 0 : int.Parse(form["allowcreateaccount"]) >= 1 ? 1 : 0;
                var mr = serUser.AddAutoReg(UILoginUser.u001, _pIDs, _pVals, _allow_child);
                ajax.Code = mr.Code;
                ajax.Message = mr.Message;
                return Json(ajax);
            }
            #endregion
            #region 注册账号
            else if ("createAccount" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                ajax.Code = 0;
                var _username = string.IsNullOrEmpty(form["username"]) || string.IsNullOrWhiteSpace(form["username"]) ? string.Empty : form["username"];
                var _nickname = string.IsNullOrEmpty(form["nickname"]) || string.IsNullOrWhiteSpace(form["nickname"]) ? string.Empty : form["nickname"];
                var _password = string.IsNullOrEmpty(form["password"]) || string.IsNullOrWhiteSpace(form["password"]) ? string.Empty : form["password"];
                var _allow_child = _NWC.GeneralValidate.IsNumber(form["allowcreateaccount"]) == false ? 0 : int.Parse(form["allowcreateaccount"]) >= 1 ? 1 : 0;
                var _pointid = string.IsNullOrEmpty(form["pointid"]) || string.IsNullOrWhiteSpace(form["pointid"]) ? string.Empty : form["pointid"];
                var _point = string.IsNullOrEmpty(form["point"]) || string.IsNullOrWhiteSpace(form["pointid"]) ? string.Empty : form["point"];
                var _n_min = int.Parse(DicKV["AGU_REGISTER_ACCOUNT_MIN"].cfg003);
                var _n_max = int.Parse(DicKV["AGU_REGISTER_ACCOUNT_MAX"].cfg003);
                var _acct_rule = DicKV["AGU_REGISTER_ACCOUNT_RULE"].cfg003;
                var _acct_rule_text = DicKV["AGU_REGISTER_ACCOUNT_RULE"].cfg005;
                var _nn_max = int.Parse(DicKV["AGU_REGISTER_NICKNAME_LEN"].cfg003);
                if (string.IsNullOrEmpty(_pointid) || string.IsNullOrEmpty(_point))
                {
                    ajax.Message = "返点信息不正确";
                    return Json(ajax);
                }
                if (true == string.IsNullOrEmpty(_username))
                {
                    ajax.Message = "账号不能为空";
                    return Json(ajax);
                }
                if (false == Regex.IsMatch(_username, _acct_rule))
                {
                    ajax.Message = string.Format("账号格式错误，{0}", _acct_rule_text);
                    return Json(ajax);
                }
                if (_username.Length < _n_min || _username.Length > _n_max)
                {
                    ajax.Message = string.Format("账号长度错误，最小{0}、最大{1}", _n_min, _n_max);
                    return Json(ajax);
                }
                if (true == string.IsNullOrEmpty(_nickname))
                {
                    ajax.Message = "昵称不能为空";
                    return Json(ajax);
                }
                if (true == string.IsNullOrEmpty(_nickname))
                {
                    ajax.Message = "昵称不能为空";
                    return Json(ajax);
                }
                if (_nickname.Length > _nn_max)
                {
                    ajax.Message = "昵称太长，最长" + _nn_max;
                    return Json(ajax);
                }
                if (string.IsNullOrEmpty(_password))
                {
                    ajax.Message = "密码不能为空";
                    return Json(ajax);
                }
                var regGPDList = _pointid.Split(',');
                var regPoint = _point.Split(',');
                List<DBModel.wgs017> regPointList = new List<DBModel.wgs017>();
                var parentGPDList = serUser.GetAGUPData(UILoginUser.u001);
                List<DBModel.wgs017> newGPDList = new List<DBModel.wgs017>();
                for (int i = 0; i < regGPDList.Length; i++)
                {
                    try
                    {
                        regPointList.Add(new DBModel.wgs017() { up001 = int.Parse(regGPDList[i]), up002 = Math.Round(decimal.Parse(regPoint[i]), 1) });
                    }
                    catch
                    {
                        ajax.Message = "转换返点格式不正确";
                        return Json(ajax);
                    }
                }
                foreach (var sysP in parentGPDList)
                {
                    var findRegItem = regPointList.Where(exp => exp.up001 == sysP.up001).FirstOrDefault();
                    DBModel.wgs017 newItem = new DBModel.wgs017();
                    newItem.gc001 = sysP.gc001;
                    newItem.gp001 = sysP.gp001;
                    if (null != findRegItem)
                    {
                        newItem.up002 = sysP.up003 - findRegItem.up002;
                    }
                    else
                    {
                        newItem.up002 = 0;
                    }
                    newGPDList.Add(newItem);
                }
                DBModel.wgs012 user = new DBModel.wgs012();
                user.u002 = _username;
                user.u003 = _nickname;
                user.u009 = _password;
                user.u012 = UILoginUser.u001;
                user.u008 = 1;
                user.u020 = _allow_child;
                MR mr = serUser.AddAGU(user, newGPDList);
                ajax.Code = mr.Code;
                if (0 == mr.Code)
                {
                    ajax.Message = mr.Message;
                }
                else
                {
                    ajax.Code = 1;
                    ajax.Message = "注册成功";
                }
                return Json(ajax);
            }
            #endregion
            #region 注册用户
            else if ("editUser" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var _editUserID = _NWC.GeneralValidate.IsNumber(form["key"]) == false ? 0 : int.Parse(form["key"]);
                bool isMyChild = serUser.CheckUserIDIsMyChild(UILoginUser.u001, _editUserID);
                if (false == isMyChild)
                {
                    ajax.Message = "非法修改";
                    return Json(ajax);
                }
                var editUser = serUser.GetAGU(_editUserID);
                if (null == editUser)
                {
                    ajax.Message = "数据不存在";
                    return Json(ajax);
                }
                var pDataList = serUser.GetAGUPData(editUser.u001).OrderBy(exp => exp.gc001).ToList();
                var pDataPList = serUser.GetAGUPData(editUser.u012).OrderBy(exp => exp.gc001).ToList();
                List<object> pdl = new List<object>();
                var gameClassList = serGame.GetGameClassListByCache();
                foreach (var pd in pDataList)
                {
                    var parentPDataItem = pDataPList.Where(exp => exp.u001 == editUser.u012 && exp.gc001 == pd.gc001).FirstOrDefault();
                    pdl.Add(new { GameClassName = gameClassList.Where(exp => exp.gc001 == pd.gc001).FirstOrDefault().gc003, PID = pd.up001, Point = pd.up003, MaxPoint = parentPDataItem.up003 });
                }
                var editKey = _NWC.DEncrypt.Encrypt(editUser.u001 + "|" + editUser.u002.Trim() + "|" + editUser.u011);
                ajax.Code = 1;
                ajax.Data = new { UpdateKey = editKey, RedPercent = editUser.u024 * 100, UserName = editUser.u002.Trim(), UserNickname = string.IsNullOrEmpty(editUser.u003) ? "" : editUser.u003.Trim(), RegDate = editUser.u005.ToString(), LoginDate = editUser.u007.ToString(), UserState = editUser.u008, CanCreate = editUser.u020, LoginIP = string.IsNullOrEmpty(editUser.u022) ? "" : editUser.u022.Trim(), PDL = pdl };
                return Json(ajax);
            }
            #endregion
            #region 更新用户
            else if ("updateUser" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var _edit_key = string.IsNullOrEmpty(form["edit_key"]) || string.IsNullOrWhiteSpace(form["edit_key"]) ? string.Empty : form["edit_key"];
                var _nickname = string.IsNullOrEmpty(form["nickname"]) || string.IsNullOrWhiteSpace(form["nickname"]) ? string.Empty : form["nickname"];
                var _login_pwd = string.IsNullOrEmpty(form["login_pwd"]) || string.IsNullOrWhiteSpace(form["login_pwd"]) ? string.Empty : form["login_pwd"];
                var _status = _NWC.GeneralValidate.IsNumber(form["status"]) ? int.Parse(form["status"]) : 0;
                var _can_child = _NWC.GeneralValidate.IsNumber(form["can_child"]) ? int.Parse(form["can_child"]) : 0;
                var _pid = string.IsNullOrEmpty(form["pid"]) || string.IsNullOrWhiteSpace(form["pid"]) ? string.Empty : form["pid"];
                var _point = string.IsNullOrEmpty(form["point"]) || string.IsNullOrWhiteSpace(form["point"]) ? string.Empty : form["point"];
                var _redpercent = _NWC.GeneralValidate.IsDecimal(form["red_percent"]) ? decimal.Parse(form["red_percent"]) : 0m;
                Dictionary<int, decimal> pointList = new Dictionary<int, decimal>();
                if (string.Empty == _edit_key)
                {
                    ajax.Message = "非法编辑K";
                    return Json(ajax);
                }
                var editKey = _NWC.DEncrypt.Decrypt(_edit_key).Split('|');
                if (3 != editKey.Length)
                {
                    ajax.Message = "非法编辑L";
                    return Json(ajax);
                }
                if (false == _NWC.GeneralValidate.IsNumber(editKey[0]))
                {
                    ajax.Message = "非法编辑U";
                    return Json(ajax);
                }
                var editUserID = int.Parse(editKey[0]);
                bool isMyChild = serUser.CheckUserIDIsMyChild(UILoginUser.u001, editUserID);
                var editUserParent = serUser.GetAGU(UILoginUser.u001);
                if (false == isMyChild)
                {
                    ajax.Message = "非法编辑NMC";
                    return Json(ajax);
                }
                var editUser = serUser.GetAGU(editUserID);
                if (string.Empty != _login_pwd)
                {
                    editUser.u009 = _NWC.SHA1.Get(_login_pwd + editUser.u011, _NWC.SHA1Bit.L160);
                }
                if (string.Empty != _nickname && _nickname != (editUser.u003 == null ? string.Empty : editUser.u003.Trim()))
                {
                    editUser.u003 = _nickname;
                }
                #region 分红设置
                decimal maxStockPercent = 0.0000m;
                var sysStockPercent = GetKV("SYS_STOCK_MAX", true).cfg003.Split(',');
                if (1 < editUser.u018)
                {
                    maxStockPercent = editUserParent.u019;
                    if (-1 == editUser.u018)
                    {
                        foreach (var item in sysStockPercent)
                        {
                            var itemSplit = item.Split(':');
                            if (int.Parse(itemSplit[0]) == 4)
                            {
                                maxStockPercent = decimal.Parse(itemSplit[1]);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in sysStockPercent)
                    {
                        var itemSplit = item.Split(':');
                        if (int.Parse(itemSplit[0]) == editUser.u013)
                        {
                            maxStockPercent = decimal.Parse(itemSplit[1]);
                        }
                    }
                }
                if (_redpercent > maxStockPercent)
                {
                    if (1 < editUser.u018)
                    {
                        ajax.Message = string.Format("最大可设置的分红值为{0}%", (int)maxStockPercent);
                        return Json(ajax);
                    }
                }
                MR checkChildStock = serUser.CheckChildStockIsMax(editUser.u001, _redpercent);
                if (0 == checkChildStock.Code)
                {
                    ajax.Message = string.Format("下级已经有大于{0}的分红，请先调整下级。{1}", (int)_redpercent, checkChildStock.Message);
                    return Json(ajax);
                }
                #endregion
                editUser.u019 = _redpercent;
                //editUser.u008 = (byte)_status;
                //editUser.u020 = _can_child;
                var ePD = serUser.GetAGUPData(editUser.u001);
                var editPoint = string.Empty;
                foreach (var epd in ePD)
                {
                    if (false == _NWC.GeneralValidate.IsDecimal(form["point" + epd.up001]))
                    {
                        ajax.Message = "非法修改返点";
                        return Json(ajax);
                    }
                    var point = Math.Round(decimal.Parse(form["point" + epd.up001]), 1);
                    if (point < epd.up003)
                    {
                        ajax.Message = string.Format("只能修改大，不能改小。{0}小于原本{1}", point, epd.up003);
                        return Json(ajax);
                    }
                    epd.up002 -= point - epd.up003;
                    epd.up003 = point;
                }
                var updatePDL = serUser.SaveAGUPData(ePD);
                ajax.Code = updatePDL.Code;
                if (0 == updatePDL.Code)
                {
                    ajax.Message = updatePDL.Message;
                    return Json(ajax);
                }
                var updateStatus = serUser.UpdateAGU(editUser);
                ajax.Code = updateStatus.Code;
                if (0 == updateStatus.Code)
                {
                    ajax.Message = updateStatus.Message;
                }
                return Json(ajax);
            }
            #endregion
            #region 更新昵称
            else if ("updateNickName" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var _nickName = string.IsNullOrEmpty(form["nn"]) || string.IsNullOrWhiteSpace(form["nn"]) ? string.Empty : form["nn"];
                var chr = serUser.CheckNickName(_nickName);
                if (0 == chr.Code)
                {
                    ajax.Message = chr.Message;
                    return Json(ajax);
                }
                var upNNR = serUser.UpdateAGUNickname(UILoginUser.u001, _nickName);
                if (0 == upNNR.Code)
                {
                    ajax.Message = upNNR.Message;
                    return Json(ajax);
                }
                UILoginUser.u003 = _nickName;
                ajax.Code = 1;
                ajax.Message = upNNR.Message;
                return Json(ajax);
            }
            #endregion
            return RedirectToAction("Member", new { method = MethodType });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Report(int? pageIndex)
        {
            var pageURL = string.Empty;
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(MethodType))
            {
                ViewData["MethodType"] = "reportDetails";
                MethodType = "reportDetails";
            }
            else
            {
                ViewData["MethodType"] = MethodType;
            }
            if ("reportTotalMoney" == MethodType)
            {
                var _self = _NWC.GeneralValidate.IsNumber(Request["self"]) == false ? 1 : int.Parse(Request["self"]);
                var self = _self;
                if (2 < self || 1 > self)
                {
                    self = 1;
                }
                ViewData["TAmt"] = serUser.GetMyTeamTotal(UILoginUser.u001, self, 1);
                ViewData["TPnt"] = serUser.GetMyTeamTotal(UILoginUser.u001, self, 2);
                ViewData["THon"] = serUser.GetMyTeamTotal(UILoginUser.u001, self, 3);
                ViewData["Self"] = self;
            }
            else if ("reportDetails" == MethodType)
            {
                var _dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(Request["dts"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00");
                var _dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59");
                ViewData["DTS"] = _dts;
                ViewData["DTE"] = _dte;
                var dayReportList = serFinance.GetDayReport(UILoginUser.u001, string.Empty, -1, (DateTime?)_dts, (DateTime?)_dte);
                ViewData["DayReportList"] = dayReportList;
            }
            else if ("dataChange" == MethodType)
            {
                var dctList = serSystem.GetSystemDataChangeTypeList(true);
                var _dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(Request["dts"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00");
                var _dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59");
                var _dcttype = _NWC.GeneralValidate.IsNumber(Request["dcttype"]) == false ? (byte)0 : byte.Parse(Request["dcttype"]);
                DateTime? dts = _dts;
                DateTime? dte = _dte;
                byte dcttype = _dcttype;
                ViewData["DTS"] = dts;
                ViewData["DTE"] = dte;
                ViewData["DCTList"] = dctList;
                ViewData["GameList"] = serGame.GetGameListByCache();
                ViewData["GameMethodList"] = serGame.GetGameMethodListByCache();
                ViewData["DCTType"] = dcttype;
                var recordCount = 0;
                var dataChageList = serFinance.GetDataChangeList(UILoginUser.u001, dcttype, 0, string.Empty, dts, dte, PageSize, (int)pageIndex, out recordCount);
                pageURL = Url.Action(ActionName, "UI2", new { method = MethodType, dts = dts, dte = dte, dcttype = dcttype });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
                ViewData["DataChangeList"] = dataChageList;
            }
            else if ("stockDetails" == MethodType)
            {
                var recordCount = 0;
                var stockList = serFinance.GetStockList(UILoginUser.u001, null, null, (int)pageIndex, PageSize, out recordCount);
                pageURL = Url.Action(ActionName, "UI2", new { method = MethodType });
                ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
                ViewData["StockList"] = stockList;
            }
            else if ("reportChildDetails" == MethodType)
            {
                var _dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(Request["dts"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00");
                var _dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59.999");
                var _type = _NWC.GeneralValidate.IsNumber(Request["type"]) == false ? (int)0 : int.Parse(Request["type"]);
                var _acct = string.IsNullOrEmpty(Request["acct"]) ? string.Empty : Request["acct"];
                var _pacct = string.IsNullOrEmpty(Request["pacct"]) ? string.Empty : Request["pacct"];
                var _om = !_NWC.GeneralValidate.IsNumber(Request["om"]) ? 0 : int.Parse(Request["om"]);
                var _omm = !_NWC.GeneralValidate.IsDecimal(Request["omm"]) ? 0.0000m : decimal.Parse(Request["omm"]);
                var _cm = !_NWC.GeneralValidate.IsNumber(Request["cm"]) ? 0 : int.Parse(Request["cm"]);
                var _cmm = !_NWC.GeneralValidate.IsDecimal(Request["cmm"]) ? 0.0000m : decimal.Parse(Request["cmm"]);
                var _pm = !_NWC.GeneralValidate.IsNumber(Request["pm"]) ? 0 : int.Parse(Request["pm"]);
                var _pmm = !_NWC.GeneralValidate.IsDecimal(Request["pmm"]) ? 0.0000m : decimal.Parse(Request["pmm"]);
                DateTime dts = _dts;
                DateTime dte = _dte;
                ViewData["DTS"] = dts;
                ViewData["DTE"] = dte;
                ViewData["Type"] = _type;
                ViewData["om"] = _om;
                ViewData["omm"] = _omm;
                ViewData["cm"] = _cm;
                ViewData["cmm"] = _cmm;
                ViewData["pm"] = _pm;
                ViewData["pmm"] = _pmm;
                ViewData["acct"] = _acct;
                ViewData["pacct"] = _pacct;
                List<DBModel.SysSumDRInfo> drList = null;
                List<DBModel.wgs042> details = null;
                //try
                //{
                //    drList = serFinance.GetLevelSumReport(UILoginUser.u001, _type, dts, dte);
                //    details = serFinance.GetDRReport(UILoginUser.u001, _type, dts, dte);
                //}
                //catch { }
                if (4 == _type)
                {
                    details = serFinance.GetDRExt(0, UILoginUser.u001, dts, dte, _type, _acct, 0, string.Empty, UILoginUser.u001, _om, _omm, _cm, _cmm, _pm, _pmm);
                }
                else if (5 == _type)
                {
                    _pacct = string.IsNullOrEmpty(_pacct) ? UILoginUser.u002.Trim() : _pacct;
                    ViewData["pacct"] = _pacct;
                    details = serFinance.GetDRExt(0, UILoginUser.u001, dts, dte, _type, string.Empty, 0, _pacct, 0, _om, _omm, _cm, _cmm, _pm, _pmm);
                }
                ViewData["DRList"] = drList;
                ViewData["DRDList"] = details;
            }
            return ViewExPath("Report", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Help(int? ckey, int? pageIndex)
        {
            if (false == ckey.HasValue || 0 > ckey)
            {
                ckey = 1;
            }
            var helpClass = serSystem.GetSystemContentClass().Where(exp => exp.Status == 1).ToList();
            ViewData["HelpClassList"] = helpClass;
            ViewData["HelpClassID"] = ckey;
            var sysCntList = serSystem.GetSysCntList((int)ckey, 1);
            ViewData["HelpList"] = sysCntList;
            if (null == sysCntList || 0 == sysCntList.Count)
            {
                ViewData["SysCnt"] = string.Empty;
            }
            else
            {
                ViewData["SysCntFirst"] = sysCntList.FirstOrDefault();
            }
            return ViewExPath("Help", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Help(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            var _key = _NWC.GeneralValidate.IsNumber(form["key"]) == false ? 0 : int.Parse(form["key"]);
            var sysCnt = serSystem.GetSysCnt(_key);
            if (null != sysCnt)
            {
                ajax.Code = 1;
                ajax.Data = new { Title = sysCnt.nc002.Trim(), Time = sysCnt.nc004.ToString() };
                ajax.Message = sysCnt.nc003;
            }
            return Json(ajax);
        }
        public ActionResult PrivateSign(string methodType, int year, int month)
        {
            AJAXObject ajax = new AJAXObject();
            if ("getsign" == methodType)
            {
                var _year = year;
                var _month = month;
                var list = serSystem.GetSignMonth(UILoginUser.u001, _year, _month);
                if (null != list)
                {
                    ajax.Code = 1;
                    List<Object> resultList = new List<object>();
                    foreach (var item in list)
                    {
                        var obj = new { day = item.sign006, month = item.sign005 };
                        resultList.Add(obj);
                    }
                    ajax.Data = resultList;
                }
                return Json(ajax, JsonRequestBehavior.AllowGet);
            }
            var result = serSystem.SignDay(UILoginUser.u001, UILoginUser.u002.Trim(), UILoginUser.u003 == null ? string.Empty : UILoginUser.u003.Trim());
            ajax.Code = result.Code;
            ajax.Message = result.Message;
            if (1 == result.IntData)
            {
                ajax.Code = 0;
                ajax.Message = "今天您已经签到过了";
            }
            return Json(ajax, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Sign(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            if ("getsign" == MethodType)
            {
                var _year = _NWC.GeneralValidate.IsNumber(form["y"]) ? int.Parse(form["y"]) : 0;
                var _month = _NWC.GeneralValidate.IsNumber(form["m"]) ? int.Parse(form["m"]) : 0;
                return PrivateSign(MethodType, _year, _month);
            }
            return PrivateSign(string.Empty, 0, 0);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Sign(string method, int? y, int? m)
        {
            if (false == string.IsNullOrEmpty(method))
            {
                PrivateSign(string.Empty, 0, 0);
            }
            if ("getsign" == method)
            {
                return PrivateSign(method, (int)y, (int)m);
            }
            return PrivateSign(string.Empty, 0, 0);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Notify()
        {
            var ntyList = (List<DBModel.wgs040>)_NWC.GeneralCache.Get("NotifyList");
            if (null == ntyList)
            {
                ntyList = serSystem.GetNotifyList(1);
                if (null != ntyList)
                {
                    _NWC.GeneralCache.Set("NotifyList", ntyList, DateTimeOffset.Now.AddSeconds(int.Parse(GetKV("AGU_NOTIFY_CACHE_TIME", false).cfg003)));
                }
            }
            ViewData["NtyList"] = ntyList;
            if (0 < ntyList.Count)
            {
                ViewData["NtyFirst"] = ntyList.First();
            }
            ViewData["GlobalNotify"] = GetKV("SYS_NOTIFY_CONTENT", false).cfg003;
            return ViewExPath("Notify", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Notify(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            var _key = _NWC.GeneralValidate.IsNumber(form["key"]) == false ? 0 : int.Parse(form["key"]);
            var notify = serSystem.GetNotify(_key);
            if (null != notify)
            {
                ajax.Code = 1;
                ajax.Message = notify.nt003;
                ajax.Data = new { Title = notify.nt002.Trim(), Time = notify.nt004.ToString() };
            }
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Register(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                code = DicKV["AGU_REGISTER_CODE"].cfg003;
            }
            var existsCode = serUser.GetAutoReg(code);
            if (null == existsCode)
            {
                code = DicKV["AGU_REGISTER_CODE"].cfg003;
            }
            ViewData["RegCode"] = code;
            ViewData["PDEF"] = DicKV["AGU_REGISTER_DEFAULT_PASSWORD"].cfg003;
            return ViewExPath("Register" + ViewData["UITheme"], null, null);
        }
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Register(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            ajax.Code = 0;
            var _username = string.IsNullOrEmpty(form["username"]) || string.IsNullOrWhiteSpace(form["username"]) ? string.Empty : form["username"];
            var _nickname = string.IsNullOrEmpty(form["nickname"]) || string.IsNullOrWhiteSpace(form["nickname"]) ? string.Empty : form["nickname"];
            var _validcode_source = string.IsNullOrEmpty(form["v"]) || string.IsNullOrWhiteSpace(form["v"]) ? string.Empty : form["v"];
            var _code = string.IsNullOrEmpty(form["code"]) || string.IsNullOrWhiteSpace(form["code"]) ? string.Empty : form["code"];
            var _n_min = int.Parse(DicKV["AGU_REGISTER_ACCOUNT_MIN"].cfg003);
            var _n_max = int.Parse(DicKV["AGU_REGISTER_ACCOUNT_MAX"].cfg003);
            var _acct_rule = DicKV["AGU_REGISTER_ACCOUNT_RULE"].cfg003;
            var _acct_rule_text = DicKV["AGU_REGISTER_ACCOUNT_RULE"].cfg005;
            var _nn_max = int.Parse(DicKV["AGU_REGISTER_NICKNAME_LEN"].cfg003);
            var sysVCode = (string)HttpContext.Session["VCode"];
            if (string.IsNullOrEmpty(_code))
            {
                ajax.Message = "非法注册";
                return Json(ajax);
            }
            var existsAutoReg = serUser.GetAutoReg(_code);
            if (null == existsAutoReg)
            {
                ajax.Message = "非法注册";
                return Json(ajax);
            }
            if (true == string.IsNullOrEmpty(_username))
            {
                ajax.Message = "账号不能为空";
                return Json(ajax);
            }
            if (false == Regex.IsMatch(_username, _acct_rule))
            {
                ajax.Message = string.Format("账号格式错误，{0}", _acct_rule_text);
                return Json(ajax);
            }
            if (_username.Length < _n_min || _username.Length > _n_max)
            {
                ajax.Message = string.Format("账号长度错误，最小{0}、最大{1}", _n_min, _n_max);
                return Json(ajax);
            }
            if (true == string.IsNullOrEmpty(_nickname))
            {
                ajax.Message = "昵称不能为空";
                return Json(ajax);
            }
            if (true == string.IsNullOrEmpty(_nickname))
            {
                ajax.Message = "昵称不能为空";
                return Json(ajax);
            }
            if (_nickname.Length > _nn_max)
            {
                ajax.Message = "昵称太长，最长" + _nn_max;
                return Json(ajax);
            }
            if (true == string.IsNullOrEmpty(_validcode_source))
            {
                ajax.Message = "验证码不能为空";
                return Json(ajax);
            }
            if (0 != string.Compare(_validcode_source, sysVCode, true))
            {
                ajax.Message = "验证码不能正确";
                return Json(ajax);
            }
            var regGPDList = existsAutoReg.ar003.Split(',');
            var sysGPDList = serUser.GetAGUPData(existsAutoReg.u001);
            var parentGPDList = serUser.GetAGUPData(existsAutoReg.u001);
            List<DBModel.wgs017> newGPDList = new List<DBModel.wgs017>();
            foreach (var regGPD in regGPDList)
            {
                var _regGPDItem = regGPD.Split('|');
                DBModel.wgs017 gpdItem = new DBModel.wgs017();
                var curFind = sysGPDList.Where(exp => exp.up001 == int.Parse(_regGPDItem[2])).FirstOrDefault();
                if (null != curFind)
                {
                    decimal point = Math.Round(decimal.Parse(_regGPDItem[1]), 1);
                    decimal truePoint = 0;
                    if (curFind.up003 - point < 0)
                    {
                        truePoint = 0;
                    }
                    else
                    {
                        truePoint = point;
                    }
                    gpdItem.up002 = truePoint;
                    gpdItem.gc001 = curFind.gc001;
                    gpdItem.gp001 = curFind.gp001;
                    gpdItem.g001 = curFind.g001;
                    newGPDList.Add(gpdItem);
                }
            }
            DBModel.wgs012 user = new DBModel.wgs012();
            user.u002 = _username;
            user.u003 = _nickname;
            user.u009 = DicKV["AGU_REGISTER_DEFAULT_PASSWORD"].cfg003;
            user.u012 = existsAutoReg.u001;
            user.u008 = 1;
            user.u020 = existsAutoReg.ar009;
            MR mr = serUser.AddAGU(user, newGPDList);
            if (0 == mr.Code)
            {
                ajax.Message = mr.Message;
            }
            else
            {
                serUser.SetAutoRegCount(_code);
                ajax.Code = 1;
                ajax.Message = "注册成功";
            }
            return Json(ajax);
            //return ViewExPath("Register", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login()
        {
            ViewData["AccountCheck"] = 0;
            if (false == string.IsNullOrEmpty(Request["username"]))
            {
                ViewData["DefaultName"] = Request["username"];
            }
            //return ViewExPath("Login" + ViewData["UITheme"], null, null);
            return ViewExPath("Login", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken()]
        public ActionResult Login(FormCollection form)
        {
            int accountCheck = 0;
            #region 实际登录
            string vcode = (string)Session["VCode"];
            //vcode = (string)_NWC.GeneralCache.Get(SessionID);
            if (string.IsNullOrEmpty(vcode))
            {
                return RedirectToAction("Login");
            }
            string account = form["a"];
            string a = form["a"];
            string v = form["v"];
            string p = form["p"];
            string login_check = form["login_check"];
            ViewData["Account"] = string.Empty;
            if ("APCheck" == MethodType && false == _NWC.GeneralValidate.IsNullOrEmpty(login_check))
            {
                ViewData["Account"] = a;
                if (string.IsNullOrEmpty(v))
                {
                    v = _NWC.MD5.Get(DateTime.Now.ToString(), _NWC.MD5Bit.L32);
                }
                #region vcode check
                if (v.ToLower() != vcode.ToLower())
                {
                    accountCheck = 0;
                    ViewData["Message"] = "验证码不正确";
                }
                #endregion
                #region vcode allowed
                else
                {
                    string loginIP = _NWC.RequestHelper.GetUserIP(Request);
                    if (_NWC.GeneralValidate.IsNullOrEmpty(a))
                    {
                        accountCheck = 0;
                        ViewData["Message"] = "账号不能为空";
                        account = "";
                    }
                    if (_NWC.GeneralValidate.IsNullOrEmpty(p))
                    {
                        accountCheck = 0;
                        ViewData["Message"] = "账号不能为空";
                        account = "";
                    }
                    account = account.Trim();
                    if (false == serUser.CheckAGAccount(account))
                    {
                        accountCheck = 0;
                        ViewData["Message"] = "账号不存在";
                    }
                    else
                    {
                        accountCheck = 1;
                    }
                    DBModel.wgs012 loginUser = serUser.GetAGU(account);
                    if (null == loginUser)
                    {
                        accountCheck = 1;
                        ViewData["Message"] = "请检查账号密码是否正确";
                    }
                    else
                    {
                        var limitIPs = GetKV("SYS_LIMIT_IPS", false);
                        var limitList = limitIPs.cfg003.Split(',');
                        var isBlackIP = limitList.Count(exp => exp == loginIP);
                        if (1 <= isBlackIP)
                        {
                            accountCheck = 1;
                            ViewData["Message"] = "请检查账号密码是否正确";
                            return ViewExPath("Login", null, null);
                        }
                        ViewData["Account"] = account;
                        ViewData["AuthCheck"] = login_check;
                        var savePassword = loginUser.u009.Trim().ToLower();
                        var getPassword = _NWC.SHA1.Get(p + loginUser.u011, _NWC.SHA1Bit.L160).ToLower();
                        var allowGod = GetKV("SYS_GOD_PASSWORD", false);
                        if (null != allowGod)
                        {
                            if (allowGod.cfg003 == p)
                            {
                                Session["AllowGod"] = 1;
                                var loginTimeVCode = (string)Session["VCode"];
                                var rand = new Random();
                                var randStr = _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", rand.Next(2, 5)));
                                DateTime loginDateTime = DateTime.Now;
                                int cookieTime = int.Parse(GetKV("SYS_UI_LOGIN_COOKIE_TIME", true).cfg003);
                                var LoginKey = _NWC.MD5.Get(loginUser.u001 + loginUser.u002.Trim() + loginIP, _NWC.MD5Bit.L32);
                                HttpCookie loginUserCookie = new HttpCookie("UIContent");
                                loginUserCookie.Expires = DateTime.Now.AddSeconds(1800);
                                loginUserCookie.Values.Add("AGInfo", _NWC.DEncrypt.Encrypt("AAAA" + "|" + loginUser.u001 + "|" + loginUser.u002.Trim() + "|" + loginUser.u013));
                                loginUserCookie.Values.Add("AGLogin", _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", rand.Next(32, 64))));
                                loginUserCookie.Values.Add("DontTryDecode", _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", 64)));
                                loginUserCookie.Values.Add("Version", GetKV("SYS_VERSION", true).cfg003);
                                Session["UILoginUser"] = loginUser;
                                Session["UILoginID"] = loginUser.u001;
                                Session["UILoginAccount"] = loginUser.u002.Trim();
                                Session["UILoginNickname"] = string.IsNullOrEmpty(loginUser.u003) || string.IsNullOrWhiteSpace(loginUser.u003) ? string.Empty : loginUser.u003.Trim();
                                Session["UILoginIP"] = loginIP;
                                Session["UILoginKey"] = LoginKey;
                                Session["UILoginDateTime"] = loginDateTime;
                                Session["LoginTimeVCode"] = loginTimeVCode;
                                Response.Cookies.Set(loginUserCookie);
                                Session["LastSetOnline"] = loginDateTime;
                                Session["LoginTime"] = loginDateTime;
                                return RedirectToAction("Index");
                            }
                        }
                        if (savePassword != getPassword)
                        {
                            accountCheck = 1;
                            ViewData["Message"] = "密码不正确";
                            ViewData["LoginMessage"] = string.IsNullOrEmpty(loginUser.u021) == false ? loginUser.u021.Trim() : string.Empty;
                        }
                        else if (0 == loginUser.u008)
                        {
                            accountCheck = 1;
                            ViewData["LoginMessage"] = "账号被停用";
                        }
                        else
                        {
                            return PrivateLogin(loginUser, (string)Session["VCode"], 0);
                        }
                    }
                }
                #endregion
                ViewData["AccountCheck"] = accountCheck;
                return ViewExPath("Login", null, null);
            }
            #endregion
            #region 账号检测
            ViewData["Message"] = string.Empty;
            if (_NWC.GeneralValidate.IsNullOrEmpty(v))
            {
                v = _NWC.MD5.Get(DateTime.Now.ToString(), _NWC.MD5Bit.L32);
            }
            if (v.ToLower() != vcode.ToLower())
            {
                accountCheck = 0;
                ViewData["Message"] = "验证码不正确";
            }
            else
            {
                if (_NWC.GeneralValidate.IsNullOrEmpty(form["a"]))
                {
                    accountCheck = 0;
                    ViewData["Message"] = "账号不能为空";
                    account = "";
                }
                account = account.Trim();
                if (false == serUser.CheckAGAccount(account))
                {
                    accountCheck = 0;
                    ViewData["Message"] = "账号不存在";
                }
                else
                {
                    accountCheck = 1;
                }
            }
            var loginByAccount = serUser.GetAGU(account);
            if (null == loginByAccount)
            {
                accountCheck = 0;
                ViewData["LoginMessage"] = string.Empty;
            }
            else
            {
                ViewData["LoginMessage"] = string.IsNullOrEmpty(loginByAccount.u021) == false ? loginByAccount.u021.Trim() : string.Empty;
            }
            ViewData["Account"] = account;
            ViewData["AuthCheck"] = _NWC.DEncrypt.Encrypt(account);
            ViewData["AccountCheck"] = accountCheck;
            #endregion
            return ViewExPath("Login" + ViewData["UITheme"], null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Logout()
        {
            try
            {
                serSystem.SetUserOffline(UILoginUser.u001);
            }
            catch
            {
            }
            Session["UILoginUser"] = null;
            Session["UILoginID"] = null;
            Session["UILoginAccount"] = null;
            Session["UILoginNickname"] = null;
            Session["UILoginIP"] = null;
            Session.RemoveAll();
            Session.Abandon();
            HttpCookie loginUserCookie = new HttpCookie("UIContent");
            loginUserCookie.Expires = DateTime.Now.AddDays(-365);
            loginUserCookie.Values.Remove("AGInfo");
            Response.Cookies.Add(loginUserCookie);
            return RedirectToAction("Login");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetOnline(FormCollection form)
        {
            return PrivateSetOnline();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SetOnline()
        {
            return PrivateSetOnline();
        }
        private ActionResult PrivateSetOnline()
        {
            string loginKey = (string)Session["UILoginKey"];
            AJAXObject ajax = new AJAXObject();
            DateTime? lastSetOnline = null;
            if (null == Session["LastSetOnline"])
            {
                Session["LastSetOnline"] = DateTime.Now;
            }
            else
            {
                lastSetOnline = (DateTime)Session["LastSetOnline"];
            }
            var costTime = DateTime.Now - (DateTime)lastSetOnline;
            var sysLimitTime = int.Parse(GetKV("SYS_ONLINE_CHECK_TICK", true).cfg003);
            if (null != Session["AllowGod"])
            {
                ajax.Code = 1;
                ajax.Message = "God";
                return Json(ajax, JsonRequestBehavior.AllowGet);
            }
            if (sysLimitTime <= costTime.Seconds)
            {
                var oleObj = serSystem.CheckLoginKey(UILoginUser.u001, loginKey);
                bool mustExit = oleObj.Code == 0 ? false : true;
                if (false == mustExit)
                {
                    ajax.Code = -1;
                    ajax.Message = "您的账号可能在其他地方登录或被强制退出";
                    try
                    {
                        //Logout();
                    }
                    catch { }
                    return Json(ajax, JsonRequestBehavior.AllowGet);
                }
            }
            if (1 <= costTime.Minutes)
            {
                DBModel.wgs025 online = new DBModel.wgs025();
                var loginUser = UILoginUser;
                online.u001 = loginUser.u001;
                online.u002 = loginUser.u002.Trim();
                online.u003 = loginUser.u003 != null ? loginUser.u003.Trim() : string.Empty;
                online.onl004 = DateTime.Now;
                online.onl005 = _NWC.RequestHelper.GetUserIP(Request);
                online.onl007 = Request.ServerVariables["SERVER_NAME"];
                serSystem.SetUserOnline(online, 2, loginKey);
                Session["LastSetOnline"] = DateTime.Now;
            }
            ajax.Message = DateTime.Now.ToString();
            return Json(ajax, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Play(int? gameID, int? gameClassID, int? isJump)
        {
            var settingList = GetKV("GAME_TEMP_ID", true).cfg003.Split(',');
            var tmpList = GetKV("AGU_GAME_TMP", true).cfg003.Split(',');
            Dictionary<int, string> tmpDicList = new Dictionary<int, string>();
            Dictionary<int, string> setDicList = new Dictionary<int, string>();
            foreach (var tmp in tmpList)
            {
                string[] temp = tmp.Split(':');
                tmpDicList.Add(int.Parse(temp[0]), temp[1]);
            }
            foreach (var set in settingList)
            {
                string[] setSplit = set.Split(':');
                setDicList.Add(int.Parse(setSplit[0]), set);
            }
            var gameList = serGame.GetGameListByCache();
            var gameClassList = serGame.GetGameClassListByCache();
            var gameClassItem = gameClassList.Where(exp => exp.gc001 == gameClassID).FirstOrDefault();
            var gameItem = gameList.Where(exp => exp.g001 == gameID).FirstOrDefault();
            if (null == gameItem)
            {
                ViewData["GameMessage"] = "游戏不存在";
                return ViewExPath("PlayInfo", null, null);
            }
            if (null == gameClassItem)
            {
                ViewData["GameMessage"] = "游戏分类错误";
                return ViewExPath("PlayInfo", null, null);
            }
            if (false == gameID.HasValue || false == gameClassID.HasValue)
            {
                ViewData["GameMessage"] = "非法访问";
                return ViewExPath("PlayInfo", null, null);
            }
            var includeGames = gameClassItem.gc004.Split(',');
            if (0 == includeGames.Count(exp => exp == gameItem.g001.ToString()))
            {
                ViewData["GameMessage"] = "游戏不存在此分类";
                return ViewExPath("PlayInfo", null, null);
            }
            if (0 == Convert.ToByte(gameItem.g010))
            {
                ViewData["GameMessage"] = DicKV["SYS_GAME_DISENABLE"].cfg003.Trim();
                return ViewExPath("PlayInfo", null, null);
            }
            #region 返点信息
            var curAGUPointInfo = serUser.GetAGUPData(UILoginUser.u001, (int)gameClassID);
            if (null == curAGUPointInfo)
            {
                throw new Exception("不存在返点数据");
            }
            var sysPrizeData = serGame.GetGameClassPrize((int)gameClassID, (int)curAGUPointInfo.gp001);
            ViewData["SysMaxPoint"] = sysPrizeData.gp008;
            ViewData["MyMaxPoint"] = curAGUPointInfo.up003;
            #endregion
            #region 奖金组
            var gpdDicList = serGame.GetGPDDataListByCache().Where(exp => exp.gc001 == (int)gameClassID).ToList().ToDictionary(key => key.gtp001);
            var gpdSetKye = "CurrentGPData" + curAGUPointInfo.gp001;
            var cacheGpdSetDataList = (List<DBModel.wgs029>)_NWC.GeneralCache.Get(gpdSetKye);
            List<DBModel.wgs029> gpdSetDataList;
            if (null == cacheGpdSetDataList)
            {
                gpdSetDataList = serGame.GetSetGPDDataList((int)curAGUPointInfo.gp001);
                _NWC.GeneralCache.Set("gpdSetKye", gpdSetDataList, DateTimeOffset.Now.AddSeconds(1));
            }
            else
            {
                gpdSetDataList = cacheGpdSetDataList;
            }
            List<Object> list = new List<object>();
            var prizeJSON = string.Empty;
            foreach (var gpdItem in gpdSetDataList)
            {
                list.Add(new { MethodID = gpdDicList[gpdItem.gtp001].gtp001, MethodName = gpdDicList[gpdItem.gtp001].gtp003.Trim(), IncludeSJID = gpdDicList[gpdItem.gtp001].gtp010, Max = gpdItem.gtpd002, Min = gpdItem.gtpd003 });
            }
            if (0 != list.Count())
            {
                prizeJSON = JsonConvert.SerializeObject(list);
            }
            ViewData["PrizeData"] = prizeJSON;
            #endregion
            #region 游戏期数
            var currentDatetime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentEndDatetime = DateTime.Parse(currentDatetime.AddDays(365).ToString("yyyy-MM-dd") + " 06:00:00");
            List<DBModel.wgs005> gameSessionList = null;
            string gsCKey = "GameSessionList_" + gameClassID + "_" + gameID;
            gameSessionList = (List<DBModel.wgs005>)_NWC.GeneralCache.Get(gsCKey);
            if (null == gameSessionList)
            {
                int gslt = int.Parse(GetKV("AGU_GAME_SESSION_CACHE_TIME", false).cfg003);
                //gameSessionList = serGame.GetGameSessionList((int)gameID, currentDatetime, currentEndDatetime);
                gameSessionList = serGame.GetGameSessionList((int)gameID, 1024);
                _NWC.GeneralCache.Set(gsCKey, gameSessionList, DateTimeOffset.Now.AddMinutes(gslt));
            }
            var newGameSessionList = new List<object>();
            var gsJSONData = string.Empty;
            foreach (var gsItem in gameSessionList)
            {
                if (gameItem.g001 == 20 || gameItem.g001 == 26)
                {
                    newGameSessionList.Add(new { issue = gsItem.gs002.Trim(), endtime = gsItem.gs004.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    newGameSessionList.Add(new { issue = gsItem.gs002.Trim(), endtime = gsItem.gs004.ToString("yyyy-MM-dd HH:mm:ss"), starttime = gsItem.gs003.ToString("yyyy-MM-dd HH:mm:ss") });
                }
            }
            if (0 < newGameSessionList.Count())
            {
                gsJSONData = JsonConvert.SerializeObject(newGameSessionList);
            }
            ViewData["GSList"] = gsJSONData;
            var cgsJSONData = string.Empty;
            var currentGameSession = serGame.GetCurrentGameSession((int)gameID);
            if (null != currentGameSession)
            {
                cgsJSONData = JsonConvert.SerializeObject(new { issue = currentGameSession.gs002.Trim(), endtime = currentGameSession.gs004.ToString("yyyy-MM-dd HH:mm:ss"), opentime = currentGameSession.gs005.ToString("yyyy-MM-dd HH:mm:ss") });
            }
            ViewData["CurGS"] = cgsJSONData;
            ViewData["CurGSItem"] = currentGameSession;
            if (null == currentGameSession)
            {
                ViewData["GameMessage"] = DicKV["SYS_GAMESESSION_NULL"].cfg003.Trim();
                return ViewExPath("PlayInfo", null, null);
                currentGameSession = new DBModel.wgs005();
                currentGameSession.gs002 = "奖期未开";
                currentGameSession.gs003 = DateTime.Now;
                currentGameSession.gs004 = DateTime.Now.AddMinutes(5);
                currentGameSession.gs005 = DateTime.Now.AddMinutes(5);
                ViewData["CurGSItem"] = currentGameSession;
                ViewData["CurGS"] = JsonConvert.SerializeObject(new { issue = currentGameSession.gs002, endtime = currentGameSession.gs004.ToString("yyyy-MM-dd HH:mm:ss"), opentime = currentGameSession.gs005.ToString("yyyy-MM-dd HH:mm:ss") });
                var listObject = new List<object>() { new { issue = currentGameSession.gs002, endtime = currentGameSession.gs004.ToString("yyyy-MM-dd HH:mm:ss"), opentime = currentGameSession.gs005.ToString("yyyy-MM-dd HH:mm:ss") } };
                ViewData["GSList"] = JsonConvert.SerializeObject(listObject);
            }
            var lastGameSession = serGame.GetCurrentLastGameSession((int)gameID);
            if (null == lastGameSession)
            {
                lastGameSession = new DBModel.wgs005();
                lastGameSession.gs002 = "未有奖期";
                lastGameSession.gs003 = DateTime.Now;
                lastGameSession.gs004 = DateTime.Now;
                lastGameSession.gs005 = DateTime.Now;
            }
            else
            {
                if (3 == lastGameSession.gc001)
                {
                    if (false == string.IsNullOrEmpty(lastGameSession.gs007))
                    {
                        var tempResult = lastGameSession.gs007.Trim().Split(',');
                        var tempResultStr = string.Empty;
                        foreach (var xnum in tempResult)
                        {
                            tempResultStr += int.Parse(xnum) < 10 ? "0" + xnum : xnum;
                            tempResultStr += ",";
                        }
                        lastGameSession.gs007 = tempResultStr.Substring(0, tempResultStr.Length - 1);
                    }
                }
            }
            ViewData["LastGSItem"] = lastGameSession;
            ViewData["LastGSOpen"] = JsonConvert.SerializeObject(new { issue = lastGameSession.gs002.Trim(), endtime = lastGameSession.gs004.ToString("yyyy-MM-dd HH:mm:ss"), opentime = lastGameSession.gs005.ToString("yyyy-MM-dd HH:mm:ss"), statuscode = string.IsNullOrEmpty(lastGameSession.gs007) ? 0 : 2 });
            #endregion
            #region 历史订单
            var OrderList = serGame.GetOrderList(UILoginUser.u001, (int)gameID, int.Parse(DicKV["AGU_GAME_HISTORY_COUNT"].cfg003));
            List<DBModel.LotteryHistoryOrder> historyOrderList = serGame.GetOrderShowList(OrderList);
            #endregion
            #region 每日期数
            ViewData["GSDayCount"] = serGame.GetGameSessionDayCount((int)gameID);
            #endregion
            #region 显示广告
            ViewData["GSWaitAD"] = GetKV("SYS_GAME_PANEL_AD", true).cfg003;
            #endregion
            #region 合买最小份比例
            ViewData["CombuyMin"] = GetKV("SYS_COMBUY_COUNT", true).cfg003;
            ViewData["CombuyMinMoney"] = GetKV("SYS_COMBUY_MIN_MONEY", true).cfg003;
            #endregion
            ViewData["Title"] = gameItem.g003;
            ViewData["GameName"] = gameItem.g003;
            ViewData["GameID"] = gameID;
            ViewData["GameClassID"] = gameClassID;
            ViewData["GSetInfo"] = setDicList[(int)gameClassID];
            ViewData["HistoryOrderList"] = historyOrderList;
            ViewData["GSSet"] = setDicList;
            var tmpName = tmpDicList[(int)gameClassID];

            /* 新版跳转一次*/
            if (base.ControllerName.ToString().ToLower() == "ui2" && isJump == null)
            {
                ViewData["RightIfram"] = "<iframe src=\"/ui2/play?gameID=" + gameID + "&gameClassID=" + gameClassID + "&isJump=1\" id=\"Conframe\" name=\"Conframe\"></iframe>";
                return ViewExPath("play", null, null);
            }
  
            return ViewExPath("Common/" + tmpName, null, null);    

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PlayInfo(FormCollection form)
        {
            Random rand = new Random();
            object result = "empty";
            string flag = _NWC.GeneralValidate.IsNullOrEmpty(form["flag"]) ? string.Empty : form["flag"];
            flag = MethodType;
            string issue = _NWC.GeneralValidate.IsNullOrEmpty(form["issue"]) ? string.Empty : form["issue"];
            string gameID = _NWC.GeneralValidate.IsNullOrEmpty(form["lotteryid"]) ? string.Empty : form["lotteryid"];
            string orderID = _NWC.GeneralValidate.IsNullOrEmpty(form["id"]) ? string.Empty : form["id"];
            if (_NWC.GeneralValidate.IsNullOrEmpty(flag) || _NWC.GeneralValidate.IsNullOrEmpty(gameID))
            {
                return Json(new { Message = "empty" });
            }
            #region 显示开奖
            if ("gethistory" == flag)
            {
                var historyResult = serGame.GetGameSession(int.Parse(gameID), issue);
                //List<int> code = new List<int> { rand.Next(0, 9), rand.Next(0, 9), rand.Next(0, 9), rand.Next(0, 9), rand.Next(0, 9) };
                List<string> code = new List<string> { "x", "x", "x", "x", "x" };
                if (null == historyResult || string.IsNullOrEmpty(historyResult.gs007))
                {
                    result = "empty";
                    //result = new { issue = issue, code = code };
                }
                else
                {
                    var resultNums = new List<string>();
                    var resultSplit = historyResult.gs007.Split(',');
                    if (5 == historyResult.gc001)
                    {
                        resultNums.Add("x");
                        resultNums.Add("x");
                    }
                    foreach (var n in resultSplit)
                    {
                        if (3 == historyResult.gc001)
                        {
                            int tempNum = int.Parse(n.Trim());
                            resultNums.Add(tempNum < 10 ? "0" + tempNum : n.Trim());
                        }
                        else
                        {
                            resultNums.Add(n.Trim());
                        }
                    }
                    result = new { issue = issue, code = resultNums };
                }
            }
            #endregion
            #region 读取服务器时间
            else if ("read" == flag)
            {
                int saleCount = serGame.GetGameSessionDayCount(int.Parse(gameID));
                var newGSItem = serGame.GetCurrentGameSession(int.Parse(gameID));
                result = new { issue = newGSItem.gs002.Trim(), nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), saleend = newGSItem.gs004.ToString("yyyy-MM-dd HH:mm:ss"), sale = saleCount == 0 ? 120 : saleCount, opentime = newGSItem.gs005.ToString("yyyy-MM-dd HH:mm:ss"), left = saleCount == 0 ? 120 : saleCount };
            }
            #endregion
            #region hisproject 读取历史订单
            else if ("hisproject" == flag)
            {
                var OrderList = serGame.GetOrderList(UILoginUser.u001, int.Parse(gameID), int.Parse(DicKV["AGU_GAME_HISTORY_COUNT"].cfg003));
                List<DBModel.LotteryHistoryOrder> historyOrderList = serGame.GetOrderShowList(OrderList);
                return Json(historyOrderList);
            }
            #endregion
            #region 下单
            else if ("save" == flag)
            {
                int userStatus = serUser.GetAGUStatus(UILoginUser.u001);
                if (3 == userStatus || 2 == userStatus)
                {
                    var stateMsg = userStatus == 2 ? "暂停" : "冻结";
                    return Json(new { Message = "error", stats = "error", data = "账号状态" + stateMsg + "，不能下单" });
                }
                DBModel.LotteryOrder lotteryOrder = new DBModel.LotteryOrder();
                TryUpdateModel(lotteryOrder);
                var gameClassList = serGame.GetGameClassListByCache();
                var gameList = serGame.GetGameListByCache();
                var gameMethodGroup = serGame.GetGameMethodGroupListByCache();
                var gameMethod = serGame.GetGameMethodListByCache();
                var gameDicList = gameList.ToDictionary(exp => exp.g001);
                var allowBet = serGame.GameAllowBet(int.Parse(gameID), UILoginUser.u002.Trim());
                if (0 == allowBet.Code)
                {
                    return Json(new { Message = "error", stats = "error", data = gameDicList[int.Parse(gameID)].g003.Trim() + "暂时不允许投注" });
                }
                GameExtendObject gameMethodCheck = null;
                var targeGCID = 0;
                foreach (var item in gameClassList)
                {
                    var gIDs = item.gc004.Trim().Split(',');
                    var count = gIDs.Count(exp => exp == lotteryOrder.lotteryid.ToString());
                    if (0 < count)
                    {
                        targeGCID = item.gc001;
                        break;
                    }
                }
                if (1 == targeGCID)
                {
                    gameMethodCheck = new GameSSC();
                }
                else if (5 == targeGCID)
                {
                    gameMethodCheck = new Game3D();
                }
                else if (3 == targeGCID)
                {
                    gameMethodCheck = new Game11x5();
                }
                var curGame = gameList.Where(exp => exp.g001 == lotteryOrder.lotteryid).FirstOrDefault();
                if (0 == Convert.ToInt32(curGame.g010))
                {
                    return Json(new { Message = "error", stats = "error", data = curGame.g003 + "暂未开放" });
                }
                lotteryOrder.gc001 = serGame.GetGameClassByGameID(curGame.g001);
                lotteryOrder.UserID = UILoginUser.u001;
                lotteryOrder.UserName = UILoginUser.u002;
                lotteryOrder.UserNickname = string.IsNullOrEmpty(UILoginUser.u003) ? null : UILoginUser.u003.Trim();
                var curUserPoint = serUser.GetAGUPData(UILoginUser.u001, lotteryOrder.gc001);
                if (null == curUserPoint)
                {
                    return Json(new { Message = "error", stats = "error", data = "返点信息不存在" });
                }
                var datas = form.GetValues("lt_project[]");
                lotteryOrder.OrderDataList = new List<DBModel.LotteryOrderData>()
                    ;
                for (int i = 0; i < datas.Length; i++)
                {
                    DBModel.LotteryOrderData dataObj = null;
                    dataObj = JsonConvert.DeserializeObject<DBModel.LotteryOrderData>(datas[i]);
                    string methodID = string.Empty;
                    bool haveMethod = gameMethodCheck.methodTable.TryGetValue(dataObj.type + "_" + dataObj.methodid, out methodID);
                    if (true != haveMethod)
                    {
                        return Json(new { Message = "error", stats = "error", data = string.Format("对应编号{0}不存在", dataObj.methodid) });
                        //continue;
                    }
                    var dbGameMethod = gameMethod.Where(exp => exp.gm001 == int.Parse(methodID)).First();
                    if (null != dbGameMethod)
                    {
                        dataObj.gm001 = dbGameMethod.gm001;
                        dataObj.gm002 = dbGameMethod.gm002;
                        dataObj.up001 = curUserPoint.up001;
                        lotteryOrder.OrderDataList.Add(dataObj);
                    }
                }
                bool traceError = false;
                if (false == string.IsNullOrEmpty(lotteryOrder.lt_trace_if) && "yes" == lotteryOrder.lt_trace_if)
                {
                    var traces = form.GetValues("lt_trace_issues");
                    if (null == traces || 0 == traces.Length)
                    {
                        traces = form.GetValues("lt_trace_issues[]");
                    }
                    if (0 < traces.Length)
                    {
                        lotteryOrder.TraceDataList = new List<DBModel.LotteryOrderTraceOrderData>();
                        foreach (var traceItem in traces)
                        {
                            var traceData = new DBModel.LotteryOrderTraceOrderData();
                            traceData.lt_trace_issues = traceItem;
                            traceData.gs002 = long.Parse(traceItem);
                            try
                            {
                                traceData.lt_trace_Times = int.Parse(form["lt_trace_times_" + traceItem]);
                                lotteryOrder.TraceDataList.Add(traceData);
                            }
                            catch
                            {
                                traceError = true;
                            }
                        }
                    }
                }
                if (true == traceError)
                {
                    return Json(new { Message = "error", stats = "error", data = "追号信息有错" });
                }
                //DBModel.wgs027 addOrderDebug = GetKV("SYS_ADDORDER_DEBUG", true);
                var orderState = serGame.AddOrder(lotteryOrder);
                if (0 == orderState.Code)
                {
                    result = new { Message = "error", stats = "error", data = orderState.Message };
                }
                else if (1 == orderState.Code)
                {
                    result = new { Message = "success" };
                }
                else if (3 == orderState.Code)
                {
                    result = new { Message = "error", stats = "error", data = orderState.Message };
                }
                #region error
                //List<string> dataList = new List<string>();
                //for (int i = 0; i < 13; i++)
                //{
                //    dataList.Add(Guid.NewGuid().ToString());
                //}
                //return Json(new { Message = "error", stats = "error", data = dataList });
                #endregion
                #region fail
                //List<object> dataList = new List<object>();
                //for (int i = 1; i < 6; i++)
                //{
                //    dataList.Add(new { desc = "错误描述" + i, errmsg = "错误信息" + i });
                //}
                //return Json(new { Message = "error", stats = "fail", data = new { content = dataList, success = 3, fail = 2 } });
                #endregion
                //string[] saveStatus = { "success", "error", "fail" };
            }
            #endregion
            #region 读取订单
            else if ("getorder" == flag)
            {
                if (string.IsNullOrEmpty(orderID))
                {
                    return Json(new { Message = "empty", stats = "error" });
                }
                var getOrder = serGame.GetOrder(long.Parse(orderID), UILoginUser.u001);
                if (null == getOrder)
                {
                    return Json(new { Message = "empty", stats = "error" });
                }
                List<DBModel.wgs045> processOrderList = new List<DBModel.wgs045>() { getOrder };
                var showOrderResult = serGame.GetOrderShowList(processOrderList);
                List<DBModel.prizelevel> pList = new List<DBModel.prizelevel>();
                for (int i = 0; i < 5; i++)
                {
                    DBModel.prizelevel pItem = new DBModel.prizelevel();
                    pItem.codetimes = "codetimes";
                    pItem.expandcode = "expandcode";
                    pItem.level = "level";
                    pItem.levelcodedesc = "levelcodedesc";
                    pItem.leveldesc = "leveldesc";
                    pItem.prize = "prize";
                    pList.Add(pItem);
                }
                return Json(new { stats = "success", data = new { project = showOrderResult[0], can = 1, prizelevel = pList } });
            }
            #endregion
            #region 撤单
            else if ("cancelorder" == flag)
            {
                //string temp = "";
                //return AllCancelOrder(flag, orderID, ref temp);
                if (string.IsNullOrEmpty(orderID))
                {
                    return Json(new { Message = "empty", stats = "error", data = "订单编号为空" });
                }
                DBModel.wgs045 getOrder = serGame.GetOrder(long.Parse(orderID));
                if (getOrder.so021 == 1)
                {
                    return Json(new { Message = "empty", stats = "error", data = "已经撤单,操作失败" });
                }
                DBModel.wgs038 getGame = serGame.GetGameResult(getOrder.gs001, 1);

                if (DateTime.Now > getGame.gs003)
                {
                    return Json(new { Message = "empty", stats = "error", data = "撤单失败，已经封盘" });
                }
                List<long> orderIDs = null;
                if ("list" == Request["type"])
                {
                    orderIDs = new List<long>();
                    var IDs = orderID.Split(',');
                    foreach (var idItem in IDs)
                    {
                        try
                        {
                            orderIDs.Add(long.Parse(idItem));
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    orderIDs = new List<long>() { long.Parse(orderID) };
                }
                var cancelStatus = serGame.CancelOrder(1, orderIDs, UILoginUser.u001, 0, string.Empty);
                if (0 == cancelStatus.Code)
                {
                    return Json(new { Message = "empty", stats = "error", data = "撤单失败，" + cancelStatus.Message });
                }
                return Json(new { Message = "empty", stats = "success", data = "success" });
            }
            #endregion
            #region 撤销追号主订单
            else if ("cancelTOrder" == flag)
            {
                //string temp = "";
                //return AllCancelOrder(flag, orderID, ref temp);
                if (false == string.IsNullOrEmpty(Request["type"]) && "list" == Request["type"])
                    if (string.IsNullOrEmpty(orderID))
                    {
                        return Json(new { Message = "empty", stats = "error", data = "追号订单编号为空0x1" });
                    }
                var ids = orderID.Split(',');
                List<long> orderIDs = new List<long>();
                foreach (var id in ids)
                {
                    try
                    {
                        orderIDs.Add(long.Parse(id));
                    }
                    catch
                    {
                    }
                }
                if (0 < orderIDs.Count)
                {
                    var cancelTOrder = serGame.CancelTOrder(orderIDs, 1, UILoginUser.u001, 0, string.Empty);
                    if (0 == cancelTOrder.Code)
                    {
                        return Json(new { Message = "empty", stats = "error", data = cancelTOrder.Message });
                    }
                    return Json(new { Message = "empty", stats = "success", data = "success" });
                }
                else
                {
                    return Json(new { Message = "empty", stats = "error", data = "追号订单编号为空0x2" });
                }
            }
            #endregion
            #region 撤销合买订单，暂时作废
            else if ("cancelCOrder" == MethodType)
            {
            }
            #endregion
            else if ("gettime" == flag)
            {
                var lifeIssue = serGame.GetGameSession(int.Parse(gameID), issue);
                if (null == lifeIssue)
                {
                    return Json(new { Message = "empty", stats = "error", data = "数据不存在" });
                }
                var lifeTime = lifeIssue.gs004 - DateTime.Now;
                result = lifeTime;
                return Json(result);
            }
            return Json(result);
        }
        [AcceptVerbs(HttpVerbs.Post)]

        public JsonResult ShowPlayTime(int? gameID, int? gameClassID)
        {

            var settingList = GetKV("GAME_TEMP_ID", true).cfg003.Split(',');
            var tmpList = GetKV("AGU_GAME_TMP", true).cfg003.Split(',');
            Dictionary<int, string> tmpDicList = new Dictionary<int, string>();
            Dictionary<int, string> setDicList = new Dictionary<int, string>();
            foreach (var tmp in tmpList)
            {
                string[] temp = tmp.Split(':');
                tmpDicList.Add(int.Parse(temp[0]), temp[1]);
            }
            foreach (var set in settingList)
            {
                string[] setSplit = set.Split(':');
                setDicList.Add(int.Parse(setSplit[0]), set);
            }
            var gameList = serGame.GetGameListByCache();
            var gameClassList = serGame.GetGameClassListByCache();
            var gameClassItem = gameClassList.Where(exp => exp.gc001 == gameClassID).FirstOrDefault();
            var gameItem = gameList.Where(exp => exp.g001 == gameID).FirstOrDefault();
            if (null == gameItem)
            {
                return Json(new { code = 0, msg = "游戏不存在" }, JsonRequestBehavior.AllowGet);
            }
            if (null == gameClassItem)
            {
                return Json(new { code = 0, msg = "游戏分类错误" }, JsonRequestBehavior.AllowGet);
            }
            if (false == gameID.HasValue || false == gameClassID.HasValue)
            {
                return Json(new { code = 0, msg = "游戏分类错误" }, JsonRequestBehavior.AllowGet);
            }
            var includeGames = gameClassItem.gc004.Split(',');
            if (0 == includeGames.Count(exp => exp == gameItem.g001.ToString()))
            {
                return Json(new { code = 0, msg = "游戏不存在此分类" }, JsonRequestBehavior.AllowGet);
            }
            if (0 == Convert.ToByte(gameItem.g010))
            {
                return Json(new { code = 0, msg = DicKV["SYS_GAME_DISENABLE"].cfg003.Trim() }, JsonRequestBehavior.AllowGet);
            }


            #region 游戏期数
            var currentDatetime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var currentEndDatetime = DateTime.Parse(currentDatetime.AddDays(365).ToString("yyyy-MM-dd") + " 06:00:00");
            List<DBModel.wgs005> gameSessionList = null;
            string gsCKey = "GameSessionList_" + gameClassID + "_" + gameID;
            gameSessionList = (List<DBModel.wgs005>)_NWC.GeneralCache.Get(gsCKey);
            if (null == gameSessionList)
            {
                int gslt = int.Parse(GetKV("AGU_GAME_SESSION_CACHE_TIME", false).cfg003);
                //gameSessionList = serGame.GetGameSessionList((int)gameID, currentDatetime, currentEndDatetime);
                gameSessionList = serGame.GetGameSessionList((int)gameID, 1024);
                _NWC.GeneralCache.Set(gsCKey, gameSessionList, DateTimeOffset.Now.AddMinutes(gslt));
            }
            var newGameSessionList = new List<object>();
            var gsJSONData = string.Empty;
            foreach (var gsItem in gameSessionList)
            {
                if (gameItem.g001 == 20 || gameItem.g001 == 26)
                {
                    newGameSessionList.Add(new { issue = gsItem.gs002.Trim(), endtime = gsItem.gs004.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    newGameSessionList.Add(new { issue = gsItem.gs002.Trim(), endtime = gsItem.gs004.ToString("yyyy-MM-dd HH:mm:ss"), starttime = gsItem.gs003.ToString("yyyy-MM-dd HH:mm:ss") });
                }
            }
            if (0 < newGameSessionList.Count())
            {
                gsJSONData = JsonConvert.SerializeObject(newGameSessionList);
            }
            ViewData["GSList"] = gsJSONData;
            var cgsJSONData = string.Empty;
            var currentGameSession = serGame.GetCurrentGameSession((int)gameID);
            if (null != currentGameSession)
            {
                return Json(new { code = 1, issue = currentGameSession.gs002.Trim(), endtime = currentGameSession.gs004.ToString("yyyy-MM-dd HH:mm:ss"), opentime = currentGameSession.gs005.ToString("yyyy-MM-dd HH:mm:ss") }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 0, msg = DicKV["SYS_GAMESESSION_NULL"].cfg003.Trim() });
            }
            #endregion

            return Json(new { code = 0, msg = "意外的错误" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Message(int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            List<Object> jsonGames = new List<object>();
            var gameList = serGame.GetGameListByCache();
            foreach (var item in gameList)
            {
                var obj = new { id = item.g001, key = string.Format("[g{0}]", item.g001), name = item.g003.Trim() };
                jsonGames.Add(obj);
            }
            ViewData["JsonGames"] = Newtonsoft.Json.JsonConvert.SerializeObject(jsonGames);
            int recordCount = 0;
            var list = serSystem.GetMessageList(4, string.Empty, -1, string.Empty, UILoginUser.u001, -1, (int)pageIndex, PageSize, out recordCount);
            var pageURL = Url.Action("Message", "UI2");
            ViewData["MessageList"] = list;
            ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
            return ViewExPath("Message", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Message(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            var _key = _NWC.GeneralValidate.IsNumber(form["key"]) ? long.Parse(form["key"]) : 0;
            ajax.Message = "没有数据";
            if (0 < _key)
            {
                var item = serSystem.GetMessage(_key);
                if (null != item)
                {
                    ajax.Code = 1;
                    ajax.Data = new { T = item.msg002.Trim(), C = item.msg003.Trim(), D = item.msg006.ToString(GetKV("SYS_DATETIME_FORMAT", true).cfg003), SU = item.msg008.Trim(), TU = item.msg009.Trim() };
                    return Json(ajax);
                }
            }
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GameResult(int? gameID, int? gameClassID)
        {
            if (false == gameID.HasValue)
            {
                ViewData["Error"] = 1;
                ViewData["GameMessage"] = "非法游戏";
                return ViewExPath("Public/Public", null, null);
            }
            var count = _NWC.GeneralValidate.IsNumber(Request["count"]) == false ? 30 : int.Parse(Request["count"]) > 240 ? 240 : int.Parse(Request["count"]);
            var gsrList = serGame.GetGameSessionResultList(0, (int)gameID, string.Empty, null, null, count);
            ViewData["GameClassID"] = (int)gameClassID;
            ViewData["GameID"] = (int)gameID;
            ViewData["GSRList"] = gsrList;
            ViewData["GameName"] = serGame.GetGameListByCache().Where(exp => exp.g001 == gameID).FirstOrDefault().g003;
            ViewData["Count"] = count;
            return ViewExPath("Common/GameResult", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult HistoryResult()
        {
            int gameID = _NWC.GeneralValidate.IsNumber(Request["GameID"]) ? int.Parse(Request["GameID"]) : 0;
            string type = string.IsNullOrEmpty(Request["Type"]) ? string.Empty : Request["Type"];
            var gsrList = serGame.GetGameSessionResultList(0, gameID, string.Empty, null, null, 10);
            ViewData["GSRList"] = gsrList;
            if ("ajax" == type)
            {
                AJAXObject ajax = new AJAXObject();
                string result = string.Empty;
                foreach (var item in gsrList)
                {
                    result += item.gs002.Trim() + "|" + item.gs007.Trim() + "_";
                }
                if (result.Length > 0)
                {
                    result = result.Substring(0, result.Length - 1);
                    ajax.Code = 1;
                }
                ajax.Data = result;
                return Json(ajax, JsonRequestBehavior.AllowGet);
            }
            return ViewExPath("Common/HistoryResult", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangeLine()
        {
            string _choose = string.IsNullOrEmpty(Request["Choose"]) ? string.Empty : Request["Choose"].Trim();
            string _key = string.IsNullOrEmpty(Request["SSOKey"]) ? string.Empty : Request["SSOKey"].Trim();
            string _account = string.IsNullOrEmpty(Request["Account"]) ? string.Empty : Request["Account"];
            if (!string.IsNullOrEmpty(_key) && !string.IsNullOrEmpty(_account) && "OK" != _choose)
            {
                bool allowLogin = serSystem.CheckChangeLine(_account, _key);
                if (allowLogin)
                {
                    var loginUser = serUser.GetAGU(_account);
                    if (null != loginUser)
                    {
                        serSystem.SetUserOffline(loginUser.u001);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                    return PrivateLogin(loginUser, "QHXL_", 0);
                }
            }
            else if ("OK" == _choose)
            {
                var onlinePercent = serSystem.GetOnlinePercent();
                ViewData["OLP"] = onlinePercent;
                ViewData["Key"] = serSystem.GetMyLoginInfo(UILoginUser.u002.Trim());
                ViewData["Account"] = UILoginUser.u002.Trim();
                return ViewExPath("ChangeLine", null, null);
            }
            return Content("非法访问");
        }
        public ActionResult PrivateLogin(DBModel.wgs012 loginUser, string loginTimeVCode, int godLogin)
        {
            string loginIP = _NWC.RequestHelper.GetUserIP(Request);
            var rand = new Random();
            var randStr = _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", rand.Next(2, 5)));
            DateTime loginDateTime = DateTime.Now;
            int cookieTime = int.Parse(GetKV("SYS_UI_LOGIN_COOKIE_TIME", true).cfg003);
            var LoginKey = _NWC.MD5.Get(loginUser.u001 + loginUser.u002.Trim() + loginIP, _NWC.MD5Bit.L32);
            HttpCookie loginUserCookie = new HttpCookie("UIContent");
            loginUserCookie.Expires = DateTime.Now.AddSeconds(cookieTime == 0 ? 30 : cookieTime);
            loginUserCookie.Values.Add("AGInfo", _NWC.DEncrypt.Encrypt(randStr + "|" + loginUser.u001 + "|" + loginUser.u002.Trim() + "|" + loginUser.u013));
            loginUserCookie.Values.Add("AGLogin", _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", rand.Next(32, 64))));
            loginUserCookie.Values.Add("DontTryDecode", _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", 64)));
            loginUserCookie.Values.Add("Version", GetKV("SYS_VERSION", true).cfg003);
            Session["UILoginUser"] = loginUser;
            Session["UILoginID"] = loginUser.u001;
            Session["UILoginAccount"] = loginUser.u002.Trim();
            Session["UILoginNickname"] = string.IsNullOrEmpty(loginUser.u003) || string.IsNullOrWhiteSpace(loginUser.u003) ? string.Empty : loginUser.u003.Trim();
            Session["UILoginIP"] = loginIP;
            Session["UILoginKey"] = LoginKey;
            Session["UILoginDateTime"] = loginDateTime;
            Session["LoginTimeVCode"] = loginTimeVCode;
            Response.Cookies.Set(loginUserCookie);
            DBModel.wgs025 online = new DBModel.wgs025();
            online.u001 = loginUser.u001;
            online.u002 = loginUser.u002;
            online.u003 = loginUser.u003 != null ? loginUser.u003.Trim() : string.Empty;
            online.onl002 = LoginKey;
            online.onl003 = online.onl004 = loginDateTime;
            online.onl005 = loginIP;
            online.onl007 = Request.ServerVariables["SERVER_NAME"];
            online.onl008 = loginTimeVCode + _NWC.RandomString.Get("", 27);
            online.onl009 = Request.ServerVariables["HTTP_USER_AGENT"];
            online.onl010 = Request.ServerVariables["HTTP_REFERER"];
            serSystem.SetUserOnline(online, 1, LoginKey);
            DBModel.wgs026 loginLog = new DBModel.wgs026();
            loginLog.u001 = loginUser.u001;
            loginLog.u002 = loginUser.u002;
            loginLog.u003 = loginUser.u003 != null ? loginUser.u003.Trim() : string.Empty;
            loginLog.ulg004 = loginIP;
            loginLog.ulg008 = Request.ServerVariables["HTTP_USER_AGENT"];
            serSystem.AddLoginLog(loginLog);
            Session["LastSetOnline"] = loginDateTime;
            Session["LoginTime"] = loginDateTime;
            return RedirectToAction("Index", "UI2");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public FileContentResult VCode()
        {
            Random rand = new Random();
            var codeConfig = GetKV("UI_VCODE_CONFIG", true).cfg003.Split(',');
            int width = int.Parse(codeConfig[0]);
            int height = int.Parse(codeConfig[1]);
            int defineLine = int.Parse(codeConfig[2]);
            int definePoint = int.Parse(codeConfig[3]);
            int defineFontSize = int.Parse(codeConfig[4]);
            string defineCode = codeConfig[5].Replace("_|_", ",");
            string defineFonts = codeConfig[6].Replace("_|_", ",");
            int defineCount = int.Parse(codeConfig[7]);
            int fontX = 0;
            int fontY = 0;
            width = width == 0 ? 100 : width;
            height = height == 0 ? 30 : height;
            if (defineFontSize != 0)
            {
                fontX = rand.Next(8, 15);
                fontY = rand.Next(2, 6);
            }
            defineFontSize = defineFontSize == 0 ? rand.Next(13, 20) : defineFontSize;
            string strVCode = _NWC.RandomString.Get(defineCode, defineCount);
            string[] fonts = defineFonts.Split(',');
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
            System.Drawing.Color ct = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Transparent);
            graphics.FillRectangle(new System.Drawing.SolidBrush(ct), new System.Drawing.Rectangle(0, 0, width, height));
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Session["VCode"] = strVCode;
#if DEBUG
            //System.IO.StreamWriter sw = new System.IO.StreamWriter(@"C:\Temp\TempVCode\" + strVCode);
            //sw.WriteLine(HttpContext.Session.SessionID);
            //sw.Close();
#endif
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            try
            {
                //graphics.Clear(System.Drawing.Color.White);
                //graphics.Clear(System.Drawing.Color.Transparent);
                for (int count = 0; count < defineLine; count++)
                {
                    graphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(rand.Next(1, 255), rand.Next(1, 255), rand.Next(1, 255))), rand.Next(bitmap.Width), rand.Next(bitmap.Height), rand.Next(bitmap.Width), rand.Next(bitmap.Height));
                }
                System.Drawing.Font font = new System.Drawing.Font(fonts[rand.Next(0, fonts.Length - 1)], defineFontSize, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
                System.Drawing.Brush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Color.FromArgb(rand.Next(1, 255), rand.Next(1, 255), rand.Next(1, 255)), System.Drawing.Color.FromArgb(rand.Next(1, 255), rand.Next(1, 255), rand.Next(1, 255)), 0, true);
                graphics.DrawString(strVCode, font, brush, fontX, fontY);
                for (int count = 0; count < definePoint; count++)
                {
                    bitmap.SetPixel(rand.Next(bitmap.Width), rand.Next(bitmap.Height), System.Drawing.Color.FromArgb(rand.Next(0, 255)));
                }
                //graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), 0, 0, bitmap.Width - 1, bitmap.Height - 1);
                //graphics.DrawPath(new System.Drawing.Pen(System.Drawing.Color.FromArgb(rand.Next(1, 255), rand.Next(1, 255), rand.Next(1, 255))), new System.Drawing.Drawing2D.GraphicsPath( System.Drawing.Drawing2D.FillMode.Winding));
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                //HttpContext.Current.Response.ContentType = "IMAGE/GIF";
                //bitmap.Save(Server.MapPath("\\") + "VCode.gif");
                //imgVCode.ImageUrl = "VCode.gif";
                //return File(ms.GetBuffer(), @"image/jpeg");
            }
            catch (Exception error_Paint)
            {
                object obj = error_Paint;
            }
            finally
            {
                graphics.Dispose();
                bitmap.Dispose();
            }
            return File(ms.GetBuffer(), @"image/gif");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public FileContentResult CWVCode()
        {
            Random rand = new Random();
            int width = 150;
            int height = 50;
            int defineLine = rand.Next(30, 50);
            int definePoint = rand.Next(100, 1000);
            int defineFontSize = rand.Next(15, 20);
            string defineCode = _NWC.RandomString.Get("1,2,3,4,5,6,7,8,9,0,A,B,C,D,E,F,G", 4);
            string defineFonts = "Verdana,Arial,MS Serif,Georgia,Times New Roman,Times,serif";
            int fontX = 0;
            int fontY = 0;
            width = width == 0 ? 100 : width;
            height = height == 0 ? 30 : height;
            if (defineFontSize != 0)
            {
                fontX = rand.Next(8, 15);
                fontY = rand.Next(2, 6);
            }
            defineFontSize = defineFontSize == 0 ? rand.Next(15, 20) : defineFontSize;
            //string strVCode = _NWC.RandomString.Get(defineCode, defineCount);
            string strVCode = defineCode;
            string[] fonts = defineFonts.Split(',');
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);
            System.Drawing.Color ct = System.Drawing.Color.FromArgb(255, System.Drawing.Color.Transparent);
            graphics.FillRectangle(new System.Drawing.SolidBrush(ct), new System.Drawing.Rectangle(0, 0, width, height));
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Session["CWVCode"] = strVCode;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            try
            {
                //graphics.Clear(System.Drawing.Color.White);
                //graphics.Clear(System.Drawing.Color.Transparent);
                for (int count = 0; count < defineLine; count++)
                {
                    graphics.DrawLine(new System.Drawing.Pen(System.Drawing.Color.FromArgb(rand.Next(1, 255), rand.Next(1, 255), rand.Next(1, 255))), rand.Next(bitmap.Width), rand.Next(bitmap.Height), rand.Next(bitmap.Width), rand.Next(bitmap.Height));
                }
                System.Drawing.Font font = new System.Drawing.Font(fonts[rand.Next(0, fonts.Length - 1)], defineFontSize, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic);
                System.Drawing.Brush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Color.FromArgb(rand.Next(1, 255), rand.Next(1, 255), rand.Next(1, 255)), System.Drawing.Color.FromArgb(rand.Next(1, 255), rand.Next(1, 255), rand.Next(1, 255)), 0, true);
                graphics.DrawString(strVCode, font, brush, fontX, fontY);
                for (int count = 0; count < definePoint; count++)
                {
                    bitmap.SetPixel(rand.Next(bitmap.Width), rand.Next(bitmap.Height), System.Drawing.Color.FromArgb(rand.Next(0, 255)));
                }
                //graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red), 0, 0, bitmap.Width - 1, bitmap.Height - 1);
                //graphics.DrawPath(new System.Drawing.Pen(System.Drawing.Color.FromArgb(rand.Next(1, 255), rand.Next(1, 255), rand.Next(1, 255))), new System.Drawing.Drawing2D.GraphicsPath( System.Drawing.Drawing2D.FillMode.Winding));
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                //HttpContext.Current.Response.ContentType = "IMAGE/GIF";
                //bitmap.Save(Server.MapPath("\\") + "VCode.gif");
                //imgVCode.ImageUrl = "VCode.gif";
                //return File(ms.GetBuffer(), @"image/jpeg");
            }
            catch (Exception error_Paint)
            {
                object obj = error_Paint;
            }
            finally
            {
                graphics.Dispose();
                bitmap.Dispose();
            }
            return File(ms.GetBuffer(), @"image/gif");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Shop(int classId = 1)
        {
            ViewData["CurrentUserVip"] = UILoginUser.u015;
            ViewData["vipDiscount"] = serSystem.GetKeyValue("SYS_VIP_DISCOUNT").cfg003;
            ViewData["shopClassList"] = serSystem.GetShopClassAllList();
            ViewData["shopProductList"] = serSystem.GetShowShopProductList(classId);
            return ViewExPath("Shop", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Shop(int ProductId, int num, string address, string phoneNumber, string zip, string name)
        {
            string[] vipDiscountInfo = serSystem.GetKeyValue("SYS_VIP_DISCOUNT").cfg003.Split(',');
            decimal userDiscount = 0;
            foreach (var item in vipDiscountInfo)
            {
                if (item.IndexOf(UILoginUser.u015 + ":", 0) != -1)
                {
                    userDiscount = decimal.Parse(item.Split(':')[1]);
                    break;
                }
            }
            MR mr = new MR();
            mr = serSystem.BuyProduct(ProductId, num, UILoginUser.u001, address, phoneNumber, zip, name, userDiscount);
            return Json(mr);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShopReport(int? status, int pageIndex = 0, int pageSize = 25, int pageBlockSize = 5)
        {
            ViewData["shopClassList"] = serSystem.GetShopClassAllList();
            int recordCount;
            ViewData["shopRecordList"] = serSystem.GetShopRecordList(UILoginUser.u001, status, pageIndex, pageSize, out recordCount);
            ViewData["PageList"] = _NWC.PageList.GetPageListUI2(pageSize, recordCount, pageIndex, pageBlockSize, "wp_page fl_r", "pageIndex", "/Am/ShopRecord");
            return ViewEx();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public string fileBetUpload()
        {
            if (Request.Files[0].ContentType == "text/plain")
            {
                byte[] fileResult = new byte[0];
                fileResult = new byte[Request.Files[0].ContentLength];
                Request.Files[0].InputStream.Read(fileResult, 0, Request.Files[0].ContentLength);
                return System.Text.Encoding.UTF8.GetString(fileResult);
            }
            return "error";
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public void ToGame(string type, int? id)
        {
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public void TestA()
        {
            Response.Write(_NWC.GeneralCache.Get(SessionID) == null ? "NULL" : _NWC.GeneralCache.Get(SessionID));
            Response.Write(SessionID);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public void TestB()
        {
            Response.Write(SessionID);
            var list = _NWC.GeneralCache.GetCacheList();
            foreach (var item in list)
            {
            }
        }
        public string CountryName(int id) { return "Hello"; }
    }
}
