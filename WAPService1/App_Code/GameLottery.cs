using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using GameServices;
using _NWC = NETCommon;
using Newtonsoft.Json;
/// <summary>
/// GameLottery 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class GameLottery : System.Web.Services.WebService {

    public GameLottery () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }
    protected GameServices.ISystem serSystem = new GameServices.System();
    protected Dictionary<string, DBModel.wgs027> DicKV
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
    protected DBModel.wgs027 GetKV(string key, bool cache)
    {
        if (cache)
        {
            return DicKV[key];
        }
        var result = serSystem.GetKeyValue(key);
        return result;
    }
    [WebMethod]
    public ObjMsg GetIndex(int UserID, string SCode, int? gameClassID, int? gameID) 
    {
        GameServices.IGame serGame = new GameServices.Game();
        Lotterys ob = new Lotterys();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, UserID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        var settingList = GetKV("GAME_TEMP_ID", false).cfg003.Split(',');
        var tmpList = GetKV("AGU_GAME_TMP", false).cfg003.Split(',');
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
            ob.Code = "0001";
            ob.Message = "游戏不存在";
            return ob;
        }
        if (null == gameClassItem)
        {
            ob.Code = "0001";
            ob.Message = "游戏分类错误";
            return ob;
        }
        if (false == gameID.HasValue || false == gameClassID.HasValue)
        {
            ob.Code = "0001";
            ob.Message = "非法访问";
            return ob;
        }
        var includeGames = gameClassItem.gc004.Split(',');
        if (0 == includeGames.Count(exp => exp == gameItem.g001.ToString()))
        {
            ob.Code = "0001";
            ob.Message = "游戏不存在此分类";
            return ob;
        }
        if (0 == Convert.ToByte(gameItem.g010))
        {
            ob.Code = "0001";
            ob.Message =DicKV["SYS_GAME_DISENABLE"].cfg003.Trim();
            return ob;
        }
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
        var newGameSessionList = new List<LotteryAll>();
        var gsJSONData = string.Empty;
        foreach (var gsItem in gameSessionList)
        {
            LotteryAll lotteryAll = new LotteryAll();
            if (gameItem.g001 == 20 || gameItem.g001 == 26)
            {
                lotteryAll.IGameName = gameClassItem.gc002;
                lotteryAll.IGameID = gsItem.g001;
                lotteryAll.IISOpen = 1;
                lotteryAll.IGameIssue = gsItem.gs002.Trim();
                lotteryAll.ICloseTime = gsItem.gs004;
                newGameSessionList.Add(lotteryAll);
            }
            else
            {
                lotteryAll.IGameName = gameClassItem.gc002;
                lotteryAll.IGameID = gsItem.g001;
                lotteryAll.IISOpen = 1;
                lotteryAll.IGameIssue = gsItem.gs002.Trim();
                lotteryAll.starttime=gsItem.gs003;
                lotteryAll.ICloseTime = gsItem.gs004;
                newGameSessionList.Add(lotteryAll);
            }
        }
      
        //LotteryAll
        //ViewData["GSList"] = gsJSONData;
        var cgsJSONData = string.Empty;
        var currentGameSession = serGame.GetCurrentGameSession((int)gameID);
        LotteryCurrent lotteryCurrent = new LotteryCurrent();
        if (null != currentGameSession)
        {
            lotteryCurrent.IGameName = gameClassItem.gc002;
            lotteryCurrent.IGameID = currentGameSession.g001;
            lotteryCurrent.IISOpen = 1;
            lotteryCurrent.IGameIssue = currentGameSession.gs002.Trim();
            lotteryCurrent.starttime = currentGameSession.gs005;
            lotteryCurrent.ICloseTime = currentGameSession.gs004;
        }
        else
        {
            lotteryCurrent.IISOpen =0;
            //return Json(new { code = 0, msg = DicKV["SYS_GAMESESSION_NULL"].cfg003.Trim() });
        }
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        ob.LotteryAllList = newGameSessionList;
        ob.LotteryCurrent = lotteryCurrent;
        return ob;
    }

    [WebMethod]
    public ObjMsg GetLotteryLog(int UserID, int GameID,int TopCount,string SCode)
    {
        GameServices.IGame serGame = new GameServices.Game();
        Lotterys ob = new Lotterys();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, UserID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        var gsrList = serGame.GetGameSessionResultList(0, GameID, string.Empty, null, null, TopCount);
        List<LotteryLog> LotteryLogList = new List<LotteryLog>();
        foreach(var item in gsrList)
        {
            LotteryLog lotteryLog = new LotteryLog();
            lotteryLog.GIssue = item.gs002.Trim();
            lotteryLog.GResult = item.gs007.Trim();
            LotteryLogList.Add(lotteryLog);
        } 
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        ob.LotteryLogList = LotteryLogList;
        return ob;
    }

    [WebMethod]
    public ObjMsg PostEveryColor(string Account, int UserID, string SCode, int GameID, string GLssue, int GGameClassID, int GMultiple, int GModel, int GMoney, string GTxt, String GBonus, string[] lt_project, string[] lt_trace_issues, int lt_trace_times_)
    {
        GameServices.IGame serGame = new GameServices.Game();
        GameServices.IUser serUser = new GameServices.User();
        ObjMsg ob = new ObjMsg();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, UserID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        int userStatus = serUser.GetAGUStatus(UserID);
            if (3 == userStatus || 2 == userStatus)
            {
                var stateMsg = userStatus == 2 ? "暂停" : "冻结";
                ob.Code = "0001";
                ob.Message = "账号状态" + stateMsg + "，不能下单";
                ob.Scode = newScode;
                return ob;
            }
            DBModel.LotteryOrder lotteryOrder = new DBModel.LotteryOrder();
            lotteryOrder.gc001 = GameID;
            lotteryOrder.lotteryid = GGameClassID;
            lotteryOrder.UserID = UserID;
            lotteryOrder.lt_combuy_check = 0;//等于1位合买
            lotteryOrder.lt_trace_if = "";
            lotteryOrder.lt_issue_start = GLssue;
            lotteryOrder.lt_sel_modes = GModel;
            lotteryOrder.lt_total_money = GMoney;
            lotteryOrder.lt_sel_dyprize = GBonus;
            var gameClassList = serGame.GetGameClassListByCache();
            var gameList = serGame.GetGameListByCache();
            var gameMethodGroup = serGame.GetGameMethodGroupListByCache();
            var gameMethod = serGame.GetGameMethodListByCache();
            var gameDicList = gameList.ToDictionary(exp => exp.g001);
            var allowBet = serGame.GameAllowBet(GameID, Account.Trim());
            if (0 == allowBet.Code)
            {
                ob.Code = "0001";
                ob.Message =  gameDicList[GameID].g003.Trim() + "暂时不允许投注";
                ob.Scode = newScode;
                return ob;
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
                ob.Code = "0001";
                ob.Message =curGame.g003 + "暂未开放";
                ob.Scode = newScode;
                return ob;
            }
            lotteryOrder.gc001 = serGame.GetGameClassByGameID(curGame.g001);
            lotteryOrder.UserID = UserID;
            lotteryOrder.UserName = Account;
            lotteryOrder.UserNickname = Account;
            var curUserPoint = serUser.GetAGUPData(UserID, lotteryOrder.gc001);
            if (null == curUserPoint)
            {
                ob.Code = "0001";
                ob.Message = curGame.g003 + "返点信息不存在";
                ob.Scode = newScode;
                return ob;
            }
            var datas = lt_project;
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
                    ob.Code = "0001";
                    ob.Message = string.Format("对应编号{0}不存在", dataObj.methodid);
                    ob.Scode = newScode;
                    return ob;
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
                var traces =lt_trace_issues;
                //if (null == traces || 0 == traces.Length)
                //{
                //    traces =lt_trace_issues;
                //}
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
                            traceData.lt_trace_Times = lt_trace_times_;
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
                ob.Code = "0001";
                ob.Message = "追号信息有错";
                ob.Scode = newScode;
                return ob;
            }
            //DBModel.wgs027 addOrderDebug = GetKV("SYS_ADDORDER_DEBUG", true);
            var orderState = serGame.AddOrder(lotteryOrder);
            if (0 == orderState.Code)
            {
                ob.Code = "0001";
                ob.Message = orderState.Message;
                ob.Scode = newScode;
                return ob;
            }
            else if (1 == orderState.Code)
            {
                ob.Code = "0001";
                ob.Message = "success";
                ob.Scode = newScode;
                return ob;
            }
            else if (3 == orderState.Code)
            {
                ob.Code = "0001";
                ob.Message = orderState.Message;
                ob.Scode = newScode;
                return ob;
            }
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        //ob.GameNameList = gameNameList;
        return ob;
    }
    [WebMethod]
    public ObjMsg CancelOrder(int UserID, string SCode, string orderID, string type, string CCode)
    {
        GameServices.IGame serGame = new GameServices.Game();
        ObjMsg ob = new ObjMsg();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, UserID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        if (string.IsNullOrEmpty(orderID))
        {
            ob.Code = "0001";
            ob.Message = "订单编号为空";
            ob.Scode = newScode;
            return ob;
        }
        DBModel.wgs045 getOrder = serGame.GetOrder(long.Parse(orderID));
        if (getOrder.so021 == 1)
        {
            ob.Code = "0001";
            ob.Message = "已经撤单,操作失败";
            ob.Scode = newScode;
            return ob;
        }
        DBModel.wgs038 getGame = serGame.GetGameResult(getOrder.gs001, 1);

        if (DateTime.Now > getGame.gs003)
        {
            ob.Code = "0001";
            ob.Message = "撤单失败，已经封盘";
            ob.Scode = newScode;
            return ob;
        }
        List<long> orderIDs = null;
        if ("list" == type)
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
        var cancelStatus = serGame.CancelOrder(1, orderIDs, UserID, 0, string.Empty);
        if (0 == cancelStatus.Code)
        {
            ob.Code = "0001";
            ob.Message = "撤单失败，" + cancelStatus.Message;
            ob.Scode = newScode;
            return ob;
        }
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        //ob.GameNameList = gameNameList;
        return ob;
    }
}
