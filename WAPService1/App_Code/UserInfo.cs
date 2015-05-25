using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// UserInfo 
/// </summary>
[WebService(Namespace = "WAPService")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class UserInfo : System.Web.Services.WebService {
    

    
    public UserInfo () {
    }

    [WebMethod]
    public ObjMsg GetAccount(string webSite, int userID, string SCode)
    {
        try
        {

            GameServices.ISystem serSystem = new GameServices.System();
            GameServices.IUser serUser = new GameServices.User();
            GameServices.IGame serGame = new GameServices.Game();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode,userID);
            if(newScode == "-1") return new WAPHelpers().getMst("未知错误");
           
            //正常获取用户资料
            var dicLeveName = serUser.GetUserLevel(true);
            var agPointList = serUser.GetAGUPData(userID);
            var gameClassList = serGame.GetGameClassListByCache();
            var defaultUser = serUser.GetAGU(userID);
            var posLevel = serUser.GetUserPositionLevel(false);
            var defaultPos = posLevel.Where(exp => exp.Level == defaultUser.u013).FirstOrDefault();

            //使用自定义类型
            MyInfo ui = new MyInfo();
            ui.AcctLevelList =  serUser.GetAccountLevel(true);
            ui.AGPosID = defaultUser.u013;
            ui.UILoginID = userID;
            ui.UILoginAccount = defaultUser.u002.Trim();
            ui.UILoginNickname = string.IsNullOrEmpty(defaultUser.u003) ? string.Empty : defaultUser.u003.Trim();
            ui.AGSMoney = defaultUser.wgs014.uf001;
            ui.AGSHoldMoney = defaultUser.wgs014.uf003;
            ui.AGSPoint = defaultUser.wgs014.uf004;
            ui.AGLevel = defaultUser.u015;
            //ui.AGLevelName = dicLeveName;
            ui.AGAcctLevel = defaultUser.u018;
            ui.AGStock = defaultUser.u024 * 100;
            ui.AGPosName = defaultPos == null ? string.Empty : defaultPos.Name;
            //ui.GCList = gameClassList;
            //ui.AGPoint = agPointList;
            //ui.GDicList = serGame.GetGameListByCache().ToDictionary(key => key.g001);
            //ui.GPList = serGame.GetGameClassPrizeByCache();
            //ui.GList = serGame.GetGameListByCache();

            ObjMsg ob = new ObjMsg();
            ob.Code = "0000";
            ob.Message = "OK";
            ob.Scode = newScode;
            ob.Data = ui;

            return ob;
        }
        catch
        {
            return new WAPHelpers().getMst("未知错误");
        }
    }


    [WebMethod]
    public UserInfoType GetMyInfo(int UserID, string SCode)
    {
        try
        {
            UserInfoType ui = new UserInfoType();
            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                ui.Code = "0024";
                ui.Message = "用户登陆错误";
                return ui;
            }
            ui.Scode = newScode;

            GameServices.User serUser = new GameServices.User();
            GameServices.Game serGame = new GameServices.Game();
            DBModel.wgs012 UILoginUser = new DBModel.wgs012();
            UILoginUser = serUser.GetAGU(UserID);


            var dicLeveName = serUser.GetUserLevel(true);
            var agPointList = serUser.GetAGUPData(UILoginUser.u001);
            var gameClassList = serGame.GetGameClassListByCache();
            var defaultUser = serUser.GetAGU(UILoginUser.u001);
            var posLevel = serUser.GetUserPositionLevel(false);
            var defaultPos = posLevel.Where(exp => exp.Level == defaultUser.u013).FirstOrDefault();

            var acctLevelList = (List<DBModel.SysAccountLevel>)serUser.GetAccountLevel(true);
            var acctLevelDicList = acctLevelList.ToDictionary(exp => exp.Level);

            var levelDicList = (Dictionary<int, string>)dicLeveName;


            ui.MyCode = UILoginUser.u002.Trim();
            ui.MName = string.IsNullOrEmpty(UILoginUser.u003) ? string.Empty : UILoginUser.u003.Trim();
            ui.MBonus = (defaultUser.u024 * 100).ToString();
            ui.MClass = acctLevelDicList[(int)defaultUser.u018].Name;
            ui.MMoney = defaultUser.wgs014.uf001;
            ui.MStopMoney = defaultUser.wgs014.uf003;
            ui.MPoints = defaultUser.wgs014.uf004;
            ui.VIP = levelDicList[defaultUser.u015];
            ui.Grade = defaultPos == null ? string.Empty : defaultPos.Name;
            
            var gpDicList = serGame.GetGameClassPrizeByCache().ToDictionary(exp => exp.gp001);
            var gcDicList = gameClassList.ToDictionary(exp => exp.gc001);
            var gDicList = serGame.GetGameListByCache().ToDictionary(exp => exp.g001);

            GameRebateType rt;
            List<GameRebateType> rtt = new List<GameRebateType>();

            foreach (var agp in agPointList)
            {
                rt = new GameRebateType();
                var gpkey = agp.gp001;
                var gcKey = gpDicList[(int)agp.gp001].gc001;
                var gcName = gcDicList[gpDicList[(int)agp.gp001].gc001].gc003;
                var gcState = gcDicList[gpDicList[(int)agp.gp001].gc001];
                var gameIDs = gcDicList[gpDicList[(int)agp.gp001].gc001].gc004.Split(',');
                if (0 == gcState.gc006) continue;

                rt.GameType = gcName;

                rt.GameName = "";
                foreach (var g in gameIDs)
                {
                    rt.GameName += gDicList[int.Parse(g)].g003 + "|";
                }

                rt.GameRebat = agp.up003;

                rtt.Add(rt);
            }

            ui.GameRebate = rtt;

            ui.Code = "0000";
            ui.Message = "OK";


            //ViewData["AGPosID"] = defaultUser.u013;
            //ViewData["UILoginID"] = UILoginUser.u001;
            
            //ViewData["AGSMoney"] = defaultUser.wgs014.uf001;
            
            //ViewData["GDicList"] = serGame.GetGameListByCache().ToDictionary(key => key.g001);
            
            //ViewData["GList"] = serGame.GetGameListByCache();



            return ui;
        }
        catch (Exception ex)
        {
            UserInfoType ui = new UserInfoType();
            ui.Code = "9999";
            ui.Message = ex.Message.ToString();
            return ui;
        }
    }


    
}
