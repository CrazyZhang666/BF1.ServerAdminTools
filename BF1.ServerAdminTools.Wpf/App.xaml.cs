using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Utils;

namespace BF1.ServerAdminTools.Wpf;

/// <summary>
/// App.xaml 的交互逻辑
/// </summary>
public partial class App : Application, IMsgCall
{
    public static Mutex AppMainMutex;
    protected override void OnStartup(StartupEventArgs e)
    {
        //初始化内核
        Core.Init(this);
        AppMainMutex = new Mutex(true, ResourceAssembly.GetName().Name, out var createdNew);

        if (createdNew)
        {
            if (!Core.IsGameRun())
            {
                MessageBox.Show("未检测到《战地1》游戏启动，工具功能不可用", " 警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            RegisterEvents();

            base.OnStartup(e);
        }
        else
        {
            MessageBox.Show("请不要重复打开，程序已经运行\n如果一直提示，请到\"任务管理器-详细信息（win7为进程）\"里结束本程序",
                "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            Current.Shutdown();
        }
    }

    private void RegisterEvents()
    {
        // UI线程未捕获异常处理事件（UI主线程）
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;

        // 非UI线程未捕获异常处理事件（例如自己创建的一个子线程）
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Task线程内未捕获异常处理事件
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        string str = GetExceptionMsg(e.Exception, e.ToString());
        Core.WriteErrorLog(str);
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        string str = GetExceptionMsg(e.ExceptionObject as Exception, e.ToString());
        Core.WriteErrorLog(str);
    }

    private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        string str = GetExceptionMsg(e.Exception, e.ToString());
        Core.WriteErrorLog(str);
    }

    /// <summary>
    /// 生成自定义异常消息
    /// </summary>
    /// <param name="ex">异常对象</param>
    /// <param name="backStr">备用异常消息：当ex为null时有效</param>
    /// <returns>异常字符串文本</returns>
    private static string GetExceptionMsg(Exception ex, string backStr)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("【出现时间】：" + DateTime.Now.ToString());
        if (ex != null)
        {
            sb.AppendLine("【异常类型】：" + ex.GetType().Name);
            sb.AppendLine("【异常信息】：" + ex.Message);
            sb.AppendLine("【堆栈调用】：\n" + ex.StackTrace);
        }
        else
        {
            sb.AppendLine("【未处理异常】：" + backStr);
        }
        return sb.ToString();
    }

    public void Info(string data)
    {
        MsgBoxUtils.InformationMsgBox(data);
    }

    public void Error(string data, Exception e)
    {
        MsgBoxUtils.ErrorMsgBox(data, e);
    }
}
