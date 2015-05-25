
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
    public class User : IUser
    {
        public MR AddMGU(DBModel.wgs016 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    if (_NWC.GeneralValidate.IsNullOrEmpty(entity.mu002))
                    {
                        return mr;
                    }
                    entity.mu002 = _NWC.DEncrypt.Encrypt(entity.mu002.Trim());
                    if (false == _NWC.GeneralValidate.IsNullOrEmpty(entity.mu003))
                    {
                        entity.mu003 = _NWC.DEncrypt.Encrypt(entity.mu003.Trim());
                    }
                    entity.mu005 = _NWC.RandomString.Get("", 10);
                    entity.mu004 = _NWC.SHA1.Get(entity.mu004 + entity.mu005, _NWC.SHA1Bit.L160);
                    db.Repositorywgs016.Add(entity);
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
        public DBModel.wgs016 GetMGU(int key)
        {
            DBModel.wgs016 entity = null;
            using (UnitOfWork db = new UnitOfWork(true))
            {
                entity = db.Repositorywgs016.GetByPrimaryKey(key);
                entity.mu002 = _NWC.DEncrypt.Decrypt(entity.mu002);
                entity.mu003 = _NWC.DEncrypt.Decrypt(entity.mu003);
            }
            return entity;
        }
        public DBModel.wgs016 GetMGU(string key)
        {
            DBModel.wgs016 entity = null;
            using (UnitOfWork db = new UnitOfWork(true))
            {
                key = _NWC.DEncrypt.Encrypt(key);
                entity = db.Repositorywgs016.IQueryable(exp => exp.mu002 == key).FirstOrDefault();
                if (null != entity)
                {
                    entity.mu002 = _NWC.DEncrypt.Decrypt(entity.mu002);
                    entity.mu003 = _NWC.DEncrypt.Decrypt(entity.mu003);
                }
            }
            return entity;
        }
        public MR CheckNickName(string nickName)
        {
            MR mr = new MR();
            if (string.IsNullOrEmpty(nickName) || string.IsNullOrWhiteSpace(nickName))
            {
                mr.Message = "不能为空";
                return mr;
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            DBModel.wgs027 config = null;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    config = db.Repositorywgs027.GetByPrimaryKey("AGU_NICKNAME_LEN");
                }
            }
            if (int.Parse(config.cfg003.Split(',')[1]) < nickName.Length || nickName.Length < int.Parse(config.cfg003.Split(',')[0]))
            {
                mr.Message = config.cfg004;
                return mr;
            }
            mr.Code = 1;
            mr.Message = nickName;
            return mr;
        }
        public MR DeleteUser(int userID)
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
                        var deleteList = db.Repositorywgs013.IQueryable(exp => exp.u001 == userID).ToList();
                        string ids = string.Empty;
                        foreach (var duid in deleteList)
                        {
                            ids += duid.u002 + ",";
                        }
                        if (ids.Length > 0)
                        {
                            ids = ids.Substring(0, ids.Length - 1);
                            db.Repositorywgs012.ExecuteSqlCommand(string.Format("DELETE FROM wgs012 WHERE u001 IN({0});", ids));
                            db.Repositorywgs013.ExecuteSqlCommand(string.Format("DELETE FROM wgs013 WHERE u001 IN({0}) OR u002 IN({0});", ids));
                            db.Repositorywgs014.ExecuteSqlCommand(string.Format("DELETE FROM wgs014 WHERE u001 IN({0});", ids));
                            db.Repositorywgs017.ExecuteSqlCommand(string.Format("DELETE FROM wgs017 WHERE u001 IN({0});", ids));
                            db.Repositorywgs018.ExecuteSqlCommand(string.Format("DELETE FROM wgs018 WHERE u001 IN({0});", ids));
                            db.Repositorywgs019.ExecuteSqlCommand(string.Format("DELETE FROM wgs019 WHERE u001 IN({0});", ids));
                            db.Repositorywgs020.ExecuteSqlCommand(string.Format("DELETE FROM wgs020 WHERE u001 IN({0});", ids));
                            db.Repositorywgs021.ExecuteSqlCommand(string.Format("DELETE FROM wgs021 WHERE u001 IN({0});", ids));
                            db.Repositorywgs022.ExecuteSqlCommand(string.Format("DELETE FROM wgs022 WHERE u001 IN({0});", ids));
                            db.Repositorywgs045.ExecuteSqlCommand(string.Format("DELETE FROM wgs045 WHERE u001 IN({0});", ids));
                            db.Repositorywgs050.ExecuteSqlCommand(string.Format("DELETE FROM wgs050 WHERE u001 IN({0});", ids));
                            db.Repositorywgs023.ExecuteSqlCommand(string.Format("DELETE FROM wgs023 WHERE u001 IN({0});", ids));
                            db.Repositorywgs025.ExecuteSqlCommand(string.Format("DELETE FROM wgs025 WHERE u001 IN({0});", ids));
                            db.Repositorywgs026.ExecuteSqlCommand(string.Format("DELETE FROM wgs026 WHERE u001 IN({0});", ids));
                            db.Repositorywgs030.ExecuteSqlCommand(string.Format("DELETE FROM wgs030 WHERE u001 IN({0});", ids));
                            db.Repositorywgs031.ExecuteSqlCommand(string.Format("DELETE FROM wgs031 WHERE u001 IN({0});", ids));
                            db.Repositorywgs035.ExecuteSqlCommand(string.Format("DELETE FROM wgs035 WHERE u001 IN({0});", ids));
                            db.Repositorywgs039.ExecuteSqlCommand(string.Format("DELETE FROM wgs039 WHERE u001 IN({0});", ids));
                            db.Repositorywgs042.ExecuteSqlCommand(string.Format("DELETE FROM wgs042 WHERE u001 IN({0});", ids));
                            db.Repositorywgs043.ExecuteSqlCommand(string.Format("DELETE FROM wgs043 WHERE tf005 IN({0}) OR tf002 IN({0});", ids));
                            db.Repositorywgs044.ExecuteSqlCommand(string.Format("DELETE FROM wgs044 WHERE msg004 IN({0}) OR msg005 IN({0});", ids));
                            db.Repositorywgs046.ExecuteSqlCommand(string.Format("DELETE FROM wgs046 WHERE u001 IN({0});", ids));
                            db.Repositorywgs047.ExecuteSqlCommand(string.Format("DELETE FROM wgs047 WHERE cur005 IN({0}) OR cur009 IN({0});", ids));
                            db.Repositorywgs048.ExecuteSqlCommand(string.Format("DELETE FROM wgs048 WHERE u001 IN({0}) OR u002 IN({0});", ids));
                            db.Repositorywgs049.ExecuteSqlCommand(string.Format("DELETE FROM wgs049 WHERE u001 IN({0});", ids));
                            db.Repositorywgs051.ExecuteSqlCommand(string.Format("DELETE FROM wgs051 WHERE us002 IN({0}) OR us005 IN({0});", ids));
                            db.Repositorywgs052.ExecuteSqlCommand(string.Format("DELETE FROM wgs052 WHERE u001 IN({0});", ids));
                            db.Repositorywgs053.ExecuteSqlCommand(string.Format("DELETE FROM wgs053 WHERE tpi002 IN({0});", ids));
                        }
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
        public List<DBModel.wgs016> GetMGUList()
        {
            List<DBModel.wgs016> list = null;
            using (UnitOfWork db = new UnitOfWork(true))
            {
                list = db.Repositorywgs016.IQueryable().ToList();
                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].mu002 = _NWC.DEncrypt.Decrypt(list[i].mu002);
                    list[i].mu003 = _NWC.DEncrypt.Decrypt(list[i].mu003);
                }
            }
            return list;
        }
        public MR UpdateMGU(DBModel.wgs016 entity)
        {
            MR mr = new MR();
            if (1 == entity.mu001)
            {
                mr.Code = 0;
                mr.Message = "此账号不能修改！";
                return mr;
            }
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    var oldEntity = db.Repositorywgs016.GetByPrimaryKey(entity.mu001);
                    if (false == _NWC.GeneralValidate.IsNullOrEmpty(entity.mu004))
                    {
                        oldEntity.mu004 = _NWC.SHA1.Get(entity.mu004 + oldEntity.mu005, _NWC.SHA1Bit.L160);
                    }
                    if (false == _NWC.GeneralValidate.IsNullOrEmpty(entity.mu003))
                    {
                        oldEntity.mu003 = _NWC.DEncrypt.Encrypt(entity.mu003.Trim());
                    }
                    oldEntity.pg001 = entity.pg001;
                    oldEntity.mu006 = entity.mu006;
                    db.Repositorywgs016.Update(oldEntity);
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
        public MR AddAGU(DBModel.wgs012 userEntity, List<DBModel.wgs017> userPrizeDataEntityList)
        {
            MR mr = new MR();
            if (0 == userPrizeDataEntityList.Count)
            {
                mr.Message = "用户没有返点数据";
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    DBModel.wgs027 pointLimit = db.Repositorywgs027.GetByPrimaryKey("SYS_POINT_LIMIT");
                    DBModel.wgs027 fatherLimit = db.Repositorywgs027.GetByPrimaryKey("SYS_POINT_LINE_LIMIT");
                    DBModel.wgs012 curUserParent = null;
                    var accountRule = db.Repositorywgs027.GetByPrimaryKey("AGU_REGISTER_ACCOUNT_RULE");
                    var defaultLevelList = db.Repositorywgs027.GetByPrimaryKey("SYS_POSITION_LEVEL_DEFAULT");
                    var sysStockPercent = db.Repositorywgs027.GetByPrimaryKey("SYS_STOCK_MAX").cfg003.Split(',');
                    var defLSplit = defaultLevelList.cfg003.Split(',');
                    if (false == Regex.IsMatch(userEntity.u002, accountRule.cfg003.Trim()))
                    {
                        mr.Message = accountRule.cfg005.Trim();
                        return mr;
                    }
                    if (CheckAGAccount(userEntity.u002))
                    {
                        mr.Message = "账号已经存在";
                        return mr;
                    }
                    if (_NWC.GeneralValidate.IsNullOrEmpty(userEntity.u009))
                    {
                        mr.Message = "密码不能为空";
                        return mr;
                    }
                    if (0 > userEntity.u012)
                    {
                        mr.Message = "上级出错";
                        return mr;
                    }
                    else
                    {
                        if (0 != userEntity.u012)
                        {
                            curUserParent = db.Repositorywgs012.GetByPrimaryKey(userEntity.u012);
                            if (null == curUserParent)
                            {
                                mr.Message = "上级不存在";
                                return mr;
                            }
                            if (0 == curUserParent.u020)
                            {
                                mr.Message = "没有权限开下级";
                                return mr;
                            }
                        }
                    }
                    List<int> parentList = new List<int>();
                    List<DBModel.wgs012> parentAllList = new List<DBModel.wgs012>();
                    if (0 < userEntity.u012)
                    {
                        int curSelParentID = userEntity.u012;
                        do
                        {
                            parentList.Add(curSelParentID);
                            var parentUser = db.Repositorywgs012.IQueryable(exp => exp.u001 == curSelParentID).FirstOrDefault();
                            parentAllList.Add(parentUser);
                            if (null != parentUser)
                            {
                                curSelParentID = parentUser.u012;
                            }
                        }
                        while (curSelParentID != 0);
                        parentList.Reverse();
                        parentAllList.Reverse();
                    }
                    try
                    {
                        userEntity.u005 = DateTime.Now;
                        userEntity.u011 = _NWC.RandomString.Get("", 10);
                        userEntity.u009 = _NWC.SHA1.Get(userEntity.u009 + userEntity.u011, _NWC.SHA1Bit.L160);
                        userEntity.u014 = curUserParent != null ? curUserParent.u002.Trim() : null;
                        if (0 == userEntity.u012)
                        {
                            userEntity.u018 = 1;
                        }
                        else
                        {
                            userEntity.u018 = curUserParent.u018 + 1;
                        }
                        foreach (var defSName in defLSplit)
                        {
                            var defSNameSplit = defSName.Split(':');
                            if (int.Parse(defSNameSplit[0]) == userEntity.u018)
                            {
                                userEntity.u013 = int.Parse(defSNameSplit[1]);
                            }
                        }
                        foreach (var item in sysStockPercent)
                        {
                            var itemSplit = item.Split(':');
                            if (int.Parse(itemSplit[0]) == userEntity.u013)
                            {
                                userEntity.u019 = decimal.Parse(itemSplit[1]);
                            }
                        }
                        userEntity.wgs014 = new DBModel.wgs014();
                        userEntity.wgs014.u001 = userEntity.u001;
                        db.Repositorywgs012.Add(userEntity);
                        db.SaveChanges();
                        DBModel.wgs013 ut = new DBModel.wgs013();
                        if (0 == parentAllList.Count())
                        {
                            ut.u001 = userEntity.u001;
                            ut.u002 = userEntity.u001;
                            ut.u001n = userEntity.u002.Trim();
                            ut.u002n = ut.u001n;
                            if (false == string.IsNullOrEmpty(userEntity.u003))
                            {
                                ut.u001nn = userEntity.u003;
                                ut.u002nn = ut.u001nn;
                            }
                            ut.u001l = 1;
                            ut.u002l = 1;
                            db.Repositorywgs013.Add(ut);
                            db.SaveChanges();
                        }
                        foreach (var item in parentAllList)
                        {
                            DBModel.wgs013 lut = new DBModel.wgs013();
                            lut.u001 = item.u001;
                            lut.u001n = item.u002.Trim();
                            lut.u001l = item.u018;
                            if (false == string.IsNullOrEmpty(item.u003))
                            {
                                lut.u001nn = item.u003.Trim();
                            }
                            lut.u002 = userEntity.u001;
                            lut.u002n = userEntity.u002.Trim();
                            if (false == string.IsNullOrEmpty(userEntity.u003))
                            {
                                lut.u002nn = userEntity.u003.Trim();
                            }
                            lut.u002l = userEntity.u018;
                            db.Repositorywgs013.Add(lut);
                            db.SaveChanges();
                        }
                        foreach (var userPrizeData in userPrizeDataEntityList)
                        {
                            userPrizeData.u001 = userEntity.u001;
                            if (null == userPrizeData.gp001 || 0 == userPrizeData.gp001)
                            {
                                continue;
                            }
                            var parentData = db.Repositorywgs017.IQueryable(exp => exp.u001 == userEntity.u012 && exp.gc001 == userPrizeData.gc001).FirstOrDefault();
                            if (null != parentData)
                            {
                                userPrizeData.up003 = parentData.up003 - userPrizeData.up002;
                                userPrizeData.up005 = parentData.up005 - userPrizeData.up004;
                                userPrizeData.u012 = parentData.u001;
                                userPrizeData.up014 = userEntity.u002;
                            }
                            else if (0 == userEntity.u012 && null == parentData)
                            {
                                var sysPrizeData = db.Repositorywgs007.Get(exp => exp.gp001 == userPrizeData.gp001).FirstOrDefault();
                                userPrizeData.up003 = sysPrizeData.gp008 - userPrizeData.up002;
                                userPrizeData.up005 = sysPrizeData.gp007 - userPrizeData.up004;
                                userPrizeData.u012 = 0;
                                userPrizeData.up014 = userEntity.u002;
                            }
                            if (0 > userPrizeData.up003)
                            {
                                throw new Exception("返点有错误");
                            }
                            var lineLimitPoint = fatherLimit.cfg003.Trim();
                            var linePointSetting = lineLimitPoint.Split(',');
                            foreach (var lps in linePointSetting)
                            {
                                var lpsTemp = lps.Split(':');
                                var isMyFather = CheckUserIsMyFather(userEntity.u002.Trim(), lpsTemp[0]);
                                if (false == isMyFather)
                                {
                                    continue;
                                }
                                var lpsNums = lpsTemp[1].Split('-');
                                foreach (var p in lpsNums)
                                {
                                    var tempLimitPoint = p.Split('|');
                                    if (isMyFather && userPrizeData.up003 == Math.Round(decimal.Parse(tempLimitPoint[0]), 1))
                                    {
                                        var tempName = lpsTemp[0];
                                        var xxTempPoint = decimal.Parse(tempLimitPoint[0]);
                                        var childs = db.Repositorywgs048.IQueryable(exp => exp.u001n == tempName && exp.u002n != tempName).Select(exp => exp.u002).ToList();
                                        var haveCount = db.Repositorywgs017.Count(exp => exp.gc001 == userPrizeData.gc001 && childs.Contains(exp.u001) && exp.up003 == xxTempPoint);
                                        if( haveCount >= int.Parse(tempLimitPoint[1]))
                                        {
                                            throw new Exception(string.Format("返点为{0:N1}的账号，只允许创建{1}个账号，请联系您的上级", userPrizeData.up003, tempLimitPoint[1]));
                                        }
                                    }
                                }
                            }
                            if (userEntity.u016 <= 0)
                            {
                                var pointLimitSplit = pointLimit.cfg003.Split(',');
                                foreach (var limitItem in pointLimitSplit)
                                {
                                    var limitData = limitItem.Split('|');
                                    var limitPoint = decimal.Parse( limitData[0] );
                                    if (limitPoint > userPrizeData.up003)
                                    {
                                        continue;
                                    }
                                    var limitParentUserInfo = limitData[1].Split('&');
                                    if ("*" == limitParentUserInfo[0])
                                    {
                                        throw new Exception(string.Format("不允许创建返点为{0:N1}的账号，请联系您的上级，只允许小于{1:N1}", userPrizeData.up003, limitPoint));
                                    }
                                    else
                                    {
                                        var lName = limitParentUserInfo[0];
                                        if (true == CheckUserIsMyFather(userEntity.u014, lName))
                                        {
                                            throw new Exception(string.Format("不允许创建返点为{0:N1}的账号，只允许小于{1:N1}，请联系您的上级", userPrizeData.up003, limitPoint));
                                        }
                                    }
                                }
                            }
                            db.Repositorywgs017.Add(userPrizeData);
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
        public Boolean CheckAGAccount(string account)
        {
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var count = db.Repositorywgs012.IQueryable(exp => exp.u002 == account.Trim()).Count();
                    if (1 == count)
                    {
                        return true;
                    }
                }
            } return false;
        }
        public MR UpdateAGUNickname(int userID, string nickname)
        {
            MR mr = new MR();
            var chr = CheckNickName(nickname);
            if (0 == chr.Code)
            {
                return chr;
            }
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    var updateItem = db.Repositorywgs012.GetByPrimaryKey(userID);
                    updateItem.u003 = nickname;
                    db.Repositorywgs012.Update(updateItem);
                    try
                    {
                        db.SaveChanges();
                        ts.Complete();
                        mr.Code = 1;
                        mr.Message = nickname;
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
        public MR UpdateAGU(DBModel.wgs012 entity)
        {
            MR mr = new MR();
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    try
                    {
                        db.Repositorywgs012.Update(entity);
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
        public List<DBModel.wgs012> GetAGUList(int parentID)
        {
            List<DBModel.wgs012> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs012.IQueryable(exp => exp.u012 == parentID).ToList();
                }
            }
            return list;
        }
        public List<DBModel.wgs014> GetAGUAll()
        {
            List<DBModel.wgs014> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs014.GetAll().ToList();
                }
            }
            return list;
        }
        public MR CheckChildStockIsMax(int myUserID, decimal myStock)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            List<DBModel.wgs049> myChildList = null;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var myChild = db.Repositorywgs048.IQueryable(exp => exp.u001 == myUserID && exp.u002 != myUserID).Select(exp => exp.u002).ToList();
                    myChildList = db.Repositorywgs049.IQueryable(exp => myChild.Contains(exp.u001) && exp.u019 > myStock).ToList();
                }
            }
            if (0 == myChildList.Count)
            {
                mr.Code = 1;
            }
            else
            {
                mr.Code = 0;
                foreach (var item in myChildList)
                {
                    mr.Message += string.Format("账号：{0}，分红比：{1}；", item.u002.Trim(), (int)item.u019);
                }
            }
            return mr;
        }
        public List<DBModel.UserPack> GetAGUList(int parentID, string account, int status, int amtT, decimal amtV, int pntT, decimal pntV, DateTime? RDTS, DateTime? RDTE, DateTime? LDTS, DateTime? LDTE, string IP, int pageIndex, int pageSize, out int recordCount, string sysNotSetMomeyAccount)
        {
            recordCount = 0;
            List<DBModel.UserPack> list = null;
            Expression<Func<DBModel.wgs012, bool>> query = PredicateExtensionses.True<DBModel.wgs012>();
            if (false == string.IsNullOrEmpty(account))
            {
                query = query.And(exp => exp.u002.Contains(account));
            }
            else
            { 
                if (-1 != parentID)
                {
                    query = query.And(exp => exp.u012 == parentID);
                }
            }
            if (-1 != status)
            {
                query = query.And(exp => exp.u008 == status);
            }
            if (false == string.IsNullOrEmpty(IP))
            {
                query = query.And(exp=>exp.u022.Contains(IP));
            }
            if (amtV != 0)
            {
                switch (amtT)
                {
                    case 1:
                        query = query.And(exp=>exp.wgs014.uf001 == amtV);
                        break;
                    case 2:
                        query = query.And(exp => exp.wgs014.uf001 <= amtV);
                        break;
                    case 3:
                        query = query.And(exp => exp.wgs014.uf001 >= amtV);
                        break;
                }
            }
            if (pntT != 0)
            {
                switch (pntT)
                {
                    case 1:
                        query = query.And(exp => exp.wgs014.uf004 == pntV);
                        break;
                    case 2:
                        query = query.And(exp => exp.wgs014.uf004 <= pntV);
                        break;
                    case 3:
                        query = query.And(exp => exp.wgs014.uf004 >= pntV);
                        break;
                }
            }
            if (RDTS.HasValue && RDTE.HasValue)
            {
                RDTS = DateTime.Parse(RDTS.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                RDTE = DateTime.Parse(RDTE.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                query = query.And(exp=>exp.u005 >= RDTS && exp.u005 <= RDTE);
            }
            if (LDTS.HasValue && LDTE.HasValue)
            {
                LDTS = DateTime.Parse(LDTS.Value.ToString("yyyy-MM-dd")+" 00:00:00");
                LDTE = DateTime.Parse(LDTE.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                query = query.And(exp => exp.u007 >= LDTS && exp.u007 <= LDTE);
            }
            if (false == string.IsNullOrEmpty(account))
            {
                query = query.And(exp=>exp.u002.Contains(account));
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCount = db.Repositorywgs012.Count(query);
                    if (0 < recordCount)
                    {
                        list = new List<DBModel.UserPack>();
                    }
                    List<DBModel.wgs012> userList = null;
                    List<DBModel.wgs012> _userList = null;
                    if (!string.IsNullOrEmpty(sysNotSetMomeyAccount))
                    {
                        List<string> AccountList = new List<string>();
                        string[] notSetMomeyAccount = sysNotSetMomeyAccount.Split('|');
                        for (int i = 0; i < notSetMomeyAccount.Length; i++)
                        {
                            AccountList.Add(notSetMomeyAccount[i]);
                        }
                        foreach (var accoun in AccountList)
                        {
                            DBModel.wgs012 user = db.Repositorywgs012.IQueryable(px => px.u002 == accoun).First();
                            if (user != null)
                            {
                                _userList = db.Repositorywgs012.IQueryable(px => px.u012 == user.u001).ToList();
                                if (_userList != null)
                                {
                                    foreach (var _user in _userList)
                                    {
                                        List<DBModel.wgs012> userList1 = db.Repositorywgs012.IQueryable(px => px.u012 == _user.u001).ToList();
                                        query = query.And(exp => exp.u002 != _user.u002);
                                        if (userList1 != null)
                                        {
                                            foreach (var user1 in userList1)
                                            {
                                                List<DBModel.wgs012> userList2 = db.Repositorywgs012.IQueryable(px => px.u012 == user1.u001).ToList();
                                                query = query.And(exp => exp.u002 != user1.u002);
                                                if (userList2 != null)
                                                {
                                                    foreach (var user2 in userList2)
                                                    {
                                                        List<DBModel.wgs012> userList3 = db.Repositorywgs012.IQueryable(px => px.u012 == user2.u001).ToList();
                                                        query = query.And(exp => exp.u002 != user2.u002);
                                                        if (userList3 != null)
                                                        {
                                                            foreach (var user3 in userList3)
                                                            {
                                                                List<DBModel.wgs012> userList4 = db.Repositorywgs012.IQueryable(px => px.u012 == user3.u001).ToList();
                                                                query = query.And(exp => exp.u002 != user3.u002);
                                                                if (userList4 != null)
                                                                {
                                                                    foreach (var user4 in userList4)
                                                                    {
                                                                        List<DBModel.wgs012> userList5 = db.Repositorywgs012.IQueryable(px => px.u012 == user4.u001).ToList();
                                                                        query = query.And(exp => exp.u002 != user4.u002);
                                                                        if (userList5 != null)
                                                                        {
                                                                            foreach (var user5 in userList5)
                                                                            {
                                                                                List<DBModel.wgs012> userList6 = db.Repositorywgs012.IQueryable(px => px.u012 == user5.u001).ToList();
                                                                                query = query.And(exp => exp.u002 != user5.u002);
                                                                                if (userList6 != null)
                                                                                {
                                                                                    foreach (var user6 in userList6)
                                                                                    {
                                                                                        List<DBModel.wgs012> userList7 = db.Repositorywgs012.IQueryable(px => px.u012 == user6.u001).ToList();
                                                                                        query = query.And(exp => exp.u002 != user6.u002);
                                                                                        if (userList7 != null)
                                                                                        {
                                                                                            foreach (var user7 in userList7)
                                                                                            {
                                                                                                List<DBModel.wgs012> userList8 = db.Repositorywgs012.IQueryable(px => px.u012 == user7.u001).ToList();
                                                                                                query = query.And(exp => exp.u002 != user7.u002);
                                                                                                if (userList8 != null)
                                                                                                {
                                                                                                    foreach (var user8 in userList8)
                                                                                                    {
                                                                                                        List<DBModel.wgs012> userList9 = db.Repositorywgs012.IQueryable(px => px.u012 == user8.u001).ToList();
                                                                                                        query = query.And(exp => exp.u002 != user8.u002);
                                                                                                        if (userList9 != null)
                                                                                                        {
                                                                                                            foreach (var user9 in userList9)
                                                                                                            {
                                                                                                                //List<DBModel.wgs012> userList4 = db.Repositorywgs012.IQueryable(px => px.u012 == user3.u001).ToList();
                                                                                                                query = query.And(exp => exp.u002 != user9.u002);
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            query = query.And(exp => exp.u002 != accoun);
                        }

                    }
                    userList = db.Repositorywgs012.IQueryable(query, order => order.OrderByDescending(exp => exp.u001), "wgs014").ToList().Skip(pageSize * pageIndex).Take(pageSize).ToList();

                    foreach (var item in userList)
                    {
                        var userPack = new DBModel.UserPack();
                        userPack.ChildCount = db.Repositorywgs012.Count(exp => exp.u012 == item.u001);
                        userPack.User = item;
                        userPack.PointList = db.Repositorywgs017.IQueryable(exp => exp.u001 == item.u001).ToList();
                        list.Add(userPack);
                    }
                }
            }
            return list;
        }
        public List<DBModel.UserPack> GetAGUList(int myUserID, int parentID, string account, int status, int amtT, decimal amtV, int pntT, decimal pntV, int pageIndex, int pageSize, DateTime? rDts, DateTime? rDte, DateTime? lDts, DateTime? lDte, out int recordCount)
        {
            List<DBModel.UserPack> result = null;
            recordCount = 0;
            Expression<Func<DBModel.wgs012, bool>> query = PredicateExtensionses.True<DBModel.wgs012>();
            
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            if (-1 == status)
            {
                query = query.And(exp => exp.u008 > status);
            }
            else if( -1 < status)
            {
                query = query.And(exp => exp.u008 == status);
            }
            if (rDts.HasValue && rDte.HasValue)
            {
                rDts = DateTime.Parse(rDts.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                rDte = DateTime.Parse(rDte.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                query = query.And(exp=>exp.u005 >= rDts && exp.u005 <= rDte);
            }
            if (lDts.HasValue && lDte.HasValue)
            {
                lDts = DateTime.Parse(lDts.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                lDte = DateTime.Parse(lDte.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                query = query.And(exp => exp.u007 >= lDts && exp.u007 <= lDte && exp.u007 != null);
            }
            if (amtV != 0)
            {
                switch (amtT)
                {
                    case 1:
                        query = query.And(exp => exp.wgs014.uf001 == amtV);
                        break;
                    case 2:
                        query = query.And(exp => exp.wgs014.uf001 <= amtV);
                        break;
                    case 3:
                        query = query.And(exp => exp.wgs014.uf001 >= amtV);
                        break;
                }
            }
            if (pntT != 0)
            {
                switch (pntT)
                {
                    case 1:
                        query = query.And(exp => exp.wgs014.uf004 == pntV);
                        break;
                    case 2:
                        query = query.And(exp => exp.wgs014.uf004 <= pntV);
                        break;
                    case 3:
                        query = query.And(exp => exp.wgs014.uf004 >= pntV);
                        break;
                }
            }
            List<DBModel.wgs012> userList = null;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var myChildIDs = db.Repositorywgs013.IQueryable(exp => exp.u001 == myUserID && myUserID != exp.u002).Select(exp => exp.u002).ToList();
                    if (!string.IsNullOrEmpty(account))
                    {
                        if (0 < myChildIDs.Count)
                        {
                            query = query.And(exp => myChildIDs.Contains(exp.u001) && exp.u002.Contains(account));
                        }
                    }
                    else if( 0 < parentID )
                    {
                        query = query.And(exp=>exp.u012 == parentID);
                    }
                    else if (0 == parentID)
                    {
                        query = query.And(exp=>exp.u012 == myUserID);
                    }
                    if (rDte.HasValue && rDts.HasValue)
                    {
                        rDts = DateTime.Parse(rDts.Value.ToString("yyyy-MM-dd ") + " 00:00:01");
                        rDte = DateTime.Parse(rDte.Value.ToString("yyyy-MM-dd ") + " 23:59:59");
                        query = query.And(exp=>exp.u005 >= rDts && exp.u005 <= rDte);
                    }
                    if (lDte.HasValue && lDts.HasValue)
                    {
                        lDts = DateTime.Parse(lDts.Value.ToString("yyyy-MM-dd ") + " 00:00:01");
                        lDte = DateTime.Parse(lDte.Value.ToString("yyyy-MM-dd ") + " 23:59:59");
                        query = query.And(exp => exp.u005 >= lDts && exp.u005 <= lDte && exp.u005 != null);
                    }

                    



                    userList = db.Repositorywgs012.IQueryable(query, order => order.OrderByDescending(exp => exp.u001), "wgs014").Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    recordCount = db.Repositorywgs012.Count(query);


                    

                    if (null != userList)
                    {
                        result = new List<DBModel.UserPack>();
                        foreach (var user in userList)
                        {
                            DBModel.UserPack userPack = new DBModel.UserPack();
                            userPack.User = user;
                            userPack.ChildCount = db.Repositorywgs013.Count(exp=>exp.u001 == user.u001 && exp.u002 != user.u001);
                            userPack.PointList = null;
                            
                            result.Add(userPack);
                        }
                    }
                }
            }
            return result;
        }
        public List<DBModel.UserPack> GetAGUAll(int parentID, string account, int status, int amtT, decimal amtV, int pntT, decimal pntV, DateTime? RDTS, DateTime? RDTE, DateTime? LDTS, DateTime? LDTE, string IP, int pageIndex, int pageSize, out int recordCount, string sysNotSetMomeyAccount)
        {
            recordCount = 0;
            List<DBModel.UserPack> list = null;
            Expression<Func<DBModel.wgs012, bool>> query = PredicateExtensionses.True<DBModel.wgs012>();
            if (-1 != status)
            {
                query = query.And(exp => exp.u008 == status);
            }
            if (false == string.IsNullOrEmpty(IP))
            {
                query = query.And(exp => exp.u022.Contains(IP));
            }
            if (amtV != 0)
            {
                switch (amtT)
                {
                    case 1:
                        query = query.And(exp => exp.wgs014.uf001 == amtV);
                        break;
                    case 2:
                        query = query.And(exp => exp.wgs014.uf001 <= amtV);
                        break;
                    case 3:
                        query = query.And(exp => exp.wgs014.uf001 >= amtV);
                        break;
                }
            }
            if (pntT != 0)
            {
                switch (pntT)
                {
                    case 1:
                        query = query.And(exp => exp.wgs014.uf004 == pntV);
                        break;
                    case 2:
                        query = query.And(exp => exp.wgs014.uf004 <= pntV);
                        break;
                    case 3:
                        query = query.And(exp => exp.wgs014.uf004 >= pntV);
                        break;
                }
            }
            if (RDTS.HasValue && RDTE.HasValue)
            {
                RDTS = DateTime.Parse(RDTS.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                RDTE = DateTime.Parse(RDTE.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                query = query.And(exp => exp.u005 >= RDTS && exp.u005 <= RDTE);
            }
            if (LDTS.HasValue && LDTE.HasValue)
            {
                LDTS = DateTime.Parse(LDTS.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                LDTE = DateTime.Parse(LDTE.Value.ToString("yyyy-MM-dd") + " 23:59:59");
                query = query.And(exp => exp.u007 >= LDTS && exp.u007 <= LDTE);
            }
            if (false == string.IsNullOrEmpty(account))
            {
                query = query.And(exp => exp.u002.Contains(account));
            }
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    recordCount = db.Repositorywgs012.Count(query);
                    if (0 < recordCount)
                    {
                        list = new List<DBModel.UserPack>();
                    }
                    List<DBModel.wgs012> userList = null;
                    List<DBModel.wgs012> _userList = null;
                    if (!string.IsNullOrEmpty(sysNotSetMomeyAccount))
                    {
                        List<string> AccountList = new List<string>();
                        string[] notSetMomeyAccount = sysNotSetMomeyAccount.Split('|');
                        for (int i = 0; i < notSetMomeyAccount.Length; i++)
                        {
                            AccountList.Add(notSetMomeyAccount[i]);
                        }
                        foreach (var accoun in AccountList)
                        {
                            DBModel.wgs012 user = db.Repositorywgs012.IQueryable(px => px.u002 == accoun).First();
                            if (user != null)
                            {
                                _userList = db.Repositorywgs012.IQueryable(px => px.u012 == user.u001).ToList();
                                if (_userList != null)
                                {
                                    foreach (var _user in _userList)
                                    {
                                        List<DBModel.wgs012> userList1 = db.Repositorywgs012.IQueryable(px => px.u012 == _user.u001).ToList();
                                        query = query.And(exp => exp.u002 != _user.u002);
                                        if (userList1 != null)
                                        {
                                            foreach (var user1 in userList1)
                                            {
                                                List<DBModel.wgs012> userList2 = db.Repositorywgs012.IQueryable(px => px.u012 == user1.u001).ToList();
                                                query = query.And(exp => exp.u002 != user1.u002);
                                                if (userList2 != null)
                                                {
                                                    foreach (var user2 in userList2)
                                                    {
                                                        List<DBModel.wgs012> userList3 = db.Repositorywgs012.IQueryable(px => px.u012 == user2.u001).ToList();
                                                        query = query.And(exp => exp.u002 != user2.u002);
                                                        if (userList3 != null)
                                                        {
                                                            foreach (var user3 in userList3)
                                                            {
                                                                List<DBModel.wgs012> userList4 = db.Repositorywgs012.IQueryable(px => px.u012 == user3.u001).ToList();
                                                                query = query.And(exp => exp.u002 != user3.u002);
                                                                if (userList4 != null)
                                                                {
                                                                    foreach (var user4 in userList4)
                                                                    {
                                                                        List<DBModel.wgs012> userList5 = db.Repositorywgs012.IQueryable(px => px.u012 == user4.u001).ToList();
                                                                        query = query.And(exp => exp.u002 != user4.u002);
                                                                        if (userList5 != null)
                                                                        {
                                                                            foreach (var user5 in userList5)
                                                                            {
                                                                                List<DBModel.wgs012> userList6 = db.Repositorywgs012.IQueryable(px => px.u012 == user5.u001).ToList();
                                                                                query = query.And(exp => exp.u002 != user5.u002);
                                                                                if (userList6 != null)
                                                                                {
                                                                                    foreach (var user6 in userList6)
                                                                                    {
                                                                                        List<DBModel.wgs012> userList7 = db.Repositorywgs012.IQueryable(px => px.u012 == user6.u001).ToList();
                                                                                        query = query.And(exp => exp.u002 != user6.u002);
                                                                                        if (userList7 != null)
                                                                                        {
                                                                                            foreach (var user7 in userList7)
                                                                                            {
                                                                                                List<DBModel.wgs012> userList8 = db.Repositorywgs012.IQueryable(px => px.u012 == user7.u001).ToList();
                                                                                                query = query.And(exp => exp.u002 != user7.u002);
                                                                                                if (userList8 != null)
                                                                                                {
                                                                                                    foreach (var user8 in userList8)
                                                                                                    {
                                                                                                        List<DBModel.wgs012> userList9 = db.Repositorywgs012.IQueryable(px => px.u012 == user8.u001).ToList();
                                                                                                        query = query.And(exp => exp.u002 != user8.u002);
                                                                                                        if (userList9 != null)
                                                                                                        {
                                                                                                            foreach (var user9 in userList9)
                                                                                                            {
                                                                                                                //List<DBModel.wgs012> userList4 = db.Repositorywgs012.IQueryable(px => px.u012 == user3.u001).ToList();
                                                                                                                query = query.And(exp => exp.u002 != user9.u002);
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            query = query.And(exp => exp.u002 != accoun);
                        }
                       
                    }
                    userList=db.Repositorywgs012.IQueryable(query, order => order.OrderByDescending(exp => exp.u001), "wgs014").ToList().Skip(pageSize * pageIndex).Take(pageSize).ToList();
                    foreach (var item in userList)
                    {
                        var userPack = new DBModel.UserPack();
                        userPack.ChildCount = db.Repositorywgs012.Count(exp => exp.u012 == item.u001);
                        userPack.User = item;
                        userPack.PointList = db.Repositorywgs017.IQueryable(exp => exp.u001 == item.u001).ToList();
                        list.Add(userPack);
                    }
                }
            }
            return list;
        }
        public MR SetUserStatus(List<int> userIDs, int status)
        {
            MR mr = new MR();
            if (0 == userIDs.Count)
            {
                mr.Message = "账号列表不能为空";
                return mr;
            }
            using (UnitOfWork db = new UnitOfWork(true))
            {
                string inIDs = string.Empty;
                foreach (var id in userIDs)
                {
                    inIDs += id + ",";
                }
                inIDs = inIDs.Substring(0, inIDs.Length - 1);
                try
                {
                    db.Repositorywgs012.ExecuteSqlCommand("UPDATE wgs012 SET u008="+status+" WHERE u001 IN(" + inIDs + ");");
                    mr.Code = 1;
                }
                catch(Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = MyException.Message(mr.Exception.Message);
                    mr.Code = 0;
                }
            }
            return mr;
        }
        public int GetAGUCount(int parentID)
        {
            int count = 0;
            using (UnitOfWork db = new UnitOfWork(true))
            {
                count = db.Repositorywgs012.Count(exp => exp.u012 == parentID);
            }
            return count;
        }
        public List<int> GetAGAParent(int userID)
        {
            List<int> list = new List<int>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs013.IQueryable(exp => exp.u002 == userID).Select(exp => exp.u001).ToList();
                }
            }
            return list;
        }
        public int GetAGUAllCount(int userID)
        {
            int count = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    count = db.Repositorywgs013.Count(exp => exp.u001 == userID && exp.u002 != userID);
                }
            }
            return count;
        }
        public int GetAGUBaseCount(int userID)
        {
            int count = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    count = db.Repositorywgs012.Count(exp => exp.u012 == userID);
                }
            }
            return count;
        }
        public List<DBModel.wgs017> GetAGUPData(int userID)
        {
            List<DBModel.wgs017> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs017.IQueryable(exp => exp.u001 == userID, order=>order.OrderBy(exp=>exp.gc001)).ToList();
                }
            }
            return list;
        }
        public MR SaveAGUPData(List<DBModel.wgs017> list)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs017.UpdateList(list);
                    db.SaveChanges();
                    mr.Code = 1;
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException( error );
                    mr.Message = MyException.Message(mr.Exception.Message);
                }
            }
            return mr;
        }
        public DBModel.wgs017 GetAGUPData(int userID, int gameClassID)
        {
            DBModel.wgs017 model = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    model = db.Repositorywgs017.IQueryable(exp => exp.u001 == userID && exp.gc001 == gameClassID).FirstOrDefault();
                }
            }
            return model;
        }
        public DBModel.wgs012 GetAGU(int key)
        {
            DBModel.wgs012 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs012.IQueryable(exp => exp.u001 == key, null, "wgs014").FirstOrDefault();
                }
            }
            return entity;
        }
        public DBModel.wgs012 GetAGU(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            DBModel.wgs012 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs012.IQueryable(exp => exp.u002 == key, null, "wgs014").FirstOrDefault();
                }
            }
            return entity;
        }
        public MR UpdateGUAPassword(int key, string password, int type)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    var oldItem = GetAGU(key);
                    if (0 == type)
                    {
                        oldItem.u009 = password;
                    }
                    else if (1 == type)
                    {
                        oldItem.u010 = password;
                    }
                    db.Repositorywgs012.Update(oldItem);
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
        public MR UpdateGUALM(int key, string message)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                var oldEntity = db.Repositorywgs012.GetByPrimaryKey(key);
                try
                {
                    oldEntity.u021 = message;
                    db.Repositorywgs012.Update(oldEntity);
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
        public DBModel.wgs014 GetAGUFData(int userID)
        {
            DBModel.wgs014 entity = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    entity = db.Repositorywgs014.GetByPrimaryKey(userID);
                }
            }
            return entity;
        }
        public List<DBModel.SysPositionLevel> GetUserPositionLevel(bool cache)
        {
            List<DBModel.SysPositionLevel> list = (List<DBModel.SysPositionLevel>)_NWC.GeneralCache.Get("UserPositionLevel");
            if (null == list || false == cache)
            {
                DBModel.wgs027 entity = null;
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        entity = db.Repositorywgs027.GetByPrimaryKey("SYS_POSITION_LEVEL");
                    }
                }
                if (entity != null)
                {
                    list = new List<DBModel.SysPositionLevel>();
                    var levelSplit = entity.cfg003.Split(',');
                    foreach (var levelItem in levelSplit)
                    {
                        var itemSplit = levelItem.Split(':');
                        DBModel.SysPositionLevel data = new DBModel.SysPositionLevel();
                        data.Level = int.Parse(itemSplit[0]);
                        data.Name = itemSplit[1];
                        data.NeedMoney = decimal.Parse(itemSplit[0]);
                        list.Add(data);
                    }
                    _NWC.GeneralCache.Set("SYS_POSITION_LEVEL", list);
                }
            }
            return list;
        }
        public List<DBModel.wgs048> GetMyChildList(int myUserID, bool includeMySelf)
        {
            List<DBModel.wgs048> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    if (includeMySelf)
                    {
                        list = db.Repositorywgs048.IQueryable(exp => exp.u001 == myUserID).ToList();
                    }
                    else
                    {
                        list = db.Repositorywgs048.IQueryable(exp => exp.u001 == myUserID && exp.u002 != myUserID).ToList();
                    }
                }
            }
            return list;
        }
        public List<DBModel.SysAccountLevel> GetAccountLevel(bool cache)
        {
            string key = "UserAccountLevel";
            List<DBModel.SysAccountLevel> list = (List<DBModel.SysAccountLevel>)_NWC.GeneralCache.Get(key);
            if (null == list || false == cache)
            {
                DBModel.wgs027 entity = null;
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        entity = db.Repositorywgs027.GetByPrimaryKey("SYS_LEVEL_LAYER");
                    }
                }
                if (entity != null)
                {
                    list = new List<DBModel.SysAccountLevel>();
                    var levelSplit = entity.cfg003.Split(',');
                    foreach (var levelItem in levelSplit)
                    {
                        var itemSplit = levelItem.Split(':');
                        DBModel.SysAccountLevel data = new DBModel.SysAccountLevel();
                        data.Level = int.Parse(itemSplit[0]);
                        data.Name = itemSplit[1];
                        list.Add(data);
                    }
                    _NWC.GeneralCache.Set(key, list);
                }
            }
            return list;
        }
        public decimal GetMyTeamTotal(int myUserID, int type, int totalType)
        {
            decimal result = 0m;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    List<int> myChildIDs = new List<int>();
                    if (1 == type)
                    {
                        myChildIDs = db.Repositorywgs013.IQueryable(exp => exp.u001 == myUserID || exp.u002 == myUserID).Select(exp => exp.u002).ToList();
                    }
                    else if (2 == type)
                    {
                        myChildIDs = db.Repositorywgs013.IQueryable(exp => exp.u001 == myUserID && exp.u002 != myUserID).Select(exp => exp.u002).ToList();
                    }
                    if (0 == myChildIDs.Count)
                    {
                        result = 0;
                    }
                    else
                    {
                        switch (totalType)
                        {
                            case 1:
                                result = db.Repositorywgs014.IQueryable(exp => myChildIDs.Contains(exp.u001)).Sum(exp=>(decimal)exp.uf001);
                                break;
                            case 2:
                                result = db.Repositorywgs014.IQueryable(exp => myChildIDs.Contains(exp.u001)).Sum(exp => (decimal)exp.uf004);
                                break;
                            case 3:
                                result = db.Repositorywgs014.IQueryable(exp => myChildIDs.Contains(exp.u001)).Sum(exp => (decimal)exp.uf003);
                                break;
                        }
                    }
                }
            }
            return result;
        }
        public Dictionary<int, string> GetUserLevel(bool cache)
        {
            Dictionary<int, string> result = (Dictionary<int, string>)_NWC.GeneralCache.Get("UserLevelNames");
            DBModel.wgs027 entity = null;
            if (null == result || false == cache)
            {
                var transactionOptions = new TransactionOptions();
                transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                {
                    using (UnitOfWork db = new UnitOfWork(true))
                    {
                        entity = db.Repositorywgs027.GetByPrimaryKey("SYS_VIP_LEVEL_NAME");
                    }
                }
                result = new Dictionary<int, string>();
                string content = entity.cfg003;
                var contentSplit = content.Split(',');
                foreach (var cnt in contentSplit)
                { 
                    var cntSplit = cnt.Split(':');
                    result.Add(int.Parse(cntSplit[0]), cntSplit[1]);
                }
                _NWC.GeneralCache.Set("UserLevelNames", result);
            }
            return result;
        }
        public List<DBModel.wgs035> GetAutoRegList(int myUserID)
        {
            List<DBModel.wgs035> list = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs035.IQueryable(exp => exp.u001 == myUserID, order=>order.OrderByDescending(exp=>exp.ar004)).ToList();
                }
            }
            return list;
        }
        public MR AddAutoReg(DBModel.wgs035 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs035.Add(entity);
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
        public MR UpdateAutoReg(DBModel.wgs035 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs035.Update(entity);
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
        public DBModel.wgs035 GetAutoReg(int key)
        {
            DBModel.wgs035 result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs035.GetByPrimaryKey(key);
                }
            }
            return result;
        }
        public DBModel.wgs035 GetAutoReg(string code)
        {
            DBModel.wgs035 result = null;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs035.IQueryable(exp => exp.ar002 == code).FirstOrDefault();
                }
            }
            return result;
        }
        public MR DeleteAutoReg(int myUserID, int key)
        {
            MR mr = new MR();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var item = db.Repositorywgs035.IQueryable(exp => exp.ar001 == key && exp.u001 == myUserID).Take(1).FirstOrDefault();
                    if (null != item)
                    {
                        db.Repositorywgs035.Delete(item);
                        db.SaveChanges();
                        ts.Complete();
                    }
                }
            }
            return mr;
        }
        public bool CheckUserIDIsMyChild(int myUserID, int childUserID)
        {
            var result = false;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var count = db.Repositorywgs013.Count(exp => exp.u001 == myUserID && exp.u002 == childUserID);
                    if (0 < count)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public bool CheckUserIsMyFather(string myUserName, string fatherUserName)
        {
            var result = false;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var count = db.Repositorywgs013.Count(exp => exp.u002n == myUserName && exp.u001n == fatherUserName);
                    if (0 < count)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public bool CheckUserIDIsMyFather(int myUserID, int fatherID)
        {
            var result = false;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var count = db.Repositorywgs013.Count(exp => exp.u002 == myUserID && exp.u001 == fatherID);
                    if (0 < count)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
        public MR AddAutoReg(int myUserID, string pdIDs, string points,int allowChild)
        {
            MR mr = new MR();
            var _pdIDs = pdIDs.Split(',');
            var _points = points.Split(',');
            var result = string.Empty;
            var myUser = GetAGU(myUserID);
            if (0 == myUser.u020)
            {
                mr.Message = "没有权限开下级";
                return mr;
            }
            var existsCount = GetAutoRegCount(myUserID);
            ISystem sys = new System();
            var sysMax = sys.GetKeyValue("AGU_AUTOREGISTER_MAX").cfg003.Trim();
            if (existsCount >= int.Parse(sysMax))
            {
                mr.Message = "已经超过系统限制";
                return mr;
            }
            var parentPDList = GetAGUPData(myUserID);
            foreach (var ppd in parentPDList)
            {
                for (int i = 0; i < _pdIDs.Length; i++)
                {
                    if (0 == parentPDList.Count(exp => exp.up001 == int.Parse(_pdIDs[i])))
                    {
                        mr.Message = "返点信息有错P";
                        return mr;
                    }
                    if (ppd.up001 == int.Parse(_pdIDs[i]))
                    {
                        var point = decimal.Parse(_points[i]);
                        if (0.1m > Math.Round(point, 1))
                        {
                            mr.Message = "返点必须大于等于0.1";
                            return mr;
                        }
                        result += string.Format("{0}|{1}|{2},", ppd.gc001, Math.Round(point, 1), ppd.up001);
                    }
                }
            }
            if (0 < result.Length)
            {
                result = result.Substring(0, result.Length - 1);
            }
            var md5 = _NWC.MD5.Get(myUserID + (existsCount+1).ToString(), _NWC.MD5Bit.L32);
            var user = GetAGU(myUserID);
            DBModel.wgs035 entity = new DBModel.wgs035();
            entity.ar002 = md5;
            entity.ar003 = result;
            entity.ar004 = DateTime.Now;
            entity.ar005 = 1;
            entity.u001 = user.u001;
            entity.u002 = user.u002;
            entity.ar009 = (byte)allowChild;
            entity.u003 = string.IsNullOrEmpty(user.u003) ? null : user.u003;
            mr = AddAutoReg(entity);
            return mr;
        }
        public MR SetAutoRegCount(string code)
        {
            var oldData = GetAutoReg(code);
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    oldData.ar008 += 1;
                    db.Repositorywgs035.Update(oldData);
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
        public int GetAutoRegCount(int userID)
        {
            int result = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = db.Repositorywgs035.Count(exp=>exp.u001 == userID);
                }
            }
            return result;
        }
        public int GetAGUStatus(int userID)
        {
            int result = 0;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.RequiresNew, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    result = (int)db.Repositorywgs012.IQueryable(exp => exp.u001 == userID).Select(exp => exp.u008).FirstOrDefault();
                }
            }
            return result;
        }
        public List<DBModel.OnlineReport> GetOnlineReport(DateTime? dt)
        {
            List<DBModel.OnlineReport> list = new List<DBModel.OnlineReport>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var days = DateTime.DaysInMonth(dt.Value.Year, dt.Value.Month);
                    for (int i = 1; i <= days; i++)
                    {
                        var dts = DateTime.Parse(dt.Value.Year + "-"+dt.Value.Month+"-"+ i + " 00:00:00.000");
                        var dte = DateTime.Parse(dt.Value.Year + "-" + dt.Value.Month + "-" + i + " 23:59:59.999");
                        var tempCount = db.SqlQuery<int>("SELECT COUNT(distinct u002) AS OnlineCount FROM wgs026(NOLOCK) WHERE ulg002>={0} AND ulg002 <= {1};", dts, dte);
                        foreach (var item in tempCount)
                        {
                            var obj = new DBModel.OnlineReport();
                            obj.Date = dts;
                            obj.RecordCount = item;
                            list.Add(obj);

                            
                        }
                    }
                }
            }
            return list;
        }
        public List<DBModel.OnlineReport> GetRegReport(DateTime? dt)
        {
            List<DBModel.OnlineReport> list = new List<DBModel.OnlineReport>();
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadUncommitted;
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    var days = DateTime.DaysInMonth(dt.Value.Year, dt.Value.Month);
                    for (int i = 1; i <= days; i++)
                    {
                        var dts = DateTime.Parse(dt.Value.Year + "-" + dt.Value.Month + "-" + i + " 00:00:00.000");
                        var dte = DateTime.Parse(dt.Value.Year + "-" + dt.Value.Month + "-" + i + " 23:59:59.999");
                        var tempCount = db.SqlQuery<int>("SELECT COUNT(u001) AS RegCount FROM wgs049(NOLOCK) WHERE u005>={0} AND u005 <= {1};", dts, dte);
                        foreach (var item in tempCount)
                        {
                            var obj = new DBModel.OnlineReport();
                            obj.Date = dts;
                            obj.RecordCount = item;
                            list.Add(obj);
                        }
                    }
                }
            }
            return list;
        }
    }
}