using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.GameImage;
using BF1.ServerAdminTools.Netty;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Utils;
using System.Drawing.Imaging;

namespace BF1.ServerAdminTools.Wpf.Views;

/// <summary>
/// OptionView.xaml 的交互逻辑
/// </summary>
public partial class OptionView : UserControl
{
    public OptionView()
    {
        InitializeComponent();

        switch (DataSave.Config.AudioSelect)
        {
            case 0:
                ClickAudioSelect0.IsChecked = true;
                break;
            case 1:
                ClickAudioSelect1.IsChecked = true;
                break;
            case 2:
                ClickAudioSelect2.IsChecked = true;
                break;
            case 3:
                ClickAudioSelect3.IsChecked = true;
                break;
            case 4:
                ClickAudioSelect4.IsChecked = true;
                break;
            case 5:
                ClickAudioSelect5.IsChecked = true;
                break;
        }

        switch (DataSave.Config.GameXYSelect)
        {
            case 0:
                ClickXYSelect0.IsChecked = true;
                break;
            case 1:
                ClickXYSelect1.IsChecked = true;
                break;
        }

        var obj = NettyCore.GetConfig();
        Server_Port.Text = obj.Port.ToString();
        Server_Key.Text = obj.ServerKey.ToString();
        AutoRun.IsChecked = DataSave.Config.AutoRunNetty;
        Slider_BG_O.Value = DataSave.Config.BackgroudOpacity;
        Window_O.IsChecked = DataSave.Config.WindowVacuity;
        Window_A.IsChecked = DataSave.Config.AutoJoinServer;
        NettyBQ1.IsChecked = DataSave.Config.NettyBQ1;
        NettyBQ2.IsChecked = DataSave.Config.NettyBQ2;
        NettyBQ3.IsChecked = DataSave.Config.NettyBQ3;
    }

    private void RadioButton_ClickAudioSelect_Click(object sender, RoutedEventArgs e)
    {
        string str = (sender as RadioButton).Content.ToString();

        switch (str)
        {
            case "无":
                DataSave.Config.AudioSelect = 0;
                break;
            case "提示音1":
                DataSave.Config.AudioSelect = 1;
                AudioUtils.ClickSound();
                break;
            case "提示音2":
                DataSave.Config.AudioSelect = 2;
                AudioUtils.ClickSound();
                break;
            case "提示音3":
                DataSave.Config.AudioSelect = 3;
                AudioUtils.ClickSound();
                break;
            case "提示音4":
                DataSave.Config.AudioSelect = 4;
                AudioUtils.ClickSound();
                break;
            case "提示音5":
                DataSave.Config.AudioSelect = 5;
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

        DataSave.Config.NettyBQ1 = NettyBQ1.IsChecked == true;
        DataSave.Config.NettyBQ2 = NettyBQ2.IsChecked == true;
        DataSave.Config.NettyBQ3 = NettyBQ3.IsChecked == true;
        DataSave.Config.AutoRunNetty = AutoRun.IsChecked == true;
        WpfConfigUtils.SaveConfig();

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

        DataSave.Config.Background = file;
        WpfConfigUtils.SaveConfig();

        MainWindow.BG();
    }

    private void Button_Click_4(object sender, RoutedEventArgs e)
    {
        DataSave.Config.Background = "";
        MainWindow.BG();
        WpfConfigUtils.SaveConfig();
    }

    private void Button_Click_5(object sender, RoutedEventArgs e)
    {
        DataSave.Config.WindowVacuity = Window_O.IsChecked == true;
        DataSave.Config.BackgroudOpacity = (int)Slider_BG_O.Value;
        MainWindow.BG();
        WpfConfigUtils.SaveConfig();
    }

    private void Button_Click_7(object sender, RoutedEventArgs e)
    {
        if (!Globals.IsGameRun || !Globals.IsToolInit)
        {
            MsgBoxUtils.WarningMsgBox("工具未初始化");
            return;
        }
        var img = GameWindowImg.GetWindow();
        img.Save("test.png", ImageFormat.Png);
    }

    private void ClickXYSelect0_Checked(object sender, RoutedEventArgs e)
    {
        DataSave.Config.GameXYSelect = 0;
        GameWindow.XY = 0;
        WpfConfigUtils.SaveConfig();
    }

    private void ClickXYSelect1_Checked(object sender, RoutedEventArgs e)
    {
        DataSave.Config.GameXYSelect = 1;
        GameWindow.XY = 1;
        WpfConfigUtils.SaveConfig();
    }

    private void ClickAudioSelect0_Checked(object sender, RoutedEventArgs e)
    {
        DataSave.Config.AudioSelect = 0;
        WpfConfigUtils.SaveConfig();
    }

    private void ClickAudioSelect1_Checked(object sender, RoutedEventArgs e)
    {
        DataSave.Config.AudioSelect = 1;
        WpfConfigUtils.SaveConfig();
    }

    private void ClickAudioSelect2_Checked(object sender, RoutedEventArgs e)
    {
        DataSave.Config.AudioSelect = 2;
        WpfConfigUtils.SaveConfig();
    }

    private void ClickAudioSelect3_Checked(object sender, RoutedEventArgs e)
    {
        DataSave.Config.AudioSelect = 3;
        WpfConfigUtils.SaveConfig();
    }

    private void ClickAudioSelect4_Checked(object sender, RoutedEventArgs e)
    {
        DataSave.Config.AudioSelect = 4;
        WpfConfigUtils.SaveConfig();
    }

    private void ClickAudioSelect5_Checked(object sender, RoutedEventArgs e)
    {
        DataSave.Config.AudioSelect = 5;
        WpfConfigUtils.SaveConfig();
    }

    private void Button_Click_6(object sender, RoutedEventArgs e)
    {
        DataSave.Config.AutoJoinServer = Window_A.IsChecked == true;
        if (DataSave.Config.AutoJoinServer)
        {
            GameWindow.Start();
        }
        else
        {
            GameWindow.Pause();
        }
        WpfConfigUtils.SaveConfig();
    }

    private void View_Option_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataSave.Config.AutoRunNetty)
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
        if (DataSave.Config.AutoJoinServer)
        {
            GameWindow.Start();
        }
    }
}
