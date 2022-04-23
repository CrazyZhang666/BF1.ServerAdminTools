using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.API.BF1Server.RespJson;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Helper;
using BF1.ServerAdminTools.Common.Models;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Common.Windows;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Tasks;

namespace BF1.ServerAdminTools.Common.Views
{
    /// <summary>
    /// RuleView.xaml 的交互逻辑
    /// </summary>
    public partial class RuleView : UserControl
    {
        /// <summary>
        /// 是否执行应用规则
        /// </summary>
        private bool isApplyRule = false;

        public RuleView()
        {
            InitializeComponent();

            // 添加武器信息列表
            foreach (var item in WeaponData.AllWeaponInfo)
            {
                ListBox_WeaponInfo.Items.Add(new WeaponInfoModel()
                {
                    English = item.English,
                    Chinese = item.Chinese,
                    Mark = ""
                });
            }
            ListBox_WeaponInfo.SelectedIndex = 0;

            new Thread(CheckState)
            {
                Name = "CheckStateThread",
                IsBackground = true
            }.Start();

            foreach (var item in DataSave.Rules)
            {
                Rule_List.Items.Add(item.Value);
            }

            DataSave.NowRule = DataSave.Rules["default"];

            LoadRule();

            MainWindow.ClosingDisposeEvent += MainWindow_ClosingDisposeEvent;
        }

        private void LoadRule()
        {
            Combo_Rule.SelectedItem = null;
            Combo_Rule.Items.Clear();

            NowName.Text = DataSave.NowRule.Name;

            if (DataSave.NowRule.Custom_WeaponList == null)
            {
                DataSave.NowRule.Custom_WeaponList = new();
            }

            if (DataSave.NowRule.Custom_BlackList == null)
            {
                DataSave.NowRule.Custom_BlackList = new();
            }

            if (DataSave.NowRule.Custom_WhiteList == null)
            {
                DataSave.NowRule.Custom_WhiteList = new();
            }

            Slider_MaxKill.Value = DataSave.NowRule.MaxKill;
            Slider_KDFlag.Value = DataSave.NowRule.KDFlag;
            Slider_MaxKD.Value = DataSave.NowRule.MaxKD;
            Slider_KPMFlag.Value = DataSave.NowRule.KPMFlag;
            Slider_MaxKPM.Value = DataSave.NowRule.MaxKPM;
            Slider_MinRank.Value = DataSave.NowRule.MinRank;
            Slider_MaxRank.Value = DataSave.NowRule.MaxRank;

            Slider_LifeMaxKD.Value = DataSave.NowRule.LifeMaxKD;
            Slider_LifeMaxKPM.Value = DataSave.NowRule.LifeMaxKPM;
            Slider_LifeMaxWeaponStar.Value = DataSave.NowRule.LifeMaxWeaponStar;
            Slider_LifeMaxVehicleStar.Value = DataSave.NowRule.LifeMaxVehicleStar;

            Slider_ScoreSwitchMap.Value = DataSave.NowRule.ScoreSwitchMap;
            Slider_ScoreStartSwitchMap.Value = DataSave.NowRule.ScoreStartSwitchMap;
            Slider_ScoreNotSwitchMap.Value = DataSave.NowRule.ScoreNotSwitchMap;
            Slider_SocreOtherRule.Value = DataSave.NowRule.ScoreOtherRule;

            if (DataSave.NowRule.SwitchMapType == 0)
            {
                RadioButton_SwitchMapSelect0.IsChecked = true;
            }
            else if (DataSave.NowRule.SwitchMapType == 1)
            {
                RadioButton_SwitchMapSelect1.IsChecked = true;
            }
            else if (DataSave.NowRule.SwitchMapType == 2)
            {
                RadioButton_SwitchMapSelect2.IsChecked = true;
            }

            Combo_Rule.Items.Add("");

            var self = DataSave.NowRule.Name.ToLower();
            foreach (var item in DataSave.Rules)
            {
                if (item.Key != self)
                {
                    Combo_Rule.Items.Add(item.Value.Name);
                }
            }

            if (!string.IsNullOrEmpty(DataSave.NowRule.OtherRule))
            {
                var temp = DataSave.NowRule.OtherRule.ToLower();
                if (!DataSave.Rules.ContainsKey(temp) || DataSave.NowRule.OtherRule == DataSave.NowRule.Name)
                {
                    DataSave.NowRule.OtherRule = "";
                }
            }

            Combo_Rule.SelectedItem = DataSave.NowRule.OtherRule;

            ListBox_BreakWeaponInfo.Items.Clear();
            foreach (var item in DataSave.NowRule.Custom_WeaponList)
            {
                ListBox_BreakWeaponInfo.Items.Add(new WeaponInfoModel()
                {
                    English = item,
                    Chinese = PlayerUtil.GetWeaponChsName(item)
                });
            }

            if (ListBox_BreakWeaponInfo.Items.Count != 0)
            {
                ListBox_BreakWeaponInfo.SelectedIndex = 0;
            }

            ListBox_Custom_BlackList.Items.Clear();
            foreach (var item in DataSave.NowRule.Custom_BlackList)
            {
                ListBox_Custom_BlackList.Items.Add(item);
            }

            ListBox_Custom_WhiteList.Items.Clear();
            foreach (var item in DataSave.NowRule.Custom_WhiteList)
            {
                ListBox_Custom_WhiteList.Items.Add(item);
            }

            foreach (WeaponInfoModel item in ListBox_WeaponInfo.Items)
            {
                foreach (WeaponInfoModel item1 in ListBox_BreakWeaponInfo.Items)
                {
                    if (item.English == item1.English)
                    {
                        item.Mark = "✔";
                    }
                    else
                    {
                        item.Mark = "";
                    }
                }
            }

            DataSave.AutoKickBreakPlayer = false;
            CheckBox_RunAutoKick.IsChecked = false;
        }

