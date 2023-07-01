using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using UmeboshiLibrary;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using System.Drawing.Drawing2D;
using System.Media;
using System.Threading;

namespace MJLPRateCalc {
    public partial class Form1 : Form {

        private bool IsTechScoreUp;

        private bool IsRatingUp;

        private PlayerDataViewer playerDataViewer;

        private Setting setting;

        private ReadingMessage readingMessage;

        /// <summary>
        /// 出力用プレイヤーデータ
        /// </summary>
        private PlayerData playerData;

        /// <summary>
        /// 記録集
        /// </summary>
        private List<Record> records;

        /// <summary>
        /// 現在のレーティング
        /// </summary>
        private double Rating;

        /// <summary>
        /// レーティング増減値
        /// </summary>
        private double IncDec;

        private const string InfoFolderPath = @".\info";

        /// <summary>
        /// プレイヤーデータを書き込んでおくためのファイル
        /// </summary>
        private const string RatingFilePath = @".\info\PlayerData.mpr";

        /// <summary>
        /// 監視対象のフォルダを保存する
        /// </summary>
        private const string FolderText = @".\info\Target.txt";

        /// <summary>
        /// 監視対象のフォルダを保存する
        /// </summary>
        private const string VersionText = @".\info\Version.txt";

        /// <summary>
        /// スコアファイル保存用フォルダ名
        /// </summary>
        private const string ScoreFolder = @".\info\Scores";

        /// <summary>
        /// プレイ履歴保存用フォルダ名
        /// </summary>
        private const string RecentFolder = @".\info\Scores\Recent";

        /// <summary>
        /// スコアファイル拡張子
        /// </summary>
        private const string ScoreExtention = ".msc";

        /// <summary>
        /// テクニカルスコア更新用音声ファイルパス
        /// </summary>
        private const string TechScoreUpWave = @".\Wave\TechScoreUp.wav";


        /// <summary>
        /// レーティング上昇時用音声ファイルパス
        /// </summary>
        private const string RatingUpWave = @".\Wave\RatingUp.wav";

        /// <summary>
        /// プレイヤー名
        /// </summary>
        private string PlayerName;

        /// <summary>
        /// 監視対象フォルダパス
        /// </summary>
        private string TargetPath;

        private bool IsUpdated;

        public const string Version = "V1.2";

        public const int MapsVersion = 20230521;

        public Form1() {
            InitializeComponent();
            records = new List<Record>();
        }

        private void BtCalc_Click(object sender, EventArgs e) {
            RateCalc();
        }

