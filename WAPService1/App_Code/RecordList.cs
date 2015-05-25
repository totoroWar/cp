using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using GameServices;
using _NWC = NETCommon;

/// <summary>
/// RecordList 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class RecordList : System.Web.Services.WebService {

    public RecordList () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }
    GameServices.ISystem serSystem = new GameServices.System();
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
    private int PageSize
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
    private int PageBloclSize
    {
        get
        {
            if (null == DicKV)
            {
                return 10;
            }
            return int.Parse(GetKV("AGU_PAGE_BLOCK_SIZE", true).cfg003);
        }
    }
    
    

   [WebMethod]
    public ObjMsg GetGameNameList(int userID, string SCode)
    {
        GameServices.IGame serGame = new GameServices.Game();
        GameNames ob = new GameNames();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, userID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        List<DBModel.wgs006> GameClassList = serGame.GetGameClassListByCache();
        List<DBModel.wgs001> GameList = serGame.GetGameListByCache();
       var gDicList = GameList.ToDictionary(key => key.g001);
       
       List<GameName> gameNameList=new List<GameName>();
        foreach (var gc in GameClassList)
        {
            if (0 == gc.gc006)
            {
                continue;
            }
            var haveGame = gc.gc004.Split(',');
            foreach (var gameItem in haveGame)
            {
                GameName gameName = new GameName();
                gameName.gameClassID=gc.gc001;
                gameName.gameID =int.Parse( gameItem);
                gameName.Name = gDicList[int.Parse(gameItem)].g003.Trim();
                gameNameList.Add(gameName);
            }
        }
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        ob.GameNameList = gameNameList;
        return ob;
    }
   [WebMethod]
   public ObjMsg GetGameMethod(int userID, string SCode, int GameListValue)
   {
       GameServices.IGame serGame = new GameServices.Game();
       GameMethods ob = new GameMethods();
       //校验验证码和UID，并生成新的验证码
       string newScode = "";
       newScode = new WAPHelpers().CheckSCode(SCode, userID);
       if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
       List<DBModel.wgs002> GameMethodList = serGame.GetGameMethodListByCache();
       var gmDicList = GameMethodList.ToDictionary(key => key.gm001);

       var nGmList = GameMethodList.Where(exp => exp.g001 == GameListValue && exp.gm002 > 0).ToList();
       
       List<GameMethod> gameMethodList = null;
       foreach (var gm in nGmList)
       {
           GameMethod gameMethod = new GameMethod();
           gameMethod.GMethodID=gm.gm001;
           gameMethod.GMethod=gmDicList[gm.gm002].gm004 +"-"+gm.gm004.Trim();
           gameMethodList.Add(gameMethod);
       }

       ob.Code = "0001";
       ob.Message = "OK";
       ob.Scode = newScode;
       ob.GameMethodList = gameMethodList;
       return ob;
   }
    
    [WebMethod]
    public ObjMsg GetGameListInfo(int userID, int GameListValue, int GMethodID, int GMoleValue, string GLssue, int GUserTypeID, string GCNumber, DateTime GSTime, DateTime GETime, int GPostID, int GCancelID, int PageIndex, string SCode)
    {
        GameServices.IFinance serFinance = new GameServices.Finance();
        GameServices.IGame serGame = new GameServices.Game();
        Orders ob = new Orders();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, userID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        int recordCount = 0;
        var orderList = serGame.GetOrderList(userID, GMethodID, GameListValue, GMoleValue, userID, GUserTypeID, null, GCancelID, GPostID, 0, 0, 0, 0, 0, GLssue, 0, 0, GMoleValue, 0, 0, string.Empty, null, GSTime, GETime, (int)PageIndex, PageSize, out recordCount);
        List<DBModel.LotteryHistoryOrder> OrderShowList = serGame.GetOrderShowList(orderList);
        List<Order> oList = null;
        if (null != OrderShowList)
        {
            foreach (var item in OrderShowList)
            {
                Order order = new Order();
                order.GCNumber = item.projectid;
                order.UserName = item.username;
                order.GameName = item.cnname;
                if (item.taskid != 0)
                {
                    order.GameName += "<span class='o_trace' title='追号' data='" + item.taskid + "'>[追号]</span>";
                }
                if (item.combineOrderID != 0 && item.combineType == 0)
                {
                    order.GameName += "<span class='o_combuy' title='发起合买' data='" + item.combineOrderID + "'>[发起合买]</span>";
                }
                if (item.combineOrderID != 0 && item.combineType == 1)
                {
                    order.GameName += "<span class='o_jcombuy' title='参与合买' data='" + item.combineOrderID + "'>[参与合买]</span>";
                }
                order.GLssue = item.issue;
                order.Gtxt = item.codeShort;
                order.GMoney = item.totalprice;
                order.GMultiple = item.multiple;

                if (item.combineOrderID != 0 && item.combineType == 0)
                {
                    order.GBetting = item.times;
                }
                else if (item.combineOrderID != 0 && item.combineType == 1)
                {
                    order.GBetting = item.times;
                }
                else
                {
                    order.GBetting = item.times;
                }
                order.GMole = item.modes;
                order.GBonus = item.bonus;
                order.GRebate = item.resultpoint;
                order.GTime = item.writeDateTime;
                var str = new string[] { "万", "千", "百", "十", "个" };
                for (var i = 0; i < 5; i++)
                {
                    item.pos = item.pos.Replace(i.ToString(), str[i]);
                }
                order.GPosition = item.pos;
                order.GState = item.statusdesc;
                oList.Add(order);
            }
        }
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        ob.OrderList = oList;
        ob.PageSize = 10;
        ob.MaxPagNumber = 200;
        return ob;
    }
    [WebMethod]
    public ObjMsg GetGameAftTheOnInfo(int UserID, int GameListValue, int GUserTypeID, long GCNumber, DateTime GSTime, DateTime GETime, int PageIndex, string SCode)
    {
        GameServices.IGame serGame = new GameServices.Game();
        OrderTraces ob = new OrderTraces();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, UserID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        var recordCount = 0;
        var tOrderList = serGame.GetTOrderList(GCNumber, 0, GameListValue, string.Empty, UserID, GUserTypeID, string.Empty, GSTime, GETime, (int)PageIndex, PageSize, out recordCount);
        List<DBModel.wgs001> gList = serGame.GetGameListByCache();
        var gDicList = gList.ToDictionary(key => key.g001);
        List<OrderTrace> orderTraceList = null;
        if (null != tOrderList)
        {
            foreach (var item in tOrderList)
            {
                OrderTrace orderTrace = new OrderTrace();
                orderTrace.GCNumber = item.sto001;
                orderTrace.UserName = item.u002.Trim();
                orderTrace.GameName = gDicList[item.g001].g003.Trim();
                orderTrace.GLssue = item.sto010.Trim();
                orderTrace.GMoneySum = item.sto002.ToString("N2");
                orderTrace.GMoney = item.sto007.ToString("N2");
                orderTrace.GWinMoney = item.sto008.ToString("N2");
                orderTrace.GTime = item.sto004;
                switch (item.sto005)
                {
                    case 0:
                        orderTrace.GState = "<span class='fc-red'>未完成</span>";
                        break;
                    case 1:
                        orderTrace.GState = "<span class='fc-green'>已完成</span>";
                        break;
                    case 2:
                        orderTrace.GState = "<span class='fc-gray'>已撤单</span>";
                        break;
                }
                orderTrace.GType = item.sto009 == 0 ? "连续追号" : "追中即停";
                orderTraceList.Add(orderTrace);
            }
        }
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        ob.OrderTraceList = orderTraceList;
        return ob;
    }
    [WebMethod]
    public ObjMsg GetGameNameList(int UserID, int GameListValue, string GLssue, DateTime STime, DateTime ETime, int PageIndex, string SCode)
    {
        GameServices.IGame serGame = new GameServices.Game();
        OrderCombines ob = new OrderCombines();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, UserID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        List<DBModel.wgs001> gList = serGame.GetGameListByCache();
        int recordCount = 0;
        var combuyList = serGame.GetCombuyList(UserID, "", GameListValue, 0, GLssue, -1, (int)PageIndex, PageSize, STime, ETime, out recordCount);
        var gDicList = gList.ToDictionary(key => key.g001);
        List<OrderCombine> OrderCombineList = null;
        if (null != combuyList)
        {
            Dictionary<int, string> dicStatus = new Dictionary<int, string>() { { 0, "进行中" }, { 1, "已满人" }, { 2, "已撤单" }, { 3, "已结算" } };
            foreach (var item in combuyList)
            {
                OrderCombine orderCombine = new OrderCombine();
                orderCombine.GCNumber = item.sco001;
                orderCombine.UserName = item.u002.Trim();
                orderCombine.GameName = gDicList[item.g001].g003;
                orderCombine.GLssue = item.gs002.Trim();
                orderCombine.GResult = item.gs007 == null ? string.Empty : item.gs007.Trim();
                orderCombine.GTxt = NETCommon.StringHelper.GetShortString(item.sco013.Trim(), 15, 15, "...");
                orderCombine.GMoneySum = item.sco007.ToString("N2");
                orderCombine.GMoney = (item.sco007 / 100.0000m).ToString("N2");
                orderCombine.GProForMe = (int)item.sco004;
                orderCombine.GProFoAll = item.sco011;
                orderCombine.GUserCount = item.sco012;
                orderCombine.GTime = item.sco017;
                orderCombine.GState = dicStatus[item.sco009];
                OrderCombineList.Add(orderCombine);
            }
        }
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        ob.OrderCombineList = OrderCombineList;
        return ob;
    }
}
