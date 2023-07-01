namespace MJLPRateCalc {
    partial class Setting {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.BtSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TbPlayerName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtSave
            // 
            this.BtSave.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BtSave.Location = new System.Drawing.Point(248, 159);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(179, 78);
            this.BtSave.TabIndex = 0;
            this.BtSave.Text = "保存";
            this.BtSave.UseVisualStyleBackColor = true;
            this.BtSave.Click += new System.EventHandler(this.Save_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "プレイヤー名";
            // 
            // TbPlayerName
            // 
            this.TbPlayerName.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TbPlayerName.Location = new System.Drawing.Point(176, 9);
            this.TbPlayerName.Name = "TbPlayerName";
            this.TbPlayerName.Size = new System.Drawing.Size(251, 46);
            this.TbPlayerName.TabIndex = 2;
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 249);
            this.Controls.Add(this.TbPlayerName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtSave);
            this.Name = "Setting";
            this.Text = "設定";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setting_FormClosed);
            this.Load += new System.EventHandler(this.Setting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbPlayerName;
    }
}