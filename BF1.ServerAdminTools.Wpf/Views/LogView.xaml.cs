using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Helper;

namespace BF1.ServerAdminTools.Common.Views
{
    /// <summary>
    /// LogView.xaml 的交互逻辑
    /// </summary>
    public partial class LogView : UserControl
    {
        public static Action<BreakRuleInfo>? AddKickOKLog;
        public static Action<BreakRuleInfo>? AddKickNOLog;
        public static Action<ChangeTeamInfo>? AddChangeTeamLog;

        public LogView()
        {
            InitializeComponent();

            AddKickOKLog = FAddKickOKLog;
            AddKickNOLog = FAddKickNOLog;
            AddChangeTeamLog = FAddChangeTeamLog;
        }

        /////////////////////////////////////////////////////

        private void AppendKickOKLog(string msg)
        {
            TextBox_KickOKLog.AppendText(msg + "\n");
        }

        private void AppendKickNOLog(string msg)
        {
            TextBox_KickNOLog.AppendText(msg + "\n");
        }

        private void AppendChangeTeamLog(string msg)
        {
            TextBox_ChangeTeamLog.AppendText(msg + "\n");
        }

        /////////////////////////////////////////////////////

        private void FAddKickOKLog(BreakRuleInfo info)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                if (TextBox_KickOKLog.LineCount >= 1000)
                {
                    TextBox_KickOKLog.Clear();
                }

                AppendKickOKLog("操作时间: " + DateTime.Now.ToString());
                AppendKickOKLog("玩家ID: " + info.Name);
                AppendKickOKLog("玩家数字ID: " + info.PersonaId);
                AppendKickOKLog("踢出理由: " + info.Reason);
                AppendKickOKLog("状态: " + info.Status + "\n");

                Core.AddLog2SQLite(DataShell.KICKOK, info);
            });
        }

        private void FAddKickNOLog(BreakRuleInfo info)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                if (TextBox_KickNOLog.LineCount >= 1000)
                {
                    TextBox_KickNOLog.Clear();
                }

                AppendKickNOLog("操作时间: " + DateTime.Now.ToString());
                AppendKickNOLog("玩家ID: " + info.Name);
                AppendKickNOLog("玩家数字ID: " + info.PersonaId);
                AppendKickNOLog("踢出理由: " + info.Reason);
                AppendKickNOLog("状态: " + info.Status + "\n");

                Core.AddLog2SQLite(DataShell.KICKFAIL, info);
            });
        }

        private void FAddChangeTeamLog(ChangeTeamInfo info)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                if (TextBox_ChangeTeamLog.LineCount >= 1000)
                {
                    TextBox_ChangeTeamLog.Clear();
                }

                AppendChangeTeamLog("操作时间: " + DateTime.Now.ToString());
                AppendChangeTeamLog("玩家等级: " + info.Rank);
                AppendChangeTeamLog("玩家ID: " + info.Name);
                AppendChangeTeamLog("玩家数字ID: " + info.PersonaId);
                AppendChangeTeamLog("状态: " + info.Status + "\n");

                Core.AddLog2SQLite(info);
            });
        }
        private void MenuItem_ClearKickOKLog_Click(object sender, RoutedEventArgs e)
        {
            TextBox_KickOKLog.Clear();
            MainWindow.SetOperatingState(1, "清空踢人成功日志成功");
        }

        private void MenuItem_ClearKickNOLog_Click(object sender, RoutedEventArgs e)
        {
            TextBox_KickNOLog.Clear();
            MainWindow.SetOperatingState(1, "清空踢人失败日志成功");
        }

        private void MenuItem_ClearChangeTeamLog_Click(object sender, RoutedEventArgs e)
        {
            TextBox_ChangeTeamLog.Clear();
            MainWindow.SetOperatingState(1, "清空更换队伍日志成功");
        }
    }
}
