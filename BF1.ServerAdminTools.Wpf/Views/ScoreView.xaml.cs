using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Extension;
using BF1.ServerAdminTools.Common.Models;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Common.Windows;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Tasks;

namespace BF1.ServerAdminTools.Common.Views
{
    /// <summary>
    /// ScoreView.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreView : UserControl
    {
        public static Semaphore Semaphore = new(0, 5);
        public ServerInfoModel ServerInfoModel { get; set; }
        public PlayerOtherModel PlayerOtherModel { get; set; }

        public ObservableCollection<PlayerListModel> DataGrid_PlayerList_Team1 { get; set; }
        public ObservableCollection<PlayerListModel> DataGrid_PlayerList_Team2 { get; set; }

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

            new Thread(UpdatePlayerList)
            {
                Name = "UpdateThread",
                IsBackground = true
            }.Start();
        }

        private void UpdatePlayerList()
        {
            while (true)
            {
                Semaphore.WaitOne();
                //////////////////////////////// 自己数据 ////////////////////////////////

                PlayerOtherModel.MySelfTeamID = $"队伍ID : {Globals.LocalPlayer.TeamID}";

                if (string.IsNullOrWhiteSpace(Globals.LocalPlayer.PlayerName))
                {
                    PlayerOtherModel.MySelfName = "玩家ID : 未知";
                }
                else
                {
                    PlayerOtherModel.MySelfName = $"玩家ID : {Globals.LocalPlayer.PlayerName}";
                }

                ////////////////////////////////////////////////////////////////////////////////

                ServerInfoModel.ServerName = $"服务器名称 : {Globals.ServerHook.ServerName}  |  GameID : {Globals.ServerHook.ServerID}";

                ServerInfoModel.ServerTime = Globals.ServerHook.ServerTimeS = PlayerUtil.SecondsToMMSS(Globals.ServerHook.ServerTime);

                if (Globals.ServerHook.Team1Score >= 0 && Globals.ServerHook.Team1Score <= 1000 &&
                    Globals.ServerHook.Team2Score >= 0 && Globals.ServerHook.Team2Score <= 1000)
                {
                    ServerInfoModel.Team1ScoreWidth = Globals.ServerHook.Team1Score / 6.25;
                    ServerInfoModel.Team2ScoreWidth = Globals.ServerHook.Team2Score / 6.25;

                    ServerInfoModel.Team1Score = $"{Globals.ServerHook.Team1Score}";
                    ServerInfoModel.Team2Score = $"{Globals.ServerHook.Team2Score}";
                }
                else if (Globals.ServerHook.Team1Score > 1000 && Globals.ServerHook.Team1Score <= 2000 ||
                    Globals.ServerHook.Team2Score > 1000 && Globals.ServerHook.Team2Score <= 2000)
                {
                    ServerInfoModel.Team1ScoreWidth = Globals.ServerHook.Team1Score / 12.5;
                    ServerInfoModel.Team2ScoreWidth = Globals.ServerHook.Team2Score / 12.5;

                    ServerInfoModel.Team1Score = $"{Globals.ServerHook.Team1Score}";
                    ServerInfoModel.Team2Score = $"{Globals.ServerHook.Team2Score}";
                }
                else
                {
                    ServerInfoModel.Team1ScoreWidth = 0;
                    ServerInfoModel.Team2ScoreWidth = 0;

                    ServerInfoModel.Team1Score = "0";
                    ServerInfoModel.Team2Score = "0";
                }

                ServerInfoModel.Team1FromeFlag = $"从旗帜获取的得分 : {Globals.ServerHook.Team1FromeFlag}";
                ServerInfoModel.Team1FromeKill = $"从击杀获取的得分 : {Globals.ServerHook.Team1FromeKill}";

                ServerInfoModel.Team2FromeFlag = $"从旗帜获取的得分 : {Globals.ServerHook.Team2FromeFlag}";
                ServerInfoModel.Team2FromeKill = $"从击杀获取的得分 : {Globals.ServerHook.Team2FromeKill}";

                ServerInfoModel.Team1Info = $"已部署/队伍1人数 : {Globals.StatisticData_Team1.PlayerCount} / {Globals.StatisticData_Team1.MaxPlayerCount}  |  150等级人数 : {Globals.StatisticData_Team1.Rank150PlayerCount}  |  总击杀数 : {Globals.StatisticData_Team1.AllKillCount}  |  总死亡数 : {Globals.StatisticData_Team1.AllDeadCount}";
                ServerInfoModel.Team2Info = $"已部署/队伍2人数 : {Globals.StatisticData_Team2.PlayerCount} / {Globals.StatisticData_Team2.MaxPlayerCount}  |  150等级人数 : {Globals.StatisticData_Team2.Rank150PlayerCount}  |  总击杀数 : {Globals.StatisticData_Team2.AllKillCount}  |  总死亡数 : {Globals.StatisticData_Team2.AllDeadCount}";

                PlayerOtherModel.ServerPlayerCountInfo = $"服务器总人数 : {Globals.StatisticData_Team1.MaxPlayerCount + Globals.StatisticData_Team2.MaxPlayerCount}";

                ////////////////////////////////////////////////////////////////////////////////

                Application.Current?.Dispatcher.Invoke(() =>
                {
                    UpdateDataGridTeam1();
                    UpdateDataGridTeam2();

                    DataGrid_PlayerList_Team1.Sort();
                    DataGrid_PlayerList_Team2.Sort();
                });

                TaskTick.Done();
            }
        }

