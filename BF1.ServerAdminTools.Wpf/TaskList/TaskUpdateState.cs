using BF1.ServerAdminTools.Wpf.Utils;

namespace BF1.ServerAdminTools.Wpf.TaskList;

internal static class TaskUpdateState
{
    private static DateTime StartTime;
    public static void Start()
    {
        StartTime = DateTime.Now;

        new Thread(Run)
        {
            Name = "TaskUpdateState",
            IsBackground = true
        }.Start();
    }

    /// <summary>
    /// 运行时间刷新
    /// </summary>
    private static void Run()
    {
        while (Tasks.IsRun)
        {
            // 获取软件运行时间
            MainWindow.MainModel.AppRunTime = "运行时间 : " + CoreUtils.ExecDateDiff(StartTime, DateTime.Now);

            Thread.Sleep(1000);
        }
    }
}
