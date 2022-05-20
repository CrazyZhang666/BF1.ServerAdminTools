using BF1.ServerAdminTools.Common.API.GT;
using BF1.ServerAdminTools.Common.API.GT.RespJson;
using BF1.ServerAdminTools.Common.Utils;
using BF1.ServerAdminTools.Wpf.Models;
using BF1.ServerAdminTools.Wpf.Utils;
using Microsoft.Toolkit.Mvvm.Input;

namespace BF1.ServerAdminTools.Wpf.Views;

/// <summary>
/// QueryView.xaml 的交互逻辑
/// </summary>
public partial class QueryView : UserControl
{
    public QueryModel QueryModel { get; set; }
    public ObservableCollection<string> PlayerDatas { get; set; }
    public ObservableCollection<AllPlayerData.WeaponsItem> WeaponsItems { get; set; }
    public ObservableCollection<AllPlayerData.VehiclesItem> VehiclesItems { get; set; }

    public RelayCommand QueryPlayerCommand { get; set; }

    ////////////////////////////////////////////

    public static Action<string> _QuickQueryPalyer;

    public QueryView()
    {
        InitializeComponent();

        this.DataContext = this;

        QueryModel = new QueryModel();

        PlayerDatas = new ObservableCollection<string>();
        WeaponsItems = new ObservableCollection<AllPlayerData.WeaponsItem>();
        VehiclesItems = new ObservableCollection<AllPlayerData.VehiclesItem>();

        QueryPlayerCommand = new RelayCommand(QueryPlayer);

        QueryModel.LoadingVisibility = Visibility.Collapsed;

        QueryModel.PlayerName = "CrazyZhang666";

        _QuickQueryPalyer = QuickQueryPlayerData;
    }

    private void QuickQueryPlayerData(string playerName)
    {
        AudioUtils.ClickSound();

        QueryModel.PlayerName = playerName;
        QueryPlayer();
    }

    private async void QueryPlayer()
    {
        AudioUtils.ClickSound();

        if (!string.IsNullOrEmpty(QueryModel.PlayerName))
        {
            PlayerDatas.Clear();
            WeaponsItems.Clear();
            VehiclesItems.Clear();

            QueryModel.Avatar = string.Empty;
            QueryModel.UserName = string.Empty;
            QueryModel.Rank = string.Empty;
            QueryModel.RankImg = string.Empty;
            QueryModel.PlayerTime = string.Empty;

            QueryModel.LoadingVisibility = Visibility.Visible;

            QueryModel.PlayerName = QueryModel.PlayerName.Trim();

            MainWindow.SetOperatingState(2, $"正在查询玩家 {QueryModel.PlayerName} 数据中...");

            var result = await GTAPI.GetPlayerAllData(QueryModel.PlayerName);

            QueryModel.LoadingVisibility = Visibility.Collapsed;

            if (result.IsSuccess)
            {
                var all = result.Obj as AllPlayerData;

                all.weapons.Sort((a, b) => b.kills.CompareTo(a.kills));
                all.vehicles.Sort((a, b) => b.kills.CompareTo(a.kills));

                QueryModel.Avatar = all.avatar;
                QueryModel.UserName = all.userName;
                QueryModel.Rank = $"等级 : {all.rank}";
                QueryModel.RankImg = all.rankImg;
                QueryModel.PlayerTime = $"游戏时间 : {PlayerUtils.GetPlayTime(all.secondsPlayed)}";

                Update(all);

                MainWindow.SetOperatingState(1, $"玩家 {QueryModel.PlayerName} 数据查询成功  |  耗时: {result.ExecTime:0.00} 秒");
            }
            else
            {
                MainWindow.SetOperatingState(3, $"玩家 {QueryModel.PlayerName} 数据查询失败  |  耗时: {result.ExecTime:0.00} 秒");
            }
        }
        else
        {
            MainWindow.SetOperatingState(2, $"请输入正确的玩家名称");
        }
    }

    private void Update(AllPlayerData all)
    {
        Task.Run(() =>
        {
            AddPlayerInfo($"数字ID : {all.id}");

            AddPlayerInfo("");

            AddPlayerInfo($"KD : {all.killDeath}");
            AddPlayerInfo($"KPM : {all.killsPerMinute}");
            AddPlayerInfo($"SPM : {all.scorePerMinute}");

            AddPlayerInfo($"命中率 : {all.accuracy}");
            AddPlayerInfo($"爆头率 : {all.headshots}");
            AddPlayerInfo($"爆头数 : {all.headShots}");

            AddPlayerInfo($"最高连续击杀数 : {all.highestKillStreak}");
            AddPlayerInfo($"最远爆头距离 : {all.longestHeadShot}");

            AddPlayerInfo("");

            AddPlayerInfo($"击杀 : {all.kills}");
            AddPlayerInfo($"死亡 : {all.deaths}");
            AddPlayerInfo($"协助击杀数 : {all.killAssists}");

            AddPlayerInfo($"胜率 : {all.winPercent}");
            AddPlayerInfo($"技巧值 : {all.skill}");

            AddPlayerInfo($"步兵KD : {all.infantryKillDeath}");
            AddPlayerInfo($"步兵KPM : {all.infantryKillsPerMinute}");
            AddPlayerInfo($"最佳兵种 : {ChsUtils.ToSimplifiedChinese(all.bestClass)}");

            AddPlayerInfo("");

            AddPlayerInfo($"仇敌击杀数 : {all.avengerKills}");
            AddPlayerInfo($"救星击杀数 : {all.saviorKills}");
            AddPlayerInfo($"急救数 : {all.revives}");
            AddPlayerInfo($"治疗分 : {all.heals}");
            AddPlayerInfo($"修理分 : {all.repairs}");

            AddPlayerInfo($"取得狗牌数 : {all.dogtagsTaken}");
            AddPlayerInfo($"胜利场数 : {all.wins}");
            AddPlayerInfo($"战败场数 : {all.loses}");
            AddPlayerInfo($"游戏总局数 : {all.roundsPlayed}");

            AddPlayerInfo("");

            AddPlayerInfo($"小隊分数 : {all.squadScore}");
            AddPlayerInfo($"奖励分数 : {all.awardScore}");
            AddPlayerInfo($"加成分数 : {all.bonusScore}");

            AddPlayerInfo($"当前等级进度 : {all.currentRankProgress}");
            AddPlayerInfo($"总计等级进度 : {all.totalRankProgress}");
        });

        Task.Run(() =>
        {
            foreach (var item in all.weapons)
            {
                if (item.kills != 0)
                {
                    item.weaponName = ChsUtils.ToSimplifiedChinese(item.weaponName);
                    item.image = ImageData.GetTempImagePath(item.image, ImageData.ImageType.weapons2);
                    item.star = PlayerUtils.GetKillStar(item.kills);
                    item.time = PlayerUtils.GetPlayTime(item.timeEquipped);

                    Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        WeaponsItems.Add(item);
                    }));
                }
            }
        });

        Task.Run(() =>
        {
            foreach (var item in all.vehicles)
            {
                if (item.kills != 0)
                {
                    item.vehicleName = ChsUtils.ToSimplifiedChinese(item.vehicleName);
                    item.image = ImageData.GetTempImagePath(item.image, ImageData.ImageType.vehicles2);
                    item.star = PlayerUtils.GetKillStar(item.kills);
                    item.time = PlayerUtils.GetPlayTime(item.timeIn);

                    Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        VehiclesItems.Add(item);
                    }));
                }
            }
        });
    }

    private void AddPlayerInfo(string str)
    {
        Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
        {
            PlayerDatas.Add(str);
        }));
    }
}