        // 更新 DataGrid 队伍1
        private void UpdateDataGridTeam1()
        {
            if (Globals.PlayerDatas_Team1.Count == 0 && DataGrid_PlayerList_Team1.Count != 0)
            {
                DataGrid_PlayerList_Team1.Clear();
            }

            if (Globals.PlayerDatas_Team1.Count != 0)
            {
                // 更新DataGrid中现有的玩家数据，并把DataGrid中已经不在服务器的玩家清除
                List<PlayerListModel> list = new();
                foreach (var item in DataGrid_PlayerList_Team1)
                {
                    if (Globals.PlayerDatas_Team1.ContainsKey(item.PersonaId))
                    {
                        item.Rank = Globals.PlayerDatas_Team1[item.PersonaId].Rank;
                        item.Clan = Globals.PlayerDatas_Team1[item.PersonaId].Clan;
                        item.Admin = Globals.PlayerDatas_Team1[item.PersonaId].Admin ? "✔" : "";
                        item.VIP = Globals.PlayerDatas_Team1[item.PersonaId].VIP ? "✔" : "";
                        item.SquadId = Globals.PlayerDatas_Team1[item.PersonaId].SquadId;
                        item.Kill = Globals.PlayerDatas_Team1[item.PersonaId].Kill;
                        item.Dead = Globals.PlayerDatas_Team1[item.PersonaId].Dead;
                        item.KD = Globals.PlayerDatas_Team1[item.PersonaId].KD.ToString("0.00");
                        item.KPM = Globals.PlayerDatas_Team1[item.PersonaId].KPM.ToString("0.00");
                        item.Score = Globals.PlayerDatas_Team1[item.PersonaId].Score;
                        item.WeaponS0 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS0CH :
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS0;
                        item.WeaponS1 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS1CH :
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS1;
                        item.WeaponS2 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS2CH :
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS2;
                        item.WeaponS3 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS3CH :
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS3;
                        item.WeaponS4 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS4CH :
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS4;
                        item.WeaponS5 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS5CH :
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS5;
                        item.WeaponS6 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS6CH :
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS6;
                        item.WeaponS7 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS7CH :
                           Globals.PlayerDatas_Team1[item.PersonaId].WeaponS7;
                    }
                    else
                    {
                        list.Add(item);
                    }
                }

                list.ForEach(item => DataGrid_PlayerList_Team1.Remove(item));

                // 增加DataGrid没有的玩家数据
                foreach (var item in Globals.PlayerDatas_Team1.Values)
                {
                    if (!DataGrid_PlayerList_Team1.Where(val => val.Name == item.Name).Any())
                    {
                        DataGrid_PlayerList_Team1.Add(new PlayerListModel()
                        {
                            Rank = item.Rank,
                            Clan = item.Clan,
                            Name = item.Name,
                            PersonaId = item.PersonaId,
                            Admin = item.Admin ? "✔" : "",
                            VIP = item.VIP ? "✔" : "",
                            SquadId = item.SquadId,
                            Kill = item.Kill,
                            Dead = item.Dead,
                            KD = item.KD.ToString("0.00"),
                            KPM = item.KPM.ToString("0.00"),
                            Score = item.Score,
                            WeaponS0 = DataSave.IsShowCHSWeaponName ? item.WeaponS0CH : item.WeaponS0,
                            WeaponS1 = DataSave.IsShowCHSWeaponName ? item.WeaponS1CH : item.WeaponS1,
                            WeaponS2 = DataSave.IsShowCHSWeaponName ? item.WeaponS2CH : item.WeaponS2,
                            WeaponS3 = DataSave.IsShowCHSWeaponName ? item.WeaponS3CH : item.WeaponS3,
                            WeaponS4 = DataSave.IsShowCHSWeaponName ? item.WeaponS4CH : item.WeaponS4,
                            WeaponS5 = DataSave.IsShowCHSWeaponName ? item.WeaponS5CH : item.WeaponS5,
                            WeaponS6 = DataSave.IsShowCHSWeaponName ? item.WeaponS6CH : item.WeaponS6,
                            WeaponS7 = DataSave.IsShowCHSWeaponName ? item.WeaponS7CH : item.WeaponS7,
                        });
                    }
                }

                // 修正序号
                for (int i = 0; i < DataGrid_PlayerList_Team1.Count; i++)
                {
                    DataGrid_PlayerList_Team1[i].Index = i + 1;
                }
            }
        }

