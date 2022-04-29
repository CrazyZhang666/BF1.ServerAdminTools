using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Utils;
using System.Collections.Concurrent;

namespace BF1.ServerAdminTools.Common.Views
{
    /// <summary>
    /// ChatView.xaml 的交互逻辑
    /// </summary>
    public partial class ChatView : UserControl
    {
        private readonly string[] defaultMsg = new string[10];

        private readonly Timer timerAutoSendMsg = new();
        private readonly Timer timerNoAFK = new();

        private readonly ConcurrentBag<string> queueMsg = new();

        private int queueMsgSleep = 1;

        public ICommand SendChsMessageCommand { get; set; }

        public class Comm : ICommand
        {
            public ChatView ChatView;

            public Comm(ChatView view)
            {
                ChatView = view;
            }
            public event EventHandler? CanExecuteChanged;

            public bool CanExecute(object? parameter)
            {
                return true;
            }

            public void Execute(object? parameter)
            {
                ChatView.SendChsMessage(null, null);
            }
        }

        public ChatView()
        {
            SendChsMessageCommand = new Comm(this);
            InitializeComponent();

            this.DataContext = this;

            defaultMsg[0] = DataSave.Config.Msg0;
            defaultMsg[1] = DataSave.Config.Msg1;
            defaultMsg[2] = DataSave.Config.Msg2;
            defaultMsg[3] = DataSave.Config.Msg3;
            defaultMsg[4] = DataSave.Config.Msg4;
            defaultMsg[5] = DataSave.Config.Msg5;
            defaultMsg[6] = DataSave.Config.Msg6;
            defaultMsg[7] = DataSave.Config.Msg7;
            defaultMsg[8] = DataSave.Config.Msg8;
            defaultMsg[9] = DataSave.Config.Msg9;

            if (string.IsNullOrEmpty(defaultMsg[0]))
            {
                defaultMsg[0] = "战地1中文输入测试，最大30个汉字";
            }

            TextBox_InputMsg.Text = defaultMsg[0];

            timerAutoSendMsg.AutoReset = true;
            timerAutoSendMsg.Elapsed += TimerAutoSendMsg_Elapsed;

            timerNoAFK.AutoReset = true;
            timerNoAFK.Interval = 30000;
            timerNoAFK.Elapsed += TimerNoAFK_Elapsed;
        }

        private void SetIMEState()
        {
            // 设置输入法为英文
            Application.Current.Dispatcher.Invoke(() =>
            {
                InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
            });
        }

        private void TimerAutoSendMsg_Elapsed(object? sender, ElapsedEventArgs e)
        {
            if (!Globals.IsGameRun)
                MsgBoxUtils.ErrorMsgBox("游戏还未启动");

            if (!Globals.IsToolInit)
                MsgBoxUtils.ErrorMsgBox("工具还未正常初始化");

            SetIMEState();
            Thread.Sleep(50);

            while (queueMsg.Any())
            {
                if (queueMsg.TryTake(out var item))
                {
                    Core.SendText(item);
                    Thread.Sleep(queueMsgSleep * 1000);
                }
            }
        }

        private void TimerNoAFK_Elapsed(object? sender, ElapsedEventArgs e)
        {
            SetIMEState();
            Thread.Sleep(50);

            Core.SetForegroundWindow();
            Thread.Sleep(50);

            Core.KeyTab();
        }

        private void SendChsMessage(object sender, RoutedEventArgs e)
        {
            AudioUtils.ClickSound();

            if (!Globals.IsGameRun)
                MsgBoxUtils.ErrorMsgBox("游戏还未启动");

            if (!Globals.IsToolInit)
                MsgBoxUtils.ErrorMsgBox("工具还未正常初始化");

            SetIMEState();
            Thread.Sleep(20);

            Core.SetKeyPressDelay((int)Slider_KeyPressDelay.Value);

            string msg = TextBox_InputMsg.Text.Trim();

            if (string.IsNullOrEmpty(msg))
            {
                MainWindow.SetOperatingState(2, "聊天框内容为空，操作取消");
                return;
            }

            MainWindow.SetOperatingState(2, Core.SendText(msg));
        }

        private void TextBox_InputMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBlock_TxtLength.Text = $"当前文本长度 : {PlayerUtils.GetStrLength(TextBox_InputMsg.Text)} 字符";

            defaultMsg[RadioButtonWhoIsChecked()] = TextBox_InputMsg.Text;
        }

        private void RadioButton_DefaultText0_Click(object sender, RoutedEventArgs e)
        {
            TextBox_InputMsg.Text = defaultMsg[RadioButtonWhoIsChecked()];
        }

