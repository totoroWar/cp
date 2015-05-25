using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel
{
    public class SysContentClass
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }

    public class SysDataChangeType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ExtID { get; set; }
    }

    public class SysUIMenu
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string CSS { get; set; }
        public int Order { get; set; }
        public string URL { get; set; }
        public int Show { get; set; }
    }

    public class SysGameResultInfo
    {
        public string GameName { get; set; }
        public int RangeMax { get; set; }
        public int RangeMin { get; set; }
        public int Length { get; set; }
        public string Regex { get; set; }
        public string Comment { get; set; }
        public int IsNormal { get; set; }
        public int GameID { get; set; }
        public bool Same { get; set; }
    }

    public class SysFirstLoadURL
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public int Order { get; set; }
        public int IsShow { get; set; }
    }

    public class SysPositionLevel
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public decimal NeedMoney { get; set; }
    }

    public class SysAccountLevel
    {
        public int Level { get; set; }
        public string Name { get; set; }
    }

    public class SysBaseLevel
    {
        public string Account { get; set; }
        public string NickName { get; set; }
        public string Image { get; set; }
        public int Attribute { get; set; }
    }

    /// <summary>
    /// t28
    /// </summary>
    public class Sysso024
    {
        public int PrizeDataID { get; set; }
        public decimal PrizeNumber { get; set; }
        public decimal Point { get; set; }
    }

    public class SysSumDRInfo
    {
        public int u001 { get; set; }
        public string u002 { get; set; }
        public decimal dr004 { get; set; }
        public decimal dr005 { get; set; }
        public decimal dr006 { get; set; }
        public decimal dr007 { get; set; }
        public decimal dr008 { get; set; }
        public decimal dr009 { get; set; }
        public decimal dr010 { get; set; }
        public decimal dr011 { get; set; }
        public decimal dr012 { get; set; }
        public decimal dr013 { get; set; }
        public decimal dr014 { get; set; }
        public decimal dr015 { get; set; }
        public decimal dr016 { get; set; }
        public decimal dr017 { get; set; }
        public decimal dr018 { get; set; }
    }

    public class ChargeReport
    {
        public int ct001 { get; set; }
        public string ChargeTypeName { get; set; }
        public decimal uc003 { get; set; }
    }

    public class WithDrawReport
    {
        public string Bank { get; set; }
        public decimal SumMoney { get; set; }
    }

    public class GameReport
    {
        public int Count { get; set; }
        public int g001 { get; set; }
        public decimal BetAmount { get; set; }
        public decimal WinAmount { get; set; }
        public decimal Point { get; set; }
    }

    public class OnlineReport
    {
        public DateTime Date { get; set; }
        public int RecordCount { get; set; }
    }

    public class SysTableName
    {
        public string name { get; set; }
        public int id { get; set; }
        public string xtype { get; set; }
        public DateTime crdate { get; set; }
    }

    public class SysTableSize
    {
        public string name { get; set; }
        public string rows { get; set; }
        public string reserved { get; set; }
        public string data { get; set; }
        public string index_size { get; set; }
        public string unused { get; set; }
    }

    public class WinnersList
    {
        public string uname { get; set; }
        public string cname { get; set; }
        public string xindex { get; set; }
        public Decimal xmoney { get; set; }
        public string about { get; set; }
    }

    #region "ORZ "

    public class UList
    {
        public int uid { get; set; }
    }

    public class PassList
    {
        public int u001 { get; set; }
        public string u002 { get; set; }
        public string u003 { get; set; }
        public Decimal s003 { get; set; }
        public Decimal uf001 { get; set; }
        public Decimal uf002 { get; set; }
        public Decimal uf003 { get; set; }
        public Decimal uf004 { get; set; }
    }

    public class UFHListSum
    {
        public decimal smony { get; set; }
        public decimal syk { get; set; }
    }

    public class WGS056Where
    {
        public DateTime dts { get; set; }
        public DateTime dte { get; set; }

        public int type { get; set; }
        public string acct { get; set; }

        public int om { get; set; }
        public decimal omm { get; set; }

    }

    #endregion

}
