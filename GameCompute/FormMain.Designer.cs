namespace GameCompute
{
    partial class FormMain
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
            this.btnCalc = new System.Windows.Forms.Button();
            this.btnRandomResult = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.addqs = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(49, 40);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(75, 21);
            this.btnCalc.TabIndex = 0;
            this.btnCalc.Text = "结算";
            this.btnCalc.UseVisualStyleBackColor = true;
            // 
            // btnRandomResult
            // 
            this.btnRandomResult.Location = new System.Drawing.Point(130, 40);
            this.btnRandomResult.Name = "btnRandomResult";
            this.btnRandomResult.Size = new System.Drawing.Size(75, 21);
            this.btnRandomResult.TabIndex = 1;
            this.btnRandomResult.Text = "随机结果";
            this.btnRandomResult.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(222, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "采奖";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // addqs
            // 
            this.addqs.Location = new System.Drawing.Point(326, 40);
            this.addqs.Name = "addqs";
            this.addqs.Size = new System.Drawing.Size(89, 23);
            this.addqs.TabIndex = 3;
            this.addqs.Text = "自动添加期数";
            this.addqs.UseVisualStyleBackColor = true;
            this.addqs.Click += new System.EventHandler(this.addqs_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 109);
            this.Controls.Add(this.addqs);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnRandomResult);
            this.Controls.Add(this.btnCalc);
            this.Name = "FormMain";
            this.Text = "平台辅助";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Button btnRandomResult;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button addqs;
    }
}

