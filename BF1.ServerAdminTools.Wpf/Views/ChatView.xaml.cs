using BF1.ServerAdminTools.Common.Utils;

namespace BF1.ServerAdminTools.Common.Views
{
    /// <summary>
    /// ChatView.xaml 的交互逻辑
    /// </summary>
    public partial class ChatView : UserControl
    {
        private string[] defaultMsg = new string[10];

        private Timer timerAutoSendMsg = new();
        private Timer timerNoAFK = new();

        private List<string> queueMsg = new();

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

            defaultMsg[0] = Globals.Config.Msg0;
            defaultMsg[1] = Globals.Config.Msg1;
            defaultMsg[2] = Globals.Config.Msg2;
            defaultMsg[3] = Globals.Config.Msg3;
            defaultMsg[4] = Globals.Config.Msg4;
            defaultMsg[5] = Globals.Config.Msg5;
            defaultMsg[6] = Globals.Config.Msg6;
            defaultMsg[7] = Globals.Config.Msg7;
            defaultMsg[8] = Globals.Config.Msg8;
            defaultMsg[9] = Globals.Config.Msg9;

            if (string.IsNullOrEmpty(defaultMsg[0]))
            {
                defaultMsg[0] = "战地1中文输入测试，最大30个汉字";
            }

            TextBox_InputMsg.Text = defaultMsg[0];

            MainWindow.ClosingDisposeEvent += MainWindow_ClosingDisposeEvent;

            timerAutoSendMsg.AutoReset = true;
            timerAutoSendMsg.Elapsed += TimerAutoSendMsg_Elapsed;

            timerNoAFK.AutoReset = true;
            timerNoAFK.Interval = 30000;
            timerNoAFK.Elapsed += TimerNoAFK_Elapsed;
        }

        private void MainWindow_ClosingDisposeEvent()
        {
            defaultMsg[RadioButtonWhoIsChecked()] = TextBox_InputMsg.Text;

            Globals.Config.Msg0 = defaultMsg[0];
            Globals.Config.Msg1 = defaultMsg[1];
            Globals.Config.Msg2 = defaultMsg[2];
            Globals.Config.Msg3 = defaultMsg[3];
            Globals.Config.Msg4 = defaultMsg[4];
            Globals.Config.Msg5 = defaultMsg[5];
            Globals.Config.Msg6 = defaultMsg[6];
            Globals.Config.Msg7 = defaultMsg[7];
            Globals.Config.Msg8 = defaultMsg[8];
            Globals.Config.Msg9 = defaultMsg[9];
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

            for (int i = 0; i < queueMsg.Count; i++)
            {
                Core.SendText(queueMsg[i]);
                Thread.Sleep(queueMsgSleep * 1000);
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
                MainWindow._SetOperatingState(2, "聊天框内容为空，操作取消");
                return;
            }

            MainWindow._SetOperatingState(2, Core.SendText(msg));
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

                MainWindow._SetOperatingState(1, "已启用定时发送指定文本功能");
            }
            else
            {
                timerAutoSendMsg.Stop();
                MainWindow._SetOperatingState(1, "已关闭定时发送指定文本功能");
            }
        }

        private void CheckBox_ActiveNoAFK_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_ActiveNoAFK.IsChecked == true)
            {
                timerNoAFK.Start();
                MainWindow._SetOperatingState(1, "已启用游戏内挂机防踢功能");
            }
            else
            {
                timerNoAFK.Stop();
                MainWindow._SetOperatingState(1, "已关闭游戏内挂机防踢功能");
            }
        }
    }
}
