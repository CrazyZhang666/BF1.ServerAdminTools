namespace BF1.ServerAdminTools.Wpf.Tasks;

internal static class Tasks
{
    public static void Start()
    {
        TaskCheckLife.Start();
        TaskTick.Start();
        TaskCheckRule.Start();
        TaskKick.Start();
        TaskCheckState.Start();
        TaskMapRule.Start();
        TaskUpdatePlayerList.Start();
    }
}
