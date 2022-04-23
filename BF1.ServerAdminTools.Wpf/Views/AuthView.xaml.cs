using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.API.BF1Server.RespJson;
using BF1.ServerAdminTools.Common.API.GT;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Common.Windows;
using ScottPlot;

using RestSharp;

namespace BF1.ServerAdminTools.Common.Views
{
    /// <summary>
    /// AuthView.xaml 的交互逻辑
    /// </summary>
    public partial class AuthView : UserControl
    {
        private WebView2Window WebView2Window = null;

        public static Action _AutoRefreshSID;

        public AuthView()
        {
            InitializeComponent();

            MainWindow.ClosingDisposeEvent += MainWindow_ClosingDisposeEvent;

            WpfPlot_Main1.Plot.Title("战地1 PC端 亚服周报", true, System.Drawing.Color.Black, 18, "微软雅黑");
            WpfPlot_Main2.Plot.Title("战地1 PC端 全服周报", true, System.Drawing.Color.Black, 18, "微软雅黑");

            UpdateWpfPlot(WpfPlot_Main1, "Asia");
            UpdateWpfPlot(WpfPlot_Main2, "ALL");

            WpfPlot_Main1.RightClicked -= WpfPlot_Main1.DefaultRightClickEvent;
            WpfPlot_Main2.RightClicked -= WpfPlot_Main2.DefaultRightClickEvent;

            WpfPlot_Main1.RightClicked += DeployCustomMenu1;
            WpfPlot_Main2.RightClicked += DeployCustomMenu2;

            var timerAutoRefresh = new Timer();
            timerAutoRefresh.AutoReset = true;
            timerAutoRefresh.Interval = 43200000;
            timerAutoRefresh.Elapsed += TimerAutoRefresh_Elapsed;
            timerAutoRefresh.Start();

            _AutoRefreshSID = AutoRefresh;
        }

        private void MainWindow_ClosingDisposeEvent()
        {
            WebView2Window?.Close();
        }

        private void AutoRefresh()
        {
            Core.LogInfo($"调用刷新SessionID功能成功");
            TimerAutoRefresh_Elapsed(null, null);
        }

