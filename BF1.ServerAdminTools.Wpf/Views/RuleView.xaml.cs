using BF1.ServerAdminTools.Common.Data;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Wpf.Data;
using BF1.ServerAdminTools.Wpf.Models;

namespace BF1.ServerAdminTools.Wpf.Views;

/// <summary>
/// RuleView.xaml 的交互逻辑
/// </summary>
public partial class RuleView : UserControl
{
    public static Action? CloseRunCheck;
    public static Action? UpdateRule;
    /// <summary>
    /// 是否执行应用规则
    /// </summary>
    private bool isApplyRule = false;

    public RuleView()
    {
        InitializeComponent();

        Loaded += RuleView_Loaded;

        CloseRunCheck = FCloseRunKick;
        UpdateRule = FUpdateRule;

        // 添加武器信息列表
        foreach (var item in WeaponData.AllWeaponInfo)
        {
            ListWeaponInfo.Items.Add(new WeaponInfoModel()
            {
                English = item.English,
                Chinese = item.Chinese,
                Mark = ""
            });
        }
        ListWeaponInfo.SelectedIndex = 0;

        foreach (var item in WeaponData.AllWeaponInfo)
        {
            ListNumberWeaponInfo.Items.Add(new WeaponInfoModel()
            {
                English = item.English,
                Chinese = item.Chinese,
                Mark = ""
            });
        }
        ListNumberWeaponInfo.SelectedIndex = 0;

        Page1Load();

        foreach (var item in DataSave.SubscribeCache.Cache)
        {
            SubscribeBlackList.Items.Add(item);
        }

        DataSave.NowRule = DataSave.Rules["default"];

        LoadRule();
    }

    private void RuleView_Loaded(object sender, RoutedEventArgs e)
    {
        UpdateSubscribeClick(null, null);
    }

    private void FCloseRunKick()
    {
        Dispatcher.Invoke(() => RunAutoKick.IsChecked = false);
    }

    private void FUpdateRule()
    {
        Dispatcher.Invoke(() => LoadRule());
    }

    private void LoadRule()
    {
        OtherRule.SelectedItem = null;
        Team2Rule.SelectedItem = null;
        OtherRule.Items.Clear();
        Team2Rule.Items.Clear();

        SwitchMapList.Items.Clear();
        foreach (var item in DataSave.NowRule.SwitchMaps)
        {
            SwitchMapList.Items.Add(item);
        }

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
        if (DataSave.NowRule.WeaponNumbers == null)
        {
            DataSave.NowRule.WeaponNumbers = new();
        }

        MaxKill.Value = DataSave.NowRule.MaxKill;
        KDFlag.Value = DataSave.NowRule.KDFlag;
        MaxKD.Value = DataSave.NowRule.MaxKD;
        KPMFlag.Value = DataSave.NowRule.KPMFlag;
        MaxKPM.Value = DataSave.NowRule.MaxKPM;
        MinRank.Value = DataSave.NowRule.MinRank;
        MaxRank.Value = DataSave.NowRule.MaxRank;

        LifeMaxKD.Value = DataSave.NowRule.LifeMaxKD;
        LifeMaxKPM.Value = DataSave.NowRule.LifeMaxKPM;
        LifeMaxWeaponStar.Value = DataSave.NowRule.LifeMaxWeaponStar;
        LifeMaxVehicleStar.Value = DataSave.NowRule.LifeMaxVehicleStar;

        ScoreSwitchMap.Value = DataSave.NowRule.ScoreSwitchMap;
        ScoreStartSwitchMap.Value = DataSave.NowRule.ScoreStartSwitchMap;
        ScoreNotSwitchMap.Value = DataSave.NowRule.ScoreNotSwitchMap;
        SocreOtherRule.Value = DataSave.NowRule.ScoreOtherRule;

        WhiteListNoKill.IsChecked = DataSave.NowRule.WhiteListNoKill;
        WhiteListNoKD.IsChecked = DataSave.NowRule.WhiteListNoKD;
        WhiteListNoKPM.IsChecked = DataSave.NowRule.WhiteListNoKPM;
        WhiteListNoW.IsChecked = DataSave.NowRule.WhiteListNoW;
        WhiteListNoN.IsChecked = DataSave.NowRule.WhiteListNoN;

        if (DataSave.NowRule.SwitchMapType == 0)
        {
            SwitchMapSelect0.IsChecked = true;
        }
        else if (DataSave.NowRule.SwitchMapType == 1)
        {
            SwitchMapSelect1.IsChecked = true;
        }
        else if (DataSave.NowRule.SwitchMapType == 2)
        {
            SwitchMapSelect2.IsChecked = true;
        }
        else if (DataSave.NowRule.SwitchMapType == 3)
        {
            SwitchMapSelect3.IsChecked = true;
        }

        OtherRule.Items.Add("");
        Team2Rule.Items.Add("");

        var self = DataSave.NowRule.Name.ToLower();
        foreach (var item in DataSave.Rules)
        {
            if (item.Key != self)
            {
                OtherRule.Items.Add(item.Value.Name);
                Team2Rule.Items.Add(item.Value.Name);
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

        if (!string.IsNullOrEmpty(DataSave.NowRule.Team2Rule))
        {
            var temp = DataSave.NowRule.Team2Rule.ToLower();
            if (!DataSave.Rules.ContainsKey(temp) || DataSave.NowRule.Team2Rule == DataSave.NowRule.Name)
            {
                DataSave.NowRule.Team2Rule = "";
            }
        }

        if (!string.IsNullOrWhiteSpace(DataSave.NowRule.OtherRule))
        {
            OtherRule.SelectedItem = DataSave.NowRule.OtherRule;
            DataSave.NowRule.Team2Rule = "";
        }
        else
        {
            Team2Rule.SelectedItem = DataSave.NowRule.Team2Rule;
        }

        BreakWeaponInfo.Items.Clear();
        foreach (var item in DataSave.NowRule.Custom_WeaponList)
        {
            BreakWeaponInfo.Items.Add(new WeaponInfoModel()
            {
                English = item,
                Chinese = PlayerUtils.GetWeaponChsName(item)
            });
        }

        if (BreakWeaponInfo.Items.Count != 0)
        {
            BreakWeaponInfo.SelectedIndex = 0;
        }

        BlackList.Items.Clear();
        foreach (var item in DataSave.NowRule.Custom_BlackList)
        {
            BlackList.Items.Add(item);
        }

        WhiteList.Items.Clear();
        foreach (var item in DataSave.NowRule.Custom_WhiteList)
        {
            WhiteList.Items.Add(item);
        }

        foreach (WeaponInfoModel item in ListWeaponInfo.Items)
        {
            if (DataSave.NowRule.Custom_WeaponList.Contains(item.English))
            {
                item.Mark = "✔";
            }
            else
            {
                item.Mark = "";
            }
        }

        NumberList.Items.Clear();
        foreach (var item in DataSave.NowRule.WeaponNumbers.Values)
        {
            NumberList.Items.Add(new WeaponNumberModel
            {
                Name = item.Name,
                Count = item.Count
            });
        }

        DataSave.AutoKickBreakPlayer = false;
        RunAutoKick.IsChecked = false;
    }
}
