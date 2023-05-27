using Newtonsoft.Json;

namespace TowerSoft.SteamAchievs.Lib.Models {
    public class HltbModel {
        [JsonProperty("game_id")]
        public int ID { get; set; }

        [JsonProperty("game_name")]
        public string Name { get; set; }

        [JsonProperty("release_world")]
        public int ReleaseYear { get; set; }

        [JsonProperty("comp_main")]
        public int? MainTime { get; set; }

        [JsonProperty("comp_plus")]
        public int? MainPlusTime { get; set; }

        [JsonProperty("comp_100")]
        public int? CompletionTime { get; set; }

        [JsonProperty("comp_all")]
        public int? AllStylesTime { get; set; }


        public override string ToString() {
            return $"{Name} - Main: {MainTime}, Plus: {MainPlusTime}, Complete: {CompletionTime}, AllStyles: {AllStylesTime}";
        }
    }
}
