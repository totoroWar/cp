using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCompute
{
    public partial class RandomResult : Form
    {
        Dictionary<int, DBModel.SysGameResultInfo> settingList;
        GameServices.IGame game = new GameServices.Game();
        GameServices.ISystem sys = new GameServices.System();
        List<int> startIDs = new List<int>();
        Dictionary<int, List<DBModel.wgs038>> issueList = new Dictionary<int, List<DBModel.wgs038>>();
        public RandomResult()
        {
            InitializeComponent();
            Shown += RandomResult_Shown;
            settingList = game.GetSysGameResultInfo(false);
        }

        void RandomResult_Shown(object sender, EventArgs e)
        {
            btnStart.Click += btnStart_Click;
            txtResult.TextChanged += txtResult_TextChanged;
            foreach (var item in settingList)
            {
                gameList.Items.Add(item.Key + "---|---" + item.Value.GameName);
            }
        }

        bool CheckSameNumber(string result, string number)
        {
            var temp = result.Split(',');
            var count = temp.Count(exp => exp == number);
            return count > 0 ? true : false;
        }

        string GetRandomResult(int len,int max,int min, bool same)
        {
             string result = string.Empty;
            for (int i = 0; i < len; i++)
            {
                var rand = new Random(Guid.NewGuid().GetHashCode());
                Thread.Sleep(1);
                var curNumber = rand.Next(min, max + 1);
                if (true == same)
                {
                    bool haveSame = CheckSameNumber(result, curNumber.ToString());
                    while (haveSame)
                    {
                        curNumber = rand.Next(min, max + 1);
                        haveSame = CheckSameNumber(result, curNumber.ToString());
                    }
                }
                result += curNumber + ",";
            }
           return result.Substring(0, result.Length-1);
        }
        
        bool IsClientWin(string result, List<DBModel.wgs045> order, ref decimal money)
        {
            var moneyX = game.CheckWinLose(result, order);
            bool flag= moneyX-money > 0 ? true : false;
            money = moneyX;
            return flag;
        }

        void SetGameResult()
        {
            //on|50|20|in|username1,username2
            string configstrinng = sys.GetKeyValue("CONTROL_WIN_LOSE").cfg003;
            string[] filterString = configstrinng.Split('|');
            bool kill = filterString[0].ToLower()=="on";
            List<int> issueIdList = new List<int>();
            decimal gtAmout = decimal.Parse(filterString[5]);
            List<DBModel.wgs045> order = new List<DBModel.wgs045>();
            if (kill)
            {
                 var rand = new Random(Guid.NewGuid().GetHashCode());
                 kill = int.Parse(filterString[1].Trim()) >= rand.Next(1, 100);
                 if (kill)
                 {
                     foreach (var item1 in issueList)
                     {
                         foreach (var item in item1.Value)
                         {
                             issueIdList.Add(item.gs001);
                         }
                     }
                     order = game.GetCheckOrderList(issueIdList, filterString[4].Split(',').ToList(),filterString[3].ToLower()=="in", gtAmout);
                 }
               
                 
             }
            foreach (var item in issueList)
            {
                var list = item.Value;
                string result = string.Empty;
                foreach (var gs in list)
                {
                    if (null == gs.gs007 || string.IsNullOrEmpty(gs.gs007))
                    {
                        var len = settingList[gs.g001].Length;
                        var max = settingList[gs.g001].RangeMax;
                        var min = settingList[gs.g001].RangeMin;
                        var same = settingList[gs.g001].Same;
                        result = GetRandomResult(len, max, min, same);
                        int count = int.Parse(filterString[2]);
                        int tickCount = 0;
                        DateTime dt = DateTime.Now;
                        decimal winMoney = 0.0000m;
                        if (kill)
                        {
                            while (IsClientWin(result, order.Where(p => p.gs001 == gs.gs001).ToList(), ref winMoney))
                            {
                                result = GetRandomResult(len, max, min, same);
                                tickCount++;
                                if (tickCount >= count)
                                {
                                    break;
                                }
                            }
                        }
                        game.SetGameResult(gs.gs001, result, false, DateTime.Now, DateTime.Now);
                        this.txtResult.Text = string.Format("{0}__{1}__{2}__{3}__KILL={4}__Count={5}__Time={6}ms__Same={9}__Money={7}{8}", 
                            gs.g001,
                            gs.gs001,
                            gs.gs002.Trim(),
                            result, 
                            kill,
                            tickCount,
                            (DateTime.Now - dt).Milliseconds,
                            winMoney,
                            "\r\n" + configstrinng + "\r\n" + this.txtResult.Text,
                            same
                            );
                       
                    }
                }
            }
        }


        void GetGSList()
        {
            if (0 < gameList.SelectedItems.Count)
            {
                issueList.Clear();
                startIDs.Clear();
                foreach (var item in gameList.SelectedItems)
                {
                    var itemSplit = item.ToString().Split(new string[] { "---|---" }, StringSplitOptions.None);
                    startIDs.Add(int.Parse(itemSplit[0]));
                }
                if (0 < startIDs.Count)
                {
                    foreach (var id in startIDs)
                    {
                        try
                        {
                            issueList.Remove(id);
                        }
                        catch { }
                        var list = game.GetNeedResultList(id, true, DateTime.Now, 1);
                        issueList.Add(id, list);
                    }
                }
            }
            else
            {
                this.txtResult.Text = "请选择一项游戏！";
            }
        }

        void tPlan_Tick(object sender, EventArgs e)
        {
            try
            {
                GetGSList();
                SetGameResult();
            }
            catch (Exception ex)
            {

                this.txtResult.Text = ex.Message+"\r\n"+ txtResult.Text;
            }
           
        }

        void btnStart_Click(object sender, EventArgs e)
        {
            var btn = (sender as Button);
            if ("启动" == btn.Text)
            {
                btn.Text = "停止";
                tPlan.Tick += tPlan_Tick;
                tPlan.Enabled = true;
                tPlan.Interval = int.Parse(txtSec.Text) * 1000;
                tPlan.Start();
            }
            else
            {
                btn.Text = "启动";
                tPlan.Enabled = false;
                tPlan.Stop();
            }
        }
        void txtResult_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text.Length>2000)
            {
               txt.Text=txt.Text.Substring(0,2000); 
            }
        }
    }
}
