using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Extension;
using BF1.ServerAdminTools.Common.Models;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Common.Windows;
using BF1.ServerAdminTools.Wpf.Tasks;

namespace BF1.ServerAdminTools.Common.Views
{
    /// <summary>
    /// ScoreView.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreView : UserControl
    {
        public static ServerInfoModel ServerInfoModel { get; set; }
        public static PlayerOtherModel PlayerOtherModel { get; set; }

        public static ObservableCollection<PlayerListModel> DataGrid_PlayerList_Team1 { get; set; }
        public static ObservableCollection<PlayerListModel> DataGrid_PlayerList_Team2 { get; set; }

        private struct DataGridSelcContent
        {
            public bool IsOK;
            public int TeamID;
            public int Rank;
            public string Name;
            public long PersonaId;
        }

        private static DataGridSelcContent _dataGridSelcContent;

        ///////////////////////////////////////////////////////

        public ScoreView()
        {
            InitializeComponent();

            DataContext = this;

            ServerInfoModel = new ServerInfoModel();
            PlayerOtherModel = new PlayerOtherModel();

            DataGrid_PlayerList_Team1 = new ObservableCollection<PlayerListModel>();
            DataGrid_PlayerList_Team2 = new ObservableCollection<PlayerListModel>();
        }

        // 手动踢出违规玩家
        private async void KickPlayer(string reason)
        {
            if (!string.IsNullOrEmpty(Globals.Config.SessionId))
            {
                if (_dataGridSelcContent.IsOK)
                {
                    MainWindow.SetOperatingState(2, $"正在踢出玩家 {_dataGridSelcContent.Name} 中...");

                    var result = await ServerAPI.AdminKickPlayer(_dataGridSelcContent.PersonaId.ToString(), reason);

                    if (result.IsSuccess)
                    {
                        MainWindow.SetOperatingState(1, $"踢出玩家 {_dataGridSelcContent.Name} 成功  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                    else
                    {
                        MainWindow.SetOperatingState(3, $"踢出玩家 {_dataGridSelcContent.Name} 失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                }
                else
                {
                    MainWindow.SetOperatingState(2, "请选择正确的玩家");
                }
            }
            else
            {
                MainWindow.SetOperatingState(2, "请先获取玩家SessionID");
            }
        }
        private void MenuItem_Admin_KickPlayer_Custom_Click(object sender, RoutedEventArgs e)
        {
            // 右键菜单 踢出玩家 - 自定义理由
            if (!string.IsNullOrEmpty(Globals.Config.SessionId))
            {
                if (_dataGridSelcContent.IsOK)
                {
                    var customKickWindow = new CustomKickWindow(_dataGridSelcContent.Name, _dataGridSelcContent.PersonaId.ToString());
                    customKickWindow.Owner = MainWindow.ThisMainWindow;
                    customKickWindow.ShowDialog();
                }
                else
                {
                    MainWindow.SetOperatingState(2, "请选择正确的玩家");
                }
            }
            else
            {
                MainWindow.SetOperatingState(2, "请先获取玩家SessionID");
            }
        }

        private void MenuItem_Admin_KickPlayer_OffensiveBehavior_Click(object sender, RoutedEventArgs e)
        {
            // 右键菜单 踢出玩家 - 攻击性行为
            KickPlayer("OFFENSIVEBEHAVIOR");
        }

        private void MenuItem_Admin_KickPlayer_Latency_Click(object sender, RoutedEventArgs e)
        {
            // 右键菜单 踢出玩家 - 延迟
            KickPlayer("LATENCY");
        }

        private void MenuItem_Admin_KickPlayer_RuleViolation_Click(object sender, RoutedEventArgs e)
        {
            // 右键菜单 踢出玩家 - 违反规则
            KickPlayer("RULEVIOLATION");
        }

        private void MenuItem_Admin_KickPlayer_General_Click(object sender, RoutedEventArgs e)
        {
            // 右键菜单 踢出玩家 - 其他
            KickPlayer("GENERAL");
        }

        private async void MenuItem_Admin_ChangePlayerTeam_Click(object sender, RoutedEventArgs e)
        {
            // 右键菜单 更换玩家队伍
            if (!string.IsNullOrEmpty(Globals.Config.SessionId))
            {
                if (_dataGridSelcContent.IsOK)
                {
                    MainWindow.SetOperatingState(2, $"正在更换玩家 {_dataGridSelcContent.Name} 队伍中...");

                    var result = await ServerAPI.AdminMovePlayer(_dataGridSelcContent.PersonaId.ToString(), _dataGridSelcContent.TeamID.ToString());

                    if (result.IsSuccess)
                    {
                        MainWindow.SetOperatingState(1, $"更换玩家 {_dataGridSelcContent.Name} 队伍成功  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                    else
                    {
                        MainWindow.SetOperatingState(3, $"更换玩家 {_dataGridSelcContent.Name} 队伍失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                }
                else
                {
                    MainWindow.SetOperatingState(2, "请选择正确的玩家，操作取消");
                }
            }
            else
            {
                MainWindow.SetOperatingState(2, "请先获取玩家SessionID后，再执行本操作");
            }
        }

        private void MenuItem_CopyPlayerName_Click(object sender, RoutedEventArgs e)
        {
            if (_dataGridSelcContent.IsOK)
            {
                // 复制玩家ID（无队标）
                Clipboard.SetText(_dataGridSelcContent.Name);
                MainWindow.SetOperatingState(1, $"复制玩家ID {_dataGridSelcContent.Name} 到剪切板成功");
            }
            else
            {
                MainWindow.SetOperatingState(2, "请选择正确的玩家，操作取消");
            }
        }

        private void MenuItem_CopyPlayerName_PID_Click(object sender, RoutedEventArgs e)
        {
            if (_dataGridSelcContent.IsOK)
            {
                // 复制玩家数字ID
                Clipboard.SetText(_dataGridSelcContent.PersonaId.ToString());
                MainWindow.SetOperatingState(1, $"复制玩家数字ID {_dataGridSelcContent.PersonaId} 到剪切板成功");
            }
            else
            {
                MainWindow.SetOperatingState(2, "请选择正确的玩家，操作取消");
            }
        }

        private void MenuItem_QueryPlayerRecord_Click(object sender, RoutedEventArgs e)
        {
            if (_dataGridSelcContent.IsOK)
            {
                // 查询玩家战绩
                MainWindow.TabControlSelect(1);
                QueryView._QuickQueryPalyer(_dataGridSelcContent.Name);
            }
            else
            {
                MainWindow.SetOperatingState(2, "请选择正确的玩家，操作取消");
            }
        }

        private void MenuItem_QueryPlayerRecordWeb_BT_Click(object sender, RoutedEventArgs e)
        {
            // 查询玩家战绩（BT）
            if (_dataGridSelcContent.IsOK)
            {
                string playerName = _dataGridSelcContent.Name;

                ProcessUtils.OpenLink(@"https://battlefieldtracker.com/bf1/profile/pc/" + playerName);
                MainWindow.SetOperatingState(1, $"查询玩家（{_dataGridSelcContent.Name}）战绩成功，请前往浏览器查看");
            }
            else
            {
                MainWindow.SetOperatingState(2, "请选择正确的玩家，操作取消");
            }
        }

        private void MenuItem_QueryPlayerRecordWeb_GT_Click(object sender, RoutedEventArgs e)
        {
            // 查询玩家战绩（GT）
            if (_dataGridSelcContent.IsOK)
            {
                string playerName = _dataGridSelcContent.Name;

                ProcessUtils.OpenLink(@"https://gametools.network/stats/pc/name/" + playerName + "?game=bf1");
                MainWindow.SetOperatingState(1, $"查询玩家（{_dataGridSelcContent.Name}）战绩成功，请前往浏览器查看");
            }
            else
            {
                MainWindow.SetOperatingState(2, "请选择正确的玩家，操作取消");
            }
        }

        private void MenuItem_ClearScoreSort_Click(object sender, RoutedEventArgs e)
        {
            // 清理得分板标题排序

            Dispatcher.BeginInvoke(new Action(delegate
            {
                CollectionViewSource.GetDefaultView(DataGrid_Team1.ItemsSource).SortDescriptions.Clear();
                CollectionViewSource.GetDefaultView(DataGrid_Team2.ItemsSource).SortDescriptions.Clear();

                MainWindow.SetOperatingState(1, "清理得分板标题排序成功（默认为玩家得分从高到低排序）");
            }));
        }

        private void MenuItem_ShowWeaponNameZHCN_Click(object sender, RoutedEventArgs e)
        {
            // 显示中文武器名称（参考）
            var item = sender as MenuItem;
            if (item != null)
            {
                if (item.IsChecked)
                {
                    DataSave.IsShowCHSWeaponName = true;
                    MainWindow.SetOperatingState(1, $"当前得分板正在显示中文武器名称");
                }
                else
                {
                    DataSave.IsShowCHSWeaponName = false;
                    MainWindow.SetOperatingState(1, $"当前得分板正在显示英文武器名称");
                }
            }
        }
        private void DataGrid_Team1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = DataGrid_Team1.SelectedItem as PlayerListModel;
            if (item != null)
            {
                _dataGridSelcContent.IsOK = true;
                _dataGridSelcContent.TeamID = 1;
                _dataGridSelcContent.Rank = item.Rank;
                _dataGridSelcContent.Name = item.Name;
                _dataGridSelcContent.PersonaId = item.PersonaId;
            }
            else
            {
                _dataGridSelcContent.IsOK = false;
                _dataGridSelcContent.TeamID = -1;
                _dataGridSelcContent.Rank = -1;
                _dataGridSelcContent.Name = string.Empty;
                _dataGridSelcContent.PersonaId = -1;
            }

            Update_DateGrid_Selection();
        }

        private void DataGrid_Team2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = DataGrid_Team2.SelectedItem as PlayerListModel;
            if (item != null)
            {
                _dataGridSelcContent.IsOK = true;
                _dataGridSelcContent.TeamID = 2;
                _dataGridSelcContent.Rank = item.Rank;
                _dataGridSelcContent.Name = item.Name;
                _dataGridSelcContent.PersonaId = item.PersonaId;
            }
            else
            {
                _dataGridSelcContent.IsOK = false;
                _dataGridSelcContent.TeamID = -1;
                _dataGridSelcContent.Rank = -1;
                _dataGridSelcContent.Name = string.Empty;
                _dataGridSelcContent.PersonaId = -1;
            }

            Update_DateGrid_Selection();
        }

        private void Update_DateGrid_Selection()
        {
            StringBuilder sb = new();

            if (_dataGridSelcContent.IsOK)
            {
                sb.Append($"玩家ID : {_dataGridSelcContent.Name}");
                sb.Append($"  |  玩家队伍ID : {_dataGridSelcContent.TeamID}");
                sb.Append($"  |  玩家等级 : {_dataGridSelcContent.Rank}");
                sb.Append($"  |  更新时间 : {DateTime.Now}");
            }
            else
            {
                sb.Append($"当前未选中任何玩家");
                sb.Append($"  |  更新时间 : {DateTime.Now}");
            }

            TextBlock_DataGridSelectionContent.Text = sb.ToString();
        }
    }
}
