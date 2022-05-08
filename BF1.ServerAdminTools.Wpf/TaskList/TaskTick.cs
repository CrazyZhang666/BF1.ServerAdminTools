using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Wpf.Data;

namespace BF1.ServerAdminTools.Wpf.TaskList;

internal class TaskTick
{
    private static object Lock = new();
    private static int Semaphore = 0;
    public static void Start()
    {
        new Thread(Run)
        {
            Name = "TaskCoreTick",
            IsBackground = true
        }.Start();
    }

    public static void Done()
    {
        lock (Lock)
        {
            Semaphore++;
        }
    }

    /// <summary>
    /// 数据更新
    /// </summary>
    private static void Run()
    {
        while (Tasks.IsRun)
        {
            if (!Globals.IsGameRun || !Globals.IsToolInit)
            {
                DataSave.AutoKickBreakPlayer = false;
                Thread.Sleep(1000);
                continue;
            }

            Core.Tick();
            TaskUpdatePlayerList.Semaphore.Release();
            TaskCheckPlayerChangeTeam.Semaphore.Release();
            TaskCheckRule.Semaphore.Release();
            TaskCheckNumber.Semaphore.Release();

            Thread.Sleep(100);

            while (Semaphore < 4)
            {
                Thread.Sleep(100);
            }
            Semaphore = 0;
        }
    }
}
