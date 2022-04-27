using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Models;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Common.Views;
using BF1.ServerAdminTools.Wpf.Tasks;
using BF1.ServerAdminTools.Wpf.Utils;
using System.Windows.Media.Imaging;

namespace BF1.ServerAdminTools.Common
{
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
        public static event ClosingDispose? ClosingDisposeEvent;


        public static MainWindow? ThisMainWindow;

        public BlurUtils blur;

        public MainModel MainModel { get; set; }

        // 声明一个变量，用于存储软件开始运行的时间
        private DateTime Origin_DateTime;

        ///////////////////////////////////////////////////////

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
            if (!string.IsNullOrWhiteSpace(DataSave.Config.Bg) && File.Exists(DataSave.Config.Bg))
            {
                var image = new ImageBrush(new BitmapImage(new(DataSave.Config.Bg)))
                {
                    Stretch = Stretch.UniformToFill,
                    Opacity = DataSave.Config.Window_O ? (double)DataSave.Config.Bg_O / 100 : 1
                };
                ThisMainWindow.Background = image;
            }
            else
            {
                if (DataSave.Config.Window_O)
                    ThisMainWindow.Background = Brushes.Transparent;
                else
                    ThisMainWindow.Background = Brushes.White;
            }

            ThisMainWindow.blur.Composite(DataSave.Config.Window_O);
        }

        private void Window_Main_Loaded(object sender, RoutedEventArgs e)
        {
            MainModel.AppRunTime = "运行时间 : Loading...";

            ////////////////////////////////

            Title = CoreUtils.MainAppWindowName + CoreUtils.ClientVersionInfo + "- 最后编译时间 : " + File.GetLastWriteTime(Process.GetCurrentProcess().MainModule.FileName);

            // 获取当前时间，存储到对于变量中
            Origin_DateTime = DateTime.Now;

            ////////////////////////////////

            new Thread(UpdateState)
            {
                Name = "UpdateStateThead",
                IsBackground = true
            }.Start();

            new Thread(InitThread)
            {
                Name = "InitThread",
                IsBackground = true
            }.Start();

            this.DataContext = this;

            if (Core.MsgAllocateMemory())
                Core.LogInfo($"中文聊天指针分配成功 0x{Core.MsgGetAllocateMemoryAddress():x}");
            else
                Core.LogError($"中文聊天指针分配失败");

            Tasks.Start();
        }

        private void Window_Main_Closing(object sender, CancelEventArgs e)
        {
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
            Core.LogInfo($"开始调用刷新SessionID功能");
            AuthView._AutoRefreshSID();
        }

        private void UpdateState()
        {
            while (true)
            {
                // 获取软件运行时间
                MainModel.AppRunTime = "运行时间 : " + CoreUtils.ExecDateDiff(Origin_DateTime, DateTime.Now);

                if (Globals.IsGameRun)
                {
                    if (!Core.IsGameRun())
                    {
                        Globals.IsToolInit = false;
                        Globals.IsGameRun = false;
                        MsgBoxUtils.WarningMsgBox("游戏已退出，功能已关闭");
                    }
                }

                Thread.Sleep(1000);
            }
        }

        #region 常用方法
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
        #endregion

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
}
