using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Transactions;
using System.Reflection;
using System.IO;
using System.Text;
using System.Collections.Generic;
using GameDBObject;
public partial class UST
{
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_UserWithdrawUpdate", Target = "wgs020", Event = "AFTER  INSERT, DELETE, UPDATE")]
    public static void UserWithdrawUpdate()
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            if (TriggerAction.Update == triggerContext.TriggerAction)
            {
                DataSet ds = new DataSet();
                SqlCommand command = connection.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                command.CommandText = "SELECT * FROM INSERTED ORDER BY uw001";
                da.SelectCommand = command;
                da.Fill(ds, "DataNew");
                command.CommandText = "SELECT * FROM DELETED ORDER BY uw001";
                da.SelectCommand = command;
                da.Fill(ds, "DataOld");
                DataColumn[] pKey = new DataColumn[1];
                pKey[0] = ds.Tables["DataOld"].Columns["uw001"];
                ds.Tables["DataOld"].PrimaryKey = pKey;
                foreach (DataRow row in ds.Tables["DataNew"].Rows)
                {
                    DataRow oldRow = ds.Tables["DataOld"].Rows.Find(row["uw001"]);
                    int oldWState = (int)oldRow["uw006"];
                    int newWState = (int)row["uw006"];
                    DateTime okDateTime = (DateTime)row["uw005"];
                    if (1 == oldWState || 2 == oldWState)
                    {
                        throw new Exception("DBE[不能再更改数据]");
                    }
                    int u001 = (int)row["u001"];
                    string u002 = (string)row["u002"];
                    string u003 = (string)row["u003"];
                    command.CommandText = "SELECT u001, uf001, uf002, uf003, uf004, uf005, uf006, uf007, uf008, uf009, uf010, uf011 FROM wgs014(NOLOCK) WHERE u001=@u001";
                    command.Parameters.AddWithValue("@u001", u001);
                    da.SelectCommand = command;
                    da.Fill(ds, "UserExtF");
                    if (0 == ds.Tables["UserExtF"].Rows.Count)
                    {
                        throw new Exception("DBE[金额信息不存在]");
                    }
                    decimal holdAmount = (decimal)ds.Tables["UserExtF"].Rows[0]["uf003"];
                    decimal wAmount = (decimal)row["uw002"];
                    decimal wFeeAmount = (decimal)row["uw016"];
                    long uw001 = (long)row["uw001"];
                    decimal uxf002 = (decimal)ds.Tables["UserExtF"].Rows[0]["uf001"];
                    decimal uxf003 = wAmount;
                    decimal uxf007 = uxf002;
                    decimal uxf004 = (decimal)ds.Tables["UserExtF"].Rows[0]["uf004"];
                    decimal uxf005 = 0;
                    decimal uxf008 = uxf004 + uxf005;
                    DateTime uxf014 = okDateTime;
                    string uxf015 = string.Empty;
                    if (0 > holdAmount - wAmount)
                    {
                        throw new Exception("DBE[提现金额超过冻洁金额]");
                    }
                    decimal uxf009 = holdAmount;
                    decimal uxf010 = holdAmount - wAmount;
                    byte uxf016 = 0;
                    switch (newWState)
                    {
                        case 1:
                            uxf016 = 5;
                            uxf015 = ExtDCFunction.GetRDList()[5].Name;
                            break;
                        case 2:
                            uxf016 = 6;
                            uxf015 = ExtDCFunction.GetRDList()[6].Name;
                            uxf007 += wAmount;
                            break;
                    }
                    if (1 == newWState)
                    {
                        WriteDataChangeLog(ref command, u001, u002, u003, 0, 0, uw001, 0, 0, 0, 0, 0, 0, wAmount, 1, 0, 0, uxf016, uxf015, null, null, 0, okDateTime);
                        command.Parameters.Clear();
                        command.CommandText = "UPDATE wgs014 SET uf003=uf003-@wAmount WHERE u001=@u001";
                        command.Parameters.AddWithValue("@u001", u001);
                        command.Parameters.AddWithValue("@wAmount", wAmount);
                        command.ExecuteNonQuery();
                    }
                    else if (2 == newWState)
                    {
                        WriteDataChangeLog(ref command, u001, u002, u003, 0, 0, uw001, 0, 0, 0, 0, wAmount, 0, wAmount, 1, 0, 0, uxf016, uxf015, null, null, 0, okDateTime);
                        command.Parameters.Clear();
                        command.CommandText = "UPDATE wgs014 SET uf003=uf003-@wAmount,uf001=uf001+@wAmount WHERE u001=@u001";
                        command.Parameters.AddWithValue("@u001", u001);
                        command.Parameters.AddWithValue("@wAmount", wAmount);
                        command.ExecuteNonQuery();
                    }
                    if (1 == newWState)
                    {
                        SetDayReport(ref command, u001, u002, u003, 0, 0, 0, 0, 0, 0, wAmount, 0, 0, 0, wFeeAmount, 0, 0, 0);
                        string content = string.Format("您的提现申请已经确认，提现总金额：{0:N2}。", wAmount);
                        ExtDCFunction.SendMessage(ref command, u001, u002, u003, "提现成功", content, okDateTime);
                    }
                }
            }
            else if (TriggerAction.Insert == triggerContext.TriggerAction)
            {
                DataSet ds = new DataSet();
                SqlCommand command = connection.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                command.CommandText = "SELECT * FROM INSERTED ORDER BY uw001";
                da.SelectCommand = command;
                da.Fill(ds, "DataNew");
                foreach (DataRow row in ds.Tables["DataNew"].Rows)
                {
                    int newWState = (int)row["uw006"];
                    DateTime okDateTime = DateTime.Now;
                    if (0 != newWState)
                    {
                        throw new Exception("DBE[写入数据时状态异常，因为状态不是初始状态]");
                    }
                    int u001 = (int)row["u001"];
                    string u002 = (string)row["u002"];
                    string u003 = (string)row["u003"];
                    command.CommandText = "SELECT u001, uf001, uf002, uf003, uf004, uf005, uf006, uf007, uf008, uf009, uf010, uf011 FROM wgs014(NOLOCK) WHERE u001=@u001";
                    command.Parameters.AddWithValue("@u001", u001);
                    da.SelectCommand = command;
                    da.Fill(ds, "UserExtF");
                    if (0 == ds.Tables["UserExtF"].Rows.Count)
                    {
                        throw new Exception("DBE[金额信息不存在]");
                    }
                    decimal holdAmount = (decimal)ds.Tables["UserExtF"].Rows[0]["uf003"];
                    decimal wAmount = (decimal)row["uw002"];
                    decimal uw016 = (decimal)row["uw016"];
                    long uw001 = (long)row["uw001"];
                    decimal uxf002 = (decimal)ds.Tables["UserExtF"].Rows[0]["uf001"];
                    decimal uxf003 = wAmount;
                    decimal uxf007 = uxf002;
                    decimal uxf004 = (decimal)ds.Tables["UserExtF"].Rows[0]["uf004"];
                    decimal uxf005 = 0;
                    decimal uxf008 = uxf004 + uxf005;
                    DateTime uxf014 = okDateTime;
                    string uxf015 = string.Empty;
                    decimal uxf009 = holdAmount;
                    decimal uxf010 = holdAmount + wAmount;
                    uxf015 = ExtDCFunction.GetRDList()[14].Name;
                    byte uxf016 = 14;
                    if (0 == newWState)
                    {
                        WriteDataChangeLog(ref command, u001, u002, u003, 0, 0, uw001, 0, 0, 0, 0, wAmount, 1, wAmount, 0, 0, 0, uxf016, uxf015, null, null, 0, okDateTime);
                        command.Parameters.Clear();
                        command.CommandText = "UPDATE wgs014 SET uf003=uf003+@wAmount,uf001=uf001-@wAmount WHERE u001=@u001;";
                        command.Parameters.AddWithValue("@u001", u001);
                        command.Parameters.AddWithValue("@wAmount", wAmount);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_UserChargeUpdate", Target = "wgs019", Event = "AFTER  INSERT, DELETE, UPDATE")]
    public static void UserChargeUpdate()
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            if (TriggerAction.Update == triggerContext.TriggerAction)
            {
                DataSet dsUpdate = new DataSet();
                DataSet dsDelete = new DataSet();
                DataSet dsUserInfo = new DataSet();
                SqlCommand command = connection.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                command.CommandText = "SELECT * FROM INSERTED ORDER BY uc001";
                da.SelectCommand = command;
                da.Fill(dsUpdate);
                command.CommandText = "SELECT * FROM DELETED ORDER BY uc001";
                da.SelectCommand = command;
                da.Fill(dsDelete);
                DataColumn[] pKey = new DataColumn[1];
                pKey[0] = dsDelete.Tables[0].Columns["uc001"];
                dsDelete.Tables[0].PrimaryKey = pKey;
                command.Parameters.Clear();
                command.CommandText = "SELECT cfg003 FROM wgs027 WHERE cfg001=@cfg001;";
                command.Parameters.AddWithValue("@cfg001", "AGU_CHARGE_POINT");
                var chargePoint = command.ExecuteScalar();
                foreach (DataRow row in dsUpdate.Tables[0].Rows)
                {
                    DataRow oldRow = dsDelete.Tables[0].Rows.Find(row["uc001"]);
                    int userID = Convert.ToInt32(row["u001"]);
                    int chargeState = Convert.ToInt32(row["uc008"]);
                    int oldChargeState = Convert.ToInt32(oldRow["uc008"]);
                    DateTime okDateTime = Convert.ToDateTime(row["uc007"]);
                    if (1 == oldChargeState || 2 == oldChargeState)
                    {
                        throw new Exception("DBE[不能再更改数据]");
                    }
                    command.Parameters.Clear();
                    command.CommandText = "SELECT u001, uf001, uf002, uf003, uf004, uf005, uf006, uf007, uf008, uf009, uf010, uf011, uf012 FROM wgs014(NOLOCK) WHERE u001=@u001";
                    command.Parameters.AddWithValue("@u001", userID);
                    da.SelectCommand = command;
                    da.Fill(dsUserInfo, "UserExtF");
                    command.CommandText = "SELECT u001,u002,u003 FROM wgs012(NOLOCK) WHERE u001=@u001";
                    da.SelectCommand = command;
                    da.Fill(dsUserInfo, "User");
                    if (0 == dsUserInfo.Tables["UserExtF"].Rows.Count)
                    {
                        throw new Exception("DBE[金额信息不存在]");
                    }
                    int u001 = (int)dsUserInfo.Tables["User"].Rows[0]["u001"];
                    string u002 = (string)dsUserInfo.Tables["User"].Rows[0]["u002"];
                    string u003 = dsUserInfo.Tables["User"].Rows[0].IsNull("u003") ? string.Empty : dsUserInfo.Tables["User"].Rows[0]["u003"].ToString().Trim();
                    decimal chargeMoney = (decimal)row["uc003"];
                    decimal uc013 = (decimal)row["uc013"];
                    int uc014 = (byte)row["uc014"];
                    int mu001x = (int)row["mu001x"];
                    long uc001 = (long)row["uc001"];
                    int ct001 = (int)row["ct001"];
                    byte uxf016 = 0;
                    string uxf015 = string.Empty;
                    switch (chargeState)
                    {
                        case 1:
                            uxf016 = 4;
                            uxf015 = ExtDCFunction.GetRDList()[4].Name;
                            break;
                        case 2:
                            uxf016 = 23;
                            uxf015 = ExtDCFunction.GetRDList()[23].Name;
                            break;
                    }
                    if (0 < mu001x)
                    {
                    }
                    bool isSystemCharge = false;
                    bool isSystemNotMomeyAccount = false;
                    if (1 == chargeState)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "SELECT cfg003 FROM wgs027 WHERE cfg001=@cfg001;";
                        command.Parameters.AddWithValue("@cfg001", "SYS_DATA_CHANGE_TYPE");
                        var sysType = (string)command.ExecuteScalar();
                        var sysTypeList = sysType.Split(',');
                        foreach (var item in sysTypeList)
                        {
                            var itemSplit = item.Split(':');
                            if (3 == itemSplit.Length)
                            {
                                if (ct001 == int.Parse(itemSplit[2]))
                                {
                                    uxf016 = byte.Parse(itemSplit[0]);
                                    uxf015 = itemSplit[1];
                                    isSystemCharge = true;
                                    isSystemNotMomeyAccount = true;
                                    break;
                                }
                            }
                            continue;
                        }
                        if (isSystemCharge)
                        {
                            SetDayReport(ref command, u001, u002, u003, 0, 0, 0, 0, 0, 0, 0, chargeMoney, 0, 0, 0, 0, 0, 0);
                        }
                        else
                        {
                            SetDayReport(ref command, u001, u002, u003, 0, chargeMoney, 0, 0, 0, 0, 0, 0, 0, 0, uc013, 0, 0, 0);
                        }

                        string content = string.Format("{0}，总金额：{1:N2}已经到账。", uxf015, chargeMoney);
                        ExtDCFunction.SendMessage(ref command, u001, u002, u003, uxf015, content, okDateTime);
                        WriteDataChangeLog(ref command, u001, u002, u003, 0, 0, 0, 0, 0, 0, 0, chargeMoney, 0, 0, 0, 0, 0, uxf016, uxf015, null, null, 0, okDateTime);
                        command.Parameters.Clear();
                        command.CommandText = "UPDATE wgs014 SET uf001=uf001+@uf001 WHERE u001=@u001";
                        command.Parameters.AddWithValue("@uf001", chargeMoney);
                        command.Parameters.AddWithValue("@u001", u001);
                        command.ExecuteNonQuery();
                        if (false == isSystemCharge)
                        {
                            var havePointPercent = Convert.ToDecimal(chargePoint) / 100.000000m;
                            var havePoint = havePointPercent * chargeMoney;
                            WriteDataChangeLog(ref command, u001, u002, u003, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, havePoint, 0, 18, ExtDCFunction.GetRDList()[18].Name, null, null, 0, okDateTime);
                            command.Parameters.Clear();
                            command.CommandText = "UPDATE wgs014 SET uf004=uf004+@uf004 WHERE u001=@u001;";
                            command.Parameters.AddWithValue("@u001", u001);
                            command.Parameters.AddWithValue("@uf004", havePoint);
                            command.ExecuteNonQuery();
                        }
                        if (false == isSystemNotMomeyAccount)
                        {
                            var havePointPercent = Convert.ToDecimal(chargePoint) / 100.000000m;
                            var havePoint = havePointPercent * chargeMoney;
                            WriteDataChangeLog(ref command, u001, u002, u003, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, havePoint, 0, 18, ExtDCFunction.GetRDList()[18].Name, null, null, 0, okDateTime);
                            command.Parameters.Clear();
                            command.CommandText = "UPDATE wgs014 SET uf012=uf012+@uf012 WHERE u001=@u001;";
                            command.Parameters.AddWithValue("@u001", u001);
                            command.Parameters.AddWithValue("@uf012", chargeMoney);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_OrderChange", Target = "wgs022", Event = "AFTER  INSERT, DELETE, UPDATE")]
    public static void OrderChange()
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            try
            {
                if (TriggerAction.Insert == triggerContext.TriggerAction)
                {
                    DataSet ds = new DataSet();
                    SqlCommand command = connection.CreateCommand();
                    SqlDataAdapter da = new SqlDataAdapter();
                    command.CommandText = "SELECT * FROM INSERTED ORDER BY so007;";
                    da.SelectCommand = command;
                    da.Fill(ds, "DataInsert");
                    foreach (DataRow row in ds.Tables["DataInsert"].Rows)
                    {
                        DateTime okDateTime = DateTime.Now;
                        var u001 = (int)row["u001"];
                        var u002 = (string)row["u002"];
                        var u003 = row["u003"] == null ? string.Empty : (string)row["u003"];
                        var gs001 = (int)row["gs001"];
                        var so006 = (byte)row["so006"];
                        var so005 = (int)row["so005"];
                        var gs002 = (string)row["gs002"];
                        var so003 = (int)row["so003"];
                        var so027 = (decimal)row["so027"];
                        var so028 = (long)row["so028"];
                        var so004 = (decimal)row["so004"];
                        var sto001 = (long)row["sto001"];
                        if (1 > so006 || 3 < so006)
                        {
                            throw new Exception(string.Format("DBE[元、角、分模式不正确]"));
                        }
                        if (1 > so005)
                        {
                            throw new Exception(string.Format("DBE[倍数不能小于1]"));
                        }
                        if (0 == gs001)
                        {
                            throw new Exception(string.Format("DBE[期号信息有错]"));
                        }
                        command.Parameters.Clear();
                        command.CommandText = "SELECT gs002,gs004 FROM wgs005(NOLOCK) WHERE gs001=@gs001;";
                        command.Parameters.AddWithValue("@gs001", gs001);
                        da.SelectCommand = command;
                        da.Fill(ds, "GameSession");
                        var gsCloseTime = (DateTime)ds.Tables["GameSession"].Rows[0]["gs004"];
                        var gsIssue = (string)ds.Tables["GameSession"].Rows[0]["gs002"];
                        var diffTime = DateTime.Now - gsCloseTime;
                        if (0 < diffTime.Seconds)
                        {
                            throw new Exception(string.Format("DBE[{0}期已经封盘]", gsIssue));
                        }
                        command.Parameters.Clear();
                        command.CommandText = "SELECT u001, uf001, uf002, uf003, uf004, uf005, uf006, uf007, uf008, uf009, uf010, uf011 FROM wgs014(NOLOCK) WHERE u001=@u001";
                        command.Parameters.AddWithValue("@u001", u001);
                        da.SelectCommand = command;
                        da.Fill(ds, "UserExtF");
                        if (0 == ds.Tables["UserExtF"].Rows.Count)
                        {
                            throw new Exception("DBE[金额信息不存在]");
                        }
                        DataRow ufRow = ds.Tables["UserExtF"].Rows[0];
                        decimal ufCurrent = (decimal)ufRow["uf001"];
                        int g001 = (int)row["g001"];
                        int gm001 = (int)row["gm001"];
                        int gm002 = (int)row["gm002"];
                        int gc001 = (int)row["gc001"];
                        var so001 = (long)row["so001"];
                        decimal orderAmount = (decimal)row["so004"];
                        if (0 != so028 && 0 != so027)
                        {
                            command.Parameters.Clear();
                            command.CommandText = "SELECT sco014 FROM wgs031(NOLOCK) WHERE sco001=@sco001;";
                            command.Parameters.AddWithValue("@sco001", so028);
                            var joinUsers = command.ExecuteScalar();
                            command.CommandText = "SELECT sco011 FROM wgs031 WHERE sco001=@sco001;";
                            var haveCombineCount = (int)command.ExecuteScalar();
                            if (0 > haveCombineCount - so003)
                            {
                                throw new Exception(string.Format("DBE[可合买注数{0}]", haveCombineCount));
                            }
                            else if (0 == haveCombineCount - so003)
                            {
                                command.CommandText = "UPDATE wgs031 SET sco009=1 WHERE sco001=@sco001 AND sco009 < 2;";
                                command.ExecuteNonQuery();
                            }
                            var joinUserData = string.Empty;
                            List<string> joinUsersList = null;
                            if (null == joinUsers)
                            {
                                joinUserData = u002.Trim();
                            }
                            else
                            {
                                var tempUsers = joinUsers.ToString();
                                joinUserData = tempUsers;
                                if (string.Empty == tempUsers || string.IsNullOrEmpty(tempUsers))
                                {
                                    joinUserData = u002.Trim();
                                }
                                else
                                {
                                    var tempUsersSplit = tempUsers.Split('|');
                                    joinUsersList = new List<string>(tempUsersSplit);
                                    var findName = joinUsersList.Find(delegate(string str) 
                                    {
                                        return str == u002.Trim();
                                    });
                                    if (string.IsNullOrEmpty(tempUsersSplit[0]) && 1 == tempUsersSplit.Length)
                                    {
                                        joinUserData = u002.Trim();
                                    }
                                    else if (string.IsNullOrEmpty(findName))
                                    {
                                        joinUserData += "|" + u002.Trim();
                                    }
                                }
                            }
                            command.CommandText = "UPDATE wgs031 SET sco014=@UserList, sco012=@joinCount,sco011=sco011-@orderTimes,sco008=sco008+@orderAmount WHERE sco001=@sco001 AND sco011-@orderTimes >= 0;";
                            command.Parameters.AddWithValue("@joinCount", joinUserData.Split('|').Length);
                            command.Parameters.AddWithValue("@orderTimes", so003);
                            command.Parameters.AddWithValue("@UserList", joinUserData);
                            command.Parameters.AddWithValue("@orderAmount", so004);
                            command.ExecuteNonQuery();
                        }
                        if (0 == so027 && 0 < so028)
                        {
                            command.Parameters.Clear();
                            command.CommandText = "UPDATE wgs031 SET so001=@so001 WHERE sco001=@so028;";
                            command.Parameters.AddWithValue("@so001", so001);
                            command.Parameters.AddWithValue("@so028", so028);
                            command.ExecuteNonQuery();
                        }
                        byte uxf016 = 9;
                        string uxf015 = ExtDCFunction.GetRDList()[9].Name;
                        if (0 < so028)
                        {
                            uxf016 = 25;
                            uxf015 = ExtDCFunction.GetRDList()[25].Name;
                        }
                        if (0 < sto001)
                        {
                            uxf016 = 24;
                            uxf015 = ExtDCFunction.GetRDList()[24].Name;
                        }
                        if (0 > ufCurrent - orderAmount)
                        {
                            throw new Exception("DBE[余额不足1]");
                        }
                        WriteDataChangeLog(ref command, u001, u002, u003, so001, 0, 0, g001, gc001, gm001, gm002, orderAmount, 1, 0, 0, 0, 0, uxf016, uxf015, gs002.Trim(), null, 0, okDateTime);
                        command.CommandText = "UPDATE wgs014 SET uf001=uf001-@orderAmount WHERE u001=@u001 AND uf001-@orderAmount>=0";
                        command.Parameters.AddWithValue("@orderAmount", orderAmount);
                        var updateRow = command.ExecuteNonQuery();
                        if (0 == updateRow)
                        {
                            throw new Exception("DBE[余额不足2]");
                        }
                        SetDayReport(ref command, u001, u002, u003, orderAmount, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    }
                    command.CommandText = "INSERT INTO wgs045 SELECT * FROM INSERTED";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO wgs050 SELECT * FROM INSERTED";
                    command.ExecuteNonQuery();
                }
                else if (TriggerAction.Update == triggerContext.TriggerAction)
                {
                    DataSet ds = new DataSet();
                    SqlCommand command = connection.CreateCommand();
                    SqlDataAdapter da = new SqlDataAdapter();
                    command.CommandText = "SELECT * FROM INSERTED ORDER BY so007;";
                    da.SelectCommand = command;
                    da.Fill(ds, "DataInsert");
                    command.CommandText = "SELECT * FROM DELETED ORDER BY so007";
                    da.SelectCommand = command;
                    da.Fill(ds, "DataDelete");
                    command.CommandText = "DELETE FROM wgs045 WHERE so001 IN(SELECT so001 FROM INSERTED);";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO wgs045 SELECT * FROM INSERTED;";
                    command.ExecuteNonQuery();
                    DataColumn[] pKey = new DataColumn[1];
                    pKey[0] = ds.Tables["DataDelete"].Columns["so001"];
                    ds.Tables["DataDelete"].PrimaryKey = pKey;
                    foreach (DataRow row in ds.Tables["DataInsert"].Rows)
                    {
                        DateTime okDateTime = DateTime.Now;
                        int g001 = (int)row["g001"];
                        int gc001 = (int)row["gc001"];
                        int gm001 = (int)row["gm001"];
                        int gm002 = (int)row["gm002"];
                        int gs001 = (int)row["gs001"];
                        string gs002 = (string)row["gs002"];
                        long so001 = (long)row["so001"];
                        decimal so013 = (decimal)row["so013"];
                        long sto001 = (long)row["sto001"];
                        int so003 = (int)row["so003"];
                        byte so006 = (byte)row["so006"];
                        string so032 = row["so032"].ToString();
                        string so029 = row.IsNull("so029") ? string.Empty : row["so029"].ToString();
                        decimal so018 = (decimal)row["so018"];
                        var so011 = (decimal)row["so011"];
                        var orderAmount = (decimal)row["so004"];
                        var orderWLAmt =(decimal)row["so010"];
                        var u001 = (int)row["u001"];
                        var u002 = (string)row["u002"];
                        var u003 = row["u003"] == null ? string.Empty : row["u003"].ToString();
                        var mu001 = (int)row["mu001"];
                        DataRow oldRow = ds.Tables["DataDelete"].Rows.Find(row["so001"]);
                        byte oldso016 = (byte)oldRow["so016"];
                        byte newso016 = (byte)row["so016"];
                        var oldso021 = (byte)oldRow["so021"];
                        var newso021 = (byte)row["so021"];
                        var oldso022 = (byte)oldRow["so022"];
                        var newso022 = (byte)row["so022"];
                        var oldso020 = oldRow.IsNull("so020") ? string.Empty : oldRow["so020"].ToString();
                        var newso020 = row.IsNull("so020") ? string.Empty : row["so020"].ToString();
                        var oldOrderAmount = (decimal)oldRow["so004"];
                        int odlso003 = (int)oldRow["so003"];
                        decimal so010 = (decimal)row["so010"];
                        var oldso011 = (decimal)oldRow["so011"];
                        if (so011 != oldso011)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@so001", so001);
                            command.CommandText = "DELETE FROM wgs045 WHERE so001=@so001;";
                            command.ExecuteNonQuery();
                            command.CommandText = "DELETE FROM wgs050 WHERE so001=@so001;";
                            command.ExecuteNonQuery();
                            command.CommandText = "INSERT INTO wgs045 SELECT * FROM INSERTED WHERE so001=@so001";
                            command.ExecuteNonQuery();
                            command.CommandText = "INSERT INTO wgs050 SELECT * FROM INSERTED WHERE so001=@so001";
                            command.ExecuteNonQuery();
                            continue;
                        }
                        var so028 = (long)row["so028"];
                        var so027 = (decimal)row["so027"];
                        if (0 < so028 && 1.0000m <= so027 && (0 == oldso016 && 0 == newso016) && 0 < newso021 && 0 == oldso021)
                        {
                            var combinePayMoney = orderAmount;
                            var combinePayTimes = so003;
                            WriteDataChangeLog(ref command, u001, u002, u003, so001, 0, 0, g001, gc001, gm001, gm002, combinePayMoney, 0, 0, 0, 0, 0, 27, ExtDCFunction.GetRDList()[27].Name, gs002, null, 0, okDateTime);
                            command.Parameters.Clear();
                            command.CommandText = "UPDATE wgs014 SET uf001=uf001+@CancelMoney WHERE u001=@u001;";
                            command.Parameters.AddWithValue("@u001", u001);
                            command.Parameters.AddWithValue("@CancelMoney", combinePayMoney);
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                            command.CommandText = "UPDATE wgs031 SET sco008=sco008-@CancelMoney,sco011=sco011+@Times,sco009=0 WHERE sco001=@sco001 AND sco009 < 2;";
                            command.Parameters.AddWithValue("@CancelMoney", combinePayMoney);
                            command.Parameters.AddWithValue("@Times", combinePayTimes);
                            command.Parameters.AddWithValue("@sco001", so028);
                            command.ExecuteNonQuery();
                            command.CommandText = "SELECT u002,count(1) as RecordCount FROM wgs022(NOLOCK) GROUP BY u002,so028,so027,so021 HAVING(so028=@so028 AND so027=1 AND so021=0)";
                            command.Parameters.AddWithValue("@so028", so028);
                            da.SelectCommand = command;
                            da.Fill(ds, "GoingUser");
                            string userNames = "";
                            foreach (DataRow userNameRow in ds.Tables["GoingUser"].Rows)
                            {
                                userNames += userNameRow["u002"].ToString().Trim() + "|";
                            }
                            if (false == string.IsNullOrEmpty(userNames) )
                            {
                                userNames = userNames.Substring(0, userNames.Length - 1);
                            }
                            int noCancelRow = ds.Tables["GoingUser"].Rows.Count;
                            int haveMemberCount = 0;
                            command.CommandText = "UPDATE wgs031 SET sco014=@NowHaveMember,sco012=@NowHaveMemberCount WHERE sco001=@sco001;";
                            command.Parameters.AddWithValue("@NowHaveMember", string.Empty==userNames ? null :userNames);
                            var userNameSplit = userNames.Split('|');
                            if (0 == noCancelRow)
                            {
                                haveMemberCount = 0;
                            }
                            else
                            {
                                haveMemberCount = userNameSplit.Length;
                            }
                            command.Parameters.AddWithValue("@NowHaveMemberCount", haveMemberCount);
                            command.ExecuteNonQuery();
                            continue;
                        }
                        if (oldso020.Trim() != newso020.Trim())
                        {
                            continue;
                        }
                        if (0 == oldso016 && 0 < newso016)
                        {
                            command.Parameters.Clear();
                            command.CommandText = "SELECT cfg003 FROM wgs027(NOLOCK) WHERE cfg001=@cfg001";
                            command.Parameters.AddWithValue("@cfg001", "AGU_ORDER_POINT");
                            var orderDEFPoint = command.ExecuteScalar();
                            if (1 == newso022)
                            {
                                command.Parameters.Clear();
                                command.CommandText = "INSERT INTO wgs053(tpi002, tpi003, tpi004, tpi005, tpi006, tpi007, tpi008, tpi009, gm001, gm002, so001, g001,gs002) VALUES(@u001, @u002, @u003, @so012, @so004, @so010, @so018, @dt, @gm001, @gm002, @so001, @g001,@gs002);";
                                command.Parameters.AddWithValue("@u001", u001);
                                command.Parameters.AddWithValue("@u002", u002);
                                command.Parameters.AddWithValue("@u003", u003);
                                command.Parameters.AddWithValue("@so012", row["so012"]);
                                command.Parameters.AddWithValue("@so004", row["so004"]);
                                command.Parameters.AddWithValue("@so010", row["so010"]);
                                command.Parameters.AddWithValue("@so018", row["so018"]);
                                command.Parameters.AddWithValue("@dt", okDateTime);
                                command.Parameters.AddWithValue("@gm001", gm001);
                                command.Parameters.AddWithValue("@gm002", gm002);
                                command.Parameters.AddWithValue("@so001", so001);
                                command.Parameters.AddWithValue("@g001", g001);
                                command.Parameters.AddWithValue("@gs002", gs002);
                                command.ExecuteNonQuery();
                            }
                            decimal orderEFPPs = Convert.ToDecimal(orderDEFPoint) == 0 ? 0m : Convert.ToDecimal(orderDEFPoint) / 100.00m;
                            if (1 == newso022)
                            {
                                byte writeWinID = 10;
                                string writeWinString = ExtDCFunction.GetRDList()[10].Name;
                                if (0 < so028)
                                {
                                    writeWinID = 28;
                                    writeWinString = ExtDCFunction.GetRDList()[28].Name;
                                    string sendContent = string.Format("您在[g{0}]第{1}期{2}合买中奖，中奖金额{3}，返点{4}", g001, gs002.Trim(), so027 == 1 ? "参与" : "发起", so010, so018);
                                    ExtDCFunction.SendMessage(ref command, u001, u002, u003, "合买中奖", sendContent, okDateTime);
                                }
                                WriteDataChangeLog(ref command, u001, u002, u003, so001, 0, 0, g001, gc001, gm001, gm002, orderWLAmt, 0, 0, 0, 0, 0, writeWinID, writeWinString, gs002, null, 0, okDateTime);
                                command.Parameters.Clear();
                                command.CommandText = "UPDATE wgs014 SET uf001=uf001+@uf001 WHERE u001=@u001;";
                                command.Parameters.AddWithValue("@uf001", orderWLAmt);
                                command.Parameters.AddWithValue("@u001", u001);
                                command.ExecuteNonQuery();
                                SetDayReport(ref command, u001, u002, u003, 0, 0, orderWLAmt, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                                if (0 < sto001)
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = "UPDATE wgs030 SET sto008=sto008+@orderAmount WHERE u001=@u001 AND sto001=@sto001;";
                                    command.Parameters.AddWithValue("@orderAmount", orderWLAmt);
                                    command.Parameters.AddWithValue("@u001", u001);
                                    command.Parameters.AddWithValue("@sto001", sto001);
                                    command.ExecuteNonQuery();
                                    ChangeTraceOrderStatus(ref command, sto001);
                                }
                            }
                            if (0 == so027 && 0 < so028)
                            {
                                command.Parameters.Clear();
                                command.CommandText = "SELECT SUM(so004) FROM wgs022(NOLOCK) WHERE so028=@so028 AND so027=1 AND so021=0 AND so009<>2;";
                                command.Parameters.AddWithValue("@so028", so028);
                                var joinSumObj = command.ExecuteScalar();
                                if (DBNull.Value != joinSumObj)
                                {
                                    if (0 < Convert.ToDecimal(joinSumObj))
                                    {
                                        var getJoinMoney = Convert.ToDecimal(joinSumObj);
                                        WriteDataChangeLog(ref command, u001, u002, u003, so001, 0, 0, g001, gc001, gm001, gm002, getJoinMoney, 0, 0, 0, 0, 0, 26, ExtDCFunction.GetRDList()[26].Name, gs002, null, 0, okDateTime);
                                        command.Parameters.Clear();
                                        command.CommandText = "UPDATE wgs014 SET uf001=uf001+@uf001 WHERE u001=@u001;";
                                        command.Parameters.AddWithValue("@uf001", getJoinMoney);
                                        command.Parameters.AddWithValue("@u001", u001);
                                        command.ExecuteNonQuery();
                                        SetDayReport(ref command, u001, u002, u003, 0 - getJoinMoney, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                                    }
                                }
                                command.Parameters.Clear();
                                command.CommandText = "UPDATE wgs031 SET sco009=3 WHERE sco001=@so028;";
                                command.Parameters.AddWithValue("@so028", so028);
                                command.ExecuteNonQuery();
                            }
                            if (0 < so018)
                            {
                                WriteDataChangeLog(ref command, u001, u002, u003, so001, 0, 0, g001, gc001, gm001, gm002, so018, 0, 0, 0, 0, 0, 8, ExtDCFunction.GetRDList()[8].Name, gs002, null, 0, okDateTime);
                                command.Parameters.Clear();
                                command.CommandText = "UPDATE wgs014 SET uf001=uf001+@uf001 WHERE u001=@u001;";
                                command.Parameters.AddWithValue("@uf001", so018);
                                command.Parameters.AddWithValue("@u001", u001);
                                command.ExecuteNonQuery();
                                SetDayReport(ref command, u001, u002, u003, 0, 0, 0, so018, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                            }
                            if (0 < so028 && 1 == so027 && 0 == so018)
                            {
                                decimal justPayPostPoint = (decimal)orderAmount * (decimal)so013;
                                command.Parameters.Clear();
                                command.CommandText = "SELECT u001 FROM wgs022(NOLOCK) WHERE so028=@so028 AND so027=0;";
                                command.Parameters.AddWithValue("@so028", so028);
                                var justPayPostUserID = (int)command.ExecuteScalar();
                                WriteDataChangeLog(ref command, justPayPostUserID, u002, u003, so001, 0, 0, g001, gc001, gm001, gm002, justPayPostPoint, 0, 0, 0, 0, 0, 29, ExtDCFunction.GetRDList()[29].Name, null, null, 0, okDateTime);
                                command.Parameters.Clear();
                                command.CommandText = "UPDATE wgs014 SET uf001=uf001+@uf001 WHERE u001=@u001;";
                                command.Parameters.AddWithValue("@uf001", justPayPostPoint);
                                command.Parameters.AddWithValue("@u001", justPayPostUserID);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                                command.CommandText = "SELECT cfg003 FROM wgs027 WHERE cfg001=@cfg001;";
                                command.Parameters.AddWithValue("@cfg001", "SYS_COMBUY_GET_PERCENT");
                                var getJionPercent = Convert.ToDecimal( command.ExecuteScalar() );
                                if (0 < getJionPercent)
                                {
                                    var payPercentToCreater = (orderWLAmt / (100.0000m - getJionPercent)) * getJionPercent;
                                    WriteDataChangeLog(ref command, justPayPostUserID, u002, u003, so001, 0, 0, g001, gc001, gm001, gm002, payPercentToCreater, 0, 0, 0, 0, 0, 30, ExtDCFunction.GetRDList()[30].Name, gs002, null, 0, okDateTime);
                                    command.Parameters.Clear();
                                    command.CommandText = "UPDATE wgs014 SET uf001=uf001+@uf001 WHERE u001=@u001;";
                                    command.Parameters.AddWithValue("@uf001", payPercentToCreater);
                                    command.Parameters.AddWithValue("@u001", justPayPostUserID);
                                    command.ExecuteNonQuery();
                                }
                            }
                            if (false == string.IsNullOrEmpty(so029))
                            {
                                decimal orderPoint = so013 * 100m;
                                PPMPay(ref command, so029, orderPoint, so001, g001, gc001, gm001, gm002, orderAmount);
                            }
                            if (0 < orderEFPPs)
                            {
                                decimal orderHavePoint = orderAmount * orderEFPPs;
                                WriteDataChangeLog(ref command, u001, u002, u003, so001, 0, 0, g001, gc001, gm001, gm002, 0, 0, 0, 0, orderHavePoint, 0, 17, ExtDCFunction.GetRDList()[17].Name, gs002, null, 0, okDateTime);
                                command.Parameters.Clear();
                                command.CommandText = "UPDATE wgs014 SET uf004=uf004+@uf004 WHERE u001=@u001;";
                                command.Parameters.AddWithValue("@uf004", orderHavePoint);
                                command.Parameters.AddWithValue("@u001", u001);
                                command.ExecuteNonQuery();
                                SetDayReport(ref command, u001, u002, u003, 0, 0, 0, 0, orderHavePoint, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                            }
                            ChangeTraceOrderStatus(ref command, sto001);
                            continue;
                        }
                        if (oldso020.Trim() != newso020.Trim())
                        {
                        }
                        else
                        {
                            if (0 != oldso022)
                            {
                                throw new Exception("DBE[已经结算不能撤单]");
                            }
                        }
                        if (0 == oldso021 && 0 < newso021)
                        {
                            command.Parameters.Clear();
                            command.CommandText = "SELECT gs004 FROM wgs005(NOLOCK) WHERE gs001=@gs001";
                            command.Parameters.AddWithValue("@gs001", gs001);
                            var gsCloseTime = DateTime.Now;
                            try
                            {
                                gsCloseTime = (DateTime)command.ExecuteScalar();
                            }
                            catch
                            {
                                throw new Exception("DBE[游戏期数不存在，可能已被删除]");
                            }
                            var diffTime = DateTime.Now - gsCloseTime;
                            if (0 < diffTime.Seconds)
                            {
                                if (mu001 < 1)
                                {
                                    throw new Exception("DBE[已经超过撤单时间]");
                                }
                            }
                            command.Parameters.Clear();
                            command.CommandText = "SELECT u001, uf001, uf002, uf003, uf004, uf005, uf006, uf007, uf008, uf009, uf010, uf011 FROM wgs014(NOLOCK) WHERE u001=@u001";
                            command.Parameters.AddWithValue("@u001", u001);
                            da.SelectCommand = command;
                            da.Fill(ds, "UserExtF");
                            if (0 == ds.Tables["UserExtF"].Rows.Count)
                            {
                                throw new Exception("DBE[金额信息不存在]");
                            }
                            string uxf015 = string.Empty;
                            byte uxf016 = 0;
                            if (1 == newso021)
                            {
                                uxf015 = ExtDCFunction.GetRDList()[11].Name;
                                uxf016 = 11;
                            }
                            else if (2 == newso021)
                            {
                                uxf015 = ExtDCFunction.GetRDList()[12].Name;
                                uxf016 = 12;
                            }
                            else if (3 == newso021)
                            {
                                uxf015 = ExtDCFunction.GetRDList()[13].Name;
                                uxf016 = 13;
                            }
                            else if (4 == newso021)
                            {
                                uxf015 = ExtDCFunction.GetRDList()[15].Name;
                                uxf016 = 15;
                            }
                            if (0 < so028)
                            {
                                uxf015 = uxf015 = ExtDCFunction.GetRDList()[27].Name;
                                uxf016 = 27;
                            }
                            WriteDataChangeLog(ref command, u001, u002, u003, so001, 0, 0, g001, gc001, gm001, gm002, orderAmount, 0, 0, 0, 0, 0, uxf016, uxf015, gs002.Trim(), null, 0, okDateTime);
                            if (0 < sto001)
                            {
                                command.Parameters.Clear();
                                command.CommandText = "UPDATE wgs030 SET sto007=(sto007-@orderAmount) WHERE u001=@u001 AND sto001=@sto001;";
                                command.Parameters.AddWithValue("@orderAmount", orderAmount);
                                command.Parameters.AddWithValue("@u001", u001);
                                command.Parameters.AddWithValue("@sto001", sto001);
                                command.ExecuteNonQuery();
                                ChangeTraceOrderStatus(ref command, sto001);
                            }
                            command.Parameters.Clear();
                            command.CommandText = "UPDATE wgs014 SET uf001=(uf001+@orderAmount) WHERE u001=@u001";
                            command.Parameters.AddWithValue("@orderAmount", orderAmount);
                            command.Parameters.AddWithValue("@u001", u001);
                            command.ExecuteNonQuery();
                            if (0 == so027 && 0 < so028)
                            {
                                command.Parameters.Clear();
                                command.CommandText = "UPDATE wgs031 SET sco009=2 WHERE sco001=@sco001 AND sco009 < 2;";
                                command.Parameters.AddWithValue("@sco001", so028);
                                command.ExecuteNonQuery();
                            }
                            SetDayReport(ref command, u001, u002, u003, 0-orderAmount, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                throw error;
            }
        }
    }
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_UpdateUserLoginInfo", Target = "wgs026", Event = "AFTER  INSERT, DELETE, UPDATE")]    
    public static void UpdateUserLoginInfo()
    { 
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            if (TriggerAction.Insert == triggerContext.TriggerAction)
            {
                DateTime okDateTime = DateTime.Now;
                DataSet ds = new DataSet();
                SqlCommand command = connection.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                command.CommandText = "SELECT * FROM INSERTED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataInsert");
                command.Parameters.Clear();
                command.CommandText = "SELECT cfg003 FROM wgs027 WHERE cfg001=@cfg001;";
                command.Parameters.AddWithValue("@cfg001", "AGU_LOGIN_POINT");
                var loginPoint = Convert.ToDecimal(command.ExecuteScalar());
                foreach (DataRow row in ds.Tables["DataInsert"].Rows)
                {
                    int u001 = (int)row["u001"];
                    string u002 = row["u002"].ToString().Trim();
                    string u003 = row.IsNull("u003") ? string.Empty : row["u003"].ToString().Trim();
                    command.Parameters.Clear();
                    command.CommandText = "UPDATE wgs012 SET u007=@LoginDateTime,u022=@LoginIP WHERE u001=@UserID";
                    command.Parameters.AddWithValue("@LoginDateTime", (DateTime)row["ulg002"]);
                    command.Parameters.AddWithValue("@LoginIP", (string)row["ulg004"]);
                    command.Parameters.AddWithValue("@UserID", u001);
                    command.ExecuteNonQuery();
                    if (0 < loginPoint)
                    {
                        WriteDataChangeLog(ref command, u001, u002, u003, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, loginPoint, 0, 19, ExtDCFunction.GetRDList()[19].Name, null, null, 0, okDateTime);
                        command.Parameters.Clear();
                        command.CommandText = "UPDATE wgs014 SET uf004=uf004+@uf004 WHERE u001=@u001;";
                        command.Parameters.AddWithValue("@uf004", loginPoint);
                        command.Parameters.AddWithValue("@u001", (int)row["u001"]);
                        command.ExecuteNonQuery();
                        SetDayReport(ref command, u001, u002, u003, 0, 0, 0, 0, loginPoint, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    }
                }
                ds.Dispose();
                da.Dispose();
            }
        }
    }
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_PointChange", Target = "wgs017", Event = "AFTER  INSERT, DELETE, UPDATE")]
    public static void PointChange()
    { 
        /*
         * 检测相同返点数
         */
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        { 
            connection.Open();
            DataSet ds = new DataSet();
            SqlCommand command = connection.CreateCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            if (TriggerAction.Insert == triggerContext.TriggerAction)
            {
                command.CommandText = "SELECT cfg003 FROM wgs027(NOLOCK) WHERE cfg001='SYS_AGU_SAME_POINT_LIMIT';";
                var limitConfig = (string)command.ExecuteScalar();
                var limitConfigSplit = limitConfig.Split(',');
                Dictionary<string, string> dicLimit = new Dictionary<string, string>();
                foreach (var litem in limitConfigSplit)
                {
                    var itemData = litem.Split(':');
                    dicLimit.Add(itemData[0], itemData[1]);
                }
                command.CommandText = "SELECT * FROM INSERTED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataInsert");
                foreach (DataRow row in ds.Tables["DataInsert"].Rows)
                {
                    var userID = (int)row["u001"];
                    var parentUserID = (int)row["u012"];
                    var gameClassID = (int)row["gc001"];
                    var myHavePoint = (decimal)row["up003"];
                    var parentHavePoint = (decimal)row["up002"];
                    if (0 != parentUserID)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "SELECT * FROM wgs017(NOLOCK) WHERE u001=@u001 AND gc001=@gc001;";
                        command.Parameters.AddWithValue("@u001", parentUserID);
                        command.Parameters.AddWithValue("@gc001", gameClassID);
                        da.Fill(ds, "FindParent");
                        var findParentUserID = (int)ds.Tables["FindParent"].Rows[0]["u012"];
                        var findUserID = (int)ds.Tables["FindParent"].Rows[0]["u001"];
                        var findParentHavePoint = (decimal)ds.Tables["FindParent"].Rows[0]["up002"];
                        var findParentPoint = (decimal)ds.Tables["FindParent"].Rows[0]["up003"];
                        if (Math.Round(myHavePoint,1) == Math.Round(findParentPoint,1))
                        {
                            var stringKey = Math.Round(myHavePoint, 1).ToString();
                            var limitNumber = string.Empty;
                            if (dicLimit.TryGetValue(stringKey, out limitNumber))
                            {
                                command.Parameters.Clear();
                                command.CommandText = "SELECT COUNT(1) FROM wgs017(NOLOCK) WHERE u012=@u012 AND gc001=@gc001 AND up003=@up003;";
                                command.Parameters.AddWithValue("@u012", parentUserID);
                                command.Parameters.AddWithValue("@gc001", gameClassID);
                                command.Parameters.AddWithValue("@up003", myHavePoint);
                                var samePointCount = (int)command.ExecuteScalar();
                                if (Convert.ToInt32(limitNumber) < samePointCount)
                                {
                                    throw new Exception(string.Format("DBE[已经存在相同返点，返点{0}系统只允许{1}个账号相同返点]", stringKey, limitNumber));
                                }
                            }
                            else 
                            {
                                throw new Exception(string.Format("DBE[{0}返点限制不存在]", stringKey));
                            }
                        }
                        if (Math.Round(myHavePoint, 1) > Math.Round(findParentPoint, 1))
                        {
                            throw new Exception(string.Format("DBE[{0}大于上级{1}]", Math.Round(myHavePoint, 1), Math.Round(findParentPoint, 1)));
                        }
                    }
                }
            }
            else if (TriggerAction.Update == triggerContext.TriggerAction)
            {
                command.CommandText = "SELECT * FROM INSERTED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataInsert");
                command.CommandText = "SELECT * FROM DELETED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataDelete");
                DataColumn[] pKey = new DataColumn[1];
                pKey[0] = ds.Tables["DataDelete"].Columns["up001"];
                ds.Tables["DataDelete"].PrimaryKey = pKey;
                foreach (DataRow row in ds.Tables["DataInsert"].Rows)
                { 
                    DataRow oldrow = ds.Tables["DataDelete"].Rows.Find(row["up001"]);
                    var u001 = (int)row["u001"];
                    var g001 = (int)row["g001"];
                    var gc001 = (int)row["gc001"];
                    var gp001 = (int)row["gp001"];
                    var up002 = (decimal)row["up002"];
                    var up003 = (decimal)row["up003"];
                    var oldup002 = (decimal)oldrow["up002"];
                    var oldup003 = (decimal)oldrow["up003"];
                    command.Parameters.Clear();
                    command.CommandText = "SELECT u012 FROM wgs012 WHERE u001=@u001";
                    command.Parameters.AddWithValue("@u001", u001);
                    var parentUID = (int)command.ExecuteScalar();
                    decimal maxPoint = 0;
                    if (0 == parentUID)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "SELECT gp008 FROM wgs007(NOLOCK) WHERE gp001=@gp001;";
                        command.Parameters.AddWithValue("@gp001", gp001);
                        maxPoint = (decimal)command.ExecuteScalar();
                    }
                    else
                    {
                        command.Parameters.Clear();
                        command.CommandText = "SELECT up003 FROM wgs017(NOLOCK) WHERE u001=@u001 AND gc001=@gc001;";
                        command.Parameters.AddWithValue("@u001", parentUID);
                        command.Parameters.AddWithValue("@gc001", gc001);
                        maxPoint = (decimal)command.ExecuteScalar();
                    }
                    string errorMessage = string.Empty;
                    command.Parameters.Clear();
                    command.CommandText = "SELECT gc003 FROM wgs006(NOLOCK) WHERE gc001=@gc001";
                    command.Parameters.AddWithValue("@gc001", gc001);
                    var gcName = (string)command.ExecuteScalar();
                    if (up003 < oldup003)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "SELECT COUNT(1) FROM wgs017(NOLOCK) WHERE u012=@u001 AND up003>=@up003;";
                        command.Parameters.AddWithValue("@u001", u001);
                        command.Parameters.AddWithValue("@up003", up003);
                        var sameCount = (int)command.ExecuteScalar();
                        if (0 < sameCount)
                        {
                            command.Parameters.Clear();
                            command.CommandText = "SELECT up003,up014,gc001 FROM wgs017(NOLOCK) WHERE u012=@u001 AND up003>=@up003;";
                            command.Parameters.AddWithValue("@u001", u001);
                            command.Parameters.AddWithValue("@up003", up003);
                            da.SelectCommand = command;
                            da.Fill(ds, "ErrorList");
                            string tempError = string.Empty;
                            foreach (DataRow errorRow in ds.Tables["ErrorList"].Rows)
                            {
                                command.Parameters.Clear();
                                command.CommandText = "SELECT gc003 FROM wgs006(NOLOCK) WHERE gc001=@gc001";
                                command.Parameters.AddWithValue("@gc001", errorRow["gc001"]);
                                var tempGCName = (string)command.ExecuteScalar();
                                tempError += string.Format("{0}:{2}:{1};", errorRow["up014"], errorRow["up003"], tempGCName);
                            }
                            errorMessage = string.Format("DBE[{0}不能设置{1}返点，存在等于或大于{1}返点的下级。{2}]", gcName, up003, tempError);
                            throw new Exception(errorMessage);
                        }
                    }
                    else if (up003 > maxPoint)
                    {
                        errorMessage = string.Format("DBE[{0}返点不能大于上级{1}]", gcName, maxPoint);
                        throw new Exception(errorMessage);
                    }
                }
            }
        }
    }
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_UserFinanceChange", Target = "wgs014", Event = "AFTER  INSERT, DELETE, UPDATE")]
    public static void UserFinanceChange()
    {
        /*
         * VIP触发
         */
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            DataSet ds = new DataSet();
            SqlCommand command = connection.CreateCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            if (TriggerAction.Update == triggerContext.TriggerAction)
            {
                command.CommandText = "SELECT * FROM INSERTED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataNew");
                command.CommandText = "SELECT * FROM DELETED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataOld");
                DataColumn[] pKey = new DataColumn[1];
                pKey[0] = ds.Tables["DataOld"].Columns["u001"];
                ds.Tables["DataOld"].PrimaryKey = pKey;
                command.CommandText = "SELECT cfg003 FROM wgs027 WHERE cfg001=@cfg001;";
                command.Parameters.AddWithValue("@cfg001", "SYS_VIP_LEVEL");
                string vipLevel =(string)command.ExecuteScalar();
                foreach (DataRow row in ds.Tables["DataNew"].Rows)
                {
                    DataRow oldRow = ds.Tables["DataOld"].Rows.Find(row["u001"]);
                    var uf004 = (decimal)row["uf004"];
                    var olduf004 = (decimal)oldRow["uf004"];
                    var u001 = (int)row["u001"];
                    if (uf004 != olduf004)
                    {
                        var level = ExtDCFunction.GetUserLevel(vipLevel, uf004);
                        command.Parameters.Clear();
                        command.CommandText = "UPDATE wgs012 SET u015=@level WHERE u001=@u001 AND u015<>@level;";
                        command.Parameters.AddWithValue("@u001", u001);
                        command.Parameters.AddWithValue("@level", level);
                        command.ExecuteNonQuery();
                    }
                }
            }
            ds.Dispose();
            da.Dispose();
        }
    }

    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_GameSession", Target = "wgs005", Event = "AFTER  INSERT, DELETE, UPDATE")]    
    public static void GameSession()
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            DataSet ds = new DataSet();
            SqlCommand command = connection.CreateCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            if (TriggerAction.Insert == triggerContext.TriggerAction)
            {
                command.CommandText = "SELECT * FROM INSERTED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataInsert");
                foreach (DataRow row in ds.Tables["DataInsert"].Rows)
                {
                    var g001 = (int)row["g001"];
                    var gc001 = (int)row["gc001"];
                    var gs001 = (int)row["gs001"];
                    var gs002 = (string)row["gs002"];
                    var gs003 =(DateTime)row["gs003"];
                    var gs004 = (DateTime)row["gs004"];
                    var gs005 = (DateTime)row["gs005"];
                    var gs006 = (DateTime)row["gs006"];
                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO wgs038(g001,gc001,gs001,gs002,gs003,gs004,gs005,gs006) VALUES(@g001,@gc001,@gs001,@gs002,@gs003,@gs004,@gs005,@gs006);";
                    command.Parameters.AddWithValue("@g001", g001);
                    command.Parameters.AddWithValue("@gc001", gc001);
                    command.Parameters.AddWithValue("@gs001", gs001);
                    command.Parameters.AddWithValue("@gs002", gs002.Trim());
                    command.Parameters.AddWithValue("@gs003", gs003);
                    command.Parameters.AddWithValue("@gs004", gs004);
                    command.Parameters.AddWithValue("@gs005", gs005);
                    command.Parameters.AddWithValue("@gs006", gs006);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_GameSessionOther", Target = "wgs038", Event = "AFTER  INSERT, DELETE, UPDATE")]    
    public static void GameSessionOther()
    { 
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            DataSet ds = new DataSet();
            SqlCommand command = connection.CreateCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            if (TriggerAction.Delete == triggerContext.TriggerAction)
            {
                command.CommandText = "SELECT * FROM DELETED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataDelete");
                foreach (DataRow row in ds.Tables["DataDelete"].Rows)
                {
                    var gs001 = (int)row["gs001"];
                    command.Parameters.Clear();
                    command.CommandText = "DELETE FROM wgs005 WHERE gs001=@gs001;";
                    command.Parameters.AddWithValue("@gs001", gs001);
                    var item = command.ExecuteNonQuery();
                }
            }
            else if (TriggerAction.Update == triggerContext.TriggerAction)
            {
                command.CommandText = "SELECT * FROM DELETED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataDelete");
                command.CommandText = "SELECT * FROM INSERTED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataInsert");
                DataColumn[] pKey = new DataColumn[1];
                pKey[0] = ds.Tables["DataDelete"].Columns["gs001"];
                ds.Tables["DataDelete"].PrimaryKey = pKey;
                foreach (DataRow row in ds.Tables["DataInsert"].Rows)
                {
                    var gs001 = (int)0;
                    gs001 = (int)row["gs001"];
                    var gs002 = row["gs002"].ToString();
                    var gs003 = (DateTime)row["gs003"];
                    var gs004 = (DateTime)row["gs004"];
                    var gs005 = (DateTime)row["gs005"];
                    var gsr004 = (byte)row["gsr004"];
                    var gs007 = row.IsNull("gs007") ? string.Empty : row["gs007"].ToString();
                    if (1 == gsr004)
                    {
                        command.Parameters.Clear();
                        command.CommandText = "UPDATE wgs005 SET gs010=1 WHERE gs001=@gs001;";
                        command.Parameters.AddWithValue("@gs001", gs001);
                        command.ExecuteNonQuery();
                        continue;
                    }
                    if (false == string.IsNullOrEmpty(gs007))
                    {
                        command.CommandText = "UPDATE wgs005 SET gs002=@gs002,gs003=@gs003,gs004=@gs004,gs005=@gs005,gs007=@gs007,gs010=2 WHERE gs001=@gs001;";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@gs002", gs002);
                        command.Parameters.AddWithValue("@gs003", gs003);
                        command.Parameters.AddWithValue("@gs004", gs004);
                        command.Parameters.AddWithValue("@gs005", gs005);
                        command.Parameters.AddWithValue("@gs007", gs007);
                        command.Parameters.AddWithValue("@gs001", gs001);
                        command.ExecuteNonQuery();
                        command.CommandText = "UPDATE wgs022 SET so020=@gs007 WHERE gs001=@gs001 AND gs002=@gs002;";
                        command.ExecuteNonQuery();
                        command.CommandText = "UPDATE wgs045 SET so020=@gs007 WHERE gs001=@gs001 AND gs002=@gs002;";
                        command.ExecuteNonQuery();
                        command.CommandText = "UPDATE wgs031 SET gs007=@gs007 WHERE gs001=@gs001 AND gs002=@gs002;";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = "UPDATE wgs005 SET gs002=@gs002,gs003=@gs003,gs004=@gs004,gs005=@gs005 WHERE gs001=@gs001;";
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@gs002", gs002);
                        command.Parameters.AddWithValue("@gs003", gs003);
                        command.Parameters.AddWithValue("@gs004", gs004);
                        command.Parameters.AddWithValue("@gs005", gs005);
                        command.Parameters.AddWithValue("@gs007", gs007);
                        command.Parameters.AddWithValue("@gs001", gs001);
                        command.ExecuteNonQuery();
                        command.CommandText = "UPDATE wgs031 SET gs002=@gs002,gs003=@gs003,gs004=@gs004,gs007=@gs007 WHERE gs001=@gs001;";
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
    private static void PPMPay(ref SqlCommand command, string _content, decimal _orderPoint,long _so001, int _g001, int _gc001, int _gm001, int _gm002, decimal orderAmt)
    {
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        if (string.IsNullOrEmpty(_content))
        {
            return;
        }
        string debugString = string.Empty;
        var dataSplit = _content.Split(',');
        for (int i = dataSplit.Length - 2; i > -1; i--)
        {
            var pDataSplit = dataSplit[i].Split(':');
            var pDataSPlit2 = dataSplit[i + 1].Split(':');
            int uid = int.Parse(pDataSplit[0]);
            decimal sj = decimal.Parse(pDataSplit[2]);
            decimal xj = decimal.Parse(pDataSPlit2[2]);
            decimal sumPoint = 0.0000m;
            decimal saveMoney = 0.0000m;
            if (dataSplit.Length - 2 == i)
            {
                decimal allowMoney = _orderPoint == 0 ? 0 : (xj - _orderPoint);
                sumPoint = sj - xj + allowMoney;
            }
            else
            {
                sumPoint = sj - xj;
            }
            sumPoint = sj - xj;
            if (0 < sumPoint)
            {
                command.Parameters.Clear();
                command.CommandText = "SELECT u001,u002,u003 FROM wgs012 WHERE u001=@u001;";
                command.Parameters.AddWithValue("@u001", uid);
                da.SelectCommand = command;
                da.Fill(ds, "User"+uid);
                if (0 == ds.Tables["User"+uid].Rows.Count)
                {
                    continue;
                }
                var u002 = ds.Tables["User"+uid].Rows[0]["u002"].ToString().Trim();
                var u003 = ds.Tables["User"+uid].Rows[0].IsNull("u003") ? string.Empty : ds.Tables["User"+uid].Rows[0]["u003"].ToString().Trim();
                saveMoney = orderAmt * (sumPoint / 100);
                WriteDataChangeLog(ref command, uid, u002, u003, _so001, 0, 0, _g001, _gc001, _gm001, _gm002, saveMoney, 0, 0, 0, 0, 0, 8, ExtDCFunction.GetRDList()[8].Name, null, null, 0, DateTime.Now);
                command.Parameters.Clear();
                command.CommandText = "UPDATE wgs014 SET uf001=uf001+@uf001 WHERE u001=@u001;";
                command.Parameters.AddWithValue("@uf001", saveMoney);
                command.Parameters.AddWithValue("@u001", uid);
                command.ExecuteNonQuery();
                SetDayReport(ref command, uid, u002, u003, 0, 0, 0, saveMoney, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }
        }
        ds.Dispose();
        da.Dispose();
    }
    public static DataSet GetUserInfo(ref SqlCommand command, int _u001, int _type)
    {
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        if (0 == _type)
        {
            command.CommandText = "SELECT u001, u002, u003, u004, u005, u006, u007, u008, u009, u010, u011, u012, u013, u014, u015, u016, u017, u018, u019, u020, u021, u022, u023 FROM wgs012 WHERE u001=@u001;";
            command.Parameters.AddWithValue("@u001", _u001);
            da.SelectCommand = command;
            da.Fill(ds, "DataUser");
        }
        else if( 1 == _type)
        {
            command.CommandText = "SELECT u001, uf001, uf002, uf003, uf004, uf005, uf006, uf007, uf008, uf009, uf010, uf011 FROM wgs014 WHERE u001=@u001;";
            command.Parameters.AddWithValue("@u001", _u001);
            da.SelectCommand = command;
            da.Fill(ds, "DataUserF");
        }
        return ds;
    }
    public static void WriteDataChangeLog(ref SqlCommand command, int _u001, string _u002, string _u003, long _so001, long _uw001, long _uc001, int _g001, int _gc001, int _gm001, int _gm002, decimal _changeMoney, int _changeMoneyT, decimal _changeHold, int _changeHoldT, decimal _changePoint, int _changePointT, byte _typeID, string _typeName, string _gs002, string _comment, long _tf001, DateTime _changeDateTime)
    {
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        decimal uxf002 = 0;
        decimal uxf003 = 0;
        decimal uxf004 = 0;
        decimal uxf005 = 0;
        decimal uxf007 = 0;
        decimal uxf008 = 0;
        decimal uxf009 = 0;
        decimal uxf010 = 0;
        DateTime uxf014 = _changeDateTime;
        command.Parameters.Clear();
        command.CommandText = "SELECT u001, uf001, uf002, uf003, uf004, uf005, uf006, uf007, uf008, uf009, uf010, uf011 FROM wgs014 WHERE u001=@u001;";
        command.Parameters.AddWithValue("@u001", _u001);
        da.SelectCommand = command;
        da.Fill(ds, "DataUF");
        DataRow dataUFRow = ds.Tables["DataUF"].Rows[0];
        decimal _oir_uf001 = (decimal)dataUFRow["uf001"];
        decimal _oir_uf002 = (decimal)dataUFRow["uf002"];
        decimal _oir_uf003 = (decimal)dataUFRow["uf003"];
        decimal _oir_uf004 = (decimal)dataUFRow["uf004"];
        decimal _oir_uf005 = (decimal)dataUFRow["uf005"];
        decimal _oir_uf006 = (decimal)dataUFRow["uf006"];
        decimal _oir_uf007 = (decimal)dataUFRow["uf007"];
        uxf002 = _oir_uf001;
        uxf003 = _changeMoney;
        if (0 != _changeHold)
        {
            uxf003 = _changeHold;
        }
        uxf007 = uxf002 + ( _changeMoneyT == 0 ? _changeMoney  : 0 - _changeMoney);
        uxf004 = _oir_uf004;
        uxf005 = _changePoint;
        uxf008 = _oir_uf004 + (_changePointT == 0 ? _changePoint : 0 - _changePoint);
        uxf009 = _oir_uf003;
        uxf010 = uxf009 + ( _changeHoldT == 0 ? _changeHold : 0 - _changeHold);
        command.Parameters.Clear();
        command.CommandText = "INSERT INTO wgs021(u001,u002,u003,uc001,uxf002,uxf003,uxf007,uxf004,uxf005,uxf008,uxf014,uxf015,uxf009,uxf010,uw001,so001,g001,gc001,gm001,gm002,uxf016,uxf017,gs002,tf001) VALUES(@u001,@u002,@u003,@uc001,@uxf002,@uxf003,@uxf007,@uxf004,@uxf005,@uxf008,@uxf014,@uxf015,@uxf009,@uxf010,@uw001,@so001,@g001,@gc001,@gm001,@gm002,@uxf016,@uxf017,@gs002,@tf001)";
        command.Parameters.AddWithValue("@u001", _u001);
        command.Parameters.AddWithValue("@u002", _u002);
        command.Parameters.AddWithValue("@u003", _u003);
        command.Parameters.AddWithValue("@uc001", _uc001);
        command.Parameters.AddWithValue("@uxf002", uxf002);
        command.Parameters.AddWithValue("@uxf003", uxf003);
        command.Parameters.AddWithValue("@uxf007", uxf007);
        command.Parameters.AddWithValue("@uxf004", uxf004);
        command.Parameters.AddWithValue("@uxf005", uxf005);
        command.Parameters.AddWithValue("@uxf008", uxf008);
        command.Parameters.AddWithValue("@uxf014", uxf014);
        command.Parameters.AddWithValue("@uxf015", _typeName);
        command.Parameters.AddWithValue("@uxf009", uxf009);
        command.Parameters.AddWithValue("@uxf010", uxf010);
        command.Parameters.AddWithValue("@uw001", _uw001);
        command.Parameters.AddWithValue("@so001", _so001);
        command.Parameters.AddWithValue("@g001", _g001);
        command.Parameters.AddWithValue("@gc001", _gc001);
        command.Parameters.AddWithValue("@gm001", _gm001);
        command.Parameters.AddWithValue("@gm002", _gm002);
        command.Parameters.AddWithValue("@uxf016", _typeID);
        command.Parameters.AddWithValue("@tf001", _tf001);
        if (string.IsNullOrEmpty(_gs002))
        {
            command.Parameters.AddWithValue("@gs002", null);
        }
        else
        {
            command.Parameters.AddWithValue("@gs002", _gs002);
        }
        if (string.IsNullOrEmpty(_comment))
        {
            command.Parameters.AddWithValue("@uxf017", null);
        }
        else
        {
            command.Parameters.AddWithValue("@uxf017", _comment);
        }
        command.ExecuteNonQuery();
        ds.Dispose();
        da.Dispose();
    }
    public static void ChangeTraceOrderStatus(ref SqlCommand command, long sto001)
    {
        command.Parameters.Clear();
        command.CommandText = "SELECT COUNT(1) FROM wgs022(NOLOCK) WHERE sto001=@sto001 AND so009=0 AND so016=0;";
        command.Parameters.AddWithValue("@sto001", sto001);
        var traceCount = (int)command.ExecuteScalar();
        if (0 == traceCount)
        {
            command.CommandText = "UPDATE wgs030 SET sto005=1 WHERE sto001=@sto001;";
            command.ExecuteNonQuery();
        }
    }
    public static void SetDayReport(ref SqlCommand command,int u001, string u002, string u003, decimal orderAmount, decimal chargeAmount, decimal bonusAmount, decimal pointAmount, decimal creditsAmount, decimal creditsSubtractAmount, decimal withdrawAmount, decimal sysPayAmount, decimal sysSubtractAmount, decimal costPointAmount, decimal chargeFee, decimal withdrawFee, decimal stockAmount, decimal transferMoney)
    {
        SqlDataAdapter da = new SqlDataAdapter();
        DataSet ds = new DataSet();
        List<string> changeSQL = new List<string>();
        string dr002String = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
        DateTime dr002 = DateTime.Parse(dr002String);
        DateTime dr003 = DateTime.Now;
        command.Parameters.Clear();
        command.CommandText = "SELECT u001,u002,u003 FROM wgs012(NOLOCK) WHERE u001=@u001;";
        command.Parameters.AddWithValue("@u001", u001);
        da.SelectCommand = command;
        da.Fill(ds, "User");
        if (0 == ds.Tables["User"].Rows.Count)
        {
            throw new Exception("DBE[账号信息不存在]");
        }
        DataRow userRow = ds.Tables["User"].Rows[0];
        u002 = userRow["u002"].ToString().Trim();
        u003 = userRow.IsNull("u003") ? string.Empty : userRow["u003"].ToString().Trim();
        command.CommandText = "SELECT COUNT(1) FROM wgs042(NOLOCK) WHERE u001=@u001 AND dr002=@dr002;";
        command.Parameters.AddWithValue("@dr002", dr002);
        var drExists = (int)command.ExecuteScalar();
        if (0 == drExists)
        {
            command.CommandText = "INSERT INTO wgs042(u001,u002,u003,dr002,dr003) VALUES(@u001,@U002,@U003,@dr002,@dr003);";
            command.Parameters.AddWithValue("@u002", u002);
            command.Parameters.AddWithValue("@u003", u003);
            command.Parameters.AddWithValue("@dr003", dr003);
            command.ExecuteNonQuery();
        }
        if (0 < orderAmount || 0 > orderAmount)
        {
            changeSQL.Add("dr004=dr004+@dr004");
            command.Parameters.AddWithValue("@dr004", orderAmount);
        }
        if (0 < chargeAmount || 0 > chargeAmount)
        {
            changeSQL.Add("dr005=dr005+@dr005");
            command.Parameters.AddWithValue("@dr005", chargeAmount);
        }
        if (0 < bonusAmount || 0 > bonusAmount)
        {
            changeSQL.Add("dr006=dr006+@dr006");
            command.Parameters.AddWithValue("@dr006", bonusAmount);
        }
        if (0 < pointAmount || 0 > pointAmount)
        {
            changeSQL.Add("dr007=dr007+@dr007");
            command.Parameters.AddWithValue("@dr007", pointAmount);
        }
        if (0 < creditsAmount || 0 > creditsAmount)
        {
            changeSQL.Add("dr008=dr008+@dr008");
            command.Parameters.AddWithValue("@dr008", creditsAmount);
        }
        if (0 < creditsSubtractAmount || 0 > creditsSubtractAmount)
        {
            changeSQL.Add("dr009=dr009+@dr009");
            command.Parameters.AddWithValue("@dr009", creditsSubtractAmount);
        }
        if (0 < withdrawAmount || 0 > withdrawAmount)
        {
            changeSQL.Add("dr010=dr010+@dr010");
            command.Parameters.AddWithValue("@dr010", withdrawAmount);
        }
        if (0 < sysPayAmount || 0 > sysPayAmount)
        {
            changeSQL.Add("dr011=dr011+@dr011");
            command.Parameters.AddWithValue("@dr011", sysPayAmount);
        }
        if (0 < sysSubtractAmount || 0 > sysSubtractAmount)
        {
            changeSQL.Add("dr012=dr012+@dr012");
            command.Parameters.AddWithValue("@dr012", sysSubtractAmount);
        }
        if (0 < costPointAmount || 0 > costPointAmount)
        {
            changeSQL.Add("dr013=dr013+@dr013");
            command.Parameters.AddWithValue("@dr013", costPointAmount);
        }
        if (0 < chargeFee || 0 > chargeFee)
        {
            changeSQL.Add("dr014=dr014+@dr014");
            command.Parameters.AddWithValue("@dr014", chargeFee);
        }
        if (0 < withdrawFee || 0 > withdrawFee)
        {
            changeSQL.Add("dr015=dr015+@dr015");
            command.Parameters.AddWithValue("@dr015", withdrawFee);
        }
        if (0 < stockAmount || 0 > stockAmount)
        {
            changeSQL.Add("dr016=dr016+@dr016");
            command.Parameters.AddWithValue("@dr016", stockAmount);
        }
        if (0 < transferMoney)
        {
            changeSQL.Add("dr017=dr017+@TransferMoney");
            command.Parameters.AddWithValue("@TransferMoney", transferMoney);
        }
        if (0 > transferMoney)
        {
            changeSQL.Add("dr018=dr018+@TransferMoney");
            command.Parameters.AddWithValue("@TransferMoney", Math.Abs(transferMoney));
        }
        if (0 == changeSQL.Count)
        {
            return;
        }
        command.CommandText = "UPDATE wgs042 SET dr003=@updatedr003,"+changeSQL[0];
        for (int i = 1; i < changeSQL.Count; i++)
        {
            command.CommandText += ","+changeSQL[i];
        }
        command.CommandText += " WHERE u001=@u001 AND dr002=@dr002;";
        command.Parameters.AddWithValue("@updatedr003", DateTime.Now);
        command.ExecuteNonQuery();
        ds.Dispose();
        da.Dispose();
    }
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_TransferMoney", Target = "wgs043", Event = "AFTER  INSERT, DELETE, UPDATE")]   
    public static void TransferMoney()
    { 
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            if (TriggerAction.Insert == triggerContext.TriggerAction)
            {
                DataSet ds = new DataSet();
                SqlCommand command = connection.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                command.CommandText = "SELECT * FROM INSERTED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataNew");
                foreach (DataRow row in ds.Tables["DataNew"].Rows)
                {
                    long tf001 = (long)row["tf001"];
                    int myUserID = (int)row["tf002"];
                    string myUserName = row["tf003"].ToString().Trim();
                    string myUserNickName = row.IsNull("tf004") ? null : row["tf004"].ToString().Trim();
                    int toUserID = (int)row["tf005"];
                    string toUserName = row["tf006"].ToString().Trim();
                    string toUserNickName = row.IsNull("tf007") ? null : row["tf007"].ToString().Trim();
                    decimal amount = (decimal)row["tf008"];
                    DateTime datetime = (DateTime)row["tf009"];
                    WriteDataChangeLog(ref command, myUserID, myUserName, myUserNickName, 0, 0, 0, 0, 0, 0, 0, amount, 1, amount, 0, 0, 0, 35, ExtDCFunction.GetRDList()[35].Name, null, null, tf001, datetime);
                    command.Parameters.Clear();
                    command.CommandText = "UPDATE wgs014 SET uf001=uf001-@uf001,uf003=uf003+@uf001 WHERE u001=@myUserID AND uf001-@uf001 >=0;";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@uf001", amount);
                    command.Parameters.AddWithValue("@MyUserID", myUserID);
                    var isTransfer = command.ExecuteNonQuery();
                    if (0 == isTransfer)
                    {
                        throw new Exception("DBE[余额不足]");
                    }
                }
            }
            else if (TriggerAction.Update == triggerContext.TriggerAction)
            {
                DataSet ds = new DataSet();
                SqlCommand command = connection.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                command.CommandText = "SELECT * FROM INSERTED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataNew");
                command.CommandText = "SELECT * FROM DELETED;";
                da.SelectCommand = command;
                da.Fill(ds, "DataOld");
                DataColumn[] pKey = new DataColumn[1];
                pKey[0] = ds.Tables["DataOld"].Columns["tf001"];
                ds.Tables["DataOld"].PrimaryKey = pKey;
                foreach (DataRow row in ds.Tables["DataNew"].Rows)
                {
                    DataRow oldrow = ds.Tables["DataOld"].Rows.Find(row["tf001"]);
                    long tf001 = (long)row["tf001"];
                    int myUserID = (int)row["tf002"];
                    string myUserName = row["tf003"].ToString().Trim();
                    string myUserNickName = row.IsNull("tf004") ? null : row["tf004"].ToString().Trim();
                    int toUserID = (int)row["tf005"];
                    string toUserName = row["tf006"].ToString().Trim();
                    string toUserNickName = row.IsNull("tf007") ? null : row["tf007"].ToString().Trim();
                    decimal amount = (decimal)row["tf008"];
                    DateTime datetime = (DateTime)row["tf009"];
                    DateTime okDT = (DateTime)row["tf014"];
                    byte status = (byte)row["tf012"];
                    byte oldstatus = (byte)oldrow["tf012"];
                    string comment = row.IsNull("tf013") ? null : row["tf013"].ToString();
                    if (status == oldstatus)
                    {
                        throw new Exception("DBE[状态没有变化]");
                    }
                    if (0 < oldstatus)
                    {
                        throw new Exception("DBE[状态不能修改]");
                    }
                    if (2 == status)
                    {
                        string cancel = string.Format("您给{0}转账，金额{1}，已经被系统取消。" + (comment == null ? "" : "原因是：" + comment), toUserName, amount.ToString("N2"));
                        ExtDCFunction.SendMessage(ref command, myUserID, myUserName, myUserNickName, "转账失败", cancel, okDT);
                        WriteDataChangeLog(ref command, myUserID, myUserName, myUserNickName, 0, 0, 0, 0, 0, 0, 0, amount, 0, amount, 1, 0, 0, 36, ExtDCFunction.GetRDList()[36].Name, null, null, tf001, datetime);
                        command.Parameters.Clear();
                        command.CommandText = "UPDATE wgs014 SET uf001=uf001+@uf001,uf003=uf003-@uf001 WHERE u001=@MyUserID;";
                        command.Parameters.AddWithValue("@uf001", amount);
                        command.Parameters.AddWithValue("@MyUserID", myUserID);
                        command.ExecuteNonQuery();
                        continue;
                    }
                    SetDayReport(ref command, toUserID, toUserName, toUserNickName, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, amount);
                    WriteDataChangeLog(ref command, toUserID, toUserName, toUserNickName, 0, 0, 0, 0, 0, 0, 0, amount, 0, 0, 0, 0, 0, 32, ExtDCFunction.GetRDList()[32].Name, null, null, tf001, datetime);
                    command.Parameters.Clear();
                    command.CommandText = "UPDATE wgs014 SET uf001=uf001+@uf001 WHERE u001=@ToUserID;";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@uf001", amount);
                    command.Parameters.AddWithValue("@ToUserID", toUserID);
                    command.ExecuteNonQuery();
                    SetDayReport(ref command, myUserID, myUserName, myUserNickName, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 - amount);
                    WriteDataChangeLog(ref command, myUserID, myUserName, myUserNickName, 0, 0, 0, 0, 0, 0, 0, 0, 0, amount, 1, 0, 0, 31, ExtDCFunction.GetRDList()[31].Name, null, null, tf001, datetime);
                    command.Parameters.Clear();
                    command.CommandText = "UPDATE wgs014 SET uf003=uf003+@uf001 WHERE u001=@MyUserID AND uf003+@uf001 >= 0;";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@uf001", 0 - amount);
                    command.Parameters.AddWithValue("@MyUserID", myUserID);
                    var isTransfer = command.ExecuteNonQuery();
                    if (0 == isTransfer)
                    {
                        throw new Exception("DBE[冻结金额不足]");
                    }
                    string sendContent = string.Format("您给{0}转账，金额{1}，{0}已经收到", toUserName, amount.ToString("N2"));
                    string getContent = string.Format("{0}您给转账，金额{1}，已经收到", myUserName, amount.ToString("N2"));
                    ExtDCFunction.SendMessage(ref command, myUserID, myUserName, myUserNickName, "转账成功", sendContent, okDT);
                    ExtDCFunction.SendMessage(ref command, toUserID, toUserName, toUserNickName, "收到转账", getContent, okDT);
                }
            }
        }
    }

    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_UserMain", Target = "wgs012", Event = "AFTER  INSERT, DELETE, UPDATE")]
    public static void UserMain()
    { 
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            DataSet ds = new DataSet();
            SqlCommand command = connection.CreateCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            if (TriggerAction.Insert == triggerContext.TriggerAction)
            {
                command.CommandText = "INSERT INTO wgs049 SELECT * FROM INSERTED";
                command.ExecuteNonQuery();
            }
            else if (TriggerAction.Update == triggerContext.TriggerAction)
            {
                command.CommandText = "DELETE FROM wgs049 WHERE u001 IN(SELECT u001 FROM INSERTED);";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO wgs049 SELECT * FROM INSERTED";
                command.ExecuteNonQuery();
            }
            else if (TriggerAction.Delete == triggerContext.TriggerAction)
            {
            }
        }
    }
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_UserFMain", Target = "wgs014", Event = "AFTER  INSERT, DELETE, UPDATE")]
    public static void UserFMain()
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            DataSet ds = new DataSet();
            SqlCommand command = connection.CreateCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            if (TriggerAction.Insert == triggerContext.TriggerAction)
            {
                command.CommandText = "INSERT INTO wgs052 SELECT * FROM INSERTED";
                command.ExecuteNonQuery();
            }
            else if (TriggerAction.Update == triggerContext.TriggerAction)
            {
                command.CommandText = "DELETE FROM wgs052 WHERE u001 IN(SELECT u001 FROM INSERTED);";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO wgs052 SELECT * FROM INSERTED";
                command.ExecuteNonQuery();
            }
            else if (TriggerAction.Delete == triggerContext.TriggerAction)
            {
            }
        }
    }
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_SignGetPoint", Target = "wgs046", Event = "AFTER  INSERT, DELETE, UPDATE")]
    public static void SignGetPoint()
    { 
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            DataSet ds = new DataSet();
            SqlCommand command = connection.CreateCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            command.CommandText = "SELECT * FROM INSERTED;";
            da.SelectCommand = command;
            da.Fill(ds, "DataNew");
            var signPoint = string.Empty; 
            command.CommandText = "SELECT cfg003 FROM wgs027 WHERE cfg001=@cfg001;";
            command.Parameters.AddWithValue("@cfg001", "SYS_SIGN_GET_POINT");
            signPoint = (string)command.ExecuteScalar();

            command.CommandText = "SELECT cfg003 FROM wgs027 WHERE cfg001=@cfg001;";
            command.Parameters.AddWithValue("@cfg001", "SYS_UI_AddPoint");
            string addPoint = (string)command.ExecuteScalar();

            if (TriggerAction.Insert == triggerContext.TriggerAction)
            {
                foreach (DataRow row in ds.Tables["DataNew"].Rows)
                {
                    var u001 = (int)row["u001"];
                    var u002 = row["u002"].ToString().Trim();
                    var u003 = row.IsNull("u003") ? row["u003"].ToString().Trim() : string.Empty;
                    var sign002 = (DateTime)row["sign002"];
                    //日报表
                    SetDayReport(ref command, u001, u002, u003, 0, 0, 0, 0, decimal.Parse(signPoint), 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    //余额变动与账变
                    WriteDataChangeLog(ref command, u001, u002, u003, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, decimal.Parse(signPoint), 0, 33, ExtDCFunction.GetRDList()[33].Name, null, null, 0, sign002);

                    command.Parameters.Clear();
                    command.CommandText = "UPDATE wgs014 SET uf004=uf004+@uf004 WHERE u001=@u001;";
                    command.Parameters.AddWithValue("@uf004", decimal.Parse(signPoint));
                    command.Parameters.AddWithValue("@u001", u001);
                    command.ExecuteNonQuery();
                    ExtDCFunction.SendMessage(ref command, u001, u002, u003, "签到积分", string.Format("您在{0}签到获得{1}积分", sign002.ToString(), signPoint), sign002);


                    command.Parameters.Clear();
                    command.CommandText = "select top 7  left(max(sign002),10) from wgs046 where u001 = " + u001.ToString() + " and sign007 = 1 group by left(sign002,10)  order by max(sign002) desc";
                    da.SelectCommand = command;
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    //=================连续七天奖励==========================
                    if (dt.Rows.Count == 7)
                    {

                        if (((Convert.ToDateTime(dt.Rows[0][0]) - Convert.ToDateTime(dt.Rows[6][0])).Days + 1) == 7)
                        {
                            try
                            {
                                command.Parameters.Clear();
                                command.CommandText = "UPDATE wgs014 SET uf004=uf004+@uf004 WHERE u001=@u001;";
                                command.Parameters.AddWithValue("@uf004", decimal.Parse(addPoint));
                                command.Parameters.AddWithValue("@u001", u001);
                                command.ExecuteNonQuery();

                                command.CommandText = "update wgs046 set sign007 = 0";
                                command.ExecuteNonQuery();
                            }
                            catch
                            {
                                ds.Dispose();
                                da.Dispose();
                                return;
                            }
                            //日报表
                            SetDayReport(ref command, u001, u002, u003, 0, 0, 0, 0, decimal.Parse(addPoint), 0, 0, 0, 0, 0, 0, 0, 0, 0);
                            //余额变动与账变
                            WriteDataChangeLog(ref command, u001, u002, u003, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, decimal.Parse(addPoint), 0, 33, ExtDCFunction.GetRDList()[33].Name, null, null, 0, sign002);

                            ExtDCFunction.SendMessage(ref command, u001, u002, u003, "连续七天签到积分", string.Format("您在{0}获得连续七天签到奖励{1}积分", sign002.ToString(), addPoint), sign002);
                        }
                    }



                }



            }



            ds.Dispose();
            da.Dispose();
        }
    }
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "CLR_UST_URForSearch", Target = "wgs013", Event = "AFTER  INSERT, DELETE, UPDATE")]
    public static void URForSearch()
    { 
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        SqlPipe pipe = SqlContext.Pipe;
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            if (TriggerAction.Insert == triggerContext.TriggerAction)
            {
                command.CommandText = "INSERT INTO wgs048 SELECT * FROM INSERTED";
                command.ExecuteNonQuery();
            }
            else if (TriggerAction.Update == triggerContext.TriggerAction)
            {
                command.CommandText = "DELETE FROM wgs048 WHERE ur001 IN(SELECT ur001 FROM INSERTED);";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO wgs048 SELECT * FROM INSERTED";
                command.ExecuteNonQuery();
            }
        }
    }
}