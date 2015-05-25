using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using _NWC = NETCommon;
using GameServices;
using System.Text.RegularExpressions;


/// <summary>
/// UserMall 的摘要说明
/// </summary>
[WebService(Namespace = "WAPService")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

[System.Web.Script.Services.ScriptService]
public class UserMall : System.Web.Services.WebService {
    GameServices.Finance serFinance = new GameServices.Finance();
    GameServices.IUser serUser = new GameServices.User();
    GameServices.Game serGame = new Game();
    GameServices.System serSystem = new GameServices.System();
    

    public UserMall () {}

    [WebMethod]
    public MallList GetMallList(int UserID, string SCode, int ClasID, int PageIndex)
    {
        try
        {
            MallList ml = new MallList();
            //校验验证码和UID，并生成新的验证码
            string newScode = "";
            newScode = new WAPHelpers().CheckSCode(SCode, UserID);
            if (newScode == "-1")
            {
                ml.Code = "0024";
                ml.Message = "用户登陆错误";
                return ml;
            }
            ml.Scode = newScode;


            DBModel.wgs012 UILoginUser = new DBModel.wgs012();
            UILoginUser = serUser.GetAGU(UserID);

            var CurrentUserVip = UILoginUser.u015;
            var vipDiscount = serSystem.GetKeyValue("SYS_VIP_DISCOUNT").cfg003;
            var shopClassList = serSystem.GetShopClassAllList();
            var shopProductList = serSystem.GetShowShopProductList(ClasID);

            MallListType mt;
            List<MallListType> mtl = new List<MallListType>();
            foreach (var item in shopProductList)
            {
                mt = new MallListType();
                mt.BID = item.r001;
                mt.BName = item.r002;
                mt.IMGUrl = "/images/shop/" + item.r006;
                mt.IsVIP = item.r011;
                mt.BMoney = item.r003;
                mt.BPopularity = item.r004;
                mt.BStock = item.r005;
                mtl.Add(mt);
            }

            ml.mlst = mtl;

            ml.Code = "0000";
            ml.Message = "OK";

            return ml;
        }
        catch (Exception ex)
        {
            MallList ml = new MallList();
            ml.Code = "9999";
            ml.Message = ex.Message.ToString();
            return ml;
        }
    }

    [WebMethod]
    public ObjMsg PayMallCommodity(int UserID, string SCode, int ProductId, int num, string address, string phoneNumber, string zip, string name)
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

            DBModel.wgs012 UILoginUser = new DBModel.wgs012();
            UILoginUser = serUser.GetAGU(UserID);


            string[] vipDiscountInfo = serSystem.GetKeyValue("SYS_VIP_DISCOUNT").cfg003.Split(',');
            decimal userDiscount = 0;
            foreach (var item in vipDiscountInfo)
            {
                if (item.IndexOf(UILoginUser.u015 + ":", 0) != -1)
                {
                    userDiscount = decimal.Parse(item.Split(':')[1]);
                    break;
                }
            }
            MR mr = new MR();
            mr = serSystem.BuyProduct(ProductId, num, UILoginUser.u001, address, phoneNumber, zip, name, userDiscount);

            ob.Code = "9999";
            ob.Message = mr.Message;

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
    public MallOrdeList GetMallOrder(int UserID, string SCode, int ClasID, int PageIndex)
    {
        try
        {
            MallOrdeList ob = new MallOrdeList();
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

            DBModel.wgs012 UILoginUser = new DBModel.wgs012();
            UILoginUser = serUser.GetAGU(UserID);


            
            int recordCount = 0;
            int pageSize = new WAPHelpers().PageSize;
            int pageIndex = 1;

            var shopRecordList = serSystem.GetShopRecordList(UILoginUser.u001, null, pageIndex, pageSize, out recordCount);

            MallOrdeType ot;
            List<MallOrdeType> otl = new List<MallOrdeType>();

            foreach (var item in shopRecordList)
            { 
                ot = new MallOrdeType();

                ot.BCode = item.sr001;
                ot.BResID = item.r001;
                ot.BResTitle = item.sr009;
                ot.BResMoney = item.sr008;
                ot.BCount = item.sr002;
                ot.BSumMoney = item.sr003;
                ot.BName = item.sr011;
                ot.BPHone = item.sr012;
                ot.BTel = item.sr010;
                ot.BZip = item.sr013;
                ot.BOldMoney = item.sr014;
                ot.BLogistics = item.sr015;
                ot.BURL = item.sr016;
                ot.BPostCode = item.sr017;
                ot.BSale = item.sr018;
                ot.BState = item.sr004 == 0 ? "未处理" : item.sr004 == 1 ? "已完成" : "已撤销";
                ot.BAbout = item.sr005;
                otl.Add(ot);

            }

            ob.OrdeList = otl;

            ob.Code = "0000";
            ob.Message = "OK";

            return ob;
        }
        catch (Exception ex)
        {
            MallOrdeList ob = new MallOrdeList();
            ob.Code = "9999";
            ob.Message = ex.Message.ToString();
            return ob;
        }
    }




}
