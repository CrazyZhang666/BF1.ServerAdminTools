using BF1.ServerAdminTools.Common.Helper;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.API.BF1Server.RespJson;
using Microsoft.Web.WebView2.Core;

namespace BF1.ServerAdminTools.Common.Windows
{
    /// <summary>
    /// WebView2Window.xaml 的交互逻辑
    /// </summary>
    public partial class WebView2Window : Window
    {
        private const string Uri = "https://accounts.ea.com/connect/auth?response_type=code&locale=zh_CN&client_id=sparta-backend-as-user-pc";

        public WebView2Window()
        {
            InitializeComponent();
        }

        #region 加载与关闭
        private async void Window_WebView2_Loaded(object sender, RoutedEventArgs e)
        {
            // 刷新DNS缓存
            CoreUtil.FlushDNSCache();
            Core.LogInfo($"启动WebView2成功，已刷新DNS缓存");

            var env = await CoreWebView2Environment.CreateAsync(null, ConfigLocal.Cache, null);

            await WebView2.EnsureCoreWebView2Async(env);

            // 禁止dev菜单
            WebView2.CoreWebView2.Settings.AreDevToolsEnabled = false;
            // 禁止所有菜单
            WebView2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            // 禁止缩放
            WebView2.CoreWebView2.Settings.IsZoomControlEnabled = false;
            // 禁止显示状态栏，鼠标悬浮在链接上时右下角没有url地址显示
            WebView2.CoreWebView2.Settings.IsStatusBarEnabled = false;

            // 新窗口打开页面的处理
            WebView2.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;

            // Url变化的处理
            WebView2.CoreWebView2.SourceChanged += CoreWebView2_SourceChanged;

            WebView2.CoreWebView2.Navigate(Uri);
        }

        private async void CoreWebView2_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            if (!WebView2.Source.ToString().Contains("http://127.0.0.1/success?code=")) return;

            string code = WebView2.Source.ToString().Replace("http://127.0.0.1/success?code=", "");

            var cookies = await WebView2.CoreWebView2.CookieManager.GetCookiesAsync(null);

            if (cookies == null)
            {
                MainWindow._SetOperatingState(3, $"登录成功，获取Cookie失败，请尝试清除缓存");
                return;
            }

            foreach (var item in cookies)
            {
                if (item.Name == "remid")
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        Globals.Config.Remid = item.Value;
                    }
                    continue;
                }

                if (item.Name == "sid")
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        Globals.Config.Sid = item.Value;
                    }
                    continue;
                }
            }

            MainWindow._SetOperatingState(1, $"登录完成，正在获取SessionId，Code为{code}");

            this.Close();

            var result = await ServerAPI.GetEnvIdViaAuthCode(code);

            if (result.IsSuccess)
            {
                var envIdViaAuthCode = JsonUtil.JsonDese<EnvIdViaAuthCode>(result.Message);
                Globals.Config.SessionId = envIdViaAuthCode.result.sessionId;
                Core.SaveConfig();
                MainWindow._SetOperatingState(1, $"获取SessionID成功:{Globals.Config.SessionId}  |  耗时: {result.ExecTime:0.00} 秒");
            }
            else
            {
                MainWindow._SetOperatingState(3, $"获取SessionID失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
            }
        }



        private void Window_WebView2_Closing(object sender, CancelEventArgs e)
        {
            WebView2.Dispose();
        }
        #endregion

        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            var deferral = e.GetDeferral();
            e.NewWindow = WebView2.CoreWebView2;
            deferral.Complete();
        }

        private async void Button_ClearCache_Click(object sender, RoutedEventArgs e)
        {
            await WebView2.CoreWebView2?.ExecuteScriptAsync("localStorage.clear()");
            WebView2.CoreWebView2?.CookieManager.DeleteAllCookies();

            WebView2.Reload();

            Core.LogInfo($"清空WebView2缓存成功");
        }
    }
}
