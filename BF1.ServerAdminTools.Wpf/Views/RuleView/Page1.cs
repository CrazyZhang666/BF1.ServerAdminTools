using BF1.ServerAdminTools.Common;
using BF1.ServerAdminTools.Common.API.BF1Server;
using BF1.ServerAdminTools.Common.API.BF1Server.RespJson;
using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Helper;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Models;
using BF1.ServerAdminTools.Wpf.TaskList;
using BF1.ServerAdminTools.Wpf.Utils;
using BF1.ServerAdminTools.Wpf.Windows;

namespace BF1.ServerAdminTools.Wpf.Views;

public partial class RuleView
{
    private void Page1Load()
    {
        foreach (var item in DataSave.Rules)
        {
            RuleList.Items.Add(item.Value);
        }

        foreach (var item in DataSave.Config.MapRule)
        {
            if (!DataSave.Rules.TryGetValue(item.Value, out var rule))
                continue;
            MapRuleList.Items.Add(new MapRuleModel { Map = item.Key, Name = rule.Name });
        }
    }

    private void AddRule(object sender, RoutedEventArgs e)
    {
        var name = new InputWindow("新的规则名字", "请输入新的规则名字", "").Set().Trim();
        if (string.IsNullOrWhiteSpace(name))
            return;

        if (DataSave.Rules.ContainsKey(name))
        {
            MessageBox.Show("改名字已被占用");
            return;
        }

        var rule = new ServerRuleObj()
        {
            Name = name
        };

        DataSave.Rules.Add(name.ToLower(), rule);
        RuleList.Items.Add(rule);
        WpfConfigUtils.SaveRule(rule);

        LoadRule();
    }

    private void LoadRule(object sender, RoutedEventArgs e)
    {
        var item = RuleList.SelectedItem as ServerRuleObj;

        if (item == null)
            return;

        RuleList.SelectedItem = null;

        DataSave.NowRule = item;
        LoadRule();
        isApplyRule = false;

        RuleLog.Clear();

        AppendLog("已切换规则");
    }

    private void DeleteRule(object sender, RoutedEventArgs e)
    {
        var item = RuleList.SelectedItem as ServerRuleObj;

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
        RuleList.Items.Remove(item);
        WpfConfigUtils.DeleteRule(name);

        RuleList.SelectedItem = null;
    }

    private async void AddMapRule(object sender, RoutedEventArgs e)
    {
        if (Globals.ServerDetailed == null)
        {
            await Core.InitServerInfo();
        }

        if (Globals.ServerDetailed == null)
        {
            MsgBoxUtils.WarningMsgBox("服务器地图获取失败");
            return;
        }

        ServerRuleObj? rule = new MapRuleWindow().Set(out string? map);
        if (map == null || rule == null)
        {
            return;
        }

        string name = rule.Name.ToLower();
        TaskMapRule.NeedPause = true;

        if (DataSave.Config.MapRule.ContainsKey(map))
        {
            DataSave.Config.MapRule[map] = name;
            foreach (MapRuleModel item in MapRuleList.Items)
            {
                if (item.Map == map)
                {
                    item.Name = rule.Name;
                }
            }
        }
        else
        {
            DataSave.Config.MapRule.Add(map, name);
            MapRuleList.Items.Add(new MapRuleModel { Map = map, Name = rule.Name });
            WpfConfigUtils.SaveConfig();
        }

        TaskMapRule.NeedPause = false;

    }

    private void DeleteMapRule(object sender, RoutedEventArgs e)
    {
        if (MapRuleList.SelectedItem is not MapRuleModel item)
            return;
        TaskMapRule.NeedPause = true;

        DataSave.Config.MapRule.Remove(item.Map);
        WpfConfigUtils.SaveConfig();
        MapRuleList.Items.Remove(item);

        TaskMapRule.NeedPause = false;
    }

