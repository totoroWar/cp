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
    
    public class User_ORZ 
    {

        public string IsPost(int uid, double uf001, double uf004)
        {
            
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    string sql = "";

                    sql += " select count(1) AS ispost from [dbo].[wgs014] where ";
                    sql += " u001 = {0}";
                    sql += " and uf001 = {1}";
                    sql += " and round(uf004,2) = {2}";
                    var mypost = db.SqlQuery<int>(sql, uid.ToString(), uf001.ToString(), uf004.ToString());
                    return mypost.First().ToString();
                    //return mypost.ToString();
                }
            }
            
        }

        public string IsPost(int uid, double uf001)
        {

            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    string sql = "";

                    sql += " select count(1) AS ispost from [dbo].[wgs014] where ";
                    sql += " u001 = {0}";
                    sql += " and uf001 = {1}"; 
                    var mypost = db.SqlQuery<int>(sql, uid.ToString(), uf001.ToString());
                    return mypost.First().ToString(); 
                }
            }

        }

        public string IsOrderChange(int uid)
        {
            //换一个方式
            //string sql = "";  refresh_current_page();

            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    string sql = "";

                    sql += " select count(1) AS tcount from [dbo].[wgs045] where ";
                    sql += " u001 = {0}";
                    sql += " and so016 = 0 ";
                    var mypost = db.SqlQuery<int>(sql, uid.ToString());
                    return mypost.First().ToString();
                    //return mypost.ToString();
                }
            }

            return "";
        }

        public string Get053List()
        {
            List<DBModel.WinnersList> list = new List<DBModel.WinnersList>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    string sql = "";

                    //sql += " select top {0} zj.tpi004, lx.gc002, zj.gs002, zj.tpi007, zj.tpi005  from wgs053 as zj, wgs006 as lx ";
                    //sql += " where zj.g001 = lx.gc001 and zj.tpi007 >= {1}";
                    //sql += " order by tpi009 desc ";
                    //var mypost = db.SqlQuery<DBModel.WinnersList>(sql, 20, 1000);

                    sql += " select top 20 zj.u002 as uname, lx.g002 as cname, zj.gs002 as xindex,  ";
                    sql += " zj.so010 as xmoney, zj.so012 as about  ";
                    sql += " from wgs045 as zj, wgs001 as lx  ";
                    sql += "  where zj.g001 = lx.g001 and zj.so010 >= 1000 ";
                    sql += " order by zj.so001 desc ";
                    var mypost = db.SqlQuery<DBModel.WinnersList>(sql).ToList();

                    string html = "";

                    html += "<ul>";
                    foreach (DBModel.WinnersList temp in mypost)
                    {
                        html += "<li>";
                        html += "<span style='color:#F00;'>" + temp.uname.ToString() + "</span>";
                        html += "在" + temp.cname.ToString() + "";
                        html += "<span style='color:#F00;'>" + temp.xindex.ToString() + "</span>期";
                        html += "中奖<span style='color:#F00;'>" + temp.xmoney.ToString() + "</span>，";
                        html += "内容：" + temp.about.ToString() + "";
                        html += "</li>";

                    }
                    html += "</ul>";
                    return html;
                    //return mypost.ToString();
                }
            }



        }

        public string Get053ListUI2()
        {
            List<DBModel.WinnersList> list = new List<DBModel.WinnersList>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    string sql = "";

                    //sql += " select top {0} zj.tpi004, lx.gc002, zj.gs002, zj.tpi007, zj.tpi005  from wgs053 as zj, wgs006 as lx ";
                    //sql += " where zj.g001 = lx.gc001 and zj.tpi007 >= {1}";
                    //sql += " order by tpi009 desc ";
                    //var mypost = db.SqlQuery<DBModel.WinnersList>(sql, 20, 1000);

                    sql += " select top 20 zj.u002 as uname, lx.g002 as cname, zj.gs002 as xindex,  ";
                    sql += " zj.so010 as xmoney, zj.so012 as about  ";
                    sql += " from wgs045 as zj, wgs001 as lx  ";
                    sql += "  where zj.g001 = lx.g001 and zj.so010 >= 1000 ";
                    sql += " order by zj.so001 desc ";
                    var mypost = db.SqlQuery<DBModel.WinnersList>(sql).ToList();

                    string html = "";

                    html += "<ul>";
                    foreach (DBModel.WinnersList temp in mypost)
                    {
                        html += "<p><font>最新中奖：</font>";
                        html += "<a href=\"javascript:void(0)\">";
                        html += "" + temp.uname.ToString() + "在" + temp.cname.ToString() + "";
                        html += "" + temp.xindex.ToString() + "期";
                        html += "中奖" + temp.xmoney.ToString() + "，";
                        html += "内容：" + temp.about.ToString() + "";
                        html += "</a></p>";

                    }
                    html += "</ul>";
                    return html;
                    //return mypost.ToString();
                }
            }



        }

        public MR GetMoneyPassAdd(DBModel.wgs056 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {                   
                    db.Repositorywgs056.Add(entity);
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
    }
}
