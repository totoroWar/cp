using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameServices
{
    public interface IUser
    {
        MR AddMGU(DBModel.wgs016 entity);
        DBModel.wgs016 GetMGU(int key);
        DBModel.wgs016 GetMGU(string key);
        MR UpdateMGU(DBModel.wgs016 entity);
        List<DBModel.wgs016> GetMGUList();
        MR AddAGU(DBModel.wgs012 userEntity, List<DBModel.wgs017> userPrizeDataEntityList);
        bool CheckAGAccount(string account);
        List<DBModel.wgs012> GetAGUList(int parentID);
        List<DBModel.wgs014> GetAGUAll();
        List<DBModel.UserPack> GetAGUList(int parentID, string account, int status, int amtT, decimal amtV, int pntT, decimal pntV, DateTime? RDTS, DateTime? RDTE, DateTime? LDTS, DateTime? LDTE, string IP, int pageIndex, int pageSize, out int recordCount, string sysNotSetMomeyAccount);
        List<DBModel.UserPack> GetAGUList(int myUserID, int parentID, string account, int status, int amtT, decimal amtV, int pntT, decimal pntV, int pageIndex, int pageSize, DateTime? rDts, DateTime? rDte, DateTime? lDts, DateTime? lDte, out int recordCount);
        List<DBModel.UserPack> GetAGUAll(int parentID, string account, int status, int amtT, decimal amtV, int pntT, decimal pntV, DateTime? RDTS, DateTime? RDTE, DateTime? LDTS, DateTime? LDTE, string IP, int pageIndex, int pageSize, out int recordCount, string sysNotSetMomeyAccount);
        MR SetUserStatus(List<int> userIDs, int status);
        List<DBModel.wgs048> GetMyChildList(int myUserID, bool includeMySelf);
        int GetAGUCount(int parentID);
        MR CheckChildStockIsMax(int myUserID, decimal myStock);
        List<int> GetAGAParent(int userID);
        int GetAGUAllCount(int userID);
        int GetAGUBaseCount(int userID);
        bool CheckUserIDIsMyChild(int myUserID, int childUserID);
        bool CheckUserIDIsMyFather(int myUserID, int fatherID);
        bool CheckUserIsMyFather(string myUserName, string fatherUserName);
        DBModel.wgs012 GetAGU(int key);
        DBModel.wgs012 GetAGU(string key);
        List<DBModel.wgs017> GetAGUPData(int userID);
        DBModel.wgs017 GetAGUPData(int userID, int gameClassID);
        MR UpdateAGUNickname(int userID, string nickname);
        MR UpdateAGU(DBModel.wgs012 entity);
        MR DeleteUser(int userID);
        MR UpdateGUAPassword(int key, string password, int type);
        MR UpdateGUALM(int key,string message);
        DBModel.wgs014 GetAGUFData(int userID);
        decimal GetMyTeamTotal(int myUserID, int type, int totalType);
        MR CheckNickName(string nickName);
        List<DBModel.wgs035> GetAutoRegList(int myUserID);
        MR DeleteAutoReg(int myUserID, int key);
        MR SaveAGUPData(List<DBModel.wgs017> list);
        Dictionary<int, string> GetUserLevel(bool cache);
        List<DBModel.SysPositionLevel> GetUserPositionLevel(bool cache);
        List<DBModel.SysAccountLevel> GetAccountLevel(bool cache);
        MR AddAutoReg(DBModel.wgs035 entity);
        MR UpdateAutoReg(DBModel.wgs035 entity);
        DBModel.wgs035 GetAutoReg(int key);
        DBModel.wgs035 GetAutoReg(string code);
        MR AddAutoReg(int myUserID, string pdIDs, string points,int allowChild);
        MR SetAutoRegCount(string code);
        int GetAutoRegCount(int userID);
        int GetAGUStatus(int userID);
        List<DBModel.OnlineReport> GetOnlineReport(DateTime? dt);
        List<DBModel.OnlineReport> GetRegReport(DateTime? dt);
    }
}
