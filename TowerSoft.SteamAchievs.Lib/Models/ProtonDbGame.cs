namespace TowerSoft.SteamAchievs.Lib.Models {
    public class ProtonDbGame {
        public long ID { get; set; }

        public string BestReportedTier { get; set; }

        public string Confidence { get; set; }

        public double Score { get; set; }

        public string Tier { get; set; }

        public int Total { get; set; }

        public string TrendingTier { get; set; }
    }
}
