using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using _NWC = NETCommon;
using System.Text.RegularExpressions;
using GameServices;

/// <summary>
/// UserTeam 的摘要说明
/// </summary>
[WebService(Namespace = "WAPService")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class UserTeam : System.Web.Services.WebService {

    GameServices.User serUser = new GameServices.User();
    GameServices.Game serGame = new Game();

    public UserTeam () {
    }


    //添加下级账号
    [WebMethod]
    public ObjMsg AddMyUser(int UserID, string SCode, string DUCode, string DName, string DPWD, int DCrreateDown,
       string Pointid, string Point)
    {



        try
        {

            ObjMsg ob = new ObjMsg();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                ob.Code = "0024";
                ob.Message = "用户登陆错误";
                return ob;
            }
            ob.Scode = newScode;



            var _username = string.IsNullOrEmpty(DUCode) || string.IsNullOrWhiteSpace(DUCode) ? string.Empty : DUCode;
            var _nickname = string.IsNullOrEmpty(DName) || string.IsNullOrWhiteSpace(DName) ? string.Empty : DName;
            var _password = string.IsNullOrEmpty(DPWD) || string.IsNullOrWhiteSpace(DPWD) ? string.Empty : DPWD;
            var _allow_child = _NWC.GeneralValidate.IsNumber(DCrreateDown.ToString()) == false ? 0 : DCrreateDown >= 1 ? 1 : 0;
            var _pointid = string.IsNullOrEmpty(Pointid) || string.IsNullOrWhiteSpace(Pointid) ? string.Empty : Pointid;
            var _point = string.IsNullOrEmpty(Point) || string.IsNullOrWhiteSpace(Point) ? string.Empty : Point;
            var _n_min = int.Parse(new WAPHelpers().DicKV["AGU_REGISTER_ACCOUNT_MIN"].cfg003);
            var _n_max = int.Parse(new WAPHelpers().DicKV["AGU_REGISTER_ACCOUNT_MAX"].cfg003);
            var _acct_rule = new WAPHelpers().DicKV["AGU_REGISTER_ACCOUNT_RULE"].cfg003;
            var _acct_rule_text = new WAPHelpers().DicKV["AGU_REGISTER_ACCOUNT_RULE"].cfg005;
            var _nn_max = int.Parse(new WAPHelpers().DicKV["AGU_REGISTER_NICKNAME_LEN"].cfg003);
            if (string.IsNullOrEmpty(_pointid) || string.IsNullOrEmpty(_point))
            {
                ob.Code = "0015";
                ob.Message = "返点信息不正确";
                return ob;
            }
            if (true == string.IsNullOrEmpty(_username))
            {
                ob.Code = "0016";
                ob.Message = "账号不能为空";
                return ob;

            }
            if (false == Regex.IsMatch(_username, _acct_rule))
            {
                ob.Code = "0017";
                ob.Message = string.Format("账号格式错误，{0}", _acct_rule_text);
                return ob;

            }
            if (_username.Length < _n_min || _username.Length > _n_max)
            {
                ob.Code = "0018";
                ob.Message = string.Format("账号长度错误，最小{0}、最大{1}", _n_min, _n_max);
                return ob;

            }
            if (true == string.IsNullOrEmpty(_nickname))
            {
                ob.Code = "0019";
                ob.Message = "昵称不能为空";
                return ob;

            }

            if (_nickname.Length > _nn_max)
            {
                ob.Code = "0020";
                ob.Message = "昵称太长，最长" + _nn_max;
                return ob;
            }

            if (string.IsNullOrEmpty(_password))
            {
                ob.Code = "0021";
                ob.Message = "密码不能为空";
                return ob;
            }

            var regGPDList = _pointid.Split(',');
            var regPoint = _point.Split(',');
            List<DBModel.wgs017> regPointList = new List<DBModel.wgs017>();

            

            var parentGPDList = serUser.GetAGUPData(UserID);
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
            user.u002 = _username;
            user.u003 = _nickname;
            user.u009 = _password;
            user.u012 = UserID;
            user.u008 = 1;
            user.u020 = _allow_child;
            MR mr = serUser.AddAGU(user, newGPDList);

            if (0 == mr.Code)
            {
                ob.Code = "9999";
                ob.Message = mr.Message;
            }
            else
            {
                ob.Code = "0000";
                ob.Message = "OK";
            }

            return ob;
        }
        catch (Exception ex)
        {
            ObjMsg ob = new ObjMsg();
            ob.Code = "9999";
            ob.Message = ex.Message.ToString();
            return ob;
        }
    }

    [WebMethod]
    public ObjMsg UpMyUserInfo(int UserID, string SCode, string DName, 
        double DCCSRebate, double DHappyRebate, double DTenRebate, double DBeiJinRebate, double DThreeRebate, double DRankFive)
    {
        ObjMsg ob = new ObjMsg();
        ob.Code = "9999";
        ob.Message = "功能未开放";
        return ob;
    }

    [WebMethod]
    public TeamMoney GetTeamMoney(int UserID, string SCode, int IsMe)
    {
        try
        {
            TeamMoney tm = new TeamMoney();
            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                tm.Code = "0024";
                tm.Message = "用户登陆错误";
                return tm;
            }
            tm.Scode = newScode;

            var _self = IsMe;
            var self = _self;
            if (2 < self || 1 > self)
            {
                self = 1;
            }

            tm.TeamMoneyx  = serUser.GetMyTeamTotal(UserID, self, 1);
            tm.TeamPoints= serUser.GetMyTeamTotal(UserID, self, 2);
            tm.TeamStop  = serUser.GetMyTeamTotal(UserID, self, 3);
            tm.TeamForMe = self;

            return tm;

        }
        catch (Exception ex)
        {
            TeamMoney tm = new TeamMoney();
            tm.Code = "9999";
            tm.Message = ex.Message.ToString();
            return tm;
        }

    }


    [WebMethod]
    public ObjMsg AddPostUrl(int UserID, string SCode, int IsOpenLower,
        string Pointid, string Point)
    {
        try
        {
            ObjMsg ob = new ObjMsg();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                ob.Code = "0024";
                ob.Message = "用户登陆错误";
                return ob;
            }
            ob.Scode = newScode;

            var _pIDs = Pointid;
            var _pVals = Point;
            var _allow_child = IsOpenLower;
            var mr = serUser.AddAutoReg(UserID, _pIDs, _pVals, _allow_child);

            ob.Code = "9999";
            ob.Message = mr.Message;

            return ob;
        }
        catch(Exception ex)
        {
            ObjMsg tm = new ObjMsg();
            tm.Code = "9999";
            tm.Message = ex.Message.ToString();
            return tm;
        }
    }

    [WebMethod]
    public PostList GetPostList(int UserID, string SCode)
    {
        try
        {
            PostList po = new PostList();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                po.Code = "0024";
                po.Message = "用户登陆错误";
                return po;
            }
            po.Scode = newScode;

            var autoList = serUser.GetAutoRegList(UserID);
            var myPDList = serUser.GetAGUPData(UserID);
            var GCList = serGame.GetGameClassListByCache();
            var gcDicList = GCList.ToDictionary(exp => exp.gc001);

            PostType pt;
            List<PostType> ptt = new List<PostType>();
            PostClass pc;
            List<PostClass> pcc = new List<PostClass>();

            if (null != autoList)
            {
                foreach (var item in autoList)
                {
                    pt = new PostType();
                    pt.TheUrl = "Register.html?code=" + item.ar002;
                    pt.AddCount = item.ar008;
                    pt.IsOpenLower = item.ar009;
                    pt.CreateTime = (DateTime)item.ar004;
                    pt.EID = item.ar001;

                    var pointInfo = item.ar003.Split(',');
                    foreach (var pi in pointInfo)
                    {
                        pc = new PostClass();
                        var p = pi.Split('|');
                        pc.PName = gcDicList[int.Parse(p[0])].gc003;
                        pc.Pint = p[1];
                        pcc.Add(pc);
                    }

                    pt.RebateInfo = pcc;
                    ptt.Add(pt);
                }
            }

            po.PList = ptt;
            return po;
        }
        catch (Exception ex)
        {
            PostList po = new PostList();
            po.Code = "9999";
            po.Message = ex.Message.ToString();
            return po;
        }
    }
    
    [WebMethod]
    public ObjMsg DElExtension(int UserID, string SCode, int EID)
    {
        try
        {
            ObjMsg ob = new ObjMsg();
            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                ob.Code = "0024";
                ob.Message = "用户登陆错误";
                return ob;
            }
            ob.Scode = newScode;
            serUser.DeleteAutoReg(UserID, EID);
            return ob;
        }
        catch(Exception ex)
        {
            ObjMsg ob = new ObjMsg();
            ob.Code = "9999";
            ob.Message = ex.Message.ToString();
            return ob;
        }
    }


    [WebMethod]
    public MyDownUserList GetMyDwonUserList(int UserID, string SCode, int Pindex,
        string UserName, DateTime RSTime, DateTime RETime, DateTime LSTime, DateTime LETime,
        int StateID, int MoneyType, string Money, int PointsType, string Points, int ParentID)
    {

        try
        {
            MyDownUserList du = new MyDownUserList();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                du.Code = "0024";
                du.Message = "用户登陆错误";
                return du;
            }
            du.Scode = newScode;

            var _parentID = ParentID == -1 ? UserID : ParentID;
            var _userName = string.IsNullOrEmpty(UserName) ? string.Empty : UserName;
            var _status = StateID == -1 ? -1 : StateID;
            var _rDTS = RSTime;
            var _rDTE = RETime;
            var _lDTS = LSTime;
            var _lDTE = LETime;
            var _amtT = MoneyType == 0 ? 0 : MoneyType;
            var _amtV = _NWC.GeneralValidate.IsNumber(Money) == false ? 0 : int.Parse(Money);
            var _pntT = PointsType == 0 ? 0 : PointsType;
            var _pntV = _NWC.GeneralValidate.IsNumber(Points) == false ? 0 : int.Parse(Points);
            var parentID = _parentID;
            var userName = _userName;
            var status = _status;
            var rDTS = _rDTS == null ? (DateTime?)null : _rDTS;
            var rDTE = _rDTE == null ? (DateTime?)null : _rDTE;
            var lDTS = _lDTS == null ? (DateTime?)null : _lDTS;
            var lDTE = _lDTE == null ? (DateTime?)null : _lDTE;
            var amtT = _amtT;
            var amtV = _amtV;
            var pntT = _pntT;
            var pntV = _pntV;

            int recordCount = 0;
            int PageSize = new WAPHelpers().PageSize;

            var userPackList = serUser.GetAGUList(UserID, parentID, _userName, status, amtT, amtV, pntT, pntV, (int)Pindex, PageSize, rDTS, rDTE, lDTS, lDTE, out recordCount);

            du.PageMaxCount = recordCount;
            du.PageSize = PageSize;

            MyDownUserType dt;
            List<MyDownUserType> dtl = new List<MyDownUserType>();
            var vipDicList = serUser.GetUserLevel(true);
            var posDicList = serUser.GetUserPositionLevel(false).ToDictionary(exp => exp.Level);

            foreach (var item in userPackList)
            {
                dt = new MyDownUserType();

                dt.LID = item.User.u001;
                dt.UserName = item.User.u002.Trim();
                dt.LCount = item.ChildCount;
                dt.Money = item.User.wgs014.uf001;
                dt.StopMoney = item.User.wgs014.uf003;
                dt.Points = item.User.wgs014.uf004;
                dt.CreateTime = item.User.u005;
                dt.LogInfo = item.User.u007 + "|" + item.User.u002;
                if (item.User.u008 == 0) dt.State = "停用";
                if (item.User.u008 == 0) dt.State = "正常";
                if (item.User.u008 == 0) dt.State = "暂停";
                if (item.User.u008 == 0) dt.State = "冻洁";
                else dt.State = "未知";

                dt.VIP = vipDicList[item.User.u015];
                dt.Grade = posDicList[item.User.u013].Name;
                dt.Bonus = ((int)item.User.u024 * 100).ToString() + "%";

                dtl.Add(dt);

            }

            du.MyDownUserLists = dtl;

            du.Code = "0000";
            du.Message = "OK";

            return du;
        }
        catch(Exception ex)
        {
            MyDownUserList du = new MyDownUserList();
            du.Code = "9999";
            du.Message = ex.Message.ToString();
            return du;
        }
    }


    [WebMethod]
    public MyDownUserInfo GetMyDownUserInfo(int UserID, string SCode, int LUID)
    {
        try
        {
            MyDownUserInfo di = new MyDownUserInfo();
            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                di.Code = "0024";
                di.Message = "用户登陆错误";
                return di;
            }
            di.Scode = newScode;

            var editUser = serUser.GetAGU(LUID);
            if (null == editUser)
            {
                di.Code = "0060";
                di.Message = "数据不存在";
                return di;
            }

            var pDataList = serUser.GetAGUPData(editUser.u001).OrderBy(exp => exp.gc001).ToList();
            var pDataPList = serUser.GetAGUPData(editUser.u012).OrderBy(exp => exp.gc001).ToList();
            List<object> pdl = new List<object>();
            var gameClassList = serGame.GetGameClassListByCache();

            MyDownDRebateType ut;
            List<MyDownDRebateType> utl = new List<MyDownDRebateType>();
            foreach (var pd in pDataList)
            {
                var parentPDataItem = pDataPList.Where(exp => exp.u001 == editUser.u012 && exp.gc001 == pd.gc001).FirstOrDefault();

                ut = new MyDownDRebateType();
                ut.GameName = gameClassList.Where(exp => exp.gc001 == pd.gc001).FirstOrDefault().gc003;
                ut.GameID = pd.up001;
                ut.DRebateValue = pd.up003;

                utl.Add(ut);

                //pdl.Add(new { GameClassName = gameClassList.Where(exp => exp.gc001 == pd.gc001).FirstOrDefault().gc003, PID = pd.up001, Point = pd.up003, MaxPoint = parentPDataItem.up003 });
            }

            di.UserName = editUser.u002.Trim();
            di.NickName = string.IsNullOrEmpty(editUser.u003) ? "" : editUser.u003.Trim();
            di.DCreateTime = editUser.u005;
            di.DBonus = (editUser.u024 * 100).ToString();
            di.DRebate = utl;

            di.Code = "0000";
            di.Message = "OK";


            return di;
        }
        catch (Exception ex)
        {
            MyDownUserInfo di = new MyDownUserInfo();
            di.Code = "9999";
            di.Message = ex.Message.ToString();
            return di;
        }
    }
}
