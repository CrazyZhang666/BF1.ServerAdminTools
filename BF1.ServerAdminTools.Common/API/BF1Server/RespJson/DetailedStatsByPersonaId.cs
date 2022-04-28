namespace BF1.ServerAdminTools.Common.API.BF1Server.RespJson;

public record DetailedStatsByPersonaId
{
    public string jsonrpc { get; set; }
    public string id { get; set; }
    public Result result { get; set; }

    public record Result
    {
        public BasicStats basicStats { get; set; }
        public string favoriteClass { get; set; }
        public List<KitStats> kitStats { get; set; }
        public float awardScore { get; set; }
        public float bonusScore { get; set; }
        public float squadScore { get; set; }
        public int avengerKills { get; set; }
        public int saviorKills { get; set; }
        public int highestKillStreak { get; set; }
        public int dogtagsTaken { get; set; }
        public int roundsPlayed { get; set; }
        public int flagsCaptured { get; set; }
        public int flagsDefended { get; set; }
        public double accuracyRatio { get; set; }
        public int headShots { get; set; }
        public float longestHeadShot { get; set; }
        public float nemesisKills { get; set; }
        public float nemesisKillStreak { get; set; }
        public float revives { get; set; }
        public float heals { get; set; }
        public float repairs { get; set; }
        public float suppressionAssist { get; set; }
        public float kdr { get; set; }
        public float killAssists { get; set; }
        public List<GameModeStats> gameModeStats { get; set; }
        public List<VehicleStats> vehicleStats { get; set; }
        public string detailedStatType { get; set; }

        public record VehicleStats
        {
            public string name { get; set; }
            public string prettyName { get; set; }
            public float killsAs { get; set; }
            public float vehiclesDestroyed { get; set; }
            public float timeSpent { get; set; }
        }
        public record GameModeStats
        {
            public string name { get; set; }
            public string prettyName { get; set; }
            public int wins { get; set; }
            public int losses { get; set; }
            public float score { get; set; }
            public float winLossRatio { get; set; }
        }

        public record KitStats
        {
            public string name { get; set; }
            public string prettyName { get; set; }
            public object kitType { get; set; }
            public float score { get; set; }
            public float kills { get; set; }
            public float secondsAs { get; set; }
        }
        public record BasicStats
        {
            public int timePlayed { get; set; }
            public int wins { get; set; }
            public int losses { get; set; }
            public int kills { get; set; }
            public int deaths { get; set; }
            public float kpm { get; set; }
            public float spm { get; set; }
            public float skill { get; set; }
            public string soldierImageUrl { get; set; }
            public object rank { get; set; }
            public object rankProgress { get; set; }
            public object freemiumRank { get; set; }
            public List<object> completion { get; set; }
            public object highlights { get; set; }
            public object highlightsByType { get; set; }
            public object equippedDogtags { get; set; }
        }
    }
}