    private void LoadMapRule(object sender, RoutedEventArgs e)
    {
        if (MapRuleList.SelectedItem is not MapRuleModel item)
            return;

        ServerRuleObj? rule = new MapRuleWindow(item.Map, item.Name).Set(out string? map);
        if (rule == null || map == null)
            return;

        if (item.Name == rule.Name)
            return;

        TaskMapRule.NeedPause = true;

        DataSave.Config.MapRule[map] = rule.Name.ToLower();
        item.Name = rule.Name;
        WpfConfigUtils.SaveConfig();

        TaskMapRule.NeedPause = false;
    }

    private void EnableMapRuleChecked(object sender, RoutedEventArgs e)
    {
        if (!TaskMapRule.NeedPause)
        {
            TaskMapRule.NeedPause = true;
            Dispatcher.BeginInvoke(() =>
            {
                EnableMapRule.IsChecked = true;
            });
        }
        else
        {
            TaskMapRule.NeedPause = false;
            Dispatcher.BeginInvoke(() =>
            {
                EnableMapRule.IsChecked = false;
            });
        }
    }

    private void ApplyRuleClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        RuleLog.Clear();

        AppendLog($"===== 操作时间 =====\n{DateTime.Now:yyyy/MM/dd HH:mm:ss}\n");

        DataSave.NowRule.MaxKill = Convert.ToInt32(MaxKill.Value);
        DataSave.NowRule.KDFlag = Convert.ToInt32(KDFlag.Value);
        DataSave.NowRule.MaxKD = Convert.ToSingle(MaxKD.Value);
        DataSave.NowRule.KPMFlag = Convert.ToInt32(KPMFlag.Value);
        DataSave.NowRule.MaxKPM = Convert.ToSingle(MaxKPM.Value);
        DataSave.NowRule.MinRank = Convert.ToInt32(MinRank.Value);
        DataSave.NowRule.MaxRank = Convert.ToInt32(MaxRank.Value);
        DataSave.NowRule.LifeMaxKD = Convert.ToSingle(LifeMaxKD.Value);
        DataSave.NowRule.LifeMaxKPM = Convert.ToSingle(LifeMaxKPM.Value);
        DataSave.NowRule.LifeMaxWeaponStar = Convert.ToInt32(LifeMaxWeaponStar.Value);
        DataSave.NowRule.LifeMaxVehicleStar = Convert.ToInt32(LifeMaxVehicleStar.Value);
        DataSave.NowRule.ScoreSwitchMap = Convert.ToInt32(ScoreSwitchMap.Value);
        DataSave.NowRule.ScoreNotSwitchMap = Convert.ToInt32(ScoreNotSwitchMap.Value);
        DataSave.NowRule.ScoreStartSwitchMap = Convert.ToInt32(ScoreStartSwitchMap.Value);
        if (SwitchMapSelect0.IsChecked == true)
        {
            DataSave.NowRule.SwitchMapType = 0;
        }
        else if (SwitchMapSelect1.IsChecked == true)
        {
            DataSave.NowRule.SwitchMapType = 1;
        }
        else if (SwitchMapSelect2.IsChecked == true)
        {
            DataSave.NowRule.SwitchMapType = 2;
        }
        else if (SwitchMapSelect3.IsChecked == true)
        {
            DataSave.NowRule.SwitchMapType = 3;
        }
        DataSave.NowRule.OtherRule = OtherRule.SelectedItem as string;
        DataSave.NowRule.Team2Rule = Team2Rule.SelectedItem as string;
        DataSave.NowRule.ScoreOtherRule = Convert.ToInt32(SocreOtherRule.Value);

        if (DataSave.NowRule.MinRank >= DataSave.NowRule.MaxRank && DataSave.NowRule.MinRank != 0 && DataSave.NowRule.MaxRank != 0)
        {
            Globals.IsRuleSetRight = false;
            isApplyRule = false;

            AppendLog($"限制等级规则设置不正确\n");

            MainWindow.SetOperatingState(3, $"限制等级规则设置不正确");

            return;
        }

        /////////////////////////////////////////////////////////////////////////////