        /// <summary>
        /// レーティングファイルを出力します
        /// </summary>
        private void WriteRatingFile() {
            XmlSerializer RatingXML = new XmlSerializer(typeof(PlayerData));

            using(StreamWriter sw = new StreamWriter(RatingFilePath)) {
                RatingXML.Serialize(sw, playerData);
                sw.Close();
                sw.Dispose();
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
            Rating = playerData.Rating;
            PlayerName = playerData.Name;
        }

        /// <summary>
        /// レート計算
        /// </summary>
        private void RateCalc() {
            try {

                DirectoryInfo tjaDirInfo = new DirectoryInfo(TargetPath);
                records = GetRecords(tjaDirInfo);

                if (records.Count < 30) Rating = records.Sum(x => x.Rating) / 30;
                else Rating = records.OrderByDescending(x => x.Rating).Take(30).Average(x => x.Rating);
                Rating = RoundDown(Rating, 2);


                RateColoring();
                GetIncDec();
                playerData = new PlayerData(PlayerName, Rating, records);
                WriteRatingFile();
                SEPlay();
                // MessageBox.Show("OK!");
            }
            catch (Exception ex){
                MessageBox.Show($"Error: {ex}");
            }
        }

        /// <summary>
        /// 差分を求めます
        /// </summary>
        private void GetIncDec() {

            if (!File.Exists(RatingFilePath)) {
                IncDec = Rating;
            } else {
                var BeforeRating = new PlayerData();

                XmlSerializer RatingXML = new XmlSerializer(typeof(PlayerData));
                using (StreamReader sr = new StreamReader(RatingFilePath)) {
                    BeforeRating = (PlayerData)RatingXML.Deserialize(sr);
                }
                if (BeforeRating == null) {
                    IncDec = Rating;
                    return;
                }

                IncDec = Rating - BeforeRating.Rating;

            }

            WriteRatingFile();

            IncDec = Math.Round(IncDec, 2);
            if (IncDec > 0) {
                LbIncDec.ForeColor = Color.FromArgb(255, 128, 128);
                LbIncDec.Text = $"(+{IncDec:F2})";

                IsRatingUp = true;
            } else if (IncDec == 0) {
                LbIncDec.ForeColor = Color.FromArgb(128, 128, 128);
                LbIncDec.Text = $"(±0.00)";
            } else {
                LbIncDec.ForeColor = Color.FromArgb(128, 128, 225);
                LbIncDec.Text = $"({IncDec:F2})";
            }
        }

        private void SEPlay() {
            if (IsTechScoreUp) {
                if (File.Exists(TechScoreUpWave)) {
                    SoundPlayer player = new SoundPlayer(TechScoreUpWave);
                    player.PlaySync();
                    player.Dispose();
                } 
            }
            if (IsRatingUp) {
                if (File.Exists(RatingUpWave)) {
                    SoundPlayer player = new SoundPlayer(RatingUpWave);
                    player.Play();
                    player.Dispose();
                }
            }
            IsRatingUp = false;
            IsTechScoreUp = false;
        }


        /// <summary>
        /// レーティングの表示
        /// </summary>
        private void RateColoring() {
            //if (Rating >= 14.00) {
            //    LbRating.ForeColor = Color.FromArgb(255, 255, 255);
            //    RainbowBox.Visible = false;
            //} else if (Rating >= 13.00) {
            //    RainbowDraw();
            //    RainbowBox.Visible = true;
            //} else if (Rating >= 12.00) {
            //    LbRating.ForeColor = Color.FromArgb(216, 195, 151);
            //    RainbowBox.Visible = false;
            //} else if (Rating >= 11.00) {
            //    LbRating.ForeColor = Color.Goldenrod;
            //} else if (Rating >= 10.00) {
            //    LbRating.ForeColor = Color.FromArgb(128, 128, 128);
            //} else if (Rating >= 9.00) {
            //    LbRating.ForeColor = Color.FromArgb(255, 102, 51);
            //} else 
            
            RainbowDraw();

        }

        /// <summary>
        /// レーティングカラー表示
        /// </summary>
        private void RainbowDraw() {
            RainbowBox.BackColor = Color.Black;
            Bitmap bitmap = new Bitmap(RainbowBox.Width, RainbowBox.Height);
            Graphics g = Graphics.FromImage(bitmap);

            double dpi = (double)DeviceDpi / 96 * 100;
            Image image;

            if (Rating >= 14.00) image = Image.FromFile($@".\Img\{dpi}\White.png");
            else if (Rating >= 13.00) image = Image.FromFile($@".\Img\{dpi}\Rainbow.png");
            else if (Rating >= 12.00) image = Image.FromFile($@".\Img\{dpi}\Platinum.png");
            else if (Rating >= 11.00) image = Image.FromFile($@".\Img\{dpi}\Gold.png");
            else if (Rating >= 10.00) image = Image.FromFile($@".\Img\{dpi}\Silver.png");
            else if (Rating >= 9.00) image = Image.FromFile($@".\Img\{dpi}\Copper.png");
            else if (Rating >= 8.00) image = Image.FromFile($@".\Img\{dpi}\Purple.png");
            else if (Rating >= 7.00) image = Image.FromFile($@".\Img\{dpi}\Red.png");
            else if (Rating >= 6.00) image = Image.FromFile($@".\Img\{dpi}\Orange.png");
            else if (Rating >= 5.00) image = Image.FromFile($@".\Img\{dpi}\Yellow.png");
            else if (Rating >= 3.00) image = Image.FromFile($@".\Img\{dpi}\Green.png");
            else image = Image.FromFile($@".\Img\{dpi}\Blue.png");

            //Rectangle rectangle = new Rectangle(0, 60, 20, 100);
            TextureBrush brush = new TextureBrush(image);


            GraphicsPath graphicsPath = new GraphicsPath();

            FontFamily fontFamily = new FontFamily("游明朝 Demibold");
            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Center;

            
            stringFormat.Alignment = StringAlignment.Center;



            graphicsPath.AddString($"{Rating.ToString("N2")}", 
                                    fontFamily, 
                                    (int)FontStyle.Bold, 
                                    (float)(140 * dpi / 100), 
                                    new Point(RainbowBox.Width / 2, RainbowBox.Height / 2 + 40), 
                                    stringFormat);

            g.FillPath(brush, graphicsPath);


            stringFormat.Dispose();
            graphicsPath.Dispose();
            
           
            fontFamily.Dispose();
            brush.Dispose();
            
            
            image.Dispose();
            g.Dispose();

            
            var oldBitmap = RainbowBox.Image;
            
            RainbowBox.Image = bitmap;
            if(oldBitmap!= null) oldBitmap.Dispose();


        }

        /// <summary>
        /// datファイルを基に記録を集める
        /// </summary>
        /// <param name="InputDirInfo"></param>
        /// <returns></returns>
        private List<Record> GetRecords(DirectoryInfo InputDirInfo) {
            // datを取得
            var DATs = InputDirInfo.GetFiles("*.dat", System.IO.SearchOption.AllDirectories).ToList();
            List<Record> records = new List<Record>();
            foreach (var DAT in DATs) {
                var record = DATtoRecord(DAT);
                Record.Write(record, Path.Combine(ScoreFolder, DAT.Directory.Name, Path.ChangeExtension(DAT.Name, ScoreExtention)));
                //if (record != null) records.Add(record);
            }

            if (!Directory.Exists(ScoreFolder)) { Directory.CreateDirectory(ScoreFolder); }
            DirectoryInfo MSCDInfo = new DirectoryInfo(ScoreFolder);
            var MiosunaScores = MSCDInfo.GetFiles("*.msc", System.IO.SearchOption.AllDirectories).ToList();

            List<Task> tasks = new List<Task>();
            foreach (var Msc in MiosunaScores) {
                tasks.Add(Task.Run(() => {
                    var record = Record.Read(Msc.FullName);
                    if (record != null) records.Add(record);

                }));
            }
            
            Task.WaitAll(tasks.ToArray());
            return records;

        }

        /// <summary>
        /// datファイルをもとに記録を抽出する
        /// </summary>
        /// <param name="datPath"></param>
        /// <returns></returns>
        private Record DATtoRecord(FileInfo datInfo) {
            try {
                // 対象のtjaが存在しない場合はスルー
                var tjaInfo = new FileInfo(datInfo.FullName.Replace($".dat", $".tja"));
                if (tjaInfo.Exists == false) return null;

                // ここで例外が起きたらランダム再生機くんを再ビルドしてね
                TJA tja = new TJA(tjaInfo);
                if (tja == null) return null;

                // バイナリ読み込み
                BinaryReader binaryReader = new BinaryReader(new FileStream(datInfo.FullName, FileMode.Open), Encoding.GetEncoding("Shift_jis"));
                // datファイルの中身を格納
                List<byte> bytes = new List<byte>();

                while (binaryReader.PeekChar() != -1) bytes.Add(binaryReader.ReadByte());
                if (bytes.Count == 0) {
                    datInfo.Delete();
                    return null;
                }

                // バイナリデータの改行文字を0x0Aへ変換
                for (int i = 0; i < bytes.Count; i++) {
                    if (i != bytes.Count() - 1 && bytes[i] == 0x0D) {
                        if (bytes[i + 1] == 0x0A) bytes.RemoveAt(i);
                    }
                }
                //foreach(var data in bytes.Select((v, i) => (v, i))){
                //    if(data.i != bytes.Count() - 1 && data.v == 0x0D) {
                //        if (bytes[data.i + 1] == 0x0A) bytes.RemoveAt(data.i);
                //    }
                //}
                binaryReader.Close();
                binaryReader.Dispose();

                List<byte> dat16byte = ByteSort(bytes);

                if (dat16byte == null) return null;

                string scoreFileName = Path.ChangeExtension(tjaInfo.Name, ScoreExtention);
                Record OldRecord = Record.Read(Path.Combine(ScoreFolder, datInfo.Directory.Name, scoreFileName));
                if(OldRecord != null) OldRecord.Rating = Record.CalcRate(OldRecord.Score, tja.LEVEL, OldRecord.JudgeLevel, OldRecord.Clear);
                

                Record NewRecord = new Record();
                NewRecord.DATPath = datInfo.FullName;
                NewRecord.Title = tja.TITLE;
                NewRecord.Level = tja.LEVEL;
                NewRecord.Great = BitConverter.ToInt16(dat16byte.ToArray(), 6);
                NewRecord.Good = BitConverter.ToInt16(dat16byte.ToArray(), 8);
                NewRecord.Bad = BitConverter.ToInt16(dat16byte.ToArray(), 10);
                NewRecord.DatScore = (int)BitConverter.ToInt64(dat16byte.ToArray(), 2);
                NewRecord.Score = Math.Floor((NewRecord.Great + NewRecord.Good * 0.5) / tja.NotesCount * 1000000);
                NewRecord.JudgeLevel = (int)NewRecord.DatScore % 10 == 0 ? 0 : (10 - (int)NewRecord.DatScore % 10);
                NewRecord.Clear = Record.GetClearMode(BitConverter.ToInt16(dat16byte.ToArray(), 14));
                NewRecord.Rating = Record.CalcRate(NewRecord.Score, tja.LEVEL, NewRecord.JudgeLevel, NewRecord.Clear);
                NewRecord.LastWrite = datInfo.LastWriteTime;
                Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(datInfo.FullName, Path.Combine(ScoreFolder, "BackUp", $"{Path.GetFileNameWithoutExtension(datInfo.Name)}_{NewRecord.Score}_{NewRecord.Clear.ToString()}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.dat"));

                // 未クリアであればレート値は無効になる
                // if (NewRecord.Clear == ClearMode.NoClear) NewRecord.Rating = RoundDown(NewRecord.Rating * 0.75, 2);
                // レートが高い方、クリア情報が高いほうが優先される
                if (OldRecord == null) {
                    IsTechScoreUp = true;
                    return NewRecord;
                }
                if (OldRecord.DATPath == null) {
                    IsTechScoreUp = true;
                    return NewRecord;
                }

                // 記録更新
                if (OldRecord.Clear < NewRecord.Clear && OldRecord.Rating < NewRecord.Rating) {
                    IsTechScoreUp = true;
                    return NewRecord;

                } else if (OldRecord.Clear < NewRecord.Clear && OldRecord.Rating > NewRecord.Rating) {
                    OldRecord.Clear = NewRecord.Clear;
                    return OldRecord;

                // 記録更新
                } else if (OldRecord.Clear > NewRecord.Clear && OldRecord.Rating < NewRecord.Rating) {
                    NewRecord.Clear = OldRecord.Clear;
                    IsTechScoreUp = true;
                    return NewRecord;
                } else if (OldRecord.Clear > NewRecord.Clear && OldRecord.Rating > NewRecord.Rating) {
                    return OldRecord;

                } else if (OldRecord.Clear > NewRecord.Clear && OldRecord.Rating == NewRecord.Rating) {
                    OldRecord.Clear = NewRecord.Clear;
                } else if (OldRecord.Clear < NewRecord.Clear && OldRecord.Rating == NewRecord.Rating) {
                    return NewRecord;
                } else if (OldRecord.Clear == NewRecord.Clear && OldRecord.Rating > NewRecord.Rating) {
                    return OldRecord;

                // 記録更新
                } else if (OldRecord.Clear == NewRecord.Clear && OldRecord.Rating < NewRecord.Rating) {
                    IsTechScoreUp = true;
                    return NewRecord;
                } else { return OldRecord; }


                return NewRecord;
            }
            catch(Exception ex) {
                return null;
            }
            
        }

        ///// <summary>
        ///// レート計算
        ///// </summary>
        ///// <param name="score"></param>
        ///// <param name="difficulty"></param>
        ///// <returns></returns>
        //private double RatingCalculate(double score, double difficulty, int judgeLevel, ClearMode clearMode) {
        //    double rate = 0;
        //    double bonus = 0;

        //    if (difficulty == 0) return 0;
        //    if (score < 500000) {
        //        return 0;
        //    } else if( score < 700000) {

        //        rate = difficulty * (score - 500000) / 200000;
        //    } else {
        //        rate = difficulty + (score - 700000) / 100000;
        //    }

        //    if (clearMode == ClearMode.FullCombo) {
        //        if (difficulty < 3) bonus = 0;
        //        else if (difficulty == 10) bonus = 0;
        //        else if (difficulty < 10) bonus = (10 - difficulty) * 1.2;
        //        else if (difficulty > 10) bonus = (difficulty - 10) * 0.4;
        //    } else if (score == 1000000) {
        //        if (difficulty < 3) bonus = 0;
        //        else if (difficulty == 10) bonus = 0;
        //        else if (difficulty < 10) bonus = (10 - difficulty) * 1.4;
        //        else if (difficulty > 10) bonus = (difficulty - 10) * 1.0;
        //    }
        //    rate += bonus;

        //    rate *= (1 - 0.05 * judgeLevel);
        //    var result = RoundDown(rate, 2);
        //    return result;
        //}

        /// <summary>
        /// 小数点n位で切り捨てにします
        /// </summary>
        /// <param name="val"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private double RoundDown(double val, int n) {
            var num = val * Math.Pow(10, n);
            num = Math.Floor(num);
            num /= Math.Pow(10, n);
            return num;
        }

