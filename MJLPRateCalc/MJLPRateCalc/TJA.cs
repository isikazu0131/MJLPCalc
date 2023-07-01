using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UmeboshiLibrary;
using System.Xml.Serialization;

namespace MJLPRateCalc {
    /// <summary>
    /// tjaファイルに関するクラス
    /// </summary>
    public class TJA {

        // 譜面の外部情報
        public string TITLE;
        public string SUBTITLE;
        public string WAVE;
        public double OFFSET;
        public double DEMOSTART;
        public double BPM;
        public double SONGVOL;
        public double SEVOL;
        public DIFFICULTY COURSE;
        public double LEVEL;
        public int SCOREMODE;

        /// <summary>
        /// 加算スコア初期値
        /// </summary>
        public double SCOREINIT;

        /// <summary>
        /// 加算スコアの加算値
        /// </summary>
        public double SCOREDIFF;

        // 譜面の内部情報
        /// <summary>
        /// TJAのパス
        /// </summary>
        public FileInfo TJAPath;

        /// <summary>
        /// ノーツ情報
        /// </summary>
        public List<Note> Notes;

        /// <summary>
        /// 小節情報
        /// </summary>
        public List<Measure> Measures;

        /// <summary>
        /// ノーツ数
        /// </summary>
        public int NotesCount;

        /// <summary>
        /// 最高BPM
        /// </summary>
        public double MaxBPM;

        /// <summary>
        /// 最低BPM
        /// </summary>
        public double MinBPM;

        /// <summary>
        /// 音源再生時間
        /// </summary>
        public double MusicPlayTime;

        /// <summary>
        /// 譜面再生時間
        /// </summary>
        public double TJAPlayTime;

        // コンストラクタ
        public TJA() {

        }

        static public List<TJA> GetTJAs(DirectoryInfo directoryInfo) {
            var tja_infos = directoryInfo.GetFiles("*.tja", SearchOption.AllDirectories);
            List<TJA> TJAs = new List<TJA>();
            foreach (var tja in tja_infos) {
                TJAs.Add(new TJA(tja));
            }
            return TJAs;
        }

