using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DBase;
using _NWC = NETCommon;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace GameServices
{
    public class User_AM
    {

        public int CheckAGAccount(string account)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    //DBModel.wgs012 wg = new DBModel.wgs012();
                    var userinfo = db.Repositorywgs012.IQueryable(exp => exp.u002 == account.Trim());

                    var count = userinfo.Count();
                    if (1 == count)
                    {
                        return userinfo.First().u001;
                        
                    }
                }
            } return -1;
        }


        public Boolean CheckAGAccountFH(string account)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    //var count = db.Repositorywgs012.IQueryable(exp => exp.u002 == account.Trim()).Count();
                    var count = db.Repositorywgs055.IQueryable(exp => exp.u002 == account.Trim()).Count();
                    if (1 == count)
                    {
                        return true;
                    }
                }
            } return false;
        }

        public MR AddFH(DBModel.wgs055 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    if (_NWC.GeneralValidate.IsNullOrEmpty(entity.u002))
                    {
                        return mr;
                    }
                    
                    db.Repositorywgs055.Add(entity);
                    db.SaveChanges();
                    mr.Code = 1;
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }

        DateTime GetUlisTime (string uid)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    string sql = "";
                    sql += " select top 1  s007 from [wgs056] where u001 = " + uid + " order by s007 desc ";
                    var mypost = db.SqlQuery<DateTime>(sql, uid.ToString());
                    if (mypost.ToList().Count == 0) return Convert.ToDateTime("1900-01-01 00:00:00");
                    else return mypost.First();
                }
            } 
        }


        string GetLogSQL()
        {

            List<DBModel.wgs055> list = new List<DBModel.wgs055>();
            list = FHList();
            string sql = "";
            DateTime lasttime, ctime;
            int day = 0;
            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                lasttime = new DateTime();
                lasttime = GetUlisTime(list[i].u001.ToString());

                if (lasttime == Convert.ToDateTime("1900-01-01 00:00:00"))
                {
                    lasttime = list[i].f004;
                }
                day = (DateTime.Now - lasttime).Days;
                count = (day / list[i].f003);
                ctime = lasttime;
                DateTime dtleiji = DateTime.Parse("1900-01-01 00:00:00");
                try
                {
                    DBModel.wgs027 onAccount = GetKV("SYS_CountProfit_Time");
                    if (onAccount != null)
                        dtleiji = DateTime.Parse(onAccount.cfg003);
                }
                catch { }
                for (int x = 0; x < count; x++)
                {
                    List<DBModel.wgs042> drdList = GetUList("", list[i].u001.ToString(), ctime, ctime.AddDays(list[i].f003));

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
                    decimal coldr004 = 0.0000m;
                    decimal coldr005 = 0.0000m;
                    decimal coldr006 = 0.0000m;
                    decimal coldr007 = 0.0000m;
                    decimal coldr008 = 0.0000m;
                    decimal coldr009 = 0.0000m;
                    decimal coldr010 = 0.0000m;
                    decimal coldr011 = 0.0000m;
                    decimal coldr012 = 0.0000m;
                    decimal coldr013 = 0.0000m;
                    decimal coldr014 = 0.0000m;
                    decimal coldr015 = 0.0000m;
                    decimal coldr016 = 0.0000m;
                    decimal coldr017 = 0.0000m;
                    decimal coldr018 = 0.0000m;
                    decimal collinesum = 0.0000m;
                    if (null != total)
                    {
                        total = total.OrderByDescending(exp => exp.dr004).ToList();
                        foreach (var item in total)
                        {
                            decimal lineSum = item.dr006 + item.dr007 - item.dr004;
                            collinesum += lineSum;
                            coldr004 += item.dr004;
                            coldr005 += item.dr005;
                            coldr006 += item.dr006;
                            coldr007 += item.dr007;
                            coldr008 += item.dr008;
                            coldr009 += item.dr009;
                            coldr010 += item.dr010;
                            coldr011 += item.dr011;
                            coldr012 += item.dr012;
                            coldr013 += item.dr013;
                            coldr014 += item.dr014;
                            coldr015 += item.dr015;
                            coldr016 += item.dr016;
                            coldr017 += item.dr017;
                            coldr018 += item.dr018;
                        }
                       
                        decimal leiJikuishun = getLeijiKuishun(list[i].u001, dtleiji, ctime);
                        decimal kefenhong = 0.0000m;
                        if (leiJikuishun < 0)
                            kefenhong = collinesum;
                        else
                            kefenhong = leiJikuishun + collinesum;
                        sql = " insert wgs056 (s012, s013, s014, s002,s015,s016, s004, s005, u001, u002, f002, s006, s007, s008, s009, s010, s011 ) ";
                        sql += "values(" + coldr006 + "," + coldr007 + "," + coldr004 + "," + kefenhong + "," + collinesum + "," + leiJikuishun + ",";
                        sql += list[i].f003.ToString() + ", ";
                        sql += " 0, ";
                        sql += list[i].u001.ToString() + ", ";
                        sql += "'" + list[i].u002.ToString() + "', ";
                        sql += list[i].f002.ToString() + ", ";
                        sql += "'" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "', ";
                        sql += "'" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "', ";
                        sql += "'', ";
                        sql += "'1900-10-01 00:00:00', ";
                        sql += "-1, ";
                        sql += "'" + list[i].f006.ToString("yyyy-MM-dd HH:mm:ss") + "' ";
                        sql += ")";
                        ctime = ctime.AddDays(list[i].f003);
                        EXEC(sql);
                    }
                }
            }
            return sql;
        }

       
        protected DBModel.wgs027 GetKV(string key)
        {
            ISystem serSystem = new GameServices.System();
            var result = serSystem.GetKeyValue(key);
            return result;
        }
        decimal getLeijiKuishun(int id,DateTime dts,DateTime dte) {
            decimal kuishun = 0.0000m;
            List<DBModel.wgs042> drdList = GetUList("", id.ToString(), dts,dte);

                    var total = from dl in drdList
                                group dl by new { dl.u002, dl.u001 } into ndl
                                select new
                                {
                                
                                    dr004 = ndl.Sum(exp => exp.dr004),
                                    
                                    dr006 = ndl.Sum(exp => exp.dr006),
                                    dr007 = ndl.Sum(exp => exp.dr007)                                   
                                };
                    decimal coldr004 = 0.0000m;
         
                    decimal coldr006 = 0.0000m;
                    decimal coldr007 = 0.0000m;
                  
                    decimal collinesum = 0.0000m;
                    if (null != total)
                    {
                        total = total.OrderByDescending(exp => exp.dr004).ToList();
                        foreach (var item in total)
                        {
                            decimal lineSum = item.dr006 + item.dr007 - item.dr004;
                            collinesum += lineSum;
                          
                        }
                        return collinesum;
                    }
            
            return kuishun;
        }
