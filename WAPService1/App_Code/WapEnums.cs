using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


[Serializable]
public class MyAjaxObj
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string Temp { get; set; }
    public Object Data { get; set; }
    public List<UserInfoList> DataList { get; set; }

}

[Serializable]
public class BaseObj
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string Scode { get; set; }
}




[Serializable]
public class UserInfoList
{
    public int uID { get; set; }
    public string userName { get; set; }
    public double uMoney { get; set; }
}

[Serializable]
public class ObjMsg
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string Scode { get; set; }
    public int UID { get; set; }
    public MyInfo Data { get; set; }
}

[Serializable]
public class MyInfo
{


    public List<DBModel.SysAccountLevel> AcctLevelList { get; set; }
    public int AGPosID { get; set; }
    public int UILoginID { get; set; }
    public string UILoginAccount { get; set; }
    public string UILoginNickname { get; set; }
    public decimal AGSMoney { get; set; }
    public decimal AGSHoldMoney { get; set; }
    public decimal AGSPoint { get; set; }
    public int AGLevel { get; set; }
    //public Dictionary<int,string> AGLevelName { get; set; } 
    public int AGAcctLevel { get; set; }
    public double? AGStock { get; set; }
    public string AGPosName { get; set; }
    public object GCList { get; set; }
    public object AGPoint { get; set; }
    public object GDicList { get; set; }
    public object GPList { get; set; }
    public object GList { get; set; }

}


[Serializable]
public class MoneyChangeTypes : BaseObj
{
    public List<MoneyChangeType> CList { get; set; }
}

[Serializable]
public class MoneyChangeType
{
    public string ChangeTypeValue { get; set; }
    public int ChangeTypeID { get; set; }

}

[Serializable]
public class MoneyChangeList : BaseObj
{
    public int PageMaxCount { get; set; }
    public int PageSize { get; set; }
    public List<MoneyChange> CList { get; set; }
}


[Serializable]
public class MoneyChange
{
    public long CCode { get; set; }
    public string CAbout { get; set; }
    public decimal CChangeMoney { get; set; }
    public decimal COldMoney { get; set; }
    public decimal CNewMoney { get; set; }
    public decimal COldStopMoney { get; set; }
    public decimal CNewStopMoney { get; set; }
    public decimal CChangePoints { get; set; }
    public decimal COldPoints { get; set; }
    public decimal CNewPoints { get; set; }
    public DateTime CUpTime { get; set; }
    
}

[Serializable]
public class BonusList : BaseObj
{
    public int PageMaxCount { get; set; }
    public int PageSize { get; set; }
    public List<BonusType> CList { get; set; }
}

/// <summary>
/// 分红实体类 
/// </summary>
[Serializable]
public class BonusType
{
    public decimal BMoney { get; set; }
    public decimal BForMoney { get; set; }
    public string CScale { get; set; }
    public int CCycle { get; set; }
    public DateTime CCSTime { get; set; }
    public DateTime CCETime { get; set; }
    public string CCState { get; set; }
}


[Serializable]
public class TeamMoney : BaseObj
{
    public decimal TeamMoneyx { get; set; }
    public decimal TeamPoints { get; set; }
    public decimal TeamStop { get; set; }
    public int TeamForMe { get; set; }
}

[Serializable]
public class PostList : BaseObj
{
    public List<PostType> PList { get; set; }
}


[Serializable]
public class PostType
{
    public long EID { get; set; }
    public string TheUrl { get; set; }
    public int AddCount { get; set; }
    public int IsOpenLower { get; set; }
    public DateTime CreateTime { get; set; }
    public List<PostClass> RebateInfo { get; set; }
}


[Serializable]
public class PostClass
{
    public string PName { get; set; }
    public string Pint { get; set; }
}

[Serializable]
public class AddMyMoney :BaseObj
{
    public string Bank { get; set; }
    public decimal AddMoney { get; set; }
    public string BankCode { get; set; }
    public string ToWho { get; set; }
    public string BankTel { get; set; }
    public string CCode { get; set; }
}


[Serializable]
public class RechargeTypeList : BaseObj
{
    public List<RechargeType> RList { get; set; }
}