        private void MainWindow_ClosingDisposeEvent()
        {
            DataSave.NowRule.MaxKill = Convert.ToInt32(Slider_MaxKill.Value);
            DataSave.NowRule.KDFlag = Convert.ToInt32(Slider_KDFlag.Value);
            DataSave.NowRule.MaxKD = Convert.ToSingle(Slider_MaxKD.Value);
            DataSave.NowRule.KPMFlag = Convert.ToInt32(Slider_KPMFlag.Value);
            DataSave.NowRule.MaxKPM = Convert.ToSingle(Slider_MaxKPM.Value);
            DataSave.NowRule.MinRank = Convert.ToInt32(Slider_MinRank.Value);
            DataSave.NowRule.MaxRank = Convert.ToInt32(Slider_MaxRank.Value);
            DataSave.NowRule.LifeMaxKD = Convert.ToSingle(Slider_LifeMaxKD.Value);
            DataSave.NowRule.LifeMaxKPM = Convert.ToSingle(Slider_LifeMaxKPM.Value);
            DataSave.NowRule.LifeMaxWeaponStar = Convert.ToInt32(Slider_LifeMaxWeaponStar.Value);
            DataSave.NowRule.LifeMaxVehicleStar = Convert.ToInt32(Slider_LifeMaxVehicleStar.Value);
            DataSave.NowRule.ScoreSwitchMap = Convert.ToInt32(Slider_ScoreSwitchMap.Value);
            DataSave.NowRule.ScoreNotSwitchMap = Convert.ToInt32(Slider_ScoreNotSwitchMap.Value);
            DataSave.NowRule.ScoreStartSwitchMap = Convert.ToInt32(Slider_ScoreStartSwitchMap.Value);
            if (RadioButton_SwitchMapSelect0.IsChecked == true)
            {
                DataSave.NowRule.SwitchMapType = 0;
            }
            else if (RadioButton_SwitchMapSelect1.IsChecked == true)
            {
                DataSave.NowRule.SwitchMapType = 1;
            }
            else if (RadioButton_SwitchMapSelect2.IsChecked == true)
            {
                DataSave.NowRule.SwitchMapType = 2;
            }
            DataSave.NowRule.OtherRule = Combo_Rule.SelectedItem as string;
            DataSave.NowRule.ScoreOtherRule = Convert.ToInt32(Slider_SocreOtherRule.Value);

            DataSave.NowRule.Custom_WeaponList.Clear();
            foreach (WeaponInfoModel item in ListBox_BreakWeaponInfo.Items)
            {
                DataSave.NowRule.Custom_WeaponList.Add(item.English);
            }
            DataSave.NowRule.Custom_BlackList.Clear();
            foreach (string item in ListBox_Custom_BlackList.Items)
            {
                DataSave.NowRule.Custom_BlackList.Add(item);
            }
            DataSave.NowRule.Custom_WhiteList.Clear();
            foreach (string item in ListBox_Custom_WhiteList.Items)
            {
                DataSave.NowRule.Custom_WhiteList.Add(item);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////

        private void CheckState()
        {
            while (true)
            {
                if (!DataSave.AutoKickBreakPlayer)
                {
                    Dispatcher.Invoke(() =>
                    {
                        CheckBox_RunAutoKick.IsChecked = false;
                    });
                }

                if (string.IsNullOrEmpty(Globals.Config.GameId))
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (CheckBox_RunAutoKick.IsChecked == true)
                        {
                            CheckBox_RunAutoKick.IsChecked = false;
                            DataSave.AutoKickBreakPlayer = false;
                        }
                    });
                }

                Thread.Sleep(1000);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////

        private void Button_BreakWeaponInfo_Add_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            bool isContains = false;

            int index = ListBox_WeaponInfo.SelectedIndex;
            if (index != -1)
            {
                var wi = ListBox_WeaponInfo.SelectedItem as WeaponInfoModel;
                if (string.IsNullOrEmpty(wi.Chinese))
                {
                    MainWindow._SetOperatingState(2, "请不要把分类项添加到限制武器列表");
                    return;
                }

                foreach (WeaponInfoModel item in ListBox_BreakWeaponInfo.Items)
                {
                    if (wi.English == item.English)
                    {
                        isContains = true;
                        break;
                    }
                }

                foreach (var item in ListBox_BreakWeaponInfo.Items)
                {
                    if (ListBox_WeaponInfo.SelectedItem == item)
                    {
                        isContains = true;
                        break;
                    }
                }

                if (!isContains)
                {
                    ListBox_BreakWeaponInfo.Items.Add(ListBox_WeaponInfo.SelectedItem);
                    (ListBox_WeaponInfo.Items[ListBox_WeaponInfo.SelectedIndex] as WeaponInfoModel).Mark = "✔";

                    ListBox_WeaponInfo.SelectedIndex = index;

                    int count = ListBox_BreakWeaponInfo.Items.Count;
                    if (count != 0)
                    {
                        ListBox_BreakWeaponInfo.SelectedIndex = count - 1;
                    }

                    MainWindow._SetOperatingState(1, "添加限制武器成功");
                }
                else
                {
                    MainWindow._SetOperatingState(2, "当前限制武器已存在，请不要重复添加");
                }
            }
            else
            {
                MainWindow._SetOperatingState(2, "请选择正确的内容");
            }
        }