//        string GetLogSQL()
//        {
            
//            List<DBModel.wgs055> list = new List<DBModel.wgs055>();
//            list = FHList();

//            string uid = "";
//            string sql = "";
//            DateTime lasttime, ctime;
//            int day = 0;
//            int count = 0;


//            sql = " ";

//            for (int i = 0; i < list.Count; i++)
//            {
//                lasttime = new DateTime();
//                lasttime = GetUlisTime(list[i].u001.ToString());

//                if (lasttime == Convert.ToDateTime("1900-01-01 00:00:00"))
//                {
//                    lasttime = list[i].f004;
//                }
//                uid = GetUList("", list[i].u001.ToString());

//                day = (DateTime.Now - lasttime).Days;
//                count = (day / list[i].f003);
//                ctime = lasttime;

//                sql = "";
//                #region
//                /*orz
//                for (int x = 0; x < count; x++)
//                {
//                    sql = " insert wgs056 (s012, s013, s014, s002, s004, s005, u001, u002, f002, s006, s007, s008, s009, s010, s011 )  ";
//                    sql += " select   ";

//                    //中奖
//                    sql += " isnull(sum(so010),0), ";
                    
//                    //赠送
//                    sql += " (select isnull(sum(uc003),0) from wgs019 where uc009 like '系统%' and ";
//                    sql += " u001  in (" + uid + ") and ";
//                    sql += " uc007 >= '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "' and uc007 <= '" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "'), ";

