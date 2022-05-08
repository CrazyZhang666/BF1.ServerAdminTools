using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.GameImage;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Models;
using BF1.ServerAdminTools.Wpf.TaskList;
using BF1.ServerAdminTools.Wpf.Utils;
using BF1.ServerAdminTools.Wpf.Views;
using System.Windows.Media.Imaging;

namespace BF1.ServerAdminTools.Wpf;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// 主窗口全局提示信息委托
    /// </summary>
    public static Action<int, string> SetOperatingState;
    /// <summary>
    /// 主窗口选项卡控件选择委托
    /// </summary>
    public static Action<int> TabControlSelect;

    public delegate void ClosingDispose();
    public static event ClosingDispose ClosingDisposeEvent;

    public static MainModel MainModel { get; set; }
    public static MainWindow? ThisMainWindow;

    private BlurUtils blur;

    public MainWindow()
    {
        ThisMainWindow = this;
        InitializeComponent();

        // 提示信息委托
        SetOperatingState = FSetOperatingState;
        // TabControl 选择切换委托
        TabControlSelect = FTabControlSelect;

        MainModel = new MainModel();

        blur = new BlurUtils(this);
        BG();
    }

    public static void BG()
    {
        if (!string.IsNullOrWhiteSpace(DataSave.Config.Background) && File.Exists(DataSave.Config.Background))
        {
            var image = new ImageBrush(new BitmapImage(new(DataSave.Config.Background)))
            {
                Stretch = Stretch.UniformToFill,
                Opacity = DataSave.Config.WindowVacuity ? (double)DataSave.Config.BackgroudOpacity / 100 : 1
            };
            ThisMainWindow.Background = image;
        }
        else
        {
            if (DataSave.Config.WindowVacuity)
                ThisMainWindow.Background = Brushes.Transparent;
            else
                ThisMainWindow.Background = Brushes.White;
        }

        ThisMainWindow.blur.Composite(DataSave.Config.WindowVacuity);
    }

    private void Window_Main_Loaded(object sender, RoutedEventArgs e)
    {
        MainModel.AppRunTime = "运行时间 : Loading...";

        Title = CoreUtils.MainAppWindowName + CoreUtils.ClientVersionInfo + "- 最后编译时间 : " + File.GetLastWriteTime(Process.GetCurrentProcess().MainModule.FileName);

        Task.Run(InitThread);

        this.DataContext = this;

        if (Core.MsgAllocateMemory())
            Core.LogInfo($"中文聊天指针分配成功 0x{Core.MsgGetAllocateMemoryAddress():x}");
        else
            Core.LogError($"中文聊天指针分配失败");

        Tasks.Start();
    }

    private void Window_Main_Closing(object sender, CancelEventArgs e)
    {
        Tasks.Stop();
        GameWindow.Stop();
        // 关闭事件
        ClosingDisposeEvent();
        Core.LogInfo($"调用关闭事件成功");
        Core.SaveConfig();
        ConfigUtils.SaveAllRule();
        Core.SQLClose();
        Core.MsgFreeMemory();
        Core.HookClose();

        Core.LogInfo($"程序关闭\n\n");
        Application.Current.Shutdown();
    }

    private void InitThread()
    {
        // 调用刷新SessionID功能
        Core.LogInfo($"开始刷新SessionID");
        AuthView.AutoRefreshSID();
    }

    /// <summary>
    /// 提示信息，绿色信息1，灰色警告2，红色错误3
    /// </summary>
    /// <param name="index">绿色信息1，灰色警告2，红色错误3</param>
    /// <param name="str">消息内容</param>
    private void FSetOperatingState(int index, string str)
    {
        if (index == 1)
        {
            Border_OperateState.Background = Brushes.Gray;
            TextBlock_OperateState.Text = $"信息 : {str}";
        }
        else if (index == 2)
        {
            Border_OperateState.Background = Brushes.LightSalmon;
            TextBlock_OperateState.Text = $"警告 : {str}";
        }
        else if (index == 3)
        {
            Border_OperateState.Background = Brushes.Red;
            TextBlock_OperateState.Text = $"错误 : {str}";
        }
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        ProcessUtils.OpenLink(e.Uri.OriginalString);
        e.Handled = true;
    }

    ///////////////////////////////////////////////////////

    private void FTabControlSelect(int index)
    {
        TabControl_Main.SelectedIndex = index;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        if (WindowState != WindowState.Maximized)
            WindowState = WindowState.Maximized;
        else
            WindowState = WindowState.Normal;
    }
}