        private void Button_BreakWeaponInfo_Remove_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            int index1 = ListBox_WeaponInfo.SelectedIndex;
            int index2 = ListBox_BreakWeaponInfo.SelectedIndex;
            if (index2 != -1)
            {
                var bwi = ListBox_BreakWeaponInfo.SelectedItem as WeaponInfoModel;
                foreach (WeaponInfoModel item in ListBox_WeaponInfo.Items)
                {
                    if (item.English == bwi.English)
                    {
                        item.Mark = "";
                    }
                }

                ListBox_BreakWeaponInfo.Items.RemoveAt(ListBox_BreakWeaponInfo.SelectedIndex);

                int count = ListBox_BreakWeaponInfo.Items.Count;
                if (count != 0)
                {
                    ListBox_BreakWeaponInfo.SelectedIndex = count - 1;
                }

                ListBox_WeaponInfo.SelectedIndex = index1;

                MainWindow._SetOperatingState(1, "移除限制武器成功");
            }
            else
            {
                MainWindow._SetOperatingState(2, "请选择正确的内容");
            }
        }

        private void Button_BreakWeaponInfo_Clear_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            int index = ListBox_WeaponInfo.SelectedIndex;

            // 清空限制武器列表
            DataSave.NowRule.Custom_WeaponList.Clear();
            ListBox_BreakWeaponInfo.Items.Clear();

            foreach (WeaponInfoModel item in ListBox_WeaponInfo.Items)
            {
                item.Mark = "";
            }

            ListBox_WeaponInfo.SelectedIndex = index;

