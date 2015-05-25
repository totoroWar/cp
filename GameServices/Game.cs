using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBase;
using System.Linq.Expressions;
using System.Transactions;
using _NWC = NETCommon;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace GameServices
{
    public class Game : IGame
    {
        private string gameCacheKey = "GameListCache";
        private string gameMethodGroupCacheKey = "GameMethodGroupListCache";
        private string gameMethodCacheKey = "GameMethodListCache";
        private string gameClassCacheKey = "GameClassListCache";
        private string gameClassPrizeCache = "GameClassPrizeListCache";
        private string gamePrizeDataCacheKey = "GamePrizeDataCache";
        public MR AddGame(DBModel.wgs001 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs001.Add(entity);
                    db.SaveChanges();
                    mr.Code = 1;
                    ClearGameListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public MR UpdateGame(DBModel.wgs001 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs001.Update(entity);
                    db.SaveChanges();
                    mr.Code = 1;
                    ClearGameListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public MR UpdateGame(List<DBModel.wgs001> entityList)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs001.UpdateList(entityList);
                    db.SaveChanges();
                    mr.Code = 1;
                    ClearGameListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public List<DBModel.wgs001> GetGameList()
        {
            List<DBModel.wgs001> list = null;
            using (UnitOfWork db = new UnitOfWork(true))
            {
                list = db.Repositorywgs001.GetAll().OrderBy(exp => exp.g009).ToList();
            }
            return list;
        }
        public List<DBModel.wgs001> GetGameListByCache()
        {
            List<DBModel.wgs001> list = (List<DBModel.wgs001>)NETCommon.GeneralCache.Get(gameCacheKey);
            if (null == list)
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs001.GetAll().OrderBy(exp => exp.g009).ToList();
                    NETCommon.GeneralCache.Set(gameCacheKey, list);
                }
            }
            return list;
        }
        public DBModel.wgs001 GetGame(int key)
        {
            DBModel.wgs001 entity = null;
            using (UnitOfWork db = new UnitOfWork(true))
            {
                entity = db.Repositorywgs001.GetByPrimaryKey(key);
            }
            return entity;
        }
        public void ClearGameListCache()
        {
            NETCommon.GeneralCache.Clear(gameCacheKey);
        }
        public MR AddGameMethodGroup(DBModel.wgs003 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs003.Add(entity);
                    db.SaveChanges();
                    mr.Code = 1;
                    ClearGameMethodGroupListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public List<DBModel.wgs003> GetGameMethodGroupList()
        {
            return GetGameMethodGroupListByCache().OrderBy(exp => exp.gmg007).ToList();
        }
        public List<DBModel.wgs003> GetGameMethodGroupListByCache()
        {
            List<DBModel.wgs003> list = (List<DBModel.wgs003>)NETCommon.GeneralCache.Get(gameMethodGroupCacheKey);
            if (null == list)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        list = db.Repositorywgs003.GetAll().OrderBy(exp => exp.gmg007).ToList();
                        NETCommon.GeneralCache.Set(gameMethodGroupCacheKey, list);
                    }
                }
            }
            return list;
        }
        public void ClearGameMethodGroupListCache()
        {
            NETCommon.GeneralCache.Clear(gameMethodGroupCacheKey);
        }
        public MR UpdateGameMethodGroup(List<DBModel.wgs003> entityList)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs003.UpdateList(entityList);
                    db.SaveChanges();
                    mr.Code = 1;
                    ClearGameMethodGroupListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public MR AddGameMethod(DBModel.wgs002 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs002.Add(entity);
                    db.SaveChanges();
                    mr.Code = 1;
                    ClearGameMethodListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public MR UpdateGameMethod(DBModel.wgs002 entity)
        {
            var entityList = new List<DBModel.wgs002>();
            entityList.Add(entity);
            return UpdateGameMethod(entityList);
        }
        public MR UpdateGameMethod(List<DBModel.wgs002> entityList)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs002.UpdateList(entityList);
                    db.SaveChanges();
                    mr.Code = 1;
                    ClearGameMethodGroupListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public List<DBModel.wgs002> GetGameMethodList(int g001, int gm002)
        {
            List<DBModel.wgs002> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    list = db.Repositorywgs002.IQueryable(exp => exp.g001 == g001 && exp.gm002 == gm002).OrderBy(exp => exp.gm013).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs002> GetGameMethodList()
        {
            List<DBModel.wgs002> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    list = db.Repositorywgs002.GetAll().OrderBy(exp => exp.gm013).ToList();
                    NETCommon.GeneralCache.Set(gameMethodCacheKey, list);
                }
            }
            return list;
        }
        public List<DBModel.wgs002> GetGameMethodListByCache()
        {
            List<DBModel.wgs002> list = (List<DBModel.wgs002>)NETCommon.GeneralCache.Get(gameMethodCacheKey);
            if (null == list || 0 == list.Count)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        list = db.Repositorywgs002.GetAll().OrderBy(exp => exp.gm013).ToList();
                        NETCommon.GeneralCache.Set(gameMethodCacheKey, list);
                    }
                }
            }
            return list;
        }
        public void ClearGameMethodListCache()
        {
            NETCommon.GeneralCache.Clear(gameMethodCacheKey);
        }
        public MR AddGameSession(int type, int gameID, DateTime dt, long ser_no, long start_no, int count, DateTime dts, DateTime dte, DateTime dto, int dtso, int dteo, int dtoo)
        {
            MR mr = new MR();
            mr.Code = 0;
            DateTime dt_build_start = dt;
            DateTime dt_create = DateTime.Now;
            List<DBModel.wgs005> entityList = new List<DBModel.wgs005>();
            int gameClassID = 0;
            var gameClassList = GetGameClassList();
            foreach (var item in gameClassList)
            {
                var gameIncludeIDs = item.gc004.Split(',');
                foreach (var id in gameIncludeIDs)
                {
                    if (int.Parse(id) == gameID)
                    {
                        gameClassID = item.gc001;
                        break;
                    }
                }
                if (0 != gameClassID)
                {
                    break;
                }
            }
            for (long i = start_no; i <= count; i++)
            {
                string lottery_no = "";
                if (i < 10)
                {
                    lottery_no = "00" + i;
                }
                else if (i >= 10 && i < 100)
                {
                    lottery_no = "0" + i;
                }
                else
                {
                    lottery_no = i.ToString();
                }
                string _sdts = dts.ToString("yyyy-MM-dd HH:mm:ss");
                string _sdte = dte.ToString("yyyy-MM-dd HH:mm:ss");
                string _sdto = dto.ToString("yyyy-MM-dd HH:mm:ss");
                string _lottery_no = dt_build_start.ToString("yyyyMMdd") + lottery_no;
                if (1 == type)
                {
                    _lottery_no = (ser_no + i).ToString();
                }
                DBModel.wgs005 saveItem = new DBModel.wgs005();
                saveItem.g001 = gameID;
                saveItem.gc001 = gameClassID;
                saveItem.gs002 = _lottery_no;
                saveItem.gs003 = dts;
                saveItem.gs004 = dte;
                saveItem.gs005 = dto;
                saveItem.gs006 = dt_create;
                saveItem.gs008 = 0;
                saveItem.gs009 = 0;
                saveItem.gs010 = 0;
                saveItem.gs011 = DateTime.Now > dte ? (byte)0 : (byte)1;
                entityList.Add(saveItem);
                dts = dts.AddSeconds(dtso);
                dte = dte.AddSeconds(dteo);
                dto = dto.AddSeconds(dtoo);
            }
            try
            {
                AddGameSession(entityList);
                mr.Code = 1;
            }
            catch (Exception error)
            {
                mr.Exception = MyException.GetInnerException(error);
                mr.Message = mr.Exception.Message;
            }
            return mr;
        }
        public MR AddGameSession(List<DBModel.wgs005> entityList)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    for (int i = 0; i < entityList.Count(); i++)
                    {
                        db.Repositorywgs005.Add(entityList[i]);
                        db.SaveChanges();
                    }
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
        public MR AddGameSession(DBModel.wgs005 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs005.Add(entity);
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
        public List<DBModel.wgs005> GetGameSessionList(int gameID, int recordCount)
        {
            List<DBModel.wgs005> list = null;
            Expression<Func<DBModel.wgs005, bool>> query = PredicateExtensionses.True<DBModel.wgs005>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DateTime now = DateTime.Now;
            if (20 == gameID || 26 == gameID)
            {
                now = now.AddDays(-4);
            }
            query = query.And(exp => exp.g001 == gameID && exp.gs004 > now);
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs005.IQueryable(query, order => order.OrderBy(exp => exp.gs004)).Skip(0).Take(recordCount).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs005> GetGameSessionList(int gameID, int status, int pageIndex, int pageSize, out int recordCount)
        {
            List<DBModel.wgs005> list = null;
            recordCount = 0;
            Expression<Func<DBModel.wgs005, bool>> query = PredicateExtensionses.True<DBModel.wgs005>();
            query = exp => exp.g001 == gameID;
            if (-1 != status)
            {
                query = query.And(exp => exp.gs010 == status);
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCount = db.Repositorywgs005.Count(query);
                    list = db.Repositorywgs005.IQueryable(query, order => order.OrderByDescending(exp => exp.gs005)).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs038> GetGameResultList(int gameID, int haveResult, int haveCalc, int pageIndex, int pageSize, out int recordCount)
        {
            List<DBModel.wgs038> list = null;
            recordCount = 0;
            Expression<Func<DBModel.wgs038, bool>> query = PredicateExtensionses.True<DBModel.wgs038>();
            query = exp => exp.g001 == gameID;
            if (-1 != haveResult)
            {
                query = query.And(exp => exp.gsr004 == haveResult);
            }
            if (-1 != haveCalc)
            {
                query = query.And(exp => exp.gsr005 == haveCalc);
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCount = db.Repositorywgs038.Count(query);
                    list = db.Repositorywgs038.IQueryable(query, order => order.OrderByDescending(exp => exp.gs005)).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs038> GetNeedResultList(int gameID, bool haveResult, DateTime? dt, int recordCount)
        {
            List<DBModel.wgs038> list = null;
            Expression<Func<DBModel.wgs038, bool>> query = PredicateExtensionses.True<DBModel.wgs038>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                query = query.And(exp => exp.g001 == gameID && exp.gs007 == null);
                if (dt.HasValue)
                {
                    query = query.And(exp => exp.gs004 <= dt);
                }
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs038.IQueryable(query, order => order.OrderByDescending(exp => exp.gs004)).Take(recordCount).ToList();
                }
            }
            return list;
        }
        public MR SetGameResult(long gameSessionID, string result, bool overwrite, DateTime? getDT, DateTime? openDT)
        {
            MR mr = new MR();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var item = db.Repositorywgs038.IQueryable(exp => exp.gs001 == gameSessionID).FirstOrDefault();
                    if (null == item)
                    {
                        mr.Message = "信息不存在";
                        return mr;
                    }
                    if ((false == string.IsNullOrEmpty(item.gs007) && false == string.IsNullOrWhiteSpace(item.gs007)) && false == overwrite)
                    {
                        mr.Code = 1;
                        mr.Message = "已经有结果";
                        return mr;
                    }
                    item.gs007 = result;
                    item.gsr004 = 3;
                    item.gsr002 = getDT;
                    item.gsr003 = openDT;
                    db.Repositorywgs038.Update(item);
                    try
                    {
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
            return mr;
        }
        public DBModel.wgs038 GetGameResult(long key, int type)
        {
            DBModel.wgs038 result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    if (0 == type)
                    {
                        result = db.Repositorywgs038.IQueryable(exp => exp.gsr001 == key).FirstOrDefault();
                    }
                    else
                    {
                        result = db.Repositorywgs038.IQueryable(exp => exp.gs001 == key).FirstOrDefault();
                    }
                    if (null == result)
                    {
                        return null;
                    }
                }
            }
            return result;
        }
        public MR SetGameResultClose()
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs038.GameSessionOtherClose();
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
        public MR UpdateGameResult(DBModel.wgs038 entity)
        {
            MR mr = new MR();
            var oldData = GetGameResult(entity.gsr001, 0);
            if (null == oldData)
            {
                mr.Message = "获取数据失败";
                return mr;
            }
            DateTime dtNow = DateTime.Now;
            if (false == string.IsNullOrEmpty(entity.gs007) && false == string.IsNullOrWhiteSpace(entity.gs007))
            {
                oldData.gs007 = entity.gs007;
                oldData.gsr002 = entity.gsr002 == null ? dtNow : entity.gsr002;
                oldData.gsr003 = entity.gsr003;
                oldData.gsr004 = entity.gsr004;
                oldData.gsr005 = entity.gsr005;
            }
            oldData.gs002 = entity.gs002;
            oldData.gs003 = entity.gs003;
            oldData.gs004 = entity.gs004;
            oldData.gs005 = entity.gs005;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs038.Update(oldData);
                    db.SaveChanges();
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
        public MR DeleteGameResult(List<long> gameSessionIDs, int type)
        {
            MR mr = new MR();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    List<DBModel.wgs038> gameResultList = null;
                    if (0 == type)
                    {
                        gameResultList = db.Repositorywgs038.IQueryable(exp => gameSessionIDs.Contains(exp.gsr001)).ToList();
                    }
                    else if (1 == type)
                    {
                        gameResultList = db.Repositorywgs038.IQueryable(exp => gameSessionIDs.Contains(exp.gs001)).ToList();
                    }
                    db.Repositorywgs038.DeleteList(gameResultList);
                    try
                    {
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
            return mr;
        }
        public MR AddGameClass(DBModel.wgs006 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs006.Add(entity);
                    db.SaveChanges();
                    ClearGameClassListCache();
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
        public MR UpdateGameClass(DBModel.wgs006 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs006.Update(entity);
                    db.SaveChanges();
                    ClearGameClassListCache();
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
        public List<DBModel.wgs006> GetGameClassList()
        {
            List<DBModel.wgs006> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs006.GetAll().ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs006> GetGameClassListByCache()
        {
            List<DBModel.wgs006> list = (List<DBModel.wgs006>)NETCommon.GeneralCache.Get(gameClassCacheKey);
            if (null == list || 0 == list.Count)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        list = db.Repositorywgs006.GetAll().OrderBy(exp => exp.gc005).ToList();
                        NETCommon.GeneralCache.Set(gameClassCacheKey, list);
                    }
                }
            }
            return list;
        }
        public void ClearGameClassListCache()
        {
            NETCommon.GeneralCache.Clear(gameClassCacheKey);
        }
        public MR AddGameClassPrize(DBModel.wgs007 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs007.Add(entity);
                    db.SaveChanges();
                    ClearGameClassPrizeListCache();
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
        public MR UpdateGameClassPrize(List<DBModel.wgs007> entityList)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs007.UpdateList(entityList);
                    db.SaveChanges();
                    ClearGameClassPrizeListCache();
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
        public List<DBModel.wgs007> GetGameClassPrize(int gameClassID)
        {
            return GetGameClassPrizeByCache().Where(exp => exp.gc001 == gameClassID).OrderBy(exp => exp.gp009).ToList();
        }
        public List<DBModel.wgs007> GetGameClassPrizeByCache()
        {
            List<DBModel.wgs007> list = (List<DBModel.wgs007>)NETCommon.GeneralCache.Get(gameClassPrizeCache);
            if (null == list || 0 == list.Count)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        list = db.Repositorywgs007.GetAll().OrderBy(exp => exp.gp009).ToList();
                        NETCommon.GeneralCache.Set(gameClassPrizeCache, list);
                    }
                }
            }
            return list;
        }
        public void ClearGameClassPrizeListCache()
        {
            NETCommon.GeneralCache.Clear(gameClassPrizeCache);
        }
        public MR InitGamePrizeData(int gameClassID, int gameClassPrizeID)
        {
            MR mr = new MR();
            mr.Code = 0;
            var selectGameMethodList = GetGameMethodListByCache().Where(exp => exp.g001 == gameClassID && exp.gm002 != 0).OrderBy(exp => exp.gm013).ToList();
            if (0 == selectGameMethodList.Count)
            {
                mr.Message = "游戏玩法不存在";
                return mr;
            }
            if (0 == GetGameClassPrizeByCache().Where(exp => exp.gp001 == gameClassPrizeID).Count())
            {
                mr.Message = "奖金组不存在";
                return mr;
            }
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    int needAddRecord = 0;
                    foreach (var gm in selectGameMethodList)
                    {
                        DBModel.wgs008 gmpd = new DBModel.wgs008();
                        gmpd.gc001 = gm.g001;
                        gmpd.gm001 = gm.gm001;
                        gmpd.gm002 = gm.gm002;
                        gmpd.gp001 = gameClassPrizeID;
                        int existsCount = db.Repositorywgs008.Count(exp => exp.gp001 == gmpd.gp001 && exp.gc001 == gmpd.gc001 && gm.gm001 == gmpd.gm001);
                        if (0 == existsCount)
                        {
                            db.Repositorywgs008.Add(gmpd);
                            needAddRecord++;
                        }
                    }
                    if (0 < needAddRecord)
                    {
                        db.SaveChanges();
                    }
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
        public MR UpdateGamePrizeData(List<DBModel.wgs008> entityList)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs008.UpdateList(entityList);
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
        public List<DBModel.wgs008> GetGameMethodPrizeData(int gameClassPrizeID)
        {
            List<DBModel.wgs008> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs008.IQueryable(exp => exp.gp001 == gameClassPrizeID).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs008> GetGameMethodPrizeData(int userID, int gameClassID, int gameClassPrizeID)
        {
            List<DBModel.wgs008> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var aguPointList = db.Repositorywgs017.IQueryable(exp => exp.u001 == userID).ToList();
                    if (0 == aguPointList.Count())
                    {
                        return null;
                    }
                    var aguPointItem = aguPointList.Where(exp => exp.gp001 == gameClassPrizeID).FirstOrDefault();
                    if (null == aguPointItem)
                    {
                        return null;
                    }
                    var gppList = db.Repositorywgs007.IQueryable(exp => exp.gp001 == aguPointItem.gp001).ToList();
                    if (0 == gppList.Count())
                    {
                        return null;
                    }
                    var gppItem = gppList.FirstOrDefault();
                    list = db.Repositorywgs008.IQueryable(exp => exp.gc001 == gppItem.gc001 && exp.gp001 == gppItem.gp001).ToList();
                }
            }
            return list;
        }
        public DBModel.wgs007 GetGameClassPrize(int gameClassID, int gamePrizeGroupID)
        {
            DBModel.wgs007 entity = null;
            entity = GetGameClassPrizeByCache().Where(exp => exp.gc001 == gameClassID && exp.gp001 == gamePrizeGroupID).FirstOrDefault();
            return entity;
        }
        public MR AddGPDData(DBModel.wgs028 entity)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs028.Add(entity);
                    db.SaveChanges();
                    ClearGPDDataListCache();
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
        public MR UpdateGPDData(List<DBModel.wgs028> entityList)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs028.UpdateList(entityList);
                    db.SaveChanges();
                    ClearGPDDataListCache();
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
        public MR DeleteGPDData(int key)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs028.Delete(db.Repositorywgs028.GetByPrimaryKey(key));
                    db.SaveChanges();
                    ClearGPDDataListCache();
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
        public List<DBModel.wgs028> GetGPDDataGroupList(int gameClassID)
        {
            return GetGPDDataList(gameClassID, 1);
        }
        public List<DBModel.wgs028> GetGPDDataList(int gameClassID, decimal group)
        {
            List<DBModel.wgs028> list = GetGPDDataListByCache();
            list = list.Where(exp => exp.gc001 == gameClassID && exp.gtp009 == group).ToList();
            return list;
        }
        public List<DBModel.wgs028> GetGPDDataListByCache()
        {
            List<DBModel.wgs028> list = (List<DBModel.wgs028>)_NWC.GeneralCache.Get(gamePrizeDataCacheKey);
            if (null == list)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        list = db.Repositorywgs028.IQueryable(null, order => order.OrderBy(exp => exp.gtp012)).ToList();
                        _NWC.GeneralCache.Set(gamePrizeDataCacheKey, list);
                    }
                }
            }
            return list;
        }
        public MR InitGPDData(int gameClassID, int gamePrizeClassID)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    if (0 >= gamePrizeClassID || 0 >= gamePrizeClassID)
                    {
                        throw new Exception("参数不能为0");
                    }
                    var gpdtList = db.Repositorywgs028.IQueryable(exp => exp.gtp009 == 0 && exp.gc001 == gameClassID).ToList();
                    foreach (var item in gpdtList)
                    {
                        var count = db.Repositorywgs029.Count(exp => exp.gp001 == gamePrizeClassID && exp.gc001 == gameClassID && exp.gtp001 == item.gtp001);
                        if (0 == count)
                        {
                            DBModel.wgs029 entity = new DBModel.wgs029();
                            entity.gtp001 = item.gtp001;
                            entity.gc001 = gameClassID;
                            entity.gp001 = gamePrizeClassID;
                            db.Repositorywgs029.Add(entity);
                        }
                        db.SaveChanges();
                        mr.Code = 1;
                    }
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public List<DBModel.wgs029> GetSetGPDDataList(int gamePrizeClassID)
        {
            List<DBModel.wgs029> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs029.IQueryable(exp => exp.gp001 == gamePrizeClassID).ToList();
                }
            }
            return list;
        }
        public MR UpdateSetGPDData(List<DBModel.wgs029> entityList)
        {
            MR mr = new MR();
            mr.Code = 0;
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs029.UpdateList(entityList);
                    db.SaveChanges();
                    ClearGPDDataListCache();
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
        public void ClearGPDDataListCache()
        {
            _NWC.GeneralCache.Clear(gamePrizeDataCacheKey);
        }
        public DBModel.wgs005 GetCurrentGameSession(int gameID)
        {
            DBModel.wgs005 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DateTime now = DateTime.Now;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs005.IQueryable(exp => exp.gs004 >= now && exp.g001 == gameID, order => order.OrderBy(exp => exp.gs004)).Take(2).FirstOrDefault();
                }
            }
            return entity;
        }
        public MR CheckLimitBet(string account, string issue, decimal nowAmount, int g001)
        {
            MR mr = new MR();
            mr.Code = 1;
            DBModel.wgs027 config = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DateTime now = DateTime.Now;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    config = db.Repositorywgs027.GetByPrimaryKey("SYS_BET_MAX_LIMIT");
                    if (null == config)
                    {
                        mr.Code = 1;
                        return mr;
                    }
                    var configSplit = config.cfg003.Split(',');
                    foreach (var item in configSplit)
                    {
                        var itemSplit = item.Split(':');
                        var limitUser = itemSplit[0].Split('|');
                        foreach (var user in limitUser)
                        {
                            var count = db.Repositorywgs048.Count(exp => exp.u001n == user && exp.u002n == account);
                            if (0 < count)
                            {
                                if (nowAmount >= decimal.Parse(itemSplit[1]))
                                {
                                    mr.Code = 0;
                                    mr.Message = config.cfg004 + itemSplit[1];
                                    return mr;
                                }
                                var sumBet = db.Repositorywgs045.IQueryable(exp => exp.gs002 == issue && exp.g001 == g001 && exp.u002 == account).Sum(exp => (decimal?)exp.so004);
                                if (sumBet >= decimal.Parse(itemSplit[1]))
                                {
                                    mr.Code = 0;
                                    mr.Message = config.cfg004 + itemSplit[1];
                                    return mr;
                                }
                            }
                        }
                    }
                }
            }
            return mr;
        }
        public DBModel.wgs005 GetCurrentLastGameSession(int gameID)
        {
            DBModel.wgs005 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DateTime now = DateTime.Now;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs005.IQueryable(exp => exp.gs004 <= now && exp.g001 == gameID).OrderByDescending(exp => exp.gs004).Take(2).FirstOrDefault();
                }
            }
            return entity;
        }
        public List<DBModel.wgs005> GetGameSessionList(int gameID, DateTime ds, DateTime de)
        {
            List<DBModel.wgs005> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs005.IQueryable(exp => exp.g001 == gameID && exp.gs003 >= ds && exp.gs004 <= de).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs028> GetIncludePrizeName(int methodID, List<DBModel.wgs028> existsList)
        {
            var tempMethodID = methodID.ToString();
            List<DBModel.wgs028> list = new List<DBModel.wgs028>();
            foreach (var item in existsList)
            {
                var tempSplit = item.gtp010.Split(',');
                foreach (var tempItem in tempSplit)
                {
                    if (tempMethodID == tempItem)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
        }
        public MR AddOrder(DBModel.LotteryOrder orderDataAll)
        {
            /*
             * 0 no mind
             * 1 ok 
             * 2 fail
             * 3 error
             */
            MR mr = new MR();
            int currentUserID = 0;
            int lotteryID = 0;
            lotteryID = 0;
            List<DBModel.wgs013> utList = new List<DBModel.wgs013>();
            List<DBModel.wgs017> upLevelList = new List<DBModel.wgs017>();
            DBModel.wgs012 currentUser = null;
            DBModel.wgs027 combuyMinPercentSet = null;
            DBModel.wgs027 combuyMinMoneySet = null;
            DBModel.wgs027 combuyPwdFormat = null;
            DBModel.wgs027 betAmountLimit = null;
            DBModel.wgs027 methodLimit = null;
            string upAll = string.Empty;
            bool betLimit = false;
            decimal? curSessionAmount = 0.0000m;
            List<string> amountLimitMessage = new List<string>();
            List<string> amountLimitValue = new List<string>();
            DBModel.wgs017 defUP = null;
            var tsoUP = new TransactionOptions();
            tsoUP.IsolationLevel = IsolationLevel.ReadUncommitted;
            decimal gpMaxPoint = 0.0000m;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tsoUP))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    defUP = db.Repositorywgs017.IQueryable(exp => exp.u001 == orderDataAll.UserID && exp.gc001 == orderDataAll.gc001).Take(1).FirstOrDefault();
                    currentUser = db.Repositorywgs012.IQueryable(exp => exp.u001 == orderDataAll.UserID, null, "wgs014").Take(1).FirstOrDefault();
                    methodLimit = db.Repositorywgs027.GetByPrimaryKey("SYS_METHOD_LIMIT");
                    betAmountLimit = db.Repositorywgs027.GetByPrimaryKey("SYS_BET_MAX_AMOUNT");
                    if (1 == orderDataAll.lt_combuy_check)
                    {
                        combuyMinPercentSet = db.Repositorywgs027.GetByPrimaryKey("SYS_COMBUY_COUNT");
                        combuyMinMoneySet = db.Repositorywgs027.GetByPrimaryKey("SYS_COMBUY_MIN_MONEY");
                        combuyPwdFormat = db.Repositorywgs027.GetByPrimaryKey("SYS_COMBUY_PWDFORMAT");
                    }
                    gpMaxPoint = db.Repositorywgs007.IQueryable(exp => exp.gp001 == defUP.gp001).Select(exp => exp.gp008).FirstOrDefault();
                    amountLimitMessage = betAmountLimit.cfg004.Split(',').ToList();
                    amountLimitValue = betAmountLimit.cfg003.Split('|').ToList();
                    betLimit = int.Parse(amountLimitValue[0]) == 0 ? false : true;
                    if (betLimit)
                    {
                        curSessionAmount = db.Repositorywgs045.IQueryable(exp => exp.gs002 == orderDataAll.lt_issue_start.Trim() && exp.g001 == orderDataAll.lotteryid && exp.u001 == orderDataAll.UserID && exp.so021 == 0).Sum(exp => (decimal?)exp.so004);
                    }
                }
            }
            var prizeNameInfo = GetGPDDataList(orderDataAll.gc001, 0);
            var prizeDataInfo = GetSetGPDDataList((int)defUP.gp001);
            Assembly _moduleLoad = null;
            object _moduleObject = null;
            Type _moduleType = null;
            try
            {
                __CGM(orderDataAll.gc001, out _moduleLoad, out _moduleObject, out _moduleType);
            }
            catch (Exception error)
            {
                mr.Message = error.Message;
                return mr;
            }
            DateTime dtNow = DateTime.Now;
            var orderList = new List<DBModel.wgs022>();
            var gmDicList = GetGameMethodListByCache().ToDictionary(exp => exp.gm001);
            decimal sumAmount = 0m;
            foreach (var ordDataItem in orderDataAll.OrderDataList)
            {
                if (methodLimit.cfg003.Length > 0)
                {
                    var limitIDs = methodLimit.cfg003.Split(',');
                    var limitCount = limitIDs.Count(exp => exp == ordDataItem.gm001.ToString());
                    if (limitCount > 0)
                    {
                        mr.Code = 3;
                        mr.Message = gmDicList[ordDataItem.gm001].gm004.ToString() + methodLimit.cfg004;
                        return mr;
                    }
                }
                DBModel.wgs022 iOrder = new DBModel.wgs022();
                MR mrGSAllow = GameSessionIsAllow(orderDataAll.lotteryid, orderDataAll.lt_issue_start);
                if (mrGSAllow.Code == 0)
                {
                    mr.Message = mrGSAllow.Message;
                    return mr;
                }
                if (0 == currentUserID)
                {
                    currentUserID = orderDataAll.UserID;
                    lotteryID = orderDataAll.lotteryid;
                    var tsoUT = new TransactionOptions();
                    tsoUT.IsolationLevel = IsolationLevel.ReadUncommitted;
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tsoUT))
                    {
                        using (UnitOfWork db = new UnitOfWork(true))
                        {
                            utList = db.Repositorywgs013.IQueryable(exp => exp.u002 == currentUserID).OrderBy(exp => exp.ur001).ToList();
                            foreach (var ut in utList)
                            {
                                var upItem = db.Repositorywgs017.IQueryable(exp => exp.gc001 == orderDataAll.gc001 && exp.u001 == ut.u001).FirstOrDefault();
                                if (null != upItem)
                                {
                                    upAll += string.Format("{0}:{1}:{2}", ut.u001, Math.Round(upItem.up002, 2), Math.Round(upItem.up003, 2));
                                    upAll += ",";
                                }
                            }
                            if (0 == currentUser.u012)
                            {
                                upAll = string.Empty;
                            }
                            else
                            {
                                var upItem = db.Repositorywgs017.IQueryable(exp => exp.gc001 == orderDataAll.gc001 && exp.u001 == orderDataAll.UserID).FirstOrDefault();
                                upAll += string.Format("{0}:{1}:{2}", orderDataAll.UserID, Math.Round(upItem.up002, 2), Math.Round(upItem.up003, 2));
                                upAll += ",";
                            }
                        }
                    }
                    if (0 < upAll.Length)
                    {
                        upAll = upAll.Substring(0, upAll.Length - 1);
                    }
                }
                iOrder.so029 = upAll;
                iOrder.up001 = defUP.u001;
                iOrder.u001 = orderDataAll.UserID;
                iOrder.u002 = orderDataAll.UserName;
                iOrder.u003 = orderDataAll.UserNickname;
                iOrder.g001 = orderDataAll.lotteryid;
                iOrder.gc001 = orderDataAll.gc001;
                iOrder.gm001 = ordDataItem.gm001;
                iOrder.gm002 = ordDataItem.gm002;
                object callResultObject = new object();
                try
                {
                    __CGMM(ref _moduleObject, ref _moduleType, 0, "Codes", ordDataItem.codes, out callResultObject);
                    __CGMM(ref _moduleObject, ref _moduleType, 0, "Position", ordDataItem.position, out callResultObject);
                }
                catch (Exception error)
                {
                    mr.Code = 3;
                    mr.Message = "系统检测到您的数据有错，请联系平台客服。代码：" + error.Message;
                    return mr;
                }
                try
                {
                    __CGMM(ref _moduleObject, ref _moduleType, 2, "Check" + iOrder.gm001, null, out callResultObject);
                }
                catch (Exception error)
                {
                    mr.Code = 3;
                    mr.Message = "系统检测到您的数据有错，请联系平台客服。代码：" + "CALL_Check" + iOrder.gm001 + "|" + error.Message;
                    return mr;
                }
                var methodCheck = (bool)callResultObject;
                if (false == methodCheck)
                {
                    mr.Code = 3;
                    mr.Message = "系统检测到您的数据有错，请联系平台客服。代码：" + "Check" + iOrder.gm001;
                    return mr;
                }
                try
                {
                    __CGMM(ref _moduleObject, ref _moduleType, 2, "Nums" + iOrder.gm001, null, out callResultObject);
                }
                catch (Exception error)
                {
                    mr.Code = 2;
                    mr.Message = "系统检测到您的数据有错，请联系平台客服。代码：" + "Call_Nums" + iOrder.gm001 + "，" + ordDataItem.nums + "|" + error.Message;
                    return mr;
                }
                var methodCheckNums = (int)callResultObject;
                if (ordDataItem.nums != methodCheckNums)
                {
                    mr.Code = 2;
                    mr.Message = "系统检测到您的数据有错，请联系平台客服。代码：" + "Nums" + iOrder.gm001 + "，" + ordDataItem.nums + "!=" + methodCheckNums;
                    return mr;
                }
                ordDataItem.nums = methodCheckNums;
                iOrder.so014 = orderDataAll.IP;
                iOrder.so015 = orderDataAll.Domain;
                iOrder.so007 = dtNow;
                iOrder.so012 = ordDataItem.desc;
                iOrder.so002 = ordDataItem.codes;
                decimal orderSumAmount = ordDataItem.nums * ordDataItem.times * 2m;
                if (2 == ordDataItem.mode)
                {
                    orderSumAmount /= 10;
                }
                else if (3 == ordDataItem.mode)
                {
                    orderSumAmount /= 100;
                }
                iOrder.so004 = ordDataItem.money;
                iOrder.so004 = orderSumAmount;
                iOrder.so003 = ordDataItem.nums;
                if (1 == orderDataAll.lt_combuy_check)
                {
                    if ("yes" == orderDataAll.lt_trace_if)
                    {
                        mr.Code = 3;
                        mr.Message = "合买、追号不能同时进行";
                        return mr;
                    }
                    var combuyMinPercent = int.Parse(combuyMinPercentSet.cfg003);
                    if (combuyMinPercent > orderDataAll.lt_self_percent || 100 < orderDataAll.lt_self_percent)
                    {
                        mr.Code = 3;
                        mr.Message = combuyMinPercentSet.cfg004;
                        return mr;
                    }
                    var combuyMinMoney = decimal.Parse(combuyMinMoneySet.cfg003);
                    if (ordDataItem.money < combuyMinMoney)
                    {
                        mr.Code = 3;
                        mr.Message = combuyMinMoneySet.cfg004;
                        return mr;
                    }
                }
                if (false == string.IsNullOrEmpty(orderDataAll.lt_combuy_password) && false == string.IsNullOrWhiteSpace(orderDataAll.lt_combuy_password))
                {
                    if (false == Regex.IsMatch(orderDataAll.lt_combuy_password, combuyPwdFormat.cfg003))
                    {
                        mr.Code = 3;
                        mr.Message = combuyPwdFormat.cfg004;
                        return mr;
                    }
                }
                iOrder.so005 = ordDataItem.times;
                iOrder.so024 = orderDataAll.lt_sel_dyprize;
                iOrder.so016 = 0;
                iOrder.so009 = 0;
                iOrder.so030 = ordDataItem.type + "_" + ordDataItem.methodid;
                iOrder.so033 = false;
                var newso024 = string.Empty;
                iOrder.so013 = ordDataItem.point;
                if (iOrder.so013 * 100 > defUP.up003)
                {
                    iOrder.so013 = defUP.up003 / 100;
                }
                var findIncludePrizeName = GetIncludePrizeName(ordDataItem.methodid, prizeNameInfo);
                if (0 == findIncludePrizeName.Count)
                {
                    mr.Code = 3;
                    mr.Message = "奖金项不存在GM" + ordDataItem.methodid;
                    return mr;
                }
                else
                {
                    foreach (var prizeNameItem in findIncludePrizeName)
                    {
                        var usePointOri = Math.Round(iOrder.so013 * 100.0000m, 1);
                        var prizeDataItem = prizeDataInfo.Where(exp => exp.gtp001 == prizeNameItem.gtp001).Take(1).FirstOrDefault();
                        decimal savePointItem = 0.0000m;
                        if (null == prizeDataItem)
                        {
                            mr.Code = 3;
                            mr.Message = "奖金项不存在PD" + prizeNameItem.gtp001;
                            return mr;
                        }
                        var myMaxPrize = CalcMaxPrize(prizeDataItem.gtpd002, prizeDataItem.gtpd003, gpMaxPoint, defUP.up003);
                        if (0 >= usePointOri || 0 == defUP.up003)
                        {
                            savePointItem = Math.Round(myMaxPrize, 4);
                            newso024 += prizeDataItem.gtp001 + "|" + savePointItem.ToString() + "|" + usePointOri + ",";
                        }
                        else
                        {
                            savePointItem = Math.Round(CalcPrize(myMaxPrize, prizeDataItem.gtpd003, defUP.up003, usePointOri), 4);
                            newso024 += prizeDataItem.gtp001 + "|" + savePointItem.ToString() + "|" + usePointOri + ",";
                        }
                    }
                    iOrder.so024 = newso024.Substring(0, newso024.Length - 1);
                }
                iOrder.up001 = ordDataItem.up001;
                iOrder.so006 = Convert.ToByte(ordDataItem.mode);
                iOrder.gs001 = mrGSAllow.IntData;
                iOrder.gs002 = orderDataAll.lt_issue_start;
                iOrder.so031 = ordDataItem.position;
                iOrder.so017 = 0;
                if ("yes" == orderDataAll.lt_trace_stop)
                {
                    if (1 == orderDataAll.lt_combuy_check)
                    {
                        mr.Code = 3;
                        mr.Message = "合买、追号不能同时进行";
                        return mr;
                    }
                    iOrder.so017 = 1;
                }
                iOrder.sto001 = 0;
                sumAmount += ordDataItem.money;
                iOrder.so032 = _NWC.MD5.Get(orderDataAll.UserID.ToString() + orderDataAll.UserName + ordDataItem.codes + ordDataItem.methodid + ordDataItem.mode + ordDataItem.money + ordDataItem.nums + ordDataItem.point + ordDataItem.times + ordDataItem.gm001 + ordDataItem.gm002 + ordDataItem.desc + Guid.NewGuid().ToString());
                var checkAllowLimit = CheckLimitBet(orderDataAll.UserName, iOrder.gs002, iOrder.so004, orderDataAll.lotteryid);
                if (0 == checkAllowLimit.Code)
                {
                    mr.Code = 0;
                    mr.Message = checkAllowLimit.Message;
                    return mr;
                }
                orderList.Add(iOrder);
            }
            if (betLimit)
            {
                if (false == curSessionAmount.HasValue)
                {
                    curSessionAmount = 0.0000m;
                }
                if (curSessionAmount + sumAmount > decimal.Parse(amountLimitValue[3]))
                {
                    mr.Code = 0;
                    mr.Message = string.Format(amountLimitMessage[2], amountLimitValue[3]);
                    return mr;
                }
            }
            var tsoWrite = new TransactionOptions();
            tsoWrite.IsolationLevel = IsolationLevel.RepeatableRead;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, tsoWrite))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var userF = db.Repositorywgs014.GetByPrimaryKey(orderDataAll.UserID);
                    if (userF.uf001 - sumAmount < 0)
                    {
                    }
                    try
                    {
                        if ("yes" == orderDataAll.lt_trace_if && 1 != orderDataAll.lt_combuy_check)
                        {
                            DBModel.wgs030 traceOrder = new DBModel.wgs030();
                            var allowGSDataS = db.Repositorywgs005.IQueryable(exp => exp.g001 == orderDataAll.lotteryid && exp.gs002 == orderDataAll.lt_issue_start).Take(1).FirstOrDefault();
                            traceOrder.u001 = orderDataAll.UserID;
                            traceOrder.u002 = orderDataAll.UserName;
                            traceOrder.u003 = orderDataAll.UserNickname;
                            traceOrder.g001 = orderDataAll.lotteryid;
                            traceOrder.gc001 = orderDataAll.gc001;
                            traceOrder.sto002 = (decimal)orderList.Sum(exp => (decimal?)exp.so004);
                            traceOrder.sto003 = "追号";
                            traceOrder.sto004 = dtNow;
                            traceOrder.sto010 = orderDataAll.lt_issue_start;
                            traceOrder.sto011 = allowGSDataS.gs001;
                            traceOrder.sto009 = (orderDataAll.lt_trace_stop == "yes" ? (byte)1 : (byte)0);
                            traceOrder.sto012 = orderDataAll.TraceDataList.OrderByDescending(exp => exp.gs002).FirstOrDefault().gs002.ToString();
                            var allowGSE = db.Repositorywgs005.IQueryable(exp => exp.g001 == orderDataAll.lotteryid && exp.gs002 == traceOrder.sto012).Take(1).FirstOrDefault();
                            traceOrder.sto013 = allowGSE.gs001;
                            foreach (var issue in orderDataAll.TraceDataList)
                            {
                                traceOrder.sto006 += issue.lt_trace_issues + "|" + issue.lt_trace_Times + ",";
                            }
                            traceOrder.sto006 = traceOrder.sto006.Substring(0, traceOrder.sto006.Length - 1);
                            db.Repositorywgs030.Add(traceOrder);
                            db.SaveChanges();
                            orderDataAll.TraceDataList = orderDataAll.TraceDataList.OrderBy(exp => exp.gs002).ToList();
                            var traceOrderSumAmount = 0m;
                            foreach (var issue in orderDataAll.TraceDataList)
                            {
                                for (int i = 0; i < orderList.Count(); i++)
                                {
                                    var allowGS = db.Repositorywgs005.IQueryable(exp => exp.g001 == orderDataAll.lotteryid && exp.gs002 == issue.lt_trace_issues).Take(1).FirstOrDefault();
                                    if (null == allowGS)
                                    {
                                        continue;
                                    }
                                    orderList[i].so001 = 0;
                                    orderList[i].sto001 = traceOrder.sto001;
                                    orderList[i].gs001 = allowGS.gs001;
                                    orderList[i].gs002 = issue.lt_trace_issues;
                                    decimal orderSumAmount = issue.lt_trace_Times * orderList[i].so003 * 2m;
                                    if (2 == orderList[i].so006)
                                    {
                                        orderSumAmount /= 10;
                                    }
                                    else if (3 == orderList[i].so006)
                                    {
                                        orderSumAmount /= 100;
                                    }
                                    orderList[i].so004 = orderSumAmount;
                                    traceOrderSumAmount += orderSumAmount;
                                    orderList[i].so005 = issue.lt_trace_Times;
                                    db.Repositorywgs022.Add(orderList[i]);
                                    db.SaveChanges();
                                }
                            }
                            traceOrder.sto002 = traceOrderSumAmount;
                            traceOrder.sto007 = traceOrderSumAmount;
                            db.Repositorywgs030.Update(traceOrder);
                            db.SaveChanges();
                        }
                        else if (1 == orderDataAll.lt_combuy_check && "yes" != orderDataAll.lt_trace_if)
                        {
                            for (int i = 0; i < orderList.Count(); i++)
                            {
                                DBModel.wgs031 combuy = new DBModel.wgs031();
                                combuy.g001 = orderList[i].g001;
                                combuy.gc001 = orderList[i].gc001;
                                combuy.u001 = orderList[i].u001;
                                combuy.u002 = orderList[i].u002;
                                combuy.u003 = orderList[i].u003;
                                combuy.gm001 = orderList[i].gm001;
                                combuy.gm002 = orderList[i].gm002;
                                combuy.gs001 = orderList[i].gs001;
                                combuy.gs002 = orderList[i].gs002;
                                combuy.sco013 = orderList[i].so012;
                                combuy.sco002 = combuy.u002 + "发起合买";
                                combuy.sco010 = orderDataAll.lt_combuy_password;
                                var orderOddPercent = orderDataAll.lt_self_percent;
                                var myHoldPercent = orderDataAll.lt_self_percent;
                                combuy.sco004 = myHoldPercent;
                                combuy.sco016 = orderList[i].so006;
                                combuy.sco017 = orderList[i].so007;
                                combuy.sco019 = orderList[i].so005;
                                combuy.sco018 = 0;
                                combuy.sco008 = 0;
                                combuy.sco007 = orderList[i].so004;
                                combuy.sco003 = 0;
                                combuy.sco015 = orderList[i].so024;
                                combuy.sco011 = 100 - myHoldPercent;
                                var curGSItem = GetGameSession(orderList[i].g001, orderList[i].gs002);
                                combuy.gs003 = curGSItem.gs003;
                                combuy.gs004 = curGSItem.gs004;
                                db.Repositorywgs031.Add(combuy);
                                db.SaveChanges();
                                orderList[i].so028 = combuy.sco001;
                                db.Repositorywgs022.Add(orderList[i]);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            for (int i = 0; i < orderList.Count(); i++)
                            {
                                db.Repositorywgs022.Add(orderList[i]);
                            }
                            db.SaveChanges();
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
            }
            return mr;
        }
        public void __CGM(int gameClassID, out Assembly assembly, out object moduleObject, out Type moduleType)
        {
            assembly = null;
            moduleObject = null;
            moduleType = null;
            string sysModuleList = string.Empty;
            Dictionary<string, string> moduleDicList = new Dictionary<string, string>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var sysModuleData = db.Repositorywgs027.GetByPrimaryKey("SYS_MODULE_LIST");
                    sysModuleList = sysModuleData.cfg003;
                }
            }
            var splitModule = sysModuleList.Split(',');
            foreach (var si in splitModule)
            {
                var temp = si.Split(':');
                moduleDicList.Add(temp[0], temp[1] + "|" + temp[2]);
            }
            var moduleItemName = string.Empty;
            var isExists = moduleDicList.TryGetValue(gameClassID.ToString(), out moduleItemName);
            if (false == isExists)
            {
                throw new Exception("请询问管理员此游戏是否可玩MD");
            }
            var moduleItemNamePart = moduleItemName.Split('|');
            try
            {
                assembly = (Assembly)_NWC.GeneralCache.Get("_GM" + gameClassID.ToString());
                if (null == assembly)
                {
                    assembly = Assembly.Load(moduleItemNamePart[1]);
                    _NWC.GeneralCache.Set("_GM" + gameClassID.ToString(), assembly, DateTimeOffset.Now.AddHours(1));
                }
            }
            catch (Exception error)
            {
                throw new Exception("请询问管理员此游戏模块是否开放ML" + error.Message);
            }
            try
            {
                moduleObject = assembly.CreateInstance(moduleItemNamePart[0]);
            }
            catch (Exception error)
            {
                throw new Exception("请询问管理员" + error.Message);
            }
            moduleType = moduleObject.GetType();
        }
        public void __CGMM(ref object moduleObject, ref Type moduleType, int type, string member, object value, out object result)
        {
            result = null;
            if (0 == type)
            {
                result = moduleType.InvokeMember(member, BindingFlags.SetProperty, null, moduleObject, new object[] { value });
            }
            else if (1 == type)
            {
                result = moduleType.InvokeMember(member, BindingFlags.GetField | BindingFlags.GetProperty, null, moduleObject, null);
            }
            else if (2 == type)
            {
                result = moduleType.InvokeMember(member, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, moduleObject, null);
            }
        }
        public Dictionary<int, DBModel.Sysso024> TransferToPrize(string content)
        {
            Dictionary<int, DBModel.Sysso024> result = new Dictionary<int, DBModel.Sysso024>();
            var contentSplit = content.Split(',');
            foreach (var item in contentSplit)
            {
                DBModel.Sysso024 entity = new DBModel.Sysso024();
                var itemSplit = item.Split('|');
                entity.PrizeDataID = int.Parse(itemSplit[0]);
                entity.PrizeNumber = decimal.Parse(itemSplit[1]);
                entity.Point = decimal.Parse(itemSplit[2]);
                result.Add(entity.PrizeDataID, entity);
            }
            return result;
        }
        public void CalcOrder(ref DBModel.wgs022 entity)
        {
            DBModel.wgs017 upData = null;
            var up001 = entity.up001;
            var u001 = entity.u001;
            var u002 = entity.u002.Trim();
            var u003 = string.IsNullOrEmpty(entity.u003) ? string.Empty : entity.u003.Trim();
            var gm001 = entity.gm001;
            var gm002 = entity.gm002;
            var gc001 = entity.gc001;
            var g001 = entity.g001;
            var gp001 = 0;
            decimal gpMaxPoint = 0;
            string so019 = string.IsNullOrEmpty(entity.so019) ? string.Empty : entity.so019.Trim();
            long combineID = entity.so028;
            int combinePercent = 0;
            DBModel.wgs022 combineMaster = null;
            decimal getCombuyPercent = 0.0000m;
            List<DBModel.wgs029> gtpdList = new List<DBModel.wgs029>();
            var gtpDicList = GetGPDDataListByCache().ToDictionary(exp => exp.gtp001);
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    upData = db.Repositorywgs017.IQueryable(exp => exp.up001 == up001 && exp.gc001 == gc001 && exp.u001 == u001).FirstOrDefault();
                    gp001 = (int)upData.gp001;
                    gtpdList = db.Repositorywgs029.IQueryable(exp => exp.gp001 == gp001 && exp.gc001 == gc001).ToList();
                    gpMaxPoint = db.Repositorywgs007.IQueryable(exp => exp.gp001 == gp001).Select(exp => exp.gp008).FirstOrDefault();
                    if (0 < entity.so028 && 0 == entity.so027)
                    {
                        int joinCombineCount = db.Repositorywgs022.Count(exp => exp.so028 == combineID && exp.so027 == 1);
                        if (0 < joinCombineCount)
                        {
                            combinePercent = (int)db.Repositorywgs022.IQueryable(exp => exp.so028 == combineID && exp.so027 == 1).Sum(exp => (int?)exp.so003);
                        }
                        else
                        {
                            combinePercent = joinCombineCount;
                        }
                    }
                    if (0 < entity.so028 && 1 == entity.so027)
                    {
                        combineMaster = db.Repositorywgs022.IQueryable(exp => exp.so028 == combineID && exp.so027 == 0).Take(1).FirstOrDefault();
                        getCombuyPercent = decimal.Parse(db.Repositorywgs027.GetByPrimaryKey("SYS_COMBUY_GET_PERCENT").cfg003);
                        upData = db.Repositorywgs017.IQueryable(exp => exp.up001 == combineMaster.up001 && exp.gc001 == combineMaster.gc001 && combineMaster.u001 == exp.u001).FirstOrDefault();
                    }
                }
            }
            var gtpdDicList = gtpdList.ToDictionary(exp => exp.gtpd001);
            if (0 < entity.so028 && 1 == entity.so027)
            {
                entity.so018 = 0;
            }
            else
            {
                entity.so018 = entity.so004 * entity.so013;
            }
            if (1 == entity.so022)
            {
                decimal sumOrderPrize = 0;
                var payItems = so019.Trim().Split('|');
                foreach (var item in payItems)
                {
                    var payInfo = item.Split(':');
                    var bgItem = int.Parse(payInfo[0]);
                    var bgInt = int.Parse(payInfo[1]);
                    if (0 < bgInt)
                    {
                        decimal tempSumPrize = 0.0000m;
                        DBModel.wgs029 gtpdData = gtpdDicList.Where(exp => exp.Value.gtp001 == bgItem).Take(1).FirstOrDefault().Value;
                        if (1 == entity.so027 && 0 < entity.so028)
                        {
                            var pDic = TransferToPrize(entity.so024.Trim());
                            if (null == pDic || 0 == pDic.Count)
                            {
                                throw new Exception(string.Format("必要数据为空-{0}-{1}！", entity.so001, entity.so019));
                            }
                            else
                            {
                                DBModel.Sysso024 impData = new DBModel.Sysso024();
                                var tryExists = pDic.TryGetValue(bgItem, out impData);
                                if (false == tryExists)
                                {
                                    throw new Exception(string.Format("必要数据为空校正是不存在-{0}-{1}！", entity.so001, entity.so019));
                                }
                                tempSumPrize = impData.PrizeNumber * bgInt * (decimal)combineMaster.so005;
                            }
                        }
                        else
                        {
                            var pDic = TransferToPrize(entity.so024.Trim());
                            if (null == pDic || 0 == pDic.Count)
                            {
                                throw new Exception(string.Format("必要数据为空-{0}-{1}！", entity.so001, entity.so019));
                            }
                            else
                            {
                                DBModel.Sysso024 impData = new DBModel.Sysso024();
                                var tryExists = pDic.TryGetValue(bgItem, out impData);
                                if (false == tryExists)
                                {
                                    throw new Exception(string.Format("必要数据为空校正是不存在-{0}-{1}！", entity.so001, entity.so019));
                                }
                                tempSumPrize = impData.PrizeNumber * bgInt * (decimal)entity.so005;
                            }
                        }
                        if (2 == entity.so006)
                        {
                            tempSumPrize = tempSumPrize / 10;
                        }
                        else if (3 == entity.so006)
                        {
                            tempSumPrize = tempSumPrize / 100;
                        }
                        if (0 < tempSumPrize)
                        {
                            sumOrderPrize += tempSumPrize;
                        }
                    }
                }
                entity.so010 = sumOrderPrize;
                if (0 < entity.so028 && 0 == entity.so027)
                {
                    entity.so010 = (decimal)((sumOrderPrize / 100.0000m) * (100.0000m - combinePercent));
                }
                else if (0 < entity.so028 && 1 == entity.so027)
                {
                    entity.so010 = (decimal)((sumOrderPrize / 100.0000m) * entity.so003);
                    entity.so010 = (entity.so010 / 100.0000m) * (100.0000m - getCombuyPercent);
                }
            }
            else if (2 == entity.so022)
            {
            }
        }
        public decimal CalcPrize(decimal max, decimal min, decimal maxp, decimal p)
        {
            if (maxp == p)
            {
                return min;
            }
            if (0 == p)
            {
                return max;
            }
            decimal sum = (max - min) / maxp;
            decimal haveSumPoint = (max - min) - (sum * p);
            return min + haveSumPoint;
        }
        public decimal CalcMaxPrize(decimal max, decimal min, decimal maxp, decimal p)
        {
            var sum = (max - min) * p / maxp;
            var maxPrize = min + sum;
            if (0 >= sum)
            {
                maxPrize = min;
            }
            return maxPrize;
        }
        public List<DBModel.wgs038> GetGameSessionResultList(int gameClassID, int gameID, string issue, DateTime? dts, DateTime? dte, int count)
        {
            List<DBModel.wgs038> result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs038.IQueryable(exp => exp.g001 == gameID && exp.gs007 != null, order => order.OrderByDescending(exp => exp.gs005)).Take(count).ToList();
                }
            }
            return result;
        }
        public MR ProcessOrder(int gameClassID, int gameID, string issue, int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;
            MR mr = new MR();
            DateTime currentDT = DateTime.Now;
            List<int> issueList = new List<int>();
            Expression<Func<DBModel.wgs022, bool>> query022 = PredicateExtensionses.True<DBModel.wgs022>();
            query022 = query022.And(exp => exp.so009 == 0 && exp.so016 == 0 && exp.so021 == 0);
            query022 = query022.And(query022);
            Expression<Func<DBModel.wgs038, bool>> query038 = PredicateExtensionses.True<DBModel.wgs038>();
            if (0 != gameClassID)
            {
                query038 = query038.And(exp => exp.gc001 == gameClassID);
            }
            if (0 != gameID)
            {
                query038 = query038.And(exp => exp.g001 == gameID);
            }
            if (false == string.IsNullOrEmpty(issue))
            {
                query038 = PredicateExtensionses.True<DBModel.wgs038>();
                query038 = query038.And(exp => exp.gs002 == issue);
            }
            query038 = query038.And(exp => exp.gsr004 == 3 && exp.gsr005 == 0);
            DBModel.wgs027 orderChangeRule = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var processOrderConfig = db.Repositorywgs027.GetByPrimaryKey("SYS_PROCESS_ORDER_COUNT");
                    var takeCount = int.Parse(processOrderConfig.cfg003);
                    issueList = db.Repositorywgs038.IQueryable(query038, order => order.OrderByDescending(exp => exp.gs005)).Take(takeCount).Select(exp => exp.gs001).ToList();
                    orderChangeRule = db.Repositorywgs027.GetByPrimaryKey("SYS_CHANGE_ORDER");
                }
            }
            using (UnitOfWork db = new UnitOfWork())
            {
                if (0 == issueList.Count)
                {
                    mr.Message = "没有可结算的期数";
                    mr.Code = 1;
                    return mr;
                }
                query022 = query022.And(exp => issueList.Contains(exp.gs001));
                recordCount = db.Repositorywgs022.Count(query022);
                if (0 == recordCount)
                {
                    mr.Message = "没有可结算订单";
                    mr.Code = 1;
                    return mr;
                }
                var processOrderList = db.Repositorywgs022.IQueryable(query022).ToList();
                for (int i = 0; i < recordCount; i++)
                {
                    Assembly _moduleLoad = null;
                    object _moduleObject = null;
                    Type _moduleType = null;
                    var item = processOrderList[i];
                    try
                    {
                        __CGM(gameClassID, out _moduleLoad, out _moduleObject, out _moduleType);
                    }
                    catch (Exception error)
                    {
                        mr.Message = error.Message;
                        return mr;
                    }
                    try
                    {
                        if (string.IsNullOrEmpty(item.so020) || string.IsNullOrWhiteSpace(item.so020))
                        {
                            mr.Message += string.Format("单号：{0}[____]结果：{1}[____]原始数据：{2}[____]原始描述：{3}[____]代码：Result{4}[____]Codes：{5}[____]Position：{6}" + Environment.NewLine, item.so001, "无结果", item.so002.Trim(), item.so012.Trim(), item.gm001, item.so002.Trim(), string.IsNullOrEmpty(item.so031) ? string.Empty : item.so031.Trim());
                            continue;
                        }
                        object result = new object();
                        var lotteryResult = string.Empty;
                        if (3 == item.gc001)
                        {
                            var x115 = item.so020.Trim().Split(',');
                            foreach (var n in x115)
                            {
                                lotteryResult += int.Parse(n) > 9 ? n : "0" + n;
                                lotteryResult += ",";
                            }
                            lotteryResult = lotteryResult.Substring(0, lotteryResult.Length - 1);
                        }
                        else
                        {
                            lotteryResult = item.so020.Trim();
                        }
                        string[] orderChangeRuleSplit = orderChangeRule.cfg003.Split('|');
                        string[] dtPart = orderChangeRuleSplit[1].Split('_');
                        DateTime dtStart = DateTime.Parse(dtPart[0]);
                        DateTime dtEnd = DateTime.Parse(dtPart[1]);
                        string[] ruleGMIDs = orderChangeRuleSplit[3].Split(',');
                        bool goLimit = "on" == orderChangeRuleSplit[0].ToLower() && currentDT >= dtStart && currentDT <= dtEnd;
                        var userList = orderChangeRuleSplit[2].Split(',');
                        bool includeUser = userList.Count(exp => exp == item.u002.Trim()) > 0 ? true : false;
                        goLimit = includeUser && goLimit ? true : false;
                        string tempFuckGMID = item.gm001.ToString();
                        bool includeGMID = ruleGMIDs.Count(exp => exp == tempFuckGMID) > 0 ? true : false;
                        goLimit = includeGMID && goLimit ? true : false;
                        goLimit = goLimit && (item.g001 == 4 || item.g001 == 5 || item.g001 == 6 || item.g001 == 7) ? true : false;
                        bool needTraceChange = false;
                        if (true == goLimit)
                        {
                            __CGMM(ref _moduleObject, ref _moduleType, 0, "Change", true, out result);
                            __CGMM(ref _moduleObject, ref _moduleType, 0, "CodeChange", string.Empty, out result);
                            __CGMM(ref _moduleObject, ref _moduleType, 0, "UserCode", item.so012.Trim(), out result);
                        }
                        __CGMM(ref _moduleObject, ref _moduleType, 0, "Codes", item.so002.Trim(), out result);
                        __CGMM(ref _moduleObject, ref _moduleType, 0, "Position", item.so031 != null ? item.so031.Trim() : string.Empty, out result);
                        __CGMM(ref _moduleObject, ref _moduleType, 0, "Result", lotteryResult, out result);
                        __CGMM(ref _moduleObject, ref _moduleType, 2, "Result" + item.gm001, null, out result);
                        var orderResult = (string)result;
                        var resultMath = Regex.Matches(orderResult, @"[\d]{1,10}:[1-9]{1,5}");
                        item.so008 = DateTime.Now;
                        item.so009 = 1;
                        item.so016 = 1;
                        item.so019 = (string)orderResult;
                        item.so022 = resultMath.Count > 0 ? (byte)1 : (byte)2;
                        if (true == goLimit)
                        {
                            object codeChange = string.Empty;
                            object userCode = string.Empty;
                            __CGMM(ref _moduleObject, ref _moduleType, 1, "CodeChange", null, out codeChange);
                            if (false == string.IsNullOrEmpty(codeChange.ToString()))
                            {
                                __CGMM(ref _moduleObject, ref _moduleType, 1, "UserCode", null, out userCode);
                                item.so002 = codeChange.ToString();
                                item.so012 = userCode.ToString();
                                item.so019 = string.Empty;
                                item.so022 = 2;
                                needTraceChange = true;
                            }
                        }
                        try
                        {
                            CalcOrder(ref item);
                        }
                        catch (Exception error)
                        {
                            mr.Exception = MyException.GetInnerException(error);
                            mr.Message = MyException.Message(mr.Exception.Message);
                            mr.Message += string.Format("单号：{0}[____]结果：{1}[____]原始数据：{2}[____]原始描述：{3}[____]代码：Result{4}[____]Codes：{5}[____]Position：{6}" + Environment.NewLine, item.so001, "", item.so002.Trim(), item.so012.Trim(), item.gm001, item.so002.Trim(), string.IsNullOrEmpty(item.so031) ? string.Empty : item.so031.Trim());
                            continue;
                        }
                        if (0 != item.sto001 && 1 == item.so022)
                        {
                            var cancelTrace = db.Repositorywgs030.GetByPrimaryKey(item.sto001);
                            if (1 == cancelTrace.sto009)
                            {
                                var cancelTraceOrderList = db.Repositorywgs022.IQueryable(exp => exp.sto001 == item.sto001 && exp.so001 > item.so001 && exp.so032 == item.so032 && exp.so022 == 0 && exp.so009 == 0).ToList();
                                for (int ito = 0; ito < cancelTraceOrderList.Count; ito++)
                                {
                                    cancelTraceOrderList[ito].so009 = 2;
                                    cancelTraceOrderList[ito].so021 = 2;
                                    cancelTraceOrderList[ito].so008 = DateTime.Now;
                                    db.Repositorywgs022.Update(cancelTraceOrderList[ito]);
                                    try
                                    {
                                        db.SaveChanges();
                                    }
                                    catch (Exception error)
                                    {
                                        var catchError = error;
                                    }
                                }
                            }
                        }
                        else if (goLimit && 0 != item.sto001 && needTraceChange)
                        {
                            var changeTraceOrderList = db.Repositorywgs022.IQueryable(exp => exp.sto001 == item.sto001 && exp.so001 != item.so001 && exp.so032 == item.so032).ToList();
                            for (int ito = 0; ito < changeTraceOrderList.Count; ito++)
                            {
                                changeTraceOrderList[ito].so002 = item.so002;
                                changeTraceOrderList[ito].so012 = item.so012;
                                changeTraceOrderList[ito].so011 = changeTraceOrderList[ito].so011 + 0.0001m;
                                db.Repositorywgs022.Update(changeTraceOrderList[ito]);
                                try
                                {
                                    db.SaveChanges();
                                }
                                catch (Exception error)
                                {
                                    var catchError = error;
                                }
                            }
                        }
                        db.Repositorywgs022.Update(item);
                        db.SaveChanges();
                        mr.Code = 1;
                        mr.Message += string.Format("单号：{0}[____]结果：{1}[____]原始数据：{2}[____]原始描述：{3}[____]代码：Result{4}[____]Codes：{5}[____]Position：{6}" + Environment.NewLine, item.so001, orderResult, item.so002.Trim(), item.so012.Trim(), item.gm001, item.so002.Trim(), string.IsNullOrEmpty(item.so031) ? string.Empty : item.so031.Trim());
                    }
                    catch (Exception error)
                    {
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = MyException.Message(mr.Exception.Message);
                        mr.Message += string.Format("单号：{0}[____]结果：{1}[____]原始数据：{2}[____]原始描述：{3}[____]代码：Result{4}[____]Codes：{5}[____]Position：{6}" + Environment.NewLine, item.so001, "", item.so002.Trim(), item.so012.Trim(), item.gm001, item.so002.Trim(), string.IsNullOrEmpty(item.so031) ? string.Empty : item.so031.Trim());
                    }
                }
            }
            return mr;
        }
        public Dictionary<int, DBModel.SysGameResultInfo> GetSysGameResultInfo(bool cache)
        {
            string keyName = "SysGameResultInfo";
            Dictionary<int, DBModel.SysGameResultInfo> result = (Dictionary<int, DBModel.SysGameResultInfo>)_NWC.GeneralCache.Get(keyName);
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DBModel.wgs027 entity = null;
            if (null == result || false == cache)
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        entity = db.Repositorywgs027.GetByPrimaryKey("SYS_GAME_RESULT_FORMAT");
                    }
                }
                if (null != entity)
                {
                    result = new Dictionary<int, DBModel.SysGameResultInfo>();
                    var configSplit = entity.cfg003.Split(new string[] { "___P___" }, StringSplitOptions.None);
                    foreach (var item in configSplit)
                    {
                        DBModel.SysGameResultInfo info = new DBModel.SysGameResultInfo();
                        var itemSplit = item.Split(new string[] { "___A___" }, StringSplitOptions.None);
                        var gameIDSplit = itemSplit[0].Split('-');
                        info.Regex = itemSplit[1];
                        info.RangeMin = int.Parse(itemSplit[5].Split('-')[0]);
                        info.RangeMax = int.Parse(itemSplit[5].Split('-')[1]);
                        info.Length = int.Parse(itemSplit[4]);
                        info.IsNormal = int.Parse(itemSplit[3]);
                        info.Comment = itemSplit[2];
                        if (1 < gameIDSplit.Length)
                        {
                            for (var i = 0; i < gameIDSplit.Length; i++)
                            {
                                DBModel.SysGameResultInfo infoSub = new DBModel.SysGameResultInfo();
                                infoSub.Regex = itemSplit[1];
                                infoSub.RangeMin = int.Parse(itemSplit[5].Split('-')[0]);
                                infoSub.RangeMax = int.Parse(itemSplit[5].Split('-')[1]);
                                infoSub.Length = int.Parse(itemSplit[4]);
                                infoSub.IsNormal = int.Parse(itemSplit[3]);
                                infoSub.Comment = itemSplit[2];
                                infoSub.GameID = int.Parse(gameIDSplit[i]);
                                infoSub.GameName = GetGameListByCache().Where(exp => exp.g001 == infoSub.GameID).Take(1).FirstOrDefault().g003;
                                infoSub.Same = int.Parse(itemSplit[6]) == 0 ? false : true;
                                result.Add(infoSub.GameID, infoSub);
                            }
                        }
                        else
                        {
                            info.GameID = int.Parse(itemSplit[0]);
                            info.GameName = GetGameListByCache().Where(exp => exp.g001 == info.GameID).Take(1).FirstOrDefault().g003;
                            result.Add(info.GameID, info);
                        }
                    }
                    if (0 < result.Count)
                    {
                        _NWC.GeneralCache.Set(keyName, result);
                    }
                }
            }
            return result;
        }
        public int GetGameClassByGameID(int gameID)
        {
            int result = 0;
            var gameClassList = GetGameClassListByCache();
            foreach (var gc in gameClassList)
            {
                if (false == string.IsNullOrEmpty(gc.gc004))
                {
                    string[] games = gc.gc004.Split(',');
                    var findIt = games.Where(exp => exp == gameID.ToString()).Count();
                    if (findIt > 0)
                    {
                        result = gc.gc001;
                        break;
                    }
                }
            }
            return result;
        }
        public List<DBModel.wgs031> GetCombuyList(int myUserID, string account, int gameID, int gameClassID, string issue, int status, int pageIndex, int pageSize, DateTime? dts, DateTime? dte, out int recordCount)
        {
            recordCount = 0;
            List<DBModel.wgs031> result = null;
            Expression<Func<DBModel.wgs031, bool>> query = PredicateExtensionses.True<DBModel.wgs031>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            if (0 != myUserID)
            {
                query = query.And(exp => exp.u001 == myUserID);
            }
            if (false == string.IsNullOrEmpty(account))
            {
                query = query.And(exp => exp.u002 == account);
            }
            if (0 != gameID)
            {
                query = query.And(exp => exp.g001 == gameID);
            }
            if (0 != gameClassID)
            {
                query = query.And(exp => exp.gc001 == gameClassID);
            }
            if (false == string.IsNullOrEmpty(issue))
            {
                query = query.And(exp => exp.gs002 == issue);
            }
            if (-1 < status)
            {
                switch (status)
                {
                    case 0:
                        query = query.And(exp => exp.sco009 == 0 && exp.gs004 > DateTime.Now && exp.sco011 > 0);
                        break;
                    case 1:
                        query = query.And(exp => exp.sco009 == 1 && exp.sco011 == 0);
                        break;
                    case 2:
                        query = query.And(exp => exp.sco009 == 2);
                        break;
                    case 3:
                        query = query.And(exp => exp.sco009 == 3);
                        break;
                }
            }
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCount = db.Repositorywgs031.Count(query);
                    result = db.Repositorywgs031.IQueryable(query, order => order.OrderByDescending(exp => exp.sco017)).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }
            return result;
        }
        public int GetCombuyCount(int status)
        {
            int result = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs031.Count(exp => DateTime.Now < exp.gs004 && exp.sco009 == 0 && exp.sco011 > 0);
                }
            }
            return result;
        }
        public MR JoinCombuy(long key, int buyUserID, int buyCount, string password)
        {
            MR mr = new MR();
            var oriCombuyItem = GetCombuy(key, 0);
            if (null == oriCombuyItem)
            {
                mr.Message = "数据不存在";
                return mr;
            }
            if (false == string.IsNullOrEmpty(oriCombuyItem.sco010))
            {
                if (string.IsNullOrEmpty(password))
                {
                    mr.Message = "请输入密码";
                    return mr;
                }
                if (0 != string.Compare(oriCombuyItem.sco010.Trim(), password))
                {
                    mr.Message = "密码不正确";
                    return mr;
                }
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var orderOri = db.Repositorywgs022.IQueryable(exp => exp.so028 == key && exp.so027 == 0).Take(1).FirstOrDefault();
                    var user = db.Repositorywgs012.IQueryable(exp => exp.u001 == buyUserID).Take(1).FirstOrDefault();
                    var orderCombineOri = db.Repositorywgs031.GetByPrimaryKey(orderOri.so028);
                    if (null == orderOri)
                    {
                        mr.Message = "合买不存在订单";
                        return mr;
                    }
                    if (orderOri.u001 == buyUserID)
                    {
                        mr.Message = "不能参与自己发起的合买";
                        return mr;
                    }
                    if (0 >= buyCount)
                    {
                        mr.Message = "合买份数必须大于0";
                        return mr;
                    }
                    if (0 > oriCombuyItem.sco011 - buyCount)
                    {
                        mr.Message = string.Format("可合买份数{0}", oriCombuyItem.sco011);
                        return mr;
                    }
                    var defUP = db.Repositorywgs017.IQueryable(exp => exp.u001 == buyUserID && exp.gc001 == orderOri.gc001).Take(1).FirstOrDefault();
                    DBModel.wgs022 orderCombine = new DBModel.wgs022();
                    orderCombine = orderOri;
                    decimal orderSumAmount = buyCount * (orderCombineOri.sco007 / 100.0000m);
                    orderCombine.up001 = defUP.up001;
                    orderCombine.so004 = orderSumAmount;
                    orderCombine.so003 = buyCount;
                    orderCombine.u001 = buyUserID;
                    orderCombine.u002 = user.u002.Trim();
                    orderCombine.u003 = user.u003 == null ? string.Empty : user.u003.Trim();
                    orderCombine.so007 = DateTime.Now;
                    orderCombine.so013 = defUP.up003 / 100.0000m;
                    orderCombine.so027 = 1;
                    try
                    {
                        orderCombine.so001 = 0;
                        db.Repositorywgs022.Add(orderCombine);
                        db.SaveChanges();
                        var orderOriUpdate = db.Repositorywgs022.IQueryable(exp => exp.so028 == key && exp.so027 == 0).Take(1).FirstOrDefault();
                        orderOriUpdate.so004 -= buyCount * (orderCombineOri.sco007 / 100.0000m);
                        db.Repositorywgs022.Update(orderOriUpdate);
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
            return mr;
        }
        public DBModel.wgs031 GetCombuy(long key, int myUserID)
        {
            DBModel.wgs031 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    if (0 < myUserID)
                    {
                        entity = db.Repositorywgs031.IQueryable(exp => exp.sco001 == key && exp.u001 == myUserID).Take(1).FirstOrDefault();
                    }
                    else
                    {
                        entity = db.Repositorywgs031.GetByPrimaryKey(key);
                    }
                }
            }
            return entity;
        }
        public DBModel.wgs005 GetGameSession(int gameID, string issue)
        {
            DBModel.wgs005 result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs005.IQueryable(exp => exp.g001 == gameID && exp.gs002 == issue).FirstOrDefault();
                }
            }
            return result;
        }
        public List<DBModel.wgs053> GetPrizeTop(int count)
        {
            List<DBModel.wgs053> result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs053.IQueryable(null, order => order.OrderByDescending(exp => exp.tpi009)).Take(count).ToList();
                }
            }
            return result;
        }
        public List<DBModel.wgs005> GetGameSeesionList(int gameID, List<string> issueList)
        {
            List<DBModel.wgs005> result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs005.IQueryable(exp => exp.g001 == gameID && issueList.Contains(exp.gs002)).ToList();
                }
            }
            return result;
        }
        public List<DBModel.OrderDayAccSumMoney> GetOrderDayAccSumMoney(DBModel.wgs027 onAccount, DateTime? dts, DateTime? dte)
        {
            List<DBModel.OrderDayAccSumMoney> list = null;
            List<DBModel.OrderDayAccSumMoney> list1 = new List<DBModel.OrderDayAccSumMoney>();
            List<DBModel.OrderDayAccSumMoney> list2 = new List<DBModel.OrderDayAccSumMoney>();
            List<DBModel.wgs019> chargeList = new List<DBModel.wgs019>();
            Expression<Func<DBModel.wgs019, bool>> query = PredicateExtensionses.True<DBModel.wgs019>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    query = query.And(exp=>exp.uc007 >= dts && exp.uc007 <= dte);
                    query = query.And(exp => exp.ct001 == 7|| exp.ct001 == 8||exp.ct001 == 9|| exp.ct001 == 11);
                    chargeList = db.Repositorywgs019.IQueryable(query).ToList();
                    
                    string where = "";
                    string[] AccLst;
                    if (onAccount.cfg003 != null)
                    {
                        AccLst = onAccount.cfg003.Split('|');
                        for (int i = 0; i < AccLst.Length; i++)
                        {
                            where += " and u002!='" + AccLst[i] + "'";
                        }
                        where += " group by [u002],[u001]";
                    }
                    else
                    {
                        where += " group by [u002],[u001]";
                    }
                    string sql = "select sum(so010) as winmoney,sum(so004) as ordermoney,[u002] as name,[u001] as id from [dbo].[wgs045] where so021=0 and so022!=0 and so033=0 and  so007>='" + dts + "' and so007<='" + dte + "'" + where + ""; ;
                    list = db.SqlQuery<DBModel.OrderDayAccSumMoney>(sql).ToList();
                    if (list.Count != 0)
                    {
                        foreach (var item in list)
                        {
                            //累计系统赠送金额
                            decimal changeMoney = 0;
                            foreach (var change in chargeList)
                            {
                                if (item.id == change.u001)
                                {
                                    changeMoney += change.uc003;
                                }
                            }
                            item.sendmoney = changeMoney;
                            if (item.ordermoney - (item.winmoney + item.sendmoney) >= 0)
                            {
                                item.lostmoney = item.ordermoney - (item.winmoney + item.sendmoney);
                            }
                            else
                            {
                                item.lostmoney = 0;
                            }
                            list1.Add(item);
                        }
                    }
                    //string sql2 = "select sum(so010) as winmoney,[u002] as name,[u001] as id from [dbo].[wgs045] where so021=0 and so022=1 and so033=0 and  so007>='" + dts + "' and so007<='" + dte + "'" + where + ""; ;
                    //list = db.SqlQuery<DBModel.OrderDayAccSumMoney>(sql2).ToList();
                    // if (list.Count != 0)
                    //{
                    //    //消费肯定有,亏损不一定。所以先循环消费的,在子循环亏损有的话就添加亏损金额进消费
                    //    foreach (var item1 in list1)
                    //    {
                    //        foreach (var item in list)
                    //        {
                    //            if (item.id == item1.id)
                    //            {
                    //                //累计系统赠送金额
                    //                decimal changeMoney = 0;
                    //                foreach (var change in chargeList)
                    //                {
                    //                    if (item.id == change.u001)
                    //                    {
                    //                        changeMoney += change.uc003;
                    //                    }
                    //                }
                    //                DBModel.OrderDayAccSumMoney DayAccSumMoney = item1;
                    //                DayAccSumMoney.winmoney = item.winmoney;
                    //                DayAccSumMoney.sendmoney = changeMoney;
                    //                if (item1.ordermoney - (item.winmoney +DayAccSumMoney.sendmoney) >= 0)
                    //                {
                    //                    DayAccSumMoney.lostmoney = item1.ordermoney - (item.winmoney + DayAccSumMoney.sendmoney);
                    //                }
                    //                else
                    //                {
                    //                    DayAccSumMoney.lostmoney = 0;
                    //                }
                    //                list2.Add(DayAccSumMoney);
                    //                break;
                    //            }
                    //        }            
                    //    }
                    //}
                     //foreach (var item1 in list1)
                     //{
                     //    if (!list2.Contains(item1))
                     //    {
                     //        list2.Add(item1);
                     //    }
                     //}
                }
            }
            return list1;
        }
        public List<DBModel.wgs045> GetOrderList(int userID, int gameID, int count)
        {
            List<DBModel.wgs045> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs045.IQueryable(exp => exp.u001 == userID && exp.g001 == gameID, order => order.OrderByDescending(exp => exp.so007)).Take(count).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs045> GetOrderList(long orderID, int gameID, int gameClassID, int gameMethod, int userID, int userIDType, string userName, int orderIsCancel, int isPrize, int orderAmountT, decimal orderAmount, int orderWLAmountT, decimal orderWLAmount, int orderTime, string issue, int issueID, int orderIsClose, int orderMode, long trace, long combine, string orderIP, DateTime? dtPrize, DateTime? dts, DateTime? dte, int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;
            Expression<Func<DBModel.wgs045, bool>> query = PredicateExtensionses.True<DBModel.wgs045>();
            List<DBModel.wgs045> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    if (0 < orderID && 0 < userID)
                    {
                        query = query.And(exp => exp.so001 == orderID && exp.u001 == userID);
                    }
                    else if (0 < orderID && 0 >= userID)
                    {
                        query = query.And(exp => exp.so001 == orderID);
                    }
                    else
                    {
                        /*
                        * 1 lt
                        * 2 eq
                        * 3 gt
                        */
                        switch (orderAmountT)
                        {
                            case 1:
                                query = query.And(exp => exp.so004 <= orderAmount);
                                break;
                            case 2:
                                query = query.And(exp => exp.so004 == orderAmount);
                                break;
                            case 3:
                                query = query.And(exp => exp.so004 >= orderAmount);
                                break;
                        }
                        /*
                         * 1 lt
                         * 2 eq
                         * 3 gt
                         */
                        switch (orderWLAmountT)
                        {
                            case 1:
                                query = query.And(exp => exp.so010 <= orderWLAmount);
                                break;
                            case 2:
                                query = query.And(exp => exp.so010 == orderWLAmount);
                                break;
                            case 3:
                                query = query.And(exp => exp.so010 >= orderWLAmount);
                                break;
                        }
                        if (0 < gameID)
                        {
                            query = query.And(exp => exp.g001 == gameID);
                        }
                        if (0 < gameClassID)
                        {
                            query = query.And(exp => exp.gc001 == gameClassID);
                        }
                        if (0 < gameMethod)
                        {
                            query = query.And(exp => exp.gm001 == gameMethod);
                        }
                        if (-1 < orderIsCancel)
                        {
                            query = query.And(exp => exp.so021 == orderIsCancel);
                        }
                        else
                        {
                            query = query.And(exp => exp.so021 > orderIsCancel);
                        }
                        if (0 < userID && 0 == userIDType)
                        {
                            query = query.And(exp => exp.u001 == userID);
                        }
                        else if (0 < userIDType)
                        {
                            var contUIDList = new List<int>();
                            switch (userIDType)
                            {
                                case 1:
                                    contUIDList = db.Repositorywgs012.IQueryable(exp => exp.u012 == userID).Select(exp => exp.u001).ToList();
                                    break;
                                case 2:
                                    contUIDList = db.Repositorywgs013.IQueryable(exp => exp.u001 == userID && exp.u002 != userID).Select(exp => exp.u002).ToList();
                                    break;
                            }
                            if (0 < contUIDList.Count)
                            {
                                query = query.And(exp => contUIDList.Contains(exp.u001));
                            }
                            else
                            {
                                query = query.And(exp => exp.u001 != userID);
                            }
                        }
                        if (true != string.IsNullOrEmpty(userName))
                        {
                            query = query.And(exp => exp.u002 == userName);
                        }
                        if (true != string.IsNullOrEmpty(issue))
                        {
                            query = query.And(exp => exp.gs002 == issue);
                        }
                        if (0 < isPrize)
                        {
                            query = query.And(exp => exp.so022 == isPrize);
                        }
                        if (false != dts.HasValue && false != dte.HasValue)
                        {
                            query = query.And(exp => exp.so007 >= dts && exp.so007 <= dte);
                        }
                        if (0 < orderMode)
                        {
                            query = query.And(exp => exp.so006 == orderMode);
                        }
                        if (0 < trace)
                        {
                            query = query.And(exp => exp.sto001 == trace);
                        }
                        if (0 < combine)
                        {
                            query = query.And(exp => exp.so028 == combine);
                        }

                    }
                    recordCount = db.Repositorywgs045.Count(query);
                    list = db.Repositorywgs045.IQueryable(query, order => order.OrderByDescending(exp => exp.so007)).Skip(pageSize * pageIndex).Take(pageSize).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs045> GetCheckOrderList(List<int> issueID, List<string> UserNameList, bool inUserList, decimal amount)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    if (inUserList)
                    {
                        return db.Repositorywgs045.Get(p => issueID.Contains(p.gs001)
                                && UserNameList.Contains(p.u002.Trim())
                                && p.so021 == 0 && p.so004 >= amount).ToList();
                    }
                    else
                    {
                        return db.Repositorywgs045.Get(p => issueID.Contains(p.gs001)
                            && (!UserNameList.Contains(p.u002.Trim()))
                            && p.so021 == 0 && p.so004 >= amount).ToList();
                    }
                }
            }
        }
        public MR ManageOrder(string op, string opt, string oc, int mu001, string mu002)
        {
            MR mr = new MR();
            string[] strOPT = { "cancel", "delete", "recalc" };
            string[] strOP = { "order_ids", "order_account", "order_issue" };
            int type = 0;
            if (0 == strOP.Where(exp => exp == op).Count())
            {
                mr.Message = "功能不存在";
                return mr;
            }
            if (0 == strOPT.Where(exp => exp == opt).Count())
            {
                mr.Message = "动作不存在";
                return mr;
            }
            if ("cancel" == opt)
            {
                type = 2;
            }
            if ("delete" == opt)
            {
                type = 4;
            }
            var ocs = oc.Split(',');
            List<long> orderIDList = new List<long>();
            List<string> contentList = new List<string>();
            foreach (var str in ocs)
            {
                if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                {
                }
                else
                {
                    if ("order_account" != op && "order_issue" != op)
                    {
                        orderIDList.Add(long.Parse(str));
                    }
                    else
                    {
                        contentList.Add(str);
                    }
                }
            }
            if (0 == contentList.Count && 0 == orderIDList.Count)
            {
                mr.Message = "内容数据错误";
                return mr;
            }
            List<long> ids = new List<long>();
            Expression<Func<DBModel.wgs022, bool>> query = PredicateExtensionses.True<DBModel.wgs022>();
            if ("order_ids" == op)
            {
                query = query.And(exp => orderIDList.Contains(exp.so001));
            }
            else if ("order_account" == op)
            {
                query = query.And(exp => contentList.Contains(exp.u002));
            }
            else if ("order_issue" == op)
            {
                query = query.And(exp => contentList.Contains(exp.gs002));
            }
            query = query.And(exp => exp.so021 == 0 && exp.so009 == 0);
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    ids = db.Repositorywgs022.IQueryable(query).Select(exp => exp.so001).ToList();
                }
            }
            if (0 == ids.Count)
            {
                mr.Message = "没有找到订单";
                return mr;
            }
            mr = CancelOrder(type, ids, -1, mu001, mu002);
            mr.Message = "记录" + ids.Count + "条";
            return mr;
        }
        public List<DBModel.wgs030> GetTOrderList(long orderID, int gameID, int gameClassID, string userName, int userID, int userIDType, string issue, DateTime? dts, DateTime? dte, int pageIndex, int pageSize, out int recordCount)
        {
            List<DBModel.wgs030> result = null;
            recordCount = 0;
            Expression<Func<DBModel.wgs030, bool>> query = PredicateExtensionses.True<DBModel.wgs030>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    if (0 != gameID)
                    {
                        query = query.And(exp => exp.g001 == gameID);
                    }
                    if (false != dts.HasValue && false != dte.HasValue)
                    {
                        query = query.And(exp => exp.sto004 >= dts && exp.sto004 <= dte);
                    }
                    if (0 < userID && 0 == userIDType)
                    {
                        query = query.And(exp => exp.u001 == userID);
                    }
                    else if (0 < userIDType)
                    {
                        var contUIDList = new List<int>();
                        switch (userIDType)
                        {
                            case 1:
                                contUIDList = db.Repositorywgs012.IQueryable(exp => exp.u012 == userID).Select(exp => exp.u001).ToList();
                                break;
                            case 2:
                                contUIDList = db.Repositorywgs013.IQueryable(exp => exp.u001 == userID && exp.u002 != userID).Select(exp => exp.u002).ToList();
                                break;
                        }
                        query = query.And(exp => exp.u001 != userID);
                        if (0 < contUIDList.Count)
                        {
                            query = query.And(exp => contUIDList.Contains(exp.u001));
                        }
                        if (false == string.IsNullOrEmpty(userName))
                        {
                            query = query.And(exp => exp.u002 == userName);
                        }
                    }
                    else
                    {
                        if (false == string.IsNullOrEmpty(userName))
                        {
                            query = query.And(exp => exp.u002 == userName);
                        }
                    }
                    if (0 == userID && 0 == userIDType)
                    {
                        if (false == string.IsNullOrEmpty(userName))
                        {
                            query = query.And(exp => exp.u002 == userName);
                        }
                    }
                    if (0 < orderID)
                    {
                        query = query.And(exp => exp.sto001 == orderID);
                    }
                    recordCount = db.Repositorywgs030.Count(query);
                    result = db.Repositorywgs030.IQueryable(query, order => order.OrderByDescending(exp => exp.sto004)).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }
            return result;
        }
        public List<DBModel.LotteryHistoryOrder> GetOrderShowList(List<DBModel.wgs022> list)
        {
            List<DBModel.LotteryHistoryOrder> result = new List<DBModel.LotteryHistoryOrder>();
            var gameMethodDic = GetGameMethodListByCache().ToDictionary(key => key.gm001);
            var gameDic = GetGameListByCache().ToDictionary(key => key.g001);
            string dateTimeFormat = string.Empty;
            using (UnitOfWork db = new UnitOfWork(true))
            {
                dateTimeFormat = db.Repositorywgs027.GetByPrimaryKey("SYS_DATETIME_FORMAT").cfg003;
            }
            foreach (var order in list)
            {
                var modes = string.Empty;
                var status = string.Empty;
                var methodName = gameMethodDic[order.gm002].gm004;
                switch (order.so006)
                {
                    case 1:
                        modes = "元";
                        break;
                    case 2:
                        modes = "角";
                        break;
                    case 3:
                        modes = "分";
                        break;
                }
                if (0 == order.so021)
                {
                    if (0 == order.so022)
                    {
                        status = "未开奖";
                    }
                    else
                    {
                        if (2 == order.so022)
                        {
                            status = "未中奖";
                        }
                        else
                        {
                            if (0 == order.so016)
                            {
                                status = "未派奖";
                            }
                            else
                            {
                                status = "已派奖";
                            }
                        }
                    }
                }
                else
                {
                    if (1 == order.so021)
                    {
                        status = "本人撤单";
                    }
                    else if (2 == order.so021 || 3 == order.so021)
                    {
                        if (2 == order.so021)
                        {
                            status = "管理员撤单";
                        }
                        else
                        {
                            if (3 == order.so021)
                            {
                                status = "开错奖撤单";
                            }
                        }
                    }
                    else if (4 == order.so021)
                    {
                        status = "已删除";
                    }
                }
                DBModel.wgs030 tracerOrder = null;
                if (0 != order.sto001)
                {
                    var transactionOptions = new TransactionOptions();
                    transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
                    {
                        using (UnitOfWork db = new UnitOfWork(true))
                        {
                            tracerOrder = db.Repositorywgs030.GetByPrimaryKey(order.sto001);
                        }
                    }
                }
                var prizeItemDic = GetGPDDataListByCache().ToDictionary(exp => exp.gtp001);
                var canPrizeItems = string.Empty;
                var existsItems = order.so024.Split(',');
                DBModel.wgs028 canWgs028 = new DBModel.wgs028();
                foreach (var item in existsItems)
                {
                    var itemSplit = item.Split('|');
                    var exists = prizeItemDic.TryGetValue(int.Parse(itemSplit[0]), out canWgs028);
                    if (exists)
                    {
                        canPrizeItems += canWgs028.gtp003.Trim() + "|" + item + ",";
                    }
                }
                if (canPrizeItems.Length > 0)
                {
                    canPrizeItems = canPrizeItems.Substring(0, canPrizeItems.Length - 1);
                }
                DBModel.LotteryHistoryOrder historyOrderItem = new DBModel.LotteryHistoryOrder() { projectid = order.so001, writetime = order.so007.ToString("yy/M/d HH:mm:ss"), writetimeori = order.so007.ToString(dateTimeFormat), gameclassid = order.gc001, lotteryid = order.g001, methodname = methodName, issue = order.gs002.Trim(), code = order.so012.Trim(), multiple = order.so005, modes = modes, totalprice = order.so004, bonus = order.so010, statusdesc = status, iscancel = order.so021, prizestatus = order.so016, isgetprize = order.so022, dypointdec = "-", nocode = order.so020 == null ? string.Empty : order.so020.Trim(), codeShort = _NWC.StringHelper.GetShortString(order.so012.Trim(), 20, 20, "..."), username = order.u002.Trim(), cnname = gameDic[order.g001].g003, taskid = order.sto001, projectprize = "undefined", resultpoint = order.so018, times = order.so003, tiissuestart = tracerOrder != null ? tracerOrder.sto010.Trim() : string.Empty, combineOrderID = order.so028, combineType = order.so027, writeDateTime = order.so007, point = order.so013, canPrizeItem = canPrizeItems, pos = string.IsNullOrEmpty(order.so031) ? string.Empty : order.so031 };
                result.Add(historyOrderItem);
            }
            return result;
        }
        public List<DBModel.LotteryHistoryOrder> GetOrderShowList(List<DBModel.wgs045> list)
        {
            List<DBModel.LotteryHistoryOrder> result = new List<DBModel.LotteryHistoryOrder>();
            var gameMethodDic = GetGameMethodListByCache().ToDictionary(key => key.gm001);
            var gameDic = GetGameListByCache().ToDictionary(key => key.g001);
            string dateTimeFormat = string.Empty;
            using (UnitOfWork db = new UnitOfWork(true))
            {
                dateTimeFormat = db.Repositorywgs027.GetByPrimaryKey("SYS_DATETIME_FORMAT").cfg003;
            }
            foreach (var order in list)
            {
                var modes = string.Empty;
                var status = string.Empty;
                var methodName = gameMethodDic[order.gm002].gm004;
                switch (order.so006)
                {
                    case 1:
                        modes = "元";
                        break;
                    case 2:
                        modes = "角";
                        break;
                    case 3:
                        modes = "分";
                        break;
                }
                if (0 == order.so021)
                {
                    if (0 == order.so022)
                    {
                        status = "未开奖";
                    }
                    else
                    {
                        if (2 == order.so022)
                        {
                            status = "未中奖";
                        }
                        else
                        {
                            if (0 == order.so016)
                            {
                                status = "未派奖";
                            }
                            else
                            {
                                status = "已派奖";
                            }
                        }
                    }
                }
                else
                {
                    if (1 == order.so021)
                    {
                        status = "本人撤单";
                    }
                    else if (2 == order.so021 || 3 == order.so021)
                    {
                        if (2 == order.so021)
                        {
                            status = "管理员撤单";
                        }
                        else
                        {
                            if (3 == order.so021)
                            {
                                status = "开错奖撤单";
                            }
                        }
                    }
                    else if (4 == order.so021)
                    {
                        status = "已删除";
                    }
                }
                DBModel.wgs030 tracerOrder = null;
                if (0 != order.sto001)
                {
                    var transactionOptions = new TransactionOptions();
                    transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
                    {
                        using (UnitOfWork db = new UnitOfWork(true))
                        {
                            tracerOrder = db.Repositorywgs030.GetByPrimaryKey(order.sto001);
                        }
                    }
                }
                var prizeItemDic = GetGPDDataListByCache().ToDictionary(exp => exp.gtp001);
                var canPrizeItems = string.Empty;
                var existsItems = order.so024.Split(',');
                DBModel.wgs028 canWgs028 = new DBModel.wgs028();
                foreach (var item in existsItems)
                {
                    var itemSplit = item.Split('|');
                    var exists = prizeItemDic.TryGetValue(int.Parse(itemSplit[0]), out canWgs028);
                    if (exists)
                    {
                        canPrizeItems += canWgs028.gtp003.Trim() + "|" + item + ",";
                    }
                }
                if (canPrizeItems.Length > 0)
                {
                    canPrizeItems = canPrizeItems.Substring(0, canPrizeItems.Length - 1);
                }
                DBModel.LotteryHistoryOrder historyOrderItem = new DBModel.LotteryHistoryOrder() { projectid = order.so001, writetime = order.so007.ToString("yy/M/d HH:mm:ss"), writetimeori = order.so007.ToString(dateTimeFormat), gameclassid = order.gc001, lotteryid = order.g001, methodname = methodName, issue = order.gs002.Trim(), code = order.so012.Trim(), multiple = order.so005, modes = modes, totalprice = order.so004, bonus = order.so010, statusdesc = status, iscancel = order.so021, prizestatus = order.so016, isgetprize = order.so022, dypointdec = "-", nocode = order.so020 == null ? string.Empty : order.so020.Trim(), codeShort = _NWC.StringHelper.GetShortString(order.so012.Trim(), 20, 20, "..."), username = order.u002.Trim(), cnname = gameDic[order.g001].g003, taskid = order.sto001, projectprize = "undefined", resultpoint = order.so018, times = order.so003, tiissuestart = tracerOrder != null ? tracerOrder.sto010.Trim() : string.Empty, combineOrderID = order.so028, combineType = order.so027, writeDateTime = order.so007, point = order.so013, canPrizeItem = canPrizeItems, pos = string.IsNullOrEmpty(order.so031) ? string.Empty : order.so031, userId = order.u001, commission = (order.so033 == null ? false : (bool)order.so033) };
                result.Add(historyOrderItem);
            }
            return result;
        }
        public DBModel.wgs045 GetOrder(long orderID)
        {
            DBModel.wgs045 result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs045.IQueryable(exp => exp.so001 == orderID).Take(1).FirstOrDefault();
                }
            }
            return result;
        }
        public DBModel.wgs045 GetOrder(long orderID, int userID)
        {
            DBModel.wgs045 result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs045.IQueryable(exp => exp.so001 == orderID && exp.u001 == userID).Take(1).FirstOrDefault();
                }
            }
            return result;
        }
        public MR GameAllowBet(int gameID, string account)
        {
            MR mr = new MR();
            DBModel.wgs027 config = null;
            DBModel.wgs027 configUser = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    config = db.Repositorywgs027.GetByPrimaryKey("SYS_ALLOW_BET_GAME");
                    configUser = db.Repositorywgs027.GetByPrimaryKey("SYS_ALLOW_BET_GAME_ACCOUNT");
                    if (null == config)
                    {
                        mr.Code = 1;
                        return mr;
                    }
                }
            }
            var allowGameSplit = config.cfg003.Split(',');
            var allowUser = configUser.cfg003.Split(',');
            Dictionary<int, string> dicAllowGame = new Dictionary<int, string>();
            List<string> listAllowUser = new List<string>();
            foreach (var alg in allowGameSplit)
            {
                var algSplit = alg.Split(':');
                dicAllowGame.Add(int.Parse(algSplit[0]), algSplit[1]);
            }
            foreach (var user in allowUser)
            {
                listAllowUser.Add(user);
            }
            var existsAllowGame = dicAllowGame.Count(exp => exp.Key == gameID);
            var existsAllowUser = listAllowUser.Count(exp => exp == account);
            if (1 == existsAllowGame)
            {
                mr.Code = 1;
                return mr;
            }
            else if (0 == existsAllowGame && 1 == existsAllowUser)
            {
                mr.Code = 1;
                return mr;
            }
            else
            {
                mr.Code = 0;
                mr.Message = config.cfg004;
            }
            return mr;
        }
        public MR CancelOrder(int type, List<long> orderIDs, int userID, int mgID, string mgName)
        {
            MR mr = new MR();
            if (0 == orderIDs.Count)
            {
                mr.Message = "编号为空";
                return mr;
            }
            if (-1 == type)
            {
                mr.Message = "类型不详";
                return mr;
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DateTime opDT = DateTime.Now;
            List<DBModel.wgs022> orderList = new List<DBModel.wgs022>();
            List<DBModel.wgs022> saveCancelOrderList = new List<DBModel.wgs022>();
            List<DBModel.wgs022> saveCancelMasterList = new List<DBModel.wgs022>();
            List<DBModel.wgs022> combineCreaterList = new List<DBModel.wgs022>();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    if (userID == -1)
                    {
                        orderList = db.Repositorywgs022.IQueryable(exp => orderIDs.Contains(exp.so001) && exp.so009 == 0 && exp.so022 == 0).ToList();
                    }
                    else
                    {
                        orderList = db.Repositorywgs022.IQueryable(exp => orderIDs.Contains(exp.so001) && exp.u001 == userID && exp.so009 == 0 && exp.so022 == 0).ToList();
                    }
                    foreach (var cancelOrder in orderList)
                    {
                        if (0 < cancelOrder.so021 || 0 < cancelOrder.so009)
                        {
                            continue;
                        }
                        cancelOrder.so021 = (byte)type;
                        cancelOrder.so009 = 2;
                        cancelOrder.mu001 = mgID;
                        cancelOrder.so008 = opDT;
                        if (0 < cancelOrder.mu001)
                        {
                            cancelOrder.mu002 = mgName;
                        }
                        if (0 < cancelOrder.so028 && 0 == cancelOrder.so027)
                        {
                            combineCreaterList.Add(cancelOrder);
                            continue;
                        }
                        saveCancelOrderList.Add(cancelOrder);
                        if (0 < cancelOrder.so028 && 1 == cancelOrder.so027)
                        {
                            var normalCombuy = db.Repositorywgs022.IQueryable(exp => cancelOrder.so028 == exp.so028 && exp.so027 == 0).Take(1).FirstOrDefault();
                            normalCombuy.so004 += cancelOrder.so004;
                            saveCancelMasterList.Add(normalCombuy);
                        }

                    }
                    try
                    {
                        if (0 < saveCancelOrderList.Count)
                        {
                            db.Repositorywgs022.UpdateList(saveCancelOrderList);
                            db.SaveChanges();
                        }
                        if (0 < saveCancelMasterList.Count)
                        {
                            db.Repositorywgs022.UpdateList(saveCancelMasterList);
                            db.SaveChanges();
                        }
                        ts.Complete();
                        mr.Code = 1;
                    }
                    catch (Exception error)
                    {
                        mr.Code = 0;
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = MyException.Message(mr.Exception.Message);
                    }
                }
            }
            using (TransactionScope tsMain = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                try
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        foreach (var mainItem in combineCreaterList)
                        {
                            var SumMoney = 0.0000m;
                            var joinOrderList = db.Repositorywgs022.IQueryable(exp => exp.so028 == mainItem.so028 && exp.so021 == 0 && exp.so009 == 0 && exp.so027 == 1).ToList();
                            foreach (var order in joinOrderList)
                            {
                                if (0 < order.so021 || 0 < order.so009)
                                {
                                    continue;
                                }
                                SumMoney += order.so004;
                                order.so021 = (byte)type;
                                order.so009 = 2;
                                order.mu001 = mgID;
                                order.so008 = opDT;
                                if (0 < order.mu001)
                                {
                                    order.mu002 = mgName;
                                }
                                db.Repositorywgs022.Update(order);
                                db.SaveChanges();
                            }
                            mainItem.so004 += SumMoney;
                            mainItem.so009 = mainItem.so009 < 2 ? (byte)0 : mainItem.so009;
                            db.Repositorywgs022.Update(mainItem);
                            db.SaveChanges();
                        }
                        tsMain.Complete();
                        mr.Code = 1;
                    }
                }
                catch (Exception error)
                {
                    mr.Code = 0;
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = MyException.Message(mr.Exception.Message);
                }
            }
            return mr;
        }
        public MR CancelTOrder(List<long> tOrderIDs, int type, int loginUserID, int mgID, string mgName)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DateTime opDT = DateTime.Now;
            List<DBModel.wgs022> orderList = new List<DBModel.wgs022>();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    orderList = db.Repositorywgs022.IQueryable(exp => tOrderIDs.Contains(exp.so001)).ToList();
                    if (0 < loginUserID)
                    {
                        orderList = orderList.Where(exp => exp.u001 == loginUserID).ToList();
                    }
                }
            }
            if (0 < orderList.Count)
            {
                mr = CancelOrder(type, orderList.Select(exp => exp.so001).ToList(), loginUserID, mgID, mgName);
            }
            return mr;
        }
        public MR CancelJoinOrder(int type, List<long> orderIDs, int userID, int mgID, string mgName)
        {
            MR mr = new MR();
            if (null == orderIDs)
            {
                mr.Message = "编号为空0x1";
            }
            if (0 == orderIDs.Count)
            {
                mr.Message = "编号为空0x2";
                return mr;
            }
            if (-1 == type)
            {
                mr.Message = "类型不详";
                return mr;
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            List<DBModel.wgs022> orderList = new List<DBModel.wgs022>();
            DateTime opDT = DateTime.Now;
            List<DBModel.wgs022> saveCancelOrderList = new List<DBModel.wgs022>();
            List<DBModel.wgs022> saveCancelMasterList = new List<DBModel.wgs022>();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    if (userID == -1)
                    {
                        orderList = db.Repositorywgs022.IQueryable(exp => orderIDs.Contains(exp.so001) && exp.so009 == 0 && exp.so022 == 0).ToList();
                    }
                    else
                    {
                        orderList = db.Repositorywgs022.IQueryable(exp => orderIDs.Contains(exp.so001) && exp.u001 == userID && exp.so009 == 0 && exp.so022 == 0).ToList();
                    }
                    foreach (var cancelOrder in orderList)
                    {
                        if (0 < cancelOrder.so021 || 0 < cancelOrder.so009)
                        {
                            continue;
                        }
                        cancelOrder.so021 = (byte)type;
                        cancelOrder.so009 = 2;
                        cancelOrder.mu001 = mgID;
                        cancelOrder.so008 = opDT;
                        if (0 < cancelOrder.mu001)
                        {
                            cancelOrder.mu002 = mgName;
                        }
                        saveCancelOrderList.Add(cancelOrder);
                    }
                }
            }
            using (TransactionScope tsdo = new TransactionScope())
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        if (0 < saveCancelOrderList.Count)
                        {
                            db.Repositorywgs022.UpdateList(saveCancelOrderList);
                            db.SaveChanges();
                        }
                        if (0 < saveCancelMasterList.Count)
                        {
                            db.Repositorywgs022.UpdateList(saveCancelMasterList);
                            db.SaveChanges();
                        }
                        tsdo.Complete();
                        mr.Code = 1;
                    }
                    catch (Exception error)
                    {
                        mr.Code = 0;
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = MyException.Message(mr.Exception.Message);
                    }
                }
            }
            return mr;
        }

        public MR CommissionOrder(string data, DBModel.wgs027 dbSet,int mgID, string mgName, DateTime orderDts, DateTime orderDte)
        {
            MR mr = new MR();
            string[] str = dbSet.cfg003.Split('|');
            string[] strCons = str[0].Split(',');
            string[] Cons0 = strCons[0].Split(':');
            string[] Cons1 = strCons[1].Split(':');

            string[] strLoss = str[1].Split(',');
            string[] Loss0 = strLoss[0].Split(':');
            string[] Loss1 = strLoss[1].Split(':');

            decimal moneyCons0 = decimal.Parse(Cons0[0]);
            decimal oddsCons01 = decimal.Parse(Cons0[1]);
            decimal oddsCons02 = decimal.Parse(Cons0[2]);
            decimal oddsCons03 = decimal.Parse(Cons0[3]);
            decimal moneyCons1 = decimal.Parse(Cons1[0]);
            decimal oddsCons11 = decimal.Parse(Cons1[1]);
            decimal oddsCons12 = decimal.Parse(Cons1[2]);
            decimal oddsCons13 = decimal.Parse(Cons1[3]);

            decimal moneyLoss0 = decimal.Parse(Loss0[0]);
            decimal oddsLoss01 = decimal.Parse(Loss0[1]);
            decimal oddsLoss02 = decimal.Parse(Loss0[2]);
            decimal oddsLoss03 = decimal.Parse(Loss0[3]);
            decimal moneyLoss1 = decimal.Parse(Loss1[0]);
            decimal oddsLoss11 = decimal.Parse(Loss1[1]);
            decimal oddsLoss12 = decimal.Parse(Loss1[2]);
            decimal oddsLoss13 = decimal.Parse(Loss1[3]);
            
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.RepeatableRead;
            DateTime opDT = DateTime.Now;
            List<DBModel.wgs022> orderList = new List<DBModel.wgs022>();
            List<DBModel.wgs022> upCommissionOrderList = new List<DBModel.wgs022>();
            List<DBModel.wgs014> userMoneyList = new List<DBModel.wgs014>();
            DBModel.wgs057 Commission = new DBModel.wgs057();
            DBModel.wgs016 mguItem = null;
            List<DBModel.OrderDayAccSumMoney> list = null;
            List<DBModel.OrderDayAccSumMoney> list1 = new List<DBModel.OrderDayAccSumMoney>();
            List<DBModel.OrderDayAccSumMoney> list2 = new List<DBModel.OrderDayAccSumMoney>();
            Expression<Func<DBModel.wgs022, bool>> query = PredicateExtensionses.True<DBModel.wgs022>();
            using (TransactionScope tsMain = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    mguItem = db.Repositorywgs016.GetByPrimaryKey(mgID);
                    Commission.c004 = orderDts;
                    Commission.c005 = _NWC.DEncrypt.Decrypt(mguItem.mu002).Trim();
                    Commission.c006 = _NWC.DEncrypt.Decrypt(mguItem.mu003).Trim();
                    string id = "",ordermoney="",lostmoney="";
                    string[] _data = data.Split(',');
                    foreach (var item1 in _data)
                    {
                        string[] s = item1.Split('|');
                        id = s[0].ToString();
                        ordermoney = s[1].ToString();
                        lostmoney = s[2].ToString();
                        Commission.u002 = "";
                        Commission.u003 = "";
                        Commission.u004 = "";
                        Commission.cm001 = 0;
                        Commission.cm002 = 0;
                        Commission.cm003 = 0;
                        Commission.cm005 = 0;
                        Commission.cm006 = 0;
                        Commission.cm008 = 0;
                        Commission.cm009 = 0;
                        var user1 = db.Repositorywgs012.GetByPrimaryKey(int.Parse(id));
                        Commission.u001 = user1.u002;
                        if (user1 != null)
                        {  
                            if (user1.u012 != 0)
                            {
                                if (0 < decimal.Parse(ordermoney))
                                {
                                    if (decimal.Parse(ordermoney) >= moneyCons0 && decimal.Parse(ordermoney) < moneyCons1)
                                    {
                                        Commission.u001 = user1.u002;
                                        user1 = db.Repositorywgs012.GetByPrimaryKey(user1.u012);
                                        if (user1 != null)
                                        {
                                            var userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user1.u001);
                                            Commission.u002 = user1.u002;
                                            Commission.cm001 = userMoney1.uf012;
                                            Commission.cm002 = oddsCons01;
                                            userMoney1.uf012 += oddsCons01;
                                            userMoneyList.Add(userMoney1);
                                            user1 = db.Repositorywgs012.GetByPrimaryKey(user1.u012);
                                            if (user1 != null)
                                            {
                                                userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user1.u001);
                                                Commission.u003 = user1.u002;
                                                Commission.cm004 = userMoney1.uf012;
                                                Commission.cm005 = oddsCons02;
                                                userMoney1.uf012 += oddsCons02;
                                                userMoneyList.Add(userMoney1);
                                                user1 = db.Repositorywgs012.GetByPrimaryKey(user1.u012);
                                                if (user1 != null)
                                                {
                                                    userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user1.u001);
                                                    Commission.u004 = user1.u002;
                                                    Commission.cm007 = userMoney1.uf012;
                                                    Commission.cm008 = oddsCons03;
                                                    userMoney1.uf012 += oddsCons03;
                                                    userMoneyList.Add(userMoney1);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (decimal.Parse(ordermoney) >= moneyCons1)
                                {
                                    Commission.u001 = user1.u002;
                                    user1 = db.Repositorywgs012.GetByPrimaryKey(user1.u012);
                                    if (user1 != null)
                                    {
                                        var userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user1.u001);
                                        Commission.u002 = user1.u002;
                                        Commission.cm001 = userMoney1.uf012;
                                        Commission.cm002 = oddsCons11;
                                        userMoney1.uf012 += oddsCons11;
                                        userMoneyList.Add(userMoney1);
                                        user1 = db.Repositorywgs012.GetByPrimaryKey(user1.u012);
                                        if (user1 != null)
                                        {
                                            userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user1.u001);
                                            Commission.u003 = user1.u002;
                                            Commission.cm004 = userMoney1.uf012;
                                            Commission.cm005 = oddsCons12;
                                            userMoney1.uf012 += oddsCons12;
                                            userMoneyList.Add(userMoney1);
                                            user1 = db.Repositorywgs012.GetByPrimaryKey(user1.u012);
                                            if (user1 != null)
                                            {
                                                userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user1.u001);
                                                Commission.u004 = user1.u002;
                                                Commission.cm007 = userMoney1.uf012;
                                                Commission.cm008 = oddsCons13;
                                                userMoney1.uf012 += oddsCons13;
                                                userMoneyList.Add(userMoney1);
                                            }
                                        }
                                    }
                                }
                                var user2 = db.Repositorywgs012.GetByPrimaryKey(int.Parse(id));
                                if (0 < decimal.Parse(lostmoney))
                                {
                                    if (decimal.Parse(lostmoney) >= moneyLoss0 && decimal.Parse(lostmoney) < moneyLoss1)
                                    {
                                        Commission.u001 = user2.u002;
                                        user2 = db.Repositorywgs012.GetByPrimaryKey(user2.u012);
                                        if (user2!= null)
                                        {
                                            var userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user2.u001);
                                            Commission.u002 = user2.u002;
                                           // Commission.cm001 = userMoney1.uf012;
                                            Commission.cm003 = oddsLoss01;
                                            userMoney1.uf012 += oddsLoss01;
                                            userMoneyList.Add(userMoney1);
                                            user2 = db.Repositorywgs012.GetByPrimaryKey(user2.u012);
                                            if (user2!= null)
                                            {
                                                userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user2.u001);
                                                Commission.u003 = user2.u002;
                                                //Commission.cm004 = userMoney1.uf012;
                                                Commission.cm006 = oddsLoss02;
                                                userMoney1.uf012 += oddsLoss02;
                                                userMoneyList.Add(userMoney1);
                                                user2 = db.Repositorywgs012.GetByPrimaryKey(user2.u012);
                                                if (user2!= null)
                                                {
                                                    userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user2.u001);
                                                    Commission.u004 = user2.u002;
                                                    //Commission.cm007 = userMoney1.uf012;
                                                    Commission.cm009 = oddsLoss03;
                                                    userMoney1.uf012 += oddsLoss03;
                                                    userMoneyList.Add(userMoney1);
                                                }
                                            }
                                        }
                                    }
                                }
                                if (decimal.Parse(lostmoney) >= moneyLoss1)
                                {
                                    Commission.u001 = user2.u002;
                                    user2 = db.Repositorywgs012.GetByPrimaryKey(user2.u012);
                                    if (user2!= null)
                                    {
                                        var userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user2.u001);
                                        Commission.u002= user2.u002;
                                        //Commission.cm001 = userMoney1.uf012;
                                        Commission.cm003 = oddsLoss11;
                                        userMoney1.uf012 += oddsLoss11;
                                        userMoneyList.Add(userMoney1);
                                        user2 = db.Repositorywgs012.GetByPrimaryKey(user2.u012);
                                        if (user2!= null)
                                        {
                                            userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user2.u001);
                                            Commission.u003 = user2.u002;
                                           // Commission.cm004 = userMoney1.uf012;
                                            Commission.cm006 = oddsLoss12;
                                            userMoney1.uf012 += oddsLoss12;
                                            userMoneyList.Add(userMoney1);
                                            user2 = db.Repositorywgs012.GetByPrimaryKey(user2.u012);
                                            if (user2!= null)
                                            {
                                                userMoney1 = db.Repositorywgs014.GetByPrimaryKey(user2.u001);
                                                Commission.u004 = user2.u002;
                                               // Commission.cm007 = userMoney1.uf012;
                                                Commission.cm009 = oddsLoss13;
                                                userMoney1.uf012 += oddsLoss13;
                                                userMoneyList.Add(userMoney1);
                                            }
                                        }
                                    }
                                }
                                var user3 = db.Repositorywgs012.GetByPrimaryKey(int.Parse(id));
                                Commission.u001 = user3.u002;
                                Commission.c002 = decimal.Parse(lostmoney);
                                Commission.c003 = decimal.Parse(ordermoney);
                                Commission.c007 = false;
                                db.Repositorywgs057.Add(Commission);
                                db.SaveChanges();
                                db.Repositorywgs014.UpdateList(userMoneyList);
                                db.SaveChanges();
                            }   
                        } 
                    }
                    orderList = db.Repositorywgs022.IQueryable(exp => exp.so021 == 0 && exp.so022 != 0 && exp.so033 == false && exp.so007 >= orderDts && exp.so007 <= orderDte).ToList();
                    foreach (var item in orderList)
                    {
                        item.so033 = true;
                        upCommissionOrderList.Add(item);
                    }
                    db.Repositorywgs022.UpdateList(upCommissionOrderList);
                    db.SaveChanges();
                    tsMain.Complete();
                    mr.Code = 1;
                }
            }
            return mr;
        }
        public List<DBModel.wgs057> CommissionList(string UserName, DateTime Dts, DateTime Dte, int pageIndex, int pageSize, out int recordCout)
        {
            recordCout = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            List<DBModel.wgs057> CommissionList = new List<DBModel.wgs057>();
            Expression<Func<DBModel.wgs057, bool>> query = PredicateExtensionses.True<DBModel.wgs057>();
            using (UnitOfWork db = new UnitOfWork())
            {
                query = query.And(exp => exp.u002 != "");
                if ("" != UserName.Trim())
                {
                    query = query.And(exp => exp.u001 == UserName.Trim() || exp.u002 == UserName.Trim() || exp.u003 == UserName.Trim() || exp.u004 == UserName.Trim());
                }
                query = query.And(exp => exp.c004 >= Dts && exp.c004 <= Dte);
                recordCout = db.Repositorywgs057.Count(query);
                CommissionList = db.Repositorywgs057.IQueryable(query,order => order.OrderBy(exp => exp.c001)).Skip(pageSize * pageIndex).Take(pageSize).ToList();
            }
            return CommissionList;
        }
        public List<DBModel.wgs057> CommissionList( DateTime Dts, DateTime Dte)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            List<DBModel.wgs057> CommissionList = new List<DBModel.wgs057>();
            Expression<Func<DBModel.wgs057, bool>> query = PredicateExtensionses.True<DBModel.wgs057>();
            using (UnitOfWork db = new UnitOfWork())
            {
                query = query.And(exp => exp.u002 != "");
                query = query.And(exp => exp.c004 >= Dts && exp.c004 <= Dte);
                CommissionList = db.Repositorywgs057.IQueryable(query,order => order.OrderBy(exp => exp.c001)).ToList();
            }
            return CommissionList;
        }

        public List<DBModel.CommissionDaySendMessage> GetCommissionList(DateTime Dts)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            List<DBModel.wgs057> CommissionList = new List<DBModel.wgs057>();
            List<DBModel.wgs057> CommissionList1 = new List<DBModel.wgs057>();
            Expression<Func<DBModel.wgs057, bool>> query = PredicateExtensionses.True<DBModel.wgs057>();
            Expression<Func<DBModel.wgs057, bool>> query1 = PredicateExtensionses.True<DBModel.wgs057>();
            List<DBModel.CommissionDaySendMessage> CDSendMessageList = new List<DBModel.CommissionDaySendMessage>();
            List<DBModel.CommissionDaySendMessage> CDSendMessageList1 = new List<DBModel.CommissionDaySendMessage>();
            List<string> toUserList = new List<string>();
            using (UnitOfWork db = new UnitOfWork())
            {
                DateTime Dte=Dts.AddDays(1);
                query = query.And(exp => exp.c004 >= Dts && exp.c004 <= Dte && exp.c007 == false);
                CommissionList = db.Repositorywgs057.IQueryable(query, order => order.OrderBy(exp => exp.c001)).ToList();
                foreach (var item in CommissionList)
                {
                    DBModel.CommissionDaySendMessage CDSendMessage2 = new DBModel.CommissionDaySendMessage();
                    DBModel.CommissionDaySendMessage CDSendMessage3 = new DBModel.CommissionDaySendMessage();
                    DBModel.CommissionDaySendMessage CDSendMessage4 = new DBModel.CommissionDaySendMessage();
                    query1 = query.Or(exp => exp.u002 == item.u002);
                    query1 = query.Or(exp => exp.u003 == item.u003);
                    query1 = query.Or(exp => exp.u004 == item.u004);
                    query1 = query.And(exp => exp.c004 >= Dts && exp.c004 <= Dte);
                    CommissionList1 = db.Repositorywgs057.IQueryable(query1, order => order.OrderBy(exp => exp.c001)).ToList();
                    foreach (var item1 in CommissionList1)
                    {
                        if (item.u002 == item1.u002)
                        {
                            CDSendMessage2.DayConsume += item1.cm002;
                            CDSendMessage2.DayLoss += item1.cm003;
                            CDSendMessage2.DayConsumeMoney += item1.c003;
                            CDSendMessage2.DayLossMoney += item1.c002;
                            CDSendMessage2.name = item.u002;
                        }
                        if (item.u002 == item1.u003)
                        {
                            CDSendMessage2.DayConsume += item1.cm005;
                            CDSendMessage2.DayLoss += item1.cm006;
                            CDSendMessage2.DayConsumeMoney += item1.c003;
                            CDSendMessage2.DayLossMoney += item1.c002;
                            CDSendMessage2.name = item.u002;
                        }
                        if (item.u002 == item1.u004)
                        {
                            CDSendMessage2.DayConsume += item1.cm008;
                            CDSendMessage2.DayLoss += item1.cm009;
                            CDSendMessage2.DayConsumeMoney += item1.c003;
                            CDSendMessage2.DayLossMoney += item1.c002;
                            CDSendMessage2.name = item.u002;
                        }
                        if (item.u003.Trim() != "")
                        {
                            if (item.u003 == item1.u002)
                            {
                                CDSendMessage3.DayConsume += item1.cm002;
                                CDSendMessage3.DayLoss += item1.cm003;
                                CDSendMessage3.DayConsumeMoney += item1.c003;
                                CDSendMessage3.DayLossMoney += item1.c002;
                                CDSendMessage3.name = item.u003;
                            }
                            if (item.u003 == item1.u003)
                            {
                                CDSendMessage3.DayConsume += item1.cm005;
                                CDSendMessage3.DayLoss += item1.cm006;
                                CDSendMessage3.DayConsumeMoney += item1.c003;
                                CDSendMessage3.DayLossMoney += item1.c002;
                                CDSendMessage3.name = item.u003;
                            }
                            if (item.u003 == item1.u004)
                            {
                                CDSendMessage3.DayConsume += item1.cm008;
                                CDSendMessage3.DayLoss += item1.cm009;
                                CDSendMessage3.DayConsumeMoney += item1.c003;
                                CDSendMessage3.DayLossMoney += item1.c002;
                                CDSendMessage3.name = item.u003;
                            }
                        }
                        if (item.u004.Trim() != "")
                        {
                            if (item.u004 == item1.u002)
                            {
                                CDSendMessage4.DayConsume += item1.cm002;
                                CDSendMessage4.DayLoss += item1.cm003;
                                CDSendMessage4.DayConsumeMoney += item1.c003;
                                CDSendMessage4.DayLossMoney += item1.c002;
                                CDSendMessage4.name = item.u004;
                            }
                            if (item.u004 == item1.u003)
                            {
                                CDSendMessage4.DayConsume += item1.cm005;
                                CDSendMessage4.DayLoss += item1.cm006;
                                CDSendMessage4.DayConsumeMoney += item1.c003;
                                CDSendMessage4.DayLossMoney += item1.c002;
                                CDSendMessage4.name = item.u004;
                            }
                            if (item.u004 == item1.u004)
                            {
                                CDSendMessage4.DayConsume += item1.cm008;
                                CDSendMessage4.DayLoss += item1.cm009;
                                CDSendMessage4.DayConsumeMoney += item1.c003;
                                CDSendMessage4.DayLossMoney += item1.c002;
                                CDSendMessage4.name = item.u004;
                            }
                        }
                    }
                    //CDSendMessageList.Add(CDSendMessage2);
                    //CDSendMessageList.Add(CDSendMessage3);
                    //CDSendMessageList.Add(CDSendMessage4);
                    if (CDSendMessage2.DayConsume != 0 || CDSendMessage2.DayLoss != 0)
                    {
                        CDSendMessageList.Add(CDSendMessage2);
                    }

                    if (CDSendMessage3.DayConsume != 0 || CDSendMessage3.DayLoss != 0)
                    {
                        CDSendMessageList.Add(CDSendMessage3);
                    }
                    if (CDSendMessage4.DayConsume != 0 || CDSendMessage4.DayLoss != 0)
                    {
                        CDSendMessageList.Add(CDSendMessage4);
                    }
                }
            }
            return CDSendMessageList;
        }
        public MR CommissionDaySendMessage(string data, string mgName, DateTime Dts,DateTime Dte)
        {
            MR mr = new MR();
            ISystem sys = new System();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            string name = "";
            string cMoney = "";
            string lMoney ="";
            List<DBModel.wgs057> CommissionList = new List<DBModel.wgs057>();
            List<DBModel.wgs057> UPCommissionList = new List<DBModel.wgs057>();
            Expression<Func<DBModel.wgs057, bool>> query = PredicateExtensionses.True<DBModel.wgs057>();
            using (TransactionScope tsMain = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    CommissionList = db.Repositorywgs057.IQueryable(exp => exp.c004 >= Dts && exp.c004 <= Dte).ToList();
                    foreach (var it in CommissionList)
                    {
                        it.c007 = true;
                        UPCommissionList.Add(it);
                    }
                    db.Repositorywgs057.UpdateList(UPCommissionList);
                    db.SaveChanges();
                    List<string> toUserList = new List<string>();
                    string[] str = data.Split(',');
                    foreach (var item in str)
                    {  
                        string[] s = item.Split('|');
                        if (!toUserList.Contains(s[0]))
                        {
                            toUserList.Add(s[0]);
                            name = s[0].ToString();
                            cMoney = s[3].ToString();
                            lMoney = s[4].ToString();
                            var userItem = db.Repositorywgs012.IQueryable(exp => exp.u002 ==name ).Take(1).FirstOrDefault();
                            DBModel.wgs044 entity = new DBModel.wgs044();
                            entity.msg002 = "返送佣金";
                            entity.msg003 = "当天消费" + s[1] + "当天亏损" + s[2] + "<br>消费返佣金" + s[3] + "+亏损返佣金" + s[4] + "=今天返的佣金" + (decimal.Parse(cMoney) + decimal.Parse(lMoney));
                            entity.msg004 = 0;
                            entity.msg005 = userItem.u001;
                            entity.msg006 = Dts;
                            entity.msg008 = "_";
                            entity.msg009 = userItem.u002;
                            entity.msg010 = "_";
                            entity.msg011 = userItem.u003 == null ? null : userItem.u003.Trim();
                            db.Repositorywgs044.Add(entity);
                            db.SaveChanges();
                            DBModel.wgs014 uf = db.Repositorywgs014.GetByPrimaryKey(userItem.u001);
                            decimal oldPoint = uf.uf012;
                            decimal newPoint = oldPoint + decimal.Parse(cMoney) + decimal.Parse(lMoney);
                            uf.uf012 += decimal.Parse(cMoney) + decimal.Parse(lMoney);
                            //db.Repositorywgs014.Update(uf);
                            //db.SaveChanges();
                            DBModel.wgs021 wgs021 = new DBModel.wgs021();
                            wgs021.u001 = userItem.u001;
                            wgs021.u002 = userItem.u002.Trim();
                            wgs021.u003 = string.IsNullOrEmpty(userItem.u003) ? string.Empty : userItem.u003.Trim();
                            wgs021.uxf002 = uf.uf001;
                            wgs021.uxf003 = decimal.Parse(cMoney) + decimal.Parse(lMoney);
                            wgs021.uxf012 = oldPoint;
                            wgs021.uxf013 = newPoint;
                            wgs021.uxf007 = uf.uf001;
                            wgs021.uxf008 = newPoint;
                            wgs021.uxf009 = uf.uf003;
                            wgs021.uxf010 = uf.uf003;
                            wgs021.uxf014 = Dts;
                            wgs021.uxf016 = 37;
                            wgs021.uxf015 = sys.GetSystemDataChangeTypeList(true)[46].Name;
                            db.Repositorywgs021.Add(wgs021);
                            db.SaveChanges();
                        }
                    }

                }
                tsMain.Complete();
                mr.Code = 1;
                return mr;
            }
        }
        public MR GameSessionIsAllow(int gameID, string issue)
        {
            MR mr = new MR();
            DBModel.wgs005 result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs005.IQueryable(exp => exp.g001 == gameID && exp.gs002 == issue, order => order.OrderByDescending(exp => exp.gs006)).Take(1).FirstOrDefault();
                }
            }
            if (null == result)
            {
                mr.Message = result.gs002.Trim() + "，期数不存在";
                return mr;
            }
            else if (0 == result.gs011)
            {
                mr.Message = result.gs002.Trim() + "，不允许下单";
                return mr;
            }
            else if (DateTime.Now >= result.gs004)
            {
                mr.Message = result.gs002.Trim() + string.Format("，已经封盘，开盘时间：{0}，封盘时间：{1}，开奖时间：{2}", result.gs003, result.gs004, result.gs005);
                return mr;
            }
            else
            {
                mr.Code = 1;
                mr.IntData = result.gs001;
            }
            return mr;
        }
        public DBModel.TraceOrderDetail GetTOrderItem(long orderID, int gameID, int gameClassID, int userID, int pageIndex, int pageSize, out int recordCount)
        {
            recordCount = 0;
            DBModel.TraceOrderDetail result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            List<int> myChildIDs = new List<int>();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    myChildIDs = db.Repositorywgs048.IQueryable(exp => exp.u001 == userID).Select(exp => exp.u002).ToList();
                    var traceItem = db.Repositorywgs030.GetByPrimaryKey(orderID);
                    var count = myChildIDs.Count(exp => exp == traceItem.u001);
                    result = new DBModel.TraceOrderDetail();
                    if (0 == count)
                    {
                        recordCount = db.Repositorywgs045.Count(exp => exp.sto001 == orderID);
                        var orderList = db.Repositorywgs045.IQueryable(exp => exp.sto001 == orderID, order => order.OrderBy(exp => exp.so001)).Skip(pageSize * pageIndex).Take(pageSize).ToList();
                        var orderShowList = GetOrderShowList(orderList);
                        result.TraceOrder = traceItem;
                        result.OrderList45 = orderList;
                        result.OrderShowList = orderShowList;
                    }
                    else
                    {
                        recordCount = db.Repositorywgs045.Count(exp => exp.sto001 == orderID && myChildIDs.Contains(exp.u001));
                        var orderList = db.Repositorywgs045.IQueryable(exp => exp.sto001 == orderID && myChildIDs.Contains(exp.u001), order => order.OrderBy(exp => exp.so001)).Skip(pageSize * pageIndex).Take(pageSize).ToList();
                        var orderShowList = GetOrderShowList(orderList);
                        result.TraceOrder = traceItem;
                        result.OrderList45 = orderList;
                        result.OrderShowList = orderShowList;
                    }
                }
            }
            return result;
        }
        public int GetGameSessionDayCount(int gameID)
        {
            int result = 0;
            string coutValues = string.Empty;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            List<int> myChildIDs = new List<int>();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    coutValues = db.Repositorywgs027.GetByPrimaryKey("COMMON_GAME_SESSION_COUNT").cfg003;
                }
            }
            if (string.IsNullOrEmpty(coutValues))
            {
                result = 0;
            }
            else
            {
                var valuesSplit = coutValues.Split(',');
                foreach (var item in valuesSplit)
                {
                    var itemSPlit = item.Split(':');
                    if (int.Parse(itemSPlit[0]) == gameID)
                    {
                        result = int.Parse(itemSPlit[1]);
                        break;
                    }
                }
            }
            return result;
        }
        public decimal CheckWinLose(string result, List<DBModel.wgs045> order)
        {
            decimal money = 0;
            foreach (var item in order)
            {
                Assembly _moduleLoad = null;
                object _moduleObject = null;
                Type _moduleType = null;
                __CGM(item.gc001, out _moduleLoad, out _moduleObject, out _moduleType);
                var result2 = string.Empty;
                if (3 == item.gc001)
                {
                    var x115 = result.Trim().Split(',');
                    foreach (var n in x115)
                    {
                        result2 += int.Parse(n) > 9 ? n : "0" + n;
                        result2 += ",";
                    }
                    result2 = result2.Substring(0, result2.Length - 1);
                }
                else
                {
                    result2 = result.Trim();
                }
                object orderResult = new object();
                __CGMM(ref _moduleObject, ref _moduleType, 0, "Codes", item.so002.Trim(), out orderResult);
                __CGMM(ref _moduleObject, ref _moduleType, 0, "Position", item.so031 != null ? item.so031.Trim() : string.Empty, out orderResult);
                __CGMM(ref _moduleObject, ref _moduleType, 0, "Result", result2, out orderResult);
                __CGMM(ref _moduleObject, ref _moduleType, 2, "Result" + item.gm001, null, out orderResult);
                var orderResultStr = (string)orderResult;
                Dictionary<string, string> moenyItem = new Dictionary<string, string>();
                item.so024 = item.so024.Trim();
                if (item.so024.Contains(','))
                {
                    foreach (var temp in item.so024.Split(','))
                    {
                        var temp2 = temp.Split('|');
                        moenyItem.Add(temp2[0], temp2[1]);
                    }
                    money -= item.so004;
                }
                else
                {
                    var temp2 = item.so024.Trim().Split('|');
                    moenyItem.Add(temp2[0], temp2[1]);
                    money -= item.so004;
                    money += item.so004 * decimal.Parse(item.so024.Split('|')[2]);
                }
                foreach (Match match in Regex.Matches(orderResultStr, @"[\d]{1,10}:[1-9]{1,5}"))
                {
                    var temp = match.Value.Split(':');
                    if (item.so006 == 1)
                    {
                        money += decimal.Parse(temp[1]) * decimal.Parse(moenyItem[temp[0]]) * item.so005;
                    }
                    else if (item.so006 == 2)
                    {
                        money += decimal.Parse(temp[1]) * decimal.Parse(moenyItem[temp[0]]) * item.so005 * (decimal)0.1;
                    }
                    else
                    {
                        money += decimal.Parse(temp[1]) * decimal.Parse(moenyItem[temp[0]]) * item.so005 * (decimal)0.01;
                    }
                }
            }
            return money;
        }
    }
}
