using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameServices;
using _NWC = NETCommon;
using System.IO;
namespace GameUI.Controllers
{
    [AMRule]
    public class AMController : BaseController
    {
        private int PageSize
        {
            get
            {
                if (null == DicKV)
                {
                    return 30;
                }
                return int.Parse(DicKV["AM_PAGE_SIZE"].cfg003);
            }
        }
        private int PageSizeU { get; set; }
        private int PageBloclSize
        {
            get
            {
                if (null == DicKV)
                {
                    return 10;
                }
                return int.Parse(DicKV["AM_PAGE_BLOCK_SIZE"].cfg003);
            }
        }
        public AMController(IGame game, IMenu menu, IFinance finance, ISystem system, IUser user)
        {
            serGame = game;
            serMenu = menu;
            serFinance = finance;
            serSystem = system;
            serUser = user;
            ViewData["GlobalTitle"] = GetKV("AGU_TITLE", true).cfg003;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            var currentUser = AMLoginUser;
            ViewData["MGU"] = currentUser.mu002.Trim();
            ViewData["MGUX"] = currentUser.mu003.Trim();
            ViewData["OnlineCount"] = serSystem.GetOnlineCount();
            return ViewExPath("Index", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Online(int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int recordCount = 0;
            var _status = _NWC.GeneralValidate.IsNumber(Request["status"]) == false ? -1 : int.Parse(Request["status"]);
            var _account = string.IsNullOrEmpty(Request["account"]) || string.IsNullOrWhiteSpace(Request["account"]) ? string.Empty : Request["account"];
            var _domain = string.IsNullOrEmpty(Request["domain"]) || string.IsNullOrWhiteSpace(Request["domain"]) ? string.Empty : Request["domain"];
            var _ip = string.IsNullOrEmpty(Request["ip"]) || string.IsNullOrWhiteSpace(Request["ip"]) ? string.Empty : Request["ip"];
            ViewData["Status"] = _status;
            ViewData["Account"] = _account;
            ViewData["Domain"] = _domain;
            ViewData["IP"] = _ip;
            ViewData["OList"] = serSystem.GetOnlineList(0, _account, _ip, _domain, _status, (int)pageIndex, PageSize, out recordCount);
            string pageURL = Url.Action("Online", "AM", new { status = _status, account = _account, domain = _domain, ip=_ip });
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            return ViewExPath("Online", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Online(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            if ("set_offline" == MethodType)
            {
                var userID = _NWC.GeneralValidate.IsNumber(Request["key"]) ? int.Parse(Request["key"]) : 0;
                if (0 < userID)
                {
                    serSystem.SetUserOffline(userID);
                }
            }
            ajax.Data = serSystem.GetOnlineCount();
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult KeyValue(string key)
        {
            DBModel.wgs027 editModel = new DBModel.wgs027();
            if ("add" == MethodType || "edit" == MethodType)
            {
                if ("edit" == MethodType)
                {
                    editModel = serSystem.GetKeyValue(key);
                }
                ViewData["EditModel"] = editModel;
                return ViewExPath("KeyValueEdit", null, null);
            }
            ViewData["KVList"] = serSystem.GetKeyValueList();
            return ViewExPath("KeyValue", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult KeyValue(FormCollection form)
        {
            bool haveData = false;
            DBModel.wgs027 entity = new DBModel.wgs027();
            List<DBModel.wgs027> entityList = new List<DBModel.wgs027>();
            if ("add" == MethodType)
            {
                haveData = TryUpdateModel(entity);
                if (haveData)
                {
                    serSystem.AddKeyValue(entity);
                }
            }
            else if ("edit" == MethodType)
            {
                haveData = TryUpdateModel(entity);
                if (haveData)
                {
                    serSystem.UpdateKeyValue(entity);
                }
            }
            return RedirectToAction("KeyValue");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Menu(int ?menuID, int? parentID, int? menuType)
        {
            if (false == parentID.HasValue)
            {
                parentID = 0;
            }
            if (false == menuType.HasValue)
            {
                menuType = 0;
            }
            ViewData["ParentID"] = parentID;
            ViewData["MenuType"] = menuType;
            if ("add" == MethodType)
            {
                return ViewExPath("MenuEdit", null, null);
            }
            List<DBModel.wgs004> model = serMenu.GetMenuList((int)parentID, (int)menuType, -1);
            return ViewExPath("Menu", null, model);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Menu(FormCollection form)
        {
            int parentID = 0;
            int menuType = 0;
            if ("add" == MethodType)
            {
                DBModel.wgs004 wgs004 = new DBModel.wgs004();
                UpdateModel(wgs004);
                parentID = wgs004.sm002;
                if (null != wgs004)
                {
                    serMenu.AddMenu(wgs004);
                    menuType = wgs004.sm005;
                }
                else
                {
                    wgs004.sm002 = 0;
                }
            }
            if ("updateList"==MethodType)
            {
                List<DBModel.wgs004> modelList = new List<DBModel.wgs004>();
                UpdateModel(modelList);
                if (0 < modelList.Count)
                {
                    parentID = modelList[0].sm002;
                    serMenu.UpdateMenu(modelList);
                }
                menuType = modelList.FirstOrDefault().sm005;
            }
            return RedirectToAction("Menu", new { parentID = parentID, menuType=menuType });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Game()
        {
            List<DBModel.wgs001> list = serGame.GetGameList();
            return ViewExPath("Game", null, list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Game(FormCollection form)
        {
            List<DBModel.wgs001> updateList = new List<DBModel.wgs001>();
            if (TryUpdateModel(updateList))
            {
                serGame.UpdateGame(updateList);
            }
            return RedirectToAction("Game");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GameMethodGroup()
        {
            if ("add" == MethodType)
            {
                return ViewExPath("GameMethodGroupEdit", null, null);
            }
            List<DBModel.wgs003> list = serGame.GetGameMethodGroupList();
            return ViewExPath("GameMethodGroup", null, list);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult TeamInfo()
        {
            return ViewExPath("TeamInfo", null, null);
        }

        #region "ORZ 分红"

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMoneyReport(int? pageIndex)
        {
            var _dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(Request["dts"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00");
            var _dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59");
            
            var _type = _NWC.GeneralValidate.IsNumber(Request["type"]) == false ? (int)-1 : int.Parse(Request["type"]);
            var _acct = string.IsNullOrEmpty(Request["acct"]) ? string.Empty : Request["acct"];
            var _om = !_NWC.GeneralValidate.IsNumber(Request["om"]) ? 0 : int.Parse(Request["om"]);
            var _omm = !_NWC.GeneralValidate.IsDecimal(Request["omm"]) ? 0.0000m : decimal.Parse(Request["omm"]);


            DateTime dts = _dts;
            DateTime dte = _dte;
            ViewData["DTS"] = dts;
            ViewData["DTE"] = dte;
            ViewData["Type"] = _type;
            ViewData["om"] = _om;
            ViewData["omm"] = _omm;
            ViewData["acct"] = _acct;


            DBModel.WGS056Where ws = new DBModel.WGS056Where();
            ws.dts = dts;
            ws.dte = dte;
            ws.type = _type;
            ws.acct = _acct;
            ws.om = _om;
            ws.omm = _omm;

            

            User_AM ua = new User_AM();
            List<DBModel.wgs056> xlist = new List<DBModel.wgs056>();

            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int recordCount = 0;
            xlist = ua.GetMyFHList((int)pageIndex, PageSize, out recordCount,ws);

            string pageURL = Url.Action("GetMoneyReport", "AM");
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);



            string mytr = "";
            decimal memoney = 0;
            foreach (DBModel.wgs056 item in xlist)
            {

                mytr += "<tr key='" + item.s001.ToString() + "' >";
                mytr += "<td>" + item.u002 + "</td>";
                mytr += "<td>" + (item.f002 * 100).ToString() + "%</td>";
                mytr += "<td>" + item.s004.ToString() + "</td>";

                mytr += "<td>" + item.s012.ToString() + "</td>";
                mytr += "<td>" + item.s013.ToString() + "</td>";
                mytr += "<td>" + (-1 * item.s014).ToString() + "</td>";

                mytr += "<td>" + item.s016.ToString() + "</td>";
                mytr += "<td>" + item.s015.ToString() + "</td>";
                mytr += "<td>" + item.s002.ToString() + "</td>";
                if (item.s003 < 0) { memoney = Math.Abs(Convert.ToDecimal(item.s003)); }
                else { memoney = 0; }
                mytr += "<td>" + memoney.ToString() + "</td>";
                mytr += "<td>" + (item.s006.ToString("yyyy-MM-dd HH:mm:ss")) + "</td>";
                mytr += "<td>" + (item.s007.ToString("yyyy-MM-dd HH:mm:ss")) + "</td>";

                mytr += "<td>" + sname(item.s005.ToString()) + "</td>";
                mytr += "<td>" + stime(item.s011.ToString("yyyy-MM-dd HH:mm:ss")) + "</td>";
                mytr += "<td>" + item.s008.ToString() + "</td>";
                mytr += "</tr>";
            }


            ViewData["FHLog"] = mytr;
            return ViewExPath("GetMoneyReport", null, null);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMoneyPass(int? pageIndex)
        {

            User_AM ua = new User_AM();
            List<DBModel.wgs056> xlist = new List<DBModel.wgs056>();

            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int recordCount = 0;
            xlist = ua.GetFHLog((int)pageIndex, PageSize, out recordCount);

            string pageURL = Url.Action("GetMoneyPass", "AM");
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);



            string mytr = "";
            decimal memoney = 0;
            foreach (DBModel.wgs056 item in xlist)
            {

                mytr += "<tr key='" + item.s001.ToString() + "' >";
                mytr += "<td>" + item.u002 + "</td>";
                mytr += "<td>" + (item.f002 * 100).ToString() + "%</td>";
                mytr += "<td>" + item.s004.ToString() + "</td>";

                mytr += "<td>" + item.s012.ToString() + "</td>";
                mytr += "<td>" + item.s013.ToString() + "</td>";
                mytr += "<td>" + (-1 * item.s014).ToString() + "</td>";

                mytr += "<td>" + item.s016.ToString() + "</td>";
                mytr += "<td>" + item.s015.ToString() + "</td>";               
                mytr += "<td>" + item.s002.ToString() + "</td>";
                if (item.s003 < 0) { memoney = Math.Abs(Convert.ToDecimal(item.s003)); }
                else { memoney = 0; }
                mytr += "<td>" + memoney.ToString() + "</td>";
                //mytr += "<td>" + item.s011.ToString("HH:mm:ss") + "</td>";
                mytr += "<td>" + (item.s006.ToString("yyyy-MM-dd HH:mm:ss")) + "</td>";
                mytr += "<td>" + (item.s007.ToString("yyyy-MM-dd HH:mm:ss")) + "</td>";
                mytr += "<td>&nbsp;<a href='javascript:void(0);'  onclick=\"CheckUser('" + item.s001.ToString() + "', '手工通过', 1);\"   title='通过分红'>通过分红</a>&nbsp;<a href='javascript:void(0);' name='send_point' data='" + item.u002 + "' data2='" + memoney + "'  data3='" + item.s001.ToString() + "' title='取消分红' >取消分红</a>&nbsp;</td>";
                mytr += "</tr>";
            }


            ViewData["FHLLog"] = mytr;

            return ViewExPath("GetMoneyPass", null, null);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult PassMoney()
        {

            User_AM ua = new User_AM();
            string msg = "";
            if (ViewData["AMLoginUserID"] != null)
            {
                msg = ua.PassFH(Request["fid"].ToString(), Convert.ToInt32(ViewData["AMLoginUserID"]), Request["about"].ToString(), Convert.ToInt32(Request["isps"].ToString()));
            }

            AJAXObject ajax = new AJAXObject();
            ajax.Data = msg;

            return Json(ajax, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMoneyLog(int? pageIndex)
        {

            User_AM ua = new User_AM();
            List<DBModel.wgs056> xlist = new List<DBModel.wgs056>();

            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int recordCount = 0;
            xlist = ua.GetMyFHLog((int)pageIndex, PageSize, out recordCount);

            string pageURL = Url.Action("GetMoneyLog", "AM");
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);



            string mytr = "";
            decimal memoney = 0;
            foreach (DBModel.wgs056 item in xlist)
            {

                mytr += "<tr key='" + item.s001.ToString() + "' >";
                mytr += "<td>" + item.u002 + "</td>";
                mytr += "<td>" + (item.f002 * 100).ToString() + "%</td>";
                mytr += "<td>" + item.s004.ToString() + "</td>";
                mytr += "<td>" + item.s012.ToString() + "</td>";
                mytr += "<td>" + item.s013.ToString() + "</td>";
                mytr += "<td>" + (-1 * item.s014).ToString() + "</td>";

                mytr += "<td>" + item.s016.ToString() + "</td>";
                mytr += "<td>" + item.s015.ToString() + "</td>";
                mytr += "<td>" + item.s002.ToString() + "</td>";
                if (item.s003 < 0) { memoney = Math.Abs(Convert.ToDecimal(item.s003)); }
                else { memoney = 0; }
                mytr += "<td>" + memoney.ToString() + "</td>";
                mytr += "<td>" + item.s011.ToString("yyyy-MM-dd HH:mm:ss") + "</td>";
                mytr += "<td>" + sname(item.s005.ToString()) + "</td>";
                mytr += "<td>" + item.s008.ToString() + "</td>";
                mytr += "</tr>";
            }


            ViewData["FHLog"] = mytr;
            return ViewExPath("GetMoneyLog", null, null);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMoneyAdd()
        {
            var editModel = new DBModel.wgs056();
           
            ViewData["EditModel"] = editModel;
            return ViewExPath("GetMoneyAdd", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetMoneyAdd(FormCollection form)
        {
            DBModel.wgs056 entity = new DBModel.wgs056();
            if (TryUpdateModel(entity)) {
                DBModel.wgs012 wgs012 = serUser.GetAGU(entity.u002.Trim());
                if (wgs012 != null)
                    entity.u001 = wgs012.u001;
                else
                    return ViewExPath("GetMoneyAdd", null, null);               
                
                entity.f002 = entity.f002 / 100;
                if (entity.s002 > 0)
                    entity.s002 = entity.s002 * -1;

                entity.s009 = DateTime.Now;
                entity.s011 = DateTime.Now;
                entity.s003 = 0;
                entity.s008 = "";
                entity.s010 = "";
                entity.s016 = 0;
                GameServices.User_ORZ orz = new GameServices.User_ORZ();
                MR mr = new MR();
                mr =orz.GetMoneyPassAdd(entity);
            }
            return RedirectToAction("GetMoneyPass");
        }


        string sname(string value)
        {
            if (value == "1") return "已通过";
            else if (value == "2") return "已取消";
            else if (value == "0") return "待审核";
            else return "未知";

        }

        string stime(string value)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(value,"1900-01-01")) return " -- ";
            else return value;

        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMoneySet(int? pageIndex)
        {

            User_AM ua = new User_AM();
            DBModel.wgs055 wg = new DBModel.wgs055();
            int count = Convert.ToInt32(Request["xindex"]);


            for (int i = 1; i <= count; i++)
            {
                if (Request["u" + i.ToString()] != null)
                {
                    wg.u002 = Request["u" + i.ToString()].ToString();
                    wg.f002 = Convert.ToDouble(Convert.ToInt32(Request["b" + i.ToString()]) * 0.01);
                    wg.f003 = Convert.ToInt32(Request["d" + i.ToString()]);
                    wg.f004 = Convert.ToDateTime(Request["s" + i.ToString()]);
                    wg.u001 = Convert.ToInt32(Request["i" + i.ToString()]);
                    wg.f006 = Convert.ToDateTime("1900-01-01 " + Request["f" + i].ToString());
                    wg.f007 = DateTime.Now;
                    wg.f008 = Convert.ToDateTime("1900-01-01 00:00:00");
                    wg.f009 = Convert.ToInt32(ViewData["AMLoginUserID"]);
                    ua.AddFH(wg);
                }
            }

            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int recordCount = 0;

            var gameClassList = new User_AM().GetFHList((int)pageIndex, PageSize, out recordCount);
            string mytr = "";
            foreach (DBModel.wgs055 item in gameClassList)
            {
                mytr += "<tr >";
                mytr += "<td>" + item.u002 + "</td>";
                mytr += "<td>" + (item.f002 * 100).ToString() + "%</td>";
                mytr += "<td>" + item.f003.ToString() + "</td>";
                mytr += "<td>" + item.f004.ToString("yyyy-MM-dd HH:mm:ss") + "</td>";
                mytr += "<td>" + item.f006.ToString("HH:mm:ss") + "</td>";
                mytr += "<td> <a href='javascript:void(0);' name='send_point'  data='" + item.f001 + "' data2='" + item.u002.ToString() + "'  data3='" + item.f002.ToString() + "' data4='" + item.f003.ToString() + "' title='修改'>修改</a></td>";
                mytr += "</tr>";
            }
            ViewData["FHList"] = mytr;


            string pageURL = Url.Action("GetMoneyPass", "AM");
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);

            return ViewExPath("GetMoneySet", null, null);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SetMoney()
        {
            User_AM ua = new User_AM();

            AJAXObject ajax = new AJAXObject();
            string msg = "";
            if (ViewData["AMLoginUserID"] != null)
            {
                msg = ua.SetMoney(Convert.ToInt32(ViewData["AMLoginUserID"]), Convert.ToInt32(Request["fbl"]), Convert.ToInt32(Request["fzq"]), Convert.ToInt32(Request["fid"]));
            }

            ajax.Data = msg;
            return Json(ajax, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CheckUser()
        {
            AJAXObject ajax = new AJAXObject();
            User_AM us = new User_AM();

            int uid = us.CheckAGAccount(Request["001"].ToString());
            if (uid == -1)
            {
                //用户不存在
                ajax.Data = "-1";
            }
            else if (us.CheckAGAccountFH(Request["001"].ToString()))
            {
                //用户已分红
                ajax.Data = "-2";
            }
            else
            {
                ajax.Data = uid.ToString();
            }
            return Json(ajax, JsonRequestBehavior.AllowGet);
        }

        #endregion


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AllUser(int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            var posLevel = serUser.GetUserPositionLevel(false);
            ViewData["PosList"] = posLevel;
            ViewData["AcctLevelList"] = serUser.GetAccountLevel(false);
            var _parentID = _NWC.GeneralValidate.IsNumber(Request["parentID"]) == false ? UILoginUser.u001 : int.Parse(Request["parentID"]);
            var _userName = string.IsNullOrEmpty(Request["userName"]) ? string.Empty : Request["userName"];
            var _status = _NWC.GeneralValidate.IsNumber(Request["userStatus"]) == false ? -1 : int.Parse(Request["userStatus"]);
            var _rDTS = _NWC.GeneralValidate.IsDatetime(Request["regDTS"]) == false ? (DateTime?)null : DateTime.Parse(Request["regDTS"]);
            var _rDTE = _NWC.GeneralValidate.IsDatetime(Request["regDTE"]) == false ? (DateTime?)null : DateTime.Parse(Request["regDTE"]);
            var _lDTS = _NWC.GeneralValidate.IsDatetime(Request["loginDTS"]) == false ? (DateTime?)null : DateTime.Parse(Request["loginDTS"]);
            var _lDTE = _NWC.GeneralValidate.IsDatetime(Request["loginDTE"]) == false ? (DateTime?)null : DateTime.Parse(Request["loginDTE"]);
            var _amtT = _NWC.GeneralValidate.IsNumber(Request["amountType"]) == false ? 0 : int.Parse(Request["amountType"]);
            var _amtV = _NWC.GeneralValidate.IsNumber(Request["amountTypeV"]) == false ? 0m : decimal.Parse(Request["amountTypeV"]);
            var _pntT = _NWC.GeneralValidate.IsNumber(Request["pointType"]) == false ? 0 : int.Parse(Request["pointType"]);
            var _pntV = _NWC.GeneralValidate.IsNumber(Request["pointTypeV"]) == false ? 0m : decimal.Parse(Request["pointTypeV"]);
            var _ip = _NWC.GeneralValidate.IsNullOrEmpty(Request["ip"]) == false ? string.Empty : Request["ip"];
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
            var ip = _ip;
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
            ViewData["IP"] = ip;
            int recordCount = 0;
            string pageURL = Url.Action("AllUser", "AM", new { parentID = -1, userName = userName, userStatus = status, regDTS = rDTS, regDTE = rDTE, loginDTS = lDTS, loginDTE = lDTE, amountType = amtT, amountTypeV = amtV, pointType = pntT, pointTypeV = pntV, IP = ip });
            var sysNotSetMomeyAccount = GetKV("SYS_NOTSET_MOMEYACCOUNT", true).cfg003;
            var userList = serUser.GetAGUAll(-1, userName, status, amtT, amtV, pntT, pntV, rDTS, rDTE, lDTS, lDTE, ip, (int)pageIndex, PageSize, out recordCount, sysNotSetMomeyAccount);
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            ViewData["UserPackList"] = userList;
            var wtList = serFinance.GetWithdrawTypeList();
            ViewData["WTypeList"] = wtList;
            return ViewExPath("AllUser", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken()]
        public ActionResult AllUser(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            if ("setallstatus" == MethodType)
            {
                var _keys = form["key"];
                var _status = form["status"];
                if (string.IsNullOrEmpty(_keys))
                {
                    ajax.Code = 0;
                    ajax.Message = "没有任何数据";
                    return Json(ajax);
                }
                List<int> userIDs = new List<int>();
                var keysSplit = _keys.Split(',');
                foreach (var item in keysSplit)
                {
                    userIDs.Add(int.Parse(item));
                }
                var result = serUser.SetUserStatus(userIDs, int.Parse(_status));
                ajax.Code = result.Code;
                ajax.Message = result.Message;
                return Json(ajax);
            }
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GameMethodGroup(FormCollection form)
        {
            if ("add" == MethodType)
            {
                DBModel.wgs003 entity = new DBModel.wgs003();
                if( TryUpdateModel( entity))
                {
                    serGame.AddGameMethodGroup(entity);
                }
            }
            if ("updateList" == MethodType)
            {
                List<DBModel.wgs003> entityList = new List<DBModel.wgs003>();
                if (TryUpdateModel(entityList))
                {
                    serGame.UpdateGameMethodGroup(entityList);
                }
            }
            return RedirectToAction("GameMethodGroup");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GameMethod(int? gameID, int? parentID)
        {
            List<DBModel.wgs006> gameClassList = serGame.GetGameClassList();
            if (false == gameID.HasValue)
            {
                gameID = gameClassList.FirstOrDefault().gc001;
            }
            if (false == parentID.HasValue)
            {
                parentID = 0;
            }
            ViewData["GameList"] = gameClassList;
            ViewData["GameID"] = gameID;
            ViewData["ParentID"] = parentID;
            ViewData["GameMethodGroupList"] = serGame.GetGameMethodGroupList();
            List<DBModel.wgs002> gameMethodList = serGame.GetGameMethodList((int)gameID, (int)parentID);
            if (parentID != 0)
            {
                var defaultParent = serGame.GetGameMethodList().Where(exp => exp.gm001 == parentID).FirstOrDefault();
                ViewData["ParentGameMethodName"] = defaultParent.gm004;
            }
            else
            {
                ViewData["ParentGameMethodName"] = "";
            }
            if ("add" == MethodType)
            {
                return ViewExPath("GameMethodEdit", null, null);
            }
            return ViewExPath("GameMethod", null, gameMethodList);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GameMethod(FormCollection form)
        {
            int gameID = 0;
            int parentID = 0;
            if ("add" == MethodType)
            {
                DBModel.wgs002 entity = new DBModel.wgs002();
                if (TryUpdateModel(entity))
                {
                    serGame.AddGameMethod(entity);
                    gameID = entity.g001;
                    parentID = entity.gm002;
                }
            }
            if ("updateList" == MethodType)
            {
                List<DBModel.wgs002> entityList = new List<DBModel.wgs002>();
                if (TryUpdateModel(entityList))
                {
                    serGame.UpdateGameMethod(entityList);
                    gameID = entityList.FirstOrDefault().g001;
                    parentID = entityList.FirstOrDefault().gm002;
                }
            }
            return RedirectToAction("GameMethod", new { gameID=gameID, parentID=parentID});
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GameSession(int? gameID, int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int recordCount = 0;
            List<DBModel.wgs001> gameList = serGame.GetGameList();
            if (false == gameID.HasValue)
            {
                gameID = gameList.FirstOrDefault().g001;
            }
            ViewData["GameID"] = gameID;
            ViewData["GameList"] = gameList;
            if ("add" == MethodType)
            {
                ViewData["GameClassID"] = serGame.GetGameClassByGameID((int)gameID);
                return ViewExPath("GameSessionEdit", null, null);
            }
            List<DBModel.wgs005> gameSessionList = serGame.GetGameSessionList((int)gameID, -1, (int)pageIndex, PageSize, out recordCount);
            string pageURL = Url.Action("GameSession", "AM", new { gameID=gameID });
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            return ViewExPath("GameSession", null, gameSessionList);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GameSession(FormCollection form)
        {
            string set_gameID = string.Empty;
            if (0 < Request.Files.Count && 0 < Request.Files[0].ContentLength )
            {
                set_gameID = form["g001"];
                string set_gcid = form["gc001"];
                byte[] cnt = new byte[Request.Files["uploadfile"].ContentLength];
                Request.Files[0].InputStream.Read(cnt, 0, Request.Files["uploadfile"].ContentLength);
                var gscontent = System.Text.Encoding.Default.GetString(cnt);
                var gsList = gscontent.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                List<DBModel.wgs005> saveList = new List<DBModel.wgs005>();
                foreach (var gsItem in gsList)
                {
                    if (string.IsNullOrEmpty(gsItem))
                    {
                        continue;
                    }
                    var itemSplit = gsItem.Split(';');
                    DBModel.wgs005 item = new DBModel.wgs005();
                    item.g001 = int.Parse(set_gameID);
                    item.gs002 = itemSplit[0];
                    item.gc001 = int.Parse(set_gcid);
                    item.gs003 = DateTime.Parse(itemSplit[1]);
                    item.gs004 = DateTime.Parse(itemSplit[2]);
                    item.gs005 = DateTime.Parse(itemSplit[3]);
                    item.gs006 = DateTime.Now;
                    item.gs011 = 1;
                    saveList.Add(item);
                }
                try
                {
                    serGame.AddGameSession(saveList);
                }
                catch (Exception error)
                {
                    throw error;
                }
            }
            else
            {
                set_gameID = Request["g001"];
                string ser_no = form["ser_no"];
                string start_type = form["start_type"];
                string start_date = form["start_date"];
                string start_no = form["start_no"];
                string end_no = form["end_no"];
                string start_time = form["start_time"];
                string start_time2 = form["start_time2"];
                string close_time = form["close_time"];
                string close_time2 = form["close_time2"];
                string open_time = form["open_time"];
                string open_time2 = form["open_time2"];
                MR mr = serGame.AddGameSession(int.Parse(start_type), int.Parse(set_gameID), Convert.ToDateTime(start_date), long.Parse(ser_no), long.Parse(start_no), int.Parse(end_no), Convert.ToDateTime(start_time), Convert.ToDateTime(close_time), Convert.ToDateTime(open_time), int.Parse(start_time2), int.Parse(close_time2), int.Parse(open_time2));
                if (0 == mr.Code)
                {
                    throw mr.Exception;
                }
            }
            return RedirectToAction("GameSession", new { gameID=set_gameID});
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GameClass(int? gameClassID)
        {
            ViewData["EditModel"] = new DBModel.wgs006();
            if (false == gameClassID.HasValue)
            {
                gameClassID = 0;
            }
            else
            {
                ViewData["EditModel"] = serGame.GetGameClassList().Where(exp => exp.gc001 == (int)gameClassID).FirstOrDefault();
            }
            var gameList = serGame.GetGameList();
            var gameClassList = serGame.GetGameClassList();
            foreach (var gc in gameClassList)
            {
                if ( gc.gc001 == gameClassID)
                {
                    continue;
                }
                string[] keys = gc.gc004.Split(',');
                foreach (var key in keys)
                {
                    var tempGameItem = gameList.Where(exp=>exp.g001 == int.Parse(key)).FirstOrDefault();
                    gameList.Remove(tempGameItem);
                }
            }
            ViewData["GameOriList"] = serGame.GetGameList(); 
            ViewData["GameList"] = gameList;
            ViewData["GameClassList"] = gameClassList;
            if ("add" == MethodType || "edit" == MethodType)
            {
                return ViewExPath("GameClassEdit", null, null);
            }
            return ViewExPath("GameClass", null, gameClassList);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GameClass(FormCollection form)
        {
            if ("add" == MethodType || "edit" == MethodType)
            {
                DBModel.wgs006 entity = new DBModel.wgs006();
                string gameIDs = form["gc004"];
                if (TryUpdateModel(entity))
                {
                    entity.gc004 = gameIDs;
                    if ("edit" == MethodType)
                    {
                        serGame.UpdateGameClass(entity);
                    }
                    else
                    {
                        serGame.AddGameClass(entity);
                    }
                }
            }
            return RedirectToAction("GameClass");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GameClassPrize(int? gameClassID, int? gameClassPrizeID)
        {
            List<DBModel.wgs006> gameClassList = serGame.GetGameClassList();
            if (false == gameClassID.HasValue)
            {
                gameClassID = gameClassList.FirstOrDefault().gc001;
            }
            ViewData["GameClassID"] = gameClassID;
            ViewData["GameClassList"] = gameClassList;
            ViewData["EditModel"] = new DBModel.wgs007();
            if ("add" == MethodType)
            {
                return ViewExPath("GamePrizeEdit", null, null);
            }
            if ("config" == MethodType)
            {
                var gameMethodGroupList = serGame.GetGameMethodGroupList();
                var gameMethodList = serGame.GetGameMethodList().Where(exp => exp.g001 == (int)gameClassID && exp.gm002!=0).ToList();
                serGame.InitGamePrizeData((int)gameClassID, (int)gameClassPrizeID);
                var gameMethodPrizeDataList = serGame.GetGameMethodPrizeData((int)gameClassPrizeID);
                var gameMethodPrizeDataDicList = gameMethodPrizeDataList.ToDictionary<DBModel.wgs008, int>(key => key.gm001);
                ViewData["GameMethodPrizeDataDicList"] = gameMethodPrizeDataDicList;
                ViewData["GameMethodGroupList"] = gameMethodGroupList;
                ViewData["GameMethodList"] = gameMethodList;
                return ViewExPath("GamePrizeConfig", null, null);
            }
            var gameClassPrizeList = serGame.GetGameClassPrize((int)gameClassID);
            return ViewExPath("GamePrize", null, gameClassPrizeList);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GamePrizeData(int? gameClassID, decimal? group, int? gameClassPrizeID)
        {
            var gcList = serGame.GetGameClassList();
            if (false == gameClassID.HasValue)
            {
                gameClassID = gcList.FirstOrDefault().gc001;
            }
            if (false == group.HasValue)
            {
                group = 0;
            }
            var gpdGroupList = serGame.GetGPDDataGroupList((int)gameClassID);
            var gpdDataList = serGame.GetGPDDataList((int)gameClassID, (decimal)group);
            ViewData["GroupList"] = gpdGroupList;
            ViewData["GameClassList"] = gcList;
            ViewData["GameClassID"] = gameClassID;
            ViewData["GPDDataList"] = gpdDataList;
            ViewData["DicItemList"] = serGame.GetGPDDataListByCache().ToDictionary(exp=>exp.gtp001);
            if ("config" == MethodType)
            {
                serGame.InitGPDData((int)gameClassID, (int)gameClassPrizeID);
                ViewData["GPDDataList"] = serGame.GetSetGPDDataList((int)gameClassPrizeID);
                return ViewExPath("GPDTConfig", null, null);
            }
            return ViewExPath("GPDT", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GamePrizeData(FormCollection form)
        {
            bool haveData = false;
            AJAXObject ajax = new AJAXObject();
            if ("updateSetGamePrizeData" == MethodType)
            {
                ajax.Message = "错误";
                List<DBModel.wgs029> gpdDataList = new List<DBModel.wgs029>();
                haveData = TryUpdateModel(gpdDataList);
                if (haveData)
                {
                    MR mr = serGame.UpdateSetGPDData(gpdDataList);
                    if (0 == mr.Code)
                    {
                        ajax.Message = mr.Message;
                        return Json(ajax);
                    }
                    else
                    {
                        ajax.Message = "操作成功";
                        ajax.Code = 1;
                        return Json(ajax);
                    }
                }
                return Json(ajax);
            }
            if ("add" == MethodType || "updateList" == MethodType)
            {
                DBModel.wgs028 entity = new DBModel.wgs028();
                List<DBModel.wgs028> entityList = new List<DBModel.wgs028>();
                if ("add" == MethodType)
                {
                    haveData = TryUpdateModel(entity);
                }
                else if ("updateList" == MethodType)
                {
                    haveData = TryUpdateModel(entityList);
                }
                if (haveData)
                {
                    MR result = new MR();
                    if ("add" == MethodType)
                    {
                        result = serGame.AddGPDData(entity);
                    }
                    else if ("updateList" == MethodType)
                    {
                        result = serGame.UpdateGPDData(entityList);
                    }
                    if (0 == result.Code)
                    {
                        ajax.Message = result.Message;
                        return Json(ajax);
                    }
                    ajax.Code = 1;
                    ajax.Message = "操作成功";
                }
            }
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GameClassPrize(FormCollection form)
        {
            if ("add" == MethodType || "updateList" == MethodType)
            {
                DBModel.wgs007 entity = new DBModel.wgs007();
                List<DBModel.wgs007> entityList = new List<DBModel.wgs007>();
                if (TryUpdateModel(entity))
                {
                    serGame.AddGameClassPrize(entity);
                }
                if (TryUpdateModel(entityList))
                {
                    serGame.UpdateGameClassPrize(entityList);
                }
            }
            if ("updateGameMethodPrize" == MethodType)
            {
                List<DBModel.wgs008> updateGMDList = new List<DBModel.wgs008>();
                if (TryUpdateModel(updateGMDList))
                {
                    serGame.UpdateGamePrizeData(updateGMDList);
                    return RedirectToAction("GameClassPrize", new { method = "config", gameClassID = updateGMDList[0].gc001, gameClassPrizeID=updateGMDList[0].gp001});
                }
            }
            return RedirectToAction("GameClassPrize");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Bank()
        {
            if ("add" == MethodType)
            {
                return ViewExPath("BankEdit", null, null);
            }
            var bankList = serFinance.GetBankList();
            return ViewExPath("Bank", null, bankList);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Bank(FormCollection form)
        {
            if ("add" == MethodType || "updateList" == MethodType)
            {
                DBModel.wgs010 entity = new DBModel.wgs010();
                List<DBModel.wgs010> entityList = new List<DBModel.wgs010>();
                if (TryUpdateModel(entity))
                {
                    serFinance.AddBank(entity);
                }
                if (TryUpdateModel(entityList))
                {
                    serFinance.UpdateBank(entityList);
                }
            }
            return RedirectToAction("Bank");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ManagerGroup(int? key)
        {
            var menuList = serMenu.GetMenuListByCache().Where(exp => exp.sm005 == 0 && exp.sm009 == 1).ToList();
            var editModelInit = new DBModel.wgs015();
            editModelInit.pg003 = "名称";
            editModelInit.pg004 = "显示名称";
            editModelInit.pg005 = "0";
            ViewData["EditModel"] = editModelInit;
            if (true == key.HasValue)
            {
                var editModel = serSystem.GetBPG((int)key);
                var editIDs = editModel.pg005.Split(',');
                string newIDs = string.Empty;
                foreach (var id in editIDs)
                {
                    var chkItem = menuList.Where(exp => exp.sm001 == int.Parse(id)).FirstOrDefault();
                    if (null != chkItem)
                    {
                        var chkCount = menuList.Where(exp => exp.sm002 == chkItem.sm001).Count();
                        if (0 == chkCount)
                        {
                            newIDs += chkItem.sm001 + ",";
                        }
                    }
                }
                editModel.pg005 = newIDs.Substring(0, newIDs.Length - 1);
                ViewData["EditModel"] = editModel;
            }
            if ("add" == MethodType || "edit"== MethodType)
            {
                ViewData["ManagerList"] = menuList;
                return ViewExPath("ManagerGroupEdit", null, null);
            }
            var BPGList = serSystem.GetBPGList();
            return ViewExPath("ManagerGroup", null, BPGList);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ManagerGroup(FormCollection form)
        {
            if ("add" == MethodType || "edit" == MethodType)
            {
                DBModel.wgs015 entity = new DBModel.wgs015();
                if (TryUpdateModel(entity))
                {
                    if ("add" == MethodType)
                    {
                        serSystem.AddBPG(entity);
                    }
                    else
                    {
                        serSystem.UpdateBPG(entity);
                    }
                }
            }
            return RedirectToAction("ManagerGroup");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Manager(int? key)
        {
            var gpList = serSystem.GetBPGList();
            var editModel = new DBModel.wgs016();
            if (false != key.HasValue)
            {
                editModel = serUser.GetMGU((int)key);
            }
            ViewData["EditModel"] = editModel;
            ViewData["PGList"] = gpList;
            if ("add" == MethodType || "edit" == MethodType)
            {
                return ViewExPath("ManagerEdit", null, null);
            }
            var muList = serUser.GetMGUList();
            return ViewExPath("Manager", null, muList);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Manager(FormCollection form)
        {
            if ("add" == MethodType || "edit" == MethodType)
            {
                DBModel.wgs016 entity = new DBModel.wgs016();
                if (TryUpdateModel(entity))
                {
                    if ("edit" == MethodType)
                    {
                        serUser.UpdateMGU(entity);
                    }
                    else
                    {
                        serUser.AddMGU(entity);
                    }
                }
            }
            return RedirectToAction("Manager");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult MemberAgent()
        {
            ViewData["CPList"] = serFinance.GetCTList();
            ViewData["GCPList"] = serGame.GetGameClassPrizeByCache();
            ViewData["GCList"] = serGame.GetGameClassList();
            ViewData["GList"] = serGame.GetGameList();
            ViewData["BankDicLidt"] = serFinance.GetBankListByCache().ToDictionary(exp => exp.sb001);
            return ViewExPath("MemberAgent", null, new DBModel.wgs012());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MemberAgent(FormCollection form)
        {
            DBModel.wgs012 entity = new DBModel.wgs012();
            #region 添加代理时的AJAX
            if ("getParentPoint" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                List<Object> result = new List<object>();
                List<DBModel.wgs007> sgpList = serGame.GetGameClassPrizeByCache();
                int parentID = 0;
                if (false == _NWC.GeneralValidate.IsNullOrEmpty(form["parentID"]))
                {
                    parentID = int.Parse(form["parentID"]);
                }
                var list = serUser.GetAGUPData(parentID);
                ajax.Code = 1;
                foreach (var item in list)
                {
                    var systemPoint = sgpList.Where(exp => exp.gp001 == item.gp001).FirstOrDefault();
                    var systemGameClass = serGame.GetGameClassListByCache().Where(exp => exp.gc001 == systemPoint.gc001).FirstOrDefault();
                    if (null != systemPoint)
                    {
                        result.Add(new { GameClassID = systemGameClass.gc001, GameClassPrizeID = systemPoint.gp001, GameClassName = systemGameClass.gc003, ShowPointName = systemPoint.gp003, SystemPoint = systemPoint.gp008,SystemPointX=systemPoint.gp007, ParentHavePoint = item.up002, CurrentHavePoint = item.up003,ParentHavePointX=item.up004, CurrentHavePointX=item.up005 });
                    }
                    else
                    {
                        ajax.Code = 0;
                    }
                }
                ajax.Data = result;
                return Json(ajax);
            }
            if ("checkAgentAccount" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                bool accountExists = false;
                if (TryUpdateModel(entity))
                {
                    accountExists = serUser.CheckAGAccount(entity.u002);
                }
                ajax.Code = accountExists == false ? 1 : 0;
                ajax.Message = accountExists ? "账号存在" : "账号不存在";
                return Json(ajax);
            }
            if ("getAGUIDs" == MethodType)
            {
                List<object> list = new List<object>();
                string treeIconCls = "parent_tree_icon";
                string id = Request["id"];
                int pid = 0;
                if (null != id)
                {
                    pid = int.Parse(id);
                }
                else
                {
                    list.Add(new { id = 0, text = "顶级", state = "closed", iconCls = treeIconCls });
                    return Json(list);
                }
                var aguList = serUser.GetAGUList(pid);
                foreach (var ag in aguList)
                {
                    int curCount = serUser.GetAGUCount(ag.u001);
                    list.Add(new { id = ag.u001, text = ag.u002.Trim() + "（" + ag.u003.Trim() + "）(" + curCount+")", state = curCount >= 1 ? "closed" : "open", iconCls = treeIconCls });
                }
                return Json(list);
            }
            #endregion
            if ("add" == MethodType)
            {
                MR mr = new MR();
                bool addEntityData = TryUpdateModel(entity);
                if (addEntityData)
                {
                    List<DBModel.wgs017> upEntityList = new List<DBModel.wgs017>();
                    bool pointData = TryUpdateModel(upEntityList);
                    if( false == pointData)
                    {
                        var pointDataError = new Exception("奖金有错");
                        throw pointDataError;
                    }
                    //string repName = string.Empty;
                    //string repName2 = string.Empty;
                    //repName = entity.u002;
                    //repName2 = entity.u003;
                    //for (int i = 0; i < 1000; i++)
                    //{
                    //    entity.u002 = repName + i;
                    //    entity.u003 = repName2 + i;
                    //    mr = serUser.AddAGU(entity);
                    //}
                    if (entity.u012 == 0)
                    {
                        entity.u017 = Request["u017"];
                    }
                    else
                    {
                        entity.u017 = null;
                    }
                    entity.u016 = AMLoginUser.mu001;
                    mr = serUser.AddAGU(entity, upEntityList);
                }
                if (0 == mr.Code)
                {
                    ViewData["CPList"] = serFinance.GetCTList();
                    ViewData["GCPList"] = serGame.GetGameClassPrizeByCache();
                    ViewData["GCList"] = serGame.GetGameClassList();
                    ViewData["GList"] = serGame.GetGameList();
                    ViewData["BankDicLidt"] = serFinance.GetBankListByCache().ToDictionary(exp => exp.sb001);
                    ViewData["ErrorMessage"] = mr.Message;
                    return ViewExPath("MemberAgent", null, entity);
                }
            }
            return RedirectToAction("MemberAgent");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Agent()
        {
            return ViewExPath("Agent", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AgentList(int? parentID, int? pageIndex)
        {
            int recordCount = 0;
            if (false == parentID.HasValue)
            {
                parentID = 0;
            }
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            var posLevel = serUser.GetUserPositionLevel(false);
            ViewData["PosList"] = posLevel;
            ViewData["AcctLevelList"] = serUser.GetAccountLevel(false);
            var _parentID = _NWC.GeneralValidate.IsNumber(Request["parentID"]) == false ? UILoginUser.u001 : int.Parse(Request["parentID"]);
            var _userName = string.IsNullOrEmpty(Request["userName"]) ? string.Empty : Request["userName"];
            var _status = _NWC.GeneralValidate.IsNumber(Request["userStatus"]) == false ? -1 : int.Parse(Request["userStatus"]);
            var _rDTS = _NWC.GeneralValidate.IsDatetime(Request["regDTS"]) == false ? (DateTime?)null : DateTime.Parse(Request["regDTS"]);
            var _rDTE = _NWC.GeneralValidate.IsDatetime(Request["regDTE"]) == false ? (DateTime?)null : DateTime.Parse(Request["regDTE"]);
            var _lDTS = _NWC.GeneralValidate.IsDatetime(Request["loginDTS"]) == false ? (DateTime?)null : DateTime.Parse(Request["loginDTS"]);
            var _lDTE = _NWC.GeneralValidate.IsDatetime(Request["loginDTE"]) == false ? (DateTime?)null : DateTime.Parse(Request["loginDTE"]);
            var _amtT = _NWC.GeneralValidate.IsNumber(Request["amountType"]) == false ? 0 : int.Parse(Request["amountType"]);
            var _amtV = _NWC.GeneralValidate.IsNumber(Request["amountTypeV"]) == false ? 0m : decimal.Parse(Request["amountTypeV"]);
            var _pntT = _NWC.GeneralValidate.IsNumber(Request["pointType"]) == false ? 0 : int.Parse(Request["pointType"]);
            var _pntV = _NWC.GeneralValidate.IsNumber(Request["pointTypeV"]) == false ? 0m : decimal.Parse(Request["pointTypeV"]);
            var _ip = _NWC.GeneralValidate.IsNullOrEmpty(Request["ip"]) == false ? string.Empty : Request["ip"];
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
            var ip = _ip;
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
            ViewData["IP"] = ip;
            ViewData["ParentID"] = parentID;
            string pageURL = Url.Action("AgentList", "AM", new { parentID = parentID, userName = userName, userStatus = status, regDTS = rDTS, regDTE = rDTE, loginDTS = lDTS, loginDTE = lDTE, amountType = amtT, amountTypeV = amtV, pointType = pntT, pointTypeV=pntV,IP=ip });
            var sysNotSetMomeyAccount = GetKV("SYS_NOTSET_MOMEYACCOUNT", true).cfg003;
            var userList = serUser.GetAGUList((int)parentID, userName, status, amtT, amtV, pntT, pntV, rDTS, rDTE, lDTS, lDTE, ip, (int)pageIndex, PageSize, out recordCount, sysNotSetMomeyAccount);
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            ViewData["UserPackList"] = userList;
            var wtList = serFinance.GetWithdrawTypeList();
            ViewData["WTypeList"] = wtList;
            return ViewExPath("AgentList", null, null);
        }
        public ActionResult StockReport()
        {
            var _dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(Request["dts"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00");
            var _dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59");
            var _type = _NWC.GeneralValidate.IsNumber(Request["type"]) == false ? (int)0 : int.Parse(Request["type"]);
            var _acct = string.IsNullOrEmpty(Request["acct"]) ? string.Empty : Request["acct"];
            var _pacct = string.IsNullOrEmpty(Request["pacct"]) ? string.Empty : Request["pacct"];
            var _om = !_NWC.GeneralValidate.IsNumber(Request["om"]) ? 0 : int.Parse(Request["om"]);
            var _bp = !_NWC.GeneralValidate.IsNumber(Request["bp"]) ? 2 : int.Parse(Request["bp"]);
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
            ViewData["bp"] = _bp;
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
                details = serFinance.GetDRExt(1, AMLoginUser.mu001, dts, dte, _type, _acct, 0, string.Empty, 0, _om, _omm, _cm, _cmm, _pm, _pmm);
            }
            else if (5 == _type)
            {
                _pacct = string.IsNullOrEmpty(_pacct) ? "_report_" : _pacct;
                ViewData["pacct"] = _pacct;
                details = serFinance.GetDRExt(1, AMLoginUser.mu001, dts, dte, _type, string.Empty, 0, _pacct, 0, _om, _omm, _cm, _cmm, _pm, _pmm);
            }
            ViewData["DRList"] = drList;
            ViewData["DRDList"] = details;
            return ViewExPath("StockReport", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AgentList(FormCollection form)
        {
            if ("editUser" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var _editUserID = _NWC.GeneralValidate.IsNumber(form["key"]) == false ? 0 : int.Parse(form["key"]);
                var _redpercent = _NWC.GeneralValidate.IsDecimal(form["red_percent"]) ? decimal.Parse(form["red_percent"]) : 0m;
                var editUser = serUser.GetAGU(_editUserID);
                if (null == editUser)
                {
                    ajax.Message = "数据不存在";
                    return Json(ajax);
                }
                var editPUser = serUser.GetAGU(editUser.u012);
                var pDataList = serUser.GetAGUPData(editUser.u001).OrderBy(exp => exp.gc001).ToList();
                decimal maxStockPercent = 0.0000m;
                var sysStockPercent = GetKV("SYS_STOCK_MAX", true).cfg003.Split(',');
                if (0 < editUser.u012)
                {
                    maxStockPercent = editPUser.u019;
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
                List<object> pdl = new List<object>();
                var gameClassList = serGame.GetGameClassListByCache();
                foreach (var pd in pDataList)
                {
                    pdl.Add(new { GameClassName = gameClassList.Where(exp => exp.gc001 == pd.gc001).FirstOrDefault().gc003, PID = pd.up001, Point = pd.up003 });
                }
                var editKey = _NWC.DEncrypt.Encrypt(editUser.u001 + "|" + editUser.u002.Trim() + "|" + editUser.u011);
                ajax.Code = 1;
                ajax.Data = new { UpdateKey = editKey,RedPercent = editUser.u024 * 100, UserName = editUser.u002.Trim(), UserNickname = string.IsNullOrEmpty(editUser.u003) ? "" : editUser.u003.Trim(), RegDate = editUser.u005.ToString(), LoginDate = editUser.u007.ToString(), UserState = editUser.u008, CanCreate = editUser.u020, LoginIP = string.IsNullOrEmpty(editUser.u022) ? "" : editUser.u022.Trim(), PDL = pdl };
                return Json(ajax);
            }
            else if("deleteUser" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var _editUserID = _NWC.GeneralValidate.IsNumber(form["key"]) == false ? 0 : int.Parse(form["key"]);
                var deleteMR = serUser.DeleteUser(_editUserID);
                ajax.Code = deleteMR.Code;
                ajax.Message = deleteMR.Message;
                return Json(ajax);
            }
            else if ("userBankUnbind" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var _editUserID = _NWC.GeneralValidate.IsNumber(form["key"]) == false ? 0 : int.Parse(form["key"]);
                var deleteMR = serFinance.DeleteWCashBank(_editUserID);
                ajax.Code = deleteMR.Code;
                ajax.Message = deleteMR.Message;
                return Json(ajax);
            }
            else if ("sendPoint" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var _key = _NWC.GeneralValidate.IsNumber(form["key"]) == false ? 0 : int.Parse(form["key"]);
                var _point = _NWC.GeneralValidate.IsNumber(form["point"]) == false ? 0 : int.Parse(form["point"]);
                var sendPointMR = serFinance.SendPointToUser(_key, _point);
                ajax.Code = sendPointMR.Code;
                ajax.Message = sendPointMR.Message;
                return Json(ajax);
            }
            else if ("sendFrozenSum" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var _key = _NWC.GeneralValidate.IsNumber(form["key"]) == false ? 0 : int.Parse(form["key"]);
                var _frozenSum = _NWC.GeneralValidate.IsNumber(form["frozenSum"]) == false ? 0 : int.Parse(form["frozenSum"]);
                var _remarks = string.IsNullOrEmpty(form["remarks"]) || string.IsNullOrWhiteSpace(form["remarks"]) ? string.Empty : form["remarks"];
                var sendPointMR = serFinance.SendFrozenSumToUser(_key, _frozenSum, _remarks);
             
                ajax.Code = sendPointMR.Code;
                ajax.Message = sendPointMR.Message;
                return Json(ajax);
            }
            else if ("updateUser" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var _edit_key = string.IsNullOrEmpty(form["edit_key"]) || string.IsNullOrWhiteSpace(form["edit_key"]) ? string.Empty : form["edit_key"];
                var _nickname = string.IsNullOrEmpty(form["nickname"]) || string.IsNullOrWhiteSpace(form["nickname"]) ? string.Empty : form["nickname"];
                var _login_pwd = string.IsNullOrEmpty(form["login_pwd"]) || string.IsNullOrWhiteSpace(form["login_pwd"]) ? string.Empty : form["login_pwd"];
                var _cash_pwd = string.IsNullOrEmpty(form["cash_pwd"]) || string.IsNullOrWhiteSpace(form["cash_pwd"]) ? string.Empty : form["cash_pwd"];
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
                var editUser = serUser.GetAGU(editUserID);
                var editPUser = serUser.GetAGU(editUser.u012);
                if (string.Empty != _login_pwd)
                {
                    editUser.u009 = _NWC.SHA1.Get(_login_pwd + editUser.u011, _NWC.SHA1Bit.L160);
                }
                if (string.Empty != _cash_pwd)
                {
                    editUser.u010 = _NWC.SHA1.Get(_cash_pwd + editUser.u011, _NWC.SHA1Bit.L160);
                }
                if (string.Empty != _nickname && _nickname != (editUser.u003 == null ? string.Empty : editUser.u003.Trim()))
                {
                    editUser.u003 = _nickname;
                }
                decimal maxStockPercent = 0.0000m;
                var sysStockPercent = GetKV("SYS_STOCK_MAX", true).cfg003.Split(',');
                if (2 < editUser.u018)
                {
                    maxStockPercent = editPUser.u019;
                    if (3 == editUser.u018)
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
                    if (2 < editUser.u018)
                    {
                        //ajax.Message = string.Format("最大可设置的分红值为{0}%", (int)maxStockPercent);
                        //return Json(ajax);
                    }
                }
                MR checkChildStock = serUser.CheckChildStockIsMax(editUser.u001, _redpercent);
                if (0 == checkChildStock.Code)
                {
                    //ajax.Message = string.Format("下级已经有大于{0}的分红，请先调整下级。{1}", (int)_redpercent, checkChildStock.Message);
                    //return Json(ajax);
                }
                editUser.u008 = (byte)_status;
                editUser.u020 = _can_child;
                editUser.u019 = _redpercent;
                var editUserPrizeData = serUser.GetAGUPData(editUser.u001);
                var editUserParentData = serUser.GetAGUPData(editUser.u012);
                foreach (var epd in editUserPrizeData)
                {
                    if (false == _NWC.GeneralValidate.IsDecimal(form["point" + epd.up001]))
                    {
                        ajax.Message = "非法修改返点";
                        return Json(ajax);
                    }
                    var point = Math.Round(decimal.Parse(form["point" + epd.up001]), 1);
                    decimal curGCMax = 0.0000m;
                    if (0 == editUser.u012)
                    {
                        curGCMax = serGame.GetGameClassPrize(epd.gc001).FirstOrDefault().gp008;
                    }
                    else
                    {
                        curGCMax = editUserParentData.Where(exp => exp.u001 == editUser.u012 && exp.gc001 == epd.gc001).FirstOrDefault().up003;
                    }
                    epd.up002 = curGCMax - point;
                    epd.up003 = point;
                }
                var updatePDL = serUser.SaveAGUPData(editUserPrizeData);
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
            else if ("editBank" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                var key = string.IsNullOrEmpty(form["key"]) || string.IsNullOrWhiteSpace(form["key"]) ? 0 : int.Parse(form["key"]);
                var updatekey = string.IsNullOrEmpty(form["updatekey"]) || string.IsNullOrWhiteSpace(form["updatekey"]) ? string.Empty : form["updatekey"];
                if (string.Empty == updatekey)
                {
                    var mwtList = serFinance.GetWCashBankList(key);
                    if (0 == mwtList.Count)
                    {
                        ajax.Message = "还没有绑定银行卡";
                    }
                    else
                    {
                        ajax.Code = 1;
                        ajax.Data = mwtList[0];
                        ajax.Message = mwtList[0].uwi010.Value.ToString(GetKV("SYS_DATETIME_FORMAT", false).cfg003);
                    }
                }
                else
                {
                    var bankCode = !_NWC.GeneralValidate.IsNumber(form["bank_code"]) ? 0 : int.Parse(form["bank_code"]);
                    var bankUserName = string.IsNullOrEmpty(form["bank_user"]) || string.IsNullOrWhiteSpace(form["bank_user"]) ? string.Empty : form["bank_user"];
                    var bankNo = string.IsNullOrEmpty(form["bank_no"]) || string.IsNullOrWhiteSpace(form["bank_no"]) ? string.Empty : form["bank_no"];
                    var BankLocation = string.IsNullOrEmpty(form["bank_location"]) || string.IsNullOrWhiteSpace(form["bank_location"]) ? string.Empty : form["bank_location"];
                    var BankOpen = string.IsNullOrEmpty(form["bank_open"]) || string.IsNullOrWhiteSpace(form["bank_open"]) ? string.Empty : form["bank_open"];
                    if (string.Empty == bankUserName || string.Empty == bankNo || string.Empty == BankLocation || string.Empty == BankOpen)
                    {
                        ajax.Message = "信息不完整";
                    }
                    else
                    {
                        var mwtList = serFinance.GetWCashBankList(int.Parse(updatekey));
                        mwtList[0].uwi003 = BankOpen;
                        mwtList[0].uwi004 = bankUserName;
                        mwtList[0].uwi005 = bankNo;
                        mwtList[0].uwi006 = BankLocation;
                        mwtList[0].uwt001 = bankCode;
                        try
                        {
                            serFinance.UpdateWCashBank(mwtList[0]);
                            ajax.Code = 1;
                        }
                        catch (Exception error)
                        {
                            ajax.Message = error.Message;
                        }
                    }
                }
                return Json(ajax);
            }
            return ViewExPath("AgentList", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChargeType(int? key)
        {
            List<DBModel.wgs010> sblist = serFinance.GetBankList();
            ViewData["SBList"] = sblist;
            DBModel.wgs009 editModel = new DBModel.wgs009();
            if (false != key.HasValue)
            {
                editModel = serFinance.GetCT((int)key);
            }
            if ("add" == MethodType || "edit" == MethodType)
            {
                ViewData["EditModel"] = editModel;
                return ViewExPath("ChargeTypeEdit", null, null);
            }
            var ctList = serFinance.GetCTList();
            return ViewExPath("ChargeType", null, ctList);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChargeType(FormCollection form)
        {
            DBModel.wgs009 entity = new DBModel.wgs009();
            if ("add" == MethodType || "edit" == MethodType)
            {
                if (TryUpdateModel(entity))
                {
                    if ("edit" == MethodType)
                    {
                        serFinance.UpdateCT(entity);
                    }
                    else
                    {
                        serFinance.AddCT(entity);
                    }
                }
            }
            return RedirectToAction("ChargeType");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AgentCharge(int key)
        {
            var agu = serUser.GetAGU(key);
            if (null == agu)
            {
                throw new Exception("要充值的用户不存在");
            }
            var ctList = serFinance.GetCTList().Where(exp => exp.ct011 == "SYS").ToList();
            ViewData["ChargetKey"] = _NWC.DEncrypt.Encrypt(agu.u001+"|"+agu.u002.Trim()+"|"+(agu.u003 != null ? agu.u003.Trim() : string.Empty));
            ViewData["UF"] = agu.wgs014;
            ViewData["CTList"] = ctList;
            return ViewExPath("AgentCharge", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AgentCharge(FormCollection form)
        {
                DBModel.wgs019 entity = new DBModel.wgs019();
                bool haveData = TryUpdateModel(entity);
                if (haveData)
                {
                    var ctObj = serFinance.GetCTList().Where(exp => exp.ct001 == entity.ct001 && exp.ct011 == "SYS").FirstOrDefault();
                    if (null == ctObj)
                    {
                        throw new Exception("充值方式存在错误");
                    }
                    string uc005 = form["uc005"];
                    uc005 = _NWC.DEncrypt.Decrypt(uc005);
                    entity.mu001 = AMLoginUser.mu001;
                    entity.mu002 = AMLoginUser.mu002.Trim();
                    entity.u001 = int.Parse(uc005.Split('|')[0]);
                    entity.u002 = uc005.Split('|')[1];
                    entity.uc006 = DateTime.Now;
                    entity.ct001 = ctObj.ct001;
                    entity.sb001 = ctObj.sb001;
                    entity.uc009 = ctObj.ct003;
                    entity.uc008 = 0;
                    entity.uc010 = Request["REMOTE_ADDR"];
                    MR result = new MR();
                    entity.uc013 = 0;
                    result = serFinance.AddCharege(entity);
                    if (0 == result.Code)
                    {
                        throw result.Exception;
                    }
                }
                else
                {
                    throw new Exception("提交数据不正确");
                }
                return RedirectToAction("AgentCharge", new { key = entity.u001 });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Charge(int? pageIndex)
        {
            if (false==pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int chrType = _NWC.GeneralValidate.IsNumber(Request["chrtype"]) ? int.Parse(Request["chrtype"]) : 0; 
            long chrKey = _NWC.GeneralValidate.IsNumber(Request["chrkey"]) ? long.Parse(Request["chrkey"]) : 0;
            string chrCkey = _NWC.GeneralValidate.IsNumber(Request["chrckey"]) ? Request["chrckey"] : string.Empty;
            var chrAccount = _NWC.GeneralValidate.IsNullOrEmpty(Request["chraccount"]) ? string.Empty : Request["chraccount"];
            var amtt = _NWC.GeneralValidate.IsNumber(Request["amtt"]) ? int.Parse(Request["amtt"]) : 0;
            var amttv = _NWC.GeneralValidate.IsDecimal(Request["amttv"]) ? decimal.Parse(Request["amttv"]) : 0m;
            var amttt = _NWC.GeneralValidate.IsNumber(Request["amttt"]) ? int.Parse(Request["amttt"]) : 0;
            var amtttv = _NWC.GeneralValidate.IsDecimal(Request["amtttv"]) ? decimal.Parse(Request["amtttv"]) : 0m;
            var amttht = _NWC.GeneralValidate.IsNumber(Request["amttht"]) ? int.Parse(Request["amttht"]) : 0;
            var amtthtv = _NWC.GeneralValidate.IsDecimal(Request["amtthtv"]) ? decimal.Parse(Request["amtthtv"]) : 0m;
            var status = _NWC.GeneralValidate.IsNumber(Request["status"]) ? int.Parse(Request["status"]) : -1;
            DateTime? dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(Request["dts"]) : DateTime.Now;
            DateTime? dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : DateTime.Now;
            ViewData["CHRType"] = chrType;
            ViewData["CHRkey"] = chrKey;
            ViewData["CHRCKey"] = chrCkey;
            ViewData["CHKAccount"] = chrAccount;
            ViewData["AMTT"] = amtt;
            ViewData["AMTTV"] = amttv;
            ViewData["AMTTT"] = amttt;
            ViewData["AMTTTV"] = amttt;
            ViewData["AMTTHT"] = amttht;
            ViewData["AMTTHTV"] = amtthtv;
            ViewData["Status"] = status;
            ViewData["DTS"] = dts;
            ViewData["DTE"] = dte;
            ViewData["CTList"] = serFinance.GetCTList().ToDictionary(exp => exp.ct001);
            ViewData["BList"] = serFinance.GetBankListByCache().ToDictionary(exp => exp.sb001);
            int recordCount = 0;
            var list = serFinance.GetChargeList(chrType, 0, chrAccount, chrKey, chrCkey, amtt, amttv, amttt, amtttv, amttht, amtthtv, status, dts,dte, PageSize, (int)pageIndex, out recordCount);
            string pageURL = Url.Action("Charge", "AM", new { chrtype = chrType, chrkey = chrKey, chrckey = chrCkey, chraccount = chrAccount, amtt = amtt, amttv = amttv, amttt = amttt, amtttv = amtttv, status = status, dts = dts, dte = dte });
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            return ViewExPath("Charge",null, list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Charge(FormCollection form)
        {
            DBModel.wgs019 entity = new DBModel.wgs019();
            if ("confirmCharge" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                ajax.Code = 0;
                bool haveData = TryUpdateModel(entity);
                int itemCancel = 0;
                if (!_NWC.GeneralValidate.IsNullOrEmpty(form["item_cancel"]))
                {
                    itemCancel = int.Parse(form["item_cancel"]);
                    if (1 != itemCancel)
                    {
                        ajax.Message = "取消数据有错";
                        return Json(ajax);
                    }
                }
                if (_NWC.GeneralValidate.IsNullOrEmpty(form["auth_key"]))
                {
                    ajax.Message = "验证数据不存在";
                    return Json(ajax);
                }
                if (haveData)
                {
                    var authKey = _NWC.DEncrypt.Decrypt(form["auth_key"]).Split('|');
                    if (0 == authKey.Length)
                    {
                        ajax.Message = "验证数据有错";
                        return Json(ajax);
                    }
                    var oldEntity = serFinance.GetCCash(int.Parse( authKey[0] ));
                    if (0 != oldEntity.uc008)
                    {
                        ajax.Message = "已经处理";
                        return Json(ajax);
                    }
                    oldEntity.u003 = entity.u003;
                    oldEntity.uc008 = 1;
                    if (1 == itemCancel)
                    {
                        oldEntity.uc008 = 2;
                    }
                    oldEntity.uc012 = entity.uc012;
                    oldEntity.uc013 = entity.uc013;
                    oldEntity.uc003 = entity.uc003;
                    oldEntity.mu001x = AMLoginUser.mu001;
                    oldEntity.mu002x = AMLoginUser.mu002;
                    MR mr = serFinance.UpdateCCash(oldEntity);
                    ajax.Code = mr.Code;
                    ajax.Message = mr.Message;
                    return Json(ajax);
                }
                else
                {
                    ajax.Code = 0;
                    ajax.Message = "提交数据有错";
                }
                return Json(ajax);
            }
            if ("edit" == MethodType)
            {
                AJAXObject ajax = new AJAXObject();
                bool haveData = TryUpdateModel(entity);
                ajax.Code = 0;
                if (haveData)
                {
                    var oldEntity = serFinance.GetCCash(entity.uc001);
                    if (null == oldEntity)
                    {
                        ajax.Message = "原始数据不存在";
                        return Json(ajax);
                    }
                    string sb003 = serFinance.GetBankListByCache().Where(exp=>exp.sb001 == oldEntity.sb001).FirstOrDefault().sb003;
                    string ct003 = serFinance.GetCT(oldEntity.ct001).ct003;
                    ajax.Code = 1;
                    ajax.Data = new { AuthKey = _NWC.DEncrypt.Encrypt(oldEntity.uc001 + "|" + oldEntity.uc005), u002 = oldEntity.u002.Trim(), uc002 = oldEntity.uc002, uc003 = oldEntity.uc003, mu002 = oldEntity.mu002.Trim(), uc006 = oldEntity.uc006, sb003 = sb003,ct003=ct003,uc005=oldEntity.uc005.Trim(),uc008=oldEntity.uc008 };
                    return Json(ajax);
                }
            }
             if (MethodType=="batchcancel")
            {
                string[] ids = form["ids"].Substring(0,form["ids"].Length-1).Split(',');
                List<long> idslong = new List<long>();
                foreach (var item in ids)
                {
                    idslong.Add(long.Parse(item));
                }
               MR mr=  serFinance.CancelCharege(idslong,AMLoginUser);
              return Json(mr);
            }
            return RedirectToAction("Charge");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Withdraw(int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int wtype = _NWC.GeneralValidate.IsNumber(Request["wtype"]) ? int.Parse(Request["wtype"]) : 0;
            long wkey = _NWC.GeneralValidate.IsNumber(Request["key"]) ? long.Parse(Request["key"]) : 0;
            var waccount = _NWC.GeneralValidate.IsNullOrEmpty(Request["waccount"]) ? string.Empty : Request["waccount"];
            var amtt = _NWC.GeneralValidate.IsNumber(Request["amtt"]) ? int.Parse(Request["amtt"]) : 0;
            var amttv = _NWC.GeneralValidate.IsDecimal(Request["amttv"]) ? decimal.Parse(Request["amttv"]) : 0m;
            var amttht = _NWC.GeneralValidate.IsNumber(Request["amttht"]) ? int.Parse(Request["amttht"]) : 0;
            var amtthtv = _NWC.GeneralValidate.IsDecimal(Request["amtthtv"]) ? decimal.Parse(Request["amtthtv"]) : 0m;
            var status = _NWC.GeneralValidate.IsNumber(Request["status"]) ? int.Parse(Request["status"]) : -1;
            DateTime? dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(Request["dts"]) : DateTime.Now;
            DateTime? dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : DateTime.Now;
            ViewData["Wtype"] = wtype;
            ViewData["Wkey"] = wkey;
            ViewData["CHKAccount"] = waccount;
            ViewData["AMTT"] = amtt;
            ViewData["AMTTV"] = amttv;
            ViewData["AMTTHT"] = amttht;
            ViewData["AMTTHTV"] = amtthtv;
            ViewData["Status"] = status;
            ViewData["DTS"] = dts;
            ViewData["DTE"] = dte;
            var wTypeList = serFinance.GetWithdrawTypeList();
            ViewData["WTypeList"] = wTypeList;
            var recordCount = 0;
            var wcList = serFinance.GetWCDataList(wtype, 0, waccount, (long)wkey, amtt, amttv, amttht, amtthtv,(int)status, dts, dte, PageSize, (int)pageIndex, out recordCount);
            string pageURL = Url.Action("Withdraw", "AM", new { wtype = wtype,waccount = waccount, key = wkey, dts = dts, dte = dte, status = status });
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            ViewData["WCList"] = wcList;
            return ViewExPath("Withdraw", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Withdraw(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            if ("handle" == MethodType)
            {
                var _wKey = form["key"];
                if (false == _NWC.GeneralValidate.IsNumber(_wKey))
                {
                    ajax.Message = "参数错误";
                    return Json(ajax);
                }
                var wcData = serFinance.GetWCData(int.Parse(_wKey));
                if (null == wcData)
                {
                    ajax.Message = "数据不存在";
                    return Json(ajax);
                }
                var authKey = _NWC.DEncrypt.Encrypt(wcData.u001+"|"+wcData.u002.Trim()+"|"+wcData.uw001);
                var bankName = wcData.uw009;
                var bankUAccount = _NWC.DEncrypt.Decrypt(wcData.uw010);
                var bankAccount = _NWC.DEncrypt.Decrypt(wcData.uw011);
                var bankOLoc = _NWC.DEncrypt.Decrypt(wcData.uw012);
                var bankLoc = _NWC.DEncrypt.Decrypt(wcData.uw017);
                var userName = wcData.u002.Trim();
                var userNickName = wcData.u003 == null ? string.Empty: wcData.u003.Trim();
                var wIP = wcData.uw007;
                var wDateTime = wcData.uw004.ToString("yyyy-MM-dd HH:mm:ss");
                var wAmount = wcData.uw002;
                var wStatus = wcData.uw006;
                var ajaxData = new { AuthKey = authKey, BankFee = wcData.uw016, BN = bankName, BUN = bankUAccount, BA = bankAccount, BOL = bankOLoc, BL = bankLoc, WAM = wAmount, Status = wStatus, MGName = AMLoginUser.mu002.Trim(), MGNickName = AMLoginUser.mu003 != null ? AMLoginUser.mu003.Trim() : string.Empty, Comment = wcData.uw008, WDT = wDateTime };
                ajax.Code = 1;
                ajax.Data = ajaxData;
                return Json(ajax);
            }
            else if ("confirmWithdraw" == MethodType)
            {
                var _authKey = form["auth_key"];
                var _itemCancel = form["item_cancel"];
                var _comment = form["comment"];
                var _fee = form["fee"];
                var status = 1;
                if (_NWC.GeneralValidate.IsNullOrEmpty(_authKey))
                {
                    ajax.Message = "验证数据有错";
                    return Json(ajax);
                }
                _authKey = _NWC.DEncrypt.Decrypt(_authKey);
                var authKyes = _authKey.Split('|');
                if (3 != authKyes.Count())
                {
                    ajax.Message = "验证数据不完整";
                    return Json(ajax);
                }
                if (_NWC.GeneralValidate.IsNumber(_fee) || _NWC.GeneralValidate.IsDecimal(_fee))
                {
                }
                else
                {
                    ajax.Message = "手续费格式不正确";
                    return Json(ajax);
                }
                if (!_NWC.GeneralValidate.IsNullOrEmpty(_itemCancel) )
                {
                    status = 2;
                }
                var mr = serFinance.SetWCData(int.Parse(authKyes[2]), status, 0, decimal.Parse(_fee), AMLoginUser.mu001, int.Parse(authKyes[0]), _comment, "", "", "");
                ajax.Message = mr.Message;
                ajax.Code = mr.Code;
                return Json(ajax);
            }
            return RedirectToAction("Withdraw");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ManagerLog(int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            var _account = string.IsNullOrEmpty(Request["account"]) ? string.Empty : Request["account"];
            var _type = _NWC.GeneralValidate.IsNumber(Request["type"]) == false ? -1 : int.Parse(Request["type"]);
            var _dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) == false ? (DateTime?)null : DateTime.Parse(Request["dts"]);
            var _dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) == false ? (DateTime?)null : DateTime.Parse(Request["dte"]);
            var _ctrl = string.IsNullOrEmpty(Request["ctrl"]) ? string.Empty : Request["ctrl"];
            var _act = string.IsNullOrEmpty(Request["act"]) ? string.Empty : Request["act"];
            var _kw = string.IsNullOrEmpty(Request["keyword"])  ? string.Empty : Request["keyword"];
            var _mtype = _NWC.GeneralValidate.IsNumber(Request["mtype"]) == false ? -1 : int.Parse(Request["mtype"]);
            if (false == _dts.HasValue)
            {
                _dts = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00");
            }
            if (false == _dte.HasValue)
            {
                _dte = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59");
            }
            ViewData["Account"] = _account;
            ViewData["Type"] = _type;
            ViewData["DTS"] = _dts;
            ViewData["DTE"] = _dte;
            ViewData["CTRL"] = _ctrl;
            ViewData["ACT"] = _act;
            ViewData["Keyword"] = _kw;
            ViewData["MType"] = _mtype;
            int recordCount = 0;
            var list = serSystem.GetLogList(_mtype,_account, _type, _ctrl, _act, _kw, _dts, _dte, PageSize, (int)pageIndex, out recordCount);
            string pageURL = Url.Action("ManagerLog", "AM", new { keyword=_kw, dts=_dts, dte=_dte, ctrl=_ctrl,act = _act,type=_type,account=_account,mtype=_mtype });
            ViewData["ReqType"] = serSystem.GetReqeustTypeI(true);
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            return ViewExPath("ManagerLog", null, list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CheckCCash(FormCollection form)
        {
            int status = -1;
            if (!_NWC.GeneralValidate.IsNullOrEmpty( form["status"] ))
            {
                status = int.Parse(form["status"]);
            }
            AJAXObject ajax = new AJAXObject();
            ajax.Data = serFinance.CheckCCashCount((int)status, 0, null, 0m, null, null);
            ajax.Code = 1;
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CheckWCash(FormCollection form)
        {
            int status = -1;
            if (!_NWC.GeneralValidate.IsNullOrEmpty(form["status"]))
            {
                status = int.Parse(form["status"]);
            }
            AJAXObject ajax = new AJAXObject();
            ajax.Data = serFinance.CheckWCashCount((int)status, 0, string.Empty, 0m, null, null);
            ajax.Code = 1;
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DataChange(int? pageIndex)
        {
            var dctList = serSystem.GetSystemDataChangeTypeList(false);
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            var _account =  _NWC.GeneralValidate.IsNullOrEmpty(Request["account"]) ? string.Empty : Request["account"];
            var _dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(Request["dts"]) : (DateTime?)null;
            var _dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : (DateTime?)null;
            var _dctype = _NWC.GeneralValidate.IsNumber(Request["dcttype"]) == false ? (byte)0 : byte.Parse(Request["dcttype"]);
            var _key = _NWC.GeneralValidate.IsNumber(Request["key"]) == false ? 0 : long.Parse(Request["key"]);
            string account = _account;
            DateTime? dts = _dts == null ? DateTime.Now : _dts;
            DateTime? dte = _dte == null ? DateTime.Now : _dte;
            byte dctype = _dctype;
            long key = _key;
            ViewData["DTS"] = dts;
            ViewData["DTE"] = dte;
            ViewData["Key"] = key;
            ViewData["Account"] = account;
            ViewData["DCTList"] = dctList;
            ViewData["DCTType"] = dctype;
            ViewData["GameList"] = serGame.GetGameListByCache();
            ViewData["GameMethodList"] = serGame.GetGameMethodListByCache();
            int recordCount = 0;
            var list = serFinance.GetDataChangeList(0, dctype, key, account, dts, dte, PageSize, (int)pageIndex, out recordCount);
            string pageURL = Url.Action("DataChange", "AM", new { account = account, dts = dts, dte = dte, dctype = dctype });
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            return ViewExPath("DataChange", null, list);
        }
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult CheckWCash(FormCollection form)
        //{
        //    return Json(new object());
        //}
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Message()
        {
            return ViewExPath("Message", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Message(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            if ("send" == MethodType)
            {
                var _type = _NWC.GeneralValidate.IsNumber(form["type"]) ? int.Parse(form["type"]) : 0;
                var _title = string.IsNullOrEmpty(form["title"]) || string.IsNullOrWhiteSpace(form["title"]) ? string.Empty : form["title"];
                var _content = string.IsNullOrEmpty(form["content"]) || string.IsNullOrWhiteSpace(form["content"]) ? string.Empty : form["content"];
                var _dt = _NWC.GeneralValidate.IsDatetime(form["dt"]) ? DateTime.Parse(form["dt"]) : DateTime.Now;
                var _users = string.IsNullOrEmpty(form["users"]) || string.IsNullOrWhiteSpace(form["users"]) ? string.Empty : form["users"];
                if (0 >= _type)
                {
                    ajax.Message = "类型不存在";
                    return Json(ajax);
                }
                if (1 == _type || 2 == _type)
                {
                    var usersSplit = _users.Split(',');
                    if (0 == usersSplit.Count() || string.IsNullOrEmpty(usersSplit[0]) || string.IsNullOrWhiteSpace(usersSplit[0]))
                    {
                        ajax.Message = "账号不能为空";
                        return Json(ajax);
                    }
                    var tr1 = serSystem.SendMessage(0, usersSplit.ToList(), _title, _content, _dt, _type);
                    ajax.Code = tr1.Code;
                    ajax.Message = tr1.Message;
                }
                else if (3 == _type)
                {
                    var tr1 = serSystem.SendMessage(0, null, _title, _content, _dt, _type);
                    ajax.Code = tr1.Code;
                    ajax.Message = tr1.Message;
                }
            }
            else
            {
                ajax.Message = "非法访问";
            }
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Report(int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            var _dts = _NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Parse(Request["dts"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 00:00:00");
            var _dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") + " 23:59:59");
            var _account = _NWC.GeneralValidate.IsNullOrEmpty(Request["account"]) ? string.Empty : Request["account"];
            ViewData["Account"] = _account;
            ViewData["DTS"] = _dts;
            ViewData["DTE"] = _dte;
            var dayReportList = serFinance.GetDayReport(0, _account, -1, (DateTime?)_dts, (DateTime?)_dte);
            ViewData["DayReportList"] = dayReportList;
            return ViewExPath("Report", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Transfer(int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int recordCount = 0;
            var tfList = serFinance.GetTransferList(0, 0, string.Empty, 0, string.Empty, 0, 0, 0, null, null, (int)pageIndex, PageSize, out recordCount);
            ViewData["TransferList"] = tfList;
            string pageURL = Url.Action("Transfer", "AM");
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            return ViewExPath("Transfer", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Transfer(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            var _key = _NWC.GeneralValidate.IsNumber(form["key"]) ? long.Parse(form["key"]) : 0;
            var _type = _NWC.GeneralValidate.IsNumber(form["type"]) ? int.Parse(form["type"]) : 0;
            var _comment = string.IsNullOrEmpty(form["comment"]) ? form["comment"] : string.Empty;
            if (0 >= _key)
            {
                ajax.Message = "编号不能为空";
            }
            else
            {
                var result = serFinance.SetTransferDone(_key, _type, AMLoginUser.mu001, AMLoginUser.mu002.Trim(), _comment);
                ajax.Code = result.Code;
                ajax.Message = result.Message;
            }
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult WithdrawType(int? key)
        {
            var editModel = new DBModel.wgs024();
            if (false == key.HasValue)
            {
                key = 0;
            }
            if ("add" == MethodType || "edit"== MethodType)
            {
                if (key != 0)
                {
                    editModel = serFinance.GetWithdrawType((int)key);
                }
                ViewData["EditModel"] = editModel;
                return ViewExPath("WithdrawTypeEdit", null, null);
            }
            ViewData["WTList"] = serFinance.GetWithdrawTypeList();
            return ViewExPath("WithdrawType", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult WithdrawType(FormCollection form)
        {
            DBModel.wgs024 entity = new DBModel.wgs024();
            bool haveData = TryUpdateModel(entity);
            if (haveData)
            {
                MR mr = new MR();
                if ("add" == MethodType)
                {
                    mr = serFinance.AddWithdrawType(entity);
                }
                else if( "edit" == MethodType)
                {
                    serFinance.UpdateWithdrawType(entity);
                }
            }
            return RedirectToAction("WithdrawType");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GamePrizeCalc()
        {
            return ViewExPath("GamePrizeCalc", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SystemCache()
        {
            ViewData["CacheMemoryLimit"] = _NWC.StringHelper.FormatNumber((ulong)System.Runtime.Caching.MemoryCache.Default.CacheMemoryLimit);
            ViewData["PhysicalMemoryLimit"] = System.Runtime.Caching.MemoryCache.Default.PhysicalMemoryLimit;
            ViewData["PollingInterval"] = System.Runtime.Caching.MemoryCache.Default.PollingInterval;
            ViewData["Name"] = System.Runtime.Caching.MemoryCache.Default.Name;
            return ViewExPath("SystemCache", null, _NWC.GeneralCache.GetCacheList().OrderBy(exp => exp.Key).ToList());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SystemCache(FormCollection form)
        {
            if (null != form["keys"])
            {
                string[] keys = form["keys"].Split(',');
                foreach (var key in keys)
                {
                    _NWC.GeneralCache.ClearLike(key);
                }
            }
            return RedirectToAction("SystemCache");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Decrypt()
        {
            return ViewExPath("Decrypt", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Decrypt(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            ajax.Code = 0;
            string key = form["key"];
            string content = form["content"];
            if (_NWC.GeneralValidate.IsNullOrEmpty(key) || _NWC.GeneralValidate.IsNullOrEmpty(content))
            {
                ajax.Message = "Key或Content不能为空！";
            }
            else
            {
                string result = string.Empty;
                try
                {
                    result = _NWC.DEncrypt.Decrypt(content, key).Replace(key, "");
                    ajax.Code = 1;
                    ajax.Data = result;
                }
                catch(Exception error)
                {
                    ajax.Code = 0;
                    ajax.Message = MyException.GetInnerException(error).Message;
                }
            }
            return Json(ajax);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ServerInfo()
        {
            Server.TransferRequest("~/WebForm/ServerInfo.aspx");
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CommonLeft()
        { 
            DBModel.wgs016 entity = new DBModel.wgs016();
            entity.mu001 = Convert.ToInt16(Session["AMLoginID"]);
            List<DBModel.wgs004> list = serMenu.GetMenuListByManage(entity); 
            list = list.Where(exp => exp.sm009 == 1 && exp.sm005 == 0 && exp.sm010 == 0).OrderBy(exp=>exp.sm012).ToList();
            return PartialViewEx("Common/Menu", list);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Notify()
        {
            ViewData["GameList"] = serGame.GetGameList();
            if (_NWC.GeneralValidate.IsNullOrEmpty(MethodType))
            {
                ViewData["MethodType"] = "finance";
                MethodType = "finance";
            }
            else
            {
                ViewData["MethodType"] = MethodType;
            }
            if ("finance" == MethodType)
            {
                var _dts = !_NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00.000" : Request["dts"];
                var _dte = !_NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59.999" : Request["dte"];
                ViewData["DTS"] = _dts;
                ViewData["DTE"] = _dte;
                var crList = serFinance.GetChargeReport(1, DateTime.Parse( _dts ), DateTime.Parse( _dte ));
                var wdList = serFinance.GetWithDrawReport(1, DateTime.Parse(_dts), DateTime.Parse(_dte));
                ViewData["WDList"] = wdList;
                ViewData["CRList"] = crList;
            }
            else if ("game" == MethodType)
            {
                var _dts = !_NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00.000" : Request["dts"];
                var _dte = !_NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59.999" : Request["dte"];
                ViewData["DTS"] = _dts;
                ViewData["DTE"] = _dte;
                var grList = serFinance.GetGameReport(DateTime.Parse(_dts), DateTime.Parse(_dte));
                ViewData["GRList"] = grList;
            }
            else if ("online" == MethodType)
            {
                var _dts = !_NWC.GeneralValidate.IsDatetime(Request["dts"]) ? DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00.000" : Request["dts"];
                var ORList = serUser.GetOnlineReport(DateTime.Parse(_dts));
                var RegList = serUser.GetRegReport(DateTime.Parse(_dts));
                ViewData["DTS"] = _dts;
                ViewData["ORList"] = ORList;
                ViewData["RegList"] = RegList;
            }
            return ViewExPath("GlobalStatus", null, null);
        }
        public ActionResult Order(int? pageIndex)
        {
            var forget = GetKV("SYS_FREE_ORDER", false).cfg003;
            List<string> forgetList = new List<string>();
            if (string.IsNullOrEmpty(forget))
            {
                forgetList.Add("______________");
            }
            else
            {
                forgetList = forget.Split(',').ToList();
            }
            ViewData["ForgetUser"] = forgetList;
            var pageURL = string.Empty;
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(MethodType))
            {
                ViewData["MethodType"] = "defaultOrder";
                MethodType = "defaultOrder";
            }
            else
            {
                ViewData["MethodType"] = MethodType;
            }
            ViewData["GameClassList"] = serGame.GetGameClassListByCache();
            ViewData["GameList"] = serGame.GetGameListByCache();
            ViewData["GameMethodList"] = serGame.GetGameMethodListByCache();
            if ("defaultOrder" == MethodType)
            { 
                var _orderDts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTS"];
                var _orderDte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTE"]; 
                var _orderUserType = Request["orderUser"];
                var _orderGameClass = Request["orderGameClass"];
                var _orderGame = Request["orderGame"];
                var _orderGameMethod = Request["orderMethod"];
                var _orderCancel = Request["orderCancel"];
                var _orderID = Request["orderID"];
                var _modes = Request["orderModes"];
                var _orderIsWin = Request["orderIsWin"];
                var _orderAmountQ = Request["orderAmountQ"];
                var _orderAmount = Request["orderAmount"];
                var _orderWinAmoutQ = Request["orderWinAmountQ"];
                var _orderWinAmount = Request["orderWinAmount"];
                var _orderIssue = Request["orderIssue"];
                var _orderUserName = Request["userName"];
                var _pageSize = _NWC.GeneralValidate.IsNumber(Request["pageSize"]) == true ? int.Parse(Request["pageSize"]) : PageSize;
                PageSizeU = _pageSize;
                ViewData["PageSize"] = PageSizeU;
                var orderDts = DateTime.Parse(_orderDts + " 00:00:00");
                var orderDte = DateTime.Parse(_orderDte + " 23:59:59");
                orderDte.AddMilliseconds(999);
                var orderUserType = !_NWC.GeneralValidate.IsNumber(_orderUserType) ? 0 : int.Parse(_orderUserType);
                var orderGameClass = !_NWC.GeneralValidate.IsNumber(_orderGameClass) ? 0 : int.Parse(_orderGameClass);
                var orderGame = !_NWC.GeneralValidate.IsNumber(_orderGame) ? 0 : int.Parse(_orderGame);
                var orderGameMethod = !_NWC.GeneralValidate.IsNumber(_orderGameMethod) ? 0 : int.Parse(_orderGameMethod);
                var orderCancel = !_NWC.GeneralValidate.IsNumber(_orderCancel) ? -1 : int.Parse(_orderCancel);
                var orderID = !_NWC.GeneralValidate.IsNumber(_orderID) ? (long)0 : long.Parse(_orderID);
                var modes = !_NWC.GeneralValidate.IsNumber(_modes) ? 0 : int.Parse(_modes);
                var orderAmount = !_NWC.GeneralValidate.IsDecimal(_orderAmount) ? -1 : decimal.Parse(_orderAmount);
                var orderAmountQ = !_NWC.GeneralValidate.IsNumber(_orderAmountQ) ? 0 : int.Parse(_orderAmountQ);
                var orderIsWin = !_NWC.GeneralValidate.IsNumber(_orderIsWin) ? 0 : int.Parse(_orderIsWin);
                var orderWinAmountQ = !_NWC.GeneralValidate.IsNumber(_orderWinAmoutQ) ? 0 : int.Parse(_orderWinAmoutQ);
                var orderWinAmount = !_NWC.GeneralValidate.IsDecimal(_orderWinAmount) ? -1 : decimal.Parse(_orderWinAmount);
                var orderIssue = string.IsNullOrEmpty(_orderIssue) ? string.Empty : _orderIssue.Trim();
                var orderUserName = string.IsNullOrEmpty(_orderUserName) ? string.Empty : _orderUserName.Trim();
                ViewData["DTS"] = orderDts.ToString("yyyy-MM-dd");
                ViewData["DTE"] = orderDte.ToString("yyyy-MM-dd");
                ViewData["OrderUserType"] = orderUserType;
                ViewData["OrderGameClass"] = orderGameClass;
                ViewData["OrderGame"] = orderGame;
                ViewData["OrderGameMethod"] = orderGameMethod;
                ViewData["OrderCancel"] = orderCancel;
                ViewData["OrderID"] = orderID;
                ViewData["OrderModes"] = modes;
                ViewData["OrderIsWin"] = orderIsWin;
                ViewData["OrderAmountQ"] = orderAmountQ;
                ViewData["OrderAmount"] = orderAmount;
                ViewData["OrderWinAmountQ"] = orderWinAmountQ;
                ViewData["OrderWinAmount"] = orderWinAmount;
                ViewData["OrderIssue"] = orderIssue;
                ViewData["OrderUserName"] = orderUserName;
                ViewData["GameClassList"] = serGame.GetGameClassListByCache();
                ViewData["GameList"] = serGame.GetGameListByCache();
                ViewData["GameMethodList"] = serGame.GetGameMethodListByCache();
                int recordCount = 0;
                var orderList = serGame.GetOrderList(orderID, orderGame, orderGameClass, orderGameMethod, 0, 0, orderUserName, orderCancel, orderIsWin, orderAmountQ, orderAmount, orderWinAmountQ, orderWinAmount, 0, orderIssue, 0, 0, modes, 0, 0, string.Empty, null, orderDts, orderDte, (int)pageIndex, PageSizeU, out recordCount);
                pageURL = Url.Action("Order", "AM", new { method = MethodType, orderDTS = ViewData["DTS"], orderDTE = ViewData["DTE"], userName = orderUserName, orderCancel = orderCancel, orderMethod = orderGameMethod, orderGame = orderGame, orderGameClass = orderGameClass, orderID = orderID, orderAmountQ = orderAmountQ, orderAmount = orderAmount, orderWinAmountQ = orderWinAmountQ, orderWinAmount = orderWinAmount, orderIsWin = orderIsWin,pageSize=PageSizeU });
                ViewData["OrderShowList"] = serGame.GetOrderShowList(orderList);
                ViewData["OrderList"] = orderList;
                ViewData["PageList"] = _NWC.PageList.GetPageList(PageSizeU, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            }
            else if ("traceOrder" == MethodType)
            { 
                var _orderDts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTS"];
                var _orderDte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTE"]; ;
                var _orderUserType = Request["orderUser"];
                var _orderGameClass = Request["orderGameClass"];
                var _orderGame = Request["orderGame"];
                var _orderGameMethod = Request["orderMethod"];
                var _orderStatus = Request["orderStatus"];
                var _orderID = Request["orderID"];
                var _modes = Request["orderModes"];
                var _orderUserName = Request["orderUserName"];
                var orderDts = DateTime.Parse(_orderDts + " 00:00:00");
                var orderDte = DateTime.Parse(_orderDte + " 23:59:50");
                orderDte.AddMilliseconds(999);
                var orderUserType = !_NWC.GeneralValidate.IsNumber(_orderUserType) ? 0 : int.Parse(_orderUserType);
                var orderGameClass = !_NWC.GeneralValidate.IsNumber(_orderGameClass) ? 0 : int.Parse(_orderGameClass);
                var orderGame = !_NWC.GeneralValidate.IsNumber(_orderGame) ? 0 : int.Parse(_orderGame);
                var orderGameMethod = !_NWC.GeneralValidate.IsNumber(_orderGameMethod) ? 0 : int.Parse(_orderGameMethod);
                var orderStatus = !_NWC.GeneralValidate.IsNumber(_orderStatus) ? -1 : int.Parse(_orderStatus);
                var orderID = !_NWC.GeneralValidate.IsNumber(_orderID) ? (long)0 : long.Parse(_orderID);
                var modes = !_NWC.GeneralValidate.IsNumber(_modes) ? 0 : int.Parse(_modes);
                var orderUserName = string.IsNullOrEmpty(_orderUserName) || string.IsNullOrWhiteSpace(_orderUserName) ? string.Empty : _orderUserName;
                ViewData["DTS"] = orderDts.ToString("yyyy-MM-dd");
                ViewData["DTE"] = orderDte.ToString("yyyy-MM-dd");
                ViewData["OrderUserType"] = orderUserType;
                ViewData["OrderGameClass"] = orderGameClass;
                ViewData["OrderGame"] = orderGame;
                ViewData["OrderGameMethod"] = orderGameMethod;
                ViewData["OrderStatus"] = orderStatus;
                ViewData["OrderID"] = orderID;
                ViewData["OrderModes"] = modes;
                ViewData["OrderUserName"] = orderUserName;
                int recordCount = 0;
                var tOrderList = serGame.GetTOrderList(orderID, orderGame, orderGameClass, string.Empty, 0, 0, orderUserName, orderDts, orderDte, (int)pageIndex, PageSize, out recordCount);
                pageURL = Url.Action("Order", "AM", new { method = MethodType, orderDTS = ViewData["DTS"], orderDTE = ViewData["DTE"], orderUser = orderUserType, orderStatus = orderStatus, orderMethod = orderGameMethod, orderGame = orderGame, orderGameClass = orderGameClass, orderID = orderID, orderUserName = orderUserName });
                ViewData["TOrderList"] = tOrderList;
                ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            }
            else if ("traceOrderDetail" == MethodType)
            {
                int recordCount = 0;
                var _traceKey = _NWC.GeneralValidate.IsNumber(Request["key"]) == false ? 0 : long.Parse(Request["key"]);
                var traceObject = serGame.GetTOrderItem(_traceKey, 0, 0, 0, (int)pageIndex, PageSize, out recordCount);
                ViewData["TraceObject"] = traceObject;
                ViewData["GameList"] = serGame.GetGameListByCache();
                pageURL = Url.Action("Order", "AM", new { method = MethodType, key = _traceKey });
                ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
                return ViewExPath("OrderTraceDetail", null, null);
            }
            else if ("combineOrder" == MethodType)
            {
                var _orderDts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTS"];
                var _orderDte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTE"];
                var _orderGame = _NWC.GeneralValidate.IsNumber(Request["orderGame"]) ? Request["orderGame"] : "0";
                var _issue = string.IsNullOrEmpty(Request["issue"]) || string.IsNullOrWhiteSpace(Request["issue"]) ? string.Empty : Request["issue"];
                var _account = string.IsNullOrEmpty(Request["account"]) || string.IsNullOrWhiteSpace(Request["account"]) ? string.Empty : Request["account"];
                var orderDts = DateTime.Parse(_orderDts + " 00:00:00");
                var orderDte = DateTime.Parse(_orderDte + " 23:59:59");
                orderDte.AddMilliseconds(999);
                var orderGame = int.Parse(_orderGame);
                var issue = _issue;
                var account = _account;
                ViewData["DTS"] = orderDts.ToString("yyyy-MM-dd");
                ViewData["DTE"] = orderDte.ToString("yyyy-MM-dd");
                ViewData["OrderGame"] = orderGame;
                ViewData["Issue"] = issue;
                ViewData["account"] = account;
                ViewData["GameClassList"] = serGame.GetGameClassListByCache();
                ViewData["GameList"] = serGame.GetGameListByCache();
                int recordCount = 0;
                var combuyList = serGame.GetCombuyList(0, account, orderGame, 0, issue, -1, (int)pageIndex, PageSize, null, null, out recordCount);
                ViewData["CombuyList"] = combuyList;
                pageURL = Url.Action("Record", "UI", new { method = MethodType, orderDTS = ViewData["DTS"], orderDTE = ViewData["DTE"], orderGame = orderGame, issue = issue });
                ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            }
            return ViewExPath("Order", "", null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Order(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            if (_NWC.GeneralValidate.IsNullOrEmpty(MethodType))
            {
                ViewData["MethodType"] = "defaultOrder";
                MethodType = "defaultOrder";
            }
            else
            {
                ViewData["MethodType"] = MethodType;
            }
            if ("cancel" == MethodType || "delete" == MethodType)
            {
                var _ids = _NWC.GeneralValidate.IsNullOrEmpty(form["id"]) ? string.Empty : form["id"];
                var sids = _ids.Split(',');
                List<long> ids = new List<long>();
                foreach (var id in sids)
                {
                    try
                    {
                        ids.Add(long.Parse(id));
                    }
                    catch { }
                }
                var cancelType = 0;
                switch (MethodType)
                {
                    case "cancel":
                        cancelType = 2;
                        break;
                    case "delete":
                        cancelType = 4;
                        break;
                    default:
                        cancelType = -1;
                        break;
                }
                var result = serGame.CancelOrder(cancelType, ids, -1, AMLoginUser.mu001, AMLoginUser.mu002.Trim());
                ajax.Code = result.Code;
                if (0 == ajax.Code)
                {
                    ajax.Message = result.Message;
                }
                return Json(ajax);
            }
            else if ("cancelTOrder" == MethodType)
            {
                var otids = _NWC.GeneralValidate.IsNullOrEmpty(form["id"]) ? string.Empty : form["id"];
                if (true == string.IsNullOrEmpty(otids))
                {
                    ajax.Message = "追号单编号为空";
                    return Json(ajax);
                }
                List<long> otidList = new List<long>();
                var idSplit = otids.Split(',');
                foreach (var id in idSplit)
                {
                    try
                    {
                        otidList.Add(long.Parse(id));
                    }
                    catch
                    { 
                    }
                }
                if (0 == otidList.Count)
                {
                    ajax.Message = "追号单编号为空";
                    return Json(ajax);
                }
                MR mr = serGame.CancelTOrder(otidList, 2, 0, AMLoginUser.mu001, AMLoginUser.mu002.Trim());
                ajax.Code = mr.Code;
                ajax.Message = mr.Message;
                return Json(ajax);
            }
            else if ("manageOrder" == MethodType)
            {
                var _dop = string.IsNullOrEmpty(form["r_op"]) || string.IsNullOrWhiteSpace(form["r_op"]) ? string.Empty : form["r_op"];
                var _doType = string.IsNullOrEmpty(form["r_op_type"]) || string.IsNullOrWhiteSpace(form["r_op_type"]) ? string.Empty : form["r_op_type"];
                var _doContent = string.IsNullOrEmpty(form["r_content"]) || string.IsNullOrWhiteSpace(form["r_content"]) ? string.Empty : form["r_content"];
                if (string.IsNullOrEmpty(_dop) || string.IsNullOrEmpty(_doType) || string.IsNullOrEmpty(_doContent))
                {
                    ajax.Message = "数据不完整";
                    return Json(ajax);
                }
                var result = serGame.ManageOrder(_dop, _doType, _doContent, AMLoginUser.mu001, AMLoginUser.mu002.Trim());
                ajax.Code = result.Code;
                ajax.Message = result.Message;
                return Json(ajax);
            }
            return ViewExPath("Order", string.Empty, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Login()
        {
            return ViewExPath("Login", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GlobalNotify(int? key)
        {
            ViewData["MethodType"] = MethodType;
            if (false == key.HasValue)
            {
                key = 0;
            }
            if ("edit" == MethodType && 0 != key)
            {
                ViewData["EditModel"] = serSystem.GetNotify((int)key);
            }
            else if ("delete" == MethodType && 0 != key)
            {
                serSystem.DeleteNotify((int)key);
                MethodType = string.Empty;
                ViewData["MethodType"] = string.Empty;
            }
            var notifyList = serSystem.GetNotifyList(-1);
            ViewData["NotifyList"] = notifyList;
            return ViewExPath("GlobalNotify", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult GlobalNotify(FormCollection form)
        {
            DBModel.wgs040 entity = new DBModel.wgs040();
            if (TryUpdateModel(entity))
            {
                if (0 == entity.nt001)
                {
                    serSystem.AddNotify(entity);
                }
                else if (0 < entity.nt001)
                {
                    serSystem.UpdateNotify(entity);
                }
            }
            return RedirectToAction("GlobalNotify");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SysContent(int? key, int? classID)
        {
            if (false == classID.HasValue || 0 >= classID)
            {
                classID = 1;
            }
            if (false == key.HasValue)
            {
                key = 0;
            }
            var sysCntClsList = serSystem.GetSystemContentClass();
            ViewData["SysCntClsList"] = sysCntClsList;
            if ("delete" == MethodType && 0 != key)
            {
                serSystem.DeleteSysCnt((int)key);
            }
            else if ("edit" == MethodType && 0 != key)
            {
                ViewData["EditModel"] = serSystem.GetSysCnt((int)key);
            }
            ViewData["SysCntList"] = serSystem.GetSysCntList((int)classID, -1);
            ViewData["ClassID"] = classID;
            return ViewExPath("SysContent", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult SysContent(FormCollection form)
        {
            DBModel.wgs041 entity = new DBModel.wgs041();
            if (TryUpdateModel(entity))
            {
                if (0 == entity.nc001)
                {
                    serSystem.AddSysCnt(entity);
                }
                else if (0 < entity.nc001)
                {
                    serSystem.UpdateSysCnt(entity);
                }
            }
            return RedirectToAction("SysContent", new { classID=entity == null ? 1 : entity.nc007});
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GameResult(int?gameID, int? pageIndex)
        {
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            int recordCount = 0;
            List<DBModel.wgs001> gameList = serGame.GetGameList();
            if (false == gameID.HasValue)
            {
                gameID = gameList.FirstOrDefault().g001;
            }
            ViewData["GameID"] = gameID;
            ViewData["GameList"] = gameList;
            var gameResultList = serGame.GetGameResultList((int)gameID, -1, -1,(int)pageIndex, PageSize, out recordCount);
            string pageURL = Url.Action("GameResult", "AM", new { gameID = gameID });
            ViewData["GameResultList"] = gameResultList;
            ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            return ViewExPath("GameResult", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GameResult(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            string _type = string.IsNullOrEmpty(form["type"]) ? string.Empty : form["type"];
            string _ids = _NWC.GeneralValidate.IsNumber(form["id"]) ? form["id"] : string.Empty;
            if ("edit" == MethodType && "get"==_type)
            {
                DBModel.wgs038 getItem = null;
                try
                {
                    getItem = serGame.GetGameResult(long.Parse(_ids), 0);
                }
                catch (Exception error)
                {
                    ajax.Message = error.Message;
                }
                finally
                {
                    if (null != getItem)
                    {
                        getItem.gs002 = string.IsNullOrEmpty(getItem.gs002) ? string.Empty : getItem.gs002.Trim();
                        getItem.gs007 = string.IsNullOrEmpty(getItem.gs007) ? string.Empty : getItem.gs007.Trim();
                        ajax.Data = getItem;
                        ajax.Code = 1;
                    }
                    else
                    {
                        ajax.Message = "数据不存在";
                    }
                }
                return Json(ajax);
            }
            else if ("edit" == MethodType && "get" !=_type)
            {
                DBModel.wgs038 updateEntity = new DBModel.wgs038();
                if (TryUpdateModel(updateEntity))
                {
                    var updateResult = serGame.UpdateGameResult(updateEntity);
                    ajax.Message = updateResult.Message;
                    ajax.Code = updateResult.Code;
                    return Json(ajax);
                }
                ajax.Code = 0;
                ajax.Message = "提交数据不正确";
                return Json(ajax);
            }
            else if ("deleteList" == MethodType)
            {
                List<long> ids = new List<long>();
                var idsSplit = _ids.Split(',');
                foreach (var id in idsSplit)
                {
                    try
                    {
                        ids.Add(long.Parse(id));
                    }
                    catch (Exception error)
                    {
                        ajax.Message = error.Message;
                    }
                }
                if (0 == ids.Count)
                {
                    ajax.Message = "没有选中要删除的数据";
                }
                else
                {
                    var deleteResult = serGame.DeleteGameResult(ids, 0);
                    ajax.Code = deleteResult.Code;
                    ajax.Message = deleteResult.Message;
                }
                return Json(ajax);
            }
            return null;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(FormCollection form)
        {
            string account = form["a"].Trim();
            string password = form["p"].Trim();
            int enable = int.Parse(GetKV("AM_VCODE_ENABLE", false).cfg003);
            string vcode = _NWC.GeneralValidate.IsNullOrEmpty(form["vc"]) == true ? "VCODE_ERROR" : form["vc"];
            bool aIN = _NWC.GeneralValidate.IsNullOrEmpty(account);
            bool pIN = _NWC.GeneralValidate.IsNullOrEmpty(password);
            string sysVCode = (string)Session["VCode"];
            if (0 != string.Compare(sysVCode, vcode, true) && 1 == enable)
            {
                ViewData["Error"] += "验证码不正确";
            }
            else if (aIN || pIN)
            {
                ViewData["Error"] += "账号和密码不能为空";
            }
            else
            {
                var mg = serUser.GetMGU(account);
                if (null == mg)
                {
                    ViewData["Error"] = "账号密码错误，请改正后再登录";
                }
                else
                {
                    var newPassword = _NWC.SHA1.Get(password + mg.mu005, _NWC.SHA1Bit.L160);
                    if (newPassword.ToLower() != mg.mu004.TrimEnd().ToLower())
                    {
                        ViewData["Error"] = "账号密码错误，请改正后再登录";
                    }
                   else if (1 != mg.mu006)
                    {
                        ViewData["Error"] = "账号被停用，请向管理员咨询";
                    }
                    else
                    {
                        int cookieTime = _NWC.ConfigHelper.GetConfigInt("CJLSoftNormalCookieTime");
                        HttpCookie loginUserCookie = new HttpCookie("AMContent");
                        loginUserCookie.Expires = DateTime.Now.AddMinutes(cookieTime == 0 ? 30 : cookieTime);
                        loginUserCookie.Values.Add("MGInfo", _NWC.DEncrypt.Encrypt(mg.mu001 + "|" + mg.mu002.Trim() + "|" + mg.pg001));
                        Response.Cookies.Set(loginUserCookie);
                        Session["AMLoginUser"] = mg;
                        if (1 != mg.mu001)
                        {
                            var pgItem = serSystem.GetBPGList().Where(exp => exp.pg001 == mg.pg001).FirstOrDefault();
                            if (null != pgItem && 0 != pgItem.pg005.Split(',').Length )
                            {
                               Session["PGIDs"] = pgItem.pg005.Split(',');
                            }
                            else
                            {
                                Session["PGIDs"] = "0".Split(',');
                            }
                        }
                        Session["AMLoginID"] = mg.mu001;
                        Session["AMLoginAccount"] = mg.mu002.Trim();
                        Session["AMLoginDateTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        Session["AMLoginIP"] = _NWC.RequestHelper.GetUserIP(Request);
                        return RedirectToAction("Index", "AM");
                    }
                }
            }
            return RedirectToAction("Login");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Logout()
        {
            Session["AMLoginUser"] = null;
            Session.Abandon();
            HttpCookie loginUserCookie = new HttpCookie("AMContent");
            loginUserCookie.Expires = DateTime.Now.AddDays(-365);
            loginUserCookie.Values.Remove("MGInfo");
            Response.Cookies.Add(loginUserCookie);
            return RedirectToAction("Login");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DBTInfo()
        {
            var tableSizeList = serSystem.GetSysTableSize();
            ViewData["TSList"] = tableSizeList;
            return ViewExPath("DBTInfo", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult DBSQL()
        {
            return ViewExPath("DBSQL", null, null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DBSQL(FormCollection form)
        {
            var _auth = form["auth"];
            var _sql = form["sql"];
            ViewData["Message"] = serSystem.RunSQL(_sql, string.Empty, _auth);
            return ViewExPath("DBSQL", null, null);
        }
        public ActionResult MemberCheck()
        {
            var _acct = string.IsNullOrEmpty( Request["acct"] ) ? string.Empty : Request["acct"];
            var _dts = _NWC.GeneralValidate.IsDatetime( Request["dts"] ) ? DateTime.Parse(Request["dts"]) : DateTime.Now;
            var _dte = _NWC.GeneralValidate.IsDatetime(Request["dte"]) ? DateTime.Parse(Request["dte"]) : DateTime.Now;
            ViewData["Show"] = 0;
            ViewData["DTS"] = _dts;
            ViewData["DTE"] = _dte;
            ViewData["Acct"] = _acct;
            if (false == string.IsNullOrEmpty(_acct))
            {
                int clRC = 0;
                int wcRC = 0;
                int oRC = 0;
                var user = serUser.GetAGU(_acct);
                if (null != user)
                {
                    ViewData["Show"] = 1;
                    var cList = serFinance.GetChargeList(0, user.u001, string.Empty, 0, string.Empty, 0, 0, 0, 0, 0, 0, 1, _dts, _dte, 1000000, 0, out clRC);
                    var wcList = serFinance.GetWCDataList(0, user.u001, string.Empty, 0, 0, 0, 0, 0, 1, _dts, _dte, 1000000, 0, out wcRC);
                    var oList = serGame.GetOrderList(0, 0, 0, 0, user.u001, 0, string.Empty, -1, 0, 0, 0, 0, 0, 0, string.Empty, 0, 0, 0, 0, 0, string.Empty, null, _dts, _dte, 0, 1000000, out oRC);
                    ViewData["User"] = user;
                    ViewData["CList"] = cList;
                    ViewData["WCList"] = wcList;
                    ViewData["OList"] = serGame.GetOrderShowList(oList);
                    ViewData["CLRC"] = clRC;
                    ViewData["WCRC"] = wcRC;
                    ViewData["ORC"] = oRC;
                }
                else
                {
                    ViewData["Show"] = 0;
                }
            }
            return ViewExPath("MemberCheck", null, null);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SysReg()
        {
            return ViewExPath("SysReg", null, null);
        }
        public ActionResult P1()
        {
            return Content("P1","text/html");
        }
        public ActionResult P2()
        {
            return Content("P2", "text/html");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public FileContentResult VCode()
        {
            Random rand = new Random();
            var codeConfig = GetKV("AM_VCODE_CONFIG", false).cfg003.Split(',');
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
            HttpContext.Session["VCode"] = strVCode;
#if DEBUG
            //System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\Temp\TempVCode\" + strVCode);
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
        public ActionResult ShopClass()
        {
            ViewData["ModelList"] = serSystem.GetShopClassAllList().OrderBy(p => p.rc003).ToList();
            return ViewEx();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShopClass(List<DBModel.wgs032> entityList,DBModel.wgs032 entity)
        {
            if (MethodType.ToLower()=="add")
            {
               return Json(serSystem.AddShopClass(entity));
            }
            else
            {
                serSystem.UpdateShopClass(entityList);
                ViewData["ModelList"] = serSystem.GetShopClassAllList().OrderBy(p=>p.rc003).ToList();
            }
            ViewData["MethodType"] = MethodType;
            return ViewEx();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ShopProduct(int shopClassId=1)
        {
            ViewData["shopClassList"] = serSystem.GetShopClassAllList().OrderBy(p => p.rc003).ToList();
            ViewData["shopProductList"] = serSystem.GetShopProductList(shopClassId).OrderBy(p => p.r008).ToList();
            return ViewEx();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShopProduct(DBModel.wgs033 entity)
        {
            MR mr = new MR();
            mr.Code = 1;
            ViewData["shopClassList"] = serSystem.GetShopClassAllList().OrderBy(p => p.rc003).ToList();
            if (MethodType.ToLower()=="add")
            {
                HttpPostedFileBase picture = Request.Files[0];
                string saveFileName = string.Empty;
                try
                {
                    saveFileName = @"\images\shop\" + Guid.NewGuid().ToString() + picture.FileName.Substring(picture.FileName.IndexOf('.'));
                    picture.SaveAs(Server.MapPath(saveFileName));
                    entity.r006 = saveFileName.Replace(@"\images\shop\","");
                    mr = serSystem.AddShopProduct(entity);
                }
                catch (Exception e)
                {
                    mr.Code = 0;
                    mr.Message = e.Message;
                }
                return Json(mr);
            }
            if (MethodType.ToLower() == "delete")
            {
                var oldImgPath = Server.MapPath(@"\images\shop\" + serSystem.GetShopProduct(int.Parse(Request.Form["id"])).r006);
               mr= serSystem.DeleteShopProduct(int.Parse(Request.Form["id"]));
               if (mr.Code == 1)
               {
                   if (System.IO.File.Exists(oldImgPath))
                   {
                       System.IO.File.Delete(oldImgPath);
                   }
               }
               return Json(mr);
            }
            if (MethodType.ToLower() == "update")
            {
                if (Request.Files.Count==0)
                {
                    entity.r006 = serSystem.GetShopProduct(entity.r001).r006;
                }
                else
                {
                    try
                    {
                        string saveFileName = string.Empty;
                        HttpPostedFileBase picture = Request.Files[0];
                        saveFileName = @"\images\shop\" + Guid.NewGuid().ToString() + picture.FileName.Substring(picture.FileName.IndexOf('.'));
                        picture.SaveAs(Server.MapPath(saveFileName));
                        entity.r006 = saveFileName.Replace(@"\images\shop\", ""); 
                    }
                    catch (Exception e)
                    {
                        mr.Code = 0;
                        mr.Message = e.Message;
                    }
                }
                if (mr.Code == 1)
                {
                    var oldImgPath = Server.MapPath(@"\images\shop\" + serSystem.GetShopProduct(entity.r001).r006);
                    mr = serSystem.UpdateShopProduct(entity);
                    if (mr.Code == 1&&Request.Files.Count!=0)
                    {
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }
                }
                return Json(mr);
            }
            if (MethodType.ToLower() == "get")
            {
                return Json(serSystem.GetShopProduct(int.Parse(Request.Form["key"])));
            }
            mr.Code = 0;
            mr.Message = "出错了";
            return Json(mr);
        }
        [HttpGet]
        public ActionResult ShopRecord(int? status, int pageIndex = 0, int pageSize = 25, int pageBlockSize = 5)
        {
            int recordCount;
            ViewData["shopRecordList"] = serSystem.GetShopRecordList(null, null,pageIndex,pageSize,out recordCount);
            ViewData["PageList"] = _NWC.PageList.GetPageList(pageSize,recordCount,pageIndex,pageBlockSize,"page_list_block", "pageIndex","/Am/ShopRecord");
            return ViewEx();
        }
        [HttpPost]
        public ActionResult ShopRecord(int recordId, int status, string streamCompany="", string searchUrl="", string num="",string why="")
        {
            MR mr = new MR();
            mr = serSystem.ProsessShopRecord(recordId, status, streamCompany, searchUrl, num, why);
            return Json(mr);
        }
        public ActionResult CommissionList(int? pageIndex)
        {
            var pageURL = string.Empty;
            if (false == pageIndex.HasValue)
            {
                pageIndex = 0;
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(MethodType))
            {
                ViewData["MethodType"] = "defaultOrder";
                MethodType = "defaultOrder";
            }
            else
            {
                ViewData["MethodType"] = MethodType;
            }

            if ("defaultOrder" == MethodType)
            {
                var _orderDts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTS"];
                var _orderDte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTE"];
                DateTime orderDts = DateTime.Parse(_orderDts);
                DateTime orderDte = orderDts.AddDays(+1);
                ViewData["DTS"] = orderDts.ToString("yyyy-MM-dd");
                ViewData["DTE"] = orderDte.ToString("yyyy-MM-dd");
                if (orderDts.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    DBModel.wgs027 onAccount = GetKV("SYS_NOTSET_COMMISSIONACCOUNT", true);
                    ViewData["OrderDay"] = serGame.GetOrderDayAccSumMoney( onAccount,orderDts, orderDte);
                }
            }
            else if ("CommissionList" == MethodType)
            {
                var _Dts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTS"];
                var _Dte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTE"]; ;
                int recordCount = 0;
                var _UserName = Request["UserName"];
                var Dts = DateTime.Parse(_Dts + " 00:00:00.0000000");
                var Dte = DateTime.Parse(_Dte + " 23:59:59.9999999");
                var UserName = string.IsNullOrEmpty(_UserName) || string.IsNullOrWhiteSpace(_UserName) ? string.Empty : _UserName;
                ViewData["DTS"] = Dts.ToString("yyyy-MM-dd");
                ViewData["DTE"] = Dte.ToString("yyyy-MM-dd");
                ViewData["UserName"] = UserName;
                ViewData["CommissionList"] = serGame.CommissionList(UserName, Dts, Dte, (int)pageIndex, PageBloclSize, out recordCount);
                pageURL = Url.Action("CommissionList", "AM", new { method = MethodType, orderDTS = ViewData["DTS"], orderDTE = ViewData["DTE"], userName =UserName,  pageSize = PageSizeU });
                ViewData["PageList"] = _NWC.PageList.GetPageList(PageSize, recordCount, (int)pageIndex, PageBloclSize, "page_list_block", "pageIndex", pageURL);
            }
            else if ("SendMessage" == MethodType)
            {
                var _Dts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTS"];
                var _Dte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTE"]; ;
                int recordCount = 0;

                var _UserName = Request["UserName"];
                var Dts = DateTime.Parse(_Dts + " 00:00:00.0000000");
                //var Dte = DateTime.Parse(Dts. + " 23:59:59.9999999");
                var UserName = string.IsNullOrEmpty(_UserName) || string.IsNullOrWhiteSpace(_UserName) ? string.Empty : _UserName;
                ViewData["DTS"] = Dts.ToString("yyyy-MM-dd");
                //ViewData["DTE"] = Dte.ToString("yyyy-MM-dd");
                ViewData["UserName"] = UserName;
                ViewData["CDDaySendMessage"] = serGame.GetCommissionList(Dts);
                //List<DBModel.CommissionDaySendMessage> DaySendMessage = serGame.CommissionDaySendMessage(Dts, Dte);
                //ViewData["CDDaySendMessage"] = DaySendMessage.Distinct();
            }
            return ViewExPath("CommissionList", "", null);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CommissionList(FormCollection form)
        {
            AJAXObject ajax = new AJAXObject();
            var data = _NWC.GeneralValidate.IsNullOrEmpty(form["data"]) ? string.Empty : form["data"];
            var _orderDts = !_NWC.GeneralValidate.IsDatetime(Request["orderDTS"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTS"];
            var _orderDte = !_NWC.GeneralValidate.IsDatetime(Request["orderDTE"]) ? DateTime.Now.ToString("yyyy-MM-dd") : Request["orderDTE"];
            if (true == string.IsNullOrEmpty(data))
            {
                ajax.Message = "数据为空";
                return Json(ajax);
            }
            if ("defaultOrder" == MethodType)
            {
                DateTime orderDts = DateTime.Parse(_orderDts);
                DateTime orderDte = orderDts.AddDays(+1);
                DBModel.wgs027 commission = GetKV("SYS_COMMISSION", true);
                MR mr = serGame.CommissionOrder(data, commission, AMLoginUser.mu001, AMLoginUser.mu002.Trim(), orderDts, orderDte);
                ajax.Code = mr.Code;
                ajax.Message = mr.Message;
                return Json(ajax);
                
            }
            else if ("SendMessage" == MethodType)
            {
                DateTime orderDts = DateTime.Parse(_orderDts + " 00:00:00.0000000");
                DateTime orderDte = DateTime.Parse(_orderDte + " 23:59:59.9999999");
                MR mr = serGame.CommissionDaySendMessage(data, AMLoginUser.mu002.Trim(), orderDts, orderDte);
                ajax.Code = mr.Code;
                ajax.Message = mr.Message;
                return Json(ajax);
            }
            return ViewExPath("CommissionList", string.Empty, null);
        }

        public static bool IsHaveRepeatIp(string ip) {
       
            int recordCount = 0;
            ISystem serSystem = new GameServices.System();
            serSystem.GetOnlineList(0, string.Empty, ip, string.Empty, -1, 0, 0, out recordCount);
            return recordCount>1;
        }
    }
}
