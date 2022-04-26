using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.GameImage;
using BF1.ServerAdminTools.Netty;

namespace BF1.ServerAdminTools.Common.Views
{
    /// <summary>
    /// OptionView.xaml 的交互逻辑
    /// </summary>
    public partial class OptionView : UserControl
    {
        public OptionView()
        {
            InitializeComponent();

            AudioUtils.ClickSoundIndex = Globals.Config.AudioIndex;

            switch (AudioUtils.ClickSoundIndex)
            {
                case 0:
                    RadioButton_ClickAudioSelect0.IsChecked = true;
                    break;
                case 1:
                    RadioButton_ClickAudioSelect1.IsChecked = true;
                    break;
                case 2:
                    RadioButton_ClickAudioSelect2.IsChecked = true;
                    break;
                case 3:
                    RadioButton_ClickAudioSelect3.IsChecked = true;
                    break;
                case 4:
                    RadioButton_ClickAudioSelect4.IsChecked = true;
                    break;
                case 5:
                    RadioButton_ClickAudioSelect5.IsChecked = true;
                    break;
            }

            MainWindow.ClosingDisposeEvent += MainWindow_ClosingDisposeEvent;

            var obj = NettyCore.GetConfig();
            Server_Port.Text = obj.Port.ToString();
            Server_Key.Text = obj.ServerKey.ToString();
            AutoRun.IsChecked = DataSave.Config.AutoRun;
            Slider_BG_O.Value = DataSave.Config.Bg_O;
            Window_O.IsChecked = DataSave.Config.Window_O;
            Window_A.IsChecked = DataSave.Config.Window_A;
            if (DataSave.Config.AutoRun)
            {
                try
                {
                    NettyCore.StartServer();
                    Button_Server.Content = "关闭";
                }
                catch (Exception ex)
                {
                    Core.LogError("Netty服务器启动出错", ex);
                    MsgBoxUtils.ErrorMsgBox("Netty服务器启动出错", ex);
                }
            }
            if (DataSave.Config.Window_A)
            {
                GameWindow.Start();
            }
        }

        private void MainWindow_ClosingDisposeEvent()
        {
            Globals.Config.AudioIndex = AudioUtils.ClickSoundIndex;
        }

        private void RadioButton_ClickAudioSelect_Click(object sender, RoutedEventArgs e)
        {
            string str = (sender as RadioButton).Content.ToString();

            switch (str)
            {
                case "无":
                    AudioUtils.ClickSoundIndex = 0;
                    break;
                case "提示音1":
                    AudioUtils.ClickSoundIndex = 1;
                    AudioUtils.ClickSound();
                    break;
                case "提示音2":
                    AudioUtils.ClickSoundIndex = 2;
                    AudioUtils.ClickSound();
                    break;
                case "提示音3":
                    AudioUtils.ClickSoundIndex = 3;
                    AudioUtils.ClickSound();
                    break;
                case "提示音4":
                    AudioUtils.ClickSoundIndex = 4;
                    AudioUtils.ClickSound();
                    break;
                case "提示音5":
                    AudioUtils.ClickSoundIndex = 5;
                    AudioUtils.ClickSound();
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Core.IsGameRun())
            {
                MsgBoxUtils.ErrorMsgBox("没有检测到游戏进程");
                return;
            }

            if (!Core.HookInit())
            {
                Core.LogError("战地1内存模块初始化失败");
                MsgBoxUtils.ErrorMsgBox("战地1内存模块初始化失败");
                return;
            }

            Core.LogInfo("战地1内存模块初始化成功");
            MsgBoxUtils.InformationMsgBox("检测到游戏运行");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Server_Port.Text))
            {
                MsgBoxUtils.ErrorMsgBox("端口号为空");
                return;
            }
            if (!int.TryParse(Server_Port.Text, out var port))
            {
                MsgBoxUtils.ErrorMsgBox("端口号错误");
                return;
            }
            if (string.IsNullOrWhiteSpace(Server_Key.Text))
            {
                MsgBoxUtils.ErrorMsgBox("服务器密钥为空");
                return;
            }
            if (!long.TryParse(Server_Key.Text, out var key))
            {
                MsgBoxUtils.ErrorMsgBox("服务器密钥错误");
                return;
            }

            NettyCore.SetConfig(new ConfigNettyObj
            {
                Port = port,
                ServerKey = key
            });

            DataSave.Config.AutoRun = AutoRun.IsChecked == true;
            ConfigUtils.SaveConfig();

            MainWindow.SetOperatingState(1, "设置成功");
            if (NettyCore.State)
                try
                {
                    NettyCore.StopServer();
                }
                catch (Exception ex)
                {
                    Core.LogError("Netty服务器关闭出错", ex);
                    MsgBoxUtils.ErrorMsgBox("Netty服务器关闭出错", ex);
                }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (NettyCore.State)
            {
                try
                {
                    NettyCore.StopServer();
                    Button_Server.Content = "启动";
                }
                catch (Exception ex)
                {
                    Core.LogError("Netty服务器关闭出错", ex);
                    MsgBoxUtils.ErrorMsgBox("Netty服务器关闭出错", ex);
                }
            }
            else
            {
                try
                {
                    NettyCore.StartServer();
                    Button_Server.Content = "关闭";
                }
                catch (Exception ex)
                {
                    Core.LogError("Netty服务器启动出错", ex);
                    MsgBoxUtils.ErrorMsgBox("Netty服务器启动出错", ex);
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var file = FileSelectUtils.FileSelectPic();
            if (file == null)
                return;

            DataSave.Config.Bg = file;
            ConfigUtils.SaveConfig();

            MainWindow.BG();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            DataSave.Config.Bg = "";
            ConfigUtils.SaveConfig();

            MainWindow.BG();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            DataSave.Config.Window_O = Window_O.IsChecked == true;
            DataSave.Config.Bg_O = (int)Slider_BG_O.Value;
            ConfigUtils.SaveConfig();

            MainWindow.BG();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            DataSave.Config.Window_A = Window_A.IsChecked == true;
            ConfigUtils.SaveConfig();
            if (DataSave.Config.Window_A)
            {
                GameWindow.Start();
            }
            else
            {
                GameWindow.Pause();
            }
        }
    }
}
