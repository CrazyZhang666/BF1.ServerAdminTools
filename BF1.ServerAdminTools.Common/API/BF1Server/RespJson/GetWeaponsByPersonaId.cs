namespace BF1.ServerAdminTools.Common.API.BF1Server.RespJson;

public record GetWeaponsByPersonaId
{
    public string jsonrpc { get; set; }
    public string id { get; set; }
    public List<Result> result { get; set; }

    public record Result
    {
        public string name { get; set; }
        public List<Weapon> weapons { get; set; }
        public record Weapon
        {
            public string guid { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string category { get; set; }
            public string imageUrl { get; set; }
            public List<object> accessories { get; set; }
            public object star { get; set; }
            public Stats stats { get; set; }
            public record Stats
            {
                public Values values { get; set; }
                public record Values
                {
                    public float kills { get; set; }
                    public float headshots { get; set; }
                    public float accuracy { get; set; }
                    public float seconds { get; set; }
                    public float hits { get; set; }
                    public float shots { get; set; }
                }
            }
        }
    }
}