[Serializable]
public class RechargeType
{
    public string ForRecharge { get; set; }
    public int ForRechargeID { get; set; }
}


[Serializable]
public class AddMoneyList : BaseObj
{
    public int PageMaxCount { get; set; }
    public int PageSize { get; set; }
    public List<AddMoneyType> AddMoneyLists { get; set; }
}


[Serializable]
public class AddMoneyType
{
    public string ACode { get; set; }
    public string AType { get; set; }
    public decimal AMoney { get; set; }
    public decimal AToMoney { get; set; }
    public DateTime ATime { get; set; }
    public string AState { get; set; }
    public string AAbout { get; set; }

}



[Serializable]
public class CashList : BaseObj
{
    public int PageMaxCount { get; set; }
    public int PageSize { get; set; }
    public List<CashType> CashLists { get; set; }
}


[Serializable]
public class CashType
{
    public string CCode { get; set; }
    public string CType { get; set; }
    public decimal CMoney { get; set; }
    public DateTime CTime { get; set; }
    public string CState { get; set; }
    public string CAbout { get; set; }

}

[Serializable]
public class TFList : BaseObj
{
    public int PageMaxCount { get; set; }
    public int PageSize { get; set; }
    public List<TFType> CashLists { get; set; }
}


[Serializable]
public class TFType
{
    public string TCode { get; set; }
    public string TToName { get; set; }
    public string TForName { get; set; }
    public decimal TMoney { get; set; }
    public DateTime TTime { get; set; }
    public string TState { get; set; }
    public string TAbout { get; set; }

}

[Serializable]
public class UserInfoType : BaseObj
{
    public string MyCode { get; set; }
    public string MName { get; set; }
    public string MBonus { get; set; }
    public string MClass { get; set; }
    public decimal MMoney { get; set; }
    public decimal MStopMoney { get; set; }
    public decimal MPoints { get; set; }
    public string VIP { get; set; }
    public string Grade { get; set; }
    public string GameClass { get; set; }
    public string GameName { get; set; }
    public List<GameRebateType> GameRebate { get; set; }
    public string GameState { get; set; }
    
}

[Serializable]
public class GameRebateType
{
    public string GameType { get; set; }
    public string GameName { get; set; }
    public decimal GameRebat { get; set; }
}



[Serializable]
public class AbotList : BaseObj
{
    public string GlobalNotify { get; set; }
    public List<AbotType> ActLists { get; set; }
}


[Serializable]
public class AbotType
{
    public int MID { get; set; }
    public string MTitle { get; set; }
    public string MTxt { get; set; }
    public DateTime TTime { get; set; }

}

[Serializable]
public class CombineList : BaseObj
{
    public int PageMaxCount { get; set; }
    public int PageSize { get; set; }
    public List<CombineType> CombineLists { get; set; }
}

[Serializable]
public class CombineType
{
    public long GID { get; set; }
    public long GCNumber { get; set; }
    public string UserName { get; set; }
    public string GameName { get; set; }
    public string GEnd { get; set; }
    public string GLssue { get; set; }
    public string GTxt { get; set; }
    public decimal GMoneySum { get; set; }
    public string GMoney { get; set; }
    public decimal GProForMe { get; set; }
    public int GProFoAll { get; set; }
    public int GUserCount { get; set; }
    public DateTime GTime { get; set; }
    public DateTime NGTime { get; set; }
    public string GState { get; set; }
    public bool TimeAllow { get; set; }
}


[Serializable]
public class MyDownUserList : BaseObj
{
    public int PageMaxCount { get; set; }
    public int PageSize { get; set; }
    public List<MyDownUserType> MyDownUserLists { get; set; }
}

[Serializable]
public class MyDownUserType
{
    public int LID { get; set; }
    public string UserName { get; set; }
    public int LCount { get; set; }
    public decimal Money { get; set; }
    public decimal StopMoney { get; set; }
    public decimal Points { get; set; }
    public DateTime CreateTime { get; set; }
    public string LogInfo { get; set; }
    public string State { get; set; }
    public string VIP { get; set; }
    public string Grade { get; set; }
    public string Bonus { get; set; }
    public int LLID { get; set; }
}

