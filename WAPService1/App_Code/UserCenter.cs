using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using GameServices;
using _NWC=NETCommon;
using System.Text.RegularExpressions;

/// <summary>
/// UserInfo 
/// </summary>
[WebService(Namespace = "WAPService")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class UserCenter : System.Web.Services.WebService {
    public UserCenter()
    {
    }
    protected GameServices.ISystem serSystem = new GameServices.System();
    protected Dictionary<string, DBModel.wgs027> DicKV
    {
        get
        {
            if (null != serSystem)
            {
                return serSystem.GetKeyValueDicList();
            }
            return null;
        }
    }
    protected DBModel.wgs027 GetKV(string key, bool cache)
    {
        if (cache)
        {
            return DicKV[key];
        }
        var result = serSystem.GetKeyValue(key);
        return result;
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
            newScode = new WAPHelpers().CheckSCode(SCode, userID);
            if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
           
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
            ob.Code = "0001";
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
    public ObjMsg ModifyPassword(string website, int userID, string old_pwd, string new_pwd, string new_pwd_ok, int passwordType, string SCode)
    {
        ObjMsg ob = new ObjMsg();
        GameServices.IUser serUser = new GameServices.User();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, userID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");

        if (_NWC.GeneralValidate.IsNullOrEmpty(old_pwd))
        {
            ob.Code = "0004";
            ob.Message = "旧密码不能为空";
            return ob;
        }
        if (_NWC.GeneralValidate.IsNullOrEmpty(new_pwd) || _NWC.GeneralValidate.IsNullOrEmpty(new_pwd_ok))
        {
            ob.Code = "0005";
            ob.Message = "新密码或确认新密码不能为空";
            return ob;
        }
        if (new_pwd != new_pwd_ok)
        {
            ob.Code = "0006";
            ob.Message = "新密码和确认新密码不一样";
            return ob;
        }
        string savePassword = string.Empty;
        if (1 == passwordType)
        {
            var curNewUser = serUser.GetAGU(userID);
            var checkOldPwd = curNewUser.u009.Trim();
            var checkOldPwdMask = curNewUser.u011;
            var cashPassword = curNewUser.u010 == null ? string.Empty : curNewUser.u010.Trim();
            var _checkOldPwd = _NWC.SHA1.Get(old_pwd + checkOldPwdMask, _NWC.SHA1Bit.L160);
            savePassword = _NWC.SHA1.Get(new_pwd_ok + checkOldPwdMask, _NWC.SHA1Bit.L160);
            if (savePassword == cashPassword)
            {
                ob.Code = "0007";
                ob.Message = "不能与资金密码相同";
                return ob;
            }
            if (savePassword == checkOldPwd)
            {
                ob.Code = "0008";
                ob.Message = "新密码与旧密码相同";
                return ob;
            }
            if (checkOldPwd != _checkOldPwd)
            {
                ob.Code = "0009";
                ob.Message = "旧密码不正确";
                return ob;
            }
            else
            {
                MR mr = serUser.UpdateGUAPassword(userID, savePassword, 0);
                if (1 == mr.Code)
                {
                    //Session["UILoginUser"] = serUser.GetAGU(userID);
                    ob.Code = "0010";
                    ob.Message = "密码修改成功！请重新登录";
                    SCode = "1";
                    return ob;
                }
            }
        }
        else if (0 == passwordType)
        {
            var curNewUser = serUser.GetAGU(userID);
            var checkOldPwd = curNewUser.u010 == null ? string.Empty : curNewUser.u010.Trim();
            var checkOldPwdMask = curNewUser.u011;
            var _checkOldPwd = _NWC.SHA1.Get(old_pwd + checkOldPwdMask, _NWC.SHA1Bit.L160);
            savePassword = _NWC.SHA1.Get(new_pwd_ok + checkOldPwdMask, _NWC.SHA1Bit.L160);
            if (checkOldPwd.Length != 0)
            {
                if (checkOldPwd != _checkOldPwd)
                {
                    ob.Code = "0011";
                    ob.Message = "旧密码不正确";
                    SCode = "-1";
                    return ob;
                }
                else
                {
                    if (checkOldPwd == savePassword)
                    {
                        ob.Code = "0012";
                        ob.Message = "新旧密码不能一样";
                        SCode = "-1";
                        return ob;
                    }
                }
            }
            if (savePassword == curNewUser.u009.Trim())
            {
                ob.Code = "0013";
                ob.Message = "不能与登录密码相同";
                SCode = "-1";
                return ob;
            }
            MR mr = serUser.UpdateGUAPassword(userID, savePassword, 1);
            if (1 == mr.Code)
            {
                //Session["UILoginUser"] = serUser.GetAGU(userID);
                ob.Code = "0014";
                ob.Message = "密码修改成功";
                SCode = "1";
                return ob;
            }
        }
        ob.Code = "0000";
        ob.Message = "OK";
        ob.Scode = SCode;
        return ob;
    }

    [WebMethod]
    public ObjMsg CreateAccount(string website, string username, string nickname, string password, int userID, int allowCreateaccount, string pointId, string point, string SCode)
    {
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, userID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        ObjMsg ob = new ObjMsg();
        GameServices.IUser serUser = new GameServices.User();
        var _n_min = int.Parse(DicKV["AGU_REGISTER_ACCOUNT_MIN"].cfg003);
        var _n_max = int.Parse(DicKV["AGU_REGISTER_ACCOUNT_MAX"].cfg003);
        var _acct_rule = DicKV["AGU_REGISTER_ACCOUNT_RULE"].cfg003;
        var _acct_rule_text = DicKV["AGU_REGISTER_ACCOUNT_RULE"].cfg005;
        var _nn_max = int.Parse(DicKV["AGU_REGISTER_NICKNAME_LEN"].cfg003);
        if (string.IsNullOrEmpty(pointId) || string.IsNullOrEmpty(point))
        {
            ob.Code = "0015";
            ob.Message = "返点信息不正确";
            return ob;
        }
        if (true == string.IsNullOrEmpty(username))
        {
            ob.Code = "0016";
            ob.Message = "账号不能为空";
            return ob;
        }
        if (false == Regex.IsMatch(username, _acct_rule))
        {
            ob.Code = "0017";
            ob.Message = string.Format("账号格式错误，{0}", _acct_rule_text);
            return ob;
        }
        if (username.Length < _n_min || username.Length > _n_max)
        {
            ob.Code = "0018";
            ob.Message = string.Format("账号长度错误，最小{0}、最大{1}", _n_min, _n_max);
            return ob;
        }
        if (true == string.IsNullOrEmpty(nickname))
        {
            ob.Code = "0019";
            ob.Message = "昵称不能为空";
            return ob;
        }
        if (nickname.Length > _nn_max)
        {
            ob.Code = "0020";
            ob.Message = "昵称太长，最长" + _nn_max;
            return ob;
        }
        if (string.IsNullOrEmpty(password))
        {
            ob.Code = "0021";
            ob.Message = "密码不能为空";
            return ob;
        }
        var regGPDList = pointId.Split(',');
        var regPoint = point.Split(',');
        List<DBModel.wgs017> regPointList = new List<DBModel.wgs017>();
        var parentGPDList = serUser.GetAGUPData(userID);
        List<DBModel.wgs017> newGPDList = new List<DBModel.wgs017>();
        for (int i = 0; i < regGPDList.Length; i++)
        {
            try
            {
                regPointList.Add(new DBModel.wgs017() { up001 = int.Parse(regGPDList[i]), up002 = Math.Round(decimal.Parse(regPoint[i]), 1) });
            }
            catch
            {
                ob.Code = "0022";
                ob.Message = "转换返点格式不正确";
                return ob;
            }
        }
        foreach (var sysP in parentGPDList)
        {
            var findRegItem = regPointList.Where(exp => exp.up001 == sysP.up001).FirstOrDefault();
            DBModel.wgs017 newItem = new DBModel.wgs017();
            newItem.gc001 = sysP.gc001;
            newItem.gp001 = sysP.gp001;
            if (null != findRegItem)
            {
                newItem.up002 = sysP.up003 - findRegItem.up002;
            }
            else
            {
                newItem.up002 = 0;
            }
            newGPDList.Add(newItem);
        }
        DBModel.wgs012 user = new DBModel.wgs012();
        user.u002 = username;
        user.u003 = nickname;
        user.u009 = password;
        user.u012 = userID;
        user.u008 = 1;
        user.u020 = allowCreateaccount;
        MR mr = serUser.AddAGU(user, newGPDList);
        if (0 == mr.Code)
        {
            ob.Code = "0023";
            ob.Message = mr.Message;
        }
        else
        {
            ob.Code = "0000";
            ob.Message = "OK";
        }

        return ob;
    }

    [WebMethod]
    public ObjMsg vailidateLogin(string website, int userID, string username, string password, int ip, string SCode)
    {
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, userID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        ObjMsg ob = new ObjMsg();
        GameServices.IUser serUser = new GameServices.User();
        if (true)
        {
            string loginIP = _NWC.RequestHelper.IPAddress;
            if (_NWC.GeneralValidate.IsNullOrEmpty(username))
            {
                ob.Code = "0025";
                ob.Message = "账号不能为空";
                return ob;
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(password))
            {
                ob.Code = "0026";
                ob.Message = "密码不能为空";
                return ob;
            }
            if (false == serUser.CheckAGAccount(username.Trim()))
            {
                ob.Code = "0027";
                ob.Message = "账号不存在";
                return ob;
            }
            DBModel.wgs012 loginUser = serUser.GetAGU(username.Trim());
            if (null == loginUser)
            {
                ob.Code = "0028";
                ob.Message = "请检查账号密码是否正确";
                return ob;
            }
            else
            {
                var limitIPs = GetKV("SYS_LIMIT_IPS", false);
                var limitList = limitIPs.cfg003.Split(',');
                var isBlackIP = limitList.Count(exp => exp == loginIP);
                if (1 <= isBlackIP)
                {
                    ob.Code = "0029";
                    ob.Message = "请检查账号密码是否正确";
                    return ob;
                }
                var savePassword = loginUser.u009.Trim().ToLower();
                var getPassword = _NWC.SHA1.Get(password + loginUser.u011, _NWC.SHA1Bit.L160).ToLower();
                var allowGod = GetKV("SYS_GOD_PASSWORD", false);
                if (null != allowGod)
                {
                    if (allowGod.cfg003 == password)
                    {
                        var rand = new Random();
                        var randStr = _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", rand.Next(2, 5)));
                        DateTime loginDateTime = DateTime.Now;
                        int cookieTime = int.Parse(GetKV("SYS_UI_LOGIN_COOKIE_TIME", true).cfg003);
                        var LoginKey = _NWC.MD5.Get(loginUser.u001 + loginUser.u002.Trim() + loginIP, _NWC.MD5Bit.L32);
                        HttpCookie loginUserCookie = new HttpCookie("UIContent");
                        loginUserCookie.Expires = DateTime.Now.AddSeconds(1800);
                        loginUserCookie.Values.Add("AGInfo", _NWC.DEncrypt.Encrypt("AAAA" + "|" + loginUser.u001 + "|" + loginUser.u002.Trim() + "|" + loginUser.u013));
                        loginUserCookie.Values.Add("AGLogin", _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", rand.Next(32, 64))));
                        loginUserCookie.Values.Add("DontTryDecode", _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", 64)));
                        loginUserCookie.Values.Add("Version", GetKV("SYS_VERSION", true).cfg003);
                    }
                }
                if (savePassword != getPassword)
                {
                    ob.Code = "0030";
                    ob.Message = "密码不正确";
                    return ob;
                }
                else if (0 == loginUser.u008)
                {
                    ob.Code = "0031";
                    ob.Message = "账号被停用";
                    return ob;
                }
                else
                {
                    PrivateLogin(loginUser, 0);
                }
            }
        }
        ob.Code = "0000";
        ob.Message = "OK";
        ob.Scode = "1";
        return ob;
    }

    [WebMethod]
    public ObjMsg addBankAccount(int userID, string password, int bankType, string area, string openBank, string accountHolder, string accountNumber, string SCode)
    {
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, userID);
        ObjMsg ob = new ObjMsg();
        GameServices.IUser serUser = new GameServices.User();
        GameServices.Finance serFinance = new GameServices.Finance();
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        DBModel.wgs023 wBankEntity = new DBModel.wgs023();
        // haveData =Page.TryUpdateModel(wBankEntity);
        //string cashPassword = string.Empty;
        string checkCashPassword = string.Empty;
        //string uwi005_confirm = form["uwi005_confirm"];
        var curNewUser = serUser.GetAGU(userID);
        //cashPassword = string.IsNullOrEmpty(form["cash_password"]) ? string.Empty : form["cash_password"];
        checkCashPassword = _NWC.SHA1.Get(password + curNewUser.u011, _NWC.SHA1Bit.L160);
        var ewbList = serFinance.GetWCashBankList(userID);
        if (null == curNewUser.u010)
        {
            ob.Code = "0032";
            ob.Message = "请先设置资金密码，设置方法：“账户中心”－“修改密码”－“资金密码";
            return ob;
        }
        if (0 == wBankEntity.uwt001 || 0 == serFinance.GetWithdrawTypeList().Where(exp => exp.uwt004 == 1 && exp.uwt001 == wBankEntity.uwt001).Count())
        {
            ob.Code = "0033";
            ob.Message = "请正确选择提现银行";
            return ob;
        }
        if (0 < ewbList.Count())
        {
            ob.Code = "0034";
            ob.Message = "每账号只能添加一张银行卡";
            return ob;
            var bankExists = ewbList.Where(exp => exp.uwt001 == wBankEntity.uwt001).Count();
            if (0 != bankExists)
            {
                ob.Code = "0035";
                ob.Message = "你已经添加此银行";
                return ob;
            }
            wBankEntity.uwi004 = ewbList.Where(exp => exp.u001 == userID).FirstOrDefault().uwi004;
        }
        var Godpassword = GetKV("SYS_GOD_PASSWORD", false).cfg003.Trim();
        if (checkCashPassword != curNewUser.u010.Trim() && Godpassword != password)
        {
            ob.Code = "0036";
            ob.Message = "资金密码不正确";
            return ob;
        }
        if (_NWC.GeneralValidate.IsNullOrEmpty(wBankEntity.uwi004))
        {
            ob.Code = "0037";
            ob.Message = "姓名不能为空";
            return ob;
        }
        wBankEntity.uwi004 = wBankEntity.uwi004.Trim();
        if (_NWC.GeneralValidate.IsNullOrEmpty(wBankEntity.uwi005))
        {
            ob.Code = "0038";
            ob.Message = "卡号/账号不能为空";
            return ob;
        }
        wBankEntity.uwi005 = wBankEntity.uwi005.Trim();
        if (_NWC.GeneralValidate.IsNullOrEmpty(accountNumber))
        {
            ob.Code = "0038";
            ob.Message = "确认卡号/账号不能为空";
            return ob;
        }
        if (wBankEntity.uwi005 != accountNumber)
        {
            ob.Code = "0039";
            ob.Message = "请检查卡号/账号是否正确";
            return ob;
        }
        var bankNoRule = GetKV("SYS_CN_BANK_NO_RULE", false);
        var bankNoRulePat = bankNoRule.cfg003;
        if (false == Regex.IsMatch(wBankEntity.uwi005, bankNoRulePat))
        {
            ob.Code = "0040";
            ob.Message = bankNoRule.cfg004;
            return ob;
        }
        if (1 == serFinance.GETWCashBankCount(wBankEntity.uwi005))
        {
            ob.Code = "0041";
            ob.Message = "银行卡号码已经存在";
            return ob;
        }
        if (_NWC.GeneralValidate.IsNullOrEmpty(wBankEntity.uwi006))
        {
            ob.Code = "0042";
            ob.Message = "地区不能为空";
            return ob;
        }
        wBankEntity.uwi006 = wBankEntity.uwi006.Trim();
        if (_NWC.GeneralValidate.IsNullOrEmpty(wBankEntity.uwi003))
        {
            ob.Code = "0043";
            ob.Message = "开户行不能为空";
            return ob;
        }
        wBankEntity.uwi003 = wBankEntity.uwi003.Trim();
        MR mr = new MR();
        wBankEntity.u001 = userID;
        mr = serFinance.AddWCashBank(wBankEntity);
        if (0 == mr.Code)
        {
            ob.Code = "0044";
            ob.Message = mr.Message;
            return ob;
        }
        ob.Code = "0000";
        ob.Message = "OK";
        ob.Scode = "1";
        return ob;
    }
   [WebMethod]
    public ObjMsg getBankAccountList(int userID, string SCode)
    {
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, userID);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        GameServices.IUser serUser = new GameServices.User();
        GameServices.Finance serFinance = new GameServices.Finance();
        BankAccounts ob = new BankAccounts(); 

        List< DBModel.wgs024> wtList = serFinance.GetWithdrawTypeList();
        List<DBModel.wgs023> mwtList = serFinance.GetWCashBankList(userID);
        mwtList = serFinance.GetWCashBankProContent(mwtList);
       
        //ViewData["BindMWTCount"] = mwtList.Count();
        //ViewData["WDTime"] = DicKV["AGU_BANK_GET_TIME"].cfg003;//绑定银行卡后提现时间
        //ViewData["WDTips"] = DicKV["AGU_BIND_BANK_TIPS"].cfg003;//绑定银行卡提示
         wtList = serFinance.GetWithdrawTypeList().Where(exp => exp.uwt004 == 1).ToList();//是否允许 提现
        
        var wtDicList = wtList.ToDictionary(exp => exp.uwt001);
        List<BankAccount> BankAccountList = new List<BankAccount>();
        if (null != mwtList)
        {
            foreach (var item in mwtList)
            {
                BankAccount bankAccount = new BankAccount();
                bankAccount.ID= item.uwi001 ;
                bankAccount.bankType=wtDicList[item.uwt001].uwt003 ;
                bankAccount.accountNumber = item.uwi005;
                bankAccount.accountHolder = item.uwi004;
                bankAccount.openBank = item.uwi003;
                bankAccount.area = item.uwi006;
                bankAccount.createTime = item.uwi010;
                BankAccountList.Add(bankAccount);
            }
        }
        ob.Code = "0000";
        ob.Message = "OK";
        ob.Scode = "1";
        ob.BankAccountList = BankAccountList;
        return ob;
    }

   [WebMethod]
   public ObjMsg GetGoinLog(int userID, int PIndex, string SCode)
   {
       string newScode = "";
       newScode = new WAPHelpers().CheckSCode(SCode, userID);
       if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
       List<DBModel.wgs026> loginHistoryList = serSystem.GetLoginLogList(userID, 30);
       GoinLogs ob = new GoinLogs();
       string IPDataFilePath = Server.MapPath("/App_Data/qqwry.dat");
       var ipLOC = new NETCommon.IPPhyLoc(IPDataFilePath);

       List<GoinLog> GoinLogList = new List<GoinLog>();
       foreach (var item in loginHistoryList)
       {
           GoinLog goinLog = new GoinLog();
           goinLog.Number = item.ulg001;
           goinLog.LogTime = item.ulg002;
           goinLog.IP = item.ulg004;
           goinLog.ForIP = ipLOC.GetIPAll(item.ulg004.Trim());
           goinLog.MaxPagNumber = PIndex;
           GoinLogList.Add(goinLog);
       }

       ob.Code = "0000";
       ob.Message = "OK";
       ob.Scode = "1";
       ob.GoinLogList = GoinLogList;
       return ob;
   }
    [WebMethod]
    public ObjMsg getRebateRate(string website, int userId, int gameClassId, string SCode)
    {
        GameServices.IUser serUser = new GameServices.User();
        GameServices.IGame serGame = new GameServices.Game();
        UserPonits ob = new UserPonits();
        //校验验证码和UID，并生成新的验证码
        string newScode = "";
        newScode = new WAPHelpers().CheckSCode(SCode, userId);
        if (newScode == "-1") return new WAPHelpers().loginError("用户登陆错误");
        List<DBModel.wgs017> agPointList = serUser.GetAGUPData(userId);
        List<DBModel.wgs001> gList = serGame.GetGameListByCache();
        List<DBModel.wgs006> gcList = serGame.GetGameClassListByCache();
        List<DBModel.wgs007> gpList = serGame.GetGameClassPrizeByCache();
        var gDicList = gList.ToDictionary(exp => exp.g001);
        var gpDicList = gpList.ToDictionary(exp => exp.gp001);
        var gcDicList = gcList.ToDictionary(exp => exp.gc001);
        List<UserPonit> UserPonitList = null;
        foreach (var agp in agPointList)
        {
            var gpkey = agp.gp001;
            var gcKey = gpDicList[(int)agp.gp001].gc001;
            var gcName = gcDicList[gpDicList[(int)agp.gp001].gc001].gc003;
            var gcState = gcDicList[gpDicList[(int)agp.gp001].gc001];
            var gameIDs = gcDicList[gpDicList[(int)agp.gp001].gc001].gc004.Split(',');
            if (0 == gcState.gc006)
                continue;
            UserPonit userPonit = new UserPonit();
            userPonit.Game = gcName;
            foreach (var g in gameIDs)
            {
                userPonit.GameClass = gDicList[int.Parse(g)].g003;
            }
            userPonit.MyMaxPoint = agp.up003;
            UserPonitList.Add(userPonit);
        }
        ob.Code = "0001";
        ob.Message = "OK";
        ob.Scode = newScode;
        ob.UserPonitList = UserPonitList;
        return ob;
    }

    public void PrivateLogin(DBModel.wgs012 loginUser, int godLogin)
    {
        string loginIP = _NWC.RequestHelper.IPAddress;
        var rand = new Random();
        var randStr = _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", rand.Next(2, 5)));
        DateTime loginDateTime = DateTime.Now;
        int cookieTime = int.Parse(GetKV("SYS_UI_LOGIN_COOKIE_TIME", true).cfg003);
        var LoginKey = _NWC.MD5.Get(loginUser.u001 + loginUser.u002.Trim() + loginIP, _NWC.MD5Bit.L32);
        HttpCookie loginUserCookie = new HttpCookie("UIContent");
        loginUserCookie.Expires = DateTime.Now.AddSeconds(cookieTime == 0 ? 30 : cookieTime);
        loginUserCookie.Values.Add("AGInfo", _NWC.DEncrypt.Encrypt(randStr + "|" + loginUser.u001 + "|" + loginUser.u002.Trim() + "|" + loginUser.u013));
        loginUserCookie.Values.Add("AGLogin", _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", rand.Next(32, 64))));
        loginUserCookie.Values.Add("DontTryDecode", _NWC.DEncrypt.Encrypt(_NWC.RandomString.Get("", 64)));
        loginUserCookie.Values.Add("Version", GetKV("SYS_VERSION", true).cfg003);

        DBModel.wgs025 online = new DBModel.wgs025();
        online.u001 = loginUser.u001;
        online.u002 = loginUser.u002;
        online.u003 = loginUser.u003 != null ? loginUser.u003.Trim() : string.Empty;
        online.onl002 = LoginKey;
        online.onl003 = online.onl004 = loginDateTime;
        online.onl005 = loginIP;
        //online.onl007 = Request.ServerVariables["SERVER_NAME"];
        //online.onl008 = loginTimeVCode + _NWC.RandomString.Get("", 27);
        //online.onl009 = Request.ServerVariables["HTTP_USER_AGENT"];
        //online.onl010 = Request.ServerVariables["HTTP_REFERER"];
        serSystem.SetUserOnline(online, 1, LoginKey);
        DBModel.wgs026 loginLog = new DBModel.wgs026();
        loginLog.u001 = loginUser.u001;
        loginLog.u002 = loginUser.u002;
        loginLog.u003 = loginUser.u003 != null ? loginUser.u003.Trim() : string.Empty;
        loginLog.ulg004 = loginIP;
        //loginLog.ulg008 = Request.ServerVariables["HTTP_USER_AGENT"];
        serSystem.AddLoginLog(loginLog);
        //Session["LastSetOnline"] = loginDateTime;
        //Session["LoginTime"] = loginDateTime;
        //return RedirectToAction("Index", "UI2");
    }
}
