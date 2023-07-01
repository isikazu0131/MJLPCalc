
namespace MJLPRateCalc {
    partial class Form1 {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.BtCalc = new System.Windows.Forms.Button();
            this.LbIncDec = new System.Windows.Forms.Label();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.label1 = new System.Windows.Forms.Label();
            this.BtPlayerData = new System.Windows.Forms.Button();
            this.RainbowBox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.BtSetting = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RainbowBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BtCalc
            // 
            this.BtCalc.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold);
            this.BtCalc.Location = new System.Drawing.Point(596, 206);
            this.BtCalc.Margin = new System.Windows.Forms.Padding(4);
            this.BtCalc.Name = "BtCalc";
            this.BtCalc.Size = new System.Drawing.Size(250, 222);
            this.BtCalc.TabIndex = 0;
            this.BtCalc.Text = "手動dat読み込み";
            this.BtCalc.UseVisualStyleBackColor = true;
            this.BtCalc.Click += new System.EventHandler(this.BtCalc_Click);
            // 
            // LbIncDec
            // 
            this.LbIncDec.BackColor = System.Drawing.Color.Black;
            this.LbIncDec.Font = new System.Drawing.Font("游明朝 Demibold", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbIncDec.ForeColor = System.Drawing.Color.DarkGray;
            this.LbIncDec.Location = new System.Drawing.Point(14, 338);
            this.LbIncDec.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbIncDec.Name = "LbIncDec";
            this.LbIncDec.Size = new System.Drawing.Size(568, 92);
            this.LbIncDec.TabIndex = 4;
            this.LbIncDec.Text = "(±0.00)";
            this.LbIncDec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            this.fileSystemWatcher1.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Changed);
            this.fileSystemWatcher1.Created += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Created);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("游明朝 Demibold", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(574, 94);
            this.label1.TabIndex = 5;
            this.label1.Text = "Your Rating";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtPlayerData
            // 
            this.BtPlayerData.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold);
            this.BtPlayerData.Location = new System.Drawing.Point(596, 14);
            this.BtPlayerData.Margin = new System.Windows.Forms.Padding(4);
            this.BtPlayerData.Name = "BtPlayerData";
            this.BtPlayerData.Size = new System.Drawing.Size(250, 184);
            this.BtPlayerData.TabIndex = 6;
            this.BtPlayerData.Text = "スコア一覧表示";
            this.BtPlayerData.UseVisualStyleBackColor = true;
            this.BtPlayerData.Click += new System.EventHandler(this.BtPlayerData_Click);
            // 
            // RainbowBox
            // 
            this.RainbowBox.BackColor = System.Drawing.Color.Black;
            this.RainbowBox.Location = new System.Drawing.Point(14, 14);
            this.RainbowBox.Margin = new System.Windows.Forms.Padding(4);
            this.RainbowBox.Name = "RainbowBox";
            this.RainbowBox.Size = new System.Drawing.Size(574, 416);
            this.RainbowBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.RainbowBox.TabIndex = 7;
            this.RainbowBox.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(890, 14);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(278, 34);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(890, 74);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(278, 34);
            this.button2.TabIndex = 9;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // BtSetting
            // 
            this.BtSetting.Font = new System.Drawing.Font("游ゴシック", 12F, System.Drawing.FontStyle.Bold);
            this.BtSetting.Location = new System.Drawing.Point(596, 207);
            this.BtSetting.Margin = new System.Windows.Forms.Padding(4);
            this.BtSetting.Name = "BtSetting";
            this.BtSetting.Size = new System.Drawing.Size(250, 106);
            this.BtSetting.TabIndex = 10;
            this.BtSetting.Text = "設定";
            this.BtSetting.UseVisualStyleBackColor = true;
            this.BtSetting.Click += new System.EventHandler(this.BtSetting_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(854, 441);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.LbIncDec);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.RainbowBox);
            this.Controls.Add(this.BtPlayerData);
            this.Controls.Add(this.BtCalc);
            this.Controls.Add(this.BtSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "みおちゃんパックレーティング計算機";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RainbowBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtCalc;
        private System.Windows.Forms.Label LbIncDec;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtPlayerData;
        private System.Windows.Forms.PictureBox RainbowBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button BtSetting;
    }
}

