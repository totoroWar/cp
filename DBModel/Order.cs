using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DBModel
{
    public class LotteryOrder
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserNickname { get; set; }
        public int gc001 { get; set; }
        public int gpd001 { get; set; }
        public int up001 { get; set; }
        public int so019 { get; set; }
        public int so020 { get; set; }
        public int lt_sel_times { get; set; }
        public int lt_sel_modes { get; set; }
        public string lt_sel_dyprize { get; set; }
        public string lt_issue_start { get; set; }
        public int lt_total_nums { get; set; }
        public int lt_total_money { get; set; }
        public string lt_trace_if { get; set; }
        public string lt_trace_stop { get; set; }
        public int lt_trace_times_margin { get; set; }
        public int lt_trace_margin { get; set; }
        public int lt_trace_times_same { get; set; }
        public int lt_trace_diff { get; set; }
        public int lt_trace_times_diff { get; set; }
        public int lt_trace_count_input { get; set; }
        public decimal lt_trace_money { get; set; }
        public int lotteryid { get; set; }
        public string IP { get; set; }
        public string Domain { get; set; }
        public List<LotteryOrderTraceOrderData> TraceDataList { get; set; }
        public List<LotteryOrderData> OrderDataList { get; set; }
        public int lt_combuy_check { get; set; }
        public int lt_self_percent { get; set; }
        public string lt_combuy_password { get; set; }
    }
    public class LotteryOrderData
    {
        public string type { get; set; }
        public int methodid { get; set; }
        public string codes { get; set; }
        public int nums { get; set; }
        public int times { get; set; }
        public decimal money { get; set; }
        public int mode { get; set; }
        public decimal point { get; set; }
        public string desc { get; set; }
        public string curtimes { get; set; }
        public int gm001 { get; set; }
        public int gm002 { get; set; }
        public int up001 { get; set; }
        public int gs001 { get; set; }
        public string position { get; set; }
    }
    public class LotteryOrderTraceOrderData
    {
        public string lt_trace_issues { get; set; }
        public int lt_trace_Times { get; set; }
        public long gs002 { get; set; }
        public int gs001 { get; set; }
    }
    public class LotteryHistoryOrder
    {
        public long projectid { get; set; }
        public string writetime { get; set; }
        public string writetimeori { get; set; }
        public DateTime writeDateTime { get; set; }
        public string methodname{get;set;}
        public string issue { get; set; }
        public string code { get; set; }
        public string codeShort { get; set; }
        public int multiple { get; set; }
        public string modes { get; set; }
        public decimal totalprice { get; set; }
        public decimal bonus { get; set; }
        public string statusdesc { get; set; }
        public int iscancel { get; set; }
        public int prizestatus { get; set; }
        public int isgetprize { get; set; }
        public string nocode { get; set; }
        public string dypointdec { get; set; }
        public object projectprize { get; set; }
        public string username { get; set; }
        public string cnname { get; set; }
        public long taskid { get; set; }
        public int lotteryid { get; set; }
        public int gameclassid { get; set; }
        public decimal resultpoint { get; set; }
        public int times { get; set; }
        public string tiissuestart { get; set; }
        public long combineOrderID { get; set; }
        public decimal combineType { get; set; }
        public decimal point { get; set; }
        public string canPrizeItem { get; set; }
        public int isBackOrd { get; set; }
        public string pos { get; set; }
        public int userId { get; set; }
        public bool commission { get; set; }
    }
    public class prizelevel
    {
        public string levelcodedesc { get; set; }
        public string leveldesc { get; set; }
        public string expandcode { get; set; }
        public string codetimes { get; set; }
        public string level { get; set; }
        public string prize { get; set; }
    }
    public class TraceOrderDetail
    {
        public DBModel.wgs030 TraceOrder { get; set; }
        public List<LotteryHistoryOrder> OrderShowList { get; set; }
        public List<DBModel.wgs022> OrderList { get; set; }
        public List<DBModel.wgs045> OrderList45 { get; set; }
    }

    public class OrderDayAccSumMoney
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal ordermoney { get; set; }
        public decimal lostmoney { get; set; }
        public decimal winmoney { get; set; }
        public decimal sendmoney { get; set; }
    }

    public class CommissionDaySendMessage
    {
        public string name { get; set; }
        public decimal DayConsume { get; set; }
        public decimal DayConsumeMoney { get; set; }
        public decimal DayLoss { get; set; }
        public decimal DayLossMoney { get; set; }
    }
}