        // 清空限制武器列表
        DataSave.NowRule.Custom_WeaponList.Clear();
        // 添加自定义限制武器
        foreach (var item in BreakWeaponInfo.Items)
        {
            DataSave.NowRule.Custom_WeaponList.Add((item as WeaponInfoModel).English);
        }

        // 清空黑名单列表
        DataSave.NowRule.Custom_BlackList.Clear();
        // 添加自定义黑名单列表
        foreach (var item in BlackList.Items)
        {
            DataSave.NowRule.Custom_BlackList.Add(item as string);
        }

        // 清空白名单列表
        DataSave.NowRule.Custom_WhiteList.Clear();
        // 添加自定义白名单列表
        foreach (var item in WhiteList.Items)
        {
            DataSave.NowRule.Custom_WhiteList.Add(item as string);
        }

        if (RunAutoKick.IsChecked == true)
        {
            RunAutoKick.IsChecked = false;
            DataSave.AutoKickBreakPlayer = false;
        }

        DataSave.NowRule.WhiteListNoKill = WhiteListNoKill.IsChecked == true;
        DataSave.NowRule.WhiteListNoKD = WhiteListNoKD.IsChecked == true;
        DataSave.NowRule.WhiteListNoKPM = WhiteListNoKPM.IsChecked == true;
        DataSave.NowRule.WhiteListNoW = WhiteListNoW.IsChecked == true;
        DataSave.NowRule.WhiteListNoN = WhiteListNoN.IsChecked == true;

        DataSave.NowRule.SwitchMaps.Clear();
        foreach (string item in SwitchMapList.Items)
        {
            DataSave.NowRule.SwitchMaps.Add(item);
        }

        Globals.IsRuleSetRight = true;
        isApplyRule = true;

        AppendLog("成功提交当前规则，请重新启动自动踢人功能\n");

        MainWindow.SetOperatingState(1, "应用当前规则成功，请点击<查询当前规则>检验规则是否正确");

