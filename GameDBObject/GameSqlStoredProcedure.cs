using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using GameDBObject;
using System.Collections.Generic;
using System.Text;
public partial class USP
{
    [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_GameSqlStoredProcedure")]
    public static void GameSqlStoredProcedure()
    {
    }


    [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_CleanUserOnline")]
    public static void CleanUserOnline()
    {
            SqlTriggerContext triggerContext = SqlContext.TriggerContext;
            SqlPipe pipe = SqlContext.Pipe;
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT cfg003 FROM wgs027(NOLOCK) WHERE [cfg001]=@key;";
                cmd.Parameters.AddWithValue("@key", "SYS_CLEAN_ONLINE");
                object time = cmd.ExecuteScalar();
                int defaultTime = -180;
                if (null != time)
                {
                    defaultTime = 0 - Convert.ToInt32(time);
                }
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE wgs025 SET onl006 = 0 WHERE DATEDIFF(second,GETDATE(),onl004) <= @time AND onl006=1; ";
                cmd.Parameters.AddWithValue("@time", defaultTime);
                int count = cmd.ExecuteNonQuery();
                pipe.Send(string.Format("小于等于{1}秒，被重置，一共{0}条记录", count, defaultTime));
            }
    }


    [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_GameSessionOtherClose")]
    public static void GameSessionOtherClose()
    {
            SqlTriggerContext triggerContext = SqlContext.TriggerContext;
            SqlPipe pipe = SqlContext.Pipe;
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                byte gsr004 = 1;
                byte gsr004Where = 0;
                cmd.CommandText = "UPDATE [wgs038] SET [gsr004]=@grs004 WHERE DATEDIFF(second,GETDATE(),[gs004])<=0 AND gsr004=@gsr004Where;";
                cmd.Parameters.AddWithValue("@grs004", gsr004);
                cmd.Parameters.AddWithValue("@gsr004Where", gsr004Where);
                int count = cmd.ExecuteNonQuery();
                pipe.Send(string.Format(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"要封盘的数据，一共{0}条记录", count));
            }
    }


    [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_LineSignPayPoint")]
    public static void LineSignPayPoint(int count,DateTime dts, DateTime dte, int debug)
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            dts = DateTime.Parse(dts.ToString("yyyy/MM/dd") + " 00:00:00");
            dte = DateTime.Parse(dte.ToString("yyyy/MM/dd") + " 23:59:59");
            if (1 == debug)
            {
                pipe.Send(string.Format("时间段{0}-{1}", dts, dte));
            }
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            var command = connection.CreateCommand();
            command.Parameters.Clear();
            command.CommandText = "SELECT COUNT(1) AS RecordCount,u001 FROM wgs046(NOLOCK) WHERE sign002 >= @DTStart AND sign002 <= @DTEnd GROUP BY u001;";
            command.Parameters.AddWithValue("@DTStart", dts);
            command.Parameters.AddWithValue("@DTEnd", dte);
            da.SelectCommand = command;
            da.Fill(ds, "GroupSet");
            if (1 == debug)
            {
                pipe.Send(string.Format("Record count : {0}", ds.Tables["GroupSet"].Rows.Count));
            }
            command.Parameters.Clear();
            command.CommandText = "SELECT cfg003 FROM wgs027(NOLOCK) WHERE cfg001=@cfg001;";
            command.Parameters.AddWithValue("@cfg001", "SYS_LINE_SIGN_POINT");
            var linePoint = Convert.ToDecimal(command.ExecuteScalar());
            foreach (DataRow row in ds.Tables["GroupSet"].Rows)
            {
                var recordCount = (int)row["RecordCount"];
                var userID = (int)row["u001"];
                if (1 == debug)
                {
                    pipe.Send(string.Format("User:{0},Count:{1}", userID, row[0]));
                }
                if (recordCount >= count)
                {
                    if (1 == debug)
                    {
                        pipe.Send(string.Format("时间段{0}-{1}，用户ID：{2}，次数：{3}", dts, dte, userID, recordCount));
                        continue;
                    }
                    command.Parameters.Clear();
                    command.CommandText = "UPDATE wgs014 SET uf004=uf004+@uf004 WHERE u001=@u001;";
                    command.Parameters.AddWithValue("@uf004", linePoint);
                    command.Parameters.AddWithValue("@u001", userID);
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT u002,u003 FROM wgs012(NOLOCK) WHERE u001=@u001;";
                    da.Fill(ds, "UserInfo");
                    DataRow userRow = ds.Tables["UserInfo"].Rows[0];
                    string userName = userRow["u002"].ToString().Trim();
                    string userNickName = userRow.IsNull("u003") ? string.Empty : userRow["u003"].ToString().Trim();
                    UST.WriteDataChangeLog(ref command, userID, userName, userNickName, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, linePoint, 0, 34, ExtDCFunction.GetRDList()[34].Name, string.Empty, "连续签到奖励积分", 0, DateTime.Now);
                    UST.SetDayReport(ref command, userID, userName, userNickName, 0, 0, 0, 0, linePoint, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    ExtDCFunction.SendMessage(ref command, userID, userName, userNickName, "连续签到奖励积分", string.Format("您在{0}至{1}期间连续签到，系统奖励{2:N2}积分", dts.ToString("yyyy/MM/dd"), dte.ToString("yyyy/MM/dd"), linePoint), DateTime.Now);
                }
            }
        }
    }


    [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_GetWeekRange")]
    public static void GetWeekRange(out DateTime dts, out DateTime dte, DateTime dtcur, int debug)
    {
        dts = new DateTime(1970, 1, 1, 0, 0, 0);
        dte = new DateTime(1970, 1, 1, 0, 0, 0);
        switch (dtcur.DayOfWeek)
        {
            case DayOfWeek.Friday:
                dte = dtcur.AddDays(-5);
                break;
            case DayOfWeek.Monday:
                dte = dtcur.AddDays(-1);
                break;
            case DayOfWeek.Saturday:
                dte = dtcur.AddDays(-6);
                break;
            case DayOfWeek.Sunday:
                dte = dtcur.AddDays(-7);
                break;
            case DayOfWeek.Thursday:
                dte = dtcur.AddDays(-4);
                break;
            case DayOfWeek.Tuesday:
                dte = dtcur.AddDays(-2);
                break;
            case DayOfWeek.Wednesday:
                dte = dtcur.AddDays(-3);
                break;
        }
        dts = dte.AddDays(-6);
        if (1 == debug)
        {
            SqlContext.Pipe.Send(string.Format("{0}-{1},{2}", dts, dte, dtcur));
        }
    }


    [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_RefreshLevel")]
    public static void RefreshLevel(DateTime dts, DateTime dte, int deleteBefor, int debug)
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        int pageSize = 100, pageIndex = 0;
        int haveCount = 0;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            if (1 == debug)
            {
                SqlContext.Pipe.Send(string.Format("dts：{0}-dte：{1}", dts, dte));
            }
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT cfg003 FROM wgs027(NOLOCK) WHERE cfg001=@cfg001;";
            command.Parameters.AddWithValue("@cfg001", "SYS_POSITION_LEVEL_DEFAULT");
            var defaultLevelStr = command.ExecuteScalar().ToString();
            var defaultLevelSplit = defaultLevelStr.Split(',');
            command.Parameters.Clear();
            command.CommandText = "SELECT cfg003 FROM wgs027(NOLOCK) WHERE cfg001=@cfg001;";
            command.Parameters.AddWithValue("@cfg001", "SYS_POSITION_LEVEL");
            var sysPosLevelStr = command.ExecuteScalar().ToString();
            var sysPosLevel = sysPosLevelStr.Split(',');
            Dictionary<int, string> sysPosDic = new Dictionary<int, string>();
            foreach (var item in sysPosLevel)
            {
                var itemSplit = item.Split(':');
                sysPosDic.Add(int.Parse(itemSplit[0]), itemSplit[1]);
            }
            command.Parameters.Clear();
            command.CommandText = "SELECT cfg003 FROM wgs027(NOLOCK) WHERE cfg001=@cfg001;";
            command.Parameters.AddWithValue("@cfg001", "SYS_STOCK_MAX");
            var sysDefMaxStr = command.ExecuteScalar().ToString();
            var sysDefMaxSplit = sysDefMaxStr.Split(',');
            Dictionary<int, decimal> sysDefMaxDic = new Dictionary<int, decimal>();
            foreach (var item in sysDefMaxSplit)
            {
                var itemSplit = item.Split(':');
                sysDefMaxDic.Add(int.Parse(itemSplit[0]), decimal.Parse(itemSplit[1]));
            }
            if (1 == debug)
            {
                SqlContext.Pipe.Send(string.Format("SYS_POSITION_LEVEL_DEFAULT:{0}", defaultLevelStr));
                SqlContext.Pipe.Send(string.Format("SYS_POSITION_LEVEL:{0}", sysPosLevelStr));
                SqlContext.Pipe.Send(ExtDCFunction.SplitLine);
            }
            do
            {
                command.Parameters.Clear();
                command.CommandText = "SELECT u001,u002,u003,u012,u013,u014,u018,u019 FROM wgs049(NOLOCK) ORDER BY u001 OFFSET @SkinRow ROWS FETCH NEXT @TakeRow ROWS ONLY;";
                command.Parameters.AddWithValue("@SkinRow", pageIndex * pageSize);
                command.Parameters.AddWithValue("@TakeRow", pageSize);
                da.SelectCommand = command;
                da.Fill(ds, "UserList");
                haveCount = ds.Tables["UserList"].Rows.Count;
                pageIndex+=1;
                foreach (DataRow row in ds.Tables["UserList"].Rows)
                {
                    command.Parameters.Clear();
                    command.CommandText = "SELECT SUM(dr004) AS SumBet,SUM(dr006) AS SumWin,SUM(dr007) AS SumPoint,SUM(dr011) AS SumFreeGet, SUM(dr014) AS SumCFee, SUM(dr015) AS SumWFee FROM wgs042(NOLOCK) WHERE u001 IN(SELECT u002 FROM wgs048(NOLOCK) AS URT WHERE URT.u001=@u001 OR URT.u002=@u001) AND (dr002 >=@dts AND dr002 <= @dte);";
                    command.Parameters.AddWithValue("@u001",row["u001"]);
                    command.Parameters.AddWithValue("@dts", dts);
                    command.Parameters.AddWithValue("@dte", dte);
                    da.SelectCommand = command;
                    da.Fill(ds, "DaySum");
                    var u001 = (int)row["u001"];
                    var u002 = row["u002"].ToString().Trim();
                    var u003 = row.IsNull("u003") ? string.Empty : row["u003"].ToString().Trim();
                    var u018 = (int)row["u018"];
                    var u019 = (decimal)row["u019"];
                    var u012 = (int)row["u012"];
                    var u014 = row.IsNull("u014") ? string.Empty : row["u014"].ToString().Trim();
                    var pUserNickName = string.Empty;
                    decimal SumBet = 0.0000m;
                    decimal SumWin = 0.0000m;
                    decimal SumPoint = 0.0000m;
                    decimal SumFreeGet = 0.0000m;
                    decimal SumCFee = 0.0000m;
                    decimal SumWFee = 0.0000m;
                    if (0 < ds.Tables["DaySum"].Rows.Count)
                    {
                        DataRow sumRow = ds.Tables["DaySum"].Rows[0];
                        SumBet = sumRow.IsNull("SumBet") ? 0 : (decimal)sumRow["SumBet"];
                        SumWin = sumRow.IsNull("SumWin") ? 0 : (decimal)sumRow["SumWin"];
                        SumPoint = sumRow.IsNull("SumPoint") ? 0 : (decimal)sumRow["SumPoint"];
                        SumFreeGet = sumRow.IsNull("SumFreeGet") ? 0 : (decimal)sumRow["SumFreeGet"];
                        SumCFee = sumRow.IsNull("SumCFee") ? 0 : (decimal)sumRow["SumCFee"];
                        SumWFee = sumRow.IsNull("SumWFee") ? 0 : (decimal)sumRow["SumWFee"];
                    }
                    if (1 == debug)
                    {
                        SqlContext.Pipe.Send(string.Format("账号：{0}，SumBet：{1}，SumWin:{2}，SumPoint:{3}，SumFreeGet:{4}，SumCFee:{5}，SumWFee:{6}", row["u002"].ToString().Trim(), SumBet, SumWin, SumPoint, SumFreeGet, SumCFee, SumWFee));
                    }
                    var newLevel = 0;
                    var levelName = string.Empty;
                    foreach (var posLevel in sysPosLevel)
                    {
                        var posLevelSplit = posLevel.Split(':');
                        var levelSum = decimal.Parse(posLevelSplit[2]);
                        var levelID = int.Parse(posLevelSplit[0]);
                        if (SumBet >= levelSum)
                        {
                            newLevel = levelID;
                            levelName = posLevelSplit[1];
                            if (1 == debug)
                            {
                                SqlContext.Pipe.Send(string.Format("SumBet：{0}>=levelSum：{1}", SumBet, levelSum));
                            }
                        }
                    }
                    if (1 == debug)
                    {
                        SqlContext.Pipe.Send(string.Format("等级：{0}，军衔：{1}", newLevel, levelName));
                    }
                    foreach (var posDefLevel in defaultLevelSplit)
                    {
                        var posDefLevelSplit = posDefLevel.Split(':');
                        if (1 == debug)
                        {
                            SqlContext.Pipe.Send(string.Format("默认：{0}，{1}", posDefLevelSplit[0], posDefLevelSplit[1]));
                        }
                        if (int.Parse(posDefLevelSplit[0]) == u018 && newLevel < int.Parse(posDefLevelSplit[1]))
                        {
                            newLevel = int.Parse(posDefLevelSplit[1]);
                            levelName = sysPosDic[newLevel];
                        }
                    }
                    if (1 == debug)
                    {
                        SqlContext.Pipe.Send(string.Format("等级：{0}，军衔：{1}，Type：{2}", newLevel, levelName, u018));
                    }
                    if (1 == deleteBefor)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "DELETE FROM wgs047 WHERE cur021=@cur021 AND cur005=@cur005;";
                        command.Parameters.AddWithValue("@cur021", dte);
                        command.Parameters.AddWithValue("@cur005", u001);
                        var deleteCount = command.ExecuteNonQuery();
                        SqlContext.Pipe.Send(string.Format("删除数据：{0}", deleteCount));
                    }
                    command.Parameters.Clear();
                    command.CommandText = "SELECT COUNT(1) FROM wgs047 WHERE cur021=@cur021 AND cur005=@cur005;";
                    command.Parameters.AddWithValue("@cur021", dte);
                    command.Parameters.AddWithValue("@cur005", u001);
                    var existsRecord = (int)command.ExecuteScalar();
                    if (1 == debug)
                    {
                        SqlContext.Pipe.Send(string.Format("存在数据：{0}", existsRecord));
                    }
                    if (0 == existsRecord)
                    {
                        var pUserLevel = 0;
                        command.Parameters.Clear();
                        command.CommandText = "SELECT u003,u013, u018 FROM wgs049(NOLOCK) WHERE u002=@u002;";
                        command.Parameters.AddWithValue("@u002", u014);
                        da.Fill(ds, "PUInfo");
                        if (0 < ds.Tables["PUInfo"].Rows.Count)
                        {
                            pUserNickName = ds.Tables["PUInfo"].Rows[0].IsNull("u003") ? string.Empty : ds.Tables["PUInfo"].Rows[0]["u003"].ToString().Trim();
                            pUserLevel = Convert.ToInt32(ds.Tables["PUInfo"].Rows[0]["u018"]);
                        }
                        ds.Tables["PUInfo"].Clear();
                        command.Parameters.Clear();
                        command.CommandText = "INSERT INTO wgs047(cur002, cur003, cur005, cur006, cur007, cur008, cur009, cur010, cur011, cur012, cur013, cur014, cur015, cur016, cur017, cur018, cur019, cur020, cur021, cur022, cur023, cur024, cur025) VALUES(@DateStart, @MyJX, @MyUserID, @MyUserName, @MyUserNickName, @MyUserLayer, @MyParentUserID , @MyParentUserName, @MyParentUserNickName, @MyParentLevelID, @SubBet, @SumWin, @SumPoint, @SumFreeGet, @SumCFee, @SumWFee, @NowDT, @DTS, @DTE, 0, @cur023, 0, @cur025);";
                        command.Parameters.AddWithValue("@DateStart", dts);
                        command.Parameters.AddWithValue("@MyUserID", u001);
                        command.Parameters.AddWithValue("@MyUserName", u002);
                        command.Parameters.AddWithValue("@MyUserNickName", u003);
                        command.Parameters.AddWithValue("@SubBet", SumBet);
                        command.Parameters.AddWithValue("@SumWin", SumWin);
                        command.Parameters.AddWithValue("@SumPoint", SumPoint);
                        command.Parameters.AddWithValue("@SumFreeGet", SumFreeGet);
                        command.Parameters.AddWithValue("@SumCFee", SumCFee);
                        command.Parameters.AddWithValue("@SumWFee", SumWFee);
                        command.Parameters.AddWithValue("@MyUserLayer", u018);
                        command.Parameters.AddWithValue("@MyParentUserID", u012);
                        command.Parameters.AddWithValue("@MyParentUserName", u014);
                        command.Parameters.AddWithValue("@MyParentUserNickName", pUserNickName);
                        command.Parameters.AddWithValue("@MyParentLevelID", pUserLevel);
                        command.Parameters.AddWithValue("@NowDT", DateTime.Now);
                        command.Parameters.AddWithValue("@DTS", dts);
                        command.Parameters.AddWithValue("@DTE", dte);
                        command.Parameters.AddWithValue("@cur025", (SumWin + SumPoint + SumFreeGet) - SumBet);
                        command.Parameters.AddWithValue("@cur023", u019);
                        command.Parameters.AddWithValue("@MyJX", newLevel);
                        command.ExecuteNonQuery();
                    }
                    command.Parameters.Clear();
                    command.CommandText = "UPDATE wgs012 SET u013=@newLevel WHERE u001=@u001;";
                    command.Parameters.AddWithValue("@newLevel", newLevel);
                    command.Parameters.AddWithValue("@u001", u001);
                    command.ExecuteNonQuery();
                    /*
                     * 2014年9月23日00时41分06秒
                     * 更改级别的时候不需要更改股份
                     * 备注
                     */
                    string title = string.Format("{0}至{1}期间军衔评定，{2}军衔", dts, dte, levelName);
                    string content = string.Format("{0}至{1}期间军衔评定，{2}军衔", dts, dte, levelName);
                    ExtDCFunction.SendMessage(ref command, u001, u002, u003, title, content, DateTime.Now);
                    ds.Tables["DaySum"].Clear();
                    if (1 == debug)
                    {
                        SqlContext.Pipe.Send(ExtDCFunction.SplitLine);
                    }
                }
                ds.Tables["UserList"].Clear();
            } while (0 < haveCount);
        }
    }


