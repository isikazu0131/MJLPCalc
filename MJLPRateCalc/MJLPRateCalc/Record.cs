using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MJLPRateCalc {
    public class Record {
        public string DATPath;
        public string Title;
        public double Level;
        public int DatScore;
        public double Score;
        public int Great;
        public int Good;
        public int Bad;
        public int JudgeLevel;
        public double Rating;
        public ClearMode Clear;
        public DateTime LastWrite;

        /// <summary>
        /// クリアフラグを返します
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static public ClearMode GetClearMode(Int16 value) {
            switch (value) {
                case 0: return ClearMode.NoPlay;
                case 1: return ClearMode.NoClear;
                case 2: return ClearMode.Clear;
                case 3: return ClearMode.HardClear;
                case 4: return ClearMode.FullCombo;
                default: return ClearMode.NoPlay;
            }
        }

        /// <summary>
        /// 記録ファイルを出力して保存します
        /// </summary>
        /// <param name="record"></param>
        static public void Write(Record record, string ScoreFilePath) {
            FileInfo scoreFileInfo = new FileInfo(ScoreFilePath);

            XmlSerializer RatingXML = new XmlSerializer(typeof(Record));
            if(!Directory.Exists(scoreFileInfo.DirectoryName)) Directory.CreateDirectory(scoreFileInfo.DirectoryName);
            using (StreamWriter sw = new StreamWriter(ScoreFilePath, false)) {
                RatingXML.Serialize(sw, record);
            }
        }

        static public Record Read(string ScoreFilePath) {
            Record record = new Record();
            if (!File.Exists(ScoreFilePath)) { return record; }

            FileInfo ScoreInfo = new FileInfo(ScoreFilePath);
            XmlSerializer RatingXML = new XmlSerializer(typeof(Record));

            using (StreamReader sr = new StreamReader(ScoreFilePath)) {
                record = (Record)RatingXML.Deserialize(sr);
            }
            if (record == null) return record;
            
            if (record.LastWrite == new DateTime(0)) record.LastWrite = ScoreInfo.LastWriteTime;
            // 難易度が更新されているかもしれないので単曲レートは毎回計算する
            var tjaPath = Path.ChangeExtension(record.DATPath, ".tja");
            if (!File.Exists(tjaPath)) {
                tjaPath = GetTJAPath(tjaPath);
                record.DATPath = Path.ChangeExtension(tjaPath, ".dat");
                Write(record, ScoreFilePath);
            }
            TJA tja = new TJA(new FileInfo(tjaPath));

            // スコア、レートの再計算
            record.Score = GetScore(record);
            record.Rating = CalcRate(record.Score, tja.LEVEL, record.JudgeLevel, record.Clear);
            if (tja.LEVEL != record.Level) {
                record.DATPath = Path.ChangeExtension(tjaPath, ".dat");
                record.Title = tja.TITLE;
                record.Level = tja.LEVEL;
                Write(record, ScoreFilePath);
            }
            return record;
        }

        /// <summary>
        /// 最新のTJAパスを取得します
        /// </summary>
        /// <returns></returns>
        static private string GetTJAPath(string tjaPath) {
            string targetDirPath;
            using (StreamReader streamReader = new StreamReader(@".\Info\Target.txt", Encoding.GetEncoding("Shift_jis"))) {
                targetDirPath = streamReader.ReadToEnd();
            }
            DirectoryInfo targetDInfo = new DirectoryInfo(targetDirPath);
            var tjas = TJA.GetTJAs(targetDInfo);
            FileInfo tja = new FileInfo(tjaPath);
            // ファイルは1つしかない

            string result = tjas.Where(x => x.TJAPath.Name == tja.Name).ToList()[0].TJAPath.FullName;
            return result;
        }

        // バージョンアップに伴うスコア修正
        static public double GetScore(Record record) {
            switch (record.Clear) {
                case ClearMode.Clear:
                case ClearMode.HardClear:
                case ClearMode.FullCombo:
                    return RoundDown(((double)record.Great + (double)record.Good * 0.5) / ((double)record.Great + (double)record.Good + (double)record.Bad) * 1000000, 0);
                default:
                    return record.Score;
            }
        }

        static public double CalcRate(double score, double level, int judgelevel, ClearMode clearMode) {
            double rate = 0;
            double bonus = 0;

            if (level == 0) return 0;
            if (score < 500000) {
                return 0;
            } else if (score < 700000) {

                rate = level * (score - 500000) / 200000;
            } else {
                rate = level + (score - 700000) / 100000;
            }

            bonus = GetBonus(score, level, clearMode);
            rate += bonus;

            rate *= (1 - 0.05 * judgelevel);
            var result = RoundDown(rate, 2);

            return result;
        }

        static private double GetBonus(double score, double level, ClearMode clearMode) {
            double bonus = 0;

            // フルコンボボーナス、全良ボーナスの倍率
            const double BonusBaseRate = 9.0;
            const double FCBonusOddsUnder = 0.6;
            const double FCBonusOddsUpper = 0.175;
            const double APBonusOddsUnder = 1.6;
            const double APBonusOddsUpper = 1.0;


            if (clearMode == ClearMode.FullCombo) {
                if (level < 3) bonus = 0;
                else if (level == BonusBaseRate) bonus = 0;
                else if (level < BonusBaseRate) bonus = (BonusBaseRate - level) * FCBonusOddsUnder;
                else if (level > BonusBaseRate) bonus = (level - BonusBaseRate) * FCBonusOddsUpper;
                //} else if (score == 1000000) {
                //    if (level < 3) bonus = 0;
                //    else if (level == BonusBaseRate) bonus = 0;
                //    else if (level < BonusBaseRate) bonus = (BonusBaseRate - level) * APBonusOddsUnder;
                //    else if (level > BonusBaseRate) bonus = (level - BonusBaseRate) * APBonusOddsUpper;
                //}
            }
            if (score == 1000000) {
                if (level < 3) bonus += 0;
                bonus += 0.3;
            }

            return bonus;
        }

        /// <summary>
        /// 小数点n位で切り捨てにします
        /// </summary>
        /// <param name="val"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static private double RoundDown(double val, int n) {
            var num = val * Math.Pow(10, n);
            num = Math.Floor(num);
            num /= Math.Pow(10, n);
            return num;
        }

    }

    public enum ClearMode {
        NoPlay,
        NoClear,
        Clear,
        HardClear,
        FullCombo
    }
}
