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
            this.SuspendLayout();
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(49, 43);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(75, 23);
            this.btnCalc.TabIndex = 0;
            this.btnCalc.Text = "结算";
            this.btnCalc.UseVisualStyleBackColor = true;
            // 
            // btnRandomResult
            // 
            this.btnRandomResult.Location = new System.Drawing.Point(130, 43);
            this.btnRandomResult.Name = "btnRandomResult";
            this.btnRandomResult.Size = new System.Drawing.Size(75, 23);
            this.btnRandomResult.TabIndex = 1;
            this.btnRandomResult.Text = "随机结果";
            this.btnRandomResult.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 118);
            this.Controls.Add(this.btnRandomResult);
            this.Controls.Add(this.btnCalc);
            this.Name = "FormMain";
            this.Text = "平台辅助";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Button btnRandomResult;
    }
}

