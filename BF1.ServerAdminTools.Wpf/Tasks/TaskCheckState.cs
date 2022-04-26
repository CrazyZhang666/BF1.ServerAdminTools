using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF1.ServerAdminTools.Wpf.Tasks;

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
    private static void Run()
    {
        while (true)
        {
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
