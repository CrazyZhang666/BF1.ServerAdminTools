using BF1.ServerAdminTools.Common.API.GT.RespJson;
using BF1.ServerAdminTools.Common.Data;
using static BF1.ServerAdminTools.Common.API.BF1Server.RespJson.FullServerDetails.Result;

namespace BF1.ServerAdminTools.Common;

public static class Globals
{
    /// <summary>
    /// 目标进程
    /// </summary>
    public const string TargetAppName = "bf1";    // 战地1
    /// <summary>
    /// 配置文件
    /// </summary>
    public static ConfigObj Config;
    /// <summary>
    /// 规则是否已经设置了
    /// </summary>
    public static bool IsRuleSetRight = false;

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 所有玩家列表
    /// </summary>
    public static Dictionary<long, PlayerData> PlayerList_All { get; } = new();

    /// <summary>
    /// 队伍1列表
    /// </summary>
    public static Dictionary<long, PlayerData> PlayerDatas_Team1 { get; } = new();
    /// <summary>
    /// 队伍2列表
    /// </summary>
    public static Dictionary<long, PlayerData> PlayerDatas_Team2 { get; } = new();
    /// <summary>
    /// 观战列表
    /// </summary>
    public static Dictionary<long, PlayerData> PlayerDatas_Team3 { get; } = new();

    /// <summary>
    /// 自己的数据
    /// </summary>
    public static ClientPlayer LocalPlayer;
    /// <summary>
    /// 对局数据
    /// </summary>
    public static ServerHook ServerHook;
    /// <summary>
    /// 服务器数据
    /// </summary>
    public static ServerInfo ServerInfo;
    /// <summary>
    /// 管理员等
    /// </summary>
    public static RspInfo RspInfo;
    /// <summary>
    /// 服务器详情
    /// </summary>
    public static ServerInfos.ServersItem ServerDetailed;
    /// <summary>
    /// 队伍1数据
    /// </summary>
    public static StatisticData StatisticData_Team1;
    /// <summary>
    /// 队伍2数据
    /// </summary>
    public static StatisticData StatisticData_Team2;

    /// <summary>
    /// 服务器管理员
    /// </summary>
    public static List<long> Server_AdminList { get; } = new();
    /// <summary>
    /// 服务器管理员
    /// </summary>
    public static List<string> Server_Admin2List { get; } = new();
    /// <summary>
    /// 服务器VIP
    /// </summary>
    public static List<long> Server_VIPList { get; } = new();

    /// <summary>
    /// 游戏是否在运行
    /// </summary>
    public static bool IsGameRun = false;

    /// <summary>
    /// 内存模块是否运行
    /// </summary>
    public static bool IsToolInit = false;
}
