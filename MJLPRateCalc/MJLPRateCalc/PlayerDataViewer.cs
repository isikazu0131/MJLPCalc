using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MJLPRateCalc {
    public partial class PlayerDataViewer : Form {

        /// <summary>
        /// プレイヤーデータ
        /// </summary>
        private PlayerData playerData;


        #region プレイヤ内部データ

        /// <summary>
        /// 全楽曲の単曲レート合計値
        /// </summary>
        private double RatingSum = 0;

        #endregion

        /// <summary>
        /// プレイヤーデータを書き込んでおくためのファイル
        /// </summary>
        private const string RatingFilePath = @".\info\PlayerData.mpr";

        /// <summary>
        /// 開かれているか
        /// </summary>
        public bool IsEnabled;

        public PlayerDataViewer() {
            IsEnabled = false;
            InitializeComponent();
        }

        private void Name_Click(object sender, EventArgs e) {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void PlayerDataViewer_Load(object sender, EventArgs e) {
            this.Text = $"プレイヤーデータ閲覧画面 {Form1.Version}";
            IsEnabled = true;
            ChangeDgvMode(true);
            ReadRatingFile();

            ShowPlayData();
            ShowBESTData();
            ShowOtherData();

            ShowPlayerInfo();
            ChangeDgvMode(false);
        }

        private void ChangeDgvMode(bool IsStart) {
            if (IsStart) {
                DgvAllScore.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                DgvAllScore.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                DgvBEST.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                DgvBEST.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            } else {
                DgvAllScore.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                DgvAllScore.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                DgvBEST.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                DgvBEST.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }

        }

        /// <summary>
        /// レーティングファイルを読み込みます
        /// </summary>
        private void ReadRatingFile() {

            // 無ければ弾く
            if (!File.Exists(RatingFilePath)) { return; }

            XmlSerializer RatingXML = new XmlSerializer(typeof(PlayerData));

            using (StreamReader sr = new StreamReader(RatingFilePath)) {
                playerData = (PlayerData)RatingXML.Deserialize(sr);
                sr.Close();
                sr.Dispose();
            }
        }

        private void ShowPlayerInfo() {
            LbRating.Text = playerData.Rating.ToString("0.00");
            LbName.Text = playerData.Name;
            LbPlayedCount.Text = playerData.Records.Count().ToString();
        }

        /// <summary>
        /// DataGridViewへBEST枠を表示します
        /// </summary>
        private void ShowBESTData() {
            DgvBEST.Columns[2].DefaultCellStyle.Format = "N1";
            DgvBEST.Columns[10].DefaultCellStyle.Format = "N2";
            foreach (var record in playerData.Records.OrderByDescending(x => x.Rating).Take(30).Select((v, i) => (v, i))) {
                if (record.v == null) break;
                DgvBEST.RowCount = 31;
                DgvBEST[0, record.i].Value = record.i + 1;  // 順位
                DgvBEST[1, record.i].Value = record.v.Title;
                DgvBEST[2, record.i].Value = record.v.Level;
                DgvBEST[3, record.i].Value = record.v.Great;
                DgvBEST[4, record.i].Value = record.v.Good;
                DgvBEST[5, record.i].Value = record.v.Bad;
                DgvBEST[6, record.i].Value = record.v.Score;
                DgvBEST[7, record.i].Value = GetRank(record.v.Score);
                DgvBEST[8, record.i].Value = record.v.Clear.ToString();
                DgvBEST[9, record.i].Value = record.v.JudgeLevel;
                DgvBEST[10, record.i].Value = record.v.Rating;
                DgvBEST[11, record.i].Value = record.v.LastWrite.ToString("yyyy/MM/dd HH:mm:ss");

            }
        }

        private void ShowPlayData() {

            DgvAllScore.Columns[2].DefaultCellStyle.Format = "N1";
            DgvAllScore.Columns[10].DefaultCellStyle.Format = "N2";
            foreach (var record in playerData.Records.Select((v, i) => (v, i))) {
                if (record.v == null) break;
                DgvAllScore.RowCount = record.i + 2;
                DgvAllScore[0, record.i].Value = record.i + 1;  // 順位
                DgvAllScore[1, record.i].Value = record.v.Title;
                DgvAllScore[2, record.i].Value = record.v.Level;
                DgvAllScore[3, record.i].Value = record.v.Great;
                DgvAllScore[4, record.i].Value = record.v.Good;
                DgvAllScore[5, record.i].Value = record.v.Bad;
                DgvAllScore[6, record.i].Value = record.v.Score;
                DgvAllScore[7, record.i].Value = GetRank(record.v.Score);
                DgvAllScore[8, record.i].Value = record.v.Clear.ToString();
                DgvAllScore[9, record.i].Value = record.v.JudgeLevel;
                DgvAllScore[10, record.i].Value = record.v.Rating;
                DgvAllScore[11, record.i].Value = record.v.LastWrite.ToString("yyyy/MM/dd HH:mm:ss");

            }
            //PlayedCount = playerData.Records.Count();
        }

        private void ShowOtherData() {
            List<double> LevelList = new List<double>() {
                0,
                1.0,
                4.5,
                5.0,
                5.5,
                6.0,
                6.5,
                7.0,
                7.5,
                8.0,
                8.5,
                9.0,
                9.5,
                10.0,
                10.5,
                11.0,
                11.5,
                12.0,
                12.5,
                13.0,
                13.5,
                14.0,
                14.5,
                15.0,
                15.5,
                16.0
            };

            DgvOther.RowCount = LevelList.Count + 2;
            /*
             * 0  : 難易度
             * 1  : クリア数
             * 2  : ハードクリア数
             * 3  : フルコンボ数
             * 4  : 平均スコア
             * 5  : レート合計値
             * 6  : 全良数
             * 7  : SSS+ (995000-)
             * 8  : SSS  (990000-)
             * 9  : SS+  (975000-)
             * 10 : SS   (950000-)
             * 11 : S+   (925000-)
             * 12 : S    (900000-)
             * 13 : AAA  (875000-)
             * 14 : AA   (850000-)
             * 15 : A    (800000-)
            */

            DgvOther.Columns[5].DefaultCellStyle.Format = "N2";
            foreach (var level in LevelList.Select((v, i) => (v, i))) {
                DgvOther[ 0, level.i].Value = IsPlus(level.v) ? $"☆{Math.Truncate(level.v)}+" : $"☆{Math.Truncate(level.v)}";
                DgvOther[ 1, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Clear == ClearMode.Clear).Count();
                DgvOther[ 2, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Clear == ClearMode.HardClear).Count();
                DgvOther[ 3, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Clear == ClearMode.FullCombo).Count();
                DgvOther[ 4, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Count() > 0 ? Math.Round(playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Average(x => x.Score)) : 0;
                DgvOther[ 5, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Sum(x => x.Rating);
                DgvOther[ 6, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score == 1000000).Count();
                DgvOther[ 7, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score >= 995000 && x.Score < 1000000).Count();
                DgvOther[ 8, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score >= 990000 && x.Score < 995000).Count();
                DgvOther[ 9, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score >= 980000 && x.Score < 990000).Count();
                DgvOther[10, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score >= 970000 && x.Score < 980000).Count();
                DgvOther[11, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score >= 960000 && x.Score < 970000).Count();
                DgvOther[12, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score >= 950000 && x.Score < 960000).Count();
                DgvOther[13, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score >= 925000 && x.Score < 950000).Count();
                DgvOther[14, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score >= 900000 && x.Score < 925000).Count();
                DgvOther[15, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score >= 850000 && x.Score < 900000).Count();
                DgvOther[16, level.i].Value = playerData.Records.Where(x => x.Level >= level.v && x.Level <= level.v + 0.4).Where(x => x.Score >= 800000 && x.Score < 850000).Count();

            }

            DgvOther[ 0, 26].Value = "合計";
            DgvOther[ 1, 26].Value = playerData.Records.Where(x => x.Clear == ClearMode.Clear).Count();
            DgvOther[ 2, 26].Value = playerData.Records.Where(x => x.Clear == ClearMode.HardClear).Count();
            DgvOther[ 3, 26].Value = playerData.Records.Where(x => x.Clear == ClearMode.FullCombo).Count();
            DgvOther[ 4, 26].Value = playerData.Records.Count() > 0 ? Math.Round(playerData.Records.Average(x => x.Score)) : 0;
            DgvOther[ 5, 26].Value = playerData.Records.Sum(x => x.Rating);
            DgvOther[ 6, 26].Value = playerData.Records.Where(x => x.Score == 1000000).Count();
            DgvOther[ 7, 26].Value = playerData.Records.Where(x => x.Score >= 995000 && x.Score < 1000000).Count();
            DgvOther[ 8, 26].Value = playerData.Records.Where(x => x.Score >= 990000 && x.Score < 995000).Count();
            DgvOther[ 9, 26].Value = playerData.Records.Where(x => x.Score >= 980000 && x.Score < 990000).Count();
            DgvOther[10, 26].Value = playerData.Records.Where(x => x.Score >= 970000 && x.Score < 980000).Count();
            DgvOther[11, 26].Value = playerData.Records.Where(x => x.Score >= 960000 && x.Score < 970000).Count();
            DgvOther[12, 26].Value = playerData.Records.Where(x => x.Score >= 950000 && x.Score < 960000).Count();
            DgvOther[13, 26].Value = playerData.Records.Where(x => x.Score >= 925000 && x.Score < 950000).Count();
            DgvOther[14, 26].Value = playerData.Records.Where(x => x.Score >= 900000 && x.Score < 925000).Count();
            DgvOther[15, 26].Value = playerData.Records.Where(x => x.Score >= 850000 && x.Score < 900000).Count();
            DgvOther[16, 26].Value = playerData.Records.Where(x => x.Score >= 800000 && x.Score < 850000).Count();
        }

        private bool IsPlus(double level) {
            if (level - Math.Truncate(level) > 0) return true;
            else return false;
        }

        private string GetRank(double score) {
            if (score == 1000000) return "PERFECT";
            else if (score >= 995000) return "S+";
            else if (score >= 990000) return "S";
            else if (score >= 980000) return "AAA+";
            else if (score >= 970000) return "AAA";
            else if (score >= 960000) return "AA+";
            else if (score >= 950000) return "AA";
            else if (score >= 925000) return "A+";
            else if (score >= 900000) return "A";
            else if (score >= 850000) return "B+";
            else if (score >= 800000) return "B";
            else if (score >= 750000) return "C+";
            else if (score >= 700000) return "C";
            else if (score >= 650000) return "D+";
            else if (score >= 600000) return "D";
            else if (score >= 500000) return "E";
            else return "F";
        }

        private void BtReload_Click(object sender, EventArgs e) {
            ChangeDgvMode(true);
            ReadRatingFile();

            ShowPlayData();
            ShowBESTData();
            ShowOtherData();
            ShowPlayerInfo();
            ChangeDgvMode(false);
        }

        private void ScoreView_SelectedIndexChanged(object sender, EventArgs e) {
            if (ScoreView.SelectedIndex == 2 || ScoreView.SelectedIndex == 3) {
                System.Media.SystemSounds.Hand.Play();
            }
        }

        private void PlayerDataViewer_FormClosed(object sender, FormClosedEventArgs e) {
            IsEnabled = false;
        }
    }
}
