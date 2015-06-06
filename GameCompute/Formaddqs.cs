using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml; 

namespace GameCompute
{
    public partial class Formaddqs : Form
    {
        Thread thread = null; 
        string day = "";
        GameServices.IGame game = new GameServices.Game();
        public Formaddqs()
        {
            InitializeComponent();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            day = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            Button curButton = (sender as Button);
            var text = curButton.Text;
            if ("开始" == text)
            {
                curButton.Text = "停止";
                timePlan.Enabled = true;
                timePlan.Interval = int.Parse(textBox1.Text) * 1000 * 60 ;
                timePlan.Tick += timePlan_Tick;
                WriteInfo("开始添加");
            }
            else if ("停止" == text)
            {
                curButton.Text = "开始";
                timePlan.Stop();
                WriteInfo("停止添加");
            }
        }

        public void WriteInfo(string message)
        {
            rtbInfo.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
            rtbInfo.Text += message + Environment.NewLine + Environment.NewLine;
        }

        void timePlan_Tick(object sender, EventArgs e)
        {
            if (thread == null)
            {
                thread = new Thread(ThreadProc);
                thread.IsBackground = true;
                thread.Start(this);
            }
            if (thread != null && thread.IsAlive == false)
            {
                thread = new Thread(ThreadProc);
                thread.IsBackground = true;
                thread.Start(this);
            }
        }

        public void ThreadProc(object sender)
        {
            try
            {
                ((Formaddqs)sender).DoIt();
            }
            catch (Exception error)
            {
                //((FormCalc)sender).WriteInfo("线程出错，"+error.Message);
            }
        }

        public void DoIt()
        {
            try
            {
                int addtime = Convert.ToInt16(textBox2.Text); 
                DateTime dtNow = DateTime.Now;


                if (dtNow.ToString("yyyy-MM-dd").Equals(day))
                {
                    if (addtime == dtNow.Hour)
                    {

                        day = Convert.ToDateTime(day).AddDays(1).ToString("yyyy-MM-dd");//日期改为明天
                        List<int> Gamesid = new List<int>();
                        Gamesid = game.GetGameListByCache().Select(exp => exp.g001).ToList();
                        if (0 == Gamesid.Count)
                        {
                            return;
                        }
                        XmlDocument xmlDoc = new XmlDocument();
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.IgnoreComments = true;//忽略文档里面的注释
                        string rootpath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("\\"));
                        XmlReader reader = XmlReader.Create(@rootpath + "\\Gamession.xml", settings);
                        xmlDoc.Load(reader); 

                        List<Gamessionlist> Gamessionlists = new List<Gamessionlist>();
                        XmlNode xn = xmlDoc.SelectSingleNode("Games"); 
                        XmlNodeList xnl = xn.ChildNodes; 
                        foreach (XmlNode xn1 in xnl)
                        { 
                            XmlElement xe = (XmlElement)xn1; 
                             // 得到节点的所有子节点
                            XmlNodeList xnl0 = xe.ChildNodes;
                            Gamessionlist list = new Gamessionlist();
                            list.Set_gameID = xnl0.Item(0).InnerText;
                            list.Start_no = xnl0.Item(1).InnerText;//开始期号
                            list.End_no = xnl0.Item(2).InnerText;//结束期号
                            list.Start_time = day + " " + xnl0.Item(3).InnerText;//开盘时间
                            list.Start_time2 = xnl0.Item(4).InnerText;
                            list.Close_time = day + " " + xnl0.Item(5).InnerText;//封盘时间
                            list.Close_time2 = xnl0.Item(6).InnerText;
                            list.Open_time = day + " " + xnl0.Item(7).InnerText;//开奖时间
                            list.Open_time2 = xnl0.Item(8).InnerText; 
                            list.Start_type = xnl0.Item(9).InnerText;
                            list.Start_date = day;//期数日期
                            Gamessionlists.Add(list);
                        }
                        reader.Close();
                        Gamessionlists = Gamessionlists.Where(exp => Gamesid.Contains(Convert.ToInt16(exp.Set_gameID))).ToList();  

                        for (int i = 0; i < Gamessionlists.Count; i++)
                        {
                            var processMessage = game.AddGameSession(int.Parse(Gamessionlists[i].Start_type), int.Parse(Gamessionlists[i].Set_gameID), Convert.ToDateTime(Gamessionlists[i].Start_date), long.Parse(Gamessionlists[i].Ser_no), long.Parse(Gamessionlists[i].Start_no), int.Parse(Gamessionlists[i].End_no), Convert.ToDateTime(Gamessionlists[i].Start_time), Convert.ToDateTime(Gamessionlists[i].Close_time), Convert.ToDateTime(Gamessionlists[i].Open_time), int.Parse(Gamessionlists[i].Start_time2), int.Parse(Gamessionlists[i].Close_time2), int.Parse(Gamessionlists[i].Open_time2));
                        }

                    }
                } 
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        class Gamessionlist
        {
            private string set_gameID;
            public string Set_gameID
            {
                get { return set_gameID; }
                set { set_gameID = value; }
            }

            private string ser_no = "1";
            public string Ser_no
            {
                get { return ser_no; }
                set { ser_no = value; }
            }

            private string start_type = "0";
            public string Start_type
            {
                get { return start_type; }
                set { start_type = value; }
            }

            private string start_date = DateTime.Now.ToString("yyyy-MM-dd");//期数日期
            public string Start_date
            {
                get { return start_date; }
                set { start_date = value; }
            }

            private string start_no;//开始期号
            public string Start_no
            {
                get { return start_no; }
                set { start_no = value; }
            }

            private string end_no;//结束期号
            public string End_no
            {
                get { return end_no; }
                set { end_no = value; }
            }

            private string start_time;//开盘时间
            public string Start_time
            {
                get { return start_time; }
                set { start_time = value; }
            }

            private string start_time2;
            public string Start_time2
            {
                get { return start_time2; }
                set { start_time2 = value; }
            }

            private string close_time;//封盘时间
            public string Close_time
            {
                get { return close_time; }
                set { close_time = value; }
            }

            private string close_time2;
            public string Close_time2
            {
                get { return close_time2; }
                set { close_time2 = value; }
            }
           
            private string open_time;//开奖时间
            public string Open_time
            {
                get { return open_time; }
                set { open_time = value; }
            }

            private string open_time2;
            public string Open_time2
            {
                get { return open_time2; }
                set { open_time2 = value; }
            }


        }
    }
}
