using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Views;

namespace BF1.ServerAdminTools.Wpf.Tasks;

internal class TaskMapRule
{
    public static bool NeedPause = true;
    public static void Start()
    {
        new Thread(Run)
        {
            Name = "TaskMapRule",
            IsBackground = true
        }.Start();
    }

    private static void Run()
    {
        while (true)
        {
            Thread.Sleep(1000);
            if (!Globals.IsGameRun || !Globals.IsToolInit)
                continue;

            if (Globals.ServerInfo == null)
                continue;

            if (DataSave.Config.MapRule.TryGetValue(Globals.ServerInfo.mapNamePretty, out var item))
            {
                if (DataSave.NowRule.Name.ToLower() == item)
                    continue;

                if (!DataSave.Rules.ContainsKey(item))
                    continue;

                TaskCheckRule.NeedPause = true;

                if (DataSave.Rules.TryGetValue(item, out var item1))
                {
                    DataSave.NowRule = item1;
                    RuleView.UpdateRule?.Invoke();
                }

                TaskCheckRule.NeedPause = false;
            }
        }
    }
}
