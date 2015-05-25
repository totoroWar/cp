using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameServices
{
    public interface IMenu
    {
        MR AddMenu(DBModel.wgs004 entity);
        MR UpdateMenu(DBModel.wgs004 entity);
        MR UpdateMenu(List<DBModel.wgs004> entityList);
        MR DeleteMenu(int key);
        DBModel.wgs004 GetMenu(int key);
        List<DBModel.wgs004> GetMenuList(int parentID, int typeID, int enable);
        List<DBModel.wgs004> GetMenuListByCache();
        List<DBModel.wgs004> GetMenuListByManage(DBModel.wgs016 entity);
        void ClearMenuListCache();
    }
}