        /// <summary>
        /// 正しい16ビットのdatデータに並べ替えて変換します
        /// </summary>
        private List<byte> ByteSort(List<byte> dat_contents) {
            if (dat_contents.Count == 0) return null;
            byte[] sorted_value = new byte[16];
            for (int i = 0; i < sorted_value.Length; i++) {
                sorted_value[i] = dat_contents[i];
            }

            return sorted_value.ToList();
        }

        private void VersionCheck() {
            if (!File.Exists(VersionText)) {
                MessageBox.Show("譜面定数変更パッチ起動後のtxtファイルが見つかりません。");
                this.Close();
                return;
            }
            string ver = "0";
            using (StreamReader sr = new StreamReader(VersionText)) { 
                ver = sr.ReadLine();
            }

            var verNum = int.Parse(ver);
            if (verNum < MapsVersion) {
                MessageBox.Show("最新の譜面定数変更パッチが適用されていません。");
                this.Close();
            }

        }
       
        /// <summary>
        /// 監視フォルダが設定されていなければ設定する
        /// </summary>
        private void SettingCheck() {
            if (!File.Exists(FolderText)) {
                while (String.IsNullOrEmpty(TargetPath) || !TargetPath.Contains("みおすな")) {
                    MessageBox.Show("みおすなちゃん次郎ライフパックの譜面が入ったフォルダを指定してください。");
                    var folderBrowserDialog = new CommonOpenFileDialog() {
                        IsFolderPicker = true,
                        InitialDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.FullName,
                        DefaultFileName = "1 譜面"

                    };
                    if (folderBrowserDialog.ShowDialog() == CommonFileDialogResult.Ok) {
                        
                        TargetPath = folderBrowserDialog.FileName;
                        FolderChange();
                    }

                }
                if (File.Exists(RatingFilePath)) {
                    MessageBox.Show("譜面のフォルダが変更されているため、取得に数分かかることがあります。");
                }
            }
            using (StreamReader sr = new StreamReader(FolderText, Encoding.GetEncoding("Shift_jis"))) {
                var line = sr.ReadLine();

                TargetPath = line;
            };
            if (!Directory.Exists(TargetPath)) {
                string MJLPPath = "";
                while (String.IsNullOrEmpty(MJLPPath)) {
                    MessageBox.Show("みおすなちゃん次郎ライフパックの譜面が入ったフォルダを指定してください。");
                    var folderBrowserDialog = new CommonOpenFileDialog() {
                        IsFolderPicker = true,
                        InitialDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.FullName,
                        DefaultFileName = "1 譜面"
                    };
                    if (folderBrowserDialog.ShowDialog() == CommonFileDialogResult.Ok) {
                        MJLPPath = folderBrowserDialog.FileName;
                        TargetPath = folderBrowserDialog.FileName;
                        FolderChange();
                    }

                }
                if (File.Exists(RatingFilePath)) {
                    MessageBox.Show("譜面のフォルダが変更されているため、取得に数分かかることがあります。");
                }
            }

            
        }

