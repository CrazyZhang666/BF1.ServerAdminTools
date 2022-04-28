namespace BF1.ServerAdminTools.Common.Utils;

public static class ProcessUtils
{
    private static int Count;
    /// <summary>
    /// 判断程序是否运行
    /// </summary>
    /// <param name="appName">程序名称</param>
    /// <returns>正在运行返回true，未运行返回false</returns>
    public static bool IsAppRun(string appName)
    {
        int temp = Process.GetProcessesByName(appName).ToList().Count;
        if (temp > Count)
        {
            Count = temp;
        }
        return Globals.IsGameRun = temp == Count && temp > 0;
    }

    /// <summary>
    /// 打开指定链接或程序
    /// </summary>
    /// <param name="link"></param>
    public static void OpenLink(string link)
    {
        Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
    }

    /// <summary>
    /// 打开指定链接或程序
    /// </summary>
    /// <param name="link"></param>
    public static void OpenLink(string link, string path)
    {
        Process.Start(new ProcessStartInfo(link, path) { UseShellExecute = true });
    }

    /// <summary>
    /// 根据名字关闭指定程序
    /// </summary>
    /// <param name="processName">程序名字，不需要加.exe</param>
    public static void CloseProcess(string processName)
    {
        Process[] appProcess = Process.GetProcesses();
        foreach (Process targetPro in appProcess)
        {
            if (targetPro.ProcessName.Equals(processName))
                targetPro.Kill();
        }
    }
}