//                    //投注
//                    sql += " isnull(sum(so004),0), ";

//                    //盈亏
//                    sql += " isnull( ";
//                    sql += " (sum(so010) ";
                    
//                    //屏蔽赠送 -
//                    //sql += "  + (select isnull(sum(uc003),0) from wgs019 where uc009 like '系统%' and ";
//                    //sql += " u001  in (" + uid + ") and ";
//                    //sql += " uc007 >= '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "' and uc007 <= '" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "') ";

//                    sql += " - sum(so004) ),0), ";

//                    sql += list[i].f003.ToString() + ", ";
//                    sql += " 0, ";
//                    sql += list[i].u001.ToString() + ", ";
//                    sql += "'" + list[i].u002.ToString() + "', ";
//                    sql += list[i].f002.ToString() + ", ";
//                    sql += "'" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "', ";
//                    sql += "'" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "', ";
//                    sql += "'', ";
//                    sql += "'1900-10-01 00:00:00', ";
//                    sql += "-1, ";
//                    sql += "'" + list[i].f006.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

//                    //sql += " ((sum(so011) +  ";
//                    //sql += " (select isnull(sum(uxf003),0) from  wgs021 where u001 in (" + uid + ") and uxf015 like '系统%'  and uxf016 <> 12 and ";
//                    //sql += " uxf014 >= '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "' and uxf014 <= '" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "') ";
//                    //sql += " - sum(so004) ) * "+ list[i].f002.ToString() + ")， ";

//                    sql += " from wgs045 where u001  in (" + uid + ") ";
//                    //sql += " and so008 > '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "' and so008 <= '" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
//                    //sql += " and so021 = 0 and so009 = 1; ";

//                    sql += " and so007 > '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "' and so007 <= '" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
//                    sql += " and so021 = 0 and so009 < 2; ";

//                    ctime = ctime.AddDays(list[i].f003);
//                    EXEC(sql);
//                }
//                */
//                #endregion
//                #region
                
//                for (int x = 0; x < count; x++)
//                {
//                    sql = " insert wgs056 (s012, s013, s014, s002,s015,s016, s004, s005, u001, u002, f002, s006, s007, s008, s009, s010, s011 )  ";
//                    sql += " select   ";

//                    //中奖
//                    sql += " isnull(sum(dr006),0), ";

//                    //赠送
//                   // sql += " isnull(sum(dr011),0), ";
//                    //返点
//                    sql += " isnull(sum(dr007),0), ";  

//                    //投注
//                    sql += " isnull(sum(dr004),0), ";

//                    //本期实际可以用于分红的盈亏
//                    sql += " case when (select  isnull(sum(dr006),0)+ isnull(sum(dr007),0)-isnull(sum(dr004),0) from wgs042 where  u001  in (" + uid + ") and dr002 <= '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "')<0 then isnull(sum(dr006),0)+ isnull(sum(dr007),0)-isnull(sum(dr004),0) else (select  isnull(sum(dr006),0)+ isnull(sum(dr007),0)-isnull(sum(dr004),0) from wgs042 where  u001  in (" + uid + ") and dr002 <= '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "')+ isnull(sum(dr006),0)+ isnull(sum(dr007),0)-isnull(sum(dr004),0) end , ";

//                    //本期盈亏
//                    sql += "  isnull(sum(dr006),0)+ isnull(sum(dr007),0)-isnull(sum(dr004),0), ";

//                    //累计盈亏
//                    sql += "(select  isnull(sum(dr006),0)+ isnull(sum(dr007),0)-isnull(sum(dr004),0) from wgs042 where  u001  in (" + uid + ") and dr002 <= '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "') ,";

//                    sql += list[i].f003.ToString() + ", ";
//                    sql += " 0, ";
//                    sql += list[i].u001.ToString() + ", ";
//                    sql += "'" + list[i].u002.ToString() + "', ";
//                    sql += list[i].f002.ToString() + ", ";
//                    sql += "'" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "', ";
//                    sql += "'" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "', ";
//                    sql += "'', ";
//                    sql += "'1900-10-01 00:00:00', ";
//                    sql += "-1, ";
//                    sql += "'" + list[i].f006.ToString("yyyy-MM-dd HH:mm:ss") + "' ";

