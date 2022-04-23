using BF1.ServerAdminTools.Wpf.Data;

namespace BF1.ServerAdminTools.Common.Data;

public static class DataSave
{
    /// <summary>
    /// 加载的规则列表
    /// </summary>
    public static Dictionary<string, ServerRule> Rules { get; } = new();
    /// <summary>
    /// 目前应用的规则
    /// </summary>
    public static ServerRule NowRule { get; set; }

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