    [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_StockByLevel")]
    public static void StockByLevel(DateTime dts, DateTime dte, int reStock, int debug)
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        int levelCount = 0;
        int levelStart = 3;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            if (1 == debug)
            {
                SqlContext.Pipe.Send(string.Format("dts：{0}-dte：{1}", dts, dte));
                SqlContext.Pipe.Send(string.Format("dts：{0}-dte：{1}", dts, dte));
            }
            connection.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            var command = connection.CreateCommand();
            List<int> badList = new List<int>();
            Dictionary<int, decimal> stockDicKey = new Dictionary<int, decimal>();
            command.Parameters.Clear();
            command.CommandText = "SELECT cfg003 FROM wgs027 WHERE cfg001=@cfg001;";
            command.Parameters.AddWithValue("@cfg001", "SYS_STOCK_MAX");
            var maxtStock = command.ExecuteScalar().ToString();
            var maxtStockSplit = maxtStock.Split(',');
            foreach (var item in maxtStockSplit)
            {
                var itemSplit = item.Split(':');
                stockDicKey.Add(int.Parse(itemSplit[0]), decimal.Parse(itemSplit[1]));
            }
            if (1 == debug)
            {
                SqlContext.Pipe.Send(string.Format("SYS_STOCK_MAX：{0}", maxtStock));
            }
            SqlContext.Pipe.Send(ExtDCFunction.SplitLine);
            command.Parameters.Clear();
            command.CommandText = "SELECT cfg003 FROM wgs027 WHERE cfg001=@cfg001;";
            command.Parameters.AddWithValue("@cfg001", "SYS_STOCK_LIMIT");
            var lowLimit = Convert.ToDecimal(command.ExecuteScalar());
            do
            {
                command.Parameters.Clear();
                command.CommandText = "SELECT u001,u002,u002,u003,u012,u013,u014,u018,u019 FROM wgs049 WHERE u018=@u018;";
                command.Parameters.AddWithValue("@u018", levelStart);
                da.SelectCommand = command;
                da.Fill(ds, "StartUserList");
                levelCount = ds.Tables["StartUserList"].Rows.Count;
                bool inBadList = false;
                foreach (DataRow item in ds.Tables["StartUserList"].Rows)
                {
                    var u001 = (int)item["u001"];
                    var u002 = item["u002"].ToString().Trim();
                    var u003 = item.IsNull("u003") ? string.Empty : item["u003"].ToString().Trim();
                    var u012 = (int)item["u012"];
                    var u018 = (int)item["u018"];
                    var u019 = (decimal)item["u019"];
                    var u014 = item.IsNull("u014") ? string.Empty : item["u014"].ToString().Trim();
                    decimal parentu019 = 0;
                    decimal parentHaveStock = 0.0000m;
                    foreach (var skip in badList)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "SELECT COUNT(1) FROM wgs048(NOLOCK) WHERE u001=@u001 AND u002=@u002;";
                        command.Parameters.AddWithValue("@u001", skip);
                        command.Parameters.AddWithValue("@u002", u001);
                        var badCount = (int)command.ExecuteScalar();
                        if (0 < badCount)
                        {
                            inBadList = true;
                        }
                    }
                    if (true == inBadList)
                    {
                        inBadList = false;
                        continue;
                    }
                    command.Parameters.Clear();
                    command.CommandText = "SELECT u018 FROM wgs049 WHERE u001=@u012;";
                    command.Parameters.AddWithValue("@u012", u012);
                    var parentu018 = (int)command.ExecuteScalar();
                    if (2 == parentu018)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "SELECT SUM(dr004) AS SumBet,SUM(dr006) AS SumWin,SUM(dr007) AS SumPoint,SUM(dr011) AS SumFreeGet, SUM(dr014) AS SumCFee, SUM(dr015) AS SumWFee FROM wgs042(NOLOCK) WHERE u001 IN(SELECT u002 FROM wgs048(NOLOCK) AS URT WHERE URT.u001=@u001 AND URT.u002<>@u001) AND (dr002 >=@dts AND dr002 <= @dte);";
                        command.Parameters.AddWithValue("@u001", u012);
                        command.Parameters.AddWithValue("@dts", dts);
                        command.Parameters.AddWithValue("@dte", dte);
                        da.SelectCommand = command;
                        da.Fill(ds, "DayParentSum");
                        DataRow calcRowP = ds.Tables["DayParentSum"].Rows[0];
                        var SumBetP = calcRowP.IsNull("SumBet") ? 0 : (decimal)calcRowP["SumBet"];
                        var SumWinP = calcRowP.IsNull("SumWin") ? 0 : (decimal)calcRowP["SumWin"];
                        var SumPointP = calcRowP.IsNull("SumPoint") ? 0 : (decimal)calcRowP["SumPoint"];
                        var SumFreeGetP = calcRowP.IsNull("SumFreeGet") ? 0 : (decimal)calcRowP["SumFreeGet"];
                        var SumAllP = (SumWinP + SumPointP + SumFreeGetP) - SumBetP;
                        if (SumAllP > 0-lowLimit)
                        {
                            badList.Add(u012);
                            if (1 == debug)
                            {
                                SqlContext.Pipe.Send(string.Format("线长未达到最底要求，系统要求{0}，线程现在{1}", 0-lowLimit, SumAllP));
                                continue;
                            }
                        }
                        ds.Tables["DayParentSum"].Clear();
                    }
                    command.Parameters.Clear();
                    command.CommandText = "SELECT u019 FROM wgs049 WHERE u001=@u012;";
                    command.Parameters.AddWithValue("@u012", u012);
                    parentu019 = (decimal)command.ExecuteScalar();
                    parentHaveStock = parentu019 - u019;
                    if (0 >= parentHaveStock)
                    {
                        continue;
                    }
                    if (1 == debug)
                    {
                        SqlContext.Pipe.Send(string.Format("上线可得：{0}:{1}-{2}:{3}", u002, u019, u014, parentu019));
                        SqlContext.Pipe.Send(string.Format("上级可得：{0}:{1}", u014, parentHaveStock));
                    }
                    command.Parameters.Clear();
                    command.CommandText = "SELECT SUM(dr004) AS SumBet,SUM(dr006) AS SumWin,SUM(dr007) AS SumPoint,SUM(dr011) AS SumFreeGet, SUM(dr014) AS SumCFee, SUM(dr015) AS SumWFee FROM wgs042(NOLOCK) WHERE u001 IN(SELECT u002 FROM wgs048(NOLOCK) AS URT WHERE URT.u001=@u001 AND URT.u002<>@u001) AND (dr002 >=@dts AND dr002 <= @dte);";
                    command.Parameters.AddWithValue("@u001", u001);
                    command.Parameters.AddWithValue("@dts", dts);
                    command.Parameters.AddWithValue("@dte", dte);
                    da.SelectCommand = command;
                    da.Fill(ds, "DaySum");
                    if (0 < ds.Tables["DaySum"].Rows.Count)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "SELECT u003 FROM wgs049(NOLOCK) WHERE u001=@u001;";
                        command.Parameters.AddWithValue("@u001", u012);
                        var pResult = command.ExecuteScalar();
                        var parentNickName = pResult == null ? string.Empty : pResult.ToString().Trim();
                        command.Parameters.Clear();
                        command.CommandText = "SELECT u018 FROM wgs049(NOLOCK) WHERE u001=@u001;";
                        command.Parameters.AddWithValue("@u001", u012);
                        var pLevel = command.ExecuteScalar();
                        var pLevelValue = pLevel == null ? string.Empty : pLevel.ToString().Trim();
                        DataRow calcRow = ds.Tables["DaySum"].Rows[0];
                        var SumBet = calcRow.IsNull("SumBet") ? 0 : (decimal)calcRow["SumBet"];
                        var SumWin = calcRow.IsNull("SumWin") ? 0 : (decimal)calcRow["SumWin"];
                        var SumPoint = calcRow.IsNull("SumPoint") ? 0 : (decimal)calcRow["SumPoint"];
                        var SumFreeGet = calcRow.IsNull("SumFreeGet") ? 0 : (decimal)calcRow["SumFreeGet"];
                        var SumAll = (SumWin + SumPoint + SumFreeGet) - SumBet;
                        var SumAllOK = SumAll * (parentHaveStock / 100.0000m);
                        var SumAllOKData = Math.Abs(SumAllOK);
                        if (1 == debug)
                        {
                            SqlContext.Pipe.Send(string.Format("编号：{0}，账号：{1}，下线纯利（不包括自己）:{2}，分红：{3}", u001, u002, SumAll, SumAllOKData));
                        }
                        if (0 > SumAll)
                        {
                            ExtDCFunction.SendMessage(ref command, u012, u014, parentNickName, string.Format("{0}至{1}系统分红", dts.ToString(), dte.ToString()), string.Format("您在{0}至{1}期间获得系统分红，分红金额{2:N2}（{3:N2}%）", dts.ToString(), dte.ToString(), SumAllOKData, parentHaveStock), DateTime.Now);
                            UST.WriteDataChangeLog(ref command, u012, u014, parentNickName, 0, 0, 0, 0, 0, 0, 0, SumAllOKData, 0, 0, 0, 0, 0, 7, ExtDCFunction.GetRDList()[7].Name, "", "", 0, DateTime.Now);
                            UST.SetDayReport(ref command, u012, u014, parentNickName, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, SumAllOKData, 0);
                            command.Parameters.Clear();
                            command.CommandText = "UPDATE wgs014 SET uf001=uf001+@uf001 WHERE u001=@u001;";
                            command.Parameters.AddWithValue("@uf001", SumAllOKData);
                            command.Parameters.AddWithValue("@u001", u012);
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                            command.CommandText = "INSERT INTO wgs051(us002,us003,us004,us005,us006,us007,us008,us009,us010,us011,us012,us013,us014,us015,us016) VALUES(@us002,@us003,@us004,@us005,@us006,@us007,@us008,@us009,@us010,@us011,@us012,@us013,@us014,@us015,@us016);";
                            command.Parameters.AddWithValue("@us002", u012);
                            command.Parameters.AddWithValue("@us003", u014);
                            command.Parameters.AddWithValue("@us004", parentNickName);
                            command.Parameters.AddWithValue("@us005", u001);
                            command.Parameters.AddWithValue("@us006", u002);
                            command.Parameters.AddWithValue("@us007", u003);
                            command.Parameters.AddWithValue("@us008", SumAllOKData);
                            command.Parameters.AddWithValue("@us009", parentHaveStock);
                            command.Parameters.AddWithValue("@us010", DateTime.Now);
                            command.Parameters.AddWithValue("@us011", dts);
                            command.Parameters.AddWithValue("@us012", dte);
                            command.Parameters.AddWithValue("@us013", SumAll);
                            command.Parameters.AddWithValue("@us014", pLevel);
                            command.Parameters.AddWithValue("@us015", u018);
                            command.Parameters.AddWithValue("@us016", u019);
                            command.ExecuteNonQuery();
                            if (1 == debug)
                            {
                                SqlContext.Pipe.Send(ExtDCFunction.SplitLine);
                            }
                        }
                        ds.Tables["DaySum"].Clear();
                    }
                }
                ds.Tables["StartUserList"].Clear();
                levelStart++;
            }
            while (0 < levelCount);
        }
    }

   [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_GetSumReport")]
    public static void GetSumReport(int type, int userID, int level, int debug, DateTime dts, DateTime dte)
    { 
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            if (1 == debug)
            {
                SqlContext.Pipe.Send(string.Format("dts：{0}-dte：{1}", dts, dte));
                SqlContext.Pipe.Send(string.Format("dts：{0}-dte：{1}", dts, dte));
            }
            connection.Open();
            var command = connection.CreateCommand();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            string runSQL = string.Empty;
            switch (type)
            {
                case 0:
                    runSQL = "SELECT u001 FROM wgs049(NOLOCK) WHERE u012=@UserID";
                    break;
                case 1:
                    runSQL = "SELECT u002 FROM wgs048(NOLOCK) AS ur WHERE ur.u001=@UserID AND ur.u002<>@UserID AND ur.u002 NOT IN (SELECT u001 FROM wgs049(NOLOCK) AS u WHERE u.u012=@UserID);";
                    break;
                case 2:
                    runSQL = "SELECT u002 FROM wgs048(NOLOCK) AS ur WHERE ur.u001=@UserID AND ur.u002<>@UserID;";
                    break;
                case 3:
                    runSQL = "SELECT u002 FROM wgs048(NOLOCK) AS ur WHERE ur.u001=@UserID AND ur.u002<>@UserID AND ur.u002 NOT IN (SELECT u001 FROM wgs049(NOLOCK) AS u WHERE u.u012=@UserID);";
                    break;
            }
            if (0 == runSQL.Length)
            {
                if (1 == debug)
                {
                    pipe.Send("句子不能为空");
                }
            }
            else
            {
                command.CommandText = runSQL;
                command.Parameters.AddWithValue("@UserID", userID);
                da.SelectCommand = command;
                da.Fill(ds, "UserIDs");
                StringBuilder ids = new StringBuilder();
                string IDs = string.Empty;
                List<int> userIDList = new List<int>();
                if (0 < ds.Tables["UserIDs"].Rows.Count)
                {
                    foreach (DataRow item in ds.Tables["UserIDs"].Rows)
                    {
                        ids.Append(item[0].ToString() + ",");
                    }
                    IDs = ids.ToString();
                    IDs = IDs.Substring(0, IDs.Length - 1);
                    if (0 < ids.ToString().Length)
                    {
                        if (1 == debug)
                        {
                            pipe.Send(string.Format("IDS：{0}", IDs));
                        }
                    }
                }
                else
                {
                    if (1 == debug)
                    {
                        pipe.Send("找不到用户");
                    }
                }
                if (0 < IDs.Length)
                {
                    command.Parameters.Clear();
                    command.CommandText = string.Format("SELECT COUNT(1) AS u001, '' AS u002, SUM(dr004) AS dr004,SUM(dr005) AS dr005,SUM(dr006) AS dr006,SUM(dr007) AS dr007,SUM(dr008) AS dr008, SUM(dr009) AS dr009,SUM(dr010) AS dr010,SUM(dr011) AS dr011,SUM(dr012) AS dr012,SUM(dr013) AS dr013,SUM(dr014) AS dr014,SUM(dr015) AS dr015,SUM(dr016) AS dr016, SUM(dr017) AS dr017,SUM(dr018) AS dr018 FROM wgs042(NOLOCK) WHERE u001 IN({0}) AND dr002 >='{1}' AND dr002 <= '{2}';", IDs, dts, dte);
                    if (1 == debug)
                    {
                        pipe.Send(command.CommandText);
                    }
                    SqlDataReader dr = command.ExecuteReader();
                    pipe.Send(dr);
                }
                else
                {
                    SqlDataRecord record = new SqlDataRecord(
                        new SqlMetaData("u001", SqlDbType.Int),
                        new SqlMetaData("u002", SqlDbType.Char, 32),
                        new SqlMetaData("dr004", SqlDbType.Decimal),
                        new SqlMetaData("dr005", SqlDbType.Decimal),
                        new SqlMetaData("dr006", SqlDbType.Decimal),
                        new SqlMetaData("dr007", SqlDbType.Decimal),
                        new SqlMetaData("dr008", SqlDbType.Decimal),
                        new SqlMetaData("dr009", SqlDbType.Decimal),
                        new SqlMetaData("dr010", SqlDbType.Decimal),
                        new SqlMetaData("dr011", SqlDbType.Decimal),
                        new SqlMetaData("dr012", SqlDbType.Decimal),
                        new SqlMetaData("dr013", SqlDbType.Decimal),
                        new SqlMetaData("dr014", SqlDbType.Decimal),
                        new SqlMetaData("dr015", SqlDbType.Decimal),
                        new SqlMetaData("dr016", SqlDbType.Decimal),
                        new SqlMetaData("dr017", SqlDbType.Decimal),
                        new SqlMetaData("dr018", SqlDbType.Decimal)
                        );
                    record.SetValues(new object[] { 0, string.Empty, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m, 0.0000m });
                    pipe.Send(record);
                }
            }
        }
    }

      [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_GetSumReportMG")]
    public static void GetSumReportMG(int type, int level, int debug, DateTime dts, DateTime dte)
    { 
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            string runSQL = string.Empty;
            var command = connection.CreateCommand();
            command.CommandText = "SELECT u001 FROM wgs049(NOLOCK) WHERE u018=@Level;";
            command.Parameters.AddWithValue("@Level", @level);
        }
    }

      [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_DeleteUser")]
    public static void DeleteUser(string userID)
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            var cmd = connection.CreateCommand();
            cmd.CommandText = string.Format("SELECT * FROM wgs013(NOLOCK) WHERE u001 IN({0});", userID);
            da.SelectCommand = cmd;
            da.Fill(ds, "Names");
            if (ds.Tables["Names"].Rows.Count > 0)
            {
                string u002List = string.Empty;
                foreach (DataRow row in ds.Tables["Names"].Rows)
                {
                    u002List += row["u002"].ToString().Trim() + ",";
                }
                if (u002List.Length > 0)
                {
                    u002List = u002List.Substring(0, u002List.Length - 1);
                    cmd.CommandText = string.Format("DELETE FROM wgs012 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs013 WHERE u001 IN({0}) OR u002 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs014 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs017 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs018 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs019 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs020 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs021 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs022 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs045 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs050 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs023 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs025 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs026 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs030 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs031 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs035 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs039 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs042 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs043 WHERE tf005 IN({0}) OR tf002 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs044 WHERE msg004 IN({0}) OR msg005 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs046 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs047 WHERE cur005 IN({0}) OR cur009 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs048 WHERE u001 IN({0}) OR u002 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs049 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs051 WHERE us002 IN({0}) OR us005 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs052 WHERE u001 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = string.Format("DELETE FROM wgs053 WHERE tpi002 IN({0});", u002List);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

      [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_GetMonthRange")]
    public static void GetMonthRange(out DateTime dts, out DateTime dte, int type)
    {
        var dt = DateTime.Now.AddMonths(type == 1 ? -1 : 1);
        dts = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, 0);
        dte = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month), 23, 59, 59, 999);
    }
     [Microsoft.SqlServer.Server.SqlProcedure(Name = "CLR_USP_Print")]
      public static void print(string str) { 
    
        SqlPipe pipe = SqlContext.Pipe;
        pipe.Send(string.Format("hello,{0}",str));
      }
}
