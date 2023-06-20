using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerSoft.SteamAchievs.Lib.Models {
    public class SearchModel {
        public string? Name { get; set; }

        public int? PlayNextScore { get; set; }

        public SearchComparisonType PlayNextScoreComparison { get; set; }

        [Range(0, 100)]
        public int? MetacriticScore { get; set; }

        public SearchComparisonType MetacriticScoreComparison { get; set; }

        public double? MainTime { get; set; }

        public SearchComparisonType MainTimeComparison { get; set; }

        public double? MainAndSidesTime { get; set; }

        public SearchComparisonType MainAndSidesTimeComparison { get; set; }

        public double? CompletionistTime { get; set; }

        public SearchComparisonType CompletionistTimeComparison { get; set; }

        public double? AllStylesTime { get; set; }

        public SearchComparisonType AllStylesTimeComparison { get; set; }
    }
}
