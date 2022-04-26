using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF1.ServerAdminTools.Wpf.Data;

public record SubscribeObj
{
    public string Name { get; set; }
    public DateTime UpdateTime { get; set; }
    public DateTime LastTime { get; set; }
    public string Url { get; set; }
    public List<PlayerItem> Players { get; set; }
}

public record PlayerItem
{ 
    public string Name { get; set; }
    public long ID { get; set; }
    public string Reason { get; set; }
}