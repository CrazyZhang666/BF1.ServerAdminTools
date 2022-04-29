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
        => ProcessUtils.IsAppRun();

    /// <summary>
    /// 保存错误日志
    /// </summary>
    /// <param name="data">日志内容</param>
    public static void WriteErrorLog(string data)
        => ConfigHelper.WriteErrorLog(data);

    /// <summary>
    /// 读取配置文件
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
    /// <summary>
    /// 保存配置文件
    /// </summary>
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

    /// <summary>
    /// info
    /// </summary>
    /// <param name="data"></param>
    public static void LogInfo(string data)
        => LoggerHelper.Info(data);

    /// <summary>
    /// error
    /// </summary>
    /// <param name="data"></param>
    public static void LogError(string data)
        => LoggerHelper.Error(data);

    /// <summary>
    /// error
    /// </summary>
    /// <param name="data"></param>
    /// <param name="e"></param>
    public static void LogError(string data, Exception e)
        => LoggerHelper.Error(data, e);

    /// <summary>
    /// 数据库初始化
    /// </summary>
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

    /// <summary>
    /// 数据库关闭
    /// </summary>
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
    /// <summary>
    /// 初始化内存钩子
    /// </summary>
    /// <returns></returns>
    public static bool HookInit()
    {
        try
        {
            LoggerHelper.Info($"正在初始化内存钩子");
            var res = MemoryHook.Initialize();
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
    /// <summary>
    /// 关闭内存钩子
    /// </summary>
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
    /// <summary>
    /// 获取进程ID
    /// </summary>
    /// <returns></returns>
    public static int HookGetProcessId()
        => MemoryHook.GetProcessId();
    /// <summary>
    /// 给聊天分配内存
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 获取聊天内存地址
    /// </summary>
    /// <returns></returns>
    public static long MsgGetAllocateMemoryAddress()
        => ChatMsg.GetAllocateMemoryAddress();
    /// <summary>
    /// 释放聊天内存
    /// </summary>
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
    /// <summary>
    /// 是否开启聊天框
    /// </summary>
    /// <returns></returns>
    public static bool MsgGetChatIsOpen()
        => ChatMsg.GetChatIsOpen();
    /// <summary>
    /// 获取聊天消息地址
    /// </summary>
    /// <returns></returns>
    public static long MsgChatMessagePointer()
        => ChatMsg.ChatMessagePointer();
    /// <summary>
    /// 给窗口按下按键
    /// </summary>
    /// <param name="key"></param>
    public static void KeyPress(WinVK key)
        => ChatHelper.KeyPress(key, ChatHelper.KeyPressDelay);
    /// <summary>
    /// 刷新DNS缓存
    /// </summary>
    public static void DnsFlushResolverCache()
        => WinAPI.DnsFlushResolverCache();
    /// <summary>
    /// 发送文本
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string SendText(string data)
        => ChatHelper.SendText2Bf1Game(data);
    /// <summary>
    /// 登录获取session
    /// </summary>
    /// <returns></returns>
    public static Task<string> Login()
        => LoginHelper.LoginSessionID();
    /// <summary>
    /// 设置窗口前置
    /// </summary>
    public static void SetForegroundWindow()
        => MemoryHook.SetForegroundWindow();
    /// <summary>
    /// 按下Tab键
    /// </summary>
    public static void KeyTab()
        => ChatHelper.KeyTab();
    /// <summary>
    /// 设置按下按键时间间隔
    /// </summary>
    /// <param name="data"></param>
    public static void SetKeyPressDelay(int data)
        => ChatHelper.KeyPressDelay = data;
    /// <summary>
    /// 初始化服务器信息
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// 初始化服务器详情
    /// </summary>
    /// <returns></returns>
    public static async Task<bool> InitServerDetailed()
    {
        var res = await GTAPI.GetServerDetailed(Globals.Config.GameId);
        if (res.IsSuccess)
        {
            Globals.ServerDetailed = res.Obj;
            Globals.ServerDetailed.currentMap = ChsUtils.ToSimplifiedChinese(Globals.ServerDetailed.currentMap);
        }

        return res.IsSuccess;
    }
    /// <summary>
    /// 给数据库添加数据
    /// </summary>
    /// <param name="sheetName"></param>
    /// <param name="info"></param>
    public static void AddLog2SQLite(DataShell sheetName, BreakRuleInfo info)
        => SQLiteHelper.AddLog2SQLite(sheetName, info);
    /// <summary>
    /// 给数据库添加数据
    /// </summary>
    /// <param name="info"></param>
    public static void AddLog2SQLite(ChangeTeamInfo info)
        => SQLiteHelper.AddLog2SQLite(info);
    /// <summary>
    /// 获取内存数据
    /// </summary>
    public static void Tick()
        => MemoryHook.Tick();
    /// <summary>
    /// 获取窗口指针
    /// </summary>
    /// <returns></returns>
    public static IntPtr GetWindowHandle()
        => MemoryHook.GetWindowHandle();
}
