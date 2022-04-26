using BF1.ServerAdminTools.Wpf.Data;

namespace BF1.ServerAdminTools.Common.Data;

public static class DataSave
{
    /// <summary>
    /// 加载的规则列表
    /// </summary>
    public static Dictionary<string, ServerRuleObj> Rules { get; } = new();
    /// <summary>
    /// 订阅的黑名单
    /// </summary>
    public static SubscribeConfigObj Subscribes { get; set; }
    /// <summary>
    /// 订阅黑名单的缓存
    /// </summary>
    public static SubscribeCacheObj SubscribeCache { get; set; }
    /// <summary>
    /// 目前应用的规则
    /// </summary>
    public static ServerRuleObj NowRule { get; set; }

    /// <summary>
    /// 是否自动踢出违规玩家
    /// </summary>
    public static bool AutoKickBreakPlayer = false;

    /// <summary>
    /// 是否显示中文武器名称
    /// </summary>
    public static bool IsShowCHSWeaponName = true;

    public static WpfConfigObj Config { get; set; }
}
