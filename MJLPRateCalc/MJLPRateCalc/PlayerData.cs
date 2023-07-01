using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJLPRateCalc {

    public class PlayerData {

        /// <summary>
        /// プレイヤー名
        /// </summary>
        public string Name;

        /// <summary>
        /// レーティング
        /// </summary>
        public double Rating;

        /// <summary>
        /// 各記録
        /// </summary>
        public List<Record> Records;

        public PlayerData(string name, double rating, List<Record> records) {
            Name = name;
            Rating = rating;
            Records = records;
        }

        public PlayerData() {
            Records = new List<Record>();
        }

    }
}