            MainWindow._SetOperatingState(1, "清空限制武器列表成功");
        }

        private void AppendLog(string msg)
        {
            TextBox_RuleLog.AppendText(msg + "\n");
        }

        private void Add_Rule(object sender, RoutedEventArgs e)
        {
            var name = new InputWindow("新的规则名字", "请输入新的规则名字", "").Set().Trim();
            if (string.IsNullOrWhiteSpace(name))
                return;

            if (DataSave.Rules.ContainsKey(name))
            {
                MessageBox.Show("改名字已被占用");
                return;
            }

            var rule = new ServerRule()
            {
                Name = name
            };

            DataSave.Rules.Add(name.ToLower(), rule);
            Rule_List.Items.Add(rule);
            ConfigUtil.SaveRule(rule);

            LoadRule();

            Rule_List.SelectedItem = null;
        }

        private void Load_Rule(object sender, RoutedEventArgs e)
        {
            var item = Rule_List.SelectedItem as ServerRule;

            if (item == null)
                return;

            DataSave.NowRule = item;
            LoadRule();
            isApplyRule = false;
            Rule_List.SelectedItem = null;

            TextBox_RuleLog.Clear();

            AppendLog("已切换规则");
        }

        private void Delete_Rule(object sender, RoutedEventArgs e)
        {
            var item = Rule_List.SelectedItem as ServerRule;

            if (item == null)
                return;

            if (item.Name is "Default")
            {
                MessageBox.Show(messageBoxText: "不能删除默认规则");
                return;
            }

            if (DataSave.NowRule == item)
            {
                DataSave.NowRule = DataSave.Rules["default"];
            }

            LoadRule();

            var name = item.Name.ToLower().Trim();

            DataSave.Rules.Remove(name);
            Rule_List.Items.Remove(item);
            ConfigUtil.DeleteRule(name);

            Rule_List.SelectedItem = null;
        }

        private void Button_ApplyRule_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            TextBox_RuleLog.Clear();

            AppendLog("===== 操作时间 =====");
            AppendLog("");
            AppendLog($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
            AppendLog("");

            DataSave.NowRule.MaxKill = Convert.ToInt32(Slider_MaxKill.Value);
            DataSave.NowRule.KDFlag = Convert.ToInt32(Slider_KDFlag.Value);
            DataSave.NowRule.MaxKD = Convert.ToSingle(Slider_MaxKD.Value);
            DataSave.NowRule.KPMFlag = Convert.ToInt32(Slider_KPMFlag.Value);
            DataSave.NowRule.MaxKPM = Convert.ToSingle(Slider_MaxKPM.Value);
            DataSave.NowRule.MinRank = Convert.ToInt32(Slider_MinRank.Value);
            DataSave.NowRule.MaxRank = Convert.ToInt32(Slider_MaxRank.Value);
            DataSave.NowRule.LifeMaxKD = Convert.ToSingle(Slider_LifeMaxKD.Value);
            DataSave.NowRule.LifeMaxKPM = Convert.ToSingle(Slider_LifeMaxKPM.Value);
            DataSave.NowRule.LifeMaxWeaponStar = Convert.ToInt32(Slider_LifeMaxWeaponStar.Value);
            DataSave.NowRule.LifeMaxVehicleStar = Convert.ToInt32(Slider_LifeMaxVehicleStar.Value);
            DataSave.NowRule.ScoreSwitchMap = Convert.ToInt32(Slider_ScoreSwitchMap.Value);
            DataSave.NowRule.ScoreNotSwitchMap = Convert.ToInt32(Slider_ScoreNotSwitchMap.Value);
            DataSave.NowRule.ScoreStartSwitchMap = Convert.ToInt32(Slider_ScoreStartSwitchMap.Value);
            if (RadioButton_SwitchMapSelect0.IsChecked == true)
            {
                DataSave.NowRule.SwitchMapType = 0;
            }
            else if (RadioButton_SwitchMapSelect1.IsChecked == true)
            {
                DataSave.NowRule.SwitchMapType = 1;
            }
            else if (RadioButton_SwitchMapSelect2.IsChecked == true)
            {
                DataSave.NowRule.SwitchMapType = 2;
            }
            DataSave.NowRule.OtherRule = Combo_Rule.SelectedItem as string;
            DataSave.NowRule.ScoreOtherRule = Convert.ToInt32(Slider_SocreOtherRule.Value);

            if (DataSave.NowRule.MinRank >= DataSave.NowRule.MaxRank && DataSave.NowRule.MinRank != 0 && DataSave.NowRule.MaxRank != 0)
            {
                Globals.IsRuleSetRight = false;
                isApplyRule = false;

                AppendLog($"限制等级规则设置不正确");
                AppendLog("");

                MainWindow._SetOperatingState(3, $"限制等级规则设置不正确");

                return;
            }

            /////////////////////////////////////////////////////////////////////////////

            // 清空限制武器列表
            DataSave.NowRule.Custom_WeaponList.Clear();
            // 添加自定义限制武器
            foreach (var item in ListBox_BreakWeaponInfo.Items)
            {
                DataSave.NowRule.Custom_WeaponList.Add((item as WeaponInfoModel).English);
            }

            // 清空黑名单列表
            DataSave.NowRule.Custom_BlackList.Clear();
            // 添加自定义黑名单列表
            foreach (var item in ListBox_Custom_BlackList.Items)
            {
                DataSave.NowRule.Custom_BlackList.Add(item as string);
            }

            // 清空白名单列表
            DataSave.NowRule.Custom_WhiteList.Clear();
            // 添加自定义白名单列表
            foreach (var item in ListBox_Custom_WhiteList.Items)
            {
                DataSave.NowRule.Custom_WhiteList.Add(item as string);
            }

            if (CheckBox_RunAutoKick.IsChecked == true)
            {
                CheckBox_RunAutoKick.IsChecked = false;
                DataSave.AutoKickBreakPlayer = false;
            }

            Globals.IsRuleSetRight = true;
            isApplyRule = true;

            AppendLog($"成功提交当前规则，请重新启动自动踢人功能");
            AppendLog("");

            MainWindow._SetOperatingState(1, $"应用当前规则成功，请点击<查询当前规则>检验规则是否正确");

            ConfigUtil.SaveRule();
        }

        private void Button_QueryRule_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            TextBox_RuleLog.Clear();

            AppendLog("===== 查询时间 =====");
            AppendLog("");
            AppendLog($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
            AppendLog("");

            AppendLog($"规则名字 : {DataSave.NowRule.Name}");
            AppendLog("");

            AppendLog($"玩家最高击杀限制 : {DataSave.NowRule.MaxKill}");
            AppendLog("");

            AppendLog($"计算玩家KD的最低击杀数 : {DataSave.NowRule.KDFlag}");
            AppendLog($"玩家最高KD限制 : {DataSave.NowRule.MaxKD}");
            AppendLog("");

            AppendLog($"计算玩家KPM的最低击杀数 : {DataSave.NowRule.KPMFlag}");
            AppendLog($"玩家最高KPM限制 : {DataSave.NowRule.MaxKPM}");
            AppendLog("");

            AppendLog($"玩家最低等级限制 : {DataSave.NowRule.MinRank}");
            AppendLog($"玩家最高等级限制 : {DataSave.NowRule.MaxRank}");
            AppendLog("");

            AppendLog($"玩家最高生涯KD限制 : {DataSave.NowRule.LifeMaxKD}");
            AppendLog($"玩家最高生涯KPM限制 : {DataSave.NowRule.LifeMaxKPM}");
            AppendLog("");

            AppendLog($"玩家最高生涯武器星数限制 : {DataSave.NowRule.LifeMaxWeaponStar}");
            AppendLog($"玩家最高生涯载具星数限制 : {DataSave.NowRule.LifeMaxVehicleStar}");
            AppendLog("\n");

            AppendLog($"========== 禁武器列表 ==========");
            AppendLog("");
            foreach (var item in DataSave.NowRule.Custom_WeaponList)
            {
                AppendLog($"武器名称 {DataSave.NowRule.Custom_WeaponList.IndexOf(item) + 1} : {item}");
            }
            AppendLog("\n");

            AppendLog($"========== 黑名单列表 ==========");
            AppendLog("");
            foreach (var item in DataSave.NowRule.Custom_BlackList)
            {
                AppendLog($"玩家ID {DataSave.NowRule.Custom_BlackList.IndexOf(item) + 1} : {item}");
            }
            AppendLog("\n");

            AppendLog($"========== 白名单列表 ==========");
            AppendLog("");
            foreach (var item in DataSave.NowRule.Custom_WhiteList)
            {
                AppendLog($"玩家ID {DataSave.NowRule.Custom_WhiteList.IndexOf(item) + 1} : {item}");
            }
            AppendLog("\n");

            if (DataSave.NowRule.ScoreSwitchMap != 0)
            {
                AppendLog($"========== 自动切图 ==========");
                AppendLog("");
                AppendLog($"分数差距达到{DataSave.NowRule.ScoreSwitchMap}自动换图");
                AppendLog($"劣势方分数达到{DataSave.NowRule.ScoreStartSwitchMap}才生效");
                if (DataSave.NowRule.ScoreNotSwitchMap != 0)
                {
                    AppendLog($"且一方分数达到{DataSave.NowRule.ScoreNotSwitchMap}不再自动换图");
                    AppendLog("\n");
                }
            }

            if (DataSave.NowRule.ScoreOtherRule != 0 && !string.IsNullOrWhiteSpace(DataSave.NowRule.OtherRule))
            {
                AppendLog($"========== 劣势方规则 ==========");
                AppendLog("");
                AppendLog($"分数差距达到{DataSave.NowRule.ScoreOtherRule}劣势方使用规则{DataSave.NowRule.OtherRule}");
                AppendLog("\n");
                var rule = DataSave.Rules[DataSave.NowRule.OtherRule.ToLower()];

                AppendLog($"玩家最高击杀限制 : {rule.MaxKill}");
                AppendLog("");

                AppendLog($"计算玩家KD的最低击杀数 : {rule.KDFlag}");
                AppendLog($"玩家最高KD限制 : {rule.MaxKD}");
                AppendLog("\n");
            }

            MainWindow._SetOperatingState(1, $"查询当前规则成功，请点击<检查违规玩家>测试是否正确");
        }

        private bool isRun;

        private async void Button_CheckBreakRulePlayer_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            if (isRun)
                return;

            isRun = true;

            TextBox_RuleLog.Clear();

            AppendLog("===== 查询时间 =====");
            AppendLog("");
            AppendLog($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
            AppendLog("");

            if (!Globals.IsGameRun || !Globals.IsToolInit)
            {
                MainWindow._SetOperatingState(3, $"运行环境检查失败");
                AppendLog("运行环境检查失败");
                AppendLog("");
                return;
            }

            AppendLog("正在检查玩家");

            TaskCheckLife.NeedPause = true;
            TaskCheckRule.NeedPause = true;

            await Task.Run(() =>
            {
                var team1Player = new List<PlayerData>();

                lock (Globals.PlayerDatas_Team1)
                {
                    team1Player.AddRange(Globals.PlayerDatas_Team1.Values);
                }
                lock (Globals.PlayerDatas_Team2)
                {
                    team1Player.AddRange(Globals.PlayerDatas_Team2.Values);
                }

                foreach (var item in team1Player)
                {
                    Dispatcher.Invoke(() =>
                    {
                        AppendLog($"正在检查玩家: {item.Name}");
                    });
                    TaskCheckLife.CheckBreakLifePlayer(item);
                }

                TaskCheckRule.StartCheck();
            });

            TaskCheckLife.NeedPause = false;
            TaskCheckRule.NeedPause = false;

            int index = 1;
            AppendLog($"========== 违规类型 : 限制玩家最高击杀 ==========");
            AppendLog("");
            var list = TaskKick.NeedKick.Values.Where(item => item.Type is BreakType.Kill_Limit);
            foreach (var item in list)
            {
                AppendLog($"玩家ID {index++} : {item.Name}");
            }
            AppendLog("\n");

            index = 1;
            AppendLog($"========== 违规类型 : 限制玩家最高KD ==========");
            AppendLog("");
            list = TaskKick.NeedKick.Values.Where(item => item.Type is BreakType.KD_Limit);
            foreach (var item in list)
            {
                AppendLog($"玩家ID {index++} : {item.Name}");
            }
            AppendLog("\n");

            index = 1;
            AppendLog($"========== 违规类型 : 限制玩家最高KPM ==========");
            AppendLog("");
            list = TaskKick.NeedKick.Values.Where(item => item.Type is BreakType.KPM_Limit);
            foreach (var item in list)
            {
                AppendLog($"玩家ID {index++} : {item.Name}");
            }
            AppendLog("\n");

            index = 1;
            AppendLog($"========== 违规类型 : 限制玩家等级范围 ==========");
            AppendLog("");
            list = TaskKick.NeedKick.Values.Where(item => item.Type is BreakType.Rank_Limit);
            foreach (var item in list)
            {
                AppendLog($"玩家ID {index++} : {item.Name}");
            }
            AppendLog("\n");

            index = 1;
            AppendLog($"========== 违规类型 : 限制玩家使用武器 ==========");
            AppendLog("");
            list = TaskKick.NeedKick.Values.Where(item => item.Type is BreakType.Weapon_Limit);
            foreach (var item in list)
            {
                AppendLog($"玩家ID {index++} : {item.Name}");
            }
            AppendLog("\n");

            MainWindow._SetOperatingState(1, $"检查违规玩家成功，如果符合规则就可以勾选<激活自动踢出违规玩家>了");

            isRun = false;
        }

        private void Button_Add_BlackList_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            if (TextBox_BlackList_PlayerName.Text != "")
            {
                bool isContains = false;

                foreach (var item in ListBox_Custom_BlackList.Items)
                {
                    if ((item as string) == TextBox_BlackList_PlayerName.Text)
                    {
                        isContains = true;
                    }
                }

                if (!isContains)
                {
                    ListBox_Custom_BlackList.Items.Add(TextBox_BlackList_PlayerName.Text);

                    MainWindow._SetOperatingState(1, $"添加 {TextBox_BlackList_PlayerName.Text} 到黑名单列表成功");
                    TextBox_BlackList_PlayerName.Text = "";
                }
                else
                {
                    MainWindow._SetOperatingState(2, $"该项 {TextBox_BlackList_PlayerName.Text} 已经存在了，请不要重复添加");
                    TextBox_BlackList_PlayerName.Text = "";
                }
            }
            else
            {
                MainWindow._SetOperatingState(2, $"待添加黑名单玩家ID为空，添加操作取消");
            }
        }

        private void Button_Remove_BlackList_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            if (ListBox_Custom_BlackList.SelectedIndex != -1)
            {
                MainWindow._SetOperatingState(1, $"从黑名单列表删除（{ListBox_Custom_BlackList.SelectedItem}）成功");
                ListBox_Custom_BlackList.Items.Remove(ListBox_Custom_BlackList.SelectedItem);
            }
            else
            {
                MainWindow._SetOperatingState(2, $"请正确选中你要删除的玩家ID或自定义黑名单列表为空，删除操作取消");
            }
        }

        private void Button_Input_BlackList_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            var res = FileSelectUtil.FileSelect();
            if (res == null)
            {
                return;
            }

            try
            {
                var data = File.ReadAllText(res);

                // 清空黑名单列表
                DataSave.NowRule.Custom_BlackList.Clear();
                ListBox_Custom_BlackList.Items.Clear();

                var list = data.Split("\n");
                foreach (var item in list)
                {
                    var name = item.Trim();
                    if (string.IsNullOrWhiteSpace(name))
                        continue;

                    DataSave.NowRule.Custom_BlackList.Add(name);
                    ListBox_Custom_BlackList.Items.Add(name);
                }

                MainWindow._SetOperatingState(1, "导入黑名单列表成功");
            }
            catch (Exception ex)
            {
                MsgBoxUtil.ErrorMsgBox("导入黑名单时发生错误");
                Core.LogError("导入黑名单发生错误", ex);
                MainWindow._SetOperatingState(1, "导入黑名单列表发生错误");
                return;
            }
        }

        private void Button_Clear_BlackList_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            // 清空黑名单列表
            DataSave.NowRule.Custom_BlackList.Clear();
            ListBox_Custom_BlackList.Items.Clear();

            MainWindow._SetOperatingState(1, $"清空黑名单列表成功");
        }

        private void Button_Add_WhiteList_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            if (TextBox_WhiteList_PlayerName.Text != "")
            {
                bool isContains = false;

                foreach (var item in ListBox_Custom_WhiteList.Items)
                {
                    if ((item as string) == TextBox_WhiteList_PlayerName.Text)
                    {
                        isContains = true;
                    }
                }

                if (!isContains)
                {
                    ListBox_Custom_WhiteList.Items.Add(TextBox_WhiteList_PlayerName.Text);

                    MainWindow._SetOperatingState(1, $"添加 {TextBox_WhiteList_PlayerName.Text} 到白名单列表成功");

                    TextBox_WhiteList_PlayerName.Text = "";
                }
                else
                {
                    MainWindow._SetOperatingState(2, $"该项 {TextBox_WhiteList_PlayerName.Text} 已经存在了，请不要重复添加");
                    TextBox_WhiteList_PlayerName.Text = "";
                }
            }
            else
            {
                MainWindow._SetOperatingState(2, $"待添加白名单玩家ID为空，添加操作取消");
            }
        }

        private void Button_Remove_WhiteList_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            if (ListBox_Custom_WhiteList.SelectedIndex != -1)
            {
                MainWindow._SetOperatingState(1, $"从白名单列表删除（{ListBox_Custom_WhiteList.SelectedItem}）成功");
                ListBox_Custom_WhiteList.Items.Remove(ListBox_Custom_WhiteList.SelectedItem);
            }
            else
            {
                MainWindow._SetOperatingState(2, $"请正确选中你要删除的玩家ID或自定义白名单列表为空，删除操作取消");
            }
        }

        private void Button_Input_WhiteList_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            var res = FileSelectUtil.FileSelect();
            if (res == null)
            {
                return;
            }

            try
            {
                var data = File.ReadAllText(res);

                // 清空黑名单列表
                DataSave.NowRule.Custom_WhiteList.Clear();
                ListBox_Custom_WhiteList.Items.Clear();

                var list = data.Split("\n");
                foreach (var item in list)
                {
                    var name = item.Trim();
                    if (string.IsNullOrWhiteSpace(name))
                        continue;

                    DataSave.NowRule.Custom_WhiteList.Add(name);
                    ListBox_Custom_WhiteList.Items.Add(name);
                }

                MainWindow._SetOperatingState(1, "导入白名单列表成功");
            }
            catch (Exception ex)
            {
                MsgBoxUtil.ErrorMsgBox("导入白名单时发生错误");
                Core.LogError("导入白名单发生错误", ex);
                MainWindow._SetOperatingState(1, "导入白名单列表发生错误");
                return;
            }
        }

        private void Button_Clear_WhiteList_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            // 清空白名单列表
            DataSave.NowRule.Custom_WhiteList.Clear();
            ListBox_Custom_WhiteList.Items.Clear();

            MainWindow._SetOperatingState(1, $"清空白名单列表成功");
        }

        /// <summary>
        /// 检查自动踢人环境是否合格
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckKickEnv()
        {
            TextBox_RuleLog.Clear();

            MainWindow._SetOperatingState(2, $"正在检查环境...");

            AppendLog("===== 操作时间 =====");
            AppendLog("");
            AppendLog($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}");

            AppendLog("");
            AppendLog("正在检查游戏是否启动...");
            if (!Globals.IsGameRun)
            {
                if (!Core.IsGameRun())
                {
                    AppendLog("❌ 游戏没有启动，请先启动游戏");
                    return false;
                }
            }
            AppendLog("✔ 游戏已启动");

            AppendLog("");
            AppendLog("正在检查游戏是否启动...");
            if (!Globals.IsToolInit)
            {
                if (!Core.HookInit())
                {
                    AppendLog("❌ 战地1内存模块初始化失败，请检查游戏是否正常运行");
                    return false;
                }
            }
            AppendLog("✔ 战地1内存模块初始化完成");

            AppendLog("");
            AppendLog("正在检查玩家是否应用规则...");
            if (!isApplyRule)
            {
                AppendLog("❌ 玩家没有正确应用规则，请点击应用当前规则");
                MainWindow._SetOperatingState(2, $"环境检查未通过");
                return false;
            }
            AppendLog("✔ 玩家已正确应用规则");

            AppendLog("");
            AppendLog("正在检查 SessionId 是否正确...");
            if (string.IsNullOrEmpty(Globals.Config.SessionId))
            {
                AppendLog("❌ SessionId为空，操作取消");
                MainWindow._SetOperatingState(2, $"环境检查未通过");
                return false;
            }
            AppendLog("✔ SessionId 检查正确");

            AppendLog("");
            AppendLog("正在检查 SessionId 是否有效...");
            var result = await ServerAPI.GetWelcomeMessage();
            if (!result.IsSuccess)
            {
                AppendLog("❌ SessionId 已过期，请刷新SessionId");
                MainWindow._SetOperatingState(2, $"环境检查未通过");
                return false;
            }
            AppendLog("✔ SessionId 检查有效");

            AppendLog("");
            AppendLog("正在检查 GameId 是否正确...");
            if (string.IsNullOrEmpty(Globals.Config.GameId))
            {
                AppendLog("❌ GameId 为空，请先进入服务器");
                MainWindow._SetOperatingState(2, $"环境检查未通过");
                return false;
            }
            AppendLog("✔ GameId检查正确");

            AppendLog("");
            AppendLog("正在检查 服务器管理员列表 是否正确...");
            if (Globals.Server_AdminList.Count == 0)
            {
                await DetailView.SLoad();
                if (Globals.Server_AdminList.Count == 0)
                {
                    AppendLog("❌ 服务器管理员列表 为空");
                    MainWindow._SetOperatingState(2, $"环境检查未通过");
                    return false;
                }
            }
            AppendLog("✔ 服务器管理员列表 检查正确");

            AppendLog("");
            AppendLog("正在检查 玩家是否为当前服务器管理...");
            var welcomeMsg = JsonUtil.JsonDese<WelcomeMsg>(result.Message);
            var firstMessage = welcomeMsg.result.firstMessage;
            string playerName = firstMessage.Substring(0, firstMessage.IndexOf("，"));
            if (!Globals.Server_Admin2List.Contains(playerName))
            {
                AppendLog("❌ 玩家不是当前服务器管理，请确认服务器是否选择正确");
                MainWindow._SetOperatingState(2, $"环境检查未通过");
                return false;
            }
            AppendLog("✔ 已确认玩家为当前服务器管理");

            return true;
        }

        // 开启自动踢人
        private async void CheckBox_RunAutoKick_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_RunAutoKick.IsChecked == true)
            {
                // 检查自动踢人环境
                if (await CheckKickEnv())
                {
                    AppendLog("");
                    AppendLog("环境检查完毕，自动踢人已开启");

                    DataSave.AutoKickBreakPlayer = true;
                    MainWindow._SetOperatingState(1, $"自动踢人开启成功");
                    await Task.Run(() =>
                    {
                        Thread.Sleep(500);
                        Dispatcher.Invoke(() => { CheckBox_RunAutoKick.IsChecked = true; });
                    });
                }
                else
                {
                    CheckBox_RunAutoKick.IsChecked = false;
                    DataSave.AutoKickBreakPlayer = false;
                }
            }
            else
            {
                DataSave.AutoKickBreakPlayer = false;
                CheckBox_RunAutoKick.IsChecked = false;
                MainWindow._SetOperatingState(1, $"自动踢人关闭成功");
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ProcessUtil.OpenLink(e.Uri.OriginalString);
            e.Handled = true;
        }

        private void Button_OpenConfigurationFolder_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            ProcessUtil.OpenLink(ConfigLocal.Base);
        }

        private async void Button_ManualKickBreakRulePlayer_Click(object sender, RoutedEventArgs e)
        {
            AudioUtil.ClickSound();

            // 检查自动踢人环境
            if (await CheckKickEnv())
            {
                AppendLog("");
                AppendLog("环境检查完毕，正在执行手动踢人操作，这可能需要一点时间");
                AppendLog("请查看日志了解执行结果");

                TaskKick.Kick();

                MainWindow._SetOperatingState(1, "执行手动踢人操作成功，请查看日志了解执行结果");
            }
        }
    }
}