        // 更新 DataGrid 队伍2
        private void UpdateDataGridTeam2()
        {
            if (Globals.PlayerDatas_Team2.Count == 0 && DataGrid_PlayerList_Team2.Count != 0)
            {
                DataGrid_PlayerList_Team2.Clear();
            }

            if (Globals.PlayerDatas_Team2.Count != 0)
            {
                // 更新DataGrid中现有的玩家数据，并把DataGrid中已经不在服务器的玩家清除
                List<PlayerListModel> list = new();
                foreach (var item in DataGrid_PlayerList_Team2)
                {
                    if (Globals.PlayerDatas_Team2.ContainsKey(item.PersonaId))
                    {
                        item.Rank = Globals.PlayerDatas_Team2[item.PersonaId].Rank;
                        item.Clan = Globals.PlayerDatas_Team2[item.PersonaId].Clan;
                        item.Admin = Globals.PlayerDatas_Team2[item.PersonaId].Admin ? "✔" : "";
                        item.VIP = Globals.PlayerDatas_Team2[item.PersonaId].VIP ? "✔" : "";
                        item.SquadId = Globals.PlayerDatas_Team2[item.PersonaId].SquadId;
                        item.Kill = Globals.PlayerDatas_Team2[item.PersonaId].Kill;
                        item.Dead = Globals.PlayerDatas_Team2[item.PersonaId].Dead;
                        item.KD = Globals.PlayerDatas_Team2[item.PersonaId].KD.ToString("0.00");
                        item.KPM = Globals.PlayerDatas_Team2[item.PersonaId].KPM.ToString("0.00");
                        item.Score = Globals.PlayerDatas_Team2[item.PersonaId].Score;
                        item.WeaponS0 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS0CH :
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS0;
                        item.WeaponS1 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS1CH :
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS1;
                        item.WeaponS2 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS2CH :
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS2;
                        item.WeaponS3 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS3CH :
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS3;
                        item.WeaponS4 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS4CH :
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS4;
                        item.WeaponS5 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS5CH :
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS5;
                        item.WeaponS6 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS6CH :
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS6;
                        item.WeaponS7 = DataSave.IsShowCHSWeaponName ?
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS7CH :
                           Globals.PlayerDatas_Team2[item.PersonaId].WeaponS7;
                    }
                    else
                    {
                        list.Add(item);
                    }
                }

                list.ForEach(item => DataGrid_PlayerList_Team2.Remove(item));

                // 增加DataGrid没有的玩家数据
                foreach (var item in Globals.PlayerDatas_Team2.Values)
                {
                    if (!DataGrid_PlayerList_Team2.Where(val => val.Name == item.Name).Any())
                    {
                        DataGrid_PlayerList_Team2.Add(new PlayerListModel()
                        {
                            Rank = item.Rank,
                            Clan = item.Clan,
                            Name = item.Name,
                            PersonaId = item.PersonaId,
                            Admin = item.Admin ? "✔" : "",
                            VIP = item.VIP ? "✔" : "",
                            SquadId = item.SquadId,
                            Kill = item.Kill,
                            Dead = item.Dead,
                            KD = item.KD.ToString("0.00"),
                            KPM = item.KPM.ToString("0.00"),
                            Score = item.Score,
                            WeaponS0 = DataSave.IsShowCHSWeaponName ? item.WeaponS0CH : item.WeaponS0,
                            WeaponS1 = DataSave.IsShowCHSWeaponName ? item.WeaponS1CH : item.WeaponS1,
                            WeaponS2 = DataSave.IsShowCHSWeaponName ? item.WeaponS2CH : item.WeaponS2,
                            WeaponS3 = DataSave.IsShowCHSWeaponName ? item.WeaponS3CH : item.WeaponS3,
                            WeaponS4 = DataSave.IsShowCHSWeaponName ? item.WeaponS4CH : item.WeaponS4,
                            WeaponS5 = DataSave.IsShowCHSWeaponName ? item.WeaponS5CH : item.WeaponS5,
                            WeaponS6 = DataSave.IsShowCHSWeaponName ? item.WeaponS6CH : item.WeaponS6,
                            WeaponS7 = DataSave.IsShowCHSWeaponName ? item.WeaponS7CH : item.WeaponS7,
                        });
                    }
                }

                // 修正序号
                for (int i = 0; i < DataGrid_PlayerList_Team2.Count; i++)
                {
                    DataGrid_PlayerList_Team2[i].Index = i + 1;
                }
            }
        }

