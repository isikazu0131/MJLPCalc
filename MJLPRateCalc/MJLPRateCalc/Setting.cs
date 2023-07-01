using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MJLPRateCalc {
    public partial class Setting : Form {
        /// <summary>
        /// 開かれているか
        /// </summary>
        public bool IsEnabled;

        public Setting() {
            IsEnabled = false;
            InitializeComponent();
        }

        private void Setting_Load(object sender, EventArgs e) {
            IsEnabled = true;
            this.MaximumSize = this.MinimumSize;
            this.MinimumSize = this.MaximumSize;

            //TbPlayerName.Text = 
        }

        private void Setting_FormClosed(object sender, FormClosedEventArgs e) {
            IsEnabled = false;
        }

        private void Save_Click(object sender, EventArgs e) {

        }
    }
}
