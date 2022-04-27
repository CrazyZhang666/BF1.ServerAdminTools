using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Views;

namespace BF1.ServerAdminTools.Wpf.Tasks;

internal class TaskTick
{
    private static object Lock = new object();
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

    private static void Run()
    {
        while (true)
        {
            if (!Globals.IsGameRun || !Globals.IsToolInit)
            {
                DataSave.AutoKickBreakPlayer = false;
                Thread.Sleep(1000);
                continue;
            }

            Core.Tick();
            TaskUpdatePlayerList.Semaphore.Release();
            LogView.Semaphore.Release();
            TaskCheckRule.Semaphore.Release();

            Thread.Sleep(100);

            while (Semaphore != 3)
            {
                Thread.Sleep(100);
            }
            Semaphore = 0;
        }
    }
}
