using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF1.ServerAdminTools.Wpf.Data;

public record SubscribeConfigObj
{ 
    public List<string> UrlList { get; set; }
}

public record SubscribeCacheObj
{
    public List<SubscribeObj> Cache { get; set; }
}

public record SubscribeObj
{
    public string UpdateTime { get; set; }
    public string LastTime { get; set; }
    public string Url { get; set; }
    public List<PlayerItem> Players { get; set; }
}

public record PlayerItem
{ 
    public string Name { get; set; }
    public long ID { get; set; }
    public string Reason { get; set; }
}