        private void Form1_Load(object sender, EventArgs e) {
            this.Text = $"みおちゃんパックレーティング計算機 {Version}";
            playerData = new PlayerData();
            playerDataViewer = new PlayerDataViewer();
            setting = new Setting();
            readingMessage = new ReadingMessage();

            //Task.Run(() => {
            //    readingMessage.Show();
            //});
            SettingCheck();
            RatingLoad();
            ReadFolder();
            WatcherInit();
            RateCalc();
            RainbowBox.BackColor = Color.Black;
            IsUpdated = false;
            IsTechScoreUp = false;
            IsRatingUp = false;

            RainbowDraw();
            //readingMessage.Close();
        }

        private void WatcherInit() {
            fileSystemWatcher1.Path = TargetPath;
            fileSystemWatcher1.Filter = "*.dat";
            fileSystemWatcher1.NotifyFilter = NotifyFilters.LastWrite;
            fileSystemWatcher1.IncludeSubdirectories = true;
        }

        private void RatingLoad() {
            if (!File.Exists(RatingFilePath)) {
                while (String.IsNullOrEmpty(PlayerName)) PlayerName = Interaction.InputBox("プレイヤー名を入力してください。");
                Rating = 0.00;
                return;
            } else {
                ReadRatingFile();
                while(String.IsNullOrEmpty(PlayerName)) PlayerName = Interaction.InputBox("プレイヤー名を入力してください。");

            }
            playerData = new PlayerData(PlayerName, Rating, records);
            WriteRatingFile();
            LbIncDec.ForeColor = Color.FromArgb(128, 128, 128);
            RateColoring();
        }

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e) {
           // RateCalc();
        }

