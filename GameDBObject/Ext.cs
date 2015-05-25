using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
namespace GameDBObject
{
    public class ExtRDClass
    {
        public byte ID { get; set; }
        public string Name { get; set; }
        public int ExtID { get; set; }
        internal static object GetUserLevel(string vipLevel, decimal uf004)
        {
            throw new NotImplementedException();
        }
    }
    public class ExtDCFunction
    {
        public readonly static string SplitLine = "--------------------------------------------------------------------";
        public readonly static string DBClassList = "255:系统充值:7,1:系统赠送:11,2:系统活动:8,3:系统理赔:9,4:账户充值,5:账户提现,6:提现取消,7:系统分红,8:系统返点,9:购彩扣款,10:彩票中奖,11:本人撤单,12:系统撒单,13:开错奖撤单,14:提现申请,15:系统删单,16:系统扣款,17:购彩积分,18:充值积分,19:登录积分,20:积分消费,21:积分退回,22:回收金额,23:充值取消,24:追号扣款,25:合买扣款,26:合买收款,27:合买撤单,28:合买中奖,29:合买返点,30:合买抽点,31:转账-出,32:转账-入,33:签到积分,34:奖励积分,35:转账申请,36:转账取消";
        public readonly static Dictionary<byte, ExtRDClass> result = new Dictionary<byte, ExtRDClass>();
        public static Dictionary<byte, ExtRDClass> GetRDList()
        {
            var split = ExtDCFunction.DBClassList.Split(',');
            foreach (var item in split)
            {
                ExtRDClass exists = new ExtRDClass();
                var itemSplit = item.Split(':');
                byte id = byte.Parse(itemSplit[0]);
                string name = itemSplit[1];
                int extID = 0;
                if (result.TryGetValue(id, out exists))
                {
                    continue;
                }
                if (3 <= itemSplit.Length)
                {
                    extID = int.Parse(itemSplit[2]);
                }
                result.Add(id, new ExtRDClass() { ID = id, ExtID = extID, Name = name });
            }
            return result;
        }
        public static int GetUserLevel(string level, decimal uf004)
        {
            var levelSplit = level.Split(',');
            var levelResult = 0;
            foreach (var item in levelSplit)
            {
                var itemSplit = item.Split(':');
                var levelSum = decimal.Parse(itemSplit[0]);
                if (uf004 >= levelSum)
                {
                    levelResult = int.Parse(itemSplit[1]);
                }
            }
            return levelResult;
        }
        public static string GetUserLevelName(string level, int levelNumber)
        {
            return string.Empty;
        }
        public static void SendMessage(ref SqlCommand command, int userID, string userName, string userNickName, string title, string content, DateTime dt)
        {
            command.Parameters.Clear();
            command.CommandText = "INSERT INTO wgs044([msg002],[msg003],[msg004],[msg005],[msg006],[msg008],[msg009],[msg010],[msg011]) VALUES(@Title, @Content, 0, @toUserID, @SendDateTime,'_', @UserName, '_', @UserNickName);";
            command.Parameters.AddWithValue("@Title", title);
            command.Parameters.AddWithValue("@Content", content);
            command.Parameters.AddWithValue("@toUserID", userID);
            command.Parameters.AddWithValue("@SendDateTime", dt);
            command.Parameters.AddWithValue("@UserName", userName);
            command.Parameters.AddWithValue("@UserNickName", userNickName);
            command.ExecuteNonQuery();
        }
        public static Dictionary<int, string> GetGameNameList()
        {
            return new Dictionary<int, string>();
        }
    }
}
