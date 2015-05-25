using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCompute
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Shown += FormMain_Shown;
        }

        void FormMain_Shown(object sender, EventArgs e)
        {
            btnCalc.Click += btnCalc_Click;
            btnRandomResult.Click += btnRandomResult_Click;
        }

        void btnRandomResult_Click(object sender, EventArgs e)
        {
            RandomResult r = new RandomResult();
            r.Show();
        }

        void btnCalc_Click(object sender, EventArgs e)
        {
            GameServices.IGame game = new GameServices.Game();
            var gcList = game.GetGameClassList();
            foreach (var item in gcList)
            {
                OtherObject go = new OtherObject();
                go.GameClassID = item.gc001;
                go.Title = item.gc003.Trim();
                go.GameClassName = item.gc003.Trim();
                Form form = new FormAllCalc(go);
                form.Show();
            }
            //FormSSCCalc form = new FormSSCCalc();
            //form.Show();
            //Form3DCalc form1 = new Form3DCalc();
            //form1.Show();
            //Form11X5 form11x5 = new Form11X5();
            //form11x5.Show();
        }

        

    }
}