        // 手动踢出违规玩家
        private async void KickPlayer(string reason)
        {
            if (!string.IsNullOrEmpty(Globals.Config.SessionId))
            {
                if (_dataGridSelcContent.IsOK)
                {
                    MainWindow._SetOperatingState(2, $"正在踢出玩家 {_dataGridSelcContent.Name} 中...");

                    var result = await ServerAPI.AdminKickPlayer(_dataGridSelcContent.PersonaId.ToString(), reason);

                    if (result.IsSuccess)
                    {
                        MainWindow._SetOperatingState(1, $"踢出玩家 {_dataGridSelcContent.Name} 成功  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                    else
                    {
                        MainWindow._SetOperatingState(3, $"踢出玩家 {_dataGridSelcContent.Name} 失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                }
                else
                {
                    MainWindow._SetOperatingState(2, "请选择正确的玩家");
                }
            }
            else
            {
                MainWindow._SetOperatingState(2, "请先获取玩家SessionID");
            }
        }

        #region 右键菜单事件
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
                    MainWindow._SetOperatingState(2, "请选择正确的玩家");
                }
            }
            else
            {
                MainWindow._SetOperatingState(2, "请先获取玩家SessionID");
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
                    MainWindow._SetOperatingState(2, $"正在更换玩家 {_dataGridSelcContent.Name} 队伍中...");

                    var result = await ServerAPI.AdminMovePlayer(_dataGridSelcContent.PersonaId.ToString(), _dataGridSelcContent.TeamID.ToString());

                    if (result.IsSuccess)
                    {
                        MainWindow._SetOperatingState(1, $"更换玩家 {_dataGridSelcContent.Name} 队伍成功  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                    else
                    {
                        MainWindow._SetOperatingState(3, $"更换玩家 {_dataGridSelcContent.Name} 队伍失败 {result.Message}  |  耗时: {result.ExecTime:0.00} 秒");
                    }
                }
                else
                {
                    MainWindow._SetOperatingState(2, "请选择正确的玩家，操作取消");
                }
            }
            else
            {
                MainWindow._SetOperatingState(2, "请先获取玩家SessionID后，再执行本操作");
            }
        }

        private void MenuItem_CopyPlayerName_Click(object sender, RoutedEventArgs e)
        {
            if (_dataGridSelcContent.IsOK)
            {
                // 复制玩家ID（无队标）
                Clipboard.SetText(_dataGridSelcContent.Name);
                MainWindow._SetOperatingState(1, $"复制玩家ID {_dataGridSelcContent.Name} 到剪切板成功");
            }
            else
            {
                MainWindow._SetOperatingState(2, "请选择正确的玩家，操作取消");
            }
        }

        private void MenuItem_CopyPlayerName_PID_Click(object sender, RoutedEventArgs e)
        {
            if (_dataGridSelcContent.IsOK)
            {
                // 复制玩家数字ID
                Clipboard.SetText(_dataGridSelcContent.PersonaId.ToString());
                MainWindow._SetOperatingState(1, $"复制玩家数字ID {_dataGridSelcContent.PersonaId} 到剪切板成功");
            }
            else
            {
                MainWindow._SetOperatingState(2, "请选择正确的玩家，操作取消");
            }
        }

        private void MenuItem_QueryPlayerRecord_Click(object sender, RoutedEventArgs e)
        {
            if (_dataGridSelcContent.IsOK)
            {
                // 查询玩家战绩
                MainWindow._TabControlSelect(1);
                QueryView._QuickQueryPalyer(_dataGridSelcContent.Name);
            }
            else
            {
                MainWindow._SetOperatingState(2, "请选择正确的玩家，操作取消");
            }
        }

        private void MenuItem_QueryPlayerRecordWeb_BT_Click(object sender, RoutedEventArgs e)
        {
            // 查询玩家战绩（BT）
            if (_dataGridSelcContent.IsOK)
            {
                string playerName = _dataGridSelcContent.Name;

                ProcessUtil.OpenLink(@"https://battlefieldtracker.com/bf1/profile/pc/" + playerName);
                MainWindow._SetOperatingState(1, $"查询玩家（{_dataGridSelcContent.Name}）战绩成功，请前往浏览器查看");
            }
            else
            {
                MainWindow._SetOperatingState(2, "请选择正确的玩家，操作取消");
            }
        }

        private void MenuItem_QueryPlayerRecordWeb_GT_Click(object sender, RoutedEventArgs e)
        {
            // 查询玩家战绩（GT）
            if (_dataGridSelcContent.IsOK)
            {
                string playerName = _dataGridSelcContent.Name;

                ProcessUtil.OpenLink(@"https://gametools.network/stats/pc/name/" + playerName + "?game=bf1");
                MainWindow._SetOperatingState(1, $"查询玩家（{_dataGridSelcContent.Name}）战绩成功，请前往浏览器查看");
            }
            else
            {
                MainWindow._SetOperatingState(2, "请选择正确的玩家，操作取消");
            }
        }

        private void MenuItem_ClearScoreSort_Click(object sender, RoutedEventArgs e)
        {
            // 清理得分板标题排序

            Dispatcher.BeginInvoke(new Action(delegate
            {
                CollectionViewSource.GetDefaultView(DataGrid_Team1.ItemsSource).SortDescriptions.Clear();
                CollectionViewSource.GetDefaultView(DataGrid_Team2.ItemsSource).SortDescriptions.Clear();

                MainWindow._SetOperatingState(1, "清理得分板标题排序成功（默认为玩家得分从高到低排序）");
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
                    MainWindow._SetOperatingState(1, $"当前得分板正在显示中文武器名称");
                }
                else
                {
                    DataSave.IsShowCHSWeaponName = false;
                    MainWindow._SetOperatingState(1, $"当前得分板正在显示英文武器名称");
                }
            }
        }
        #endregion

        #region DataGrid相关方法
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
        #endregion
    }
}
