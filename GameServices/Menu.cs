using DBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameServices
{
    public class Menu : IMenu
    {
        private string menuCacheKey = "MenuCacheKey";
        public MR AddMenu(DBModel.wgs004 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs004.Add(entity);
                    db.SaveChanges();
                    ClearMenuListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public MR UpdateMenu(DBModel.wgs004 entity)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs004.Update(entity);
                    db.SaveChanges();
                    ClearMenuListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public MR UpdateMenu(List<DBModel.wgs004> entityList)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs004.UpdateList(entityList);
                    db.SaveChanges();
                    ClearMenuListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public MR DeleteMenu(int key)
        {
            MR mr = new MR();
            using (UnitOfWork db = new UnitOfWork())
            {
                try
                {
                    db.Repositorywgs004.Delete(db.Repositorywgs004.GetByPrimaryKey(key));
                    db.SaveChanges();
                    ClearMenuListCache();
                }
                catch (Exception error)
                {
                    mr.Exception = MyException.GetInnerException(error);
                    mr.Message = mr.Exception.Message;
                }
            }
            return mr;
        }
        public DBModel.wgs004 GetMenu(int key)
        {
            DBModel.wgs004 entity = null;
            using (UnitOfWork db = new UnitOfWork(true))
            {
                entity = db.Repositorywgs004.GetByPrimaryKey(key);
            }
            return entity;
        }
        public List<DBModel.wgs004> GetMenuList(int parentID, int typeID, int enable)
        {
            List<DBModel.wgs004> list = GetMenuListByCache();
            List<DBModel.wgs004> resultList;
            if (-1 == enable)
            {
                resultList = list.Where(exp => exp.sm002 == parentID && exp.sm005 == typeID).OrderBy(exp => exp.sm012).ToList();
            }
            else
            {
                resultList = list.Where(exp => exp.sm002 == parentID && exp.sm005 == typeID && exp.sm009 == enable).OrderBy(exp => exp.sm012).ToList();
            }
            return resultList;
        }
        public List<DBModel.wgs004> GetMenuListByCache()
        {
            List<DBModel.wgs004> list = (List<DBModel.wgs004>)NETCommon.GeneralCache.Get(menuCacheKey);
            if ( null == list)
            {
                using (UnitOfWork db = new UnitOfWork(true))
                {
                    list = db.Repositorywgs004.GetAll().ToList();
                    NETCommon.GeneralCache.Set(menuCacheKey, list);
                }
            }
            return list;
        }

        public List<DBModel.wgs004> GetMenuListByManage(DBModel.wgs016 entity)
        {
            List<DBModel.wgs004> newmenuList;
            using (UnitOfWork db = new UnitOfWork())
            {
                  List<DBModel.wgs004> menuList = GetMenuListByCache();
                  var old16Entity = db.Repositorywgs016.GetByPrimaryKey(entity.mu001);
                  var old015Entity = db.Repositorywgs015.GetOne(exp => exp.pg001 == old16Entity.pg001);
                  string [] newMenuIDs = old015Entity.pg005.Split(','); 
                  int[] userids;
                  userids = Array.ConvertAll<string, int>(newMenuIDs, s => int.Parse(s));
                  newmenuList = menuList.Where(exp => userids.Contains(exp.sm001)).ToList(); ;
             }
            return newmenuList;
        }

        public void ClearMenuListCache()
        {
            NETCommon.GeneralCache.Clear(menuCacheKey);
        }
    }
}
