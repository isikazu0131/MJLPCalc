namespace MJLPRateCalc {
    partial class ReadingMessage {
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 30F);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(624, 120);
            this.label1.TabIndex = 0;
            this.label1.Text = "読み込み中！\r\n時間かかるかも！！！！";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 100F);
            this.label2.Location = new System.Drawing.Point(12, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(266, 216);
            this.label2.TabIndex = 1;
            this.label2.Text = "ご";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 100F);
            this.label3.Location = new System.Drawing.Point(238, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(266, 216);
            this.label3.TabIndex = 2;
            this.label3.Text = "め";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 100F);
            this.label4.Location = new System.Drawing.Point(522, 195);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(266, 216);
            this.label4.TabIndex = 3;
            this.label4.Text = "ん";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(629, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 116);
            this.button1.TabIndex = 4;
            this.button1.Text = "早く終わらせる";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ReadingMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ReadingMessage";
            this.Text = "ReadingMessage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}