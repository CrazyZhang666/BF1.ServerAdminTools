using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Utils;
using BF1.ServerAdminTools.Wpf.Views;

namespace BF1.ServerAdminTools.Wpf.TaskList;

internal static class TaskCheckState
{
    public static void Start()
    {
        new Thread(Run)
        {
            Name = "TaskCheckState",
            IsBackground = true
        }.Start();
    }

    /// <summary>
    /// 软件状态更新
    /// </summary>
    private static void Run()
    {
        while (Tasks.IsRun)
        {
            if (Globals.IsGameRun)
            {
                if (!Core.IsGameRun())
                {
                    Globals.IsToolInit = false;
                    Globals.IsGameRun = false;
                    MsgBoxUtils.WarningMsgBox("游戏已退出，功能已关闭");
                }
            }

            if (string.IsNullOrEmpty(Globals.Config.GameId))
            {
                RuleView.CloseRunCheck?.Invoke();

                DataSave.AutoKickBreakPlayer = false;
            }

            if (!DataSave.AutoKickBreakPlayer)
            {
                RuleView.CloseRunCheck?.Invoke();
            }

            Thread.Sleep(1000);
        }
    }
}