//                    //sql += " ((sum(so011) +  ";
//                    //sql += " (select isnull(sum(uxf003),0) from  wgs021 where u001 in (" + uid + ") and uxf015 like '系统%'  and uxf016 <> 12 and ";
//                    //sql += " uxf014 >= '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "' and uxf014 <= '" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "') ";
//                    //sql += " - sum(so004) ) * "+ list[i].f002.ToString() + ")， ";

//                    sql += " from wgs042 where u001  in (" + uid + ") ";
//                    //sql += " and so008 > '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "' and so008 <= '" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
//                    //sql += " and so021 = 0 and so009 = 1; ";

//                    sql += " and dr002 > '" + ctime.ToString("yyyy-MM-dd HH:mm:ss") + "' and dr002 <= '" + ctime.AddDays(list[i].f003).ToString("yyyy-MM-dd HH:mm:ss") + "' ";
            

//                    ctime = ctime.AddDays(list[i].f003);
//                    EXEC(sql);
                 
//#endregion
//                }
//            }

//            return sql;
//        }


        public List<DBModel.wgs056> GetMyFHList(int pageIndex, int pageSize, out int recordCount, DBModel.WGS056Where ws)
        {

            recordCount = 0;



            MR mr = new MR();
            List<DBModel.wgs056> xlist = new List<DBModel.wgs056>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        string sql = "";
                        sql += " select * from [dbo].[wgs056] ";
                        sql += " where 1 = 1 ";
                        sql += " and s007 >= '" + ws.dts.ToString("yyyy-MM-dd 00:00:00") + "' and s007 <= '" + ws.dte.ToString("yyyy-MM-dd 23:59:59") + "' ";
                        if (ws.type >= 0)
                        {
                            sql += " and s005 = " + ws.type.ToString();
                        }
                        if (ws.acct != "")
                        {
                            sql += " and u002 like '%"+ ws.acct.Replace("'","") +"%' ";
                        }

                        if (ws.om > 0)
                        {
                            if (ws.om == 1) sql += " and s003 < " + ws.omm.ToString();
                            else if (ws.om == 2) sql += " and s003 = " + ws.omm.ToString();
                            else if (ws.om == 3) sql += " and s003 > " + ws.omm.ToString();
                        }

                        sql += " order by u002  ";

                        var tempDRs = db.SqlQuery<DBModel.wgs056>(sql).ToList();
                        recordCount = tempDRs.Count;
                        return tempDRs;

                    }
                    catch (Exception error)
                    {
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = mr.Exception.Message;
                    }
                }
            }


            return xlist;

        }

        public List<DBModel.wgs056> GetMyFHLog(int pageIndex, int pageSize, out int recordCount)
        {

            recordCount = 0;

            

            MR mr = new MR();
            List<DBModel.wgs056> xlist = new List<DBModel.wgs056>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        var couwgnt = db.Repositorywgs056.IQueryable(exp => exp.s005 > 0, order => order.OrderByDescending(exp => exp.s007)).ToList().Skip(pageSize * pageIndex).Take(pageSize).ToList(); ;
                        recordCount = couwgnt.Count;
                        foreach (var item in couwgnt)
                        {
                            xlist.Add(item);
                        }
                    }
                    catch (Exception error)
                    {
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = mr.Exception.Message;
                    }
                }
            }


            return xlist;

        }

        public MR EXEC(string sql)
        {
            
            MR mr = new MR();

            if (sql == "") return mr;

            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        db.ExecuteSqlCommand(sql);
                        ts.Complete();
                        mr.Message = "";
                    }
                    catch (Exception error)
                    {
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = mr.Exception.Message;
                    }
                }
            }

            return mr;

        }

        public List<DBModel.wgs056> GetFHLog(int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;

            GetLogSQL();

            MR mr = new MR();
            List<DBModel.wgs056> xlist = new List<DBModel.wgs056>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        var couwgnt = db.Repositorywgs056.IQueryable(exp => exp.s005 == 0, order => order.OrderByDescending(exp => exp.s007)).ToList().Skip(pageSize * pageIndex).Take(pageSize).ToList(); ;
                        recordCount = couwgnt.Count;
                        foreach (var item in couwgnt)
                        {
                            xlist.Add(item);
                        }
                    }
                    catch (Exception error)
                    {
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = mr.Exception.Message;
                    }
                }
            }
            

            return xlist;

        }

        public List<DBModel.wgs056> GetFHLogForUser(int pageIndex, int pageSize, out int recordCount, int uid)
        {
            recordCount = 0;

            GetLogSQL();

            MR mr = new MR();
            List<DBModel.wgs056> xlist = new List<DBModel.wgs056>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        var couwgnt = db.Repositorywgs056.IQueryable(exp => exp.u001 == uid, order => order.OrderByDescending(exp => exp.s007)).ToList().Skip(pageSize * pageIndex).Take(pageSize).ToList(); ;
                        recordCount = couwgnt.Count;
                        foreach (var item in couwgnt)
                        {
                            xlist.Add(item);
                        }
                    }
                    catch (Exception error)
                    {
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = mr.Exception.Message;
                    }
                }
            }


            return xlist;

        }


        List<DBModel.wgs055> FHList()
        {

            List<DBModel.wgs055> list = new List<DBModel.wgs055>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var couwgnt = db.Repositorywgs055.IQueryable();

                    foreach (var item in couwgnt)
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }


        List<DBModel.wgs042> GetUList(string suid, string uid,DateTime dts,DateTime dte)
        {
            List<DBModel.UList> list = new List<DBModel.UList>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    //string sql = "";

                    //sql += " select u001 as uid from [wgs012] where u012 in (" + uid + ")";
                    //var mypost = db.SqlQuery<DBModel.UList>(sql).ToList();

                    //if (mypost.Count <= 0)
                    //{
                    //    //if (suid == "") return uid;
                    //    //else return (suid);
                    //    return (suid);
                    //}

                    //string myuid = "";

                    //foreach (DBModel.UList temp in mypost)
                    //{
                    //    myuid += temp.uid.ToString() + ',';
                    //}

                    //myuid = myuid.Remove(myuid.Length - 1, 1);
                    ////if (suid == "") return GetUList(uid + "," + myuid, myuid);
                    //if (suid == "") return GetUList(myuid, myuid);
                    //else return GetUList(suid + "," + myuid, myuid);


                    List<int> childUserIDs = new List<int>();
                    int myId = int.Parse(uid);
                    childUserIDs = db.Repositorywgs049.IQueryable(exp => exp.u012 == myId).Select(exp => exp.u001).ToList();

                    List<DBModel.wgs042> drlist = new List<DBModel.wgs042>();

                    string sql1 = string.Format("SELECT u001 AS u001,u002 AS u002, '-' AS u003, CAST(0 AS bigint) AS dr001, CAST('2014-01-01' AS Date) AS dr002, CAST('2014-01-01 00:00:00' AS datetime) AS dr003,SUM(dr004) AS dr004,SUM(dr005) AS dr005,SUM(dr006) AS dr006,SUM(dr007) AS dr007,SUM(dr008) AS dr008, SUM(dr009) AS dr009,SUM(dr010) AS dr010,SUM(dr011) AS dr011,SUM(dr012) AS dr012,SUM(dr013) AS dr013,SUM(dr014) AS dr014,SUM(dr015) AS dr015,SUM(dr016) AS dr016, SUM(dr017) AS dr017,SUM(dr018) AS dr018,SUM(dr019) AS dr019,SUM(dr020) AS dr020 FROM wgs042(NOLOCK) WHERE u001 IN({0}) AND dr002 >='{1}' AND dr002 <= '{2}' GROUP BY u001,u002", myId, dts, dte);
                    var tempDRs1 = db.SqlQuery<DBModel.wgs042>(sql1).ToList();
                    if (tempDRs1.Count() > 0)
                    {
                        for (int i = 0; i < tempDRs1.Count; i++)
                        {
                            tempDRs1[i].u001 = myId;
                            tempDRs1[i].u002 = db.Repositorywgs049.GetByPrimaryKey(myId).u002.Trim();
                        }
                        drlist = drlist.Concat(tempDRs1).ToList();
                    }

                    foreach (var childID in childUserIDs)
                    {

                        var targetIDs = db.Repositorywgs048.IQueryable(exp => exp.u001 == childID || exp.u002 == childID).Select(exp => exp.u002).ToList();
                      
                        string sql = string.Format("SELECT u001 AS u001,u002 AS u002, '-' AS u003, CAST(0 AS bigint) AS dr001, CAST('2014-01-01' AS Date) AS dr002, CAST('2014-01-01 00:00:00' AS datetime) AS dr003,SUM(dr004) AS dr004,SUM(dr005) AS dr005,SUM(dr006) AS dr006,SUM(dr007) AS dr007,SUM(dr008) AS dr008, SUM(dr009) AS dr009,SUM(dr010) AS dr010,SUM(dr011) AS dr011,SUM(dr012) AS dr012,SUM(dr013) AS dr013,SUM(dr014) AS dr014,SUM(dr015) AS dr015,SUM(dr016) AS dr016, SUM(dr017) AS dr017,SUM(dr018) AS dr018,SUM(dr019) AS dr019,SUM(dr020) AS dr020 FROM wgs042(NOLOCK) WHERE u001 IN({0}) AND dr002 >='{1}' AND dr002 <= '{2}' GROUP BY u001,u002", string.Join(",", targetIDs), dts, dte);
                        var tempDRs = db.SqlQuery<DBModel.wgs042>(sql).ToList();
                        if (tempDRs.Count() > 0)
                        {
                            for (int i = 0; i < tempDRs.Count; i++)
                            {
                                tempDRs[i].u001 = childID;
                                tempDRs[i].u002 = db.Repositorywgs049.GetByPrimaryKey(childID).u002.Trim();
                            }
                            drlist = drlist.Concat(tempDRs).ToList();
                        }
                    }
                    return drlist;

                }
            }

        }

        public string GetSum(int uid)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    string sql = "";
                    sql += "select  isnull(abs(sum(s003)),0) from [dbo].[wgs056] where u001 = "+ uid.ToString() +" and  s005 = 1 ";
                    //var mypost = db.SqlQuery<DateTime>(sql).First().ToString();
                    //return mypost.First();

                    return db.SqlQuery<double>(sql).First().ToString();
                }
            } 

        }

        public string SetMoney(int uid, double fbl, int fzq, int fid)
        {
            string sql = " update wgs055 set  f002 = " + (fbl * 0.01).ToString() + ", f003 = " + fzq.ToString() + ", f009 = " + uid.ToString() + ", f008 = getdate() where f001 = " + fid.ToString() + " ";
            EXEC(sql);
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="uid"></param>
        /// <param name="about"></param>
        /// <param name="ispass">1:通过,2:取消</param>
        /// <returns></returns>
        public string PassFH(string fid, int uid, string about, int ispass)
        {
            var ids = fid.Split(',');
            List<long> orderIDs = new List<long>();
            string tin = "";
            foreach (var id in ids)
            {
                try
                {
                    tin += long.Parse(id).ToString() + ",";
                    orderIDs.Add(long.Parse(id));
                }
                catch
                {
                }
            }

            about = Regex.Replace(about, "'", "");
            tin = tin.Remove(tin.Length - 1, 1);
            string sql = " update wgs056 set s005 = " + ispass.ToString() + ", s008 = '" + about + "', s010 = " + uid.ToString() + ", s011 = getdate() where s001 in ( " + tin.ToString() + " ) and s005 = 0;  ";

            if (ispass == 1)
            {
                sql += " declare @count int; ";
                for (int i = 0; i < orderIDs.Count; i++)
                {

                    sql += " update wgs014 set wgs014.uf013 = (wgs014.uf013 + abs(wgs056.s003)) ";
                    sql += " from wgs056 where wgs056.u001 = wgs014.u001 ";
                    sql += " and wgs056.s003 < 0 ";
                    sql += " and wgs056.s001 = " + orderIDs[i].ToString() + "; ";

                 
                    sql += " set @count = (select isnull(max(dr001),0) from wgs042 where convert(varchar(10),dr002,120) = convert(varchar(10),getdate(),120) and u001 = (select wgs056.u001 from  wgs056 where wgs056.s001 =  " + orderIDs[i].ToString() + ")); ";

                    sql += " if(@count = 0) ";
                    sql += " begin ";
                    sql += " INSERT INTO wgs042(dr002,dr003,dr016,u001,u002, u003) ";
                    sql += " select ";
                    sql += " '" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "',  ";
                    sql += " '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', ";
                    sql += " abs(wgs056.s003), wgs012.u001, wgs012.u002, wgs012.u003 ";
                    sql += " from wgs012,  wgs056  where 1 = 1 ";
                    sql += " and wgs056.u001 = wgs012.u001 ";
                    sql += " and wgs056.s003 < 0 ";
                    sql += " and wgs056.s001 = " + orderIDs[i].ToString() + " ";
                    sql += " end ";
                    sql += " else ";
                    sql += " begin ";
                    sql += " update wgs042 set wgs042.dr016 = (wgs042.dr016 + abs(wgs056.s003)), wgs042.dr003 = getdate() from wgs056 where  ";
                    sql += " wgs042.u001 = wgs056.u001  ";
                    sql += " and  wgs042.dr001 = @count;   ";
                    sql += " end ";

                    sql += " INSERT INTO wgs021(u001,u002,u003,uxf002,uxf003,uxf004,uxf007,uxf008,uxf014,uxf015,uxf016) ";
                    sql += " select wgs012.u001, wgs012.u002, wgs012.u003, ";
                    sql += " wgs014.uf013, abs(wgs056.s003), ";
                    sql += " wgs014.uf004, (wgs014.uf013 + abs(wgs056.s003)), ";
                    sql += " wgs014.uf004, getdate(),'系统分红',7 ";
                    sql += " from wgs012, wgs014, wgs056  where 1 = 1 ";
                    sql += " and wgs012.u001 = wgs014.u001 ";
                    sql += " and wgs056.u001 = wgs012.u001 ";
                    sql += " and wgs056.s003 < 0 ";
                    sql += " and wgs056.s001 = " + orderIDs[i].ToString() + "; ";

                    sql += " INSERT INTO wgs044([msg002],[msg003],[msg004],[msg005],[msg006],[msg008],[msg009],[msg010],[msg011]) ";
                    sql += " select '系统分红', '系统分红，总金额：'+ Convert(varchar,abs(wgs056.s003)) +'已经到账。',0, ";
                    sql += " wgs012.u001,getdate(), '-',wgs012.u002,'-',wgs012.u003 ";
                    sql += " from wgs012, wgs014, wgs056  where 1 = 1 ";
                    sql += " and wgs012.u001 = wgs014.u001 ";
                    sql += " and wgs056.u001 = wgs012.u001 ";
                    sql += " and wgs056.s003 < 0 ";
                    sql += " and wgs056.s001 = " + orderIDs[i].ToString() + "; ";

                   

                }
            
            }


            MR mr = new MR();
            
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        db.ExecuteSqlCommand(sql);
                        ts.Complete();
                        mr.Message = "";
                    }
                    catch (Exception error)
                    {
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = mr.Exception.Message;
                    }
                }
            }


            return mr.Message;

        }


        #region "ORZ 获取分红设置"

        public List<DBModel.wgs055> GetFHList(int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;
            List<DBModel.wgs055> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs055.GetAll().ToList().Skip(pageSize * pageIndex).Take(pageSize).ToList();
                    recordCount = list.Count;
                }
            }
            return list;
        }

        public string GetFHSet(int uid)
        {
            string msg = "";

            MR mr = new MR();
            List<DBModel.wgs055> xlist = new List<DBModel.wgs055>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        var couwgnt = db.Repositorywgs055.IQueryable(exp => exp.u001 == uid);
                        if (couwgnt.ToList().Count == 1)
                        {
                            msg = (couwgnt.ToList()[0].f002 * 100).ToString() + "%";
                        }
                        else
                        {
                            msg = "不参与分红";
                        }
                        
                    }
                    catch (Exception error)
                    {
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = mr.Exception.Message;
                    }
                }
            }


            return msg;
        }

        #endregion





    }
}
