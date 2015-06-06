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
namespace GameCompute
{
    public partial class Formcj : Form
    {
        Thread thread = null;
        GameServices.IGame game = new GameServices.Game();
        public Formcj()
        {
            InitializeComponent();
            Shown += FormCalc_Shown;
        }


        void FormCalc_Shown(object sender, EventArgs e)
        { 
            for (int i = 1; i <= 600; i++)
            {
                cbbSec.Items.Add(i.ToString());
            }
            cbbSec.SelectedItem = "5";
        } 

        private void btnStart_Click(object sender, EventArgs e)
        {
            Button curButton = (sender as Button);
            var text = curButton.Text;
            if ("开始" == text)
            {
                curButton.Text = "停止";
                timePlan.Enabled = true;
                timePlan.Interval = int.Parse(cbbSec.Text) * 1000;
                timePlan.Tick += timePlan_Tick;
                WriteInfo("开始采集");
            }
            else if ("停止" == text)
            {
                curButton.Text = "开始";
                timePlan.Stop();
                WriteInfo("停止采集");
            }
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

        public void WriteInfo(string message)
        {
            rtbInfo.Text += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
            rtbInfo.Text += message + Environment.NewLine + Environment.NewLine;
        }


        public void ThreadProc(object sender)
        {
            try
            {
                ((Formcj)sender).DoIt();
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

                int sDate = Convert.ToInt16(textBox1.Text);
                int eDate = Convert.ToInt16(textBox2.Text); 
                DateTime dtNow = DateTime.Now;

                if (sDate > dtNow.Hour || dtNow.Hour > eDate)
                {
                    var processMessage = game.Gamecj();
                    if (0 == processMessage.Code)
                    {
                        FileStream fs = new FileStream(System.Environment.CurrentDirectory + "\\Log\\cj" + DateTime.Now.ToString("yyyyMMddHH") + ".log", FileMode.Append);
                        var content = Encoding.Default.GetBytes(processMessage.Message + Environment.NewLine);
                        fs.Write(content, 0, content.Length);
                        fs.Flush();
                        fs.Close();
                    }
                    else if (1 == processMessage.Code)
                    {
                        // WriteInfo(string.Format("结算{0}条订单" + Environment.NewLine + processMessage.Message, count));
                    }
                }                       
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }
    }
}
