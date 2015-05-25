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
        public MR SendPointToUser(int userID, decimal point)
        {
            ISystem sys = new System();
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        if (0 >= point)
                        {
                            mr.Message = "积分应大于等于0";
                            return mr;
                        }
                        var toUser = db.Repositorywgs049.GetByPrimaryKey(userID);
                        if (null == toUser)
                        {
                            mr.Message = "账号不存在";
                            return mr;
                        }
                        DBModel.wgs014 uf = db.Repositorywgs014.GetByPrimaryKey(toUser.u001);
                        decimal oldPoint = uf.uf004;
                        decimal newPoint = oldPoint + point;
                        uf.uf004 += point;
                        DBModel.wgs021 wgs021 = new DBModel.wgs021();
                        wgs021.u001 = toUser.u001;
                        wgs021.u002 = toUser.u002.Trim();
                        wgs021.u003 = string.IsNullOrEmpty(toUser.u003) ? string.Empty : toUser.u003.Trim();
                        wgs021.uxf002 = uf.uf001;
                        wgs021.uxf004 = oldPoint;
                        wgs021.uxf005 = point;
                        wgs021.uxf007 = uf.uf001;
                        wgs021.uxf008 = newPoint;
                        wgs021.uxf009 = uf.uf003;
                        wgs021.uxf010 = uf.uf003;
                        wgs021.uxf014 = DateTime.Now;
                        wgs021.uxf016 = 37;
                        wgs021.uxf015 = sys.GetSystemDataChangeTypeList(true)[37].Name;
                        DateTime dtq = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        var existsDRData = db.Repositorywgs042.IQueryable(exp => exp.dr002 == dtq && exp.u001 == toUser.u001).Take(1).FirstOrDefault();
                        if (null == existsDRData)
                        {
                            DBModel.wgs042 drData = new DBModel.wgs042();
                            drData.u001 = toUser.u001;
                            drData.u002 = toUser.u002;
                            drData.u003 = string.IsNullOrEmpty(toUser.u003) ? string.Empty : toUser.u003.Trim();
                            drData.dr008 = point;
                            drData.dr002 = dtq;
                            drData.dr003 = DateTime.Now;
                            db.Repositorywgs042.Add(drData);
                            db.SaveChanges();
                        }
                        else
                        {
                            existsDRData.dr008 += point;
                            existsDRData.dr003 = DateTime.Now;
                            db.Repositorywgs042.Update(existsDRData);
                            db.SaveChanges();
                        }
                        db.Repositorywgs021.Add(wgs021);
                        db.SaveChanges();
                        db.Repositorywgs014.Update(uf);
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
        public MR SendFrozenSumToUser(int userID, decimal frozenSum, string remarks)
        {
            ISystem sys = new System();
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        if (0 >= frozenSum)
                        {
                            mr.Message = "冻结金额应大于等于0";
                            return mr;
                        }
                        if (string.IsNullOrEmpty(remarks))
                        {
                            mr.Message = "请填写备注";
                            return mr;
                        }
                        var toUser = db.Repositorywgs049.GetByPrimaryKey(userID);
                        if (null == toUser)
                        {
                            mr.Message = "账号不存在";
                            return mr;
                        }
                        DBModel.wgs014 uf = db.Repositorywgs014.GetByPrimaryKey(toUser.u001);
                        
                        decimal oldFrozenSum = uf.uf003;
                        decimal newFrozenSum = oldFrozenSum + frozenSum;
                        uf.uf003 += frozenSum;
                        if (uf.uf001 - frozenSum < 0)
                        {
                            mr.Message = "冻结金额应小于等可用金额";
                            return mr;
                        }
                        
                        DBModel.wgs021 wgs021 = new DBModel.wgs021();
                        wgs021.u001 = toUser.u001;
                        wgs021.u002 = toUser.u002.Trim();
                        wgs021.u003 = string.IsNullOrEmpty(toUser.u003) ? string.Empty : toUser.u003.Trim();
                        wgs021.uxf002 = uf.uf001;
                        wgs021.uxf004 = uf.uf004;
                        wgs021.uxf005 = uf.uf004;
                        wgs021.uxf007 = uf.uf001 - frozenSum;
                        wgs021.uxf008 = uf.uf004;
                        wgs021.uxf009 = newFrozenSum;
                        wgs021.uxf010 = oldFrozenSum;
                        wgs021.uxf012 = frozenSum;
                        wgs021.uxf014 = DateTime.Now;
                        wgs021.uxf016 = 38;
                        wgs021.uxf015 = sys.GetSystemDataChangeTypeList(true)[38].Name;
                        wgs021.uxf017 = remarks;
                        DateTime dtq = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                        var existsDRData = db.Repositorywgs042.IQueryable(exp => exp.dr002 == dtq && exp.u001 == toUser.u001).Take(1).FirstOrDefault();
                        if (null == existsDRData)
                        {
                            DBModel.wgs042 drData = new DBModel.wgs042();
                            drData.u001 = toUser.u001;
                            drData.u002 = toUser.u002;
                            drData.u003 = string.IsNullOrEmpty(toUser.u003) ? string.Empty : toUser.u003.Trim();
                            drData.dr020 = frozenSum;
                            drData.dr002 = dtq;
                            drData.dr003 = DateTime.Now;
                            db.Repositorywgs042.Add(drData);
                            db.SaveChanges();
                        }
                        else
                        {
                            existsDRData.dr020 += frozenSum;
                            existsDRData.dr003 = DateTime.Now;
                            db.Repositorywgs042.Update(existsDRData);
                            db.SaveChanges();
                        }
                     
                        DBModel.wgs044 wgs044 = new DBModel.wgs044();
                        wgs044.msg002 = "冻结金额";
                        wgs044.msg003 = "冻结金额，金额：" + frozenSum + "已经冻结。原因：" + remarks;
                        wgs044.msg004 = 0;
                        wgs044.msg005 = wgs021.u001;
                        wgs044.msg006 = DateTime.Now;
                        wgs044.msg007 = 0;
                        wgs044.msg008 = "-";
                        wgs044.msg009 = wgs021.u002;
                        wgs044.msg010 = "-";
                        wgs044.msg011 = wgs021.u003;
                        wgs044.msg012 = 0;
                        db.Repositorywgs044.Add(wgs044);
                        db.SaveChanges();
                        db.Repositorywgs021.Add(wgs021);
                        db.SaveChanges();
                        uf.uf001 = uf.uf001 - frozenSum;
                        db.Repositorywgs014.Update(uf);
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
        public MR DeleteWCashBank(int userID)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs023.DeleteList(db.Repositorywgs023.IQueryable(exp=>exp.u001 == userID).ToList());
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
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
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
            int chargeCount = 0;
            mr.Code = 1;
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
            if( chargeCount >= int.Parse(config.cfg003) )
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
                        entity.uc005 = _NWC.RandomString.Get("1,2,3,4,5,6,7,8,9,0", 10);
                        if ("0" == entity.uc005.Substring(0, 1))
                        {
                            exists = 1;
                        }
                        else
                        { 
                            exists = db.Repositorywgs019.Count(exp => exp.uc005 == entity.uc005);
                        }
                    } while (0 < exists);
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
                    if (!string.IsNullOrEmpty(account))
                    {
                        query = query.And(exp => exp.u002 == account);
                    }
                    if (0 != amount)
                    {
                        query = query.And(exp => exp.uc002 == amount);
                    }
                    if (null != dts && null != dte)
                    {
                        query = query.And(exp => exp.uc006 >= dts && exp.uc006 <= dte);
                    }
                    count = db.Repositorywgs019.Count(query);
                }
            }
            return count;
        }
        public int CheckWCashCount(int status, int userID, string account, decimal amount, DateTime? dts, DateTime? dte)
        {
            int count = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    Expression<Func<DBModel.wgs020, bool>> query = PredicateExtensionses.True<DBModel.wgs020>();
                    if (-1 != status)
                    {
                        query = query.And(exp => exp.uw006 == status);
                    }
                    else
                    {
                        query = query.And(exp => exp.uw006 != status);
                    }
                    if (0 < userID)
                    {
                        query = query.And(exp => exp.u001 == userID);
                    }
                    if (!string.IsNullOrEmpty(account))
                    {
                        query = query.And(exp => exp.u002 == account);
                    }
                    if (0 < amount)
                    {
                        query = query.And(exp => exp.uw002 == amount);
                    }
                    if (null != dts && null != dte)
                    {
                        query = query.And(exp => exp.uw004 >= dts && exp.uw004 <= dte);
                    }
                    count = db.Repositorywgs020.Count(query);
                }
            }
            return count;
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
        public MR UpdateCCashNY(long key, int nyKey)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var old = db.Repositorywgs019.GetByPrimaryKey(key);
                    old.mu002 = nyKey.ToString();
                    try
                    {
                        db.Repositorywgs019.Update(old);
                        db.SaveChanges();
                        ts.Complete();
                        mr.Code = 1;
                    }
                    catch (Exception error)
                    {
                        mr.Message = error.Message;
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
        public MR AddWCData(int userID, int wcType, int moneyType, decimal amount, string ip, int isfh)
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
                    int todayCount = db.Repositorywgs020.Count(exp => exp.u001 == userID && exp.uw004 >= dts && exp.uw004 <= dte && exp.uw006 == 1);
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
                    if (isfh == 1)
                    {
                        if (0 > userItem.wgs014.uf013 - amount)
                        {
                            mr.Message = "分红余额不足";
                            return mr;
                        }
                    }
                    else
                    {
                        if (moneyType == 0)
                        {
                            if (0 > userItem.wgs014.uf001 - amount)
                            {
                                mr.Message = "余额不足";
                                return mr;
                            }
                        }
                    }
                    DBModel.wgs020 wcRecord = new DBModel.wgs020();
                    DBModel.wgs012 R012 = new DBModel.wgs012();

                    wcRecord.u001 = userItem.u001;
                    wcRecord.u002 = userItem.u002.Trim();
                    wcRecord.u003 = _NWC.GeneralValidate.IsNullOrEmpty(userItem.u003) ? "" : userItem.u003.Trim();
                    wcRecord.uw002 = amount;
                    wcRecord.uw003 = userItem.wgs014.uf003;
                    wcRecord.uw004 = now;
                    wcRecord.uwi001 = wcType;
                    wcRecord.uw006 = 0;
                    wcRecord.uw007 = ip;
                    wcRecord.uw018 = moneyType;



                    if (isfh == 0)
                    {
                        userItem.wgs014.uf001 -= amount;
                        userItem.wgs014.uf003 += amount;

                    }
                    else
                    {
                        wcRecord.uw008 = "分红提现";

                        
                        //userItem.wgs014.uf013 -= amount;
                        //userItem.wgs014.uf014 += amount;
                    }
                    /*
                     * 附加信息
                     * 银行卡资料，经过加密
                     */
                    wcRecord.uw009 = uwtEntity.uwt003;
                    wcRecord.uw010 = uwiEntity.uwi004;
                    wcRecord.uw011 = uwiEntity.uwi005;
                    wcRecord.uw012 = uwiEntity.uwi003;
                    wcRecord.uw017 = uwiEntity.uwi006;

                    if (isfh == 1)
                    {
                        wcRecord.uw018 = 2; 
                    }


                    try
                    {


                        //if (isfh == 1)
                        //{
                        //    //User_AM ua = new User_AM();
                        //    string sql = " update wgs014 set uf013 = uf013 - " + amount.ToString() + ", uf014 = uf014 + " + amount.ToString() + " where u001 = " + userItem.u001;
                        //    //ua.EXEC(sql);
                        //    db.ExecuteSqlCommand(sql);
                        //}


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
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                if (key <= 0)
                {
                    if (0 < type)
                    {
                        query = query.And(exp => exp.uw006 == type);
                    }
                    if (0 < userID)
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
                }
                else
                {
                    query = query.And(exp=>exp.uw001 == key);
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
        public List<DBModel.ChargeReport> GetChargeReport(int status, DateTime? dt, DateTime? dte)
        {
            List<DBModel.ChargeReport> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.SqlQuery<DBModel.ChargeReport>("SELECT ct001,SUM(uc003) AS uc003, '-' AS ChargeTypeName  FROM wgs019 WHERE uc008={2} AND uc006>= {0} AND uc006 <={1}  GROUP BY ct001 ORDER BY uc003", dt, dte, status).ToList();
                }
            }
            if (0 < list.Count)
            {
                var ctList = GetCTListByCache();
                var sbList = GetBankListByCache();
                for (int i = 0; i < list.Count(); i++)
                {
                    var sb001 = ctList.Where(exp => exp.ct001 == list[i].ct001).FirstOrDefault().sb001;
                    var sbName = sbList.Where(exp => exp.sb001 == sb001).FirstOrDefault().sb003;
                    list[i].ChargeTypeName = sbName;
                }
            }
            return list;
        }
        public List<DBModel.WithDrawReport> GetWithDrawReport(int status, DateTime? dt, DateTime? dte)
        {
            List<DBModel.WithDrawReport> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.SqlQuery<DBModel.WithDrawReport>("SELECT uw009 AS Bank,SUM(uw002 + uw016) AS SumMoney, '-' AS ChargeTypeName  FROM wgs020 WHERE uw006={2} AND uw004 >={0} AND uw004 <={1}  GROUP BY uw009 ORDER BY SumMoney", dt, dte, status).ToList();
                }
            }
            return list;
        }
        public List<DBModel.GameReport> GetGameReport(DateTime? dt, DateTime? dte)
        {
            List<DBModel.GameReport> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.SqlQuery<DBModel.GameReport>("SELECT COUNT(1) AS [Count],g001,SUM(so004) BetAmount,SUM(so010) AS WinAmount, SUM(so018) AS Point FROM wgs045 WHERE so016=1 AND so007>={0} AND so007<={1} GROUP BY g001 ORDER BY BetAmount",dt, dte).ToList();
                }
            }
            return list;
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
                        if (wcSaveData.uw018 == 1)
                        {
                            uf.uf001 += wcSaveData.uw002;
                            uf.uf003 -= wcSaveData.uw002;
                        }
                        if (wcSaveData.uw018 == 2)
                        {
                            //User_AM ua = new User_AM();
                            //string sql = " update wgs014 set uf013 = uf013 + " + wcSaveData.uw002.ToString() + ", uf014 = uf014 - " + wcSaveData.uw002.ToString() + " where u001 = " + aguItem.u001;
                            //ua.EXEC(sql);
                            //db.ExecuteSqlCommand(sql);
                        }
                        else
                        {
                            uf.uf001 += wcSaveData.uw002;
                            uf.uf003 -= wcSaveData.uw002;
                        }
                        dataChange.uxf015 = "取消提现";
                    }
                    else if (1 == status)
                    {
                       
                        if (wcSaveData.uw018 == 2)
                        {
                            //User_AM ua = new User_AM();
                            //string sql = " update wgs014 set  uf014 = uf014 - " + wcSaveData.uw002.ToString() + " where u001 = " + aguItem.u001;
                            //ua.EXEC(sql);

                            //db.ExecuteSqlCommand(sql);
                        }
                        else
                        {
                            uf.uf003 -= wcSaveData.uw002;
                        }
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
        public List<DBModel.wgs042> GetDRReport(int myUserID, int type, DateTime? dts, DateTime? dte)
        {
            List<DBModel.wgs042> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            Expression<Func<DBModel.wgs042, bool>> query = PredicateExtensionses.True<DBModel.wgs042>();
            List<DBModel.wgs042> resultList = new List<DBModel.wgs042>();
            List<int> userIDs = new List<int>();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    switch (type)
                    {
                        case 0:
                            userIDs = db.Repositorywgs049.IQueryable(exp => exp.u012 == myUserID).Select(exp => exp.u001).ToList();
                            break;
                        case 2:
                            userIDs = db.Repositorywgs048.IQueryable(exp => exp.u001 == myUserID && exp.u002 != myUserID).Select(exp => exp.u002).ToList();
                            break;
                        case 3:
                            var dChilds = db.Repositorywgs049.IQueryable(exp => exp.u012 == myUserID).Select(exp => exp.u001).ToList();
                            userIDs = db.Repositorywgs048.IQueryable(exp => exp.u001 == myUserID && exp.u002 != myUserID && !dChilds.Contains(exp.u001) && !dChilds.Contains(exp.u002) ).Select(exp => exp.u002).ToList();
                            break;
                    }
                    list = db.Repositorywgs042.IQueryable(exp => userIDs.Contains(exp.u001) && exp.dr002 >= dts && exp.dr002 <= dte, ord => ord.OrderBy(exp => exp.u001)).ToList();
                }
            }
            if (list.Count > 0)
            {
                var groupData = from dl in list
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
                foreach (var item in groupData)
                {
                    resultList.Add(new DBModel.wgs042() { u001=item.u001, u002 = item.u002, dr004 = item.dr004, dr005 = item.dr005, dr006 = item.dr006, dr007 = item.dr007, dr008 = item.dr008, dr009 = item.dr009, dr010 = item.dr010, dr011 = item.dr011, dr012 = item.dr012, dr013 = item.dr013, dr014 = item.dr014, dr015 = item.dr015, dr016 = item.dr016, dr017 = item.dr017, dr018 = item.dr018 });
                }
            }
            else
            {
                resultList.Add(new DBModel.wgs042() { u001=0, u002=string.Empty, dr004 = 0, dr005 = 0, dr006 = 0, dr007 = 0, dr008 = 0, dr009 = 0, dr010 = 0, dr011 = 0, dr012 = 0, dr013 = 0, dr014 = 0, dr015 = 0, dr016 = 0, dr017 = 0, dr018 = 0 });
            }
            return resultList;
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
        public List<DBModel.wgs042> GetDRExt(int admin, int myUserID, DateTime? dts, DateTime? dte, int st, string acct, int acctID, string parentAcct, int parentID, int omt, decimal omv, int cmt, decimal cmv, int pmt, decimal pmv)
        {
            List<DBModel.wgs042> drlist = null;
            Expression<Func<DBModel.wgs042, bool>> q42 = PredicateExtensionses.True<DBModel.wgs042>();
            Expression<Func<DBModel.wgs049, bool>> q49 = PredicateExtensionses.True<DBModel.wgs049>();
            Expression<Func<DBModel.wgs048, bool>> q48 = PredicateExtensionses.True<DBModel.wgs048>();
            if (dts.HasValue && dte.HasValue)
            {
                q42 = q42.And(exp=>exp.dr002 >= dts && exp.dr002 <= dte);
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    if (4 == st)
                    {
                        switch (omt)
                        {
                            case 1:
                                q42 = q42.And(exp=>exp.dr004 <= omv);
                                break;
                            case 2:
                                q42 = q42.And(exp => exp.dr004 == omv);
                                break;
                            case 3:
                                q42 = q42.And(exp => exp.dr004 >= omv);
                                break;
                        }
                        switch (cmt)
                        {
                            case 1:
                                q42 = q42.And(exp => exp.dr005 <= cmv);
                                break;
                            case 2:
                                q42 = q42.And(exp => exp.dr005 == cmv);
                                break;
                            case 3:
                                q42 = q42.And(exp => exp.dr005 >= cmv);
                                break;
                        }
                        switch (pmt)
                        {
                            case 1:
                                q42 = q42.And(exp => exp.dr006 <= pmv);
                                break;
                            case 2:
                                q42 = q42.And(exp => exp.dr006 == pmv);
                                break;
                            case 3:
                                q42 = q42.And(exp => exp.dr006 >= pmv);
                                break;
                        }
                        if (0 == admin)
                        {
                            if (0 < parentID && myUserID == parentID && string.IsNullOrEmpty(acct))
                            {
                                q48 = q48.And(exp => exp.u001 == parentID);
                            }
                            else if (false == string.IsNullOrEmpty(acct))
                            {
                                q48 = q48.And(exp => exp.u001 == myUserID && exp.u002n == acct);
                            }
                        }
                        else if( 1 == admin)
                        {
                            if (false == string.IsNullOrEmpty(acct))
                            {
                                q48 = q48.And(exp =>exp.u002n == acct);
                            }
                        }
                        List<int> childIDs = new List<int>();
                        childIDs = db.Repositorywgs048.IQueryable(q48).Select(exp => exp.u002).ToList();
                        if (0 == childIDs.Count)
                        {
                            if (1 == admin && string.IsNullOrEmpty(acct))
                            {
                                childIDs = db.Repositorywgs049.IQueryable(exp => exp.u012 == 0).Select(exp => exp.u001).ToList();
                            }
                            else
                            {
                                return null;
                            }
                        }
                        q42 = q42.And(exp => childIDs.Contains(exp.u001));
                        drlist = db.Repositorywgs042.IQueryable(q42).ToList();
                    }
                    else if (5 == st)
                    {
                        List<int> childUserIDs = new List<int>();
                        var curUser = db.Repositorywgs012.GetByPrimaryKey(myUserID);
                        var count = 0;
                        if (curUser!=null&&curUser.u002.Trim() == parentAcct)
                        {
                            count = db.Repositorywgs048.Count(exp => exp.u001 == myUserID && exp.u002 != myUserID);
                        }
                        else
                        {
                            count = db.Repositorywgs048.Count(exp => exp.u001 == myUserID && exp.u002n == parentAcct);
                        }
                        if (1 == admin && 0 == count)
                        {
                            count = 1;
                        }
                        int myId = 0;
                        if (0 < count)
                        {
                            var curFindUser = db.Repositorywgs049.IQueryable(exp => exp.u002 == parentAcct).Take(1).FirstOrDefault();
                            if (null != curFindUser)
                            {
                                myId = curFindUser.u001;
                                childUserIDs = db.Repositorywgs049.IQueryable(exp => exp.u012 == curFindUser.u001).Select(exp => exp.u001).ToList();
                            }
                        }
                        if (1 == admin && "_report_" == parentAcct)
                        {
                            childUserIDs = db.Repositorywgs049.IQueryable(exp => exp.u012 == 0).Select(exp=>exp.u001).ToList();
                        }
                        drlist = new List<DBModel.wgs042>();

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
                       
                    }
                }
            }
            return drlist;
        }
        public MR CancelCharege(List<long> ids,DBModel.wgs016 controlUser)
        { 
          MR mr = new MR();
          mr.Code = 1;
          using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
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
                         oldEntity[i].uc007 = DateTime.Now; 
                     }
                     db.Repositorywgs019.UpdateList(oldEntity);
                     db.SaveChanges();
                     ts.Complete();
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
        public MR GetExtChargeCode(string code)
        {
            MR mr = new MR();
            DBModel.wgs027 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = global::System.Transactions.IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs027.GetByPrimaryKey("SYS_CHARGE_EXTEND_INFO");
                }
            }
            if (null == entity)
            {
                mr.Message = "配置数据为空";
                return mr;
            }
            try
            {
                global::System.Data.DataSet ds = new global::System.Data.DataSet();
                global::System.Data.SqlClient.SqlDataAdapter da = new global::System.Data.SqlClient.SqlDataAdapter();
                string[] info = entity.cfg003.Trim().Split('@');
                string connectionString = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};password={3};Application Name=\"{4}\"";
                global::System.Data.SqlClient.SqlConnection conn = new global::System.Data.SqlClient.SqlConnection(string.Format(connectionString, info[0], info[3], info[1], info[2], info[4]));
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(1) FROM my18_cz_temp WHERE sjm=@sjm;";
                cmd.Parameters.AddWithValue("@sjm", code);
                var exists = (int)cmd.ExecuteScalar();
                if (0 < exists)
                {
                    mr.Code = 1;
                    return mr;
                }
                else
                {
                    return mr;
                }
            }
            catch (Exception error)
            {
                mr.Message = error.Message;
            }
            return mr;
        }
        public MR GetExtChargeByAccount(string account)
        {
            MR mr = new MR();
            DBModel.wgs027 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = global::System.Transactions.IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs027.GetByPrimaryKey("SYS_CHARGE_EXTEND_INFO");
                }
            }
            if (null == entity)
            {
                mr.Message = "配置数据为空";
                return mr;
            }
            try
            {
                global::System.Data.DataSet ds = new global::System.Data.DataSet();
                global::System.Data.SqlClient.SqlDataAdapter da = new global::System.Data.SqlClient.SqlDataAdapter();
                string[] info = entity.cfg003.Trim().Split('@');
                string connectionString = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};password={3};Application Name=\"{4}\"";
                global::System.Data.SqlClient.SqlConnection conn = new global::System.Data.SqlClient.SqlConnection(string.Format(connectionString, info[0], info[3], info[1], info[2], info[4]));
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM my18_cz_temp WHERE state=1 OR DATEDIFF(n, sj, GETDATE()) >= @max";
                cmd.Parameters.AddWithValue("@max", info[5]);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT TOP 1 * FROM my18_cz_temp WHERE username=@account;";
                cmd.Parameters.AddWithValue("@account", account);
                da.SelectCommand = cmd;
                da.Fill(ds);
                global::System.Data.DataTable dt = ds.Tables[0];
                if (0 == dt.Rows.Count)
                {
                    mr.Message = "不存在充值数据";
                    return mr;
                }
                string content = dt.Rows[0]["username"].ToString() 
                    + "|" + dt.Rows[0]["dkje"].ToString() 
                    + "|" + dt.Rows[0]["sj"].ToString() 
                    + "|" + dt.Rows[0]["sjm"].ToString() 
                    + "|"+dt.Rows[0]["userid"]
                    + "|" + info[5];
                mr.Message = content;
                mr.Code = 1;
            }
            catch (Exception error)
            {
                mr.Message = error.Message;
            }
            return mr;
        }
        public MR SetExtChargeByAccount(int userid, string account, decimal money, string type, string dkkh, string dkrm, string sjm, DateTime sj, string O_id)
        {
            MR mr = new MR();
            DBModel.wgs027 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = global::System.Transactions.IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs027.GetByPrimaryKey("SYS_CHARGE_EXTEND_INFO");
                }
            }
            if (null == entity)
            {
                mr.Message = "配置数据为空";
                return mr;
            }
            try
            {
                string[] info = entity.cfg003.Trim().Split('@');
                string connectionString = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};password={3};Application Name=\"{4}\"";
                global::System.Data.SqlClient.SqlConnection conn = new global::System.Data.SqlClient.SqlConnection(string.Format(connectionString, info[0], info[3], info[1], info[2], info[4]));
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO my18_cz_temp(userid, username, dkkh, dkrm, dkje, dkje1, sj, ptype, state, sjm) VALUES(@userid, @username, @dkkh, @dkrm, @dkje, @dkje1, @sj, @ptype, @state, @sjm);";
                cmd.Parameters.AddWithValue("@userid", O_id);
                cmd.Parameters.AddWithValue("@username", account);
                cmd.Parameters.AddWithValue("@dkkh", dkkh);
                cmd.Parameters.AddWithValue("@dkrm", dkrm);
                cmd.Parameters.AddWithValue("@dkje", money);
                cmd.Parameters.AddWithValue("@dkje1", money);
                cmd.Parameters.AddWithValue("@sj", sj);
                cmd.Parameters.AddWithValue("@ptype", type);
                cmd.Parameters.AddWithValue("@state", 0);
                cmd.Parameters.AddWithValue("@sjm", sjm);
                var insert = (int)cmd.ExecuteNonQuery();
                if (0 < insert)
                {
                    mr.Code = 1;
                }
                else
                {
                    mr.Code = 0;
                }
            }
            catch (Exception error)
            {
                mr.Message = error.Message;
            }
            return mr;
        }
    }
}
