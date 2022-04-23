using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF1.ServerAdminTools.Common.API.BF1Server.RespJson;

public class GetVehiclesByPersonaId
{
    public string jsonrpc { get; set; }
    public string id { get; set; }
    public List<Result> result { get; set; }

    public record Result
    {
        public string name { get; set; }
        public object star { get; set; }
        public List<Vehicle> vehicles { get; set; }
        public record Vehicle
        {
            public string guid { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string imageUrl { get; set; }
            public List<object> accessories { get; set; }
            public Stats stats { get; set; }
            public record Stats
            {
                public Values values { get; set; }
                public record Values
                {
                    public float seconds { get; set; }
                    public float kills { get; set; }
                    public float destroyed { get; set; }
                }
            }
        }
    }
}
