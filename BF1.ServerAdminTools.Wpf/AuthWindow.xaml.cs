using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Helper;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Wpf.Helper;
using BF1.ServerAdminTools.Wpf.Utils;
using BF1.ServerAdminTools.Wpf.Windows;

namespace BF1.ServerAdminTools.Wpf;

/// <summary>
/// AuthWindow.xaml 的交互逻辑
/// </summary>
public partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
    }

    private void Window_Auth_Loaded(object sender, RoutedEventArgs e)
    {
        TextBlock_VersionInfo.Text = CoreUtils.ClientVersionInfo.ToString();
        TextBlock_BuildDate.Text = CoreUtils.ClientBuildTime.ToString();

        Task.Run(() =>
        {
            UpdateState("欢迎来到《BATTLEFIELD 1》...");
            Core.LogInfo("开始初始化程序...");
            Core.LogInfo($"当前程序版本号 {CoreUtils.ClientVersionInfo}");
            Core.LogInfo($"当前程序最后编译时间 {CoreUtils.ClientBuildTime}");

            Core.DnsFlushResolverCache();
            Core.LogInfo("刷新DNS缓存成功");

            // 初始化
            if (Core.HookInit())
            {
                Core.LogInfo("战地1内存模块初始化成功");
            }
            else
            {
                UpdateState($"战地1内存模块初始化失败！");
                Core.LogError("战地1内存模块初始化失败");
                Task.Delay(1000).Wait();
            }

            UpdateState("正在为您营造个性化体验...");

            Core.ConfigInit();
            ConfigUtils.Init();
            ConfigUtils.LoadAll();
            Core.SQLInit();

            ImageData.InitDict();
            Core.LogInfo("本地图片缓存库初始化成功");

            ChsUtils.ToTraditionalChinese("免费，跨平台，开源！");
            Core.LogInfo("简繁翻译库初始化成功");

            ////////////////////////////////////////////////////////////////////

            bool isNew = false;

            try
            {
                UpdateState("正在检测版本更新...");
                Core.LogInfo($"正在检测版本更新...");
                Update(ref isNew);
            }
            catch (Exception ex)
            {
                UpdateState("软件更新服务器链接失败");
                Task.Delay(2000).Wait();
            }

            if (!isNew)
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    MainWindow mainWindow = new();
                    mainWindow.Show();
                    Application.Current.MainWindow = mainWindow;

                    this.Close();
                });
        });
    }

    private void UpdateState(string msg)
    {
        Dispatcher.BeginInvoke(() =>
        {
            TextBlock_State.Text = msg;
        });
    }

    private void Update(ref bool isNew)
    {
        // 获取版本更新
        var webConfig = HttpUtil.HttpClientGET(CoreUtils.Config_Address).Result;
        if (string.IsNullOrEmpty(webConfig))
        {
            UpdateState("获取新版本信息失败！");
            return;
        }
        var updateInfo = JsonUtils.JsonDese<UpdateInfo>(webConfig);
        if (updateInfo == null)
        {
            return;
        }
        CoreUtils.ServerVersionInfo = new Version(updateInfo.Version);

        if (CoreUtils.ServerVersionInfo > CoreUtils.ClientVersionInfo)
        {
            Core.LogInfo($"发现新版本 {CoreUtils.ServerVersionInfo}");

            CoreUtils.Notice_Address = updateInfo.Address.Notice;
            CoreUtils.Change_Address = updateInfo.Address.Change;

            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                this.Hide();
            });

            if (MessageBox.Show($"检测到新版本已发布，是否立即前往更新？                                        " +
                $"\n\n{updateInfo.Latest.Date}\n{updateInfo.Latest.Change}\n\n强烈建议大家使用最新版本！点否退出程序",
                "发现新版本", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                isNew = true;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var UpdateWindow = new UpdateWindow(updateInfo);
                    UpdateWindow.Owner = MainWindow.ThisMainWindow;
                    UpdateWindow.ShowDialog();

                    this.Close();
                });
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
        else
        {
            UpdateState("连线中...");
            Core.LogInfo($"当前已是最新版本 {CoreUtils.ServerVersionInfo}");
        }
    }
}