[Serializable]
public class MyDownUserInfo : BaseObj
{
    public string UserName { get; set; }
    public string NickName { get; set; }
    public DateTime DCreateTime { get; set; }
    public string DBonus { get; set; }
    public List<MyDownDRebateType>DRebate { get; set; }
}


[Serializable]
public class MyDownDRebateType
{
    public string GameName { get; set; }
    public int GameID { get; set; }
    public decimal DRebateValue { get; set; }
}


[Serializable]
public class HelloWordInfo : BaseObj
{
    public string MTxt { get; set; }
}

[Serializable]
public class MallList : BaseObj
{
    public List<MallListType> mlst { get; set; }
}

[Serializable]
public class MallListType
{
    public int BID { get; set; }
    public string BName { get; set; }
    public string IMGUrl { get; set; }
    public int IsVIP { get; set; }
    public decimal BMoney { get; set; }
    public int BPopularity { get; set; }
    public int BStock { get; set; }
}


[Serializable]
public class MallOrdeList : BaseObj
{
    public List<MallOrdeType> OrdeList { get; set; }
}

[Serializable]
public class MallOrdeType
{
    public int BCode { get; set; }
    public int BResID { get; set; }
    public string BResTitle { get; set; }
    public decimal BResMoney { get; set; }
    public int BCount { get; set; }
    public decimal BSumMoney { get; set; }
    public string BName { get; set; }
    public string BPHone { get; set; }
    public string BTel { get; set; }
    public string BZip { get; set; }
    public decimal BOldMoney { get; set; }
    public string BLogistics { get; set; }
    public string BURL { get; set; }
    public string BPostCode { get; set; }
    public decimal BSale { get; set; }
    public string BState { get; set; }
    public string BAbout { get; set; }
}



[Serializable]
public class ObjMsgs
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string Scode { get; set; }
    public int UID { get; set; }
    public MyInfo Data { get; set; }

}


[Serializable]
public class UserPonits : ObjMsg
{
    public List<UserPonit> UserPonitList { get; set; }
}
[Serializable]
public class UserPonit
{
    public decimal SysMaxPoint { get; set; }
    public decimal MyMaxPoint { get; set; }
    public string GameClass { get; set; }
    public string Game { get; set; }
}
[Serializable]
public class BankAccounts : ObjMsg
{
    public List<BankAccount> BankAccountList { get; set; }
}
[Serializable]
public class BankAccount
{
    public int ID { get; set; }
    public string bankType { get; set; }
    public string accountNumber { get; set; }
    public string accountHolder { get; set; }
    public string openBank { get; set; }
    public string area { get; set; }
    public DateTime? createTime { get; set; }
}
[Serializable]
public class GoinLogs : ObjMsg
{
    public List<GoinLog> GoinLogList { get; set; }
}
[Serializable]
public class GoinLog
{
    public int Number { get; set; }
    public DateTime LogTime { get; set; }
    public string IP { get; set; }
    public string ForIP { get; set; }
    public int MaxPagNumber { get; set; }
}
[Serializable]
public class MentionNow : ObjMsg
{
    public string BankInfo { get; set; }
    public decimal MyMoney { get; set; }
    public decimal StopMoney { get; set; }
    public decimal Commission { get; set; }
}
[Serializable]
public class Reports : ObjMsg
{
    public List<Report> ReportList { get; set; }
}
[Serializable]
public class Report
{
    public string MName { get; set; }
    public DateTime MUpTime { get; set; }
    public decimal MAddMoney { get; set; }
    public decimal MPAYMoney { get; set; }
    public decimal MBonus { get; set; }
    public decimal MBack { get; set; }
    public decimal MGetMoney { get; set; }
    public decimal MPostMoney { get; set; }
    public decimal MBonusMoney { get; set; }
    public decimal MGoInMoney { get; set; }
    public decimal MGoOutoney { get; set; }
    public decimal MGetPints { get; set; }
    public decimal MConsumePints { get; set; }
    public decimal MProfit { get; set; }
}
[Serializable]
public class BearkDownList : ObjMsg
{
    public string MName { get; set; }
    public decimal MyMoney { get; set; }
    public decimal StopMoney { get; set; }
    public decimal Commission { get; set; }
}
[Serializable]
public class GameNames : ObjMsg
{
    public List<GameName> GameNameList { get; set; }
}
[Serializable]
public class GameName
{
    public int gameClassID { get; set; }
    public int gameID { get; set; }
    public string Name { get; set; }
}
[Serializable]
public class GameMethods : ObjMsg
{
    public List<GameMethod> GameMethodList { get; set; }
}
[Serializable]
public class GameMethod
{
    public string GMethod { get; set; }
    public int GMethodID { get; set; }
}
[Serializable]
public class Orders : ObjMsg
{
    public int MaxPagNumber { get; set; }
    public int PageSize { get; set; }
    public List<Order> OrderList { get; set; }
}
[Serializable]
public class Order
{
    public long GCNumber { get; set; }
    public string UserName { get; set; }
    public string GameName { get; set; }
    public string GLssue { get; set; }
    public string Gtxt { get; set; }
    public decimal GMoney { get; set; }
    public int GMultiple { get; set; }
    public int GBetting { get; set; }
    public string GMole { get; set; }
    public decimal GBonus { get; set; }
    public decimal GRebate { get; set; }
    public DateTime GTime { get; set; }
    public string GPosition { get; set; }
    public string GState { get; set; }
    public int GCancel { get; set; }
}
[Serializable]
public class OrderTraces : ObjMsg
{
    public int MaxPagNumber { get; set; }
    public List<OrderTrace> OrderTraceList { get; set; }
}
[Serializable]
public class OrderTrace
{
    public long GCNumber { get; set; }
    public string UserName { get; set; }
    public string GameName { get; set; }
    public string GLssue { get; set; }
    public string GMoneySum { get; set; }
    public string GMoney { get; set; }
    public string GWinMoney { get; set; }
    public DateTime GTime { get; set; }
    public string GState { get; set; }
    public string GType { get; set; }