        private async void TimerAutoRefresh_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                await Core.Login();
            }
            catch (Exception ex)
            {
                Core.LogError($"刷新SessionID失败", ex);
            }
        }

        private async void Button_RefreshPlayerSessionId_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            if (string.IsNullOrEmpty(TextBox_Remid.Text)) TextBox_Remid.Text = Globals.Config.Remid;
            else Globals.Config.Remid = TextBox_Remid.Text;
            if (string.IsNullOrEmpty(TextBox_Sid.Text)) TextBox_Sid.Text = Globals.Config.Sid;
            else Globals.Config.Sid = TextBox_Sid.Text;

            if (string.IsNullOrEmpty(Globals.Config.Remid) && string.IsNullOrEmpty(Globals.Config.Sid))
            {
                MainWindow._SetOperatingState(2, $"Cookie不存在，进行网页登录");
                WebLogin();
                return;
            }

            MainWindow._SetOperatingState(1, "正在获取AuthCode");

            string url = "https://accounts.ea.com/connect/auth?response_type=code&locale=zh_CN&client_id=sparta-backend-as-user-pc";
            var options = new RestClientOptions(url)
            {
                Timeout = 5000,
                FollowRedirects = false
            };

            string cookie = "";
            if (!string.IsNullOrEmpty(Globals.Config.Remid)) cookie = $"{cookie}remid={Globals.Config.Remid};";
            if (!string.IsNullOrEmpty(Globals.Config.Sid)) cookie = $"{cookie}sid={Globals.Config.Sid};";

            var client = new RestClient(options);
            var request = new RestRequest()
                .AddHeader("Cookie", cookie);

            var response = await client.ExecuteGetAsync(request);
            if (response.StatusCode != HttpStatusCode.Redirect)
            {
                MainWindow._SetOperatingState(3, $"EA连接失败{response.StatusCode}");
            }

            string location = response.Headers.ToList()
                .Find(x => x.Name == "Location")
                .Value.ToString();

            if (location.Contains("http://127.0.0.1/success?code="))
            {
                string code = location.Replace("http://127.0.0.1/success?code=", "");
                MainWindow._SetOperatingState(1, $"正在获取SessionId，Code为{code}");

                if (response.Cookies["remid"] != null)
                {
                    Globals.Config.Remid = response.Cookies["remid"].Value;
                }
                if (response.Cookies["sid"] != null)
                {
                    Globals.Config.Sid = response.Cookies["sid"].Value;
                }

                TextBox_Remid.Text = "";
                TextBox_Sid.Text = "";

                var result = await ServerAPI.GetEnvIdViaAuthCode(code);

                if (result.IsSuccess)
                {
                    var envIdViaAuthCode = JsonUtil.JsonDese<EnvIdViaAuthCode>(result.Message);
                    Globals.Config.SessionId = envIdViaAuthCode.result.sessionId;
                    MainWindow._SetOperatingState(1, $"获取SessionID成功  |  耗时: {result.ExecTime:0.00} 秒");
                    Core.SaveConfig();
                }
                else
                {
                    MainWindow._SetOperatingState(3, $"获取SessionID失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
                }
            }
            else
            {
                MainWindow._SetOperatingState(2, $"Cookie已失效，进行网页登录");
                WebLogin();
            }
        }

        private void WebLogin()
        {
            if (CoreUtil.IsWebView2DependencyInstalled())
            {
                if (WebView2Window == null)
                {
                    WebView2Window = new WebView2Window();
                    WebView2Window.Show();
                }
                else
                {
                    if (WebView2Window.IsVisible)
                    {
                        WebView2Window.Topmost = true;
                        WebView2Window.Topmost = false;
                        WebView2Window.WindowState = WindowState.Normal;
                    }
                    else
                    {
                        WebView2Window = null;
                        WebView2Window = new WebView2Window();
                        WebView2Window.Show();
                    }
                }
            }
            else
            {
                MainWindow._SetOperatingState(3, "未安装WebView2对应依赖，请安装依赖或手动获取Cookie");
                return;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ProcessUtil.OpenLink(e.Uri.OriginalString);
            e.Handled = true;
        }

        private async void UpdateWpfPlot(WpfPlot wpfPlot, string region)
        {
            var result = await GTAPI.GetStatusArray("7", region);

            if (result.IsSuccess)
            {
                var statusArray = result.Obj;

                int count = statusArray.timeStamps.Count;

                double[] dates = new double[count];
                for (int i = 0; i < count; i++)
                {
                    var d0 = Convert.ToDateTime(statusArray.timeStamps[i]).ToLocalTime();
                    dates[i] = d0.ToOADate();
                }

                double[] as_values = new double[count];
                for (int i = 0; i < count; i++)
                {
                    as_values[i] = statusArray.soldierAmount[i];
                }

                double[] cs_values = new double[count];
                for (int i = 0; i < count; i++)
                {
                    cs_values[i] = statusArray.communitySoldierAmount[i];
                }

                double[] ds_values = new double[count];
                for (int i = 0; i < count; i++)
                {
                    ds_values[i] = statusArray.diceSoldierAmount[i];
                }
                wpfPlot.Plot.Clear();

                wpfPlot.Plot.AxisAuto();
                wpfPlot.Plot.SetOuterViewLimits(dates[0], dates[count - 1], 0, as_values.Max() + 1000);

                wpfPlot.Plot.Style(ScottPlot.Style.Monospace);
                wpfPlot.Plot.Palette = Palette.Category10;

                wpfPlot.Plot.XAxis.Label("时间日期", System.Drawing.Color.Black, 14, false, "微软雅黑");
                wpfPlot.Plot.YAxis.Label("在线人数", System.Drawing.Color.Black, 14, false, "微软雅黑");

                wpfPlot.Plot.AddScatterLines(dates, as_values, System.Drawing.Color.Blue, 2, LineStyle.Solid, "所有玩家");
                wpfPlot.Plot.AddScatterLines(dates, cs_values, System.Drawing.Color.Green, 3, LineStyle.Solid, "私服玩家");
                wpfPlot.Plot.AddScatterLines(dates, ds_values, System.Drawing.Color.Red, 1, LineStyle.Solid, "官服玩家");

                wpfPlot.Plot.Legend(location: Alignment.UpperRight);

                wpfPlot.Plot.XAxis.DateTimeFormat(true);
                wpfPlot.Plot.XAxis.TickLabelFormat("MM/dd HH:mm", true);

                // 定义刻度间隔
                //wpfPlot.Plot.XAxis.ManualTickSpacing(6, ScottPlot.Ticks.DateTimeUnit.Hour);
                wpfPlot.Plot.XAxis.TickLabelStyle(rotation: 45);

                // 为旋转的刻度添加额外的空间
                //wpfPlot.Plot.XAxis.SetSizeLimit(min: 25);

                wpfPlot.Refresh();

                MainWindow._SetOperatingState(1, $"获取战地1玩家人数成功  |  耗时: {result.ExecTime:0.00} 秒");
            }
            else
            {
                MainWindow._SetOperatingState(3, $"获取战地1玩家人数失败  |  耗时: {result.ExecTime:0.00} 秒");
            }
        }

        private void DeployCustomMenu1(object sender, EventArgs e)
        {
            MenuItem updateDataMenuItem = new()
            { Header = "更新表格数据" };
            updateDataMenuItem.Click += UpdateData1;

            MenuItem defaultViewMenuItem = new()
            { Header = "恢复默认视图" };
            defaultViewMenuItem.Click += DefaultView1;

            ContextMenu rightClickMenu = new();
            rightClickMenu.Items.Add(updateDataMenuItem);
            rightClickMenu.Items.Add(defaultViewMenuItem);

            rightClickMenu.IsOpen = true;
        }

        private void UpdateData1(object sender, RoutedEventArgs e)
        {
            MainWindow._SetOperatingState(2, $"正在更新表格数据中...");
            UpdateWpfPlot(WpfPlot_Main1, "Asia");
        }

        private void DefaultView1(object sender, RoutedEventArgs e)
        {
            WpfPlot_Main1.Plot.AxisAuto();
            WpfPlot_Main1.Refresh();
        }

        private void DeployCustomMenu2(object sender, EventArgs e)
        {
            MenuItem updateDataMenuItem = new() { Header = "更新表格数据" };
            updateDataMenuItem.Click += UpdateData2;

            MenuItem defaultViewMenuItem = new() { Header = "恢复默认视图" };
            defaultViewMenuItem.Click += DefaultView2;

            ContextMenu rightClickMenu = new();
            rightClickMenu.Items.Add(updateDataMenuItem);
            rightClickMenu.Items.Add(defaultViewMenuItem);

            rightClickMenu.IsOpen = true;
        }

        private void UpdateData2(object sender, RoutedEventArgs e)
        {
            MainWindow._SetOperatingState(2, $"正在更新表格数据中...");
            UpdateWpfPlot(WpfPlot_Main2, "ALL");
        }

        private void DefaultView2(object sender, RoutedEventArgs e)
        {
            WpfPlot_Main1.Plot.AxisAuto();
            WpfPlot_Main1.Refresh();
        }
    }
}
