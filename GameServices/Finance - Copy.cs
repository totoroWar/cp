using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBase;
using _NWC = NETCommon;
using System.Linq.Expressions;
using System.Transactions;
namespace GameServices
{
    public class Finance : IFinance
    {
        private string bankCacheKey = "BankListCacheKey";
        private string withdrawTypeListCacheKey = "WithdrawTypeListCacheKey";
        private string ctListCacheKey = "CTListCacheKey";
        public MR AddBank(DBModel.wgs010 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs010.Add(entity);
                    db.SaveChanges();
                    ClearBankListCache();
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
        public MR UpdateBank(List<DBModel.wgs010> entityList)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs010.UpdateList(entityList);
                    db.SaveChanges();
                    ClearBankListCache();
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
        public List<DBModel.wgs010> GetBankList()
        {
            List<DBModel.wgs010> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs010.GetAll().OrderBy(exp => exp.sb009).ToList();
                    NETCommon.GeneralCache.Set(bankCacheKey, list);
                }
            }
            return list;
        }
        public List<DBModel.wgs010> GetBankListByCache()
        {
            List<DBModel.wgs010> list = (List<DBModel.wgs010>)NETCommon.GeneralCache.Get(bankCacheKey);
            if (null == list)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        list = db.Repositorywgs010.GetAll().OrderBy(exp => exp.sb009).ToList();
                        NETCommon.GeneralCache.Set(bankCacheKey, list);
                    }
                }
            }
            return list;
        }
        public void ClearBankListCache()
        {
            NETCommon.GeneralCache.Clear(bankCacheKey);
        }
        public MR CheckDayChargeCount(int myUserID)
        {
            MR mr = new MR();
            mr.Code = 1;
            int chargeCount = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DBModel.wgs027 config = null;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                DateTime dts = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd " + "00:00:00"));
                DateTime dte = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd " + "23:59:59"));
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    chargeCount = db.Repositorywgs019.Count(exp => exp.u001 == myUserID && exp.uc006 >= dts && exp.uc006 <= dte && exp.uc008 == 1);
                    config = db.Repositorywgs027.GetByPrimaryKey("SYS_DAY_CHARGE");
                }
            }
            if (chargeCount >= int.Parse(config.cfg003))
            {
                mr.Code = 0;
                mr.Message = config.cfg004;
            }
            return mr;
        }
        public MR AddCT(DBModel.wgs009 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    entity.ct002 = _NWC.DEncrypt.Encrypt(entity.ct002);
                    entity.ct003 = _NWC.DEncrypt.Encrypt(entity.ct003);
                    entity.ct004 = _NWC.DEncrypt.Encrypt(entity.ct004);
                    db.Repositorywgs009.Add(entity);
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
        public MR UpdateCT(DBModel.wgs009 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    entity.ct002 = _NWC.DEncrypt.Encrypt(entity.ct002);
                    entity.ct003 = _NWC.DEncrypt.Encrypt(entity.ct003);
                    entity.ct004 = _NWC.DEncrypt.Encrypt(entity.ct004);
                    db.Repositorywgs009.Update(entity);
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
        public List<DBModel.wgs009> GetCTList()
        {
            List<DBModel.wgs009> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs009.IQueryable(null, order => order.OrderByDescending(exp => exp.ct001)).ToList();
                    for (int i = 0; i < list.Count(); i++)
                    {
                        list[i].ct002 = _NWC.DEncrypt.Decrypt(list[i].ct002);
                        list[i].ct003 = _NWC.DEncrypt.Decrypt(list[i].ct003);
                        list[i].ct004 = _NWC.DEncrypt.Decrypt(list[i].ct004);
                    }
                }
            }
            return list;
        }
        public List<DBModel.SysSumDRInfo> GetLevelSumReport(int userID, int type, DateTime dts, DateTime dte)
        {
            dts = DateTime.Parse(dts.ToString("yyyy/MM/dd") + " 00:00:00");
            dte = DateTime.Parse(dte.ToString("yyyy/MM/dd") + " 23:59:59");
            List<DBModel.SysSumDRInfo> result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.SqlQuery<DBModel.SysSumDRInfo>("EXECUTE CLR_USP_GetSumReport {0}, {1}, 0, 0, {2}, {3}", new object[] { type, userID, dts, dte }).ToList();
                }
            }
            return result;
        }
        public List<DBModel.wgs009> GetCTListByCache()
        {
            List<DBModel.wgs009> list = (List<DBModel.wgs009>)_NWC.GeneralCache.Get(ctListCacheKey);
            if (null == list)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        list = db.Repositorywgs009.IQueryable(null, order => order.OrderByDescending(exp => exp.ct001)).ToList();
                        for (int i = 0; i < list.Count(); i++)
                        {
                            list[i].ct002 = _NWC.DEncrypt.Decrypt(list[i].ct002);
                            list[i].ct003 = _NWC.DEncrypt.Decrypt(list[i].ct003);
                            list[i].ct004 = _NWC.DEncrypt.Decrypt(list[i].ct004);
                        }
                    }
                }
            }
            return list;
        }
        public void ClearCTListCache()
        {
            _NWC.GeneralCache.Clear(ctListCacheKey);
        }
        public DBModel.wgs009 GetCT(int key)
        {
            DBModel.wgs009 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs009.IQueryable(exp => exp.ct001 == key).FirstOrDefault();
                    if (null != entity)
                    {
                        entity.ct002 = _NWC.DEncrypt.Decrypt(entity.ct002);
                        entity.ct003 = _NWC.DEncrypt.Decrypt(entity.ct003);
                        entity.ct004 = _NWC.DEncrypt.Decrypt(entity.ct004);
                    }
                }
            }
            return entity;
        }
        public MR AddCharege(DBModel.wgs019 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    var userItem = db.Repositorywgs012.IQueryable(exp => exp.u001 == entity.u001, null, "wgs014").FirstOrDefault();
                    entity.uc003 = 0;
                    entity.uc004 = userItem.wgs014.uf001;
                    entity.u001 = userItem.u001;
                    entity.u002 = userItem.u002;
                    entity.u003 = userItem.u003 != null ? userItem.u003 : string.Empty;
                    int exists = 1;
                    do
                    {
                        entity.uc005 = _NWC.RandomString.Get("1,2,3,4,5,6,7,8,9,0", 15);
                        if ("0" == entity.uc005.Substring(0, 1))
                        {
                            exists = 1;
                        }
                        else
                        { 
                            exists = db.Repositorywgs019.Count(exp => exp.uc005 == entity.uc005);
                        }
                    } while (0 > exists);
                    db.Repositorywgs019.Add(entity);
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
        public List<DBModel.wgs019> GetChargeList(int bankType, int userID, string userName, long key, string ckey, int amtT, decimal amtTV, int amtTT, decimal amtTTV, int amtTHT, decimal amtTHTV, int status, DateTime? dts, DateTime? dte, int pageSize, int pageIndex, out int recordCout)
        {
            recordCout = 0;
            List<DBModel.wgs019> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    Expression<Func<DBModel.wgs019, bool>> query = PredicateExtensionses.True<DBModel.wgs019>();
                    if (dts.HasValue && dte.HasValue)
                    {
                        query = query.And(exp=>exp.uc006>=dts && exp.uc006 <=dte);
                    }
                    if (0 < bankType)
                    {
                        query = query.And(exp=>exp.sb001 == bankType);
                    }
                    if (0 < userID)
                    {
                        query = query.And(exp => exp.u001 == userID);
                    }
                    if (0 != key)
                    {
                        query = query.And(exp=>exp.uc001 == key);
                    }
                    if (false == string.IsNullOrEmpty(ckey))
                    {
                        query = query.And(exp=>exp.uc005 == ckey);
                    }
                    if (false == _NWC.GeneralValidate.IsNullOrEmpty(userName))
                    {
                        query = query.And(exp => exp.u002 == userName);
                    }
                    if (-1 != status)
                    {
                        query = query.And(exp => exp.uc008 == status);
                    }
                    else
                    {
                        query = query.And(exp => exp.uc008 > status);
                    }
                    if (amtT != 0)
                    {
                        switch (amtT)
                        {
                            case 1:
                                query = query.And(exp => exp.uc002 < amtTV);
                                break;
                            case 2:
                                query = query.And(exp => exp.uc002 == amtTV);
                                break;
                            case 3:
                                query = query.And(exp => exp.uc002 > amtTV);
                                break;
                        }
                    }
                    if (amtTT != 0)
                    {
                        switch (amtTT)
                        {
                            case 1:
                                query = query.And(exp => exp.uc003 < amtTTV);
                                break;
                            case 2:
                                query = query.And(exp => exp.uc003 == amtTTV);
                                break;
                            case 3:
                                query = query.And(exp => exp.uc003 > amtTTV);
                                break;
                        }
                    }
                    if (amtTHT != 0)
                    {
                        switch (amtTHT)
                        {
                            case 1:
                                query = query.And(exp => exp.uc013 < amtTHTV);
                                break;
                            case 2:
                                query = query.And(exp => exp.uc013 == amtTHTV);
                                break;
                            case 3:
                                query = query.And(exp => exp.uc013 > amtTHTV);
                                break;
                        }
                    }
                    recordCout = db.Repositorywgs019.Count(query);
                    list = db.Repositorywgs019.IQueryable(query).OrderByDescending(exp => exp.uc006).Skip(pageSize * pageIndex).Take(pageSize).ToList();
                }
            }
            return list;
        }
        public int CheckCCashCount(int status, int userID, string account, decimal amount, DateTime? dts, DateTime? dte)
        {
            int count = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    Expression<Func<DBModel.wgs019, bool>> query = PredicateExtensionses.True<DBModel.wgs019>();
                    if (-1 != status)
                    {
                        query = query.And(exp => exp.uc008 == status);
                    }
                    else
                    {
                        query = query.And(exp => exp.uc008 != status);
                    }
                    if (0 < userID)
                    {
                        query = query.And(exp => exp.u001 == userID);
                    }
                    if (null != account)
                    {
                        query = query.And(exp => exp.u002 == account);
                    }
                    if (0 != amount)
                    {
                        query = query.And(exp => exp.uc002 == amount);
                    }
                    if (null != dts && null != dte)
                    {
                        DateTime dtsn = DateTime.Parse(dts.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                        DateTime dten = DateTime.Parse(dte.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                        query = query.And(exp => exp.uc006 <= dtsn && exp.uc006 >= dten);
                    }
                    count = db.Repositorywgs019.Count(query);
                }
            }
            return count;
        }
        public int CheckWCashCount(int status, int userID, string account, decimal amount, DateTime dts, DateTime dte)
        {
            throw new NotImplementedException();
        }
        public DBModel.wgs019 GetCCash(long key)
        {
            DBModel.wgs019 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.RepeatableRead;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs019.GetByPrimaryKey(key);
                }
            }
            return entity;
        }
        public DBModel.wgs019 GetCCash(string chargeCode)
        {
            DBModel.wgs019 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.RepeatableRead;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs019.IQueryable(exp=>exp.uc005 == chargeCode).Take(1).FirstOrDefault();
                }
            }
            return entity;
        }
        public MR UpdateCCash(DBModel.wgs019 entity)
        {
            MR mr = new MR();
            if (1 == entity.uc008 || 2 == entity.uc008)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.RepeatableRead;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork())
                    {
                        DBModel.wgs019 wgs19 = new DBModel.wgs019();
                        wgs19 = db.Repositorywgs019.GetByPrimaryKey(entity.uc001);
                        if (1 == wgs19.uc008 || 2 == wgs19.uc008)
                        {
                            throw new Exception("已经处理");
                        }
                        DateTime curDateTime = DateTime.Now; 
                        wgs19.uc008 = entity.uc008;
                        wgs19.uc013 = entity.uc013;
                        wgs19.uc003 = entity.uc003;
                        wgs19.mu001x = entity.mu001x;
                        wgs19.mu002x = entity.mu002x;
                        wgs19.uc007 = curDateTime;
                        wgs19.uc012 = entity.uc012;
                        if (2 == entity.uc008)
                        {
                            wgs19.uc003 = 0;
                        }
                        try
                        {
                            if (1 == entity.uc008)
                            {
                            }
                            db.Repositorywgs019.Update(wgs19);
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
        public List<DBModel.wgs051> GetStockList(int userID, DateTime? dts, DateTime? dte, int pageIndex, int pageSize, out int recordCount)
        {
            List<DBModel.wgs051> list = null;
            Expression<Func<DBModel.wgs051, bool>> query = PredicateExtensionses.True<DBModel.wgs051>();
            if (0 < userID)
            {
                query = query.And(exp => exp.us002 == userID);
            }
            if (dts.HasValue && dte.HasValue)
            {
                query = query.And(exp => exp.us011 >= dts && exp.us012 <=dte);
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCount = db.Repositorywgs051.Count(query);
                    list = db.Repositorywgs051.IQueryable(query, order => order.OrderByDescending(exp => exp.us010)).Skip(pageSize * pageIndex).Take(pageSize).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs021> GetDataChangeList(int userID, int type, long key, string account, DateTime? dts, DateTime? dte, int pageSize, int pageIndex, out int recordCout)
        {
            recordCout = 0;
            List<DBModel.wgs021> list = null;
            Expression<Func<DBModel.wgs021, bool>> query = PredicateExtensionses.True<DBModel.wgs021>();
            if (0 != userID)
            {
                query = query.And(exp => exp.u001== userID);
            }
            if (!_NWC.GeneralValidate.IsNullOrEmpty(account))
            {
                query = query.And(exp=>exp.u002 == account);
            }
            if (null != dts && null != dte)
            {
                query = query.And(exp => exp.uxf014 >= dts && exp.uxf014 <= dte);
            }
            if (0 == type)
            {
                query = query.And(exp => exp.uxf016 > type);
            }
            else
            {
                query = query.And(exp => exp.uxf016 == type);
            }
            if (0 < key)
            {
                query = query.And(exp=>exp.uxf001 == key);
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCout = db.Repositorywgs021.Count(query);
                    list = db.Repositorywgs021.IQueryable(query).OrderByDescending(exp => exp.uxf001).Skip(pageIndex *pageSize).Take(pageSize).ToList();
                }
            }
            return list;
        }
        public MR AddWithdrawType(DBModel.wgs024 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs024.Add(entity);
                    db.SaveChanges();
                    ClearWithdrawTypeListCache();
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
        public MR UpdateWithdrawType(DBModel.wgs024 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs024.Update(entity);
                    db.SaveChanges();
                    ClearWithdrawTypeListCache();
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
        public List<DBModel.wgs024> GetWithdrawTypeList()
        {
            List<DBModel.wgs024> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                if (null == list)
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        list = db.Repositorywgs024.GetAll().OrderBy(exp => exp.uwt006).ToList();
                    }
                }
            }
            return list;
        }
        public List<DBModel.wgs024> GetWithdrawTypeListByCache()
        {
            List<DBModel.wgs024> list = (List<DBModel.wgs024>)_NWC.GeneralCache.Get(withdrawTypeListCacheKey);
            if (null == list)
            {
                list = GetWithdrawTypeList();
                _NWC.GeneralCache.Set(withdrawTypeListCacheKey, list);
            }
            return list;
        }
        public void ClearWithdrawTypeListCache()
        {
            _NWC.GeneralCache.Clear(withdrawTypeListCacheKey);
        }
        public DBModel.wgs024 GetWithdrawType(int key)
        {
            DBModel.wgs024 entity = null;
            using (UnitOfWork db = new UnitOfWork(true))
            {
                entity = db.Repositorywgs024.GetByPrimaryKey(key);
            }
            return entity;
        }
        public MR AddWCashBank(DBModel.wgs023 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    entity.uwi003 = _NWC.DEncrypt.Encrypt(entity.uwi003);
                    entity.uwi004 = _NWC.DEncrypt.Encrypt(entity.uwi004);
                    entity.uwi005 = _NWC.DEncrypt.Encrypt(entity.uwi005);
                    entity.uwi006 = _NWC.DEncrypt.Encrypt(entity.uwi006);
                    entity.uwi010 = DateTime.Now;
                    db.Repositorywgs023.Add(entity);
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
        public MR UpdateWCashBank(DBModel.wgs023 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    entity.uwi003 = _NWC.DEncrypt.Encrypt(entity.uwi003);
                    entity.uwi004 = _NWC.DEncrypt.Encrypt(entity.uwi004);
                    entity.uwi005 = _NWC.DEncrypt.Encrypt(entity.uwi005);
                    entity.uwi006 = _NWC.DEncrypt.Encrypt(entity.uwi006);
                    db.Repositorywgs023.Update(entity);
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
        public List<DBModel.wgs023> GetWCashBankList(int userID)
        {
            List<DBModel.wgs023> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs023.IQueryable(exp => exp.u001 == userID).ToList();
                }
            }
            List<DBModel.wgs023> newList = null;
            if ( 0 < list.Count())
            {
                newList = new List<DBModel.wgs023>();
            }
            foreach (var item in list)
            {
                item.uwi003 = _NWC.DEncrypt.Decrypt(item.uwi003);
                item.uwi004 = _NWC.DEncrypt.Decrypt(item.uwi004);
                item.uwi005 = _NWC.DEncrypt.Decrypt(item.uwi005);
                item.uwi006 = _NWC.DEncrypt.Decrypt(item.uwi006);
                newList.Add(item);
            }
            return list;
        }
        public int GETWCashBankCount(int userID, int bankID, string name)
        {
            throw new NotImplementedException();
        }
        public int GETWCashBankCount(int userID, int bankID)
        {
            throw new NotImplementedException();
        }
        public int GETWCashBankCount(string cardNumber)
        {
            int count = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    cardNumber = _NWC.DEncrypt.Encrypt(cardNumber);
                    count = db.Repositorywgs023.Count(exp=>exp.uwi005 == cardNumber);
                }
            }
            return count;
        }
        public decimal? ChargeSum(int userID, string account, int status, int type, DateTime? dts, DateTime? dte)
        {
            decimal? sum = 0m;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    Expression<Func<DBModel.wgs019, bool>> query = PredicateExtensionses.True<DBModel.wgs019>();
                    if (-1 != status)
                    {
                        query = query.And(exp => exp.uc008 == status);
                    }
                    else
                    {
                        query = query.And(exp => exp.uc008 > status);
                    }
                    if (0 < userID)
                    {
                        query = query.And(exp => exp.u001 == userID);
                    }
                    if (!_NWC.GeneralValidate.IsNullOrEmpty(account))
                    {
                        query = query.And(exp => exp.u002 == account);
                    }
                    if (null != dts && null != dte)
                    {
                        DateTime dtsn = DateTime.Parse(dts.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                        DateTime dten = DateTime.Parse(dte.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                        query = query.And(exp => exp.uc006 <= dtsn && exp.uc006 >= dten);
                    }
                    switch( type)
                    {
                        case 0:
                            sum = db.Repositorywgs019.IQueryable(query).Sum(exp => (decimal?)exp.uc002);
                            break;
                        case 1:
                            sum = db.Repositorywgs019.IQueryable(query).Sum(exp => (decimal?)exp.uc003);
                            break;
                    }
                }
            }
            return sum;
        }
        public List<DBModel.wgs023> GetWCashBankProContent(List<DBModel.wgs023> oriList)
        {
            List<DBModel.wgs023> list = oriList.ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                list[i].uwi003 = list[i].uwi003.Substring(0, 1) + "***";
                list[i].uwi004 = list[i].uwi004.Substring(0, 1) + "***";
                list[i].uwi005 = list[i].uwi005.Substring(0, 3) + "***" + list[i].uwi005.Substring(list[i].uwi005.Length - 4, 4);
                list[i].uwi006 = list[i].uwi006.Substring(0, 1) + "***";
            }
            return list;
        }
        public MR AddWCData(int userID, int wcType, decimal amount, string ip)
        {
            MR mr = new MR();
            DateTime now = DateTime.Now; 
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DBModel.wgs023 uwiEntity = null;
            DBModel.wgs024 uwtEntity = null;
            DBModel.wgs027 config = null;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                DateTime dts = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd " + "00:00:00"));
                DateTime dte = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd " + "23:59:59"));
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    config = db.Repositorywgs027.GetByPrimaryKey("AGU_BANK_GET_COUNT");
                    int todayCount = db.Repositorywgs020.Count(exp => exp.u001 == userID && exp.uw004 >= dts && exp.uw004 <=dte && exp.uw006 == 1);
                    if (todayCount >= int.Parse(config.cfg003))
                    {
                        mr.Code = 0;
                        mr.Message = config.cfg004;
                        return mr;
                    }
                    uwiEntity = db.Repositorywgs023.GetByPrimaryKey(wcType);
                    if (null == uwiEntity)
                    {
                        mr.Message = "提现信息不存在";
                        return mr;
                    }
                    else
                    {
                        uwtEntity = db.Repositorywgs024.GetByPrimaryKey(uwiEntity.uwt001);
                        if (null == uwtEntity)
                        {
                            mr.Message = "提现信息存在，但提现类型不存在";
                            return mr;
                        }
                        else
                        {
                            if (0 == uwtEntity.uwt004)
                            {
                                mr.Message = "提现类型已经禁用";
                            }
                        }
                    }
                }
            }
            transactionOptions.IsolationLevel = IsolationLevel.RepeatableRead;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var userItem = db.Repositorywgs012.IQueryable(exp => exp.u001 == userID, null, "wgs014").FirstOrDefault();
                    if (0 > userItem.wgs014.uf001 - amount)
                    {
                        mr.Message = "余额不足";
                        return mr;
                    }
                    DBModel.wgs020 wcRecord = new DBModel.wgs020();
                    wcRecord.u001 = userItem.u001;
                    wcRecord.u002 = userItem.u002.Trim();
                    wcRecord.u003 = _NWC.GeneralValidate.IsNullOrEmpty(userItem.u003) ? "" : userItem.u003.Trim();
                    wcRecord.uw002 = amount;
                    wcRecord.uw003 = userItem.wgs014.uf003;
                    wcRecord.uw004 = now;
                    wcRecord.uwi001 = wcType;
                    wcRecord.uw006 = 0;
                    wcRecord.uw007 = ip;
                    userItem.wgs014.uf001 -= amount;
                    userItem.wgs014.uf003 += amount;
                    /*
                     * 附加信息
                     * 银行卡资料，经过加密
                     */
                    wcRecord.uw009 = uwtEntity.uwt003;
                    wcRecord.uw010 = uwiEntity.uwi004;
                    wcRecord.uw011 = uwiEntity.uwi005;
                    wcRecord.uw012 = uwiEntity.uwi003;
                    wcRecord.uw017 = uwiEntity.uwi006;
                    try
                    {
                        db.Repositorywgs020.Add(wcRecord);
                        db.SaveChanges();
                        ts.Complete();
                        mr.Code = 1;
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
        public List<DBModel.wgs020> GetWCDataList(int type, int userID, string userAccount, long key, int amtT, decimal amtTV, int amtTH, decimal amtTHV, int status, DateTime? dts, DateTime? dte, int pageSize, int pageIndex, out int recordCount)
        {
            recordCount = 0;
            List<DBModel.wgs020> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            Expression<Func<DBModel.wgs020, bool>> query = PredicateExtensionses.True<DBModel.wgs020>();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                if (0 < type)
                {
                    query = query.And(exp=>exp.uw006 == type);
                }
                if (0 != userID)
                {
                    query = query.And(exp => exp.u001 == userID);
                }
                if (!string.IsNullOrEmpty(userAccount))
                {
                    query = query.And(exp => exp.u002 == userAccount);
                }
                if (dts.HasValue && dte.HasValue)
                {
                    query = query.And(exp => exp.uw004 >= dts.Value && exp.uw004 <= dte.Value);
                }
                if (-1 != status)
                {
                    query = query.And(exp => exp.uw006 == status);
                }
                if (amtT != 0)
                {
                    switch (amtT)
                    {
                        case 1:
                            query = query.And(exp => exp.uw002 < amtTV);
                            break;
                        case 2:
                            query = query.And(exp => exp.uw002 == amtTV);
                            break;
                        case 3:
                            query = query.And(exp => exp.uw002 > amtTV);
                            break;
                    }
                }
                if (amtTH != 0)
                {
                    switch (amtTH)
                    {
                        case 1:
                            query = query.And(exp => exp.uw016 < amtTHV);
                            break;
                        case 2:
                            query = query.And(exp => exp.uw016 == amtTHV);
                            break;
                        case 3:
                            query = query.And(exp => exp.uw016 > amtTHV);
                            break;
                    }
                }
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCount = db.Repositorywgs020.Count(query);
                    list = db.Repositorywgs020.IQueryable(query, order => order.OrderByDescending(exp => exp.uw004)).Skip(pageIndex*pageSize).Take(pageSize).ToList();
                }
            }
            return list;
        }
        public DBModel.wgs020 GetWCData(int key)
        {
            DBModel.wgs020 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs020.GetByPrimaryKey(key);
                }
            }
            return entity;
        }
        public MR SetWCData(int key, int status, decimal money, decimal fee, int mgID, int agID, string comment, string mySendBank, string MySendAccount, string MySendOrderID)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            DateTime now = DateTime.Now;
            int wcCount = 0;
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DBModel.wgs012 aguItem = null;
            DBModel.wgs016 mguItem = null;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    wcCount = db.Repositorywgs020.Count(exp => exp.uw001 == key);
                    aguItem = db.Repositorywgs012.GetByPrimaryKey(agID);
                    mguItem = db.Repositorywgs016.GetByPrimaryKey(mgID);
                }
            }
            if( 0 == wcCount)
            {
                mr.Message = "提现信息不存在";
                return mr;
            }
            if (null == aguItem)
            {
                mr.Message = "会员不存在";
                return mr;
            }
            if (null == mguItem)
            {
                mr.Message = "管理不存在";
                return mr;
            }
            transactionOptions.IsolationLevel = IsolationLevel.RepeatableRead;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var uf = db.Repositorywgs014.GetByPrimaryKey(agID);
                    var wcSaveData = db.Repositorywgs020.GetByPrimaryKey(key);
                    wcSaveData.uw016 = fee;
                    var dataChange = new DBModel.wgs021();
                    dataChange.u001 = aguItem.u001;
                    dataChange.u002 = aguItem.u002.Trim();
                    dataChange.u003 = _NWC.GeneralValidate.IsNullOrEmpty(aguItem.u003) ? null : aguItem.u003.Trim();
                    dataChange.uw001 = wcSaveData.uw001;
                    wcSaveData.mu001 = mguItem.mu001;
                    wcSaveData.mu002 = _NWC.DEncrypt.Decrypt(mguItem.mu002).Trim();
                    dataChange.uxf002 = uf.uf001;
                    dataChange.uxf003 = wcSaveData.uw002;
                    dataChange.uxf014 = now;
                    dataChange.uxf004 = uf.uf004 ;
                    dataChange.uxf005 = 0;
                    if (status == wcSaveData.uw006)
                    {
                        mr.Message = "状态没有更新";
                        return mr;
                    }
                    wcSaveData.uw006 = status;
                    if (2 == status)
                    {
                        uf.uf001 += wcSaveData.uw002;
                        uf.uf003 -= wcSaveData.uw002;
                        dataChange.uxf015 = "取消提现";
                    }
                    else if (1 == status)
                    {
                        uf.uf003 -= wcSaveData.uw002;
                        dataChange.uxf015 = "提现成功";
                    }
                    dataChange.uxf007 = uf.uf001;
                    dataChange.uxf008 = uf.uf004;
                    if (false == _NWC.GeneralValidate.IsNullOrEmpty(comment))
                    {
                        wcSaveData.uw008 = comment;
                    }
                    wcSaveData.uw005 = now;
                    try
                    {
                        db.Repositorywgs020.Update(wcSaveData);
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
        public MR TransferMoney(int myUserID, int toUserID, decimal amount, string ip, long port)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var myUser = db.Repositorywgs012.IQueryable(exp => exp.u001 == myUserID, null, "wgs014").Take(1).FirstOrDefault();
                    var toUser = db.Repositorywgs012.IQueryable(exp => exp.u001 == toUserID, null, "wgs014").Take(1).FirstOrDefault();
                    DBModel.wgs043 entity = new DBModel.wgs043();
                    entity.tf002 = myUser.u001;
                    entity.tf003 = myUser.u002.Trim();
                    entity.tf004 = myUser.u003 == null ? null : myUser.u003.Trim();
                    entity.tf005 = toUser.u001;
                    entity.tf006 = toUser.u002.Trim();
                    entity.tf007 = toUser.u003 == null ? null : toUser.u003.Trim();
                    entity.tf008 = amount;
                    entity.tf009 = DateTime.Now;
                    entity.tf010 = ip;
                    entity.tf011 = port;
                    try
                    {
                        db.Repositorywgs043.Add(entity);
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
        public List<DBModel.wgs043> GetTransferList(long key, int myUserID, string myUserName, int toUserID, string toUserName, int type, int amtT, decimal amtV, DateTime? dts, DateTime? dte, int pageIndex, int pageSize, out int recordCount)
        {
            List<DBModel.wgs043> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            Expression<Func<DBModel.wgs043, bool>> query = PredicateExtensionses.True<DBModel.wgs043>();
            if (0 != myUserID)
            {
                query = query.And(exp=>exp.tf002 == myUserID || exp.tf005 == myUserID);
            }
            if (false == string.IsNullOrEmpty(myUserName))
            {
                query = query.And(exp => exp.tf003 == myUserName || exp.tf006 == myUserName);
            }
            if (0 != toUserID)
            {
                query = query.And(exp => exp.tf005 == myUserID);
            }
            if (false == string.IsNullOrEmpty(toUserName))
            {
                query = query.And(exp => exp.tf006 == toUserName);
            }
            if (0 < type)
            {
                if (1 == type)
                {
                    query = query.And(exp=>exp.tf002 == myUserID);
                }
                else if (2 == type)
                {
                    query = query.And(exp => exp.tf005 == myUserID);
                }
            }
            if (dts.HasValue && dte.HasValue)
            {
                query = query.And(exp=>exp.tf009 >= dts && exp.tf009 <= dte);
            }
            switch (amtT)
            {
                case 1:
                    query = query.And(exp => exp.tf008 > amtV);
                    break;
                case 2:
                    query = query.And(exp => exp.tf008 == amtV);
                    break;
                case 3:
                    query = query.And(exp => exp.tf008 < amtV);
                    break;
            }
            if (0 < key)
            {
                query = query.And(exp=>exp.tf001 == key);
            }
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCount = db.Repositorywgs043.Count(query);
                    list = db.Repositorywgs043.IQueryable(query, order => order.OrderByDescending(exp => exp.tf009)).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                }
            }
            return list;
        }
        public MR SetTransferDone(long key, int type, int mu001, string mu002, string comment)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var oriItem = db.Repositorywgs043.GetByPrimaryKey(key);
                    if (null == oriItem)
                    {
                        mr.Message = "信息不存在";
                        return mr;
                    }
                    if (oriItem.tf012 > 0)
                    {
                        mr.Message = "不允许修改";
                        return mr;
                    }
                    if (oriItem.tf012 == type)
                    {
                        mr.Message = "状态没有变化";
                        return mr;
                    }
                    oriItem.tf012 = (byte)type;
                    oriItem.mu001 = mu001;
                    oriItem.mu002 = mu002;
                    if (false == string.IsNullOrEmpty(comment))
                    {
                        oriItem.tf013 = comment;
                    }
                    oriItem.tf014 = DateTime.Now;
                    try
                    {
                        db.Repositorywgs043.Update(oriItem);
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
        public DBModel.wgs021 GetDataChange(long key)
        {
            DBModel.wgs021 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs021.GetByPrimaryKey(key);
                }
            }
            return entity;
        }
        public DBModel.wgs042 GetDayReport(int userID, DateTime? dt)
        {
            DBModel.wgs042 entity = new DBModel.wgs042();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            Expression<Func<DBModel.wgs042, bool>> query = PredicateExtensionses.True<DBModel.wgs042>();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    query = query.And(exp=>exp.u001 == userID);
                    DateTime dtq = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                    if (true == dt.HasValue)
                    {
                        dtq = DateTime.Parse(dt.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                    }
                    query = query.And(exp => exp.dr002 == dtq);
                    entity = db.Repositorywgs042.IQueryable(query, order=>order.OrderByDescending(exp=>exp.dr002).ThenByDescending(exp=>exp.dr003)).Take(1).FirstOrDefault();
                }
            }
            return entity;
        }
        public List<DBModel.wgs042> GetDayReport(int myUserID, string account, int userType, DateTime? dts, DateTime? dte)
        {
            List<DBModel.wgs042> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            Expression<Func<DBModel.wgs042, bool>> query = PredicateExtensionses.True<DBModel.wgs042>();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                if (dte.HasValue && dts.HasValue)
                {
                    dts = DateTime.Parse(dts.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                    dte = DateTime.Parse(dte.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                }
                else
                {
                    dts = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                    dte = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");
                }
                query = query.And(exp=>exp.dr002 >= dts && exp.dr002<=dte);
                if (0 < myUserID)
                {
                    query = query.And(exp=>exp.u001 == myUserID);
                }
                if (false == string.IsNullOrEmpty(account))
                {
                    query = query.And(exp=>exp.u002 == account);
                }
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs042.IQueryable(query, order=>order.OrderByDescending(exp=>exp.dr003)).ToList();
                }
            }
            return list;
        }
        public MR AddDataChange(int userID, int type, object id, string comment)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.RepeatableRead;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var userInfo = db.Repositorywgs012.IQueryable(exp => exp.u001 == userID, null, "wgs014").FirstOrDefault();
                    DBModel.wgs021 entity = new DBModel.wgs021();
                    if (2 == type)
                    {
                    }
                    try
                    {
                        ts.Complete();
                        mr.Code = 1;
                    }
                    catch (Exception error)
                    {
                        mr.Exception = MyException.GetInnerException(error);
                        mr.Message = mr.Exception.Message;
                    }
                }
            }
            throw new NotImplementedException();
        }
        public MR CancelCharege(List<long> ids,DBModel.wgs016 controlUser)
        { 
          MR mr = new MR();
          mr.Code = 1;
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew))
          {
              using (UnitOfWork db = new UnitOfWork())
              {
                  try
                  {
                      List<DBModel.wgs019> oldEntity = db.Repositorywgs019.Get(p => ids.Contains(p.uc001) && p.uc008 == 0).ToList();
                      for (int i = 0; i < oldEntity.Count; i++)
                     {
                         oldEntity[i].uc012 = "";
                         oldEntity[i].uc013 = 0;
                         oldEntity[i].uc003 = 0;
                         oldEntity[i].mu001x = controlUser.mu001;
                         oldEntity[i].mu002x = controlUser.mu002;
                         oldEntity[i].uc008 = 2;
                     }
                      db.Repositorywgs019.UpdateList(oldEntity);
                     db.SaveChanges();
                  }
                  catch (Exception e)
                  {
                      mr.Message=e.Message;
                     mr.Code=0;
                     return mr;
                  }
              }
          }
          mr.Message = "成功";
          return mr;
        }
    }
}