        private int RadioButtonWhoIsChecked()
        {
            if (RadioButton_DefaultText0 != null && RadioButton_DefaultText0.IsChecked == true)
                return 0;

            if (RadioButton_DefaultText1 != null && RadioButton_DefaultText1.IsChecked == true)
                return 1;

            if (RadioButton_DefaultText2 != null && RadioButton_DefaultText2.IsChecked == true)
                return 2;

            if (RadioButton_DefaultText3 != null && RadioButton_DefaultText3.IsChecked == true)
                return 3;

            if (RadioButton_DefaultText4 != null && RadioButton_DefaultText4.IsChecked == true)
                return 4;

            if (RadioButton_DefaultText5 != null && RadioButton_DefaultText5.IsChecked == true)
                return 5;

            if (RadioButton_DefaultText6 != null && RadioButton_DefaultText6.IsChecked == true)
                return 6;

            if (RadioButton_DefaultText7 != null && RadioButton_DefaultText7.IsChecked == true)
                return 7;

            if (RadioButton_DefaultText8 != null && RadioButton_DefaultText8.IsChecked == true)
                return 8;

            if (RadioButton_DefaultText9 != null && RadioButton_DefaultText9.IsChecked == true)
                return 9;

            return 0;
        }

        private void CheckBox_ActiveAutoSendMsg_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_ActiveAutoSendMsg.IsChecked == true)
            {
                queueMsg.Clear();

                if (CheckBox_DefaultText0 != null && CheckBox_DefaultText0.IsChecked == true)
                    queueMsg.Add(defaultMsg[0]);

                if (CheckBox_DefaultText1 != null && CheckBox_DefaultText1.IsChecked == true)
                    queueMsg.Add(defaultMsg[1]);

                if (CheckBox_DefaultText2 != null && CheckBox_DefaultText2.IsChecked == true)
                    queueMsg.Add(defaultMsg[2]);

                if (CheckBox_DefaultText3 != null && CheckBox_DefaultText3.IsChecked == true)
                    queueMsg.Add(defaultMsg[3]);

                if (CheckBox_DefaultText4 != null && CheckBox_DefaultText4.IsChecked == true)
                    queueMsg.Add(defaultMsg[4]);

                if (CheckBox_DefaultText5 != null && CheckBox_DefaultText5.IsChecked == true)
                    queueMsg.Add(defaultMsg[5]);

                if (CheckBox_DefaultText6 != null && CheckBox_DefaultText6.IsChecked == true)
                    queueMsg.Add(defaultMsg[6]);

                if (CheckBox_DefaultText7 != null && CheckBox_DefaultText7.IsChecked == true)
                    queueMsg.Add(defaultMsg[7]);

                if (CheckBox_DefaultText8 != null && CheckBox_DefaultText8.IsChecked == true)
                    queueMsg.Add(defaultMsg[8]);

                if (CheckBox_DefaultText9 != null && CheckBox_DefaultText9.IsChecked == true)
                    queueMsg.Add(defaultMsg[9]);

                Core.SetKeyPressDelay((int)Slider_KeyPressDelay.Value);

                queueMsgSleep = (int)Slider_AutoSendMsgSleep.Value;

                timerAutoSendMsg.Interval = Slider_AutoSendMsg.Value * 1000 * 60;
                timerAutoSendMsg.Start();

                MainWindow.SetOperatingState(1, "已启用定时发送指定文本功能");
            }
            else
            {
                timerAutoSendMsg.Stop();
                MainWindow.SetOperatingState(1, "已关闭定时发送指定文本功能");
            }
        }

        private void CheckBox_ActiveNoAFK_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_ActiveNoAFK.IsChecked == true)
            {
                timerNoAFK.Start();
                MainWindow.SetOperatingState(1, "已启用游戏内挂机防踢功能");
            }
            else
            {
                timerNoAFK.Stop();
                MainWindow.SetOperatingState(1, "已关闭游戏内挂机防踢功能");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            defaultMsg[RadioButtonWhoIsChecked()] = TextBox_InputMsg.Text;

            DataSave.Config.Msg0 = defaultMsg[0];
            DataSave.Config.Msg1 = defaultMsg[1];
            DataSave.Config.Msg2 = defaultMsg[2];
            DataSave.Config.Msg3 = defaultMsg[3];
            DataSave.Config.Msg4 = defaultMsg[4];
            DataSave.Config.Msg5 = defaultMsg[5];
            DataSave.Config.Msg6 = defaultMsg[6];
            DataSave.Config.Msg7 = defaultMsg[7];
            DataSave.Config.Msg8 = defaultMsg[8];
            DataSave.Config.Msg9 = defaultMsg[9];

            ConfigUtils.SaveConfig();
        }
    }
}
