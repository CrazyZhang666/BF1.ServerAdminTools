using BF1.ServerAdminTools.Common.API;
using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.API.BF1Server.RespJson;
using BF1.ServerAdminTools.Common.API.GT;
using BF1.ServerAdminTools.Common.Chat;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Helper;
using BF1.ServerAdminTools.Common.Hook;
using BF1.ServerAdminTools.Common.Utils;

namespace BF1.ServerAdminTools.Common;

public interface IMsgCall
{
    /// <summary>
    /// 普通日志
    /// </summary>
    /// <param name="data">内容</param>
    public void Info(string data);
    /// <summary>
    /// 错误日志
    /// </summary>
    /// <param name="data">内容</param>
    /// <param name="e">错误内容</param>
    public void Error(string data, Exception e);
}

public static class Core
{
    public static IMsgCall Msg;

    /// <summary>
    /// 初始化日志窗口
    /// </summary>
    /// <param name="call">对象</param>
    public static void Init(IMsgCall call)
    {
        Msg = call;
        Globals.LocalPlayer.BaseAddress = 0;
        Globals.LocalPlayer.TeamID = 0;
        Globals.LocalPlayer.Spectator = 0;
        Globals.LocalPlayer.PersonaId = 0;
        Globals.LocalPlayer.PlayerName = "";

        Globals.ServerHook.Offset0 = 0;
        Globals.ServerHook.ServerName = "";
        Globals.ServerHook.ServerID = 0;
        Globals.ServerHook.ServerTime = 0f;
        Globals.ServerHook.ServerTimeS = "";
        Globals.ServerHook.Team1Score = 0;
        Globals.ServerHook.Team2Score = 0;
        Globals.ServerHook.Team1FromeKill = 0;
        Globals.ServerHook.Team2FromeKill = 0;
        Globals.ServerHook.Team1FromeFlag = 0;
        Globals.ServerHook.Team2FromeFlag = 0;

        Globals.StatisticData_Team1.MaxPlayerCount = 0;
        Globals.StatisticData_Team1.PlayerCount = 0;
        Globals.StatisticData_Team1.Rank150PlayerCount = 0;
        Globals.StatisticData_Team1.AllKillCount = 0;
        Globals.StatisticData_Team1.AllDeadCount = 0;

        Globals.StatisticData_Team2.MaxPlayerCount = 0;
        Globals.StatisticData_Team2.PlayerCount = 0;
        Globals.StatisticData_Team2.Rank150PlayerCount = 0;
        Globals.StatisticData_Team2.AllKillCount = 0;
        Globals.StatisticData_Team2.AllDeadCount = 0;
    }

    /// <summary>
    /// 检测游戏是否在运行
    /// </summary>
    /// <returns>运行结果</returns>
    public static bool IsGameRun()
    {
        return ProcessUtil.IsAppRun(Globals.TargetAppName);
    }

    /// <summary>
    /// 保存错误日志
    /// </summary>
    /// <param name="data">日志内容</param>
    public static void WriteErrorLog(string data)
        => ConfigHelper.WriteErrorLog(data);

    /// <summary>
    /// 初始化配置文件
    /// </summary>
    public static void ConfigInit()
    {
        try
        {
            LoggerHelper.Info($"正在读取配置文件");
            ConfigHelper.LoadConfig();
            LoggerHelper.Info($"配置文件读取完成");
        }
        catch (Exception e)
        {
            LoggerHelper.Error($"配置文件读取失败", e);
            Msg.Error("配置文件读取失败", e);
        }
    }

    public static void SaveConfig()
    {
        try
        {
            LoggerHelper.Info($"正在保存配置文件");
            ConfigHelper.SaveConfig();
            LoggerHelper.Info($"配置文件保存完成");
        }
        catch (Exception e)
        {
            LoggerHelper.Error($"配置文件保存失败", e);
            Msg.Error("配置文件保存失败", e);
        }
    }

    public static void LogInfo(string data)
        => LoggerHelper.Info(data);

    public static void LogError(string data)
        => LoggerHelper.Error(data);

    public static void LogError(string data, Exception e)
        => LoggerHelper.Error(data, e);

    public static void SQLInit()
    {
        try
        {
            LoggerHelper.Info($"SQLite数据库正在初始化");
            SQLiteHelper.Initialize();
            LoggerHelper.Info($"SQLite数据库初始化成功");
        }
        catch (Exception e)
        {
            LoggerHelper.Error($"SQLite数据库初始化失败", e);
            Msg.Error("SQLite数据库初始化失败", e);
        }
    }

    public static void SQLClose()
    {
        try
        {
            LoggerHelper.Info($"SQLite数据库正在关闭");
            SQLiteHelper.CloseConnection();
            LoggerHelper.Info($"SQLite数据库关闭成功");
        }
        catch (Exception e)
        {
            LoggerHelper.Error($"SQLite数据库关闭失败", e);
            Msg.Error("SQLite数据库关闭失败", e);
        }
    }