    public int GCancel { get; set; }
}
[Serializable]
public class OrderCombines : ObjMsg
{
    public int MaxPagNumber { get; set; }
    public List<OrderCombine> OrderCombineList { get; set; }
}
[Serializable]
public class OrderCombine
{
    public long GCNumber { get; set; }
    public string UserName { get; set; }
    public string GameName { get; set; }
    public string GLssue { get; set; }
    public string GResult { get; set; }
    public string GTxt { get; set; }
    public string GMoneySum { get; set; }
    public string GMoney { get; set; }
    public int GProForMe { get; set; }
    public int GProFoAll { get; set; }
    public int GUserCount { get; set; }
    public DateTime GTime { get; set; }
    public string GState { get; set; }
    public string GType { get; set; }

    public int GCancel { get; set; }
}
[Serializable]
public class Messages : ObjMsg
{
    public int MaxPagNumber { get; set; }
    public int MCount { get; set; }
    public List<Message> MessageList { get; set; }
}
[Serializable]
public class Message
{
    public string ForName { get; set; }
    public string ToName { get; set; }
    public string MTitle { get; set; }
    public DateTime PTime { get; set; }
    public long MID { get; set; }
}
[Serializable]
public class Lotterys : ObjMsg
{
    public List<LotteryAll> LotteryAllList { get; set; }
    public LotteryCurrent LotteryCurrent { get; set; }
    public List<LotteryLog> LotteryLogList { get; set; }
}
[Serializable]
public class LotteryAll
{
    public string IGameName { get; set; }
    public int IGameID { get; set; }
    public string IGameIssue { get; set; }
    public long IISOpen { get; set; }
    public DateTime starttime { get; set; }
    public DateTime ICloseTime { get; set; }
}
[Serializable]
public class LotteryCurrent
{
    public string IGameName { get; set; }
    public int IGameID { get; set; }
    public string IGameIssue { get; set; }
    public long IISOpen { get; set; }
    public DateTime starttime { get; set; }
    public DateTime ICloseTime { get; set; }
}
[Serializable]
public class LotteryLog
{
    public string GIssue { get; set; }
    public string GResult { get; set; }
}


