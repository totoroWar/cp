using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using _NWC = NETCommon;
using GameServices;
using System.Text.RegularExpressions;


/// <summary>
/// UserMoney 的摘要说明
/// </summary>
[WebService(Namespace = "WAPService")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)] 
[System.Web.Script.Services.ScriptService]
public class UserMoney : System.Web.Services.WebService {
    GameServices.Finance serFinance = new GameServices.Finance();
    GameServices.IUser serUser = new GameServices.User();
    GameServices.Game serGame = new Game();

    public UserMoney () {
    }

   

    [WebMethod]
    public MoneyChangeTypes GetMoneyChangeType(int userID, string SCode)
    {
        try
        {

            MoneyChangeTypes mcs = new MoneyChangeTypes();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, userID);
            if (newScode == "-1")
            {
                mcs.Code = "0024";
                mcs.Message = "用户登陆错误";
                return mcs;
            }
            mcs.Scode = newScode;

            GameServices.System serSystem = new GameServices.System();

            //(Dictionary<int, DBModel.SysDataChangeType>)

            
            var dctList = serSystem.GetSystemDataChangeTypeList(true);
            List<MoneyChangeType> mcss = new List<MoneyChangeType>();


            MoneyChangeType mct;
            foreach (var item in dctList)
            {
                mct = new MoneyChangeType();
                mct.ChangeTypeValue = item.Value.Name;
                mct.ChangeTypeID = item.Value.ID;
                mcss.Add(mct);
            }

            mct = new MoneyChangeType();
            mct.ChangeTypeValue = "全部";
            mct.ChangeTypeID = 0;
            mcss.Add(mct);

            mcs.CList = mcss;
            mcs.Scode = "0000";
            mcs.Message = "OK";

            return mcs;
        }
        catch(Exception ex)
        {
            MoneyChangeTypes mcs = new MoneyChangeTypes();
            mcs.Code = "9999";
            mcs.Message = ex.Message.ToString();
            return mcs;
        }
    
    }

    [WebMethod]
    public MoneyChangeList GetMoneyChangeList(int userID, string SCode, DateTime STime, DateTime ETime, int ChangeTypeID, int PageIndex)
    {
        try
        {

            MoneyChangeList mcs = new MoneyChangeList();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, userID);
            if (newScode == "-1")
            {
                mcs.Code = "0024";
                mcs.Message = "用户登陆错误";
                return mcs;
            }
            mcs.Scode = newScode;

            //GameServices.Finance serFinance = new GameServices.Finance();

            //(Dictionary<int, DBModel.SysDataChangeType>)


            int pagesize = 0;
            pagesize = new WAPHelpers().PageSize;
            var recordCount = 0;
            var dataChageList = serFinance.GetDataChangeList(userID, ChangeTypeID, 0, string.Empty, STime, ETime, pagesize, PageIndex, out recordCount);

            

            List<MoneyChange> mcss = new List<MoneyChange>();
            MoneyChange mct;

            foreach (var item in dataChageList)
            {
                mct = new MoneyChange();
                mct.CCode = item.uxf001;
                mct.CAbout = item.uxf015;

                mct.CChangeMoney = item.uxf003;
                mct.COldMoney = item.uxf002;
                mct.CNewMoney = item.uxf007;

                mct.COldStopMoney = item.uxf010;
                mct.CNewStopMoney = item.uxf009;

                mct.CChangePoints = item.uxf005;
                mct.COldPoints = item.uxf004;
                mct.CNewPoints = item.uxf008;

                mct.CUpTime = item.uxf014;
                
                mcss.Add(mct);
            }


            mcs.CList = mcss;
            mcs.Scode = "0000";
            mcs.Message = "OK";
            mcs.PageMaxCount = recordCount;
            mcs.PageSize = pagesize;

            return mcs;
        }
        catch (Exception ex)
        {
            MoneyChangeList mcs = new MoneyChangeList();
            mcs.Code = "9999";
            mcs.Message = ex.Message.ToString();
            return mcs;
        }
    }

    [WebMethod]
    public BonusList GetBonusList(int userID, string SCode)
    {

        try
        {
            BonusList gbl = new BonusList();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, userID);
            if (newScode == "-1")
            {
                gbl.Code = "0024";
                gbl.Message = "用户登陆错误";
                return gbl;
            }
            gbl.Scode = newScode;

            GameServices.User_AM ua = new GameServices.User_AM();
            List<DBModel.wgs056> xlist = new List<DBModel.wgs056>();

            int pageIndex = 0;
            int recordCount = 0;
            int pagesize = 0;
            pagesize = new WAPHelpers().PageSize;

            xlist = ua.GetFHLogForUser((int)pageIndex, pagesize, out recordCount, userID);

            BonusType bt;
            List<BonusType> bts = new List<BonusType>();

            foreach (DBModel.wgs056 item in xlist)
            {

                bt = new BonusType();
                if (item.s003 < 0) { bt.BMoney = Math.Abs(Convert.ToDecimal(item.s003)); }
                else { bt.BMoney = 0; }
                bt.BMoney = Convert.ToDecimal(bt.BMoney.ToString("N4"));
                bt.BForMoney = (item.s002 * -1);
                bt.CScale = (item.f002 * 100).ToString() + "%";
                bt.CCycle = item.s004;
                bt.CCSTime = item.s006;
                bt.CCETime = item.s007;
                if (item.s005 == 0)
                {
                    bt.CCState = "待审核";
                }
                else if (item.s005 == 1)
                {
                    bt.CCState = "通过";
                }
                else
                {
                    bt.CCState = "取消";
                }

                bts.Add(bt);

            }

            gbl.CList = bts;
            gbl.Scode = "0000";
            gbl.Message = "OK";

            return gbl;

        }
        catch (Exception ex)
        {
            BonusList gbl = new BonusList();
            gbl.Code = "9999";
            gbl.Message = ex.Message.ToString();
            return gbl;
        }




    }

    [WebMethod]
    public ObjMsg Transfer(int UserID, string SCode, string MPWD, string ToAccount, string ToAcountCheck, decimal ToMoney, string ip)
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



            var myInfo = serUser.GetAGU(UserID);
            if (string.IsNullOrEmpty(myInfo.u010))
            {
                ob.Code = "0045";
                ob.Message = "请先设置资金密码，设置方法：“账户中心”－“修改密码”－“资金密码”";

                return ob;
            }

            var _toacct = string.IsNullOrEmpty(ToAccount) || string.IsNullOrWhiteSpace(ToAccount) ? string.Empty : ToAccount;
            var _tofacct = string.IsNullOrEmpty(ToAcountCheck) || string.IsNullOrWhiteSpace(ToAcountCheck) ? string.Empty :ToAcountCheck;
            var _amt = string.IsNullOrEmpty(ToMoney.ToString()) || string.IsNullOrWhiteSpace(ToMoney.ToString()) ? 0m : decimal.Parse(ToMoney.ToString());
            var _pwd = string.IsNullOrEmpty(MPWD) || string.IsNullOrWhiteSpace(MPWD) ? string.Empty : MPWD;

            if (string.IsNullOrEmpty(_pwd))
            {
                
                ob.Code = "0052";
                ob.Message = "资金密码不能为空";
                return ob;
            }

            DBModel.wgs012 UILoginUser = new DBModel.wgs012();
            UILoginUser = serUser.GetAGU(UserID);

           
            var checkPassword = _NWC.SHA1.Get(_pwd + UILoginUser.u011, _NWC.SHA1Bit.L160);
            if (0 != string.Compare(myInfo.u010.Trim(), checkPassword, true))
            {
                ob.Code = "0046";
                ob.Message = "资金密码不正确";
                return ob;
            }
            if (string.IsNullOrEmpty(_toacct))
            {
                ob.Code = "0053";
                ob.Message = "收款账号不能为空";
                return ob;
            }
            if (string.IsNullOrEmpty(_tofacct))
            {
                ob.Code = "0054";
                ob.Message = "确认收款账号不能为空";
                return ob;
            }
            var toUser = serUser.GetAGU(_tofacct);
            if (null == toUser)
            {
                ob.Code = "0055";
                ob.Message = "收款账号不存在";
                return ob;
            }
            var accountRule = new WAPHelpers().GetKV("AGU_REGISTER_ACCOUNT_RULE", true);
            if (false == Regex.IsMatch(_toacct, accountRule.cfg003.Trim()))
            {
                ob.Code = "9999";
                ob.Message = accountRule.cfg005.Trim();
                return ob;
            }
            if (_tofacct != _toacct)
            {
                ob.Code = "0056";
                ob.Message = "收款账号与确认收款账号不一样";
                return ob;
            }
            if (0 >= _amt)
            {
                ob.Code = "0057";
                ob.Message = "转账金额不正确，金额值必须大于0";
                return ob;
            }
            if (false == serUser.CheckUserIDIsMyChild(UILoginUser.u001, toUser.u001) && false == serUser.CheckUserIDIsMyFather(UILoginUser.u001, toUser.u001))
            {
                ob.Code = "0045";
                ob.Message = "请先设置资金密码，设置方法：“账户中心”－“修改密码”－“资金密码”";
                return ob;
            }
            var result = serFinance.TransferMoney(UILoginUser.u001, toUser.u001, _amt,ip, 80);

            if (result.Code == 1)
            {
                ob.Code = "0000";
                ob.Message = "OK";
            }
            else
            {
                ob.Code = "9999";
                ob.Message = result.Message;
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
    public AddMyMoney SetAddMyMoney(int UserID, string SCode, int ForRechargeID, string MPWD, double amoney, string IP)
    {
        try
        {
            AddMyMoney tf = new AddMyMoney();
            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                tf.Code = "0024";
                tf.Message = "用户登陆错误";
                return tf;
            }
            tf.Scode = newScode;

            string _f_charge_type = ForRechargeID.ToString();
            string _f_char_password = MPWD;

            string _f_charge_amount = amoney.ToString();

            string check_pwd = string.Empty;
            decimal check_amount = 0m;
            decimal check_min_amount = decimal.Parse(new WAPHelpers().GetKV("AGU_CHARGE_MIN", false).cfg003);
            int charge_type = 0;
            var chargeStatus = serFinance.CheckDayChargeCount(UserID);

            DBModel.wgs012 UILoginUser = new DBModel.wgs012();
            UILoginUser = serUser.GetAGU(UserID);

            if (0 == chargeStatus.Code)
            {
                tf.Code = "9999";
                tf.Message = chargeStatus.Message;
                tf.Scode = "";
                return tf;
            }

            if (_NWC.GeneralValidate.IsNullOrEmpty(UILoginUser.u010))
            {
                tf.Code = "0045";
                tf.Message = "请先设置资金密码，设置方法：“账户中心”－“修改密码”－“资金密码”";
                tf.Scode = "";
                return tf;
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(_f_char_password))
            {
                tf.Code = "0026";
                tf.Message = "密码不能为空";
                tf.Scode = "";
                return tf;
            }
            check_pwd = _NWC.SHA1.Get(_f_char_password + UILoginUser.u011, _NWC.SHA1Bit.L160);
            if (0 != string.Compare(check_pwd, UILoginUser.u010.Trim(), true))
            {

                tf.Code = "0046";
                tf.Message = "资金密码不正确";
                tf.Scode = "";
                return tf;
            }
            if (_NWC.GeneralValidate.IsNullOrEmpty(_f_charge_type))
            {
                tf.Code = "0047";
                tf.Message = "没有选择充值类型";
                tf.Scode = "";
                return tf;
            }
            if (false == _NWC.GeneralValidate.IsNumber(_f_charge_type))
            {
                tf.Code = "0048";
                tf.Message = "充值类型错误，非法类型";
                tf.Scode = "";
                return tf;
            }
            charge_type = int.Parse(_f_charge_type);
            var allowChargeType = serFinance.GetCTListByCache().Where(exp => exp.ct001 == int.Parse(_f_charge_type) && exp.ct011 != "SYS" && exp.ct012 == 1).FirstOrDefault();
            allowChargeType = serFinance.GetCT(allowChargeType.ct001);
            if (null == allowChargeType)
            {
                tf.Code = "0049";
                tf.Message = "充值类型错误，是不允许类型";
                tf.Scode = "";
                return tf;
            }
            if (true != _NWC.GeneralValidate.IsDecimal(_f_charge_amount) || true != _NWC.GeneralValidate.IsNumber(_f_charge_amount))
            {
                tf.Code = "0050";
                tf.Message = "充值金额错误";
                tf.Scode = "";
                return tf;
            }
            check_amount = decimal.Parse(_f_charge_amount);
            if (check_amount < check_min_amount)
            {
                tf.Code = "0051";
                tf.Message = string.Format("充值金额小于{0}", check_min_amount);
                tf.Scode = "";
                return tf;
            }
            var bankInfo = serFinance.GetBankListByCache().Where(exp => exp.sb001 == allowChargeType.sb001).FirstOrDefault();
            if (4 == int.Parse(_f_charge_type))
            {
                //var oldNYCharge = serFinance.GetExtChargeByAccount(UILoginUser.u002.Trim());
                //if (1 == oldNYCharge.Code)
                //{
                //    ajax.Code = 1;
                //    string[] oldKeys = oldNYCharge.Message.Split('|');
                //    var chargeOld = serFinance.GetCCash(oldKeys[4]);
                //    int lefeTime = int.Parse(oldKeys[5]);
                //    DateTime haveTime = DateTime.Parse(oldKeys[2]).AddMinutes(lefeTime);
                //    var encodeInfoX = _NWC.DEncrypt.Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", chargeOld.u001, chargeOld.u002.Trim(), chargeOld.sb001, chargeOld.ct001, chargeOld.uc005.Trim(), chargeOld.uc001));
                //    var ctInfoX = new { Amount = chargeOld.uc002, OrderID = chargeOld.mu001, BankName = bankInfo.sb004, BankUserName = allowChargeType.ct002, BankLocation = allowChargeType.ct004, BankAccount = allowChargeType.ct003, EncodeInfo = encodeInfoX, ChargeCode = chargeOld.mu001, MaxTime = haveTime.ToString("yyyy/MM/dd HH:mm:ss") };
                //    ajax.Data = ctInfoX;
                //    return Json(ajax);
                //}

                tf.Code = "9999";
                tf.Message = "充值类型错误";
                tf.Scode = "";
                return tf;
            }
            DBModel.wgs019 charge = new DBModel.wgs019();
            charge.mu001 = -1;
            charge.mu002 = "";
            charge.u001 = UILoginUser.u001;
            charge.u002 = UILoginUser.u002.Trim();
            charge.u003 = UILoginUser.u003 != null ? UILoginUser.u003.Trim() : string.Empty;
            charge.uc006 = DateTime.Now;
            charge.ct001 = charge_type;
            charge.sb001 = allowChargeType.sb001;
            charge.uc009 = allowChargeType.ct003;
            charge.uc008 = 0;
            charge.uc010 = IP;
            charge.uc013 = 0;
            charge.uc002 = check_amount;
            #region NY
            string tempNYCode = _NWC.RandomString.Get("1,2,3,4,5,6,7,8,9", 4);
            if (4 == charge_type)
            {
                while (true)
                {
                    var exists = serFinance.GetExtChargeCode(tempNYCode);
                    if (0 == exists.Code)
                    {
                        break;
                    }
                    tempNYCode = _NWC.RandomString.Get("1,2,3,4,5,6,7,8,9", 4);
                }
                charge.mu001 = int.Parse(tempNYCode);
            }
            #endregion
            MR result = new MR();
            result = serFinance.AddCharege(charge);
            //ajax.Code = result.Code;
            if (0 == result.Code)
            {
                tf.Code = "9999";
                tf.Message = result.Message;
            }
            var chargeCode = charge.uc005.Trim();
            #region NY
            if (4 == charge_type)
            {
                charge.uc005 = tempNYCode;
                var setExtData = serFinance.SetExtChargeByAccount(UILoginUser.u001, UILoginUser.u002.Trim(), check_amount, "农业银行", string.Empty, string.Empty, tempNYCode, charge.uc006.Value, chargeCode);
                if (0 == setExtData.Code)
                {
                    tf.Code = "9999";
                    tf.Message = setExtData.Message;
                    return tf;
                }
            }
            #endregion
            var encodeInfo = _NWC.DEncrypt.Encrypt(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", charge.u001, charge.u002.Trim(), charge.sb001, charge.ct001, charge.uc005.Trim(), charge.uc001));
            var ctInfo = new { Amount = charge.uc002, OrderID = charge.uc005.Trim(), BankName = bankInfo.sb004, BankUserName = allowChargeType.ct002, BankLocation = allowChargeType.ct004, BankAccount = allowChargeType.ct003, EncodeInfo = encodeInfo, ChargeCode = charge.uc005.Trim(), ToURL = string.IsNullOrEmpty(allowChargeType.ct011) ? "NONE" : allowChargeType.ct011.Trim() };
            //ajax.Data = ctInfo;
            tf.Bank = bankInfo.sb004;
            tf.AddMoney = charge.uc002;
            tf.BankCode = allowChargeType.ct003;
            tf.ToWho = allowChargeType.ct002;
            tf.BankTel = allowChargeType.ct004;
            tf.CCode = charge.uc005.Trim();

            return tf;
        }
        catch (Exception ex)
        {
            AddMyMoney tf = new AddMyMoney();
            tf.Code = "9999";
            tf.Message = ex.Message.ToString();
            return tf;
        }
    }


    [WebMethod]
    public RechargeTypeList GetRechargeType(int UserID, string SCode)
    {
        try
        {
            RechargeTypeList tl = new RechargeTypeList();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                tl.Code = "0024";
                tl.Message = "用户登陆错误";
                return tl;
            }
            tl.Scode = newScode;

            var ctList = serFinance.GetCTList();
            var bankList = serFinance.GetBankListByCache();
            var CTList = ctList.Where(exp => exp.ct011 != "SYS" && exp.ct012 == 1).OrderBy(exp => exp.ct001).ToList();
            var BankDicList = bankList.ToDictionary(key => key.sb001);

            var dicBankList = (Dictionary<int, DBModel.wgs010>)BankDicList;
            var ctListOrd = from a in ctList from b in dicBankList where a.sb001 == b.Value.sb001 orderby b.Value.sb009 select a;

            RechargeType rt;
            List<RechargeType> rtl = new List<RechargeType>();

            foreach (var item in ctListOrd)
            {
                rt = new RechargeType();
                rt.ForRechargeID = item.ct001;
                rt.ForRecharge = dicBankList[item.sb001].sb003;

                rtl.Add(rt);
            }

            tl.RList = rtl;

            return tl;
        }
        catch (Exception ex)
        {
            RechargeTypeList tl = new RechargeTypeList();
            tl.Code = "9999";
            tl.Message = ex.Message.ToString();
            return tl;
        }
    }


    [WebMethod]
    public AddMoneyList AddMoneyList(int UserID, string SCode, int PIndex)
    {
        try
        {
            AddMoneyList am = new AddMoneyList();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                am.Code = "0024";
                am.Message = "用户登陆错误";
                return am;
            }
            am.Scode = newScode;


            var bankList = serFinance.GetBankListByCache();
            //List<DBModel.wgs009> nCTList = (List<DBModel.wgs009>)(from ctl in ctList select new DBModel.wgs009(){ ct001 = ctl.ct001, ct003= ctl.ct003 }).ToList();
            List<DBModel.wgs010> nBankList = (List<DBModel.wgs010>)(from bl in bankList select new DBModel.wgs010() { sb001 = bl.sb001, sb003 = bl.sb003 }).ToList();
            //ViewData["CTList"] = nCTList.ToDictionary(k=>k.ct001);
            var BList = nBankList.ToDictionary(k => k.sb001);

            int recordCount = 0;
            int pagesize = new WAPHelpers().PageSize;
            var crhlList = serFinance.GetChargeList(0, UserID, string.Empty, 0, string.Empty, 0, 0, 0, 0, 0, 0, -1, (DateTime?)null, (DateTime?)null, pagesize, (int)PIndex, out recordCount);
            //string pageURL = Url.Action("Bank", "UI2", new { method = MethodType });
            //ViewData["PageList"] = _NWC.PageList.GetPageListUI2(PageSize, recordCount, (int)pageIndex, PageBloclSize, "wp_page fl_r", "pageIndex", pageURL);
            
            var bList = (Dictionary<int, DBModel.wgs010>)BList;

            AddMoneyType at;
            List<AddMoneyType> atl = new List<AddMoneyType>();
            foreach (var item in crhlList)
            {
                at = new AddMoneyType();

                at.ACode = item.uc005;
                at.AType = bList[item.sb001].sb003;
                at.AMoney = item.uc002;
                at.AToMoney = item.uc003;
                at.ATime = (DateTime)item.uc006;
                if (item.uc008 == 0) at.AState = "处理中";
                if (item.uc008 == 1) at.AState = "已到账";
                if (item.uc008 == 2) at.AState = "已取消";
                else at.AState = "处理中";
                at.AAbout = item.uc012;

                atl.Add(at);
            }


            am.AddMoneyLists = atl;

            am.PageSize = pagesize;
            am.PageMaxCount = recordCount;

            am.Code = "0000";
            am.Message = "OK";

            return am;
        }
        catch (Exception ex)
        {
            AddMoneyList am = new AddMoneyList();
            am.Code = "9999";
            am.Message = ex.Message.ToString();
            return am;
        }
    
    }



    [WebMethod]
    public CashList CashList(int UserID, string SCode, int PIndex)
    {
        try
        {
            CashList cl = new CashList();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                cl.Code = "0024";
                cl.Message = "用户登陆错误";
                return cl;
            }
            cl.Scode = newScode;

            int recordCount = 0;
            int PageSize = new WAPHelpers().PageSize;
            var wcDataList = serFinance.GetWCDataList(-1, UserID, string.Empty, 0, 0, 0, 0, 0, -1, null, null, PageSize, (int)PIndex, out recordCount);


            CashType ct;
            List<CashType> ctl = new List<CashType>();
            foreach (var item in wcDataList)
            {
                ct = new CashType();

                ct.CCode = item.uw001.ToString();
                ct.CType = item.uw009;
                ct.CMoney = item.uw002;
                ct.CTime = (DateTime)item.uw005;

                if (item.uw006 == 0) ct.CState = "处理中";
                if (item.uw006 == 1) ct.CState = "已到账";
                if (item.uw006 == 2) ct.CState = "已取消";
                else ct.CState = "处理中";

                ct.CAbout = item.uw008;

                ctl.Add(ct);
            }

            cl.CashLists = ctl;

            cl.PageMaxCount = recordCount;
            cl.PageSize = PageSize;

            cl.Code = "0000";
            cl.Message = "OK";

            return cl;
        }
        catch (Exception ex)
        {
            CashList cl = new CashList();
            cl.Code = "9999";
            cl.Message = ex.Message.ToString();
            return cl;
        }
    }


    [WebMethod]
    public TFList TransferList(int UserID, string SCode, int PIndex, int ttype, DateTime STime, DateTime ETime)
    {


        try
        {
            TFList tf = new TFList();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                tf.Code = "0024";
                tf.Message = "用户登陆错误";
                return tf;
            }
            tf.Scode = newScode;


            var _type = ttype;
            var _dts = STime;
            var _dte = ETime;

            int recordCount = 0;
            int pagesize = new WAPHelpers().PageSize;
            var list = serFinance.GetTransferList(0, UserID, string.Empty, 0, string.Empty, _type, 0, 0, _dts, _dte, (int)PIndex, pagesize, out recordCount);

            TFType tt;
            List<TFType> ttl = new List<TFType>();
            Dictionary<int, string> icStatus = new Dictionary<int, string>() { { 0, "未审核" }, { 1, "已审核" }, { 2, "已取消" } };

            foreach (var item in list)
            {
                tt = new TFType();
                tt.TCode = item.tf001.ToString();
                tt.TToName = item.tf003;
                tt.TForName = item.tf006;
                tt.TMoney = item.tf008;
                tt.TTime = item.tf009;
                tt.TState = icStatus[item.tf012];
                tt.TAbout = "暂无";
                ttl.Add(tt);
            }

            tf.CashLists = ttl;
            tf.PageMaxCount = recordCount;
            tf.PageSize = pagesize;

            tf.Code = "9999";
            tf.Message = "OK";

            return tf;



        }
        catch(Exception ex)
        {
            TFList tf = new TFList();

            tf.Code = "9999";
            tf.Message = ex.Message.ToString();
            return tf;
        }
    
    }

    [WebMethod]
    public CombineList GetCombineList(int UserID, string SCode, int Pindex, DateTime GSTime, DateTime GETime, int GameID, string CUserName, string CNumber, int CSate, string GCNumber)
    {
        try
        {
            CombineList cl = new CombineList();

            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                cl.Code = "0024";
                cl.Message = "用户登陆错误";
                return cl;
            }
            cl.Scode = newScode;

            var _orderDts = GSTime;
            var _orderDte = GETime;
            var _orderGame = GameID.ToString();
            var _issue = string.IsNullOrEmpty(CNumber) || string.IsNullOrWhiteSpace(CNumber) ? string.Empty : CNumber;
            var _account = string.IsNullOrEmpty(CUserName) || string.IsNullOrWhiteSpace(CUserName) ? string.Empty : CUserName;
            var _status = CSate;
            var orderDts = DateTime.Parse(_orderDts.ToString("yyyy-mm-dd") + " 00:00:00");
            var orderDte = DateTime.Parse(_orderDte.ToString("yyyy-mm-dd") + " 23:59:59");
            var orderGame = int.Parse(_orderGame);
            var issue = _issue;
            var account = _account;
            var status = _status;

            int recordCount = 0;
            int PageSize = new WAPHelpers().PageSize;
            
            var combuyList = serGame.GetCombuyList(0, account, orderGame, 0, issue, status, (int)Pindex, PageSize, orderDts, orderDte, out recordCount);


            var gDicList = serGame.GetGameListByCache().ToDictionary(key => key.g001);

            Dictionary<int, string> dicStatus = new Dictionary<int, string>() { { 0, "进行中" }, { 1, "已满人" }, { 2, "已撤单" }, {3,"已结算"} };

            CombineType ct;
            List<CombineType> ctl = new List<CombineType>();
            
            foreach (var item in combuyList)
            {
                ct = new CombineType();
                var haveTime = DateTime.Now - item.gs004.Value;
                var timeAllow = (haveTime.Seconds < 0) ? true : false;

                ct.GID = item.sco001;
                ct.GCNumber = item.sco001;
                ct.UserName = item.u002.Trim();
                ct.GameName = gDicList[item.g001].g003;
                ct.GLssue = item.gs002.Trim();
                ct.GEnd = item.gs007.Trim();
                ct.GTxt = item.sco013;
                ct.GMoneySum = item.sco007;
                ct.GMoney = (item.sco007 / 100).ToString("N2");
                ct.GProForMe = item.sco004;
                ct.GProFoAll = item.sco011;
                ct.GUserCount = item.sco012;
                ct.GTime = item.sco017;
                ct.NGTime = (DateTime)item.gs004;
                ct.GState = dicStatus[item.sco009];
                ct.TimeAllow = timeAllow;

                ctl.Add(ct);
                   

            }

            cl.CombineLists = ctl;
            cl.Code = "0000";
            cl.Message = "ok";

            return cl;
        }
        catch (Exception ex)
        {
            CombineList cl = new CombineList();
            cl.Code = "9999";
            cl.Message = ex.Message.ToString();
            return cl;
        }
    }

    [WebMethod]
    public ObjMsg SetCombine(int UserID, string SCode, int GID, int GPro)
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

            var _hashKey = GID.ToString();
            var _password = "";
            var _times = "";
            var hashKey = string.IsNullOrWhiteSpace(_hashKey) || string.IsNullOrEmpty(_hashKey) ? string.Empty : _hashKey;
            var password = string.IsNullOrWhiteSpace(_password) || string.IsNullOrEmpty(_password) ? string.Empty : _password;
            var times = _NWC.GeneralValidate.IsNumber(_times) ? int.Parse(_times) : 0;
            if (string.IsNullOrEmpty(hashKey))
            {
                ob.Code = "0058";
                ob.Message = "信息不存在";
                return ob;
            }
            var hashKeyOri = _NWC.DEncrypt.Decrypt(hashKey);
            var hashKeyOriSplit = hashKeyOri.Split('|');
            if (3 != hashKeyOriSplit.Length)
            {
                ob.Code = "0059";
                ob.Message = "信息不正确";
                return ob;
            }
            var result = serGame.JoinCombuy(long.Parse(hashKeyOriSplit[0]), UserID, times, password);
            ob.Code = result.Code.ToString();
            ob.Message = result.Message;



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

}
