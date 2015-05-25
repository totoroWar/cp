namespace GameCompute
{
    partial class RandomResult
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gameList = new System.Windows.Forms.ListBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.tPlan = new System.Windows.Forms.Timer(this.components);
            this.txtSec = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // gameList
            // 
            this.gameList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gameList.FormattingEnabled = true;
            this.gameList.Location = new System.Drawing.Point(13, 13);
            this.gameList.Name = "gameList";
            this.gameList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.gameList.Size = new System.Drawing.Size(205, 340);
            this.gameList.TabIndex = 0;
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(224, 13);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(529, 340);
            this.txtResult.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(27, 373);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // txtSec
            // 
            this.txtSec.Location = new System.Drawing.Point(108, 376);
            this.txtSec.Name = "txtSec";
            this.txtSec.Size = new System.Drawing.Size(41, 20);
            this.txtSec.TabIndex = 3;
            this.txtSec.Text = "2";
            // 
            // RandomResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 408);
            this.Controls.Add(this.txtSec);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.gameList);
            this.Name = "RandomResult";
            this.Text = "随机开奖";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox gameList;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer tPlan;
        private System.Windows.Forms.TextBox txtSec;
    }
}