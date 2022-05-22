using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Helper;
using BF1.ServerAdminTools.GameImage;
using BF1.ServerAdminTools.Netty;
using BF1.ServerAdminTools.Wpf.Utils;
using DotNetty.Buffers;
using System.Drawing;

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
        NettyCore.SendTopCall(NettyCall);
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

    private IByteBuffer NettyCall(IByteBuffer buff)
    {
        byte index = buff.ReadByte();
        IByteBuffer res = Unpooled.Buffer();
        res.WriteByte(127);
        switch (index)
        {
            //获取游戏截图
            case 0:
                Bitmap map = GameWindowImg.GetWindow();
                string local = $"{ConfigLocal.Base}/image.png";
                map.Save(local);
                res.WriteByte(0).WriteString(local);
                break;
            default:
                res.WriteByte(0xFF);
                break;
        }

        return res;
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
        Core.WriteExceptionMsg(e.Exception, e.ToString());
    }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Core.WriteExceptionMsg(e.ExceptionObject as Exception, e.ToString());
    }

    private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        Core.WriteExceptionMsg(e.Exception, e.ToString());
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
