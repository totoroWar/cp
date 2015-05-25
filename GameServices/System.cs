using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBase;
using System.Transactions;
using System.Linq.Expressions;
using _NWC = NETCommon;
using System.IO;
using System.Text.RegularExpressions;
namespace GameServices
{
    public class System :ISystem
    {
        private string keyValueListCacheKey = "KeyValueListCacheKey";
        public void AddMenuLog(DBModel.wgs011 entity)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        db.Repositorywgs011.Add(entity);
                        db.SaveChanges();
                        mr.Code = 1;
                        ts.Complete();
                    }
                    catch (Exception error)
                    {
                        mr.Code = 0;
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = mr.Exception.Message;
                    }
                }
            }
        }
        public MR AddBPG(DBModel.wgs015 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                string newMenuIDs = string.Empty;
                var menuList = db.Repositorywgs004.IQueryable(exp=>exp.sm005 == 0).ToList();
                string[] menuIDs = entity.pg005.Split(',');
                foreach (string id in menuIDs)
                {
                    DBModel.wgs004 parent;
                    DBModel.wgs004 curMenu = menuList.Where(exp=>exp.sm001 == int.Parse(id)).Take(1).FirstOrDefault();
                    if (null!=curMenu && 0 == curMenu.sm002)
                    {
                        newMenuIDs += id + ",";
                        continue;
                    }
                    else if( null!=curMenu && 0 != curMenu.sm002)
                    {
                        newMenuIDs += id + ",";
                    }
                    int curMenuID = curMenu.sm002;
                    do
                    {
                        parent = null;
                        parent = menuList.Where(exp => exp.sm001 == curMenuID).Take(1).FirstOrDefault();
                        if (null != parent)
                        {
                            string[] chkIDs = newMenuIDs.Split(',');
                            if (0 == chkIDs.Where(exp => exp == parent.sm001.ToString()).Count())
                            {
                                newMenuIDs += parent.sm001 + ",";
                            }
                            curMenuID = parent.sm002;
                        }
                    } while (null != parent);
                }
                if (0 < newMenuIDs.Length)
                {
                    newMenuIDs = newMenuIDs.Substring(0, newMenuIDs.Length - 1);
                }
                entity.pg002 = 0;
                entity.pg005 = newMenuIDs;
                try
                {
                    if (entity.pg001 != 0)
                    {
                        db.Repositorywgs015.Update(entity);
                    }
                    else
                    {
                        db.Repositorywgs015.Add(entity);
                    }
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
        public List<DBModel.wgs015> GetBPGList()
        {
            List<DBModel.wgs015> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs015.GetAll().ToList();
                }
            }
            return list;
        }
        public MR UCBorwserCheck(string agent)
        {
            MR mr = new MR();
            DBModel.wgs027 config = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    config = db.Repositorywgs027.GetByPrimaryKey("SYS_CNS_BROWSER");
                }
                var dataSplit = config.cfg003.Split(new string[] { "_SAPP_" }, StringSplitOptions.None);
                foreach (var item in dataSplit)
                {
                    if (true == Regex.IsMatch(agent, item))
                    {
                        mr.Message = config.cfg004;
                        return mr;
                    }
                }
            }
            mr.Code = 1;
            return mr;
        }
        public DBModel.wgs015 GetBPG(int key)
        {
            DBModel.wgs015 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs015.GetOne(exp => exp.pg001 == key);
                }
            }
            return entity;
        }
        public MR UpdateBPG(DBModel.wgs015 entity)
        {
            return AddBPG(entity);
        }
        public List<DBModel.wgs011> GetLogList(int menuType,string account, int type, string ctrl, string act, string keyword, DateTime? dts, DateTime? dte, int pageSize, int pageIndex, out int recordCount)
        {
            recordCount = 0;
            List<DBModel.wgs011> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                Expression<Func<DBModel.wgs011, bool>> query = PredicateExtensionses.True<DBModel.wgs011>();
                if (false == string.IsNullOrEmpty(account))
                {
                    query = query.And(exp=>exp.u002 == account);
                }
                if (-1 < menuType)
                {
                    query = query.And(exp => exp.sm005 == menuType);
                }
                if (-1 < type)
                {
                    query = query.And(exp => exp.log011 == type);
                }
                if (false == string.IsNullOrEmpty(ctrl))
                {
                    query = query.And(exp=>exp.log002 == ctrl);
                }
                if (false == string.IsNullOrEmpty(act))
                {
                    query = query.And(exp => exp.log003 == act);
                }
                if (false == dts.HasValue && false == dte.HasValue)
                {
                    query = query.And(exp=>exp.log004 >= dts && exp.log004 <= dte);
                }
                if (false == string.IsNullOrEmpty(keyword))
                {
                    query = query.And(exp => exp.log007.Contains(keyword) || exp.log008.Contains(keyword) || exp.log009.Contains(keyword) || exp.log010.Contains(keyword) || exp.log012.Contains(keyword) || exp.log013.Contains(keyword) || exp.log014.Contains(keyword) || exp.log002.Contains(keyword) || exp.log003.Contains(keyword));
                }
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCount = db.Repositorywgs011.Count(query);
                    list = db.Repositorywgs011.IQueryable(query).OrderByDescending(exp => exp.log004).Skip(pageSize * pageIndex).Take(pageSize).ToList();
                }
            }
            return list;
        }
        public MR SendMessage(int sendUserID, List<string> toUserList, string title, string content, DateTime? dt, int setType)
        {
            MR mr = new MR();
            if (string.IsNullOrEmpty(title))
            {
                mr.Message = "标题不能为空";
                return mr;
            }
            if (string.IsNullOrEmpty(content))
            {
                mr.Message = "内容不能为空";
                return mr;
            }
            if (false == dt.HasValue)
            {
                mr.Message = "时间不能为空";
                return mr;
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                try
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        if (0 == sendUserID && 1 == setType)
                        {
                            if (null == toUserList || 0 == toUserList.Count)
                            {
                                mr.Message = "接收列表不能为空";
                                return mr;
                            }
                            foreach (var item in toUserList)
                            {
                                var userItem = db.Repositorywgs012.IQueryable(exp => exp.u002 == item).Take(1).FirstOrDefault();
                                if (null == userItem)
                                {
                                    continue;
                                }
                                DBModel.wgs044 entity = new DBModel.wgs044();
                                entity.msg002 = title;
                                entity.msg003 = content;
                                entity.msg004 = sendUserID;
                                entity.msg005 = userItem.u001;
                                entity.msg006 = dt.Value;
                                entity.msg008 = "_";
                                entity.msg009 = userItem.u002;
                                entity.msg010 = "_";
                                entity.msg011 = userItem.u003 == null ? null : userItem.u003.Trim();
                                db.Repositorywgs044.Add(entity);
                            }
                            db.SaveChanges();
                        }
                        else if (0 == sendUserID && 2 == setType)
                        {
                            foreach (var item in toUserList)
                            {
                                var ids = db.Repositorywgs048.IQueryable(exp => exp.u001n == item).Select(exp=>exp.u002).ToList();
                                var list = db.Repositorywgs049.IQueryable(exp => ids.Contains(exp.u001)).ToList();
                                foreach (var uitem in list)
                                {
                                    DBModel.wgs044 entity = new DBModel.wgs044();
                                    entity.msg002 = title;
                                    entity.msg003 = content;
                                    entity.msg004 = sendUserID;
                                    entity.msg006 = dt.Value;
                                    entity.msg005 = uitem.u001;
                                    entity.msg008 = "_";
                                    entity.msg009 = uitem.u002.Trim();
                                    entity.msg010 = "_";
                                    entity.msg011 = !string.IsNullOrEmpty(uitem.u003) ? uitem.u003.Trim() : string.Empty;
                                    db.Repositorywgs044.Add(entity);
                                }
                                db.SaveChanges();
                            }
                        }
                        else if (0 == sendUserID && 3 == setType)
                        {
                            int pageSize = 30;
                            for (int i = 0; i < 5000; i++)
                            {
                                var list = db.Repositorywgs049.IQueryable(null, order=>order.OrderBy(exp=>exp.u001)).Skip(i * pageSize).Take(pageSize).ToList();
                                if (null == list)
                                {
                                    break;
                                }
                                foreach (var item in list)
                                {
                                    DBModel.wgs044 entity = new DBModel.wgs044();
                                    entity.msg002 = title;
                                    entity.msg003 = content;
                                    entity.msg004 = sendUserID;
                                    entity.msg006 = dt.Value;
                                    entity.msg005 = item.u001;
                                    entity.msg008 = "_";
                                    entity.msg009 = item.u002.Trim();
                                    entity.msg010 = "_";
                                    entity.msg011 = !string.IsNullOrEmpty(item.u003) ? item.u003.Trim() : string.Empty;
                                    db.Repositorywgs044.Add(entity);
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                    ts.Complete();
                    mr.Code = 1;
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = MyException.Message(mr.Exception.Message);
                }
            }
            return mr;
        }
        public MR SignDay(int myUserID, string myUserName, string myUserNickName)
        {
            DateTime dtNow = DateTime.Now;
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var exists = db.Repositorywgs046.Count(exp => exp.sign004 == dtNow.Year && exp.sign005 == dtNow.Month && exp.sign006 == dtNow.Day && exp.u001 == myUserID);
                    if (0 < exists)
                    {
                        mr.IntData = 1;
                        mr.Message = "已经签到";
                        return mr;
                    }
                    else
                    {
                        DBModel.wgs046 entity = new DBModel.wgs046();
                        entity.sign004 = dtNow.Year;
                        entity.sign005 = dtNow.Month;
                        entity.sign006 = dtNow.Day;
                        entity.sign002 = dtNow;
                        entity.sign003 = 1;
                        entity.u001 = myUserID;
                        entity.u002 = myUserName;
                        entity.u003 = myUserNickName;
                        try
                        {
                            db.Repositorywgs046.Add(entity);
                            db.SaveChanges();
                            ts.Complete();
                            mr.Code = 1;
                        }
                        catch (Exception error)
                        {
                            mr.Exception = MyException.GetInnerException(error);
                            mr.Message = MyException.Message(mr.Exception.Message);
                        }
                    }
                }
            }
            return mr;
        }
        public Dictionary<string, DBModel.SysBaseLevel> GetSysBaseLevel(bool cache)
        {
            var keyName = "SysBaseLevel";
            Dictionary<string, DBModel.SysBaseLevel> dicList = (Dictionary<string, DBModel.SysBaseLevel>)_NWC.GeneralCache.Get(keyName);
            DBModel.wgs027 config = null;
            if (false == cache || null == dicList)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        config = db.Repositorywgs027.GetByPrimaryKey("SysBaseLevel");
                    }
                }
            }
            if (null != config)
            {
                dicList = new Dictionary<string, DBModel.SysBaseLevel>();
                var configSplit = config.cfg003.Split(',');
                foreach (var item in configSplit)
                {
                    DBModel.SysBaseLevel data = new DBModel.SysBaseLevel();
                    var itemSplit = item.Split(':');
                    data.Account = itemSplit[0];
                    data.NickName = itemSplit[1];
                    data.Image = itemSplit[2];
                    data.Attribute = int.Parse(itemSplit[3]);
                    dicList.Add(data.Account, data);
                }
                _NWC.GeneralCache.Set(keyName, dicList);
            }
            return dicList;
        }
        public List<DBModel.wgs046> GetSignMonth(int myUserID, int year, int month)
        {
            List<DBModel.wgs046> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs046.IQueryable(exp => exp.u001 == myUserID && exp.sign004 == year && exp.sign005 == month).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs044> GetMessageList(int setType, string sendUserName, int sendUserID, string toUserName, int toUserID, int isRead, int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;
            List<DBModel.wgs044> result = null;
            Expression<Func<DBModel.wgs044, bool>> query = PredicateExtensionses.True<DBModel.wgs044>();
            if (-1 < isRead)
            {
                query = query.And(exp => exp.msg007 == isRead);
            }
            if (false == string.IsNullOrEmpty(sendUserName))
            {
                query = query.And(exp=>exp.msg008 == sendUserName);
            }
            if (-1 < sendUserID)
            {
                query = query.And(exp => exp.msg004 == sendUserID);
            }
            if (false == string.IsNullOrEmpty(toUserName))
            {
                query = query.And(exp => exp.msg009 == toUserName);
            }
            if (-1 < toUserID)
            {
                query = query.And(exp => exp.msg005 == toUserID);
            }
            if (4 == setType)
            {
                query = query.Or(exp=>exp.msg005 == 0 || exp.msg005 == toUserID);
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCount = db.Repositorywgs044.Count(query);
                    result = db.Repositorywgs044.IQueryable(query, order => order.OrderBy(exp => exp.msg005).ThenByDescending(exp=>exp.msg006)).Skip(pageSize * pageIndex).Take(pageSize).ToList();
                }
            }
            return result;
        }
        public int GetUnReadMessage(int setType, int myUserID, int isRead)
        {
            int result = 0;
            Expression<Func<DBModel.wgs044, bool>> query = PredicateExtensionses.True<DBModel.wgs044>();
            if (0 < myUserID)
            {
                query = query.And(exp => exp.msg005 == myUserID);
            }
            if (-1 < isRead)
            {
                query = query.And(exp => exp.msg007 == isRead);
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs044.Count(query);
                }
            }
            return result;
        }
        public DBModel.wgs044 GetMessage(long key)
        {
            DBModel.wgs044 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    entity = db.Repositorywgs044.GetByPrimaryKey(key);
                    if (0 == entity.msg007 && 0 < entity.msg005)
                    {
                        entity.msg007 = 1;
                        db.Repositorywgs044.Update(entity);
                        db.SaveChanges();
                        ts.Complete();
                    }
                }
            }
            return entity;
        }
        public Dictionary<int, string> GetReqeustTypeI(bool cache)
        {
            Dictionary<int, string> result = (Dictionary<int, string>)_NWC.GeneralCache.Get("SysRequestTypeI");
            if (null == result || false == cache)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        var item = db.Repositorywgs027.GetByPrimaryKey("SYS_REQUEST_TYPE");
                        if (null != item)
                        {
                            result = new Dictionary<int, string>();
                            var reqs = item.cfg003.Split(',');
                            foreach (var req in reqs)
                            {
                                var reqSplit = req.Split(':');
                                result.Add(int.Parse(reqSplit[0]), reqSplit[1]);
                            }
                            _NWC.GeneralCache.Set("SysRequestTypeI", result);
                        }
                    }
                }
            }
            return result;
        }
        public Dictionary<string, int> GetReqeustTypeS(bool cache)
        {
            Dictionary<string, int> result = (Dictionary<string, int>)_NWC.GeneralCache.Get("SysRequestTypeS");
            if (null == result || false == cache)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        var item = db.Repositorywgs027.GetByPrimaryKey("SYS_REQUEST_TYPE");
                        if (null != item)
                        {
                            result = new Dictionary<string, int>();
                            var reqs = item.cfg003.Split(',');
                            foreach (var req in reqs)
                            {
                                var reqSplit = req.Split(':');
                                result.Add(reqSplit[1], int.Parse(reqSplit[0]));
                            }
                            _NWC.GeneralCache.Set("SysRequestTypeS", result);
                        }
                    }
                }
            }
            return result;
        }
        public List<DBModel.SysFirstLoadURL> GetUIFirstLoad(bool cache)
        {
            string keyName = "UIFirstLoad";
            List<DBModel.SysFirstLoadURL> result = (List<DBModel.SysFirstLoadURL>)_NWC.GeneralCache.Get(keyName);
            DBModel.wgs027 entity = new DBModel.wgs027();
            if (null == result || false == cache)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        entity = db.Repositorywgs027.GetByPrimaryKey("SYS_UI_POPUP");
                    }
                }
                if (null != entity)
                {
                    result = new List<DBModel.SysFirstLoadURL>();
                    var uiPopup = entity.cfg003.Split(',');
                    foreach (var item in uiPopup)
                    {
                        var itemSplit = item.Split(':');
                        DBModel.SysFirstLoadURL firstLoad = new DBModel.SysFirstLoadURL();
                        firstLoad.Text = itemSplit[0];
                        firstLoad.URL = itemSplit[1];
                        firstLoad.Order = int.Parse(itemSplit[2]);
                        firstLoad.IsShow = int.Parse(itemSplit[3]);
                        firstLoad.Title = itemSplit[4];
                        result.Add(firstLoad);
                    }
                    _NWC.GeneralCache.Set(keyName, result);
                }
            }
            return result;
        }
        public MR SetUserOnline(DBModel.wgs025 entity,int type)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var existsONL = db.Repositorywgs025.IQueryable(exp=>exp.u001 == entity.u001).Take(1).FirstOrDefault();
                    try
                    {
                        if (null == existsONL)
                        {
                            entity.onl006 = 1;
                            db.Repositorywgs025.Add(entity);
                        }
                        else
                        {
                            if (0 == type && 1 == existsONL.onl006)
                            {
                                existsONL.onl002 = entity.onl002;
                                existsONL.onl003 = DateTime.Now;
                                existsONL.onl005 = entity.onl005;
                                existsONL.onl006 = 1;
                                existsONL.onl007 = entity.onl007;
                                existsONL.onl008 = entity.onl008;
                                existsONL.onl009 = entity.onl009;
                                existsONL.onl010 = entity.onl010;
                                existsONL.onl004 = DateTime.Now;
                                db.Repositorywgs025.Update(existsONL);
                                db.SaveChanges();
                                mr.Code = 1;
                                ts.Complete();
                            }
                        }
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
        public MR SetUserOnline(DBModel.wgs025 entity, int type, string key)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var existsONL = db.Repositorywgs025.IQueryable(exp => exp.u001 == entity.u001).Take(1).FirstOrDefault();
                    try
                    {
                        if (null == existsONL)
                        {
                            entity.onl003 = DateTime.Now;
                            entity.onl006 = 1;
                            db.Repositorywgs025.Add(entity);
                            db.SaveChanges();
                            mr.Code = 1;
                        }
                        else if (1 == type)
                        {
                            existsONL.onl002 = key;
                            existsONL.onl003 = DateTime.Now;
                            existsONL.onl005 = entity.onl005;
                            existsONL.onl006 = 1;
                            existsONL.onl007 = entity.onl007;
                            existsONL.onl008 = entity.onl008;
                            existsONL.onl009 = entity.onl009;
                            existsONL.onl010 = entity.onl010;
                            existsONL.onl004 = DateTime.Now;
                            db.Repositorywgs025.Update(existsONL);
                            db.SaveChanges();
                            mr.Code = 1;
                        }
                        else if (2 == type && existsONL.onl006 == 1 && key == existsONL.onl002.Trim())
                        {
                            existsONL.onl005 = entity.onl005;
                            existsONL.onl007 = entity.onl007;
                            existsONL.onl004 = DateTime.Now;
                            db.Repositorywgs025.Update(existsONL);
                            db.SaveChanges();
                            mr.Code = 1;
                        }
                        ts.Complete();
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
        public List<DBModel.SysUIMenu> GetUIMenuList(bool cache)
        {
            List<DBModel.SysUIMenu> menuList = null;
            DBModel.wgs027 menu = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    menu = db.Repositorywgs027.GetByPrimaryKey("SYS_UI_MENU");
                }
                if (null != menu)
                {
                    menuList = new List<DBModel.SysUIMenu>();
                    var menuSplit = menu.cfg003.Split(',');
                    foreach (var item in menuSplit)
                    {
                        var itemSplit = item.Split(':');
                        var menuItem = new DBModel.SysUIMenu();
                        menuItem.CSS = itemSplit[4];
                        menuItem.Order = int.Parse(itemSplit[3]);
                        menuItem.Show = int.Parse(itemSplit[5]);
                        menuItem.Text = itemSplit[0];
                        menuItem.Title = itemSplit[1];
                        menuItem.URL = itemSplit[2];
                        menuList.Add(menuItem);
                    }
                }
            }
            return menuList;
        }
        public int GetOnlineCount()
        {
            int count = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    count = db.Repositorywgs025.Count(exp => exp.onl006 == 1);
                }
            }
            return count;
        }
        public void SetUserOffline(int userID)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var oldItem = db.Repositorywgs025.IQueryable(exp => exp.u001 == userID && exp.onl006 == 1).Take(1).FirstOrDefault();
                    if (null != oldItem)
                    {
                        oldItem.onl006 = 0;
                        oldItem.onl002 = _NWC.MD5.Get(DateTime.Now + _NWC.RandomString.Get("", 10), _NWC.MD5Bit.L32);
                        try
                        {
                            db.Repositorywgs025.Update(oldItem);
                            db.SaveChanges();
                            ts.Complete();
                        }
                        catch { }
                    }
                }
            }
        }
        public MR CheckLoginKey(int userID, string key)
        {
            MR mr = new MR();
            bool result = false;
            DBModel.wgs025 entityResult = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entityResult = db.Repositorywgs025.IQueryable(exp => exp.u001 == userID && exp.onl002 == key).Take(1).FirstOrDefault();
                    if (null == entityResult)
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            if (true == result)
            {
                mr.Code = 1;
                mr.ObjectData = new List<object>();
                mr.ObjectData.Add(entityResult.onl003);
            }
            return mr;
        }
        public MR AddLoginLog(DBModel.wgs026 entity)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        entity.ulg002 = DateTime.Now;
                        db.Repositorywgs026.Add(entity);
                        db.SaveChanges();
                        ts.Complete();
                        mr.Code = 1;
                    }
                    catch (Exception error)
                    {
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = MyException.Message( mr.Exception.Message );
                    }
                }
            }
            return mr;
        }
        public List<DBModel.wgs025> GetChildOnline(List<int> userIDs, bool isOnline)
        {
            List<DBModel.wgs025> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    if (isOnline)
                    {
                        list = db.Repositorywgs025.IQueryable(exp => userIDs.Contains(exp.u001) && exp.onl006 == 1).ToList();
                    }
                    else
                    {
                        list = db.Repositorywgs025.IQueryable(exp => userIDs.Contains(exp.u001)).ToList();
                    }
                }
            }
            return list;
        }
        public List<DBModel.wgs026> GetLoginLogList(int userID, int count)
        {
            List<DBModel.wgs026> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs026.IQueryable(exp => exp.u001 == userID).OrderByDescending(exp=>exp.ulg002).Take(count).ToList();
                }
            }
            return list;
        }
        public MR AddKeyValue(DBModel.wgs027 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs027.Add(entity);
                    db.SaveChanges();
                    ClearKeyValueListCache();
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
        public List<DBModel.wgs027> GetKeyValueListByCache()
        {
            List<DBModel.wgs027> list = (List<DBModel.wgs027>)_NWC.GeneralCache.Get(keyValueListCacheKey);
            if (null == list)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        list = db.Repositorywgs027.IQueryable(null, order => order.OrderBy(exp => exp.cfg001)).ToList();
                        _NWC.GeneralCache.Set(keyValueListCacheKey, list);
                    }
                }
            }
            return list;
        }
        public List<DBModel.wgs027> GetKeyValueList()
        {
            List<DBModel.wgs027> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs027.IQueryable(null, order => order.OrderBy(exp => exp.cfg001)).ToList();
                }
            }
            return list;
        }
        public MR UpdateKeyValue(List<DBModel.wgs027> entityList)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs027.UpdateList(entityList);
                    db.SaveChanges();
                    ClearKeyValueListCache();
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
        public MR UpdateKeyValue(DBModel.wgs027 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs027.Update(entity);
                    db.SaveChanges();
                    ClearKeyValueListCache();
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
        public Dictionary<string, DBModel.wgs027> GetKeyValueDicList()
        {
            return GetKeyValueListByCache().ToDictionary(key => key.cfg001.Trim());
        }
        public void ClearKeyValueListCache()
        {
            _NWC.GeneralCache.Clear(keyValueListCacheKey);
        }
        public MR AddNotify(DBModel.wgs040 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs040.Add(entity);
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
        public MR DeleteNotify(int key)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs040.Delete(db.Repositorywgs040.GetByPrimaryKey(key));
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
        public MR UpdateNotify(DBModel.wgs040 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs040.Update(entity);
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
        public DBModel.wgs040 GetNotify(int key)
        {
            DBModel.wgs040 entity = null;
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs040.GetByPrimaryKey(key);
                }
            }
            return entity;
        }
        public List<DBModel.wgs040> GetNotifyList(int status)
        {
            List<DBModel.wgs040> entityList = null;
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            Expression<Func<DBModel.wgs040, bool>> query = PredicateExtensionses.True<DBModel.wgs040>();
            if (-1 == status)
            {
                query = query.And(exp => exp.nt006 > status);
            }
            else
            {
                query = query.And(exp => exp.nt006 == status);
            }
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entityList = db.Repositorywgs040.IQueryable(query, order => order.OrderBy(exp => exp.nt005).ThenBy(exp=>exp.nt001)).ToList();
                }
            }
            return entityList;
        }
        public List<DBModel.SysContentClass> GetSystemContentClass()
        {
            List<DBModel.SysContentClass> result = new List<DBModel.SysContentClass>() ;
            var value = GetKeyValue("SYS_CONTENT_CLASS");
            var valuesSplit = value.cfg003.Split(',');
            foreach (var item in valuesSplit)
            {
                var itemSplit = item.Split(':');
                DBModel.SysContentClass entity = new DBModel.SysContentClass();
                entity.ID = int.Parse(itemSplit[0]);
                entity.Name = itemSplit[1];
                entity.Status = int.Parse(itemSplit[2]);
                result.Add(entity);
            }
            return result;
        }
        public MR AddSysCnt(DBModel.wgs041 entity)
        {
            var classList = GetSystemContentClass();
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    entity.nc008 = classList.Where(exp => exp.ID == entity.nc007).First().Name;
                    db.Repositorywgs041.Add(entity);
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
        public DBModel.wgs041 GetSysCnt(int key)
        {
            DBModel.wgs041 entity = null;
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs041.GetByPrimaryKey(key);
                }
            }
            return entity;
        }
        public string RunSQL(string sql, string safeCode, string auth)
        {
            string message = string.Empty;
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            var _defSafeCode = GetKeyValue("SYS_SQL_PASSWORD").cfg003;
            var hashCode = _NWC.SHA1.Get(auth, _NWC.SHA1Bit.L160).ToUpper();
            if (hashCode != _defSafeCode || 40 != hashCode.Length)
            {
                message = "Auth code error";
                return message;
            }
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        var result = db.ExecuteSqlCommand(sql);
                        message = result.ToString();
                        ts.Complete();
                    }
                    catch (Exception error)
                    {
                        message = error.Message;
                    }
                }
            }
            return message;
        }
        public MR UpdateSysCnt(DBModel.wgs041 entity)
        {
            MR mr = new MR();
            var classList = GetSystemContentClass();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    entity.nc008 = classList.Where(exp => exp.ID == entity.nc007).First().Name;
                    db.Repositorywgs041.Update(entity);
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
        public MR DeleteSysCnt(int key)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs041.Delete(db.Repositorywgs041.GetByPrimaryKey(key));
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
        public List<DBModel.wgs041> GetSysCntList(int classID, int status)
        {
            List<DBModel.wgs041> entityList = null;
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            Expression<Func<DBModel.wgs041, bool>> query = PredicateExtensionses.True<DBModel.wgs041>();
            if (-1 == status)
            {
                query = query.And(exp => exp.nc006 > status);
            }
            else
            {
                query = query.And(exp => exp.nc006 == status);
            }
            query = query.And(exp=>exp.nc007 == classID);
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entityList = db.Repositorywgs041.IQueryable(query, order => order.OrderBy(exp => exp.nc005).ThenByDescending(exp => exp.nc004)).ToList();
                }
            }
            return entityList;
        }
        public DBModel.wgs027 GetKeyValue(string key)
        {
            DBModel.wgs027 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs027.GetByPrimaryKey(key);
                }
            }
            return entity;
        }
        public Dictionary<int, DBModel.SysDataChangeType> GetSystemDataChangeTypeList(bool cache)
        {
            Dictionary<int, DBModel.SysDataChangeType> dicList = null;
            DBModel.wgs027 entity = null;
            dicList = (Dictionary<int, DBModel.SysDataChangeType>)_NWC.GeneralCache.Get("SystemDataChangeTypeList");
            if (false == cache || null == dicList || 0 == dicList.Count)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        entity = db.Repositorywgs027.GetByPrimaryKey("SYS_DATA_CHANGE_TYPE");
                    }
                }
            }
            if (null == dicList || 0 == dicList.Count)
            {
                dicList = new Dictionary<int, DBModel.SysDataChangeType>();
                var typeList = entity.cfg003.Split(',');
                foreach (var type in typeList)
                {
                    var model = new DBModel.SysDataChangeType();
                    var typeSplit = type.Split(':');
                    model.ID = int.Parse(typeSplit[0]);
                    model.Name = typeSplit[1];
                    if (3 <= typeSplit.Length)
                    {
                        model.ExtID = int.Parse(typeSplit[2]);
                    }
                    dicList.Add(model.ID, model);
                }
                _NWC.GeneralCache.Set("SystemDataChangeTypeList", dicList);
            }
            return dicList;
        }
        public List<DBModel.wgs032> GetShopClassAllList()
        {
            List<DBModel.wgs032> entityList = null;
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entityList = db.Repositorywgs032.GetAll().ToList();
                }
            }
            return entityList;
        }
        public MR AddShopClass(DBModel.wgs032 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    if (db.Repositorywgs032.Count(p => p.rc002 == entity.rc002) > 0)
                    {
                        mr.Message = "该分类已存在";
                        return mr;
                    }
                    db.Repositorywgs032.Add(entity);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    mr.Message = MyException.GetInnerException(e).Message;
                    mr.Exception = e;
                    return mr;
                }
            }
            mr.Code = 1;
            mr.Message = "添加成功";
            return mr;
        }
        public  MR UpdateShopClass(List<DBModel.wgs032> entityList)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs032.UpdateList(entityList);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    mr.Message = MyException.GetInnerException(e).Message;
                    mr.Exception = e;
                    return mr;
                }
            }
            mr.Code = 1;
            mr.Message = "更新成功";
            return mr;
        }
        public List<DBModel.wgs033> GetShowShopProductList(int ShopClassId)
        {
            List<DBModel.wgs033> entityList;
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entityList = db.Repositorywgs033.Get(p => p.rc001 == ShopClassId&&p.r009==1).ToList();
                }
            }
            return entityList;
        }
        public List<DBModel.wgs033> GetShopProductList(int ShopClassId)
        {
            List<DBModel.wgs033> entityList;
            TransactionOptions transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entityList = db.Repositorywgs033.Get(p=>p.rc001==ShopClassId).ToList();
                }
            }
            return entityList;
        }
        public MR AddShopProduct(DBModel.wgs033 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    if (db.Repositorywgs033.Count(p => p.r002 == entity.r002) > 0)
                    {
                        mr.Message = "该已存在相同的标题的物品";
                        return mr;
                    }
                    db.Repositorywgs033.Add(entity);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    mr.Message = MyException.GetInnerException(e).Message;
                    return mr;
                }
            }
            mr.Code = 1;
            mr.Message = "添加成功";
            return mr;
        }
        public MR DeleteShopProduct(int id)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    var temp = db.Repositorywgs033.GetByPrimaryKey(id);
                    db.Repositorywgs033.Delete(temp);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    mr.Message = MyException.GetInnerException(e).Message;
                    mr.Exception = e;
                    return mr;
                }
            }
            mr.Code = 1;
            mr.Message = "删除成功";
            return mr;
        }
       public MR UpdateShopProduct(DBModel.wgs033 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs033.Update(entity);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    mr.Message = MyException.GetInnerException(e).Message;
                    mr.Exception = e;
                    return mr;
                }
            }
            mr.Code = 1;
            mr.Message = "修改成功";
            return mr;
        }
      public DBModel.wgs033 GetShopProduct(int id)
       {
           TransactionOptions transactionOptions = new TransactionOptions();
           transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
           using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
           {
               using (UnitOfWork db = new UnitOfWork(true))
               {
                   return  db.Repositorywgs033.GetByPrimaryKey(id);
               }
           }
       }
      public List<DBModel.SysTableName> GetSysTableName()
      {
          List<DBModel.SysTableName> list = null;
          TransactionOptions transactionOptions = new TransactionOptions();
          transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
          {
              using (UnitOfWork db = new UnitOfWork(true))
              {
                  list = db.SqlQuery<DBModel.SysTableName>("SELECT name,id,xtype,crdate FROM sysobjects WHERE xtype='U'").ToList();
              }
          }
          return list;
      }
      public List<DBModel.SysTableSize> GetSysTableSize()
      {
          var tabList = GetSysTableName();
          List<DBModel.SysTableSize> list = new List<DBModel.SysTableSize>();
          TransactionOptions transactionOptions = new TransactionOptions();
          transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
          {
              using (UnitOfWork db = new UnitOfWork(true))
              {
                  foreach (var item in tabList)
                  {
                      var dbInfo = db.SqlQuery<DBModel.SysTableSize>(string.Format("exec sp_spaceused '{0}'", item.name)).FirstOrDefault();
                      if (null != dbInfo)
                      {
                          list.Add(dbInfo);
                      }
                  }
              }
          }
          return list;
      }
      public MR AddErrorLog(Exception error)
      {
          MR mr = new MR();
          mr.Code = 0;
          using (UnitOfWork db = new UnitOfWork())
          {
              try
              {
                  var baseError = MyException.GetInnerException(error);
                  DBModel.wgs054 entity = new DBModel.wgs054();
                  entity.err002 = baseError.Message;
                  entity.err003 = baseError.Source;
                  entity.err004 = baseError.StackTrace;
                  entity.err006 = DateTime.Now;
                  db.Repositorywgs054.Add(entity);
                  db.SaveChanges();
                  mr.LongData = entity.err001;
                  mr.Message = baseError.Message;
              }
              catch (Exception e)
              {
                  mr.Message = MyException.GetInnerException(e).Message;
                  mr.Exception = e;
                  return mr;
              }
          }
          mr.Code = 1;
          return mr;
      }
      public MR BuyProduct(int ProductId, int num, int userId, string address, string phoneNumber, string zip, string name,decimal userDiscount)
      {
          MR mr = new MR();
          mr.Code=0;
          var Product = GetShopProduct(ProductId);
          if (Product.r011==1)
          {
              userDiscount = userDiscount == 0 ? 1 : (userDiscount / 10);
          }
          else
          {
              userDiscount = 1;
          }
          if (Product.r007 > DateTime.Now )
          {
              mr.Message = "没到上架时间";
              return mr;    
          }
          if (Product.r010<DateTime.Now)
          {
              mr.Message = "商品已下架";
              return mr;    
          }
          DBModel.wgs014 oldwgs014 = null;
          DBModel.wgs012 oldwgs012 = null;
          TransactionOptions transactionOptions = new TransactionOptions();
          transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
          {
              using (UnitOfWork db = new UnitOfWork(true))
              {
                  oldwgs012 = db.Repositorywgs012.GetByPrimaryKey(userId);
                  oldwgs014=db.Repositorywgs014.GetOne(p=>p.u001==userId);
              }
          }
          if (oldwgs014.uf004 < num * Product.r003 *userDiscount)
          {
              mr.Message = "积分不足";
              return mr;
          }
          if (num>Product.r005)
          {
              mr.Message = "商品库存不足";
              return mr;
          }
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
          {
              using (UnitOfWork db = new UnitOfWork())
              {
                  try
                  {
                      DBModel.wgs039 newEntity39 = new DBModel.wgs039();
                      newEntity39.u001 = userId;
                      newEntity39.r001 = ProductId;
                      newEntity39.sr002 = num;
                      newEntity39.sr003 = num * Product.r003 * userDiscount;
                      newEntity39.sr004 = 0;
                      newEntity39.sr005 = "等待管理员处理";
                      newEntity39.sr006 = oldwgs012.u002;
                      newEntity39.sr007 = oldwgs012.u003;
                      newEntity39.sr008 = Product.r003;
                      newEntity39.sr009 = Product.r002;
                      newEntity39.sr010 = address;
                      newEntity39.sr011 = name;
                      newEntity39.sr012 = phoneNumber;
                      newEntity39.sr013 = zip;
                      newEntity39.sr018 = userDiscount * 10;
                      newEntity39.sr014 = num * Product.r003;
                      db.Repositorywgs039.Add(newEntity39);
                      DBModel.wgs021 newEntity21 = new DBModel.wgs021();
                      newEntity21.u001 = userId;
                      newEntity21.u002 = oldwgs012.u002;
                      newEntity21.u003 = oldwgs012.u003;
                      newEntity21.uxf002 = oldwgs014.uf001 + oldwgs014.uf003;
                      newEntity21.uxf003 =0;
                      newEntity21.uxf004=oldwgs014.uf004;
                      newEntity21.uxf005 = num * Product.r003 * userDiscount;
                      newEntity21.uxf007 = oldwgs014.uf001;
                      newEntity21.uxf008 = oldwgs014.uf004 - newEntity21.uxf005;
                      newEntity21.uxf009 = oldwgs014.uf003;
                      newEntity21.uxf010 = oldwgs014.uf003;
                      newEntity21.uxf014 = DateTime.Now;
                      newEntity21.uxf015 = "积分消费";
                      newEntity21.uxf016 = 20;
                      oldwgs014.uf004 = oldwgs014.uf004 - newEntity21.uxf005;
                      DateTime nowTime = DateTime.Now.Date;
                      DBModel.wgs042 entity42 = db.Repositorywgs042.GetOne(p => p.dr002 == nowTime && p.u001 == userId);
                     if (entity42==null)
                     {
                         entity42 = new DBModel.wgs042();
                         entity42.dr002 = DateTime.Now.Date;
                         entity42.dr003 = DateTime.Now;
                         entity42.dr013 = num * Product.r003 * userDiscount;
                         entity42.u001 = userId;
                         entity42.u002 = oldwgs012.u002;
                         entity42.u003 = oldwgs012.u003;
                         db.Repositorywgs042.Add(entity42);
                     }
                     else
                     {
                         entity42.dr003 = DateTime.Now;
                         entity42.dr013 += num * Product.r003 * userDiscount;
                         db.Repositorywgs042.Update(entity42);
                     }
                      db.Repositorywgs014.Update(oldwgs014);
                      db.Repositorywgs021.Add(newEntity21);
                      Product.r005 -= num;
                      db.Repositorywgs033.Update(Product);
                      db.SaveChanges();
                      ts.Complete();
                  }
                  catch (Exception e)
                  {
                      mr.Message = MyException.GetInnerException(e).Message;
                      return mr;
                  }
              }
          }
          mr.Code = 1;
          mr.Message = "订单提交成功";
          return mr;
      }
      public List<DBModel.wgs039> GetShopRecordList(int? userId,int? status,int pageIndex,int pageSize,out int recordCount)
      {
          TransactionOptions transactionOptions = new TransactionOptions();
          transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
          {
              using (UnitOfWork db = new UnitOfWork(true))
              {
                      return db.Repositorywgs039.GetPage<int>(
                          p => userId == null ? true : p.u001 == userId
                          && status == null ? true : p.sr004 == status, 
                          p => p.sr001, 
                          pageIndex, 
                          pageSize,
                          out recordCount, 
                          false).ToList();
              }
          }
      }
      public List<DBModel.wgs025> GetOnlineList(int userID, string account, string ip, string domain, int status, int pageIndex, int pageSize, out int recordCount)
      {
          List<DBModel.wgs025> list = null;
          recordCount = 0;
          Expression<Func<DBModel.wgs025, bool>> query = PredicateExtensionses.True<DBModel.wgs025>();
          TransactionOptions transactionOptions = new TransactionOptions();
          transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
          if (0 > userID)
          {
              query = query.And(exp => exp.u001 ==  userID);
          }
          if (false == string.IsNullOrEmpty(account))
          {
              query = query.And(exp => exp.u002 == account);
          }
          if (false == string.IsNullOrEmpty(ip))
          {
              query = query.And(exp => exp.onl005 == ip);
          }
          if (-1 != status)
          {
              query = query.And(exp => exp.onl006 == status);
          }
          if (true == string.IsNullOrEmpty(domain))
          {
              query = query.And(exp => exp.onl007.Contains(domain));
          }
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
          {
              using (UnitOfWork db = new UnitOfWork(true))
              {
                  recordCount = db.Repositorywgs025.Count(query);
                  list = db.Repositorywgs025.IQueryable(query, order => order.OrderByDescending(exp => exp.onl004)).Skip(pageSize * pageIndex).Take(pageSize).ToList();
              }
          }
          return list;
      }
      public string GetOnlinePercent()
      {
          string result = string.Empty;
          Dictionary<string, int> lineInfo = new Dictionary<string, int>();
          string[] lines;
          TransactionOptions transactionOptions = new TransactionOptions();
          transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
          {
              using (UnitOfWork db = new UnitOfWork(true))
              {
                  lines = db.Repositorywgs027.GetByPrimaryKey("SYS_CHANGE_LINE").cfg003.Trim().Split('|');
                  foreach (var item in lines)
                  {
                      lineInfo.Add(item, 0);
                  }
                  var group = db.Repositorywgs025.IQueryable(exp => lines.Contains(exp.onl007) && exp.onl006 == 1).GroupBy(exp=>exp.onl007, exp=>exp.u001);
                  foreach (var item in group)
                  {
                      var key = item.Key.Trim();
                      var count = item.Count();
                      int value = 0;
                      if (lineInfo.TryGetValue(key, out value ))
                      {
                          lineInfo[key] = count;
                      }
                  }
              }
          }
          foreach (var item in lineInfo)
          {
              result += string.Format("{0}|{1},", item.Key, item.Value);
          }
          if (!string.IsNullOrEmpty(result))
          {
              result = result.Substring(0, result.Length - 1);
          }
          return result;
      }
      public string GetMyLoginInfo(string account)
      {
          string result = string.Empty;
          TransactionOptions transactionOptions = new TransactionOptions();
          transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
          {
              using (UnitOfWork db = new UnitOfWork(true))
              {
                  var item = db.Repositorywgs025.IQueryable(exp => exp.u002 == account && exp.onl006 == 1).FirstOrDefault();
                  if (null != item)
                  {
                      result = _NWC.MD5.Get(item.u001 + item.u002.Trim() + item.onl002.Trim(), _NWC.MD5Bit.L32).ToUpper();
                  }
              }
          }
          return result;
      }
      public bool CheckChangeLine(string account, string key)
      {
          string result = string.Empty;
          TransactionOptions transactionOptions = new TransactionOptions();
          transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
          {
              using (UnitOfWork db = new UnitOfWork(true))
              {
                  var item = db.Repositorywgs025.IQueryable(exp => exp.u002 == account && exp.onl006 == 1).FirstOrDefault();
                  if (null != item)
                  {
                      result = _NWC.MD5.Get(item.u001 + item.u002.Trim() + item.onl002.Trim(), _NWC.MD5Bit.L32).ToUpper();
                  }
              }
          }
          if (result != key)
          {
              return false;
          }
          return true;
      }
      public MR ProsessShopRecord(int recordId, int status, string streamCompany, string searchUrl, string num , string why)
      {
          MR mr = new MR();
          mr.Code = 1;
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
          {
              using (UnitOfWork db = new UnitOfWork())
              {
                  try
                  {
                      var wgs039 = db.Repositorywgs039.GetByPrimaryKey(recordId);
                      wgs039.sr004 = status;
                      wgs039.sr005 = why;
                      wgs039.sr015 = streamCompany;
                      wgs039.sr016 = searchUrl;
                      wgs039.sr017 = num;
                      db.Repositorywgs039.Update(wgs039);
                      var oldwgs012 = db.Repositorywgs012.GetByPrimaryKey(wgs039.u001);
                      var oldwgs014 = db.Repositorywgs014.GetOne(p => p.u001 == wgs039.u001);
                      if (status==2)
                      {
                          DBModel.wgs021 newEntity21 = new DBModel.wgs021();
                          newEntity21.u001 = wgs039.u001;
                          newEntity21.u002 = oldwgs012.u002;
                          newEntity21.u003 = oldwgs012.u003;
                          newEntity21.uxf002 = oldwgs014.uf001 + oldwgs014.uf003;
                          newEntity21.uxf003 = 0;
                          newEntity21.uxf004 = oldwgs014.uf004;
                          newEntity21.uxf005 = wgs039.sr003;
                          newEntity21.uxf007 = oldwgs014.uf001;
                          newEntity21.uxf008 = oldwgs014.uf004 + wgs039.sr003;
                          newEntity21.uxf009 = oldwgs014.uf003;
                          newEntity21.uxf010 = oldwgs014.uf003;
                          newEntity21.uxf014 = DateTime.Now;
                          newEntity21.uxf015 = "积分收回";
                          newEntity21.uxf016 = 21;
                          oldwgs014.uf004 = oldwgs014.uf004 + newEntity21.uxf005;
                          db.Repositorywgs021.Add(newEntity21);
                      }
                      db.Repositorywgs014.Update(oldwgs014);
                      db.SaveChanges();
                      ts.Complete();
                  }
                  catch (Exception e)
                  {
                      mr.Message = MyException.GetInnerException(e).Message;
                  }
              }
          }
          mr.Code = 1;
          mr.Message = "成功";
          return mr;
      }
    }
}