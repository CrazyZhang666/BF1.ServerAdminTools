namespace BF1.ServerAdminTools.Wpf.Data;

public record SubscribeConfigObj
{
    /// <summary>
    /// 订阅地址列表
    /// </summary>
    public List<string> UrlList { get; set; }
}

public record SubscribeCacheObj
{
    /// <summary>
    /// 订阅缓存
    /// </summary>
    public List<SubscribeObj> Cache { get; set; }
}

public record SubscribeObj
{
    /// <summary>
    /// 刷新时间
    /// </summary>
    public string UpdateTime { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public string LastTime { get; set; }
    /// <summary>
    /// 网址
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// 黑名单玩家列表
    /// </summary>
    public List<PlayerItem> Players { get; set; }
}

public record PlayerItem
{
    /// <summary>
    /// 昵称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 玩家ID
    /// </summary>
    public long ID { get; set; }
    /// <summary>
    /// 理由
    /// </summary>
    public string Reason { get; set; }
}

public record HttpSubscribe
{
    /// <summary>
    /// 玩家列表
    /// </summary>
    public List<PlayerItem> Players { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public string Time { get; set; }
}

public record RespSubscribe
{
    public HttpSubscribe http;
    public SubscribeObj obj;
    public bool OK;
}