        /// <summary>
        /// dat状況を監視する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e) {
            //　なぜか2回参照しやがるので応急措置
            
            if (!IsUpdated) {
                RateCalc();
                IsUpdated = true;
            } else {
                IsUpdated = false;
            }
        }

        private void TJAFolderBox_TextChanged(object sender, EventArgs e) {
            FolderChange();
            WatcherInit();
        }

        /// <summary>
        /// 監視対象フォルダを変更します
        /// </summary>
        private void FolderChange() {
            if(!Directory.Exists(InfoFolderPath)) Directory.CreateDirectory(InfoFolderPath);

            StreamWriter streamWriter = new StreamWriter(FolderText, false, Encoding.GetEncoding("Shift_jis"));
            streamWriter.WriteLine(TargetPath);
            streamWriter.Close();
            streamWriter.Dispose();
        }

        private void ReadFolder() {
            if (!File.Exists(FolderText)) return;
            string line = "";
            using (StreamReader sr = new StreamReader(FolderText, Encoding.GetEncoding("Shift_jis"))) {
                line = sr.ReadLine();
            }
            TargetPath = line;
        }

        private void BtPlayerData_Click(object sender, EventArgs e) {
            if (playerDataViewer.IsDisposed) {

                playerDataViewer = new PlayerDataViewer();
            }
            if(playerDataViewer.IsEnabled == true) {
                playerDataViewer.TopMost = true;
                playerDataViewer.TopMost = false;

                MessageBox.Show("既に開かれています。");
                this.TopMost = true;
                return;
            }
            playerDataViewer.Show();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e) {

        }

        private void button1_Click_1(object sender, EventArgs e) {
            Rating += 0.5;
            Rating = Math.Round(Rating, 2);
            
            RateColoring();
        }

        private void Form1_Shown(object sender, EventArgs e) {
            VersionCheck();
            RainbowDraw();
        }

        private void button2_Click(object sender, EventArgs e) {
            Rating -= 0.5;
            Rating = Math.Round(Rating, 2);

            RateColoring();
        }

        private void BtSetting_Click(object sender, EventArgs e) {
            if (setting.IsDisposed) {

                setting = new Setting();
            }
            if (setting.IsEnabled == true) {
                setting.TopMost = true;
                setting.TopMost = false;

                MessageBox.Show("既に開かれています。");
                this.TopMost = true;
                return;
            }
            setting.Show();
        }
    }
}