    public static bool HookInit()
    {
        try
        {
            LoggerHelper.Info($"正在初始化内存钩子");
            var res = MemoryHook.Initialize(Globals.TargetAppName);
            if (res)
            {
                LoggerHelper.Info($"初始化内存钩子成功");
            }
            else
            {
                LoggerHelper.Info($"初始化内存钩子失败");
            }
            return res;
        }
        catch (Exception e)
        {
            LoggerHelper.Error($"初始化内存钩子错误", e);
            Msg.Error("初始化内存钩子错误", e);
        }
        return false;
    }
    public static void HookClose()
    {
        try
        {
            LoggerHelper.Info($"正在释放内存钩子");
            MemoryHook.CloseHandle();
            LoggerHelper.Info($"释放内存钩子失败");
        }
        catch (Exception e)
        {
            LoggerHelper.Error($"释放内存钩子错误", e);
            Msg.Error("释放内存钩子错误", e);
        }
    }
    public static int HookGetProcessId()
        => MemoryHook.GetProcessId();
    public static bool MsgAllocateMemory()
    {
        try
        {
            LoggerHelper.Info($"中文聊天指针正在初始化");
            var res = ChatMsg.AllocateMemory();
            LoggerHelper.Info($"中文聊天指针初始化成功");
            return res;
        }
        catch (Exception e)
        {
            LoggerHelper.Error($"中文聊天指针初始化失败", e);
            Msg.Error("中文聊天指针初始化失败", e);
        }
        return false;
    }
    public static long MsgGetAllocateMemoryAddress()
        => ChatMsg.GetAllocateMemoryAddress();
    public static void MsgFreeMemory()
    {
        try
        {
            LoggerHelper.Info($"正在释放中文聊天指针内存");
            ChatMsg.FreeMemory();
            LoggerHelper.Info($"释放中文聊天指针内存成功");
        }
        catch (Exception e)
        {
            LoggerHelper.Error($"释放中文聊天指针内存成功失败", e);
            Msg.Error("释放中文聊天指针内存成功失败", e);
        }
    }
    public static bool MsgGetChatIsOpen()
        => ChatMsg.GetChatIsOpen();
    public static long MsgChatMessagePointer()
        => ChatMsg.ChatMessagePointer();
    public static void KeyPress(WinVK key)
        => ChatHelper.KeyPress(key, ChatHelper.KeyPressDelay);
    public static void DnsFlushResolverCache()
        => WinAPI.DnsFlushResolverCache();
    public static string SendText(string data)
        => ChatHelper.SendText2Bf1Game(data);

    public static Task<string> Login()
        => LoginHelper.LoginSessionID();

    public static void SetForegroundWindow()
        => MemoryHook.SetForegroundWindow();

    public static void KeyTab()
        => ChatHelper.KeyTab();

    public static void SetKeyPressDelay(int data)
        => ChatHelper.KeyPressDelay = data;

    public static async Task<RespContent<FullServerDetails>> InitServerInfo()
    {
        if (string.IsNullOrEmpty(Globals.Config.GameId) ||
            string.IsNullOrEmpty(Globals.Config.SessionId))
            return null;
        await ServerAPI.SetAPILocale();
        var result = await ServerAPI.GetFullServerDetails();

        if (result.IsSuccess)
        {
            var server = result.Obj;

            Globals.ServerInfo = server.result.serverInfo;
            Globals.RspInfo = server.result.rspInfo;

            Globals.Config.ServerId = server.result.rspInfo.server.serverId;
            Globals.Config.PersistedGameId = server.result.rspInfo.server.persistedGameId;

            Globals.Server_AdminList.Add(long.Parse(server.result.rspInfo.owner.personaId));
            Globals.Server_Admin2List.Add(server.result.rspInfo.owner.displayName);

            foreach (var item in server.result.rspInfo.adminList)
            {
                Globals.Server_AdminList.Add(long.Parse(item.personaId));
                Globals.Server_Admin2List.Add(item.displayName);
            }

            foreach (var item in server.result.rspInfo.vipList)
            {
                Globals.Server_VIPList.Add(long.Parse(item.personaId));
            }
        }

        return result;
    }

    public static async Task<bool> InitServerDetailed() 
    {
        var res = await GTAPI.GetServerDetailed(Globals.Config.GameId);
        if (res.IsSuccess)
        {
            Globals.ServerDetailed = res.Obj;
            Globals.ServerDetailed.currentMap = ChsUtil.ToSimplifiedChinese(Globals.ServerDetailed.currentMap);
        }

        return res.IsSuccess;
    }

    public static void AddLog2SQLite(DataShell sheetName, BreakRuleInfo info)
        => SQLiteHelper.AddLog2SQLite(sheetName, info);

    public static void AddLog2SQLite(ChangeTeamInfo info)
        => SQLiteHelper.AddLog2SQLite(info);

    public static void Tick()
        => MemoryHook.Tick();

    public static IntPtr GetWindowHandle()
        => MemoryHook.GetWindowHandle();
}
