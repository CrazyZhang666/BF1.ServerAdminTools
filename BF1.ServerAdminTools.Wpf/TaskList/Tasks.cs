namespace BF1.ServerAdminTools.Wpf.TaskList;

internal static class Tasks
{
    public static bool IsRun { get; private set; }
    public static void Start()
    {
        IsRun = true;
        TasCheckPlayerLifeData.Start();
        TaskTick.Start();
        TaskCheckRule.Start();
        TaskKick.Start();
        TaskCheckState.Start();
        TaskMapRule.Start();
        TaskUpdatePlayerList.Start();
        TaskCheckPlayerChangeTeam.Start();
        TaskUpdateState.Start();
        TaskSendChar.Start();
        TaskCheckNumber.Start();
    }

    public static void Stop()
    {
        IsRun = false;
        TaskCheckRule.Semaphore.Release();
        TaskUpdatePlayerList.Semaphore.Release();
        TaskCheckPlayerChangeTeam.Semaphore.Release();
        TaskCheckNumber.Semaphore.Release();
        TaskTick.Done();
        TaskTick.Done();
        TaskTick.Done();
        TaskTick.Done();
    }
}