        public TJA(FileInfo Path) {
            try {
                TJAPath = Path;
                Notes = new List<Note>();
                var Contents = File.ReadAllLines(Path.FullName, Encoding.GetEncoding("Shift_JIS"));
                NotesCount = GetNotes(Contents);

                // 抽出した文字列を一時格納
                string extracted_data;
                foreach (var line in Contents) {

                    string processedline = DeleteComment(line).Trim(); // 余計なコメントを削除します

                    if (processedline.StartsWith("TITLE:")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "TITLE:");
                        TITLE = extracted_data;
                    }
                    if (processedline.StartsWith("SUBTITLE:")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "SUBTITLE:");
                        SUBTITLE = extracted_data;
                    }
                    if (processedline.StartsWith("WAVE:")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "WAVE:");
                        WAVE = extracted_data;
                    }
                    if (processedline.StartsWith("OFFSET:")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "OFFSET:");
                        extracted_data = DataToNumString(extracted_data);
                        if (double.TryParse(extracted_data, out double extracted_data_converted) == false) {
                            OFFSET = 0;
                            continue;
                        }
                        OFFSET = extracted_data_converted;
                    }
                    if (processedline.StartsWith("DEMOSTART:")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "DEMOSTART:");
                        extracted_data = DataToNumString(extracted_data);
                        if (String.IsNullOrEmpty(extracted_data) || double.TryParse(extracted_data, out double extracted_data_converted) == false) {
                            DEMOSTART = 0;
                            continue;
                        }
                        DEMOSTART = extracted_data_converted;
                    }
                    if (processedline.StartsWith("BPM:")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "BPM:").Trim();
                        extracted_data = System.Text.RegularExpressions.Regex.Matches(extracted_data, @"[0-9]+\.?[0-9]*")[0].Value;
                        BPM = double.Parse(extracted_data);
                        MinBPM = BPM;
                        MaxBPM = BPM;
                    }
                    if (processedline.StartsWith("SONGVOL:")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "SONGVOL:");
                        extracted_data = DataToNumString(extracted_data);
                        if (String.IsNullOrEmpty(extracted_data) || double.TryParse(extracted_data, out double extracted_data_converted) == false) {
                            SONGVOL = 100;
                            continue;
                        }
                        SONGVOL = double.Parse(extracted_data);
                    }
                    if (processedline.StartsWith("SEVOL:")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "SEVOL:");
                        extracted_data = DataToNumString(extracted_data);
                        if (String.IsNullOrEmpty(extracted_data) || double.TryParse(extracted_data, out double extracted_data_converted) == false) {
                            SEVOL = 100;
                            continue;
                        }
                        SEVOL = double.Parse(UmeboshiString.CutToEnd(extracted_data, "SEVOL:"));
                    }
                    if (processedline.StartsWith("COURSE:")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "COURSE:");
                        COURSE = GetCourse(extracted_data);
                    }
                    if (processedline.StartsWith("#BPMCHANGE")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "#BPMCHANGE");
                        if (String.IsNullOrEmpty(extracted_data) || double.TryParse(extracted_data, out double ChangedBPM) == false) {
                            continue;
                        }
                        if (MaxBPM < ChangedBPM) MaxBPM = ChangedBPM;
                        if (MinBPM > ChangedBPM) MinBPM = ChangedBPM;
                    }
                    if (processedline.StartsWith("LEVEL:")) {
                        extracted_data = UmeboshiString.CutToEnd(processedline, "LEVEL:");
                        extracted_data = DataToNumString(extracted_data);
                        if (String.IsNullOrEmpty(extracted_data) || double.TryParse(extracted_data, out double extracted_data_converted) == false) {
                            LEVEL = 0;
                            continue;
                        }
                        LEVEL = extracted_data_converted;
                    }

                }


            }
            catch (Exception ex){
                
            }
            
        }

        /// <summary>
        /// 数値情報に変換します
        /// </summary>
        private string DataToNumString(string data) {
            if(System.Text.RegularExpressions.Regex.Matches(data, @"[0-9]+\.?[0-9]*").Count == 0) {
                return "";
            }
            return System.Text.RegularExpressions.Regex.Matches(data, @"[0-9]+\.?[0-9]*")[0].Value;
        }

        private DIFFICULTY GetCourse(string coursetxt) {
            if (int.TryParse(coursetxt, out var coursevalue)) {
                switch (coursevalue) {
                    case 0: return DIFFICULTY.EASY;
                    case 1: return DIFFICULTY.NORMAL;
                    case 2: return DIFFICULTY.HARD;
                    case 3: return DIFFICULTY.ONI;
                    case 4: return DIFFICULTY.EDIT;
                    default: return DIFFICULTY.ONI;
                }
            } else {
                switch (coursetxt) {
                    case "Easy": return DIFFICULTY.EASY;
                    case "Normal": return DIFFICULTY.NORMAL;
                    case "Hard": return DIFFICULTY.HARD;
                    case "Oni": return DIFFICULTY.ONI;
                    case "Edit": return DIFFICULTY.EDIT;
                    default: return DIFFICULTY.ONI;
                }
            }
        }

        /// <summary>
        /// ノーツ数をカウント、音符の取得をします
        /// </summary>
        /// <param name="Contains"></param>
        /// <returns></returns>
        private int GetNotes(string[] Contents) {
            int count = 0; // 現在の行数取得
            int combo = 0; // コンボ数
            bool isTJA = false;
            string DeleteCommentContent; // コメントを削除した行
            while (count < Contents.Length) {

                if (Contents[count].StartsWith("#START")) { 
                    isTJA = true;
                    break; 
                
                }
                count++;
            }
            if(isTJA == false) return 0;
            for (int i = count; i < Contents.Length; i++) {
                DeleteCommentContent = DeleteComment(Contents[i]);
                if (DeleteCommentContent.StartsWith("#MEASURE") ||
                    DeleteCommentContent.StartsWith("#BPMCHANGE") ||
                    DeleteCommentContent.StartsWith("#SCROLL") ||
                    DeleteCommentContent.StartsWith("#DELAY") ||
                    DeleteCommentContent.StartsWith("//")) continue;
                // 「1,2,3,4」の個数を計測
                for (int j = 0; j < Contents[i].Length; j++) {
                    if (Contents[i][j] == '1' || Contents[i][j] == '2' || Contents[i][j] == '3' || Contents[i][j] == '4') {
                        Notes.Add(new Note(int.Parse(Contents[i][j].ToString()), combo));
                        combo++;
                    }
                }

                if (Contents[i].StartsWith("#END")) break;
            }

            return combo;
        }

        ///// <summary>
        ///// 小節情報を登録し、譜面再生時間を計算します
        ///// </summary>
        ///// <returns></returns>
        //private double GetTJAPlayTime(string[] Contents) {
        //    int count = 0; // 現在の行数取得

        //    double result = 0; // 計算結果

        //    List<Measure> Measures = new List<Measure>();

        //    List<string> measure_strings = new List<string>(); // 1小節内の譜面情報を1行の文字列に一時格納

        //    double NowBPM = BPM;
        //    double NowMeasure_den = 4; // 現在の拍子の分母部分
        //    double NowMeasure_mol = 4; // 現在の拍子の分子部分
        //    string DeleteCommentContent; // コメントを削除した行

        //    bool MapStart = false;

        //    while (true) {
        //        count++;
        //        if (Contents[count].StartsWith("#START")) break;
        //    }
        //    count++;

        //    // 1行ずつ処理
        //    for (int i = count; i < Contents.Length; i++) {

        //        // コメントを除去
        //        DeleteCommentContent = DeleteComment(Contents[i]);

        //        if (DeleteCommentContent.EndsWith(",") == false) {
        //            measure_strings.Add(DeleteCommentContent);


        //        } else {
        //            measure_strings.Add(DeleteCommentContent);
        //            // 1小節あたりの処理

        //        }

        //        //DeleteCommentContent = DeleteComment(Contents[i]);
        //        //if (DeleteCommentContent.StartsWith("#MEASURE")) {
        //        //    NowMeasure_den = double.Parse(UmeboshiString.CutToEnd(UmeboshiString.CutToEnd(DeleteCommentContent, "#MEASURE"), '/'));
        //        //    NowMeasure_mol = double.Parse(UmeboshiString.CutToStart(UmeboshiString.CutToEnd(DeleteCommentContent, "#MEASURE"), '/'));
        //        //    continue;

        //        //}
        //        //if (DeleteCommentContent.StartsWith("#BPMCHANGE")) {
        //        //    if(MeasureRow.Length != 0) {

        //        //    }
        //        //    NowBPM = double.Parse(UmeboshiString.CutToEnd(DeleteCommentContent, "#BPMCHNGE"));
        //        //    continue;

        //        //}
        //        //if (DeleteCommentContent.StartsWith("#DELAY")) {
        //        //    result += double.Parse(UmeboshiString.CutToEnd(DeleteCommentContent, "#DELAY"));
        //        //    continue;

        //        //}
        //        //if (DeleteCommentContent.StartsWith("#SCROLL")) {
        //        //    continue;
        //        //}

        //        //if (DeleteCommentContent.EndsWith(",") == false) {
        //        //    MeasureRow += DeleteCommentContent;
        //        //} else {
        //        //    MeasureRow += DeleteCommentContent;

        //        //    // 小節情報を取得
        //        //    // Measures.Add(new Measure(MeasureRow, NowMeasure_den, NowMeasure_mol));

        //        //    MeasureRow = "";
        //        //}

        //        //if(MapStart == false) {
        //        //    if (DeleteCommentContent.Contains("1") ||
        //        //        DeleteCommentContent.Contains("2") ||
        //        //        DeleteCommentContent.Contains("3") ||
        //        //        DeleteCommentContent.Contains("4")) {
        //        //        MapStart = true;
        //        //    }
        //        //} else {
        //        //    result += 60 / NowBPM * 4 * (NowMeasure_mol / NowMeasure_den);
        //        //}



        //        if (Contents[i].StartsWith("#END")) break;
        //    }

        //    return result;
        //}

        /// <summary>
        /// 余計なコメントを削除します
        /// </summary>
        /// <param name="line">読み込まれた行</param>
        /// <returns></returns>
        private string DeleteComment(string line) {
            if (line.Contains("//") == false) return line;
            else if (line.StartsWith("//")) return line;
            try {
                return UmeboshiString.CutToStart(line, "//");
            }
            catch {
                return "";
            }
            
        }

    }

    public class Note {
        public NoteKind noteKind;
        public Fingering fingering;
        public double timing;

        public Note(int kind, int count) {
            noteKind = (NoteKind)kind;
            if (count % 2 == 0) {
                fingering = Fingering.Right;
            } else {
                fingering = Fingering.Left;
            }
        }

        public Note(int kind) {
            noteKind = (NoteKind)kind;
        }

        public enum Fingering {
            Left,
            Right
        }

        public enum NoteKind {
            None,
            Dong,
            Ka,
            DongL,
            KaL,
            Roll,
            RollL,
            Balloon,
            RollEnd,
            Poteto
        }
    }

    /// <summary>
    /// 各小節の内部情報
    /// </summary>
    public class Measure {

        /// <summary>
        /// 現在のBPM
        /// </summary>
        public double BPM;

        /// <summary>
        /// 拍子の分母部分
        /// </summary>
        public double Measure_den;

        /// <summary>
        /// 拍子の分子部分
        /// </summary>
        public double Measure_mol;

        /// <summary>
        /// 小節内のノーツ情報
        /// </summary>
        public List<Note> Notes;

        /// <summary>
        /// 1小節の1行を参照
        /// </summary>
        /// <param name="Content"></param>
        public Measure(string Content, double den, double mol) {
            Notes = new List<Note>();
            Measure_den = den;
            Measure_mol = mol;
            foreach (var note in Content) {
                Notes.Add(new Note(int.Parse(note.ToString())));
            }
        }

        //    /// <summary>
        //    /// 1小節あたりの時間を計算
        //    /// </summary>
        //    /// <param name="MeasureString"></param>
        //    /// <returns></returns>
        //    public double CalcMeasureTime(List<string> MeasureString, double BPM, double den, double mol) {
        //        // 現在のノーツ数
        //        int notecount = 0;

        //        // 経過時間
        //        double elapsed = 0;

        //        foreach(var line in MeasureString) {
        //            if (line.StartsWith("#MEASURE")) {
        //                continue;
        //            }
        //            if (line.StartsWith("#BPMCHANGE")) {
        //                if(MeasureRow.Length != 0) {

        //                }
        //                NowBPM = double.Parse(UmeboshiString.CutToEnd(line, "#BPMCHNGE"));
        //                continue;

        //            }
        //            if (line.StartsWith("#DELAY")) {
        //                elapsed += double.Parse(UmeboshiString.CutToEnd(line, "#DELAY"));
        //                continue;

        //            }
        //            if (line.StartsWith("#SCROLL")) {
        //                continue;
        //            }

        //            if (line.EndsWith(",")) {
        //                // 1行で書かれた1小節
        //                if(notecount == 0) {
        //                    elapsed = 60 / BPM * 4 * (mol / den);
        //                }
        //            }
        //        }

        //        return elapsed
        //    }

        //}

    }

    public enum DIFFICULTY {
        EASY,
        NORMAL,
        HARD,
        ONI,
        EDIT
    }
}
