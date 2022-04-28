using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Views;

namespace BF1.ServerAdminTools.Wpf.Tasks;

internal static class TaskCheckPlayerChangeTeam
{
    public static Semaphore Semaphore = new(0, 5);

    private static Dictionary<long, PlayerData> Player_Team1 = new();
    private static Dictionary<long, PlayerData> Player_Team2 = new();

    private static Dictionary<long, PlayerData> New_Player_Team1 = new();
    private static Dictionary<long, PlayerData> New_Player_Team2 = new();

    public static void Start()
    {
        new Thread(Run)
        {
            Name = "TaskCheckPlayerChangeTeam",
            IsBackground = true
        }.Start();
    }
    /// <summary>
    /// 检查玩家换边
    /// </summary>
    private static void Run()
    {
        while (Tasks.IsRun)
        {
            Semaphore.WaitOne();
            if (!Tasks.IsRun)
                return;
            CheckPlayerChangeTeam();
            TaskTick.Done();
        }
    }

    private static void CheckPlayerChangeTeam()
    {
        if (string.IsNullOrEmpty(Globals.Config.GameId))
            return;

        if (Globals.PlayerDatas_Team1.Count == 0 && Globals.PlayerDatas_Team2.Count == 0)
        {
            New_Player_Team1.Clear();
            New_Player_Team2.Clear();
            Player_Team1.Clear();
            Player_Team2.Clear();
            return;
        }

        // 第一次初始化
        if (Player_Team1.Count == 0 && Player_Team2.Count == 0)
        {
            foreach (var item in Globals.PlayerDatas_Team1)
            {
                Player_Team1.Add(item.Key, item.Value);
            }
            foreach (var item in Globals.PlayerDatas_Team2)
            {
                Player_Team2.Add(item.Key, item.Value);
            }
            return;
        }

        New_Player_Team1.Clear();
        New_Player_Team2.Clear();
        // 更新保存的数据
        foreach (var item in Globals.PlayerDatas_Team1)
        {
            New_Player_Team1.Add(item.Key, item.Value);
        }
        foreach (var item in Globals.PlayerDatas_Team2)
        {
            New_Player_Team2.Add(item.Key, item.Value);
        }

        // 变量保存的队伍1玩家列表
        foreach (var item in New_Player_Team1)
        {
            if (Player_Team2.ContainsKey(item.Key))
            {
                LogView.AddChangeTeamLog?.Invoke(new ChangeTeamInfo()
                {
                    Rank = item.Value.Rank,
                    Name = item.Value.Name,
                    PersonaId = item.Value.PersonaId,
                    Status = "从 队伍2 更换到 队伍1"
                });
            }
        }

        // 变量保存的队伍2玩家列表
        foreach (var item in New_Player_Team2)
        {
            if (Player_Team1.ContainsKey(item.Key))
            {
                LogView.AddChangeTeamLog?.Invoke(new ChangeTeamInfo()
                {
                    Rank = item.Value.Rank,
                    Name = item.Value.Name,
                    PersonaId = item.Value.PersonaId,
                    Status = "从 队伍1 更换到 队伍2"
                });
            }
        }

        Player_Team1.Clear();
        Player_Team2.Clear();
        // 更新保存的数据
        foreach (var item in New_Player_Team1)
        {
            Player_Team1.Add(item.Key, item.Value);
        }
        foreach (var item in New_Player_Team2)
        {
            Player_Team2.Add(item.Key, item.Value);
        }
    }
}
