﻿using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Utils;
using Microsoft.Web.WebView2.Core;

namespace BF1.ServerAdminTools.Windows
{
    /// <summary>
    /// WebView2Window.xaml 的交互逻辑
    /// </summary>
    public partial class WebView2Window : Window
    {
        private const string Uri = "https://companion-api.battlefield.com/companion/home";

        public WebView2Window()
        {
            InitializeComponent();
        }

        #region 加载与关闭
        private async void Window_WebView2_Loaded(object sender, RoutedEventArgs e)
        {
            var env = await CoreWebView2Environment.CreateAsync(null, FileUtil.D_Cache_Path, null);

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

            WebView2.CoreWebView2.Navigate(Uri);
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

        private async void Button_GetLastSessionID_Click(object sender, RoutedEventArgs e)
        {
            var session = await WebView2.CoreWebView2.ExecuteScriptAsync("localStorage.gatewaySessionId");

            if (!string.IsNullOrEmpty(session) && session != "null")
            {
                session = session.Replace('\"', ' ').Trim();
                Globals.SessionId = session;

                if (MessageBox.Show($"成功获取到玩家SessionID\n\n{session}\n\n是否现在就关闭此窗口？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
            else
            {
                MsgBoxUtil.ErrorMsgBox("获取玩家SessionID失败，请重新登录网页后再次尝试");
            }
        }

        private async void Button_ClearCache_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("你确认要清空本地缓存吗，这一般会在SessionID失效的情况下使用，你可能需要重新登录小帮手", "警告",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await WebView2.CoreWebView2.ExecuteScriptAsync("localStorage.clear()");

                WebView2.CoreWebView2.CookieManager.DeleteAllCookies();
                WebView2.Reload();
            }
        }
    }
}