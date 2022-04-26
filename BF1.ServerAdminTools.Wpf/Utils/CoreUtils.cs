using Microsoft.Web.WebView2.Core;

namespace BF1.ServerAdminTools.Common.Utils;

public static class CoreUtils
{
    /// <summary>
    /// 主窗口标题
    /// </summary>
    public const string MainAppWindowName = "战地1服务器管理工具 v";

    public const string WebSite_Address = "https://github.com/CrazyZhang666/BF1.ServerAdminTools/tree/preview";

    public const string Config_Address = "http://175.178.108.122:8082/WebAPI/bf1";

    public static string Notice_Address = "http://175.178.108.122:8082/WebAPI/bf1/notice";
    public static string Change_Address = "http://175.178.108.122:8082/WebAPI/bf1/change";

    public static string Update_Address = "http://175.178.108.122:8082/BF1.ServerAdminTools.Wpf.exe";

    /// <summary>
    /// 正在更新时的文件名
    /// </summary>
    public const string HalfwayAppName = "未下载完的服管工具更新文件.exe";

    /// <summary>
    /// 程序服务端版本号，如：1.2.3.4
    /// </summary>
    public static Version ServerVersionInfo = Version.Parse("0.0.0.0");

    /// <summary>
    /// 程序客户端版本号，如：1.2.3.4
    /// </summary>
    public static Version ClientVersionInfo = Application.ResourceAssembly.GetName().Version;

    /// <summary>
    /// 程序客户端最后编译时间
    /// </summary>
    public static string ClientBuildTime = File.GetLastWriteTime(Process.GetCurrentProcess().MainModule.FileName).ToString();

    /// <summary>
    /// 计算时间差，即软件运行时间
    /// </summary>
    public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
    {
        TimeSpan ts1 = new(dateBegin.Ticks);
        TimeSpan ts2 = new(dateEnd.Ticks);

        return ts1.Subtract(ts2).Duration().ToString("c").Substring(0, 8);
    }

    /// <summary>
    /// 更新完成后的文件名
    /// </summary>
    /// <returns></returns>
    public static string FinalAppName()
    {
        return MainAppWindowName + ServerVersionInfo + ".exe";
    }

    /// <summary>
    /// 执行CMD指令
    /// </summary>
    public static void CMD_Code(string cmd)
    {
        Process CmdProcess = new Process();
        CmdProcess.StartInfo.FileName = "cmd.exe";
        CmdProcess.StartInfo.CreateNoWindow = true;                     // 不创建新窗口
        CmdProcess.StartInfo.UseShellExecute = false;                   // 不启用shell启动进程  
        CmdProcess.StartInfo.RedirectStandardInput = true;              // 重定向输入    
        CmdProcess.StartInfo.RedirectStandardOutput = true;             // 重定向标准输出    
        CmdProcess.StartInfo.RedirectStandardError = true;              // 重定向错误输出  
        CmdProcess.StartInfo.Arguments = "/c " + cmd;                   // "/C" 表示执行完命令后马上退出  
        CmdProcess.Start();                                             // 执行   
        CmdProcess.WaitForExit();                                       // 等待程序执行完退出进程  
        CmdProcess.Close();                                             // 结束  
    }

    /// <summary>
    /// 是否安装了WebView2依赖，安装了返回true，否则返回false
    /// </summary>
    /// <returns></returns>
    public static bool IsWebView2DependencyInstalled()
    {
        try
        {
            return !string.IsNullOrEmpty(CoreWebView2Environment.GetAvailableBrowserVersionString());
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 返回两个时间差秒数
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    public static double DiffSeconds(DateTime startTime, DateTime endTime)
    {
        TimeSpan secondSpan = new TimeSpan(endTime.Ticks - startTime.Ticks);
        return secondSpan.TotalSeconds;
    }
}