        WpfConfigUtils.SaveRule();
    }

    private void QueryRuleClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        RuleLog.Clear();

        StringBuilder builder = new();
        builder.AppendLine("===== 查询时间 =====");
        builder.AppendLine("");
        builder.AppendLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
        builder.AppendLine("");

        builder.AppendLine($"规则名字 : {DataSave.NowRule.Name}");
        builder.AppendLine("");

        builder.AppendLine($"玩家最高击杀限制 : {DataSave.NowRule.MaxKill}");
        builder.AppendLine("");

        builder.AppendLine($"计算玩家KD的最低击杀数 : {DataSave.NowRule.KDFlag}");
        builder.AppendLine($"玩家最高KD限制 : {DataSave.NowRule.MaxKD}");
        builder.AppendLine("");

        builder.AppendLine($"计算玩家KPM的最低击杀数 : {DataSave.NowRule.KPMFlag}");
        builder.AppendLine($"玩家最高KPM限制 : {DataSave.NowRule.MaxKPM}");
        builder.AppendLine("");

        builder.AppendLine($"玩家最低等级限制 : {DataSave.NowRule.MinRank}");
        builder.AppendLine($"玩家最高等级限制 : {DataSave.NowRule.MaxRank}");
        builder.AppendLine("");

        builder.AppendLine($"玩家最高生涯KD限制 : {DataSave.NowRule.LifeMaxKD}");
        builder.AppendLine($"玩家最高生涯KPM限制 : {DataSave.NowRule.LifeMaxKPM}");
        builder.AppendLine("");

        builder.AppendLine($"玩家最高生涯武器星数限制 : {DataSave.NowRule.LifeMaxWeaponStar}");
        builder.AppendLine($"玩家最高生涯载具星数限制 : {DataSave.NowRule.LifeMaxVehicleStar}");
        builder.AppendLine("\n");

        builder.AppendLine($"========== 禁武器列表 ==========");
        builder.AppendLine("");
        for (int a = 0; a < DataSave.NowRule.Custom_WeaponList.Count; a++)
        {
            builder.AppendLine($"武器名称 {a + 1} : {DataSave.NowRule.Custom_WeaponList[a]}");
        }
        builder.AppendLine("\n");

        builder.AppendLine($"========== 黑名单列表 ==========");
        builder.AppendLine("");
        foreach (var item in DataSave.NowRule.Custom_BlackList)
        {
            builder.AppendLine($"玩家ID {DataSave.NowRule.Custom_BlackList.IndexOf(item) + 1} : {item}");
        }
        builder.AppendLine("\n");

        builder.AppendLine($"========== 白名单列表 ==========");
        builder.AppendLine("");
        if (DataSave.NowRule.WhiteListNoKill)
        {
            builder.AppendLine($"白名单不限制击杀数量");
        }
        if (DataSave.NowRule.WhiteListNoKD)
        {
            builder.AppendLine($"白名单不限制KD");
        }
        if (DataSave.NowRule.WhiteListNoKPM)
        {
            builder.AppendLine($"白名单不限制KPM");
        }
        if (DataSave.NowRule.WhiteListNoW)
        {
            builder.AppendLine($"白名单不限制武器");
        }
        if (DataSave.NowRule.WhiteListNoN)
        {
            builder.AppendLine($"白名单不限制武器数量");
        }
        foreach (var item in DataSave.NowRule.Custom_WhiteList)
        {
            builder.AppendLine($"玩家ID {DataSave.NowRule.Custom_WhiteList.IndexOf(item) + 1} : {item}");
        }
        builder.AppendLine("\n");

        if (DataSave.NowRule.ScoreSwitchMap != 0)
        {
            builder.AppendLine($"========== 自动切图 ==========");
            builder.AppendLine("");
            builder.AppendLine($"分数差距达到{DataSave.NowRule.ScoreSwitchMap}自动换图");
            builder.AppendLine($"劣势方分数达到{DataSave.NowRule.ScoreStartSwitchMap}才生效");
            if (DataSave.NowRule.ScoreNotSwitchMap != 0)
            {
                builder.AppendLine($"且一方分数达到{DataSave.NowRule.ScoreNotSwitchMap}不再自动换图");
                builder.AppendLine("\n");
            }
        }

        if (DataSave.NowRule.ScoreOtherRule != 0 && !string.IsNullOrWhiteSpace(DataSave.NowRule.OtherRule))
        {
            builder.AppendLine($"========== 劣势方规则 ==========");
            builder.AppendLine("");
            builder.AppendLine($"分数差距达到{DataSave.NowRule.ScoreOtherRule}劣势方使用规则{DataSave.NowRule.OtherRule}");
            builder.AppendLine("\n");
            var rule = DataSave.Rules[DataSave.NowRule.OtherRule.ToLower()];

            builder.AppendLine($"玩家最高击杀限制 : {rule.MaxKill}");
            builder.AppendLine("");

            builder.AppendLine($"计算玩家KD的最低击杀数 : {rule.KDFlag}");
            builder.AppendLine($"玩家最高KD限制 : {rule.MaxKD}");
            builder.AppendLine("\n");
        }

        if (!string.IsNullOrWhiteSpace(DataSave.NowRule.Team2Rule))
        {
            builder.AppendLine($"========== 队伍2规则 ==========");
            builder.AppendLine("");
            var rule = DataSave.Rules[DataSave.NowRule.Team2Rule.ToLower()];

            builder.AppendLine($"玩家最高击杀限制 : {rule.MaxKill}");
            builder.AppendLine("");

            builder.AppendLine($"计算玩家KD的最低击杀数 : {rule.KDFlag}");
            builder.AppendLine($"玩家最高KD限制 : {rule.MaxKD}");
            for (int a = 0; a < rule.Custom_WeaponList.Count; a++)
            {
                builder.AppendLine($"禁武器名称 {a + 1} : {rule.Custom_WeaponList[a]}");
            }
            builder.AppendLine("\n");
        }

        AppendLog(builder.ToString());

        MainWindow.SetOperatingState(1, $"查询当前规则成功，请点击<检查违规玩家>测试是否正确");
    }

    private bool isRun;

    private async void CheckBreakRulePlayerClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        if (isRun)
            return;

        isRun = true;

        RuleLog.Clear();

        StringBuilder builder = new();

        builder.AppendLine("===== 查询时间 =====");
        builder.AppendLine("");
        builder.AppendLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}");
        builder.AppendLine("");

        if (!Globals.IsGameRun || !Globals.IsToolInit)
        {
            MainWindow.SetOperatingState(3, $"运行环境检查失败");
            builder.AppendLine("运行环境检查失败");
            builder.AppendLine("");
            AppendLog(builder.ToString());
            return;
        }

        builder.AppendLine("正在检查玩家");

        TasCheckPlayerLifeData.NeedPause = true;
        TaskCheckRule.NeedPause = true;
        TaskCheckNumber.NeedPause = true;

        AppendLog(builder.ToString());
        builder.Clear();

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

            TaskCheckRule.StartCheck();

            Parallel.ForEach(team1Player, (a, b) =>
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        AppendLog($"正在检查玩家: {a.Name}");
                    });
                    TasCheckPlayerLifeData.CheckBreakLifePlayer(a);
                }
                catch (Exception e)
                {
                    Core.LogError("生涯获取失败", e);
                    AppendLog($"检查玩家: {a.Name}生涯失败");
                }
            });
        });

        TasCheckPlayerLifeData.NeedPause = false;
        TaskCheckNumber.NeedPause = false;
        TaskCheckRule.NeedPause = false;

        int index = 1;
        builder.AppendLine($"========== 违规类型 : 限制玩家最高击杀 ==========");
        builder.AppendLine("");
        var list = TaskKick.NeedKick.Values.Where(item => item.Type is BreakType.Kill_Limit);
        foreach (var item in list)
        {
            builder.AppendLine($"玩家ID {index++} : {item.Name}");
        }
        builder.AppendLine("\n");

        index = 1;
        builder.AppendLine($"========== 违规类型 : 限制玩家最高KD ==========");
        builder.AppendLine("");
        list = TaskKick.NeedKick.Values.Where(item => item.Type is BreakType.KD_Limit);
        foreach (var item in list)
        {
            builder.AppendLine($"玩家ID {index++} : {item.Name}");
        }
        builder.AppendLine("\n");

        index = 1;
        builder.AppendLine($"========== 违规类型 : 限制玩家最高KPM ==========");
        builder.AppendLine("");
        list = TaskKick.NeedKick.Values.Where(item => item.Type is BreakType.KPM_Limit);
        foreach (var item in list)
        {
            builder.AppendLine($"玩家ID {index++} : {item.Name}");
        }
        builder.AppendLine("\n");

        index = 1;
        builder.AppendLine($"========== 违规类型 : 限制玩家等级范围 ==========");
        builder.AppendLine("");
        list = TaskKick.NeedKick.Values.Where(item => item.Type is BreakType.Rank_Limit);
        foreach (var item in list)
        {
            builder.AppendLine($"玩家ID {index++} : {item.Name}");
        }
        builder.AppendLine("\n");

        index = 1;
        builder.AppendLine($"========== 违规类型 : 限制玩家使用武器 ==========");
        builder.AppendLine("");
        list = TaskKick.NeedKick.Values.Where(item => item.Type is BreakType.Weapon_Limit);
        foreach (var item in list)
        {
            builder.AppendLine($"玩家ID {index++} : {item.Name}");
        }
        builder.AppendLine("\n");

        AppendLog(builder.ToString());

        MainWindow.SetOperatingState(1, $"检查违规玩家成功，如果符合规则就可以勾选<激活自动踢出违规玩家>了");

        isRun = false;
    }
    private async void ManualKickBreakRulePlayerClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        // 检查自动踢人环境
        if (await CheckKickEnv())
        {
            StringBuilder builder = new();

            builder.AppendLine("");
            builder.AppendLine("环境检查完毕，正在执行手动踢人操作，这可能需要一点时间");
            builder.AppendLine("请查看日志了解执行结果");

            AppendLog(builder.ToString());

            TaskKick.Kick();

            MainWindow.SetOperatingState(1, "执行手动踢人操作成功，请查看日志了解执行结果");
        }
    }
    // 开启自动踢人
    private async void RunAutoKickClick(object sender, RoutedEventArgs e)
    {
        if (RunAutoKick.IsChecked == true)
        {
            // 检查自动踢人环境
            if (await CheckKickEnv())
            {
                AppendLog("");
                AppendLog("环境检查完毕，自动踢人已开启");

                DataSave.AutoKickBreakPlayer = true;
                MainWindow.SetOperatingState(1, $"自动踢人开启成功");
                await Task.Run(() =>
                {
                    Thread.Sleep(500);
                    Dispatcher.Invoke(() => { RunAutoKick.IsChecked = true; });
                });
            }
            else
            {
                RunAutoKick.IsChecked = false;
                DataSave.AutoKickBreakPlayer = false;
            }
        }
        else
        {
            DataSave.AutoKickBreakPlayer = false;
            RunAutoKick.IsChecked = false;
            MainWindow.SetOperatingState(1, $"自动踢人关闭成功");
        }
    }

    private void AppendLog(string msg)
    {
        RuleLog.AppendText(msg + "\n");
    }

    private void OpenConfigurationFolderClick(object sender, RoutedEventArgs e)
    {
        AudioUtils.ClickSound();

        ProcessUtils.OpenLink(ConfigLocal.Base);
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        ProcessUtils.OpenLink(e.Uri.OriginalString);
        e.Handled = true;
    }

    /// <summary>
    /// 检查自动踢人环境是否合格
    /// </summary>
    /// <returns></returns>
    private async Task<bool> CheckKickEnv()
    {
        RuleLog.Clear();

        MainWindow.SetOperatingState(2, $"正在检查环境...");

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
            MainWindow.SetOperatingState(2, $"环境检查未通过");
            return false;
        }
        AppendLog("✔ 玩家已正确应用规则");

        AppendLog("");
        AppendLog("正在检查 SessionId 是否正确...");
        if (string.IsNullOrEmpty(Globals.Config.SessionId))
        {
            AppendLog("❌ SessionId为空，操作取消");
            MainWindow.SetOperatingState(2, $"环境检查未通过");
            return false;
        }
        AppendLog("✔ SessionId 检查正确");

        AppendLog("");
        AppendLog("正在检查 SessionId 是否有效...");
        var result = await ServerAPI.GetWelcomeMessage();
        if (!result.IsSuccess)
        {
            AppendLog("❌ SessionId 已过期，请刷新SessionId");
            MainWindow.SetOperatingState(2, $"环境检查未通过");
            return false;
        }
        AppendLog("✔ SessionId 检查有效");

        AppendLog("");
        AppendLog("正在检查 GameId 是否正确...");
        if (string.IsNullOrEmpty(Globals.Config.GameId))
        {
            AppendLog("❌ GameId 为空，请先进入服务器");
            MainWindow.SetOperatingState(2, $"环境检查未通过");
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
                MainWindow.SetOperatingState(2, $"环境检查未通过");
                return false;
            }
        }
        AppendLog("✔ 服务器管理员列表 检查正确");

        AppendLog("");
        AppendLog("正在检查 玩家是否为当前服务器管理...");
        var welcomeMsg = JsonUtils.JsonDese<WelcomeMsg>(result.Message);
        var firstMessage = welcomeMsg.result.firstMessage;
        string playerName = firstMessage.Substring(0, firstMessage.IndexOf("，"));
        if (!Globals.Server_Admin2List.Contains(playerName))
        {
            AppendLog("❌ 玩家不是当前服务器管理，请确认服务器是否选择正确");
            MainWindow.SetOperatingState(2, $"环境检查未通过");
            return false;
        }
        AppendLog("✔ 已确认玩家为当前服务器管理");

        return true;
    }
}
